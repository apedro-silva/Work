var Smile= WinJS.Class.define(
    function (canvas, image, x, y, velocity, objects) {
        this.image = image;
        this.x = x;
        this.y = y;
        this.c = canvas;
        this.velocity = velocity;
        this.objects = objects;
    },
    {
        getInfo : function () {
            return this.image;
        },
        draw : function (canvas) {
            canvas.drawImage(this.image, this.x, this.y);
        },
        step : function () {
            if (this.x > (this.c.canvas.width - this.image.width+6) || this.x <= -8) this.velocity[0] *= -1;
            if (this.y > (this.c.canvas.height - this.image.height + 6) || this.y <= -10) this.velocity[1] *= -1;
            var x = this.x;
            var y = this.y;
            var velocity = this.velocity;

            this.objects.forEach(function (paddle) {
                if ((x <= paddle.position[0] && paddle.position[0] <= x + 58) && (y+32 >= paddle.position[1]) && ((y+32) <= (paddle.position[1] + paddle.position[3])))
                    velocity[0] = 0;
            });

            this.x += velocity[0];
            this.y += velocity[1];
        },
        stop: function () {
            if (this.velocity[0] == 0) {
                this.velocity[0] = 3;
                this.velocity[1] = 3;
            }
            else {
                this.velocity[0] = 0;
                this.velocity[1] = 0;
            }
        }
},
    {
        getInfo: function () {
            return 'Smile object';
        }
    }
);

WinJS.Namespace.define("Robotics", {
    Smile: Smile
});
