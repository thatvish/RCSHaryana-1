(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function indexCtrl($scope, $modal, apiService, notificationService) {

        $scope.loadingRegistrations = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Registrations = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openSendMsgDialog = openSendMsgDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingRegistrations = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filterRegistrations
                }
            };
            var myurl = apiURL + "api/Data/searchReg/";
            apiService.get(myurl, config,
            RegistrationsLoadCompleted,
            RegistrationsLoadFailed);
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

        function RegistrationsLoadCompleted(result) {

            $scope.Registrations = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingRegistrations = false;

            if ($scope.filterRegistrations && $scope.filterRegistrations.length) {
                notificationService.displayInfo(result.data.Items.length + ' Registrations found');
            }
        }

        function RegistrationsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterRegistrations = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));