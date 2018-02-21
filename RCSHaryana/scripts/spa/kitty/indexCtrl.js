(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService', '$rootScope', '$http'];

    function indexCtrl($scope, $modal, apiService, notificationService, $rootScope, $http) {

        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.kittyList = [];

        $scope.search = search;
        $scope.deletekitty = deletekitty;
        $scope.clearSearch = clearSearch;
        $scope.selectedKitty = {};
        $scope.CustomerIDs = '';

        // Add Kitty Members (subscription)
        $scope.addSubsKitty = function () {
            console.log($scope.selectedKitty);
            console.log($scope.CustomerIDs);
            var dataToPost = { // SubscriptionMulti
                "ID": 0,
                "RequestDate": null,
                "ActualStartDate": $scope.selectedKitty.StartDate,
                "ActualEndDate": null,
                "MonthlyInstalment": $scope.selectedKitty.MonthlyInstalment,
                "CustomerIDs": $scope.CustomerIDs,
                "ProductID": $scope.selectedKitty.ProductID,
                "ProductType": 1,
                "CancelDate": null,
                "Remarks": null,
                "PackageTypeID": null,
                "TenureInMonths": $scope.selectedKitty.TenureInMonths
            };

            var myurl = apiURL + "api/subscription/multi-members-sub";
            apiService.post(myurl, dataToPost,
                addKittySubsSucceeded,
                addKittySubsFailed);
        }

        function addKittySubsSucceeded(response) {
            notificationService.displaySuccess('Subscription has been added');
            //$scope.search();
            window.location.href = '/kitty/members?id=' + $scope.selectedKitty.ID;
        }

        function addKittySubsFailed(response) {
            notificationService.displayError(response.statusText);
        }

        // Selected Kitty details
        $scope.doMembers = function (kitty) {
            $scope.selectedKitty = kitty;
           
        }

        // Bind Customers
        $scope.loadCustomers = function (page) {
            var config = {
                params: {
                    page: page,
                    pageSize: 10000,
                    filter: ''
                }
            };
            apiService.get(apiURL + '/api/customers/search/', config,
                custLoadCompleted,
                custLoadFailed);
        }

        function custLoadCompleted(response) {
            $scope.customers = response.data.Items;
        }

        function custLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function loadKittyCompleted(response) {
            console.log(response.data);
            $scope.kitty = response.data;
        }

        function loadKittyFailed(response) {
            notificationService.displayError(response.data);
        }

        function deletekitty(kittyModel) {
            apiService.get(apiURL + 'api/kitty/delete/' + kitty.id, null,
                loadKittyCompleted,
                loadKittyFailed);
        }

        function search(page) {
            page = page || 0;

            var config = {
                params: {
                    page: page,
                    pageSize: 20,
                    filter: $scope.filterKitty
                }
            };
            var myurl = apiURL + "api/kitty/search/";
            apiService.get(myurl, config,
                kittyLoadCompleted,
                kittyLoadFailed);
        }

        function kittyLoadCompleted(result) {
            //alert(result.data.Items.length);
            $scope.kittyList = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingKitty = false;

            if ($scope.filterCustomers && $scope.filterCustomers.length) {
                notificationService.displayInfo(result.data.Items.length + ' Kitty Not found');
            }
        }

        function kittyLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterKitty = '';
            search();
        }

        // Load Kitty
        $scope.search();
        // Load Customers
        $scope.loadCustomers();
    }
})(angular.module('ace'));