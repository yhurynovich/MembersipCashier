'use strict';

htm.app.controller('ProductsCtrl',
    ['$scope', '$location', '$q', '$routeParams', 'productService', '$http', '$uibModal',
    function ($scope, $location, $q, $routeParams, productService, $http, $uibModal) {
        var ctrl = this;
        $scope.userAndProducts = [];
        $scope.products = [];
        $scope.loading = false;
        $scope.filter = $routeParams.filter;

        var currentPageIndex = 0, currentRequestStream = 0;
        const PAGE_SIZE = 20;

        $scope.getMore = function () {
            $scope.loading = true;

            var promise = $q.defer();
            var pageIndex = ++currentPageIndex;
            var requestStream = currentRequestStream;
	        var curentFilter = $scope.filter;
            var skip = (pageIndex - 1) * PAGE_SIZE;

            productService.ClientProducts.query({
                blob: $scope.filter ? $scope.filter : "",
                skip: skip,
                take: PAGE_SIZE
            }, function (extraProducts) {
                if (requestStream === currentRequestStream && curentFilter === $scope.filter) {
                    //Array.prototype.push.apply($scope.userAndProducts, extraProducts);
                    $scope.userAndProducts = extraProducts;
                    $scope.loading = false;
                }

                promise.resolve();
            }, function (err) {
                promise.resolve();
                if (requestStream === currentRequestStream && curentFilter === $scope.filter) {
                    $scope.loading = false;
                }
                $scope.error = err.data.Message || err.data;
            });

            return promise.promise;
        };

        $scope.doSearch = function () {
            if ($scope.searchString != null && $scope.searchString.length > 0) {
                $scope.filter = $scope.searchString;
            } else {
                $scope.filter = "";
            }

            $scope.products = [];
            currentPageIndex = 0;
            currentRequestStream = 0;

            $scope.getMore();
        };

        $scope.loadClientLastUsedProducts = function (userId) {
            $scope.loading = true;
            $http({
                method: 'GET',
                url: appRoot + 'api/ClientLastUsedProducts?blob=a&skip=0&take=100'
            }).then(function successCallback(response) {
                $scope.userAndProducts = response.data;
                $scope.loading = false;
            }, function errorCallback(response) {
                $scope.loading = false;
            });
        };

        $scope.updateBalance = function (product, ballanceUnits) {
            if (product) {
                let transaction = {
                    UserId: product.ProfileCredit.UserId,
                    ProductId: product.ProfileCredit.ProductId,
                    LocationId: product.ProfileCredit.LocationId,
                    BallanceUnits: ballanceUnits
                }

                $http({
                    method: 'PUT',
                    url: appRoot + 'api/CreditTransaction',
                    data: [transaction]
                }).then(function successCallback(response) {
                    var ballanseResponse = response.data[0];
                    product.ProfileCredit.Ballance = ballanseResponse.Ballance;
                    $scope.loading = false;
                }, function errorCallback(response) {
                    $scope.errorResponse = response.data;
                    $scope.loading = false;
                });
            }
        };

        $scope.calcCurrentBalance = function (products) {
            var total = 0;
            angular.forEach(products, function (key, value) {
                total += (key.ProfileCredit.Ballance * key.ProfileCredit.BallanceUnits);                
            });
            return total;
        };

        $scope.OpenAddProductDialog = function (selectedUser) {
            //var modalScope = $scope.$new();

            //var modalInstance = $uibModal.open({
            //    templateUrl: '/ScriptControls/Views/addProductModal.html',
            //    controller: 'AddProductCtrl',
            //    scope: modalScope
            //});

            //modalScope.modalInstance = modalInstance;

            //modalInstance.result.then(function (result) {
            //}, null);
            $uibModal.open({
                templateUrl: '/ScriptControls/Views/addProductModal.html',
                controller: 'AddProductCtrl',
                resolve: {
                    user: function () {
                        return selectedUser;
                    }
                }
            }).result.then(function ($scope) {
            }, function () {
            });
        };

        $scope.openTransHistoryDialog = function (selectedUser) {
            $uibModal.open({
                templateUrl: '/ScriptControls/Views/transHistoryModal.html',
                controller: 'TransHistoryCtrl',
                resolve: {
                    user: function () {
                        return selectedUser;
                    }
                }
            }).result.then(function ($scope) {
            }, function () {
            });
        };
    }]);