'use strict';

htm.app.controller('ProductsCtrl',
    ['$location', '$q', '$routeParams', 'productService', '$http',
    function ($location, $q, $routeParams, productService, $http) {
        var ctrl = this;
        ctrl.userAndProducts = [];
        ctrl.products = [];
        ctrl.loading = false;
        ctrl.filter = $routeParams.filter;

        var currentPageIndex = 0, currentRequestStream = 0;
        const PAGE_SIZE = 20;

        ctrl.getMore = function () {
            ctrl.loading = true;

            var promise = $q.defer();
            var pageIndex = ++currentPageIndex;
            var requestStream = currentRequestStream;
	        var curentFilter = ctrl.filter;
            var skip = (pageIndex - 1) * PAGE_SIZE;

            productService.ClientProducts.query({
                blob: ctrl.filter ? ctrl.filter : "",
                skip: skip,
                take: PAGE_SIZE
            }, function (extraProducts) {
                if (requestStream === currentRequestStream && curentFilter === ctrl.filter) {
                    Array.prototype.push.apply(ctrl.userAndProducts, extraProducts);
                    ctrl.loading = false;
                }

                promise.resolve();
            }, function (err) {
                promise.resolve();
                if (requestStream === currentRequestStream && curentFilter === ctrl.filter) {
                    ctrl.loading = false;
                }
                ctrl.error = err.data.Message || err.data;
            });

            return promise.promise;
        };

        ctrl.doSearch = function () {
            if (ctrl.searchString != null && ctrl.searchString.length > 0) {
                ctrl.filter = ctrl.searchString;
            } else {
                ctrl.filter = "";
            }

            ctrl.products = [];
            currentPageIndex = 0;
            currentRequestStream = 0;

            ctrl.getMore();
        };

        ctrl.loadProducts = function (userId) {
            ctrl.loading = true;
            $http({
                method: 'GET',
                url: appRoot + 'api/ClientLastUsedProducts?blob=a&skip=0&take=100'
            }).then(function successCallback(response) {
                ctrl.userAndProducts = response.data;
                ctrl.loading = false;
            }, function errorCallback(response) {
                ctrl.loading = false;
            });
        };

        ctrl.updateBalance = function (product, ballanceUnits) {
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
                    ctrl.BallanseResponse = response.data;
                    ctrl.loading = false;
                }, function errorCallback(response) {
                    ctrl.errorResponse = response.data;
                    ctrl.loading = false;
                });
            }
        };

        ctrl.calcCurrentBalance = function (products) {
            var total = 0;
            angular.forEach(products, function (key, value) {
                total += key.ProfileCredit.Ballance;                
            });
            return total;
        };
    }]);