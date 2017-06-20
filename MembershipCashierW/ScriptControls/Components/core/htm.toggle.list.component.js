(function () {
    "use strict";

    if (!htm.app) return;

    htm.app.component("ngToggleListComponent", {
        template: function () {
            return "\
                <div class='btn-group' role='group'>\
                    <button type='button' class='btn btn-default' ng-class='{ active: $ctrl.selectedValue == $ctrl.defaultValue1.value }' ng-click='$ctrl.toggleValue($ctrl.defaultValue1.value)'>{{$ctrl.defaultValue1.display | translate}}</button>\
                    <div class='separator'></div>\
                    <button type='button' class='btn btn-default' ng-class='{ active: $ctrl.selectedValue == $ctrl.defaultValue2.value }' ng-click='$ctrl.toggleValue($ctrl.defaultValue2.value)'>{{$ctrl.defaultValue2.display | translate}}</button>\
                    <div class='separator' ng-show='$ctrl.values.length > 0'></div>\
                    <div class='btn-group' role='group' ng-show='$ctrl.values.length > 0'>\
                        <button type='button' class='btn btn-default dropdown-toggle' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false' ng-class='{active: $ctrl.selectedValue != null && $ctrl.selectedValue != $ctrl.defaultValue1.value && $ctrl.selectedValue != $ctrl.defaultValue2.value}'>{{$ctrl.toggleListTouched? $ctrl.selectedValueDisplay : 'More'}} <span class='caret'></span></button>\
                        <ul class='dropdown-menu' role='menu' style='max-height: 300px; overflow: auto;'>\
                            <li ng-repeat='val in $ctrl.values'><a ng-click='$ctrl.toggleListValue(val.value)' style='cursor: pointer;'>{{val.display | translate}}</a></li>\
                        </ul>\
                    </div>\
                </div>";
        },
        bindings: {
            ngModel: "=",
            ngValues: "=",
            ngDefaultValue1: "@",
            ngDefaultValue2: "@",
            ngValue: "@",
            ngDisplay: "@"
        },
        controller: function ($scope, $element, $attrs) {
            var ctrl = this;
            
            ctrl.values = [];
            ctrl.toggleListTouched = false;

            ctrl.selectedValue = null;
            ctrl.selectedValueDisplay = null;

            ctrl.defaultValue1 = null;
            ctrl.defaultValue2 = null;

            ctrl.initValues = function () {
                if (!ctrl.ngValues || ctrl.ngValues.length == 0) return;

                var default1 = $.grep(ctrl.ngValues, function (d) {
                    return ctrl.ngValue ? d[ctrl.ngValue] == ctrl.ngDefaultValue1 : d == ctrl.ngDefaultValue1
                })[0];

                ctrl.defaultValue1 = {
                    value: ctrl.ngValue ? default1[ctrl.ngValue] : default1,
                    display: ctrl.ngDisplay ? default1[ctrl.ngDisplay] : ctrl.ngValue ? default1[ctrl.ngValue] : default1
                };

                var default2 = $.grep(ctrl.ngValues, function (d) {
                    return ctrl.ngValue ? d[ctrl.ngValue] == ctrl.ngDefaultValue2 : d == ctrl.ngDefaultValue2
                })[0];

                ctrl.defaultValue2 = {
                    value: ctrl.ngValue ? default2[ctrl.ngValue] : default2,
                    display: ctrl.ngDisplay ? default2[ctrl.ngDisplay] : ctrl.ngValue ? default2[ctrl.ngValue] : default2
                };

                ctrl.values = $.map(ctrl.ngValues, function (d) {
                    if ((ctrl.ngValue ? d[ctrl.ngValue] == ctrl.ngDefaultValue1 : d == ctrl.ngDefaultValue1) ||
                        (ctrl.ngValue ? d[ctrl.ngValue] == ctrl.ngDefaultValue2 : d == ctrl.ngDefaultValue2))
                        return;

                    return {
                        value: ctrl.ngValue ? d[ctrl.ngValue] : d,
                        display: ctrl.ngDisplay ? d[ctrl.ngDisplay] : ctrl.ngValue ? d[ctrl.ngValue] : d
                    };
                });

            };

            ctrl.$onInit = function () {
                ctrl.initValues();
                ctrl.selectedValue = this.ngModel;

                $scope.$watch(
                    function () {
                        return (ctrl.ngModel);
                    },
                    function (val) {
                        ctrl.selectedValue = val;
                    });

                $scope.$watchCollection(
                    function () {
                        return (ctrl.ngValues);
                    },
                    function (val) {
                        ctrl.initValues();
                    });
            };

            ctrl.toggleValue = function (value) {
                ctrl.selectedValue = value;

                try {
                    ctrl.ngModel = value;
                }
                catch(e){
                    throw "ngModel is not set.";
                }
            }

            ctrl.toggleListValue = function (value) {
                ctrl.selectedValue = value;
                ctrl.selectedValueDisplay = $.grep(ctrl.values, function (v) { return v.value === value; })[0].display;
                ctrl.toggleListTouched = true;

                try{
                    ctrl.ngModel = value;
                }
                catch (e) {
                    throw "ngModel is not set.";
                }
            };
        }
    });
})();