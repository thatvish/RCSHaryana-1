(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function indexCtrl($scope, $modal, apiService, notificationService) {

        $scope.loadingPOss = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.POs = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.deleteRecord = deleteRecord;

        function search(page) {
            page = page || 0;

            $scope.loadingPOs = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filterPOs
                }
            };
            var myurl = apiURL + "api/Data/searchPO/";
            apiService.get(myurl, config,
            POsLoadCompleted,
            POsLoadFailed);
        }

        // Delete PO
        function deleteRecord(index) {
            console.log(index);
            var res = confirm("Are you sure you want to delete this record?");
            if (res == true) {
                apiService.post(apiURL + 'api/data/deleteOrder?id=' + index,
                  deleteQSucceeded,
                  deleteQFailed);
            }
        }

        function deleteQSucceeded(result) {
            console.log(result);
            $scope.search();
        }

        function deleteQFailed(response) {
            notificationService.displayInfo(response.data);
            $scope.search();
        }

        function POsLoadCompleted(result) {

            //alert(result.data.Items.length);
            $scope.POs = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingPOs = false;

            if ($scope.filterPOs && $scope.filterPOs.length) {
                notificationService.displayInfo(result.data.Items.length + ' POs found');
            }
        }

        function POsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterPOs = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));