(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;
    var backImage = new Image();
    var shipImage = new Image();
    var ship = null;

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

            var canvas = document.getElementById("canvas");
            var ctx = canvas.getContext("2d");

            backImage.onload = function () {
                shipImage.onload = function () {
                    var x = shipImage.width;
                    var y = shipImage.height;

                    ship = new Asteroids.Ship(ctx, shipImage, 10, 100, 100, [0, 0]);
                    setInterval(function () { draw(ctx); ship.step(); }, 30);
                    document.addEventListener("keydown", doKeyDown, true);
                    document.addEventListener("keyup", doKeyUp, true);
                };
            };

        }
        function draw(ctx) {
            ctx.clearRect(0, 0, canvas.width, canvas.height);    // Clear the last image, if it exists.
            ctx.drawImage(backImage, 1, 1, canvas.width, canvas.height);

            ship.draw();
        }
        function doKeyDown(e) {
            switch (e.key) {
                case "Right":
                    ship.rotate(0.05);
                    break;
                case "Left":
                    ship.rotate(-0.05);
                    break;
                case "Up":
                    ship.accelerate(true);
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
