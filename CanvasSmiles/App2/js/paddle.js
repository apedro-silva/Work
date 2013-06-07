var Paddle = WinJS.Class.define(
    function (canvas, position, color, horizontalLimit, side) {
        this.position = position;
        this.color = color;
        this.c = canvas;
        this.velocity = [0, 0];
        this.horizontalLimit = horizontalLimit;
        this.side = side;
    },
    {
        up: function (value) {
            this.velocity[1] = -5 * value
        },
        down : function (value) {
            this.velocity[1] = +5 * value
        },

        left: function (value) {
            this.velocity[0] = -5 * value
        },
        right: function (value) {
            this.velocity[0] = +5 * value
        },

        getPosition: function () {
            return this.position;
        },

        update: function (value) {
            this.position[0] += this.velocity[0];
            this.position[1] += this.velocity[1];

            if (this.position[0] < 0)
                this.position[0] = 0;

            if (this.position[0] + this.position[2] > this.c.canvas.width)
                this.position[0] = this.c.canvas.width - this.position[2];

            if (this.side == "L" && this.position[0] > this.horizontalLimit)
                this.position[0] = this.horizontalLimit;

            if (this.side == "R" && this.position[0] < (this.c.canvas.width - this.horizontalLimit))
                this.position[0] = (this.c.canvas.width - this.horizontalLimit);

            if (this.position[1] <= 0)
                this.position[1] = 0;

            if (this.position[1] + this.position[3] > this.c.canvas.height)
                this.position[1] = this.c.canvas.height - this.position[3];

        },

        draw: function () {
            this.update();
            this.c.beginPath();
            this.c.fillStyle = this.color;
            this.c.fillRect(this.position[0], this.position[1], this.position[2], this.position[3]);
            this.c.closePath();
            this.c.fill();
        }
},
    {
        getInfo: function () {
            return 'Paddle object';
        }
    }
);

WinJS.Namespace.define("Robotics", {
    Paddle: Paddle
});
