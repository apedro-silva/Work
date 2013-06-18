var Explosion = WinJS.Class.define(
    function (ctx, position) {
        this.image = new Image();
        this.position = position;
        this.ctx = ctx;
        this.image.src = "/images/explosion_alpha.png";
        this.age = 0;
    },
    {
        getInfo : function () {
            return 'Explosion:' + this.velocity + '# Position:' + this.position;
        },
        getRadius: function () {
            return this.image.width / 4;
        },
        getAge: function () {
            return this.age;
        },
        getCenter: function () {
            return [this.position[0] + this.image.width / 4, this.position[1] + this.image.height / 4];
        },
        draw: function () {
            if (this.image.width > 0) {
                Explosion.drawImage(this.ctx, this.image, this.position, this.age);
            }
        },
        step: function () {
            this.age += 1;
        },
},
    {
        getInfo: function () {
            return 'Explosion object';
        },
        drawImage: function (ctx, image, position, age) {

            // draw it up and to the left by half the width
            // and height of the image 
            var explosionInfo = ([64, 64], [128, 128], 17, 24, true);
            ctx.drawImage(image, 128*age, 0, 128, 128, position[0]-64, position[1]-64, 128, 128);
        }

    }
);

WinJS.Namespace.define("Asteroids", {
    Explosion: Explosion
});
