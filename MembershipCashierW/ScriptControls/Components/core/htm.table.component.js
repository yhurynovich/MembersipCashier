// HTM TABLE:
// A table component used for paginating, sorting and filtering.
// The table uses bootstrap Tables for a base and expands on it using features and functionality
// to allow a user to paginate, sort and filter the table.
(function () {
    "use strict";

    if (!htm.app) return;

    htm.app.component("ngTableComponent", {
        templateUrl: "/scriptcontrols/views/core/table/table.view.html",
        bindings: {
            ngTableData: "=",
            ngPageSizes: "=",
            ngColumnHeaders: "=",
            ngOnPageChanged: "=",
            ngSelectable: "<",
            ngSelectedRow: "="
        },
        controller: function ($scope) {
            var ctrl = this;

            ctrl.selectedIndex = -1;

            ctrl.currentPage = 1;
            ctrl.total = 0;
            ctrl.pages = 0;
            ctrl.size = this.ngPageSizes.first();
            ctrl.limitPerPage = 5;

            ctrl.$onInit = function () {
                $scope.$watchCollection(
                    function () {
                        return ctrl.ngTableData;
                    },
                    function (val) {
                        if (!val) return; //no object definition

                        ctrl.total = val.total;

                        var rows = [];

                        val.results.forEach(function (r) {
                            var cells = [];

                            for (var prop in r) {
                                var bind = null;

                                for (var i = 0; i < ctrl.ngColumnHeaders.length; i++) 
                                    if (ctrl.ngColumnHeaders[i].bind === prop){
                                        bind = ctrl.ngColumnHeaders[i];
                                        bind.order = i;
                                        break;
                                    }
                                
                                if (!bind) continue;

                                cells.push({
                                    bind: bind.bind,
                                    order: bind.order,
                                    value: r[prop],
                                    visible: bind.visible,
                                    format: bind.format,
                                    cellClass: bind.cellClass
                                });
                            }

                            cells.sort(function (a, b) {
                                return a.order - b.order;
                            });

                            rows.push(cells);
                        });

                        ctrl.results = rows;
                        ctrl.pages = [];

                        var totalPages = Math.ceil(ctrl.total / ctrl.size);

                        for (var i = 0; i < totalPages; i++)
                            ctrl.pages.push(i + 1);
                    });

                $scope.$watch(
                    function () {
                        return ctrl.size;
                    },
                    function ($new, $old) {
                        if (!$new || !$old || $new === $old) return;

                        ctrl.ngOnPageChanged(ctrl.currentPage, $new);
                    });
            };

            ctrl.selectRow = function (row, index) {
                ctrl.selectedIndex = index;
                ctrl.ngSelectedRow = {}; //todo: copy data...

                row.forEach(function (r) {
                    ctrl.ngSelectedRow[r.bind] = r.value;
                });
            };

            ctrl.nextPage = function () {
                ctrl.currentPage++;
                ctrl.ngOnPageChanged(ctrl.currentPage, ctrl.size);

                if (ctrl.ngSelectable) ctrl.ngSelectedRow = null;
            };

            ctrl.previousPage = function () {
                ctrl.currentPage--;
                ctrl.ngOnPageChanged(ctrl.currentPage, ctrl.size);

                if (ctrl.ngSelectable) ctrl.ngSelectedRow = null;
            };

            ctrl.firstPage = function () {
                ctrl.currentPage = 1;
                ctrl.ngOnPageChanged(ctrl.currentPage, ctrl.size);

                if (ctrl.ngSelectable) ctrl.ngSelectedRow = null;
            };

            ctrl.lastPage = function () {
                ctrl.currentPage = ctrl.pageCount();
                ctrl.ngOnPageChanged(ctrl.currentPage, ctrl.size);

                if (ctrl.ngSelectable) ctrl.ngSelectedRow = null;
            }

            ctrl.gotoPage = function (page) {
                ctrl.currentPage = page;
                ctrl.ngOnPageChanged(ctrl.currentPage, ctrl.size);

                if (ctrl.ngSelectable) ctrl.ngSelectedRow = null;
            };

            ctrl.pageCount = function () {
                return Math.ceil(parseInt(ctrl.total) / parseInt(ctrl.size));
            };

            ctrl.isFirstPage = function () {
                return ctrl.currentPage == 1;
            };

            ctrl.isLastPage = function () {
                return ctrl.currentPage == this.pageCount() - 1;
            };

            ctrl.lowerLimit = function () {
                var pageCountLimitPerPageDiff = ctrl.pageCount() - ctrl.limitPerPage;

                if (pageCountLimitPerPageDiff < 0)
                    return 1;

                if (ctrl.isFirstPage())
                    return ctrl.currentPage;

                var low = ctrl.currentPage - (Math.ceil(ctrl.limitPerPage / 2) - 1);

                if (low > pageCountLimitPerPageDiff + 1)
                    low = pageCountLimitPerPageDiff + 1;

                return Math.max(low, 1);
            };
        }
    });

    htm.app.filter("cellFormatter", function ($filter) {
        return function (value, filterName) {
            return $filter(filterName)(value);
        };
    });
})();