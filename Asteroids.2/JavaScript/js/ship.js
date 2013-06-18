//// Copyright (c) António Pedro Silva. All rights reserved

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
        getPosition: function () {
            var fv = Ship.getAngleVector(this.angle)
            return [this.x + this.imageCenter[0] - 10, (this.y + this.imageCenter[1] - 3)];
        },
        getCenter: function () {
            if (this.image.width > 0) {
                var fv = Ship.getAngleVector(this.angle)
                return [this.x + this.imageCenter[0] + (1 * fv[0]), (this.y + this.imageCenter[1] - 3) + (1 * fv[1])];
            }
            return 0;
        },
        draw: function () {
            Ship.drawImage(this.ctx, this.image, this.x, this.y, this.angle, this.thrust);
        },
        collide: function (otherObject) {
            var otherObjCenter = otherObject.getCenter();
            var distance = Ship.getDistance(this.getCenter(), otherObjCenter);
            if (distance <= this.getRadius() -8 + otherObject.getRadius())
                return true;
            return false;

        },
        step: function () {
            // keep ship in domain
            this.x = ((this.x + this.velocity[0]) % this.ctx.canvas.width);
            this.y = ((this.y + this.velocity[1]) % this.ctx.canvas.height);
            var x = Math.round(this.x);
            var y = Math.round(this.y);

            if (x < 0-this.image.width/2)
                this.x = this.ctx.canvas.width;
            if (y < 0-this.image.height / 2)
                this.y = this.ctx.canvas.height;

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
        getCanonTipPos: function (fv) {
            return [this.x + this.imageCenter[0] + (1 * fv[0]), (this.y + this.imageCenter[1] - 3) + (1 * fv[1])];
            //return [this.x + this.imageCenter[0] - 10 + (this.imageSize[0] / 2 * fv[0]), (this.y + this.imageCenter[1] - 3) + ((this.imageSize[1] - 3) / 2 * fv[1])];
        },
        getMissileVelocity: function (fv) {
            return [this.velocity[0] + 6 * fv[0], this.velocity[1] + 6 * fv[1]];
        },
        shoot: function (missiles) {
            if (missiles == 0)
                return null;

            var fv = Ship.getAngleVector(this.angle)
            var missilePosition = this.getCanonTipPos(fv);
            var missileVelocity = this.getMissileVelocity(fv);

            return new Asteroids.Missile(this.ctx, missilePosition[0], missilePosition[1], missileVelocity);
        },
        accelerate: function (thrust) {
            this.thrust = thrust;
        },
        reduce: function () {
            var forwardVector = Ship.getAngleVector(this.angle)
            this.velocity[0] -= 0.1 * forwardVector[0];
            this.velocity[1] -= 0.1 * forwardVector[1];
        }
},
    {
        getInfo: function () {
            return 'Ship object';
        },
        getAngleVector: function (ang) {
            return [Math.cos(ang), Math.sin(ang)];
        },
        getDistance: function (p,q) {
            return Math.sqrt(Math.pow(p[0] - q[0], 2)+Math.pow(p[1] - q[1], 2));
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
