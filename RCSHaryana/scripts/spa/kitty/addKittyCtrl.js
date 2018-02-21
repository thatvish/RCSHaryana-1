(function (app) {
    'use strict';

    app.controller('addKittyCtrl', addKittyCtrl);

    addKittyCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService'];

    function addKittyCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService) {

        $scope.kitty = {};
        $scope.addKitty = addKitty;
       
        var attcahedDoc = null;

        function addKitty() {
            AddKittyModel();
        }

        function AddKittyModel() {
            var myurl = apiURL + "api/kitty/post";
            apiService.post(myurl, $scope.kitty,
                addKittySucceded,
                addKittyFailed);
        }

        function prepareFiles($files) {
            attcahedDoc.push($files);
        }

        function addKittySucceded(response) {
            notificationService.displaySuccess('Kitty has been added');

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