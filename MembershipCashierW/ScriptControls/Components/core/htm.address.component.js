(function () {
    "use strict";

    if (!fedex.app) return;

    fedex.app.AddressController = function ($scope, AddressFactory) {
        var ctrl = this;

        ctrl.blob = {
            Address: null
        };

        ctrl.init = function () {
            $scope.$watch(
                function () {
                    return ctrl.addressId;
                },
                function (newVal, oldVal) {
                    if (!newVal) {
                        //clear address since no address provided
                        ctrl.blob.Address = null;
                        return;
                    }

                    AddressFactory.getAddress(newVal)
                        .then(
                        function (response) {
                            var address = response.data[0];
                            ctrl.blob.Address = angular.copy(address);
                        },
                        function (response) {

                        });
            });
        };

        ctrl.init();
    };

    fedex.app.component("ngAddressComponent", {
        templateUrl: "/ScriptControls/Views/core/address.view.html",
        bindings: {
            "addressId": "="
        },
        controller: fedex.app.AddressController
    });
})();