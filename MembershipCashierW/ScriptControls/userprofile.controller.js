if (app) {
    app.directive("userProfile", function () {
        return {
            restrict: "A",
            controller: function ($scope, $global, userProfileDataFactory) {
                $scope.loadProfile = function (userName) {

                    userProfileDataFactory.get(userName)
                        .then(function (response) {
                            try {
                                var d = response.data[0]; if (!d) return;

                                $scope.firstName = d.FirstName;
                                $scope.lastName = d.LastName;
                                $scope.activeProfile = d.UserName;
                            } catch (e) {
                                $global.setAntiForgeryToken(response.headers("antiForgery"));
                                throw (e);
                            }
                    });

                };

                $scope.updateProfile = function () {
                    for (var i = 0; i < $scope.blob.UserProfiles.length; i++)
                        if ($scope.blob.UserProfiles[i].UserName === $scope.activeProfile) {
                            $scope.blob.UserProfiles[i].FirstName = $scope.firstName;
                            $scope.blob.UserProfiles[i].LastName = $scope.lastName;
                        }

                    userProfileDataFactory.update($scope.blob.UserProfiles)
                        .then(
                            function (response) {
                                $global.setAntiForgeryToken(response.headers("antiForgery"));
                            },
                            function (response) {
                                $global.setAntiForgeryToken(response.headers("antiForgery"));
                            });
                };
            },
            templateUrl: "../scriptcontrols/views/userprofile.view.html"
        };
    });
}