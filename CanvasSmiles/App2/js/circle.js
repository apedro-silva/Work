var Circle = WinJS.Class.define(
    function (c, color, radius, x, y, velocity) {
        this.color = color;
        this.radius = radius;
        this.velocity = velocity;
        this.position = [x, y];
        this.x = x;
        this.y = y;
        this.c = c;
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
        step : function (objects) {
            var inCollision = false;
            var vel = [this.velocity[0], this.velocity[1]];
            if (this.x > this.c.canvas.width - this.radius || this.x < this.radius) {
                inCollision = true;
                vel[0] *= -1;
            }
            if (this.y > this.c.canvas.height - this.radius || this.y < this.radius) {
                inCollision = true;
                vel[1] *= -1;
            }

            a = { x: this.x - this.radius, y: this.y - this.radius, width: this.radius * 2, height: this.radius * 2 };

            objects.forEach(function (paddle) {
                b = { x: paddle.position[0], y: paddle.position[1], width: paddle.position[2], height: paddle.position[3] };

                if (Smile.inCollision(a, b)) {
                    vel[0] *= -1;
                    inCollision = true;
                }

            });

            if (this.inCollision) {
                this.x += this.velocity[0];
                this.y += this.velocity[1];
                this.position = [this.x, this.y];
            }
            else {
                this.x += vel[0];
                this.y += vel[1];
                this.position = [this.x, this.y];
                this.velocity = [vel[0], vel[1]];
            }
            this.inCollision = inCollision;
        }
    },
    {
        getInfo: function () {
            return 'Circle object';
        },
        inCollision: function (a, b) {
            return (!(((a.y + a.height) < (b.y)) ||
                    (a.y > (b.y + b.height)) ||
                    ((a.x + a.width) < b.x) ||
                    (a.x > (b.x + b.width))
                ));
        }

    }
);

WinJS.Namespace.define("Robotics", {
    Circle: Circle
});
