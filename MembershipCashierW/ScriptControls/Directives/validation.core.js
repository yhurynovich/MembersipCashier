(function () {
    "use strict";

    if (!htm.app) return;        

    htm.app.directive("ngInteger", function () {
        return {
            require: "ngModel",
            link: function (scope, element, attrs, ctrl) {
                ctrl.$validators.integer = function (viewValue) {
                    var regex = /^\-?\d*$/gm;

                    if (!viewValue) return true; //return no validation required if it is an empty value
                    
                    return regex.test(viewValue.toString());
                };
            }
        }
    });

    htm.app.directive("ngLimit", function () {
        return {
            require: "ngModel",
            link: function (scope, element, attrs, ctrl) {
                ctrl.$validators.limit = function (viewValue) {
                    var limitNumbers = parseInt(attrs.ngLimit);

                    if (!viewValue || !limitNumbers) return true; //no validation required if either no limit or no value.

                    return viewValue.toString().length <= limitNumbers;
                };
            }
        }
    });

    htm.app.directive("ngNotEquals", function () {
        return {
            require: "ngModel",
            link: function (scope, element, attrs, ctrl) {
                ctrl.$validators.notEquals = function (viewValue) {
                    var compare = attrs.ngNotEquals;

                    if (!viewValue || !compare) return true;

                    return viewValue !== compare;
                };
            }
        };
    });

    htm.app.directive("ngPhone", function () {
        return {
            require: "ngModel",
            link: function (scope, element, attrs, ctrl) {
                ctrl.$validators.phone = function (viewValue) {
                    if (!viewValue) return true;

                    var regex = /^\d{0,15}$/;

                    var digits = $.parsePhoneNumber(viewValue);

                    return regex.test(digits);
                }
            }
        }
    });

    htm.app.directive("ngCommodity", function () {
        return {
            require: "ngModel",
            scope:{
                ngCommodity: "="
            },
            link: function (scope, element, attrs, ctrl) {
                var list = ["NC Parts","AC Parts","Accessories","Advertising Material","Aircraft Parts","Aircraft Spare Parts","Aircraft Spares","Antibodies","Antibody","Apparel","Appliance","Appliances","Art","As Per Attached INV","Assorted Merchandise","Auto Part","Auto Parts","Automotive Parts","Autoparts","Bag","Battery","Bearing","Belts","Box","BrakeParts","Brake","Business Correspondence","Cable","Cap","Caps","Carton","CD","CDs","Cell Line","Cells","Chemical","Chemicals","Chip","Christmas Gifts","Cl Attached","Civil Aircraft Parts","Civil Aircraft Spares","Clothes / Textiles","Clothes","Clothing and Accessories","Clothing","Comat","Commercial Invoice","Components","Computer Parts","Computer Peripherals","Connector","Cosmetic Products","Cosmetics","Culture","Dangerous Good","Dangerous Goods","Data Processing Part","Data Processing Parts","Defective Goods","DESCN","DESCRI","DG","DGs","Disk","Disks","Display","DNA","Doc","Document","Documentation","Documents","Drug","Drugs","Dry Ice","DVD","DVDs","Electrical Parts","Electronic Component","Electronic Components","Electronic Equipment","Electronic Good","Electronic Goods","Electronic Part","Electronic Parts","Electronic","Electronics","Equipment","Fabric Samples","Fabric","Fabrics","FAC","FAK","Flooring","Food Items","Food","Foodstuff","Foodstuffs","Freight Of All Kinds","General Cargo","Gift","Gifts","Goods","Hardware","Haz Mat","Haz Material","Haz Materials","Hazardous Chemical","Hazardous Chemicals","Hazardous Good","Hazardous Goods","Hazardous Material","Hazardou s Materials","HazMat","Household Goods","HS #","HS NON","HS#","I C","IC","ILLEDG","Illegible","Implants","Industrial Goods","Integrated Circuit","Integrated Circuits","Iron","Items","Jeans","Jewelry","Laboratory Reagents","Ladies Apparel","Leather Article","Leather Articles","Leather","Letter","Liquid","Luggage","Machine Part","Machine Parts","Machinery","Machines","Medical Equipment","Medical Parts","Medical Spare Parts","Medical Supplies","Medicaments","Medication","Medications","Medicine","Medicines","Meds","Mens Apparel","Metal Work","Miscellaneous Items","NAFTA","New Goods","No Cl","NO COM","NO DES","NON G","Non-Hazardous","NOT GI","Packaging Supplies","Pants","Paper","Paperwork","Part","Parts Of","Parts","PC Hardware","PCB","PCBA","Peripheral","Personal Effects","Personal Item","Personal Items","Pharmac euticals","PIB","PIBs","Pipe","Pipes","Plastic Good","Plastic Goods","Plastic Parts","Plastic","Polyurethane","Power Supply","Precious Metal","Printed Circuit Board","Printed Material","Printed Materials","Printed Matter","Printed Matters","Promo Item","Promo Items","Promo Material","Promo Materials","Promotional Item","Promotional Items","Promotional Material","Promotional Materials","Promotional","Receivers","Records","Report","Rod","Rods","Rubber Articles","Rubber","Said To Contain","Sample","Samples","Scrap","See Attached","SEE CO","SEE IN","See Invoice","Shirt","Software","Spare Parts for Machine","Spare Parts","Spares","Sportswear","STC","Steel","Surgical Instruments","Swatches","Tape","Tapes","Textile Samples","Textile","Textiles Samples","Textiles","Tile","Tiles","Tools","Toy","Training Material","Training Materials","Tubes","Unlist","Used Goods","Various Goods","Video Tape","Video Tapes","Video","Videotape","Videotapes","VISA MOR Table","Wafer","Waste","Wearing Apparel","Wire","Wires"];

                scope.$watch(
                    function () {
                        return (scope.ngCommodity)
                    },
                    function (val) {
                        if (val) {
                            ctrl.$validators.commodity = function (viewValue) {
                                if (!viewValue || ctrl.ngCommodity) return true;

                                return !list.any(function (l) { return l.toLowerCase().trim() == viewValue.toLowerCase().trim() });
                            };
                            ctrl.$validate();
                        }
                        else {
                            ctrl.$validators.commodity = function (viewValue) { return true; };
                            ctrl.$validate();
                        }
                    });
            }
        }
    });
})();