(function (app) {
    'use strict';

    app.controller('unitsCtrl', unitsCtrl);

    unitsCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function unitsCtrl($scope, $modal, apiService, notificationService) {

        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Units = [];

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
                    filter: $scope.filterUnits
                }
            };
            var myurl = apiURL + "api/Data/searchUnits/";
            apiService.get(myurl, config,
            UnitsLoadCompleted,
            UnitsLoadFailed);
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

        function UnitsLoadCompleted(result) {

            //alert(result.data.Units.length);
            $scope.Units = result.data;

            //$scope.page = result.data.Page;
            //$scope.pagesCount = result.data.TotalPages;
            //$scope.totalCount = result.data.TotalCount;

            if ($scope.filterUnits && $scope.filterUnits.length) {
                notificationService.displayInfo(result.data.Units.length + ' Units found');
            }

        }

        function UnitsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterUnits = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));