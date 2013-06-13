var Ship = WinJS.Class.define(
    function (ctx, image, radius, x, y, velocity) {
        this.image = image;
        this.radius = radius;
        this.velocity = velocity;
        this.x = x;
        this.y = y;
        this.angle = 0;
        this.angle_vel = 0;
        this.ctx = ctx;
        this.thrust = false;
        this.missile = null;
        this.imageSize = [image.width / 2, image.height];
        this.imageCenter = [image.width / 4, image.height/2];
    },
    {
        getInfo : function () {
            return 'color:' + this.color + ' radius:' + this.radius;
        },
        getRadius: function () {
            if (this.image.width > 0)
                return this.image.width / 4;
            return 0;
        },
        getCenter: function () {
            if (this.image.width > 0)
                return [this.x + this.image.width / 4, this.y + this.image.height / 2];
            return 0;
        },
        draw: function () {
            Ship.drawImage(this.ctx, this.image, this.x, this.y, this.angle, this.thrust);
            if (this.missile!=null)
                this.missile.draw();
        },
        collide: function (otherObject) {
            var otherObjCenter = otherObject.getCenter();
            var distance = Missile.getDistance(this.getCenter(), otherObjCenter);
            if (distance <= this.getRadius() + otherObject.getRadius())
                return true;
            return false;

        },
        step: function () {
            // keep ship in domain
            this.x = ((this.x + this.velocity[0]) % 800);
            this.y = ((this.y + this.velocity[1]) % 600);
            var x = Math.round(this.x);
            var y = Math.round(this.y);

            if (x < 0-this.image.width/2)
                this.x = 800;
            if (y < 0-this.image.height / 2)
                this.y = 600;

            // get forward vector
            this.angle += this.angle_vel;
            var forwardVector = Ship.getAngleVector(this.angle)

            // update acceleration on forward vector
            if (this.thrust) {
                this.velocity[0] += 0.1 * forwardVector[0];
                this.velocity[1] += 0.1 * forwardVector[1];
            }

            // update friction
            this.velocity[0] *= (1 - 0.01);
            this.velocity[1] *= (1 - 0.01);
            
            if (this.missile != null)
                this.missile.step();

        },
        rotate: function (vel) {
            this.angle_vel = vel;
        },
        getCanonTipPos: function (fv) {
            return [this.x + this.imageCenter[0] -10 + (this.imageSize[0] / 2 * fv[0]), (this.y + this.imageCenter[1]-3) + ((this.imageSize[1]-3) / 2 * fv[1])];
        },
        getMissileVelocity: function (fv) {
            return [this.velocity[0] + 6 * fv[0], this.velocity[1] + 6 * fv[1]];
        },
        shoot: function () {
            var fv = Ship.getAngleVector(this.angle)
            var missilePosition = this.getCanonTipPos(fv);
            var missileVelocity = this.getMissileVelocity(fv);

            this.missile = new Asteroids.Missile(this.ctx, missilePosition[0], missilePosition[1], missileVelocity);
        },
        accelerate: function (thrust) {
            this.thrust = thrust;
        }
},
    {
        getInfo: function () {
            return 'Ship object';
        },
        getAngleVector: function (ang) {
            return [Math.cos(ang), Math.sin(ang)];
        },
        drawImage: function (ctx, image, x, y, angle, thrust) {
            // save the current co-ordinate system 
            // before we screw with it
            ctx.save();

            // move to the middle of where we want to draw our image
            ctx.translate(x, y);
            ctx.translate(image.width/4, image.height/2);

            // rotate around that point, converting our 
            // angle from degrees to radians 
            ctx.rotate(angle);

            // draw it up and to the left by half the width
            // and height of the image 
            if (thrust)
                ctx.drawImage(image, image.width/2, 0, image.width / 2, image.height, -image.width / 4, -image.height / 2, image.width / 2, image.height);
            else
                ctx.drawImage(image, 0, 0, image.width / 2, image.height, -image.width / 4, -image.height / 2, image.width / 2, image.height);


            // and restore the co-ords to how they were when we began
            ctx.restore();
        }

    }
);

WinJS.Namespace.define("Asteroids", {
    Ship: Ship
});
