(function (app) {
    'use strict';

    app.controller('inFavorOfCtrl', inFavorOfCtrl);

    inFavorOfCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function inFavorOfCtrl($scope, $modal, apiService, notificationService) {

        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.InFavorOf = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            var config = {
                params: {
                    page: page,
                    pageSize: 1,
                    filter: $scope.filterInFavorOf
                }
            };
            var myurl = apiURL + "api/Data/searchInFavorOf/";
            apiService.get(myurl, config,
            InFavorOfLoadCompleted,
            InFavorOfLoadFailed);
        }

        function InFavorOfLoadCompleted(result) {

            $scope.InFavorOf = result.data;

            //$scope.page = result.data.Page;
            //$scope.pagesCount = result.data.TotalPages;
            //$scope.totalCount = result.data.TotalCount;

            if ($scope.filterInFavorOf && $scope.filterInFavorOf.length) {
                notificationService.displayInfo(result.data.InFavorOf.length + ' records found');
            }

        }

        function InFavorOfLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterInFavorOf = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));