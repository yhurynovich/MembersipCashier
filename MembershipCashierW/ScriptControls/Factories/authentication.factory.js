(function () {
    "use strict";
    if (!htm.app) return;

    htm.app.factory("$authentication", ["$http", function ($http) {
        var factory = {};
        var token = "htmAuth";

        factory.login = function (credentials) {
            return $http.post("/api/authentication/login", {
                UserName: credentials.username,
                Password: credentials.password,
                Captcha: credentials.captcha
            });
        };

        //factory.loginDevice = function (credentials) {
        //    return $http.post("/api/deviceauthentication/login", {
        //        DeviceId: credentials.deviceId,
        //        Captcha: credentials.captcha
        //    });
        //};

        factory.getUserLocations = function () {
            return $http.get("/api/userlocations");
        };

        factory.setUserLocation = function (locationId) {
            return $http.post("/api/userlocations", locationId);
        };

        factory.logout = function () {
            return $http.get("/api/authentication/logout");
        };

        factory.logoutDevice = function () {
            return $http.get("/api/deviceauthentication/logout");
        };

        factory.changeLanguage = function (language) {
            return $http.get("/api/localization?lng=" + language);
        };

        return factory;
    }]);
})();