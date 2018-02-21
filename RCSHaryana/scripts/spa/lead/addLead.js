(function (app) {
    'use strict';

    app.controller('addLeadCtrl', addLeadCtrl);

    addLeadCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService', '$modal'];

    function addLeadCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService, $modal) {

        $scope.lead = {};

        $scope.addLead = addLead;
        $scope.prepareFiles = prepareFiles;

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        var attcahedDoc = [];

        function addLead(isDraft) {
            addLeadModel(isDraft);
            //$scope.lead.ID = 1;
            //if (attcahedDoc) {
            //    fileUploadService.uploadImage(attcahedDoc, $scope.lead.ID, redirectToEdit);
            //}
        }

        function addLeadModel(isDraft) {
            $scope.lead.IsDraft = isDraft;
            $scope.lead.Salutation = document.getElementsByName("Salutation").text;

            console.log($scope.lead);
            apiService.post('/api/lead/add', $scope.lead,
                addLeadSucceded,
                addLeadFailed);
        }

        function prepareFiles($files) {
            //attcahedDoc = $files;
            attcahedDoc.push($files);
        }

        function addLeadSucceded(response) {
            console.log(response);
            notificationService.displaySuccess('Lead for '+response.data.Result.CompanyName + ' has been added');
        }

        function addLeadFailed(response) {
            console.log(response.data);
           
            notificationService.displayError(response.data.Errors[0]);
        }

        function redirectToEdit() {
            $location.url('lead/edit/' + $scope.lead.ID);
        }

        function loadRegis() {
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

        //loadRegis();
    }

})(angular.module('ace'));