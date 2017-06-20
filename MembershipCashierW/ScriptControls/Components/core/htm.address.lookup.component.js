(function () {
    "use strict";

    if (!fedex.app) return;

    fedex.app.AddressLookupController = function ($rootScope, $scope, $global, Model, AddressLookupFactory, CountryFactory) {
        var ctrl = this;

        //google places api autocomplete class...
        ctrl.autocomplete = null;

        //address details...
        ctrl.valid = false;
        ctrl.submitted = false;
        ctrl.addressError = "";
        ctrl.proposedAddresses = [];
        ctrl.address = {};

        //address classification
        ctrl.classification = "";
        ctrl.residentialClassification = null;

        //collections
        ctrl.selectedCountry = null;
        ctrl.countries = [];
        ctrl.provinces = [];

        ctrl.$onInit = function () {
            if(!window && !window.document) throw "Address Lookup cannot be used without an html document.";

            ctrl.autocomplete = new google.maps.places.Autocomplete(
                document.getElementById("autocomplete"),
                {
                    types: ["geocode"]
                });

            ctrl.autocomplete.addListener("place_changed", ctrl.placeChanged);

            $global.startWork();

            CountryFactory.getCountries().then(function (result) {
                ctrl.countries = result;
            }).then($global.endWork);

            $scope.$watch(
                function () {
                    return (ctrl.valid);
                },
                function (newVal, oldVal) {
                    if (newVal) {
                        ctrl.address.IsResidential = ctrl.residentialClassification;

                        $rootScope.$emit("addressChanged", {
                            Valid: true,
                            Address: ctrl.address
                        });
                    }
                    else {
                        $rootScope.$emit("addressChanged", {
                            Valid: false
                        });
                    }
                });

            $scope.$watch(
                function () {
                    return (ctrl.residentialClassification);
                },
                function (newVal, oldVal) {
                    if (newVal === null) {
                        ctrl.valid = false; //no classification should invalidate the address since it's required.
                        return;
                    }

                    ctrl.valid = true; //A value was received.
                });

            $scope.$watch(
                function () {
                    return (ctrl.address && ctrl.address.Country);
                },
                function (val) {
                    if (!val) {
                        ctrl.provinces = [];
                        return;
                    }

                    ctrl.provinces = CountryFactory.getProvinces(val);
                    ctrl.selectedCountry = ctrl.countries.first(function (c) { return c.CountryCode == val });
                });

            $("#new-location-modal").on("show.bs.modal", function () {
                ctrl.previousValues = {
                    Address1: ctrl.address.Address1,
                    City: ctrl.address.City,
                    Province: ctrl.address.Province,
                    Country: ctrl.address.Country,
                    PostalCode: ctrl.address.PostalCode
                };
            });
        };

        ctrl.placeChanged = function () {
            var place = ctrl.autocomplete.getPlace();
            ctrl.valid = false;

            if (!place.address_components) {
                ctrl.address = {};
                $("#new-location-modal").modal("show");

                return;
            }

            ctrl.address = {};
            var components = place.address_components;

            $.each(components, function (i) {
                var comp = components[i];
                ctrl.address[comp.types[0]] = comp.short_name;
            });
            
            ctrl.address = AddressLookupFactory.convertGoogleAddressToJson(ctrl.address);

            var country = ctrl.countries.first(function (c) { return c.CountryCode == ctrl.address.Country });

            if ((country.RequiresPostalCode && !ctrl.address.PostalCode.trim()) || (country.RequiresCity && !ctrl.address.City.trim())) {
                $scope.$apply(function () {
                    ctrl.addressError = "Please fill in required fields.";
                    ctrl.address = ctrl.address
                });

                $("#new-location-modal").modal("show");

                return;
            }

            ctrl.saveAddress(true);
        };

        ctrl.saveAddress = function (showModalOnError) {
            $global.startWork("Looking up Address");

            AddressLookupFactory.validate(ctrl.address)
                .then(
                    function (response) {
                        var data = response.data[0];

                        if (data) {
                            ctrl.classification = data.Classification;
                            ctrl.residentialClassification = ctrl.classification === "RESIDENTIAL";

                            var country = ctrl.countries.first(function (c) { return c.CountryCode === ctrl.address.Country });
                            ctrl.address.CountryName = country ? country.CountryName : null;
                        }

                        //var proposedAddress = data.ProposedAddress ? data.ProposedAddress.first() : null;
                        ////proposed address fixes
                        //if (proposedAddress) {
                        //    proposedAddress.PostalCode = proposedAddress.PostalCode ? proposedAddress.PostalCode.replace(" ", "").replace("-", "") : "";
                        //    proposedAddress.ProvinceName = proposedAddress.Province && proposedAddress.Province.length == 2 ? null : proposedAddress.Province;
                        //    proposedAddress.Province = proposedAddress.Province && proposedAddress.Province.length == 2 ? proposedAddress.Province : null;

                        //    ctrl.address = proposedAddress;
                        //}

                        if (!showModalOnError) {
                            $("#new-location-modal").modal("hide");
                            ctrl.submitted = false;
                        }

                        $("#autocomplete").val(AddressLookupFactory.formatAddressToString(ctrl.address));
                        ctrl.valid = true;
                        ctrl.addressError = "";
                    },
                    function (response) {
                        //hack for classification if only 1 error and that error is unknown it is a valid address but
                        //it cannot classify it
                        if (response.data.length === 1 && response.data[0].ErrorMessage === "UNKNOWN") {
                            ctrl.classification = response.data[0].ErrorMessage;
                            ctrl.residentialClassification = null;
                            var country = ctrl.countries.first(function (c) { return c.CountryCode === ctrl.address.Country });
                            ctrl.address.CountryName = country ? country.CountryName : null;

                            ctrl.addressError = ""; //no address errors since UNKNOWN WAS RETURNED

                            //var proposedAddress = data.ProposedAddress != null ? data.ProposedAddress.first() : null;
                            //if (proposedAddress) {
                            //    //proposed address fixes
                            //    proposedAddress.PostalCode = proposedAddress.PostalCode ? proposedAddress.PostalCode.replace(" ", "").replace("-", "") : "";
                            //    proposedAddress.ProvinceName = proposedAddress.Province && proposedAddress.Province.length == 2 ? null : proposedAddress.Province;
                            //    proposedAddress.Province = proposedAddress.Province && proposedAddress.Province.length == 2 ? proposedAddress.Province : null;

                            //    ctrl.address = proposedAddress;
                            //}

                            if (!showModalOnError) {
                                $("#new-location-modal").modal("hide");
                                ctrl.submitted = false;
                            }

                            $("#autocomplete").val(AddressLookupFactory.formatAddressToString(ctrl.address));
                            ctrl.valid = false;

                            return;
                        }

                        if (response.data.length > 0)
                            ctrl.addressError = $.map(response.data, function (d) { return d.ErrorMessage; }).join("<br/>");

                        if (!showModalOnError) return;

                        $("#new-location-modal").modal("show");
                    })
                .then($global.endWork);
        };

        ctrl.cancelAddress = function () {
            //address currently is still valid therefore, do not refresh any fields.
            if (ctrl.address) {
                ctrl.address.Address1 = ctrl.previousValues.Address1;
                ctrl.address.City = ctrl.previousValues.City;
                ctrl.address.Province = ctrl.previousValues.Province;
                ctrl.address.Country = ctrl.previousValues.Country;
                ctrl.address.PostalCode = ctrl.previousValues.PostalCode;

                ctrl.previousValues = null;

                return;
            }

            ctrl.reset();
        };

        ctrl.reset = function () {
            //reset model
            ctrl.valid = false;

            ctrl.address = {};
            ctrl.addressError = "";
            ctrl.residentialClassification = null;
            ctrl.proposedAddresses = [];
            ctrl.selectedCountry = null;

            $("#autocomplete").val("");
        };
    };

    fedex.app.component("ngAddressLookupComponent", {
        templateUrl: "/ScriptControls/Views/core/address.lookup.view.html",
        bindings: {
            ngLocked: "="
        },
        controller: fedex.app.AddressLookupController
    });
})();