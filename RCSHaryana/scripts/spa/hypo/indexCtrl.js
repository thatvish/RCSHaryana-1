(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function indexCtrl($scope, $modal, apiService, notificationService) {

        $scope.loadingHypos = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Hypos = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            $scope.loadingHypos = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 3,
                    filter: $scope.filterHypos
                }
            };
            var myurl = apiURL + "api/Data/searchHypo/";
            apiService.get(myurl, config,
            HyposLoadCompleted,
            HyposLoadFailed);
        }

        function HyposLoadCompleted(result) {
            $scope.Hypos = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingHypos = false;

            if ($scope.filterHypos && $scope.filterHypos.length) {
                notificationService.displayInfo(result.data.Items.length + ' Hypothecations found');
            }
        }

        function HyposLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterHypos = '';
            search();
        }
        $scope.search();
    }

})(angular.module('ace'));