var Smile= WinJS.Class.define(
    function (canvas, image, x, y, velocity) {
        this.image = image;
        this.x = x;
        this.y = y;
        this.c = canvas;
        this.position = [x, y];
        this.velocity = velocity;
        this.inCollision = false;
    },
    {
        getInfo : function () {
            return this.image;
        },
        draw : function (canvas) {
            canvas.drawImage(this.image, this.x, this.y);
        },
        step: function (objects) {
            var inCollision = false;
            var vel = [this.velocity[0], this.velocity[1]];
            if (this.x > (this.c.canvas.width - this.image.width + 6) || this.x <= -8) {
                inCollision = true;
                vel[0] *= -1;
            }
            if (this.y > (this.c.canvas.height - this.image.height + 6) || this.y <= -10) {
                inCollision = true;
                vel[1] *= -1;
            }

            a = { x: this.x, y: this.y, width: 57, height: 57 };

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
        },

        inCollision : function(a, b) {
            return (!(((a.y + a.height) < (b.y)) ||
                    (a.y > (b.y + b.height)) ||
                    ((a.x + a.width) < b.x) ||
                    (a.x > (b.x + b.width))
                ));
        }
    }
);

WinJS.Namespace.define("Robotics", {
    Smile: Smile
});
