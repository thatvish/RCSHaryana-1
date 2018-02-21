(function (app) {
    'use strict';

    app.controller('dueCtrl', dueCtrl);

    dueCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$rootScope'];

    function dueCtrl($scope, apiService, notificationService, $rootScope) {

        $scope.duePayments = [];
        $scope.loadDuePayments = loadDuePayments;

        function loadDuePayments() {

            var myurl = apiURL + "api/payments/payments-due/";
            apiService.get(myurl, null,
                duePaymentsLoadCompleted,
                duePaymentsLoadFailed);
        }

        function duePaymentsLoadCompleted(result) {
            $scope.duePayments = result.data;
        }

        function duePaymentsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        $scope.loadDuePayments();
    }

})(angular.module('ace'));