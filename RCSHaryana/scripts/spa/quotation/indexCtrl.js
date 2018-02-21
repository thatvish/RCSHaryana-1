(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService', '$rootScope'];

    function indexCtrl($scope, $modal, apiService, notificationService, $rootScope) {

        $scope.loadingQuotations = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Quotations = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openSendMsgDialog = openSendMsgDialog;

        console.log($rootScope.repository.loggedUser.role);

        $scope.lockToggle = false;
        $scope.lock = lock;

        //var roles = $rootScope.repository.loggedUser.role;
        //if (roles) {
        //    if (roles.indexOf('finance') > -1) {
        //        $scope.lockToggle = true;
        //    }
        //}

        function search(page) {
            page = page || 0;

            $scope.loadingQuotations = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filterQuotations
                }
            };
            var myurl = apiURL + "api/Data/searchQ/";
            apiService.get(myurl, config,
            QuotationsLoadCompleted,
            QuotationsLoadFailed);
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

        function QuotationsLoadCompleted(result) {

            //alert(result.data.Items.length);
            $scope.Quotations = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingQuotations = false;

            if ($scope.filterQuotations && $scope.filterQuotations.length) {
                notificationService.displayInfo(result.data.Items.length + ' Quotations found');
            }
        }

        function QuotationsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterQuotations = '';
            search();
        }

        function lock(q) {
            console.log(q);
            
            apiService.post(apiURL + 'api/data/lockUnlock', q,
            a,
            b);
           
        }
        function a(response) {
            notificationService.displaySuccess('success');
            $scope.search();
        }

        function b(response) {
            notificationService.displayError('error');
        }

        $scope.search();
    }

})(angular.module('ace'));