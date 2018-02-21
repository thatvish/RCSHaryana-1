(function (app) {
    'use strict';

    app.controller('editCustCtrl', editCustCtrl);

    editCustCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService'];

    function editCustCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService) {

        $scope.customerID = getUrlParameter('id');
        $scope.cust = {};
        $scope.editCust = editCust;
        $scope.prepareFiles = prepareFiles;

        var attcahedDoc = null;

        // Customer
        $scope.loadCustomer = function() {
            apiService.get(apiURL + 'api/customers/details/' + $scope.customerID, null,
                loadCustomerCompleted,
                loadCustomerFailed);
        }

        function loadCustomerCompleted(response) {
            $scope.cust = response.data;
        }

        function loadCustomerFailed(response) {
            notificationService.displayError(response.data);
        }

        function editCust() {
            editCustModel();
        }

        function editCustModel() {
            var myurl = apiURL + "api/customers/update/";
          
            apiService.post(myurl, $scope.cust,
                editCustSucceded,
                editCustFailed);
        }

        function prepareFiles($files) {
            attcahedDoc.push($files);
        }

        function editCustSucceded(response) {
            notificationService.displaySuccess('Customer has been updated');

            if (attcahedDoc) {
                fileUploadService.uploadCust(attcahedDoc, $scope.cust.ID, redirectToEdit);
            }
        }

        function editCustFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

        // Load customer details
        $scope.loadCustomer();
    }

})(angular.module('ace'));