(function () {
	"use strict";

	if (!htm.app) return;

	htm.app.SystemEventsController = function ($global, Model, SystemEventsFactory) {
	    var ctrl = this;
	    ctrl.model = Model;

	    ctrl.systemEvents = [];

	    ctrl.$onInit = function () {
	        $global.setAutoWorker();

	        //SystemEventsFactory.getSystemEvents()
            //    .then(
            //        function (response) {
	        //            ctrl.systemEvents = response.data;
            //        },
            //        function (response) {
            //            //TODO: error retreiving events.
            //        });
		};
	};

	htm.app.component("ngSystemEventsComponent", {
		templateUrl: "/ScriptControls/Views/admin/system.events.view.html",
		controller: htm.app.SystemEventsController
	});
})();