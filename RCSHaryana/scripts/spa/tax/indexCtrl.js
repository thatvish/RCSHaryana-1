(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function indexCtrl($scope, $modal, apiService, notificationService) {

        $scope.loadingTaxes = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Taxes = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            $scope.loadingTaxes = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 3,
                    filter: $scope.filterTaxes
                }
            };
            var myurl = apiURL + "api/Data/searchTax/";
            apiService.get(myurl, config,
            TaxesLoadCompleted,
            TaxesLoadFailed);
        }

        function TaxesLoadCompleted(result) {
            $scope.Taxes = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingTaxes = false;

            if ($scope.filterTaxes && $scope.filterTaxes.length) {
                notificationService.displayInfo(result.data.Items.length + ' Taxes found');
            }
        }

        function TaxesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterTaxes = '';
            search();
        }
        $scope.search();
    }

})(angular.module('ace'));