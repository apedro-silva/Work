var Missile = WinJS.Class.define(
    function (ctx, x, y, velocity) {
        this.image = new Image();
        this.velocity = velocity;
        this.x = x;
        this.y = y;
        this.angle = 0;
        this.angle_vel = 0;
        this.ctx = ctx;
        this.image.src = "/images/shot2.png";
    },
    {
        getInfo : function () {
            return 'Missile:' + this.velocity + '# Position:' + this.x + ',' + this.y;
        },
        draw: function () {
            this.ctx.drawImage(this.image, 0, 0, this.image.width, this.image.height, this.x, this.y, this.image.width, this.image.height);
        },
        step: function () {
            // keep ship in domain
            this.x = ((this.x + this.velocity[0]) % 800);
            this.y = ((this.y + this.velocity[1]) % 600);
            var x = Math.round(this.x);
            var y = Math.round(this.y);

            if (x < 0)
                this.x = 800;
            if (y < 0)
                this.y = 600;
        },
},
    {
        getInfo: function () {
            return 'Missile object';
        },

    }
);

WinJS.Namespace.define("Asteroids", {
    Missile: Missile
});
