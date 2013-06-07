var Robot = WinJS.Class.define(
    function (name) {
        this.name = name;
    },
    {
        modelName: "",
        on: function () {
            return "ola";
        },
        off: function () {
            // Turn the robot off.
        }
    },
    {
        harmsHumans: false,
        getModels: function () {
            // Return all the available models.
        }
    }
);

WinJS.Namespace.define("Robotics", {
    Robot: Robot
});