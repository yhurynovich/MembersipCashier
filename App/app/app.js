(function(){
	"use strict";
	
	//Modules
	var app = angular.module("app", ["ngRoute"]);
	
	//Configuration
	app.config(function($locationProvider, $routeProvider, $httpProvider){
		$httpProvider.interceptors.push(function(){
			return {
				request: function(config){
					return config;
				},
				response: function(response){
					return response;
				}
			};
		});
		
		$routeProvider
			.when("/home",{
				templateUrl: "home.html",
				controller: "HomeController",
				controllerAs: "$ctrl"
			})
			.when("/sales",{
				templateUrl: "sales.html",
				controller: "SalesController",
				controllerAs: "$ctrl"
			})
			.when("/admin", {
				templateUrl: "admin.html",
				controller: "AdminController",
				controllerAs: "$ctrl"
			})
			.otherwise({
				redirectTo: "/home"
			})
		
		$locationProvider.html5Mode({enabled: true, requireBase: false});
	});
	
    //Factories/Values

	app.value("global",{
		api: "",
		version: "2017.01.18",
		debug: false
	});

	//Controllers
	app.controller("AdminController", function($scope, global){
		var ctrl = this;
		
		ctrl.users = [];
		ctrl.products = [];
		
		ctrl.initialize = function(){
			ctrl.getUsers(); //to make AJAX ready...
			ctrl.getProducts();
			
			$scope.$watch(	
				function(){
					return (ctrl.selectedUser);
				},
				function(val){
					if (!val) return;
					ctrl.originalUserValues = angular.copy(val);
				});
				
							
			$scope.$watch(	
				function(){
					return (ctrl.selectedProduct);
				},
				function(val){
					if (!val) return;
					ctrl.originalProductValues = angular.copy(val);
				});
		};
		
		ctrl.getProducts = function(){
			//Document below code out to simulate when no products exist...
			
			//adding products
			ctrl.products.push({
				Code: "000000000000",
				Name: "Bananas",
				Description: "Dole Banana's. Product of Columbia",
				Price: 0.59,
				LastModified: new Date(),
			});
		};
		
		ctrl.getUsers = function(){
			//Document below code out to simulate when no users exist...
			
			//adding users
			ctrl.users.push({
				FirstName: "John",
				LastName: "Do",
				Phone: "(000) 000-0000",
				Email: "john.do@foo.com",
				CustomerSince: new Date(2016,2,2)
			});
		};
		
		ctrl.cancelUserChanges = function(){
			if (ctrl.originalUserValues){
				ctrl.selectedUser.Email = ctrl.originalUserValues.Email;
				ctrl.selectedUser.FirstName = ctrl.originalUserValues.FirstName;
				ctrl.selectedUser.LastName = ctrl.originalUserValues.LastName;
				ctrl.selectedUser.Phone = ctrl.originalUserValues.Phone;
			}
			
			ctrl.userEdit = false;
		};
		
		ctrl.saveUserChanges = function(){
			if (ctrl.selectedUser && ctrl.selectedUser.$new){
				//adding users
				ctrl.selectedUser.CustomerSince = new Date();
				ctrl.users.push(ctrl.selectedUser);
			}
			
			ctrl.userEdit = false;
		};
		
		ctrl.cancelProductChanges = function(){
			if (ctrl.originalProductValues){
				ctrl.selectedProduct.Code = ctrl.originalProductValues.Code;
				ctrl.selectedProduct.Name = ctrl.originalProductValues.Name;
				ctrl.selectedProduct.Description = ctrl.originalProductValues.Description;
				ctrl.selectedProduct.Price = ctrl.originalProductValues.Price;
			}
			
			ctrl.userEdit = false;
		};
		
		ctrl.saveProductChanges = function(){
			if (ctrl.selectedProduct && ctrl.selectedProduct.$new){
				//adding users
				ctrl.selectedProduct.LastModified = new Date();
				ctrl.products.push(ctrl.selectedProduct);
			}
			
			ctrl.productEdit = false;
		};
		
		ctrl.initialize();
	});
	
	app.controller("HomeController", function(){
		
	});
	
	app.controller("SalesController", function($scope){
	    var ctrl = this;

		ctrl.customers = [];
		ctrl.salesIndex = 1;
		
		ctrl.products = [
            { Code: "00001", Description: "Bananas", Price: 0.59 },
            { Code: "00002", Description: "Apples", Price: 1.99 },
		];

		ctrl.initialize = function(){
			ctrl.getCustomers();
		};
		
		ctrl.getCustomers = function(){
			//TODO: ajax to get data per customer...
			
			ctrl.customers.push({
				Email: "john.do@foo.com",
				FirstName: "John",
				LastName: "Do",
				Phone: "(000) 000-0000",
				CustomerSince: new Date(2016,2,1),
				Balance: 12.99
			});
		};
		
		ctrl.createNewSale = function(){
		    if (!ctrl.selectedCustomer.Sales) ctrl.selectedCustomer.Sales = [];

			ctrl.currentSale = {
			    Items: [],
			    Number: ctrl.salesIndex++,
			    Date: new Date(),
			    Total: function () {
			        var total = 0;

			        for (var i = 0; i < this.Items.length; i++)
			            if (this.Items[i].Product)
			                total += (this.Items[i].Quantity * this.Items[i].Product.Price);

			        return total;
			    }
			};

			ctrl.currentSaleIndex = ctrl.selectedCustomer.Sales.push(ctrl.currentSale);
		};
		
		ctrl.cancelNewSale = function(){
			ctrl.currentSale = null;
			ctrl.currentSaleIndex = 0;
			ctrl.selectedCustomer.Sales.splice(ctrl.current - 1, 1);
		};
		
		ctrl.saveNewSale = function(){
			ctrl.currentSale = null;
			ctrl.currentSaleIndex = 0;
		};
		
		ctrl.addSaleItems = function () {
		    var item = {
		        Quantity: 0
		    };

		    $scope.$watchCollection(
                function () {
                    return item;
                },
                function (val) {
                    if (!val) return;

                    var product = $.grep(ctrl.products, function (p) {
                        return p.Code == val.Code;
                    })[0];

                    val.Product = product;
                });

		    ctrl.currentSale.Items.push(item);
		};
		
		ctrl.initialize();
	});
})();