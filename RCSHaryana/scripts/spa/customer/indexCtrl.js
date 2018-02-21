(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService', '$rootScope'];

    function indexCtrl($scope, $modal, apiService, notificationService, $rootScope) {

        $scope.loadingCustomers = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.customers = [];
       
        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            $scope.loadingCustomers = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 20,
                    filter: $scope.filterCustomers
                }
            };
            var myurl = apiURL + "api/customers/search/";
            apiService.get(myurl, config,
            customersLoadCompleted,
            customersLoadFailed);
        }

        function customersLoadCompleted(result) {

            //alert(result.data.Items.length);
            $scope.customers = result.data.Items;
            
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingCustomers = false;

            if ($scope.filterCustomers && $scope.filterCustomers.length) {
                notificationService.displayInfo(result.data.Items.length + ' Customers found');
            }
        }

        function customersLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterCustomers = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));