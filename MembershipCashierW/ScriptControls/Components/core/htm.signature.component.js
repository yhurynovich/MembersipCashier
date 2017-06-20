(function () {
    "use strict";

    if (!fedex.app) return;

    fedex.app.component("ngSignatureComponent", {
        templateUrl: "/ScriptControls/Views/core/signature.view.html",
        bindings:{
            ngSignaturePadDeviceType: "=",
            ngIsMobileDevice: "=",
            ngSignatureSVG: "="
        },
        controller: function ($scope, ) {

        }
    });
})();