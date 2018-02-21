(function (app) {
    'use strict';

    app.controller('templatesCtrl', templatesCtrl);

    templatesCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$modal'];

    function templatesCtrl($scope, apiService, notificationService, $modal) {

        $scope.totalCount = 0;
        $scope.SmsTemplate = {};

        $scope.deleteTemp = function (id) {
            var result = confirm('Are you sure you want to delete this template?');

            if (result === true) {
                apiService.post(apiURL + 'api/sms/delete-template?id=' + id, null,
                    deleteTempCompleted,
                    deleteTempFailed);
            }
        }

        function deleteTempCompleted(response) {
            notificationService.displaySuccess('Template deleted');
            $scope.loadTemplates();
        }

        function deleteTempFailed(response) {
            notificationService.displayError(response);
        }


        // Selected Template
        $scope.selectTemplate = function (template) {
            $scope.selectedTemplate = template;

            if ($scope.selectedTemplate != null) {
                if ($scope.selectedTemplate.ID > 0) {
                    $scope.SmsTemplate = $scope.selectedTemplate;
                }
            }
        }

        // Load Templates
        $scope.loadTemplates = function () {
            apiService.get(apiURL + 'api/sms/templates/', null,
                tempLoadCompleted,
                tempLoadFailed);
        }

        function tempLoadCompleted(response) {
            $scope.templates = response.data;
            $scope.totalCount = response.data.length;
        }

        function tempLoadFailed(response) {
            notificationService.displayError(response.data);
            console.log(data);
        }

        // Add
        $scope.addTemplate = function () {
            if ($scope.selectedTemplate == null) {
                apiService.post(apiURL + 'api/sms/add-template', $scope.SmsTemplate,
                    addTSucceded,
                    addTFailed);
            }
            else {
                apiService.post(apiURL + 'api/sms/edit-template', $scope.SmsTemplate,
                    updateTSucceded,
                    updateTFailed);
            }

        }

        function addTSucceded(response) {
            notificationService.displaySuccess('Template sucessfully added');
            document.getElementById('addTempModal').style.display = 'none';
            $scope.loadTemplates();
        }

        function addTFailed(response) {
            notificationService.displayError(response.statusText);
        }

        function updateTSucceded(response) {
            notificationService.displaySuccess('Template sucessfully updated');
            document.getElementById('addTempModal').style.display = 'none';
            $scope.loadTemplates();
        }

        function updateTFailed(response) {
            notificationService.displayError(response.statusText);
        }

        $scope.loadTemplates();
    }

})(angular.module('ace'));