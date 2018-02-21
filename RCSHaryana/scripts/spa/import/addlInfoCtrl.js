(function (app) {
    'use strict';

    app.controller('addlInfoCtrl', addlInfoCtrl);

    addlInfoCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function addlInfoCtrl($scope, $modal, apiService, notificationService) {

        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.AI = [];

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
                    filter: $scope.filterAI
                }
            };
            var myurl = apiURL + "api/Data/searchAI/";
            apiService.get(myurl, config,
            AILoadCompleted,
            AILoadFailed);
        }

        function AILoadCompleted(result) {

            //alert(result.data.AI.length);
            $scope.AI = result.data;

            //$scope.page = result.data.Page;
            //$scope.pagesCount = result.data.TotalPages;
            //$scope.totalCount = result.data.TotalCount;

            if ($scope.filterAI && $scope.filterAI.length) {
                notificationService.displayInfo(result.data.AI.length + ' records found');
            }
        }

        function AILoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterAI = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));