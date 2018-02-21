(function (app) {
    'use strict';

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
        $scope.openSendMsgDialog = openSendMsgDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingUsers = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 1,
                    filter: $scope.filterUsers
                }
            };

            apiService.get(apiURL + 'api/case/search/', config,
            usersLoadCompleted,
            usersLoadFailed);
        }

        function openSendMsgDialog(customer) {
            $scope.EditedUser = customer;
            //alert(customer.ProceedingDate);
            $modal.open({
                templateUrl: '/scripts/spa/home/sendMessage.html',
                controller: 'sendMessageCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function usersLoadCompleted(result) {
            //alert(result.data.Items.length);
            $scope.Users = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingUsers = false;

            if ($scope.filterUsers && $scope.filterUsers.length) {
                notificationService.displayInfo(result.data.Items.length + ' users found');
            }

        }

        function usersLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterUsers = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));