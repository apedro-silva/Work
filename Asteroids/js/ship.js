var Ship = WinJS.Class.define(
    function (ctx, image, radius, x, y, velocity) {
        this.shipImage = image;
        this.radius = radius;
        this.velocity = velocity;
        this.x = x;
        this.y = y;
        this.angle = 0;
        this.angle_vel = 0;
        this.ctx = ctx;
        this.thrust = false;
    },
    {
        getInfo : function () {
            return 'color:' + this.color + ' radius:' + this.radius;
        },
        draw: function () {
            Ship.drawImage(this.ctx, this.shipImage, this.x, this.y, this.angle, this.thrust);
        },
        step: function () {
            // keep ship in domain
            this.x = ((this.x + this.velocity[0]) % 800);
            this.y = ((this.y + this.velocity[1]) % 600);
            var x = Math.round(this.x);
            var y = Math.round(this.y);

            if (x < 0-this.shipImage.width/2)
                this.x = 800;
            if (y < 0-this.shipImage.height / 2)
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
            

        },
        rotate: function (vel) {
            this.angle_vel = vel;
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
            var pos = [x, y];
            // save the current co-ordinate system 
            // before we screw with it
            ctx.save();

            // move to the middle of where we want to draw our image
            ctx.translate(pos[0], pos[1]);
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
