(function (app) {
    'use strict';

    app.controller('addTaxCtrl', addTaxCtrl);

    addTaxCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService', '$modal'];

    function addTaxCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService, $modal) {
       
        $scope.taxTypes = [];
        $scope.regis = [];
        $scope.tax = {};
       
        var xx = parseInt(getUrlParameter('id'));
        console.log(xx);
        $scope.tax.AssetRegistrationID = xx;//getUrlParameter('id');
        $scope.tax.TaxItems = new Array(3);
        $scope.tax.TaxItems = [
             { "DepartmentName": "", "TaxType": 1, "TaxFrom": "", "TaxTo": "", "Amount": "", "Fine": "", "TotalAmount": "", "IssuingOfficer": "", "IssuingDate": "" },
             { "DepartmentName": "", "TaxType": 1, "TaxFrom": "", "TaxTo": "", "Amount": "", "Fine": "", "TotalAmount": "", "IssuingOfficer": "", "IssuingDate": "" },
             { "DepartmentName": "", "TaxType": 1, "TaxFrom": "", "TaxTo": "", "Amount": "", "Fine": "", "TotalAmount": "", "IssuingOfficer": "", "IssuingDate": "" }];

        $scope.addTax = addTax;
        $scope.AddMore = AddMore;
        $scope.RemoveItem = RemoveItem;
        $scope.prepareFiles = prepareFiles;
        $scope.openDatePicker = openDatePicker;
        
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};

        var attcahedDoc = [];

        function addTax() {
            addTaxModel();
            //$scope.tax.ID = 1;
            //if (attcahedDoc) {
            //    fileUploadService.uploadImage(attcahedDoc, $scope.tax.ID, redirectToEdit);
            //}
        }

        function AddMore() {
            $scope.tax.TaxItems.push({ "Item": "", "Grade": "", "Description": "", "Size": "", "Quantity": "", "Unit": "", "Rate": "", "Amount": "" });
        }

        function RemoveItem(idx) {
            $scope.tax.TaxItems.splice(idx, 1);
        }

        function addTaxModel() {
            apiService.post(apiURL + 'api/data/addTax', $scope.tax,
            addTaxSucceded,
            addTaxFailed);
        }

        function prepareFiles($files) {
            attcahedDoc.push($files);
        }

        function addTaxSucceded(response) {
            notificationService.displaySuccess('Tax successfully added');
            $scope.movie = response.data;

            alert(attcahedDoc.length);
            if (attcahedDoc) {
                fileUploadService.uploadTax(attcahedDoc, $scope.tax.ID, redirectToEdit);
            }
            else
                redirectToEdit();
        }

        function addTaxFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

        function openDatePicker($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker.opened = true;
        };

        function redirectToEdit() {
        }

        // Load Registrations
        function loadregis() {
            apiService.get('/api/data/getRegistrations/', null,
            regisLoadCompleted,
            regisLoadFailed);
        }

        function regisLoadCompleted(response) {
            $scope.regis = response.data;
        }

        function regisLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        // Load TaxTypes
        function loadTT() {
            apiService.get('/api/data/searchTaxMaster/', null,
            loadTTCompleted,
            loadTTFailed);
        }

        function loadTTCompleted(response) {
            $scope.taxTypes = response.data;
        }

        function loadTTFailed(response) {
            notificationService.displayError(response.data);
        }

        loadregis();
        loadTT();
    }

})(angular.module('ace'));