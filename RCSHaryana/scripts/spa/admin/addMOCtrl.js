(function (app) {
    'use strict';

    app.controller('addMOCtrl', addMOCtrl);

    addMOCtrl.$inject = ['$scope', 'apiService', 'notificationService'];

    function addMOCtrl($scope, apiService, notificationService) {

        // Membershop option
        $scope.mo = {};
        $scope.addMO = addMO;

        // Package Type
        $scope.pt = {};
        $scope.addPT = addPT;

        function addMO() {
            var myurl = apiURL + "api/Admin/add-gym-membership-option/";
            apiService.post(myurl, $scope.mo,
                addMOSucceeded,
                addMOFailed);
        }

        function addMOSucceeded(response) {
            notificationService.displaySuccess('Gym Membership Option has been added');
        }

        function addMOFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

        function addPT() {
            var myurl = apiURL + "api/Admin/add-pt/";
            apiService.post(myurl, $scope.pt,
                addPTSucceeded,
                addPTFailed);
        }

        function addPTSucceeded(response) {
            notificationService.displaySuccess('Package has been added');
        }

        function addPTFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }
    }
})(angular.module('ace'));