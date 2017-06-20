if (app) {
    app.directive("userProfile", function () {
        return {
            restrict: "A",
            controller: function ($scope, $global, $controller, userProfileDataFactory) {
                $scope.model = {};
                $scope.activeProfile = null;

                $scope.loadProfile = function (userName) {
                    userProfileDataFactory.get(userName)
                        .then(function (response) {
                            try {
                                var d = response.data[0];

                                if (!d) return;

                                $scope.activeProfile = d.UserName;

                                $scope.model = {
                                    firstName: d.FirstName,
                                    lastName: d.LastName,
                                };
                            } catch (e) {
                                throw (e);
                            }
                        });
                };

                $scope.save = function () {
                    if (!$scope.valid) return;

                    for (var i = 0; i < $scope.blob.UserProfiles.length; i++){
                        var profile = null;

                        if ($scope.blob.UserProfiles[i].UserName === $scope.activeProfile) {
                            profile = $scope.blob.UserProfiles[i];
                            profile.FirstName = $scope.firstName;
                            profile.LastName = $scope.lastName;

                            return;
                        }
                    }

                    userProfileDataFactory.update([profile])
                        .then(
                            function (response) {

                            },
                            function (response) {

                            });
                };

                $scope.cancel = function () {
                    $scope.activeProfile = null;
                    $scope.model = {};
                }

                $scope.valid = function () {
                    return false;
                }
            },
            templateUrl: "../scriptcontrols/views/userprofile.view.html"
        };
    });
}