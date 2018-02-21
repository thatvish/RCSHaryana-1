(function (app) {
    'use strict';

    app.controller('beneficiariesCtrl', beneficiariesCtrl);

    beneficiariesCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function beneficiariesCtrl($scope, $modal, apiService, notificationService) {

        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Beneficiaries = [];

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
                    filter: $scope.filterBeneficiaries
                }
            };
            var myurl = apiURL + "api/Data/searchBeneficiaries/";
            apiService.get(myurl, config,
            BeneficiariesLoadCompleted,
            BeneficiariesLoadFailed);
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

        function BeneficiariesLoadCompleted(result) {

            //alert(result.data.Beneficiaries.length);
            $scope.Beneficiaries = result.data;

            //$scope.page = result.data.Page;
            //$scope.pagesCount = result.data.TotalPages;
            //$scope.totalCount = result.data.TotalCount;

            if ($scope.filterBeneficiaries && $scope.filterBeneficiaries.length) {
                notificationService.displayInfo(result.data.Beneficiaries.length + ' Beneficiaries found');
            }

        }

        function BeneficiariesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterBeneficiaries = '';
            search();
        }

        $scope.search();
    }

})(angular.module('ace'));