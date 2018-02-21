(function (app) {
    'use strict';
Hypo
    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function indexCtrl($scope, $modal, apiService, notificationService) {

        $scope.loadingUsers = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Users = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            $scope.loadingUsers = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 3,
                    filter: $scope.filterUsers
                }
            };
            var myurl = apiURL + "api/Data/searchUsers/";
            apiService.get(myurl, config,
            UsersLoadCompleted,
            UsersLoadFailed);
        }

        function UsersLoadCompleted(result) {
            $scope.Users = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingUsers = false;

            if ($scope.filterUsers && $scope.filterUsers.length) {
                notificationService.displayInfo(result.data.Items.length + ' Userthecations found');
            }
        }

        function UsersLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterUsers = '';
            search();
        }
        $scope.search();
    }

})(angular.module('ace'));