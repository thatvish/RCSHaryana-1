(function (app) {
    'use strict';

    app.controller('addRegistrationCtrl', addRegistrationCtrl);

    addRegistrationCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService'];

    function addRegistrationCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService) {

        $scope.Registration = {};
        $scope.addRegistration = addRegistration;
        $scope.prepareFiles = prepareFiles;
        $scope.openDatePicker = openDatePicker;

        $scope.Registration.Attachments = [
            { "Item": "", "Grade": "", "Description": "", "Size": "", "Quantity": "", "Unit": "", "Rate": "", "Amount": "" },
            { "Item": "", "Grade": "", "Description": "", "Size": "", "Quantity": "", "Unit": "", "Rate": "", "Amount": "" },
            { "Item": "", "Grade": "", "Description": "", "Size": "", "Quantity": "", "Unit": "", "Rate": "", "Amount": "" }];

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};

        var attcahedDoc = null;

        function loadRegistrationDetails() {
            $scope.loadingRegistration = true;
            apiService.get(apiURL + 'api/Data/invoiceDetails/' + getUrlParameter('id'), null,
            RegistrationLoadCompleted,
            RegistrationLoadFailed);
        }

        function RegistrationLoadCompleted(result) {
            $scope.Registration = result.data;
            $scope.Registration.InvoiceID = getUrlParameter('id');
            $scope.loadingRegistration = false;
        }

        function RegistrationLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function addRegistration() {
            AddRegistrationModel();
        }

        function AddRegistrationModel() {
            var myurl = apiURL + "api/Data/addRegistration/";
            apiService.post(myurl, $scope.Registration,
            addRegistrationSucceded,
            addRegistrationFailed);
        }

        function prepareFiles($files) {
            attcahedDoc = $files;
        }

        function addRegistrationSucceded(response) {
            notificationService.displaySuccess('Registration has been added');
           
            if (attcahedDoc) {
                fileUploadService.uploadReg(attcahedDoc, $scope.Registration.ID, redirectToEdit);
            }
        }

        function addRegistrationFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

        function openDatePicker($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker.opened = true;
        };

        loadRegistrationDetails();
    }

})(angular.module('ace'));