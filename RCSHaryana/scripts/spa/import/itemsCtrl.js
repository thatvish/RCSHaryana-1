(function (app) {
    'use strict';

    app.controller('itemsCtrl', itemsCtrl);

    itemsCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function itemsCtrl($scope, $modal, apiService, notificationService) {

        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Items = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openSendMsgDialog = openSendMsgDialog;

        function search(page) {
            page = page || 0;

            var config = {
                params: {
                    page: page,
                    pageSize: 1,
                    filter: $scope.filterItems
                }
            };
            var myurl = apiURL + "api/Data/searchItems/";
            apiService.get(myurl, config,
            ItemsLoadCompleted,
            ItemsLoadFailed);
        }

        function openSendMsgDialog(customer) {
            $scope.EditedUser = customer;
            $modal.open({
                templateUrl: '/scripts/spa/home/sendMessage.html',
                controller: 'sendMessageCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function ItemsLoadCompleted(result) {

            //alert(result.data.Items.length);
            $scope.Items = result.data;

            //$scope.page = result.data.Page;
            //$scope.pagesCount = result.data.TotalPages;
            //$scope.totalCount = result.data.TotalCount;

            if ($scope.filterItems && $scope.filterItems.length) {
                notificationService.displayInfo(result.data.Items.length + ' Items found');
            }

        }

        function ItemsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterItems = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));