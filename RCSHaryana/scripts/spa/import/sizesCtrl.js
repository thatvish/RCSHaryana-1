(function (app) {
    'use strict';

    app.controller('sizesCtrl', sizesCtrl);

    sizesCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function sizesCtrl($scope, $modal, apiService, notificationService) {

        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Sizes = [];

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
                    filter: $scope.filterSizes
                }
            };
            var myurl = apiURL + "api/Data/searchSizes/";
            apiService.get(myurl, config,
            SizesLoadCompleted,
            SizesLoadFailed);
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

        function SizesLoadCompleted(result) {

            //alert(result.data.Sizes.length);
            $scope.Sizes = result.data;

            //$scope.page = result.data.Page;
            //$scope.pagesCount = result.data.TotalPages;
            //$scope.totalCount = result.data.TotalCount;

            if ($scope.filterSizes && $scope.filterSizes.length) {
                notificationService.displayInfo(result.data.Sizes.length + ' Sizes found');
            }

        }

        function SizesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterSizes = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));