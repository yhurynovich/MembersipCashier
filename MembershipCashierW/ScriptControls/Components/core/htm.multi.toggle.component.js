(function () {
    "use strict";

    if (!htm.app) return;

    htm.app.component("ngMultiToggleButton", {
        template: function(){
            return "<div class='btn-group' data-toggle='buttons'>\
                <label ng-repeat-start='value in $ctrl.ngValues' class='btn btn-default' ng-class='{active: $ctrl.ngModel === value.Value}' ng-click='$ctrl.toggle(value.Value)'>\
                    <input type='radio' name='{{$ctrl.ngName}}' value='{{value.Value}}' ng-model='$ctrl.ngModel' autocomplete='off'>{{value.Text | translate}}\
                </label>\
                <div ng-repeat-end ng-if='!$last' class='separator'></div>\
            </div>";
        },
        bindings: {
            ngValues: "=",
            ngModel: "=",
            ngName: "@"
        },
        controller: function ($scope, $element, $attrs) {
            var ctrl = this;

            ctrl.init = function () {
                $scope.$watch(
                    function(scope){
                        return (ctrl.ngModel);
                    },
                    function (newVal, oldVal) {
                        if (newVal !== oldVal)
                            ctrl.toggle(newVal);
                    });
            };

            ctrl.toggle = function(val) {
                if (ctrl.ngModel === undefined)
                    throw "ngModel is a required parameter!";

                ctrl.ngModel = val;
            };

            ctrl.init();
        }
    });
})();