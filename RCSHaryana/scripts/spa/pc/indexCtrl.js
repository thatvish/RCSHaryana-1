(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function indexCtrl($scope, $modal, apiService, notificationService) {

        $scope.loadingPCs = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.PCs = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            $scope.loadingPCs = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 1,
                    filter: $scope.filterPCs
                }
            };
            var myurl = apiURL + "api/Data/searchPC/";
            apiService.get(myurl, config,
            PCsLoadCompleted,
            PCsLoadFailed);
        }

        function PCsLoadCompleted(result) {

            $scope.PCs = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingPCs = false;

            if ($scope.filterPCs && $scope.filterPCs.length) {
                notificationService.displayInfo(result.data.Items.length + ' PCs found');
            }
        }

        function PCsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterPCs = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));