'use strict';

htm.app.service('productService', ['$http', '$resource', '$json', '$q', function ($http, $resource, $json, $q) {

	this.ClientProducts = $resource(appRoot + 'api/ClientLastUsedProducts');

}]);