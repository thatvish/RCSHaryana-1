(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$rootScope', '$http'];

    function indexCtrl($scope, apiService, notificationService, $rootScope, $http) {

        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.smsList = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            var config = {
                params: {
                    page: page,
                    pageSize: 20,
                    filter: $scope.filterSms
                }
            };
            var myurl = apiURL + "api/sms/search/";
            apiService.get(myurl, config,
                smsLoadCompleted,
                smsLoadFailed);
        }

        function smsLoadCompleted(result) {
            $scope.smsList = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingSms = false;

            if ($scope.filterCustomers && $scope.filterCustomers.length) {
                notificationService.displayInfo(result.data.Items.length + ' Sms found');
            }
        }

        function smsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterSms = '';
            search();
        }

        // Load Sms
        $scope.search();
    }
})(angular.module('ace'));