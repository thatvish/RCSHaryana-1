(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function indexCtrl($scope, $modal, apiService, notificationService) {

        $scope.loadingBGs = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.BGs = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            $scope.loadingBGs = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 3,
                    filter: $scope.filterBGs
                }
            };
            var myurl = apiURL + "api/Data/searchBG/";
            apiService.get(myurl, config,
            BGsLoadCompleted,
            BGsLoadFailed);
        }

        function BGsLoadCompleted(result) {
            $scope.BGs = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingBGs = false;

            if ($scope.filterBGs && $scope.filterBGs.length) {
                notificationService.displayInfo(result.data.Items.length + ' BGs found');
            }
        }

        function BGsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterBGs = '';
            search();
        }
        $scope.search();
    }

})(angular.module('ace'));