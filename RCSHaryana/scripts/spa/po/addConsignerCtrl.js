(function (app) {
    'use strict';

    app.controller('addConsignerCtrl', addConsignerCtrl);

    addConsignerCtrl.$inject = ['$scope', '$modalInstance', '$timeout', 'apiService', 'notificationService', '$sce'];

    function addConsignerCtrl($scope, $modalInstance, $timeout, apiService, notificationService, $sce) {

        $scope.loadConsignerTerms = loadConsignerTerms;
        $scope.cancelConsigner = cancelConsigner;
        $scope.addConsignerTerms = addConsignerTerms;
        $scope.pickTerms = pickTerms;

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
                $scope.po.TermsConditionsConsignee = $scope.x; //
                document.getElementById('Consignee').innerHTML = $scope.po.TermsConditionsConsignee;
            }
            else {
                $scope.po.TermsConditionsConsigner = $scope.x; //$sce.trustAsHtml($scope.x);
                document.getElementById('Consigner').innerHTML = $scope.po.TermsConditionsConsigner;
            }
            $modalInstance.dismiss();
        }

        function pickTerms(tid) {
            console.log($scope.Terms);
        }

        function cancelConsigner() {
            $modalInstance.dismiss();
        }

        // Load ConsignerTerms
        loadConsignerTerms();
    }

})(angular.module('ace'));