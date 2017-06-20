var htm = {};
(function () {
    "use strict";

    htm.app = angular.module("htm.app", ['ngRoute','ngResource'])
        .factory("$global",
        [
            "$rootScope", function($rootScope) {
                $rootScope.$watch("doingHeavyWork",
                    function(val) {

                        if (!val) return;

                        if ($(".modal-open") && $(".modal-open").length > 0) {
                            $(".overlay").css({ top: 0 });
                            return;
                        }

                        var top = $(document.body).scrollTop(); //this works in chrome
                        var top2 = (document.documentElement || document.body.parentNode || document.body).scrollTop; //this works only in IE
                        top = top >= top2 ? top : top2;
                        //In IE, $(document.body).scrollTop() always returns 0. In order to fix, we check the values of top and top2 and return the value which is greater.

                        var offset = $(".navbar.navbar-default").height();

                       
                        if (top > 0 && top <= offset) {
                            $(".overlay").css({ top: (offset - top) + "px" });
                        } else if (top > offset) {
                            $(".overlay").css({ top: 0 }); //cover whole screen...
                        } else {
                            $(".overlay").css({ top: offset + "px" });
                        }

                    });

                return {
                    _autoWorkSet: false,
                    setLanguage: function(lang) {
                        localStorage["retailShipLanguage"] = lang ? lang : "en";
                    },
                    getLanguage: function() {
                        var lang = localStorage["retailShipLanguage"];
                        return lang ? lang : "en";
                    },
                    setAntiForgeryToken: function (token) {
                        if (token !== undefined && token != null) {
                            localStorage["requestAuthenticationToken"] = token;
                            //console.log("set: " + token);
                        }
                    },
                    getAntiForgeryToken: function() {
                        return localStorage["requestAuthenticationToken"];
                    },
                    startWork: function(workText) {
                        $rootScope.doingHeavyWorkText = workText ? workText : "Please Wait";
                        $rootScope.doingHeavyWork = true;
                    },
                    endWork: function() {
                        $rootScope.doingHeavyWork = false;
                    },
                    setAutoWorker: function() {
                        if (!this._autoWorkSet) this._autoWorkSet = true;
                    },
                    unsetAutoWorker: function() {
                        if (this._autoWorkSet) this._autoWorkSet = false;
                    },
                    autoWorker: function() {
                        return this._autoWorkSet;
                    }
                };
            }
        ])
        .config(function ($httpProvider, $locationProvider, $routeProvider) {
            $httpProvider.interceptors.push(function($global) {
                return {
                    request: function(config) {
                        config.headers["RequestVerificationToken"] = $global.getAntiForgeryToken();
                        config.headers["detailedErrors"] = "yes";

                        config.headers["Cache-Control"] = "no-cache";
                        config.headers["Pragma"] = "no-cache";

                        if ($global.autoWorker()) {
                            $global.startWork();
                        }

                        return config;
                    },
                    response: function(response) {
                        $global.setAntiForgeryToken(response.headers("antiForgery"));
                        //Program Errored END WORKER!
                        if (response.status < 200 && response.status >= 300) {
                            $global.endWork();
                        }

                        if ($global.autoWorker()) {
                            $global.endWork();
                        }

                        return response;
                    },
                    responseError: function (response) {
                        $global.setAntiForgeryToken(response.headers("antiForgery"));

                        if (response.status === 401) {
                            window.location = appRoot + 'root/login';
                        }

                        return $q.reject(response);
                    }
                };
            });
            //$locationProvider.html5Mode({ enabled: true, requireBase: false, rewriteLinks: false });

            $routeProvider
				.when('/Clients', {
					templateUrl: appRoot + '/ScriptControls/Views/clients.html',
					controller: 'ClientsCtrl',
					title: 'Clients'
				})
				.when('/Client/:id', {
					templateUrl: appRoot + '/ScriptControls/Views/client.html',
					controller: 'ClientCtrl',
					title: 'Client'
				})
				.otherwise({
					redirectTo: '/Clients'
				});
        })
        .directive("ngTranslate",
            function($global, LocalizationFactory) {
                return {
                    restrict: "A",
                    scope: {
                        translation: "&"
                    },
                    controller: function($http, $scope, $attrs) {
                        var lang = $global.getLanguage();

                        if (lang === "en") {
                            $scope.translation = $attrs.ngTranslate;
                            return;
                        }

                        $scope.translation = LocalizationFactory.get($attrs.ngTranslate, lang);
                    },
                    template: function() {
                        return "{{::translation}}";
                    }
                }
            })
        .filter("translate",
            function($global, LocalizationFactory) {
                return function(input) {
                    var lang = $global.getLanguage();
                    return LocalizationFactory.get(input, lang);
                };
            })

        .filter('forLoop',
            function() {
                return function (input, start, end) {
                    if (end > start) {
                        input = new Array(end - start);
                        for (var i = 0; start <= end; start++, i++) {
                            input[i] = start;
                        }
                    }
                    return input;
                }
            });

    if (!htm.app) throw "There was an error initializing the application.";
})();