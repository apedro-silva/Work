(function () {
    "use strict";

    WinJS.UI.Pages.define("/canvasball.html", {
        ready: function (element, options) {
            var x = 100, y = 100;
            var keyDown;
            var keyUp;
            var c = document.getElementById("canvas").getContext("2d");

            var paddleL = new Robotics.Paddle(c, [0, 0, 10, 50], "Black", 300, "L");
            var paddleR = new  Robotics.Paddle(c, [c.canvas.width-10, 0, 10, 50], "Black", 300, "R");

            var smile = new Robotics.Smile(c, document.getElementById("smile"), x, y, [3, 3]);
            var circle = new Robotics.Circle(c, "red", 20, x, y, [5, 5]);

            var objects = [paddleR, paddleL, circle];
            setInterval(function () { draw(); smile.step(objects); }, 30);

            var objects = [paddleR, paddleL];
            setInterval(function () { draw(); circle.step(objects); }, 30);

            document.addEventListener("keydown", doKeyDown, true);
            document.addEventListener("keyup", doKeyUp, true);

            function doKeyDown(e) {
                keyDown = e.key;
                switch(e.key)
                {
                    case "Enter":
                        break;
                    case "Right":
                        paddleR.right(1);
                        break;
                    case "Left":
                        paddleR.left(1);
                        break;
                    case "Down":
                        paddleR.down(1);
                        break;
                    case "Up":
                        paddleR.up(1);
                        break;
                    case "d":
                        paddleL.right(1);
                        break;
                    case "a":
                        paddleL.left(1);
                        break;
                    case "x":
                        paddleL.down(1);
                        break;
                    case "w":
                        paddleL.up(1);
                        break;
                    case "Spacebar":
                        smile.stop();
                    default:
                        break;
                }            
            }
            function doKeyUp(e) {
                keyUp = e.key;
                switch (e.key) {
                    case "Enter":
                        break;
                    case "Right":
                        paddleR.right(0);
                        break;
                    case "Left":
                        paddleR.left(0);
                        break;
                    case "Down":
                        paddleR.down(0);
                        break;
                    case "Up":
                        paddleR.up(0);
                        break;
                    case "d":
                        paddleL.right(0);
                        break;
                    case "a":
                        paddleL.left(0);
                        break;
                    case "x":
                        paddleL.down(0);
                        break;
                    case "w":
                        paddleL.up(0);
                        break;
                    case "Spacebar":
                        break;
                    default:
                        break;
                }
            }

            function printKey(key, x, y) {
                c.font = "20px Arial";
                c.fillText("key:" + key, x, y);
            }

            function draw() {
                c.clearRect(0, 0, c.canvas.width, c.canvas.height);

                circle.draw(c);
                smile.draw(c);
                paddleR.draw(c);
                paddleL.draw(c);
                printKey(keyDown, 10, 20);
                printKey(keyUp, 10, 50);
            }
        }
    });
})();
