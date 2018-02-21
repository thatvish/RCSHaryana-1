(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function indexCtrl($scope, $modal, apiService, notificationService) {

        $scope.loadingInvoices = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Invoices = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            $scope.loadingInvoices = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filterInvoices
                }
            };
            var myurl = apiURL + "api/Data/searchInvoice/";
            apiService.get(myurl, config,
            InvoicesLoadCompleted,
            InvoicesLoadFailed);
        }

        function InvoicesLoadCompleted(result) {
            $scope.Invoices = result.data.Result.Items;

            $scope.page = result.data.Result.Page;
            $scope.pagesCount = result.data.Result.TotalPages;
            $scope.totalCount = result.data.Result.TotalCount;
            $scope.loadingInvoices = false;

            if ($scope.filterInvoices && $scope.filterInvoices.length) {
                notificationService.displayInfo(result.data.Result.Items.length + ' Invoices found');
            }
        }

        function InvoicesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterInvoices = '';
            search();
        }
        $scope.search();
    }

})(angular.module('ace'));