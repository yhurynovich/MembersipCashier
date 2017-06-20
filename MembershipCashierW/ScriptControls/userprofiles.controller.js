if (app) {
    app.factory("userProfileDataFactory", ["$global", "$http", function($global, $http){
        return {
            //get user profile
            get: function (username) {
                return $http.get(username ? "/api/UserProfile/" + username : "/api/UserProfile");
            },
            //load user profile....
            update: function (profiles) {
                return $http.post(
                    "/api/UserProfile",
                    JSON.stringify(profiles),
                    //Options
                    {
                        "Content-Type": "application/json",
                        headers: {
                            "RequestVerificationToken": $global.getAntiForgeryToken()
                        }
                    });
            }
        };
    }]);

    app.directive("userProfiles", function () {
        return {
            restrict: "A",
            controller: function ($scope, $global, userProfileDataFactory) {
                $scope.blob = {};

                this.load = function () {
                    var result = userProfileDataFactory.get()
                        .then(
                        function (response) {
                            try
                            {
                                $scope.blob.UserProfiles = [];

                                $.each(response.data, function (i, x) {
                                    $scope.blob.UserProfiles.push({
                                        UserName: x.UserName,
                                        FirstName: x.FirstName,
                                        LastName: x.LastName,
                                        EmailId: x.EmailId
                                    });
                                });
                            } catch (e) {
                                $global.setAntiForgeryToken(response.headers("antiForgery"));
                                throw (e);
                            }
                        },
                        function (response) {
                            alert("An error occurred! Please contact your administrator for help.");
                        });
                };

                this.loadProfile = function (username) {
                    if ($scope.loadProfile) $scope.loadProfile(username);
                };

                this.load();
            },
            templateUrl: "../scriptcontrols/views/userprofiles.view.html"
        };
    });
}