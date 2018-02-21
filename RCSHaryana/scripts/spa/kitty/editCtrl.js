(function (app) {
    'use strict';

    app.controller('editCtrl', editCtrl);

    editCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$sce'];

    function editCtrl($scope, apiService, notificationService, $sce) {

        $scope.kitty = {};
        $scope.loadKitty = loadKitty;

        $scope.updateKitty = function() {
            var myurl = apiURL + "api/kitty/update";
            apiService.post(myurl, $scope.kitty,
                updateKittySucceded,
                updateKittyFailed);
        }

        function updateKittySucceded(response) {
            notificationService.displaySuccess('Kitty has been updated');
        }

        function updateKittyFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

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

        // Load Kitty details
        loadKitty();
    }
})(angular.module('ace'));