'use strict'

htm.app.controller('AddProductCtrl',
    ['$scope', '$timeout', '$http', '$uibModalInstance', 'user',
    function ($scope, $timeout, $http, $uibModalInstance, user) {
        //var ctrl = this;
        //$scope.selectedUser = user;
        $scope.products = [];
        $scope.loading = false;
        //ctrl.filter = $routeParams.filter;
        $scope.products = [];

        $scope.doSearch = function () {
            if ($scope.searchString != null && $scope.searchString.length > 0) {
                $scope.filter = $scope.searchString;
            } else {
                $scope.filter = "";
            }
            //$http({
            //    method: 'GET',
            //    url: appRoot + 'api/Product?request.notInCurrentUserHistory=true' //request.productLambda=x%253D%253Ex.Description%253D%253D%22' + $scope.searchString + '%22'
            //}).then(function successCallback(response) {
            //    $scope.products = response.data;   
            //    $scope.loading = false;
            //}, function errorCallback(response) {
            //    $scope.loading = false;                
            //});
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        $scope.loadProducts = function () {
            $http({
                method: 'GET',
                url: appRoot + 'api/Product?request.notInUserHistory=' + user.UserProfile.UserId
            }).then(function successCallback(response) {
                $scope.products = response.data;

                $scope.loading = false;

            }, function errorCallback(response) {
                $scope.loading = false;
            });
        };

        $scope.addProduct = function (product) {
            let profileCredit = {
                UserId: user.UserProfile.UserId,
                ProductId: product.ProductId,
                LocationId: (user.Products.length > 0)? user.Products[0].ProfileCredit.LocationId: 1,
                CalculatedTime: new Date(),
                Ballance: 0,
                BallanceUnits: 0
            }                 
            $http({
                method: 'POST',
                url: appRoot + 'api/ProfileCredit',
                data: [profileCredit]
            }).then(function successCallback(response) {
                var newProduct = {
                    ProfileCredit: {
                        UserId: user.UserProfile.UserId,
                        ProductId: product.ProductId,
                        LocationId: profileCredit.LocationId,
                        CalculatedTime: profileCredit.CalculatedTime,
                        Ballance: 0,
                        BallanceUnits: 0
                    },
                    Product: {
                        Description: product.Description,
                        ProductId: product.ProductId
                    }
                };               
                user.Products.push(newProduct);
                if ($scope.products.indexOf(product) >= 0) {
                    $scope.products.splice($scope.products.indexOf(product), 1);
                }
            }, function errorCallback(response) {
            });
        };
    }]);