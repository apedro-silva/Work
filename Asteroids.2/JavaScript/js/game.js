//// Copyright (c) Microsoft Corporation. All rights reserved

var Game = WinJS.Class.define(
    null,
{
    // Convenience variables
    gameContext: null,
    missiles:[],
    rocks: [],
    explosions: [],
    ship: null,
    stateHelper: null,
    state: null,
    settings: null,
    velocity: 0,
    rocksImage: [],
    isSnapped: function () {
        var currentState = Windows.UI.ViewManagement.ApplicationView.value;
        return currentState === Windows.UI.ViewManagement.ApplicationViewState.snapped;
    },

    // Called when Game is first loaded
    initialize: function (state) {
        if (GameManager.gameId === null) {
            this.stateHelper = state;
            this.state = state.internal;
            this.settings = state.external;
        }
        this.rocksImage.push(GameManager.assetManager.assets.rockImage.object);
        this.rocksImage.push(GameManager.assetManager.assets.rock1Image.object);
        this.rocksImage.push(GameManager.assetManager.assets.rock2Image.object);


    },

    // Called to get list of assets to pre-load
    getAssets: function () {
        // To add asset to a list of loading assets follow the the examples below
        var assets = {
            soundtrackSound: { object: null, fileName: "/sounds/soundtrack.mp3", fileType: AssetType.audio, loop: true, autoplay: true },
            sndBounce: { object: null, fileName: "/sounds/bounce.wav", fileType: AssetType.audio, loop: false },
            backgroundImage: { object: null, fileName: "/images/helix-nebula-1920.jpg", fileType: AssetType.image },
            explosion1Image: { object: null, fileName: "/images/explosion_alpha.png", fileType: AssetType.image },
            explosion2Image: { object: null, fileName: "/images/explosion_blue.png", fileType: AssetType.image },
            explosion3Image: { object: null, fileName: "/images/explosion_blue2.png", fileType: AssetType.image },
            rockImage: { object: null, fileName: "/images/asteroid_blend.png", fileType: AssetType.image },
            rock1Image: { object: null, fileName: "/images/asteroid_brown.png", fileType: AssetType.image },
            rock2Image: { object: null, fileName: "/images/asteroid_blue.png", fileType: AssetType.image },
            shipImage: { object: null, fileName: "/images/double_ship.png", fileType: AssetType.image },
            missileSound: { object: null, fileName: "/sounds/missile.mp3", fileType: AssetType.audio, loop: false },
            explosionSound: { object: null, fileName: "/sounds/explosion.mp3", fileType: AssetType.audio, loop: false },
            thrustSound: { object: null, fileName: "/sounds/thrust.mp3", fileType: AssetType.audio, loop: false }
        };

        return assets;
    },
    
    // Called the first time the game is shown
    showFirst: function () {
        // If game was previously running, pause it.
        if (this.state.gamePhase === "started") {
            this.pause();
        }

        // Note: gameCanvas is the name of the <canvas> in default.html
        this.gameContext = gameCanvas.getContext("2d");
        var shipImage = GameManager.assetManager.assets.shipImage;
        this.ship = new Asteroids.Ship(this.gameContext, shipImage.object, 10, 100, 100, [0, 0]);
    },

    // Called each time the game is shown
    show: function () {
        // Resume playing
        this.unpause();
        GameManager.assetManager.playSound(GameManager.assetManager.assets.soundtrackSound);
        //GameManager.assetManager.playSound(GameManager.assetManager.assets.missileSound);
    },

    // Called each time the game is hidden
    hide: function () {
        GameManager.assetManager.stopSound(GameManager.assetManager.assets.soundtrackSound);
        this.pause();
    },

    // Called when the game enters snapped view
    snap: function () {
        // TODO: Update game state when in snapped view - basic UI styles can be set with media queries in gamePage.css
        this.pause();
        // Temporarily resize game area to maintain aspect ratio
        gameCanvas.width = window.innerWidth;
        gameCanvas.height = window.innerHeight;
    },

    // Called when the game enters fill or full-screen view
    unsnap: function () {
        // TODO: Update game state when in fill, full-screen, or portrait view - basic UI styles can be set with media queries in gamePage.css

        // It's possible the ball is now outside the play area. Fix it if so.
        if (this.state.position.x > window.innerWidth) {
            this.state.position.x += window.innerWidth - this.state.position.x;
        }
        if (this.state.position.y > window.innerHeight) {
            this.state.position.y += window.innerHeight - this.state.position.y;
        }

        // Restore game area to new aspect ratio
        gameCanvas.width = window.innerWidth;
        gameCanvas.height = window.innerHeight;

        // Resume playing
        this.unpause();
    },

    // Called to reset the game
    newGame: function () {
        GameManager.game.ready();
    },

    // Called when the game is being prepared to start
    ready: function () {
        // TODO: Replace with your own new game initialization logic
        if (this.isSnapped()) {
            this.state.gamePaused = true;
        } else {
            this.state.gamePaused = false;
        }
        this.state.gamePhase = "started";
        switch (this.settings.skillLevel) {
            case 0:
                this.state.speed.x = 5;
                this.state.speed.y = 5;
                break;
            case 1:
                this.state.speed.x = 10;
                this.state.speed.y = 4;
                break;
            case 2:
                this.state.speed.x = 8;
                this.state.speed.y = 12;
                break;
        }
        this.state.position.x = 100;
        this.state.position.y = 100;
        this.state.previousScore = 0;
        this.state.score = 0;
        this.state.lives = 3;
        this.state.missiles = 20;

    },

    // Called when the game is started
    start: function () {
        // Don't allow start when game is snapped
        if (!this.isSnapped()) {
            this.state.gamePhase = "started";
        }
    },
    
    // Called when the game is over
    end: function () {
        this.state.gamePhase = "ended";
        var newRank = GameManager.scoreHelper.newScore({ player: this.settings.playerName, score: this.state.score, skill: this.settings.skillLevel });
    },

    // Called when the game is paused
    pause: function () {
        this.state.gamePaused = true;
    },

    // Called when the game is un-paused
    unpause: function () {
        // Don't allow unpause when game is snapped
        if (!this.isSnapped()) {
            this.state.gamePaused = false;
        }
    },

    // Called to toggle the pause state
    togglePause: function () {
        if (GameManager.game.state.gamePaused) {
            GameManager.game.unpause();
        } else {
            GameManager.game.pause();
        }
    },

    // Touch events... All touch events come in here before being passed onward based on type
    doTouch: function (touchType, e) {
        switch (touchType) {
            case "start": GameManager.game.touchStart(e); break;
            case "end": GameManager.game.touchEnd(e); break;
            case "move": GameManager.game.touchMove(e); break;
            case "cancel": GameManager.game.touchCancel(e); break;
        }
    },

    touchStart: function (e) {
        // TODO: Replace game logic
        // Touch screen to move from ready phase to start the game
        if (this.state.gamePhase === "ready") {
            this.start();
        }
    },

    touchEnd: function (e) {
        // TODO: Replace game logic.
        if (this.state.gamePhase === "started" && !this.state.gamePaused) {
            if (Math.sqrt(((e.x - this.state.position.x) * (e.x - this.state.position.x)) +
            ((e.y - this.state.position.y) * (e.y - this.state.position.y))) < 50) {
                this.state.score++;
            }
        }
    },
    
    touchMove: function (e) {
        // TODO: Add game logic
    },

    touchCancel: function (e) {
        // TODO: Add game logic
    },

    // Called before preferences panel or app bar is shown
    showExternalUI: function (e) {
        if (e.srcElement.id === "settingsDiv") {
            this.pause();
        }
    },

    // Called after preferences panel or app bar is hidden
    hideExternalUI: function (e) {
        if (e.srcElement.id === "settingsDiv") {
            this.unpause();
        }
    },

    // Called by settings panel to populate the current values of the settings
    getSettings: function () {
        // Note: The left side of these assignment operators refers to the setting controls in default.html
        // TODO: Update to match any changes in settings panel
        settingPlayerName.value = this.settings.playerName;
        settingSoundVolume.value = this.settings.soundVolume;
        for (var i = 0; i < settingSkillLevel.length; i++) {
            if (settingSkillLevel[i].value === "" + this.settings.skillLevel) {
                settingSkillLevel[i].checked = true;
            }
        }
    },

    // Called when changes are made on the settings panel
    setSettings: function () {
        // Note: The right side of these assignment operators refers to the controls in default.html
        // TODO: Update to match any changes in settings panel
        this.settings.playerName = settingPlayerName.value;
        this.settings.soundVolume = settingSoundVolume.value;
        // Changing the skill level re-starts the game
        var skill = 0;
        for (var i = 0; i < settingSkillLevel.length; i++) {
            if (settingSkillLevel[i].checked) {
                skill = parseInt(settingSkillLevel[i].value);
            }
        }
        if (this.settings.skillLevel !== skill) {
            // Update the skill level
            this.settings.skillLevel = skill;

            // Start a new game so high scores represent entire games at a given skill level only.
            this.ready();

            // Save state so that persisted skill-derived values match the skill selected
            this.stateHelper.save("internal");
        }

        // Save settings out
        this.stateHelper.save("external");
    },

    // Called when the app is suspended
    suspend: function () {
        this.pause();
        this.stateHelper.save();
    },

    // Called when the app is resumed
    resume: function () {
    },

    createRock : function() {
        var position = [Math.floor((Math.random() * this.gameContext.canvas.width) + 1), Math.floor((Math.random() * this.gameContext.canvas.height) + 1)];
        var angle_vel = Math.random() * .2 - .1;
        var velocity = [Math.floor(Math.random() * .6 - .3), Math.floor(Math.random() * .6 - .3)];

        var rockImage = this.rocksImage[Math.floor(Math.random() * 3)];
        var rock = new Asteroids.Sprite(this.gameContext, rockImage, [90, 90], position, velocity, angle_vel, false, null)

        if (!this.objectCollision(this.ship, rock) && this.rocks.length <= 5)
            this.rocks.push(rock);
    },

    // Main game update loop
    update: function () {
        // TODO: Sample game logic to be replaced
        if (!this.state.gamePaused && this.state.gamePhase === "started") {
            
            this.ship.step();
            this.createRock();

            // process rocks
            for (var i = 0; i < this.rocks.length; i++) {
                this.rocks[i].step();
            }

            // process missiles
            var tmpMissiles = this.missiles.slice(0, this.missiles.length);
            for (var i = 0; i < tmpMissiles.length; i++) {
                if (tmpMissiles[i].getAge() <= 0)
                    this.missiles.splice(i, 1);
                else
                    tmpMissiles[i].step();
            }

            // process ship collision with rocks
            var collisions = this.groupCollision(this.ship, this.rocks, 2);
            this.state.lives -= collisions;

            // process missiles hiting rocks
            collisions = this.groupGroupCollision(this.missiles, this.rocks, 1);
            this.state.score += collisions;

            if (this.state.score != this.state.previousScore & this.state.score % 10 == 0) {
                this.state.lives += 1;
                this.state.missiles += 10;
            }

            this.state.previousScore = this.state.score;

            // process rocks collisions
            this.groupGroupCollision(this.rocks, this.rocks, 3);

            // process explosions
            var tempExplosions = this.explosions.slice(0, this.explosions.length);
            for (var i = 0; i < tempExplosions.length; i++) {
                if (tempExplosions[i].getAge() >= 24)
                    this.explosions.splice(i, 1);
                else {
                    tempExplosions[i].step();
                }
            }

            // Check if game is over
            if (this.state.lives == 0) {
                this.end();
            }
        }
    },

    // Main game render loop
    draw: function () {
        this.drawBackground();

        //draw ship
        this.ship.draw();

        // draw missiles
        for (var i = 0; i < this.missiles.length; i++) {
              this.missiles[i].draw();
        }

        // draw rocks
        for (var i = 0; i < this.rocks.length; i++) {
            this.rocks[i].draw();
        }

        for (var i = 0; i < this.explosions.length; i++) {
            this.explosions[i].draw();
        }

        // Draw a ready or game over or paused indicator
        if (this.state.gamePhase === "ready") {
            this.gameContext.textAlign = "center";
            this.gameContext.fillText("READY", gameCanvas.width / 2, gameCanvas.height / 2);
        } else if (this.state.gamePhase === "ended") {
            this.gameContext.textAlign = "center";
            this.gameContext.fillText("GAME OVER", gameCanvas.width / 2, gameCanvas.height / 2);
        } else if (this.state.gamePaused) {
            this.gameContext.textAlign = "center";
            this.gameContext.fillText("PAUSED", gameCanvas.width / 2, gameCanvas.height / 2);
        }

        // Draw the number of lives remaining
        this.gameContext.fillStyle = "#FF8040";
        this.gameContext.textAlign = "left";
        this.gameContext.fillText("Lives:" + this.state.lives, 5, 20);

        this.gameContext.font = "bold 28px Segoe UI";
        this.gameContext.textBaseline = "middle";

        // Draw the current score
        this.gameContext.fillStyle = "#FFFF99";
        this.gameContext.textAlign = "left";
        this.gameContext.fillText("Score:" + this.state.score, 5, 60);

        // Draw the number of missiles remaining
        this.gameContext.fillStyle = "#FF8040";
        this.gameContext.textAlign = "left";
        this.gameContext.fillText("Missiles:" + this.state.missiles, 5, 100);

    },
    drawBackground: function () {
        this.gameContext.clearRect(0, 0, gameCanvas.width, gameCanvas.height);
        var backImage = GameManager.assetManager.assets.backgroundImage.object;
        var imageW = backImage.width;
        var imageH = backImage.height;
        var canvasW = this.gameContext.canvas.width;
        var canvasH = this.gameContext.canvas.height;

        //context.drawImage(img,x,y,width,height);
        //Position the image on the canvas, and specify width and height of the image:

        this.gameContext.drawImage(backImage, 0, 0, imageW, imageH, this.velocity, 0, canvasW, canvasH);

        //this.gameContext.drawImage(backImage, this.velocity, 0, canvasW, canvasH);


        //this.gameContext.drawImage(backImage,imageW - Math.abs(this.velocity), 0, canvasW, canvasH);
        //if (Math.abs(this.velocity) > imageW)
        //    this.velocity = 0;

        //this.gameContext.drawImage(GameManager.assetManager.assets.backgroundImage.object,
        //    0, 0,
        //    imageW, imageH,
        //    this.velocity, 0,
        //    canvasW, canvasH);
        //this.velocity -= 2;
    },
    groupCollision: function (self, group, explosionType) {
        var x = group.slice(0, group.length);
        var collisions = 0;

        for (var i = 0; i < x.length; i++) {
            if (self == x[i])
                continue;
            if (this.objectCollision(self, x[i])) {
                 var explosionImage = this.getExplosionImage(explosionType);
                 var explosion = new Asteroids.Sprite(this.gameContext, explosionImage, [128, 128], x[i].getCenter(), [0, 0], 0, true, GameManager.assetManager.assets.explosionSound)
                this.explosions.push(explosion);
                //this.explosions.push(new Asteroids.Explosion(this.gameContext, explosionImage, x[i].getCenter()));
                group.splice(i, 1);
                collisions += 1;
            }
        }
        return collisions;
    },
    getExplosionImage: function (explosionType) {
        if (explosionType==1)
            return GameManager.assetManager.assets.explosion1Image.object;
        else if (explosionType==2)
            return GameManager.assetManager.assets.explosion2Image.object;
        else if (explosionType==3)
            return GameManager.assetManager.assets.explosion3Image.object;
    },

    groupGroupCollision: function (group1, group2, explosionType) {
        var tmpGroup1 = group1.slice(0, group1.length);
        var collisions = 0;
        for (var i = 0; i < tmpGroup1.length; i++) {
            collisions = this.groupCollision(tmpGroup1[i], group2, explosionType);
            if (collisions > 0) {
                group1.splice(i, 1);
            }
        }
        return collisions;
    },
    objectCollision: function (self, other) {
        if (self.collide(other)) {
            return true;
        }
        return false;
    }

});
