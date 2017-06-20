// FEDEX Toggle Switch
//
// A basic toggle switch with 2 values (on/off, yes/no, true/false etc)
// Binds to a property and changes the properties value to true (on,yes,true) or false (off, no, false)
// depending on what toggle is selected.
// 
// ngToggleButtonComponent (<ng-toggle-button></ng-toggle-button>)
// Attributes:
//  ngModel (ng-model): property to bind state
//  ngName (ng-name): name of toggle group.
//  ngOnText (ng-on-text) : Text to display for on state, example, 'On', 'Enabled', 'True' etc. Default: 'True'
//  ngOffText (ng-off-text) : Text to display for off state, example, 'Off', 'Disabled', 'False' etc. Default: 'True'
(function () {
    "use strict";

    if (!htm.app) return;

    htm.app.component("ngToggleButton", {
        template: function(){
            return "<div class='btn-group' data-toggle='buttons'>\
                <label id='{{$ctrl.ngName}}Option1' class='btn btn-default' ng-class='{active: $ctrl.ngModel === $ctrl.ngOnValue}' ng-click='$ctrl.toggle($ctrl.ngOnValue)'>\
                    <input type='radio' name='{{$ctrl.ngName}}' value='{{$ctrl.ngOnValue? $ctrl.ngOnValue : true}}' ng-model='ngModel' ng-toggle='{{$ctrl.ngToggle}}' autocomplete='off'>{{$ctrl.ngOnText? $ctrl.ngOnText : 'True'}}\
                </label>\
                <div class='separator'></div>\
                <label id='{{$ctrl.ngName}}Option2' class='btn btn-default' ng-class='{active: $ctrl.ngModel === $ctrl.ngOffValue || $ctrl.ngModel === false}' ng-click='$ctrl.toggle($ctrl.ngOffValue)'>\
                    <input type='radio' name='{{$ctrl.ngName}}' value='{{$ctrl.ngOffValue? $ctrl.ngOffValue : false}}' ng-model='ngModel' ng-toggle='{{$ctrl.ngToggle}}' autocomplete='off'>{{$ctrl.ngOffText? $ctrl.ngOffText : 'False'}}\
                </label>\
            </div>";
        },
        bindings: {
            ngToggle: "=",
            ngToggleParameter: "=",
            ngModel: "=",
            ngName: "@",
            ngOnValue: "@",
            ngOffValue: "@",
            ngOnText: "@",
            ngOffText: "@",
        },
        controller: function ($scope, $element, $attrs) {
            var ctrl = this;

            ctrl.ngOnValue = ctrl.ngOnValue || true;
            ctrl.ngOffValue = ctrl.ngOffValue || false;

            ctrl.init = function () {
                $scope.$watch(
                    function(scope){
                        return (ctrl.ngModel);
                    },
                    function (newVal, oldVal) {
                        if (newVal !== oldVal){
                            ctrl.toggle(newVal);

                            if (ctrl.ngToggle)
                                ctrl.ngToggle(ctrl.ngToggleParameter);
                        }
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