if (app) {
    app.factory("authenticationFactory", ["$global", "$http", function ($global, $http) {
        return {
            authenticate: function (identity) {
                return $http.post("/api/Authentication/authenticate", JSON.stringify(identity), { "Content-Type": "application/json" });
            }
        };
    }]);

    app.directive("login", function () {
        return {
            restrict: "A",
            controller: function ($scope, $global, authenticationFactory) {
                $scope.login = function () {
                    authenticationFactory.authenticate({
                        UserName: $scope.userName,
                        Password: $scope.password,
                        Captcha: $scope.captcha
                    })
                    .then(
                    //success
                    function (response) {
                        var requestTokenAuthentication = response.headers("antiForgery");
                        $global.setAntiForgeryToken(requestTokenAuthentication);

                        window.location = "../root"
                    },
                    //error
                    function (response) {
                        $scope.error = true;
                        var captcha = response.headers("WWW-Authenticate");

                        if (captcha) {
                            $scope.captchaRequired = true;
                            var base64 = (/data:image\/jpeg;base64.*/ig).exec(captcha);
                            $scope.captchaImage = base64[0];
                        }
                    });
                }
            },
            templateUrl: "../scriptcontrols/views/login.view.html"
        };
    });

}
    