(function (app) {
    'use strict';

    app.controller('receiveCtrl', receiveCtrl);

    receiveCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService', '$modal'];

    function receiveCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService, $modal) {

        $scope.payment = {};
        $scope.page = 0;
        $scope.pagesCount = 0;

        $scope.addPayment = addPayment;
        $scope.prepareFiles = prepareFiles;

        var attcahedDoc = [];

        function addPayment() {
            addPaymentModel();
            //$scope.payment.ID = 1;
            //if (attcahedDoc) {
            //    fileUploadService.uploadImage(attcahedDoc, $scope.payment.ID, redirectToEdit);
            //}
        }

        function addPaymentModel() {
            console.log($scope.payment);
            apiService.post(apiURL + '/api/payments/post', $scope.payment,
                addPaymentSucceded,
                addPaymentFailed);
        }

        function prepareFiles($files) {
            //attcahedDoc = $files;
            attcahedDoc.push($files);
        }

        function addPaymentSucceded(response) {
            console.log(response);
            notificationService.displaySuccess('Payment saved successfully');
        }

        function addPaymentFailed(response) {
            console.log(response.data);
            notificationService.displayError(response);
        }

        function redirectToEdit() {
            $location.url('payment/edit/' + $scope.payment.ID);
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

        loadCustomers();
    }

})(angular.module('ace'));