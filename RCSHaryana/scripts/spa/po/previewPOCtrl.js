(function (app) {
    'use strict';

    app.controller('previewPOCtrl', previewCtrl);

    previewCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$sce'];

    function previewCtrl($scope, apiService, notificationService, $sce) {

        $scope.po = {};
        $scope.loadPO = loadPO;

        function loadPO() {
            apiService.get(apiURL + 'api/Data/poFull/' + getUrlParameter('id'), null,
            loadPOCompleted,
            loadPOFailed);
        }

        function loadPOCompleted(response) {
            console.log(response.data);
            $scope.po = response.data;
            $scope.Consignee = $sce.trustAsHtml(response.data.TermsConditionsConsignee);
            $scope.Consigner = $sce.trustAsHtml(response.data.TermsConditionsConsigner);
        }

        function loadPOFailed(response) {
            notificationService.displayError(response.data);
        }

        loadPO();
    }

})(angular.module('ace'));