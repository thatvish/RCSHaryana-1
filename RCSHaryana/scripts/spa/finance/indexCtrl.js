(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function indexCtrl($scope, $modal, apiService, notificationService) {

        $scope.counters = {};

        $scope.bindCounters = bindCounters;

        function bindCounters() {
            var myurl = apiURL + "api/Data/getCounters/";
            apiService.get(myurl, null,
            countersLoadCompleted,
            countersLoadFailed);
        }

        function countersLoadCompleted(result) {
            console.log(result);
            $scope.counters = result.data;
        }

        function countersLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        $scope.bindCounters();
    }

})(angular.module('ace'));