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
                    });
            }
        };
    }]);

    //app.directive("userProfiles", function () {
    //    return {
    //        restrict: "A",
    //        controller: function ($scope, $global, userProfileDataFactory) {

    //        },
    //        templateUrl: "../scriptcontrols/views/userprofiles.view.html"
    //    };
    //});

    var UserProfilesController = function ($scope, $global, userProfileDataFactory) {
        var $this = this;
        $this.blob = {};

        $this.load = function () {
            
            var result = userProfileDataFactory.get()
                .then(
                function (response) {
                    try {
                        $this.blob.UserProfiles = [];

                        $.each(response.data, function (i, x) {
                            $this.blob.UserProfiles.push({
                                UserId: x.UserId,
                                UserName: x.UserName,
                                FirstName: x.FirstName,
                                LastName: x.LastName,
                                EmailId: x.EmailId
                            });
                        });
                    } catch (e) {
                        throw (e);
                    }
                },
                function (response) {
                    alert("An error occurred! Please contact your administrator for help.");
                });
        };

        $this.loadProfile = function (username) {
            if ($scope.loadProfile) $scope.loadProfile(username);
        };

        $this.load();
    };

    app.component("userProfiles", {
        templateUrl: "../scriptcontrols/views/userprofiles.view.html",
        bindings: {

        },
        controller: UserProfilesController
    });
}