var Circle = WinJS.Class.define(
    function (c, color, radius, x, y, velocity, objects) {
        this.color = color;
        this.radius = radius;
        this.velocity = velocity;
        this.x = x;
        this.y = y;
        this.c = c;
        this.objects = objects;
    },
    {
        getInfo : function () {
            return 'color:' + this.color + ' radius:' + this.radius;
        },
        draw : function (c) {
            c.beginPath();
            c.fillStyle = this.color;
            c.arc(this.x, this.y, this.radius, 0, Math.PI * 2, true);
            c.closePath();
            c.fill();
        },
        step : function () {
            if (this.x > this.c.canvas.width - this.radius || this.x < this.radius) this.velocity[0] *= -1;
            if (this.y > this.c.canvas.height - this.radius || this.y < this.radius) this.velocity[1] *= -1;

            var x = this.x;
            var y = this.y;
            var velocity = this.velocity;
            var radius = this.radius;

            this.objects.forEach(function (paddle) {
                if (x + radius == paddle.position[0] && y >= paddle.position[1] && y <= paddle.position[1] + paddle.position[3])
                    velocity[0] *= -1;
                else if (x - radius - paddle.position[2] == paddle.position[0] && y >= paddle.position[1] && y <= paddle.position[1] + paddle.position[3])
                    velocity[0] = 0;
            });

            this.x += this.velocity[0];
            this.y += this.velocity[1];
        }
    },
    {
        getInfo: function () {
            return 'Circle object';
        }
    }
);

WinJS.Namespace.define("Robotics", {
    Circle: Circle
});
