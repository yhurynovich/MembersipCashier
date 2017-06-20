'use strict';

htm.app.controller('ClientsCtrl',
['$location', '$q', '$routeParams', 'clientService', '$http',
	function ($location, $q, $routeParams, clientService, $http) {
	    var ctrl = this;

	    ctrl.clients = [];
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

	        clientService.Clients.query({
	            blob: ctrl.filter ? ctrl.filter : "",
	            skip: skip,
	            take: PAGE_SIZE
	        }, function (extraClients) {

	            if (requestStream === currentRequestStream && curentFilter === ctrl.filter) {
	                Array.prototype.push.apply(ctrl.clients, extraClients);
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

	        ctrl.clients = [];
	        currentPageIndex = 0;
	        currentRequestStream = 0;

	        ctrl.getMore();
	    };

	    ctrl.goto = function (id) {
	        $location.path('Client/' + id);
	    };

	    ctrl.loadProducts = function (userId) {

	        $http({
	            method: 'POST',
	            url: appRoot + 'api/ClientLastUsedProducts?lambda=x%3D%3Ex.UserId%3D%3D' + userId + "&skip=0&take=100"
	        }).then(function successCallback(response) {
	            let client = ctrl.clients.filter(x => x.UserId === userId)[0];
	            client.Products = response.data[0].Products;
	        }, function errorCallback(response) {
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

	            }, function errorCallback(response) {
	            });
	        }
	    }
	}]);