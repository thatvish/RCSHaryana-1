(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function indexCtrl($scope, $modal, apiService, notificationService) {

        $scope.loadingLeads = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Leads = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            $scope.loadingLeads = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 3,
                    filter: $scope.filterLeads
                }
            };
            var myurl = apiURL + "api/lead/list/";
            apiService.get(myurl, config,
                leadsLoadCompleted,
                leadsLoadFailed);
        }

        function leadsLoadCompleted(result) {
            $scope.Leads = result.data.Result.Items;

            $scope.page = result.data.Result.Page;
            $scope.pagesCount = result.data.Result.TotalPages;
            $scope.totalCount = result.data.Result.TotalCount;
            $scope.loadingLeads = false;

            if ($scope.filterLeads && $scope.filterLeads.length) {
                notificationService.displayInfo(result.data.Result.Items.length + ' Leads found');
            }
        }

        function leadsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterLeads = '';
            search();
        }
        $scope.search();
    }

})(angular.module('ace'));