(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;
    var backImage = new Image();
    var shipImage = new Image();
    var ship = null;
    var rocks = [];
    var explosions = [];
    var missiles = [];
    var lives = 3;
    var score = 0;
    var loading = true;
    var angle_vel = 0.001;
    var angle = 0;
    var canvas = null;
    var ctx = null;
    var appTimer = null;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched. Initialize
                // your application here.
                backImage.src = "/images/helix-nebula-1920.jpg";

                shipImage.src = "/images/double_ship.png";
            } else {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }
            args.setPromise(WinJS.UI.processAll());

            canvas = document.getElementById("canvas");
            ctx = canvas.getContext("2d");

            backImage.onload = function () {
                shipImage.onload = function () {
                    loading = false;
                    ship = new Asteroids.Ship(ctx, shipImage, 10, 100, 100, [0, 0]);
                    appTimer = setInterval(function () { runGame(); }, 30);
                };
            };
            document.addEventListener("keydown", doKeyDown, true);
            document.addEventListener("keyup", doKeyUp, true);
        }
        function runGame() {
            if (loading) return;
            if (lives == 0) {
                clearInterval(appTimer);
                return;
            }
            doStep();
            createRock();
            draw();
        }

        function doStep() {
            ship.step();

            // process rocks
            for (var i = 0; i < rocks.length; i++) {
                rocks[i].step();
            }

            // process missiles
            var tmpMissiles = missiles.slice(0, missiles.length);
            for (var i = 0; i < tmpMissiles.length; i++) {
                if (tmpMissiles[i].getAge() <= 0)
                    missiles.splice(i, 1);
                else
                    tmpMissiles[i].step();
            }

            var collisions = groupCollision(ship, rocks, true);
            lives -= collisions;

            collisions = groupGroupCollision(missiles, rocks, false);
            score += collisions;

            groupGroupCollision(rocks, rocks, false);

            var tempExplosions = explosions.slice(0, explosions.length);

            for (var i = 0; i < tempExplosions.length; i++) {
                if (tempExplosions[i].getAge() >= 24)
                    explosions.splice(i, 1);
                else
                    tempExplosions[i].step();
            }

            var l = document.getElementById("lives");
            l.innerText = lives;
            var l = document.getElementById("score");
            l.innerText = score;
        }
        function objectCollision(self, other) {
            if (self.collide(other)) {
                return true;
            }
            return false;
        }

        function groupGroupCollision(group1, group2, dualExplosion) {
            var tmpGroup1 = group1.slice(0, group1.length);
            var collisions = 0;
            for (var i = 0; i < tmpGroup1.length; i++) {
                collisions = groupCollision(tmpGroup1[i], group2, dualExplosion);
                if (collisions > 0) {
                    group1.splice(i, 1);
                }
            }
            return collisions;
        }

        function groupCollision(self, group, dualExplosion) {
            var x = group.slice(0, group.length);
            var collisions = 0;

            for (var i = 0; i < x.length; i++) {
                if (self == x[i])
                    continue;
                if (objectCollision(self, x[i])) {
                    if (dualExplosion)
                        explosions.push(new Explosion(ctx, self.getCenter()));
                    explosions.push(new Explosion(ctx, x[i].getCenter()));
                    group.splice(i, 1);
                    collisions += 1;
                }
            }
            return collisions;
        }

        function createRock() {
            var position = [Math.floor((Math.random() * canvas.width) + 1), Math.floor((Math.random() * canvas.height) + 1)];
            var angle_vel = Math.random() * .2 - .1;
            var velocity = [Math.floor(Math.random() * .6 - .3), Math.floor(Math.random() * .6 - .3)];

            var rock = new Asteroids.Sprite(ctx, position, velocity, angle_vel)

            if (!objectCollision(ship, rock) && rocks.length<=5)
                rocks.push(rock);
        }
        function draw() {
            ctx.clearRect(0, 0, canvas.width, canvas.height);    // Clear the last image, if it exists.

            ctx.drawImage(backImage, 0, 0, canvas.width, canvas.height);

            // draw rocks
            for (var i = 0; i < rocks.length; i++) {
                rocks[i].draw();
            }
            // draw missiles
            for (var i = 0; i < missiles.length; i++) {
                missiles[i].draw();
            }

            for (var i = 0; i < explosions.length; i++) {
                explosions[i].draw();
            }
            ship.draw();
        }

        function newGame(e) {
            Windows.UI.Popups.MessageDialog("New game?", "Asteroids").showAsync();
        }
        function doKeyDown(e) {
            switch (e.key) {
                case "Spacebar":
                    missiles.push(ship.shoot());
                    break;
                case "Right":
                    ship.rotate(0.05);
                    break;
                case "Left":
                    ship.rotate(-0.05);
                    break;
                case "Up":
                    ship.accelerate(true);
                    break;
                case "Down":
                    ship.reduce();
                    break;
            }
        }
        function doKeyUp(e) {
            switch (e.key) {
                case "Right":
                    ship.rotate(0);
                    break;
                case "Left":
                    ship.rotate(0);
                    break;
                case "Up":
                    ship.accelerate(false);
                    break;
            }
        }

    };

    app.oncheckpoint = function (args) {
        // TODO: This application is about to be suspended. Save any state
        // that needs to persist across suspensions here. You might use the
        // WinJS.Application.sessionState object, which is automatically
        // saved and restored across suspension. If you need to complete an
        // asynchronous operation before your application is suspended, call
        // args.setPromise().
    };

    app.start();
})();
