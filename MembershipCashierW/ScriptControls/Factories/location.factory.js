(function () {
    "use strict";

    if (!fedex.app) return;

    fedex.app.factory("LocationFactory", ["$http", "Model", "SpecialServiceFactory", function ($http, Model, SpecialServiceFactory) {
        var factory = {};

        factory.getLocations = function (companyId) {
            var query = "l => l.CompanyId == " + companyId;
            return $http.get("/api/location/query?lamda=" + encodeURI(query));
        };

        factory.getLocation = function (locationId) {
            var query = "l => l.LocationId ==" + locationId;
            return $http.get("/api/location/query?lamda=" + encodeURI(query));
        };

        factory.updateLocation = function (location) {
            return $http.post("/api/location", JSON.stringify([location]));
        };
        factory.getTimeZones = function () {
            return $http.get("/api/location");
        };
        //Location Accounts

        factory.getLocationAccounts = function (locationId) {
            return $http.get("/api/shipperaccount?locationId=" + locationId);
        };

        factory.insertLocationAccounts = function (accounts) {
            return $http.put("/api/shipperaccount", JSON.stringify(accounts));
        };

        factory.updateLocationAccounts = function (accounts) {
            return $http.post("/api/shipperaccount", JSON.stringify(accounts));
        };

        factory.getLocationContactParty = function (locationId) {
            return $http.get("/api/locationparty?locationId=" + locationId);
        };
          
        factory.updateLocationContactParty = function (locationId, partyId) {
            return $http.post("/api/locationpartysync", JSON.stringify([{
                LocationId: locationId,
                PartyId: partyId
            }]));
        };

        //Location Promotion

        factory.getPromotions = function (locationId, includeDeleted) {
            return $http.get("/api/Promotion?locationId=" + locationId + "&includeDeleted="+includeDeleted);
        };

        factory.getPromoAccounts = function (promoCodeHash, locationId) {
            return $http.get("/api/ShipperAccountVsPromotion?promoCodeHash=" + promoCodeHash + "&locationId=" + locationId);
        };

        factory.insertPromotion = function (promo,locationIds) {
            var promoRequest = {
                Promotions: [promo],
                Accounts: null,
                LocationIds: [locationIds]
            };

            return $http.put("/api/Promotion", JSON.stringify(promoRequest));
        };
        factory.updateLocationPromotion = function (promo, locationIds) {
            var promoRequest = {
                Promotions: [promo],
                Accounts: null,
                LocationIds: [locationIds]
            };
            return $http.post("/api/LocationVsPromotion", JSON.stringify(promoRequest));
        };
        factory.updatePromotion = function (promo) {
            return $http.post("/api/Promotion", JSON.stringify([promo]));
        };

        factory.deletePromotion = function (locationId, promoCodeHash) {
            return $http.delete("/api/LocationVsPromotion?locationId=" + locationId + "&promoCodeHash=" + promoCodeHash);
        };

        //Location Preferences

        factory.getPreferences = function (locationId) {
            var query = "l => l.LocationId == " + locationId;
            return $http.get("/api/locationpreference?lamda=" + encodeURI(query));
        };

        factory.updatePreferences = function (prefs) {
            return $http.post("/api/locationpreference", JSON.stringify([prefs]));
        };

        factory.insertPreferences = function (prefs) {
            return $http.put("/api/locationpreference", JSON.stringify([prefs]));
        };

        factory.getSaturdayPickup = function (deviceId) {
            return SpecialServiceFactory.getSpecialService(Model.SpecialServiceCodes.SaturdayPickup)
                .then(function (service) {
                    var hash = service ? service.SpecialServiceCodeHash : null;

                    if (!hash) return false;

                    var query = "a => a.DeviceIdHash == {0} && a.SpecialServiceCodeHash == {1}".replace("{0}", deviceId).replace("{1}", hash);

                    return $http.get("/api/allowedspecialservice?lamda=" + encodeURI(query)).then(function (response) {
                        return response.data.length == 1 && response.data[0];
                    });
                });
        };

        factory.updateSaturdayPickup = function (deviceId) {
            return SpecialServiceFactory.getSpecialService(Model.SpecialServiceCodes.SaturdayPickup)
                .then(function (service) {
                    var hash = service ? service.SpecialServiceCodeHash : null;

                    if (!hash)
                        throw "Currently no special services are available. Please contact your system administrator for help.";

                    return $http.put("/api/allowedspecialservice", JSON.stringify([{
                        DeviceIdHash: deviceId,
                        SpecialServiceCodeHash: hash
                    }]));
                });
        };

        factory.deleteSaturdayPickup = function (deviceId) {
            return SpecialServiceFactory.getSpecialService(Model.SpecialServiceCodes.SaturdayPickup)
                .then(function (service) {
                    var hash = service ? service.SpecialServiceCodeHash : null;

                    if (!hash)
                        throw "Currently no special services are available. Please contact your system administrator for help.";

                    var deviceQuery = "d => d.DeviceIdHash == " + deviceId;
                    var specialServiceQuery = "s => s.SpecialServiceCodeHash == " + hash;

                    return $http.delete("/api/allowedspecialservice?DeviceLambda=" + encodeURI(deviceQuery) + "&SpecialServiceLambda=" + encodeURI(specialServiceQuery));
                });
        };

        return factory;
    }]);
})();