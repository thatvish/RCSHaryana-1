(function (app) {
    'use strict';

    app.controller('previewCtrl', previewCtrl);
    
    previewCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$sce'];

    function previewCtrl($scope, apiService, notificationService, $sce) {

        $scope.quotation = {};
        $scope.loadQuotation = loadQuotation;
       

        function loadQuotation() {

            apiService.get(apiURL + 'api/Data/quotationFull/' + getUrlParameter('id'), null,
            //apiService.get(apiURL + 'api/Data/quotationFull/' + 62, null,
            loadQuotationCompleted,
            loadQuotationFailed);
        }

        function loadQuotationCompleted(response) {
            console.log(response.data);
            $scope.quotation = response.data;
            $scope.Consignee = $sce.trustAsHtml(response.data.TermsConditionsConsignee);
            $scope.Consigner = $sce.trustAsHtml(response.data.TermsConditionsConsigner);
        }

        function loadQuotationFailed(response) {
            notificationService.displayError(response.data);
        }

        loadQuotation();
    }

})(angular.module('ace'));