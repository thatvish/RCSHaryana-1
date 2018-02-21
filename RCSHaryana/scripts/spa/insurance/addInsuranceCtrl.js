(function (app) {
    'use strict';

    app.controller('addInsuranceCtrl', addInsuranceCtrl);

    addInsuranceCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService', '$modal'];

    function addInsuranceCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService, $modal) {

        $scope.regis = [];
        $scope.insurance = {};
        $scope.insurance.AssetRegistrationID = 1;

        $scope.insurance.InsuranceItems = new Array(3);
        $scope.addInsurance = addInsurance;
        $scope.AddMore = AddMore;
        $scope.RemoveItem = RemoveItem;
        $scope.prepareFiles = prepareFiles;
        $scope.openDatePicker = openDatePicker;
        $scope.openDatePicker2 = openDatePicker2;

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};
        $scope.datepicker2 = {};

        var attcahedDoc = [];

        function addInsurance() {
            addInsuranceModel();
            //$scope.insurance.ID = 1;
            //if (attcahedDoc) {
            //    fileUploadService.uploadImage(attcahedDoc, $scope.insurance.ID, redirectToEdit);
            //}
        }

        function AddMore() {
            $scope.insurance.InsuranceItems.push({ "Item": "", "Grade": "", "Description": "", "Size": "", "Quantity": "", "Unit": "", "Rate": "", "Amount": "" });
        }

        function RemoveItem(idx) {
            $scope.insurance.InsuranceItems.splice(idx, 1);
        }

        function addInsuranceModel() {
            apiService.post('/api/data/addInsurance', $scope.insurance,
            addInsuranceSucceded,
            addInsuranceFailed);
        }

        function prepareFiles($files) {
            //attcahedDoc = $files;
            attcahedDoc.push($files);
        }

        function addInsuranceSucceded(response) {
            notificationService.displaySuccess($scope.insurance.ID + ' has been added');
            $scope.movie = response.data;

            alert(attcahedDoc.length);
            if (attcahedDoc) {
                fileUploadService.uploadInsurance(attcahedDoc, $scope.insurance.ID, redirectToEdit);
            }
            else
                redirectToEdit();
        }

        function addInsuranceFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

        function openDatePicker($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker.opened = true;
        };

        function openDatePicker2($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker2.opened = true;
        };

        function redirectToEdit() {
            $location.url('insurance/edit/' + $scope.movie.ID);
        }

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

        loadregis();
    }

})(angular.module('ace'));