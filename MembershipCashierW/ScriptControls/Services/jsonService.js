'use strict';

htm.app.service('$json', function () {
	var iso8601regex = /^(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d)(\.?\d*)(Z?)$/;

	function reviver(_, property) {
		var match;
		if (typeof (property) == 'string' && (match = property.match(iso8601regex))) {
			return new Date(match[1] + (match[3] ? match[3] : 'Z'));
		} else {
			return property;
		}
	}

	return {
		parse: function (string) {
			try {
				return JSON.parse(string, reviver);
			}
			catch (e) {
				return string;
			}
		},
		stringify: function (value) {
			return JSON.stringify(value, function (key, val) {
				if (key != '$$hashKey') return val;
			});
		}
	};
});