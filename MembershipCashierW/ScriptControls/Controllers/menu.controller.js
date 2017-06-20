(function () {
    "use strict";

    if (!htm.app) return;

    htm.app.controller("MenuController", ["$scope", "$window", "$timeout", "$authentication", "$global", function ($scope, $window, $timeout, $authentication, $global) {
        var animations = new htm.app.MenuAnimations($timeout);
        var languages = [
            { code: "en", description: "English" },
            { code: "fr", description: "Français" }
        ];

        $scope.currentLanguage = $global.getLanguage();
        $scope.altLanguage = "";

        this.$onInit = function(){
            $scope.altLanguage = languages.first(function (l) { return l.code !== $scope.currentLanguage }).code;
        };

        $scope.logout = function () {
            $authentication.logout().then(
                function (response) {
                    $window.location = "/root/login";
                },
                function (response) {
                    //ERROR TODO: ERROR HANDLING LOGGING OUT
                });
        };

        $scope.changeLanguage = function (lang) {
            $scope.currentLanguage = lang;
            $scope.altLanguage = languages.first(function (l) { return l.code !== $scope.currentLanguage }).code;

            $authentication.changeLanguage(lang).then(function () {
                $global.setLanguage(lang);
                window.location.reload();
            });
        };

        $scope.getLanguage = function (lang) {
            return languages.first(function (l) { return l.code === lang; }).description;
        };

        $scope.manageAccounts = function () {
            $window.open('/USR/ManageAccounts?ui_lang=' + $scope.currentLanguage, '_blank', 'toolbar,status');
        };

        $scope.showMenu = function () {
            animations.dropMenu();
        };
    }]);
})();