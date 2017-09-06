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
            $http({
                method: 'GET',
                url: appRoot + 'api/Product?request.productLambda=x%253D%253Ex.Description%253D%253D%22' + $scope.searchString + '%22'
            }).then(function successCallback(response) {
                $scope.products = response.data;
                $scope.products.push({
                    Description: "Spa"
                });
                $scope.products.push({
                    Description: "Soup"
                });
                $scope.products.push({
                    Description: "Test"
                });

                $scope.loading = false;
            }, function errorCallback(response) {
                $scope.loading = false;                
            });
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        $scope.loadProducts = function () {
            //alert('Test');
        };
    }]);