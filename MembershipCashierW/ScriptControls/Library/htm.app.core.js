(function () {
    "use strict";

    if (!htm.app) return;

    //jquery .fn extensions
    jQuery.extend({
        max: function (array, callback) {
            array = array || [];

            var map = $.map(array, callback);

            var max = 0;

            return map.sort(function(a, b){
                return b > a;
            })[0];
        },
        sum: function (array, callback) {
            array = array || [];
            var sum = 0;

            $.each($.map(array, callback), function (i, x) {
                sum += x;
            });

            return sum;
        },
        parsePhoneNumber: function (number) {
            var regex = /((?=[^0-9a-zA-Z]*)([0-9a-zA-Z]+))+/g;
            var parts = [];

            var part = regex.exec(number);

            while (part) {
                parts.push(part[0]);
                part = regex.exec(number);
            }

            return parts.join("");
        }
    });

    Date.prototype.toUTC = function () {
        return new Date(this.getUTCFullYear(), this.getUTCMonth(), this.getUTCDate(), this.getUTCHours(), this.getUTCMinutes(), this.getUTCSeconds(), this.getUTCMilliseconds());
    };

    Date.prototype.isNextDay = function () {
        var tomorrow = new Date().toUTC();
        return tomorrow.getFullYear() == this.getFullYear() && tomorrow.getMonth() == this.getMonth()
            && (tomorrow.getDate() + 1) == this.getDate();
    };

    Date.prototype.Today = function () {
        var today = new Date().toUTC();
        return today.getFullYear() == this.getFullYear() &&
            today.getMonth() == this.getMonth() && today.getDate() == this.getDate();
    };

    Date.prototype.isSaturday = function () {
        return this.getDay() == 6;
    };

    String.prototype.toTitleCase = function () {
        return this.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
    };
})();