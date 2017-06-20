(function () {
    "use strict";

    if (!htm.app) return;

    //SPECIALIZED INPUT CONTROLS
    var ENTER_KEY = "Enter";
    var TAB_KEY = "Tab";
    var BACKSPACE_KEY = "Backspace";

    htm.app.factory("inputService", function () {
        return {
            validate: function (regex, event) {
                if (event.key === BACKSPACE_KEY || event.key === ENTER_KEY || event.key === TAB_KEY) return; //FireFox HACK!

                var selection = event.target.selectionStart != event.target.selectionEnd;
                var value = !selection ? event.target.value + event.key : event.key;

                if (!regex.test(value)) {
                    event.preventDefault();
                }
            }
        };
    });

    htm.app.directive("ngNumberField", function (inputService) {
        return {
            require: "ngModel",
            link: function (scope, element, attrs) {
                //set element to text
                element.attr("type", "text");

                var _maxlength = attrs.maxlength;
                var _min = attrs.min;

                var _decimals = attrs.ngDecimals;

                var regex = _decimals ? new RegExp("^" + (_min && _min >= 0 ? "" : "\\-?") + "\\d{0," + _maxlength + "}\\.?\\d{0," + _decimals + "}?$") :
                    new RegExp("^" + (_min && _min >= 0 ? "" : "\\-?") + "\\d{0," + _maxlength + "}$");

                element.on("keypress", function (e) {
                    inputService.validate(regex, e);
                });
            }
        };
    }); 
    htm.app.directive("ngAlphaField", function (inputService) {
        return {
            require: "ngModel",
            link: function (scope, element, attrs) {
                //set element to text
                element.attr("type", "text");

                var _maxlength = attrs.maxlength;
                var _min = attrs.min;

                var regex = new RegExp("^[a-zA-Z]+$");

                element.on("keypress", function (e) {
                    inputService.validate(regex, e);
                });
            }
        };
    }); 
    htm.app.directive("ngAlphanumericField", function (inputService) {
        return {
            require: "ngModel",
            link: function (scope, element, attrs) {
                //set element to text
                element.attr("type", "text");

                var _maxlength = attrs.maxlength;
                var _min = attrs.min;

                var regex =  new RegExp("^[a-zA-Z0-9]+$");

                element.on("keypress", function (e) {
                    inputService.validate(regex, e);
                });
            }
        };
    });
})();