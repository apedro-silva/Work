//// Copyright (c) António Pedro Silva. All rights reserved

var Sprite = WinJS.Class.define(
    function (ctx, image, imageSize, position, velocity, angle_vel, lifespan, sound) {
        this.image = new Image();
        this.imageSize = imageSize;
        this.velocity = velocity;
        this.position = position;
        this.angle = 0;
        this.angle_vel = angle_vel;
        this.ctx = ctx;
        this.image = image;
        this.sizeFactor = 1;
        this.lifespan = lifespan;
        this.age = 0;
        if (sound)
            GameManager.assetManager.playSound(sound);
    },
    {
        getInfo : function () {
            return 'Sprite:' + this.velocity + '# Position:' + this.position;
        },
        getRadius: function () {
            return this.imageSize[0] * this.sizeFactor / 2;
        },
        getCenter: function () {
            return [this.position[0], this.position[1]];
        },
        getAge: function () {
            return this.age;
        },
        draw: function () {
            Sprite.drawImage(this.ctx, this.image, this.imageSize, this.position, this.angle, this.sizeFactor, this.age);
        },
        collide: function (otherObject) {
            var otherObjCenter = otherObject.getCenter();
            var distance = Sprite.getDistance(this.getCenter(), otherObjCenter);
            if (distance <= this.getRadius() + otherObject.getRadius())
                return true;
            return false;

        },
        step: function () {
            // keep sprite in domain
            //return;
            var x = ((this.position[0] + this.velocity[0]) % this.ctx.canvas.width);
            var y = ((this.position[1] + this.velocity[1]) % this.ctx.canvas.height);
            x = Math.round(x);
            y = Math.round(y);

            if (x < 0 - this.imageSize[0] / 2)
                x = this.ctx.canvas.width;
            if (y < 0 - this.imageSize[1] / 2)
                y = this.ctx.canvas.height;

            this.position[0] = x;
            this.position[1] = y;

            this.angle += this.angle_vel;
            if (this.lifespan)
                this.age += 1;
        },
},
    {
        getInfo: function () {
            return 'Sprite object';
        },
        getDistance: function (p, q) {
            return Math.sqrt(Math.pow(p[0] - q[0], 2) + Math.pow(p[1] - q[1], 2));
        },
        drawImage: function (ctx, image, imageSize, position, angle, sizeFactor, age) {
            var imageW = imageSize[0] * sizeFactor;
            var imageH = imageSize[1] * sizeFactor;
            // save the current co-ordinate system 
            // before we screw with it
            ctx.save();

            // move to the middle of where we want to draw our image
            ctx.translate(position[0], position[1]);
            // move to the middle of the image
            //ctx.translate((imageW / 2), (imageH / 2));

            // rotate around that point, converting our 
            // angle from degrees to radians 
            ctx.rotate(angle);

            // draw it up and to the left by half the width
            // and height of the image 
            ctx.drawImage(image, imageSize[0] * age, 0, imageSize[0], imageSize[1], (-imageW / 2), (-imageH / 2), imageW, imageH);

            // and restore the co-ords to how they were when we began
            ctx.restore();
        }

    }
);

WinJS.Namespace.define("Asteroids", {
    Sprite: Sprite
});
