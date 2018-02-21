(function (app) {
    'use strict';

    app.controller('detailsCtrl', detailsCtrl);
    
    detailsCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$sce'];

    function detailsCtrl($scope, apiService, notificationService, $sce) {

        $scope.lead = {};
        $scope.loadLead = loadLead;
       

        function loadLead() {

            apiService.get(apiURL + 'api/lead/details/' + getUrlParameter('id'), null,
            loadLeadCompleted,
            loadLeadFailed);
        }

        function loadLeadCompleted(response) {
            console.log(response.data);
            $scope.lead = response.data.Result;
        }

        function loadLeadFailed(response) {
            notificationService.displayError(response.data.Errors[0]);
        }

        loadLead();
    }

})(angular.module('ace'));