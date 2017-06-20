(function () {
    "use strict";

    if (!htm.app) return;

    htm.app.factory("SystemEventsFactory", ["$http", function ($http) {
        var factory = {};

        //factory.getSystemEvents = function () {
        //    return $http.post("/api/systemevent/query?lamda=");
        //};

        return factory;
    }]);
})();