(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function indexCtrl($scope, $modal, apiService, notificationService) {

        $scope.loadingInsurances = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Insurances = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            $scope.loadingInsurances = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 1,
                    filter: $scope.filterInsurances
                }
            };
            var myurl = apiURL + "api/Data/searchInsurance/";
            apiService.get(myurl, config,
            InsurancesLoadCompleted,
            InsurancesLoadFailed);
        }

        function InsurancesLoadCompleted(result) {

            $scope.Insurances = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingInsurances = false;

            if ($scope.filterInsurances && $scope.filterInsurances.length) {
                notificationService.displayInfo(result.data.Items.length + ' Insurances found');
            }
        }

        function InsurancesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterInsurances = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));