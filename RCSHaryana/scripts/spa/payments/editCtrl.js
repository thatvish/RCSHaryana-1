(function (app) {
    'use strict';

    app.controller('editCtrl', editCtrl);

    editCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService', '$modal'];

    function editCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService, $modal) {

        $scope.payment = {};
        $scope.page = 0;
        $scope.pagesCount = 0;

        $scope.editPayment = editPayment;

        function editPayment() {
            editPaymentModel();
        }

        function editPaymentModel() {
            console.log($scope.payment);
            apiService.post(apiURL + '/api/payments/update', $scope.payment,
                editPaymentSucceded,
                editPaymentFailed);
        }

        function editPaymentSucceded(response) {
            console.log(response);
            notificationService.displaySuccess('Payment updated successfully');
        }

        function editPaymentFailed(response) {
            console.log(response.data);
            notificationService.displayError(response);
        }

        function loadCustomers(page) {

            var config = {
                params: {
                    page: page,
                    pageSize: 10000,
                    filter: ''
                }
            };
            apiService.get(apiURL + '/api/customers/search/', config,
                custLoadCompleted,
                custLoadFailed);
        }

        function custLoadCompleted(response) {
            $scope.customers = response.data.Items;
        }

        function custLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        // Load Payments
        function loadPayments() {

            var config = {
                params: {
                    id: getUrlParameter('id')
                }
            };
            apiService.get(apiURL + '/api/payments/details/', config,
                paymentLoadCompleted,
                paymentLoadFailed);
        }

        function paymentLoadCompleted(response) {
            $scope.payment = response.data;
            document.getElementById("customer").text = response.data.CustomerID;
        }

        function paymentLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        loadCustomers();
        loadPayments();
    }

})(angular.module('ace'));