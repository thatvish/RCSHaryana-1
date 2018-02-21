(function (app) {
    'use strict';

    app.controller('detailsCtrl', detailsCtrl);
    
    detailsCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$sce'];

    function detailsCtrl($scope, apiService, notificationService, $sce) {

        $scope.kitty = {};
        $scope.loadKitty = loadKitty;

        

        function loadKitty() {
            console.log(getUrlParameter('id'));
            apiService.get(apiURL + 'api/kitty/details/' + getUrlParameter('id'), null,
            loadKittyCompleted,
            loadKittyFailed);
        }

        function loadKittyCompleted(response) {
            console.log(response.data);
            $scope.kitty = response.data;
        }

        function loadKittyFailed(response) {
            notificationService.displayError(response.data);
        }

        loadKitty();
    }

    function UpdateKittyCtrl($scope, apiService, notificationService) {
        $scope.kitty = {};
        $scope.addKitty = addKitty;

        var attcahedDoc = null;

        function addKitty() {
            AddKittyModel();
        }

        function AddKittyModel() {
            var myurl = apiURL + "api/kitty/Update";
            apiService.post(myurl, $scope.kitty,
                addKittySucceded,
                addKittyFailed);
        }

        function prepareFiles($files) {
            attcahedDoc.push($files);
        }

        function addKittySucceded(response) {
            notificationService.displaySuccess('Kitty has been updated');

            if (attcahedDoc) {
                fileUploadService.uploadKitty(attcahedDoc, $scope.kitty.ID, redirectToEdit);
            }
        }

        function addKittyFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }
    }

})(angular.module('ace'));