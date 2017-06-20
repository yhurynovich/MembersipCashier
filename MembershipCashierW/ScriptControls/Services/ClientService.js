'use strict';

htm.app.service('clientService', ['$http', '$resource', '$json', '$q', function ($http, $resource, $json, $q) {

	this.Clients = $resource(appRoot + 'api/searchClient/');

	this.ClientProducts = $resource(appRoot + 'api/ClientLastUsedProducts');

}]);