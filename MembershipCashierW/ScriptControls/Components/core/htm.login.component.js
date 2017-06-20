(function () {
    "use strict";

    if (!htm.app) return;

    htm.app.LoginController = function ($scope, $window, $global, $authentication, Model) {
        var ctrl = this;
        ctrl.model = Model;

        ctrl.selectedLoginType = ctrl.model.LoginType.MainStation;
        ctrl.userName = null;
        ctrl.password = null;

        ctrl.captchaRequired = false;
        ctrl.captchaImage = null;
        ctrl.captcha = null;

        ctrl.deviceId = null;

        ctrl.error = null;
        ctrl.suggestedUrl = null;


        ctrl.locations = [];
        ctrl.selectedLocation = null;

        ctrl.$onInit = function () {
            var modal = $("#login-modal");

            if (modal) {
                modal.on("hide.bs.modal", function () {
                    ctrl.reset();
                });
            }
        };

        ctrl.$onDestroy = function () {
            var modal = $("#login-modal");

            if (modal) {
                modal.off("hide.bs.modal");
            }
        };

        ctrl.login = function () {
            var credentials = {
                captcha: ctrl.captcha
            };

            if (ctrl.selectedLoginType == Model.LoginType.MainStation) {
                credentials.username = ctrl.userName;
                credentials.password= ctrl.password;
                
                $authentication.login(credentials)
                    .then(
                    //success
                    function (response) {
                        if (response.data === undefined || response.data.suggestedUrl === undefined)
                            ctrl.suggestedUrl = "../root";
                        else
                            ctrl.suggestedUrl = response.data.suggestedUrl;
                        ctrl.error = null;

                        $authentication.getUserLocations()
                            .then(function (response) {
                                var locations = response.data;

                                //no user locations loaded.
                                if (locations.length == 0)
                                    $window.location = ctrl.suggestedUrl;

                                var $default = locations.length == 1;

                                if ($default) {
                                    $authentication.setUserLocation(locations.first().UserLocation.LocationId)
                                        .then(
                                            function (response) {
                                                ctrl.error = null;

                                                if (response.data.Success) {
                                                    $window.location = ctrl.suggestedUrl;
                                                    return;
                                                }
                                                else {
                                                    ctrl.error = response.data.Reason;
                                                }
                                            },
                                            function (response) {
                                                ctrl.error = response.data;
                                            });
                                }

                                ctrl.locations = locations;
                            });
                    },
                    //error
                    function (response) {
                        ctrl.error = response.status === 401 ? "The user name and password entered are incorrect. Please ensure you've entered them in correctly." : "There was an error attempting to login. Please try again later.";

                        //reset password and captcha
                        ctrl.password = null;
                        ctrl.captcha = null;

                        var captcha = response.headers("WWW-Authenticate");

                        if (captcha) {
                            var base64 = (/data:image\/jpeg;base64.*/ig).exec(captcha);

                            ctrl.captchaRequired = true;
                            ctrl.captchaImage = base64[0];
                        }
                    });
            }
            else {
                credentials.deviceId = ctrl.deviceId;

                $authentication.loginDevice(credentials)
                    .then(
                        function (response) {
                            ctrl.error = null;

                            $window.location = ctrl.suggestedUrl;
                        },
                        function (response) {
                            ctrl.error = response.status === 401 ? "Device Not Registered" : "There was an error attempting to login. Please try again later.";

                            //reset device Id and captcha
                            ctrl.deviceId = null;
                            ctrl.captcha = null;

                            var captcha = response.headers("WWW-Authenticate");

                            if (captcha) {
                                var base64 = (/data:image\/jpeg;base64.*/ig).exec(captcha);

                                ctrl.captchaRequired = true;
                                ctrl.captchaImage = base64[0];
                            }
                        });
            }
        };

        ctrl.reset = function () {
            $scope.$apply(function () {
                ctrl.userName = null;
                ctrl.password = null;
                ctrl.captchaRequired = false;
                ctrl.captchaImage = null;
                ctrl.captcha = null;
                ctrl.deviceId = null;
                ctrl.error = null;
                ctrl.locations = [];
                ctrl.selectedLocation = null;
            });
        };

        ctrl.locationSelected = function () {
            $authentication.setUserLocation(ctrl.selectedLocation.UserLocation.LocationId)
                .then(
                    function (response) {
                        ctrl.error = null;

                        if (response.data.Success) {
                            $window.location = "/root";
                            return;
                        }
                        else {
                            ctrl.error = response.data.Reason;
                        }
                    });
        };
    };

    htm.app.component("ngLoginComponent", {
        templateUrl: "/ScriptControls/Views/core/login.view.html",
        controller: htm.app.LoginController
    });
})();