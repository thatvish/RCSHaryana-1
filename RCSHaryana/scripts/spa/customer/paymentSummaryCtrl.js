(function (app) {
    'use strict';

    app.controller('paymentSummaryCtrl', paymentSummaryCtrl);

    paymentSummaryCtrl.$inject = ['$scope', '$modalInstance', '$timeout', 'apiService', 'notificationService', '$sce'];

    function paymentSummaryCtrl($scope, $modalInstance, $timeout, apiService, notificationService, $sce) {

        $scope.msg = "";
        $scope.loadSummary = loadSummary;
        $scope.adjustInstalment = adjustInstalment;
        $scope.cancelConsigner = cancelConsigner;
       
        // Load Summary
        function loadSummary() {

            var config = {
                params: {
                    id: $scope.customerID
                }
            };
            var myurl = apiURL + "api/customers/balance/";
            apiService.get(myurl, config,
            summaryLoadCompleted,
            summaryLoadFailed);
        }

        function summaryLoadCompleted(result) {
            $scope.summary = result.data;
            if ($scope.summary.ClosingBalance <= 0)
            {
                $scope.msg = "Negative closing balance. No amount can be adjusted. Need to add payment for this customer.";
            }
        }

        function summaryLoadFailed() {
            notificationService.displayError(response.data);
        }

        // Adjust Instalment
        function adjustInstalment() {
            alert('hi');
            var dataToPost = {
                CustomerID: $scope.customerID,
                ProductID: $scope.theProductID,
                InstDate: $scope.installmentDate,
                Amount: $scope.installmentAmnt
            };

            console.log(dataToPost);
            var myurl = apiURL + "api/customers/adjust-payment/";
            apiService.post(myurl, dataToPost,
                aiSucceded,
                aiFailed);

            $modalInstance.dismiss();
            window.location.href = "/customers/show?id=" + $scope.customerID;
        }

        function aiSucceded(response) {
            notificationService.displaySuccess('Amount has been adjusted');
        }

        function aiFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

        function cancelConsigner() {
            $modalInstance.dismiss();
        }

        loadSummary();
    }

})(angular.module('ace'));