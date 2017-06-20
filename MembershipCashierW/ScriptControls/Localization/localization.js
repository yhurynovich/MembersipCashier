(function ($http) {
    "use strict";

    if (!htm.app) return;

    htm.app.factory("LocalizationFactory", function () {
        return {
            get: function (txt, lang) {
                if (!txt) return;

                var result = htm.app.resource.first(function (r) { return r.en === txt });
                return result && result[lang] ? result[lang] : txt;
            }
        };
    });
})();