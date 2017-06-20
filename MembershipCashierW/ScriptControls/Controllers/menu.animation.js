(function() {
    "use strict";
    if (!htm.app) return;

    htm.app.MenuAnimations = function ($timeout) {
        var ctrl = this;
        
        ctrl.dropMenu = function () {
            $("#htm-nav-menu").show();
            $("#htm-menu").hide();

            $timeout(function () {
                $("#htm-nav-menu").hide();
                $("#htm-menu").show();
            }, 5000);
        };
    };
})();