if (app) {
    app.factory("userSnapshotDataFactory", ["$global", "$http", function ($global, $http) {
        return {
            //get user profile
            get: function (lambda) {
                return $http.get(lambda ? "/api/userSnapshot/Query?lamda=" + lambda : "/api/userSnapshot");
            },
            //load user profile....
            update: function (profiles) {
                return $http.post(
                    "/api/userSnapshot",
                    JSON.stringify(profiles),
                    //Options
                    {
                        "Content-Type": "application/json",
                    });
            }
        };
    }]);

    var UserSnapshotsController = function ($scope, $global, userSnapshotDataFactory) {
        var $this = this;
        $this.blob = {};
        $this.lambda = null;

        $this.load = function () {
            var result = userSnapshotDataFactory.get($this.lambda)
                .then(
                function (response) {
                    try {
                        $this.blob.UserProfiles = [];

                        $.each(response.data, function (i, x) {
                            $this.blob.UserProfiles.push({
                                UserId: x.UserProfile2.UserId,
                                UserName: x.UserProfile2.UserName,
                                FirstName: x.UserProfile2.FirstName,
                                LastName: x.UserProfile2.LastName,
                                EmailId: x.UserProfile2.EmailId,
                                ProfileCredits: x.ProfileCredits
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

        //$this.loadProfile = function (lambda) {
        //    if ($scope.loadProfile) $scope.loadProfile(lambda);
        //};

        $this.load();
        app.UserSnapshotsController = $this;
    };

    app.component("userSnapshots", {
        templateUrl: "../scriptcontrols/views/usersnapshots.view.html",
        bindings: {

        },
        controller: UserSnapshotsController
    });

    app.component("userSnapshot", {
        templateUrl: "../scriptcontrols/views/usersnapshot.view.html",
        bindings: {
            profile: '='
        },
    });

    app.component("shortUserprofile", {
        templateUrl: "../scriptcontrols/views/shortuserprofile.view.html",
        bindings: {
            profile: '='
        },
        controller: function ($http) {
            this.update = function () {
                $http.post("/api/UserProfile", [this.profile]);
            }
        }
    });

    app.component("profileCredits", {
        templateUrl: "../scriptcontrols/views/profilecredits.view.html",
        bindings: {
            credits: '='
        },
    });

    app.component("profileCredit", {
        templateUrl: "../scriptcontrols/views/profilecredit.view.html",
        bindings: {
            credit: '='
        },
        controller: function ($http) {
            this.update = function () {
                $http.post("/api/ProfileCredit", [ this.credit ]);
            }
        }
    });

    app.component("findUserprofile", {
        templateUrl: "../scriptcontrols/views/finduserprofile.view.html",
        bindings: {
            profileFilter: "="
        },
        controller: function ($http) {
            this.find = function () {};
            this.myModel = new function () { };
            this.find = function (fieldSet) {
                app.UserSnapshotsController.lambda = GetUserProfileLambda(fieldSet);
                app.UserSnapshotsController.load();
            };
        }
    });
}

function GetUserProfileLambda(x)
{
    var lambda = "x=>x.";
    if (x.FirstName !==undefined && x.FirstName.length > 0)
        lambda += "FirstName~=\"" + x.FirstName + "\"";
    if (x.LastName !== undefined && x.LastName.length > 0) {
        if (lambda.length > 3)
            lambda += " && "
        lambda += "LastName~=\"" + x.LastName + "\"";
    }
    return escape(lambda);
}