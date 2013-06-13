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
        getRadius: function () {
            return this.image.width / 2;
        },
        getCenter: function () {
            return [this.x + this.image.width / 2, this.y + this.image.height / 2];
        },
        draw: function () {
            this.ctx.drawImage(this.image, 0, 0, this.image.width, this.image.height, this.x, this.y, this.image.width, this.image.height);
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
            //return;
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
        getDistance: function (p,q) {
            return Math.sqrt(Math.pow(p[0] - q[0], 2)+Math.pow(p[1] - q[1], 2));
        }
    }
);

WinJS.Namespace.define("Asteroids", {
    Missile: Missile
});
