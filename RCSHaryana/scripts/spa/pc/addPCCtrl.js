(function (app) {
    'use strict';

    app.controller('addPCCtrl', addPCCtrl);

    addPCCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService', '$modal'];

    function addPCCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService, $modal) {

        $scope.regis = [];
        $scope.pc = {};
        $scope.pc.AssetRegistrationID = 1;

        $scope.pc.PCItems = new Array(3);
        $scope.addPC = addPC;
        
        $scope.prepareFiles = prepareFiles;
        $scope.openDatePicker = openDatePicker;
        $scope.openDatePicker2 = openDatePicker2;
        $scope.openDatePicker3 = openDatePicker3;

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};
        $scope.datepicker2 = {};
        $scope.datepicker3 = {};

        var attcahedDoc = [];

        function addPC() {
            addPCModel();
        }
        
        function addPCModel() {
            apiService.post('/api/data/addPC', $scope.pc,
            addPCSucceded,
            addPCFailed);
        }

        function prepareFiles($files) {
            attcahedDoc.push($files);
        }

        function addPCSucceded(response) {
            notificationService.displaySuccess($scope.pc.ID + ' has been added');
            $scope.movie = response.data;

            alert(attcahedDoc.length);
            if (attcahedDoc) {
                fileUploadService.uploadPC(attcahedDoc, $scope.pc.ID, redirectToEdit);
            }
            else
                redirectToEdit();
        }

        function addPCFailed(response) {
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

        function openDatePicker3($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker3.opened = true;
        };

        function redirectToEdit() {
            $location.url('pc/edit/' + $scope.movie.ID);
        }

        function loadRegis() {
            apiService.get('/api/data/getRegistrations/', null,
            RegisLoadCompleted,
            RegisLoadFailed);
        }

        function RegisLoadCompleted(response) {
            $scope.regis = response.data;
        }

        function RegisLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        loadRegis();
    }

})(angular.module('ace'));