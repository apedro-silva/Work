var Sprite = WinJS.Class.define(
    function (ctx, position, velocity, angle_vel) {
        this.image = new Image();
        this.velocity = velocity;
        this.position = position;
        this.angle = 0;
        this.angle_vel = angle_vel;
        this.ctx = ctx;
        this.image.src = "/images/asteroid_blend.png";
    },
    {
        getInfo : function () {
            return 'Missile:' + this.velocity + '# Position:' + this.position;
        },
        getRadius: function () {
            return this.image.width / 4;
        },
        getCenter: function () {
            return [this.position[0] + this.image.width / 4, this.position[1] + this.image.height / 4];
        },
        draw: function () {
            if (this.image.width > 0) {
                Sprite.drawImage(this.ctx, this.image, this.position, this.angle);

                //this.ctx.drawImage(this.image, 0, 0, this.image.width, this.image.height, this.position[0], this.position[1], this.image.width, this.image.height);
            }
        },
        step: function () {
            // keep sprite in domain
            //return;
            var x = ((this.position[0] + this.velocity[0]) % 800);
            var y = ((this.position[1] + this.velocity[1]) % 600);
            x = Math.round(x);
            y = Math.round(y);

            if (x < 0 - this.image.width / 2)
                x = 800;
            if (y < 0 - this.image.height / 2)
                y = 600;

            this.position[0] = x;
            this.position[1] = y;

            this.angle += this.angle_vel;

        },
},
    {
        getInfo: function () {
            return 'Sprite object';
        },
        drawImage: function (ctx, image, position, angle) {
            // save the current co-ordinate system 
            // before we screw with it
            ctx.save();

            // move to the middle of where we want to draw our image
            ctx.translate(position[0], position[1]);
            ctx.translate(image.width / 4, image.height / 4);

            // rotate around that point, converting our 
            // angle from degrees to radians 
            ctx.rotate(angle);

            // draw it up and to the left by half the width
            // and height of the image 
            ctx.drawImage(image, 0, 0, image.width, image.height, -image.width / 4, -image.height/4, image.width/2, image.height/2);


            // and restore the co-ords to how they were when we began
            ctx.restore();
        }

    }
);

WinJS.Namespace.define("Asteroids", {
    Sprite: Sprite
});
