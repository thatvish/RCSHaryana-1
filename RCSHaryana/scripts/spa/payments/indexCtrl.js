(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService', '$rootScope'];

    function indexCtrl($scope, $modal, apiService, notificationService, $rootScope) {

        $scope.loadingPayments = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.payments = [];
       
        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            $scope.loadingPayments = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filterPayments
                }
            };
            var myurl = apiURL + "api/payments/search/";
            apiService.get(myurl, config,
            paymentsLoadCompleted,
            paymentsLoadFailed);
        }

        function paymentsLoadCompleted(result) {

            //alert(result.data.Items.length);
            $scope.payments = result.data.Items;
            
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingPayments = false;

            if ($scope.filterPayments && $scope.filterPayments.length) {
                notificationService.displayInfo(result.data.Items.length + ' Payments found');
            }
        }

        function paymentsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterPayments = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));