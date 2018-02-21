(function (app) {
    'use strict';

    app.controller('addConsignerCtrl', addConsignerCtrl);

    addConsignerCtrl.$inject = ['$scope', '$modalInstance', '$timeout', 'apiService', 'notificationService', '$sce'];

    function addConsignerCtrl($scope, $modalInstance, $timeout, apiService, notificationService, $sce) {

        //$scope.loadConsignerTerms = loadConsignerTerms;
        $scope.cancelConsigner = cancelConsigner;
        //$scope.addConsignerTerms = addConsignerTerms;

        function loadConsignerTerms() {

            var config = {
                params: {
                    termType: $scope.tt
                }
            };
            var myurl = apiURL + "api/Data/listConsignerTerms/";
            apiService.get(myurl, config,
            consignerLoadCompleted,
            consignerLoadFailed);
        }

        function consignerLoadCompleted(result) {
            $scope.Terms = result.data;
        }

        function consignerLoadFailed() {
            notificationService.displayError(response.data);
        }

        function addConsignerTerms() {
            $scope.x = "";
            $scope.x = "<ol>";
            angular.forEach($scope.Terms, function (trm) {
                if (trm.selected) {
                    $scope.x += "<li>" + trm.Term + "</li>";
                };
            });
            $scope.x += "</ol>";
            if ($scope.tt == "Consignee") {
                $scope.quotation.TermsConditionsConsignee = $scope.x; //
                document.getElementById('Consignee').innerHTML = $scope.quotation.TermsConditionsConsignee;
            }
            else {
                $scope.quotation.TermsConditionsConsigner = $scope.x; //$sce.trustAsHtml($scope.x);
                document.getElementById('Consigner').innerHTML = $scope.quotation.TermsConditionsConsigner;
            }
            $modalInstance.dismiss();
        }

        function cancelConsigner() {
            $modalInstance.dismiss();
        }
    }

})(angular.module('ace'));