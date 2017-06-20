'use strict';

htm.app.directive('proximityWatch', [
	'$timeout', function ($timeout) {

		return function (scope, element, attrs) {

			scope.$on('$destroy', function () {
				document.removeEventListener('resize', handleFunction);
				document.removeEventListener('scroll', handleFunction);
			});

			var handleFunction = function () {
				scope.$apply(handle);
			};


			function handle() {
				$timeout(function () {
					var viewportHeight = window.innerHeight,
						scrollPosition = window.pageYOffset,
						bottomOfViewportPosition = scrollPosition + viewportHeight,
						elementPosition = element.prop('offsetTop');

					if (bottomOfViewportPosition > elementPosition) {
						scope.$eval(attrs.proximityAction);
						document.removeEventListener('resize', handleFunction);
						document.removeEventListener('scroll', handleFunction);
					}
				});
			}

			scope.$watch(attrs.proximityWatch, function () {
				handle();
				document.addEventListener('resize', handleFunction);
				document.addEventListener('scroll', handleFunction);
			}, true);
		};
	}]);