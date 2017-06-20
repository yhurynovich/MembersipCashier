(function () {
    "use strict";

    if (!htm.app) return;

    htm.app.Model = {};

    htm.app.Model.SeverityType = {
        Error: 0,
        Failure: 1,
        Note: 2,
        Success: 3,
        Warning: 4
    };

    htm.app.Model.NotificationSeverityTypeString = {
        0: "Error",
        1: "Failure",
        2: "Note",
        3: "Success",
        4: "Warning"
    };

    htm.app.Model.LoginType = {
        MainStation: 1,
        Guest: 2
    };

    htm.app.Model.LoginTypes = [
        { Type: 1, Description: "Main Station" },
        { Type: 2, Description: "Try as GUEST" }
    ];

    htm.app.value("Model", htm.app.Model);
})();