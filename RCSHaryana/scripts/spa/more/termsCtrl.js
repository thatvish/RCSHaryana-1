(function (app) {
    'use strict';

    app.controller('termsCtrl', termsCtrl);

    termsCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService', '$modal'];

    function termsCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService, $modal) {

        $scope.terms = {};
        $scope.addUpdateTerms = addUpdateTerms;

        function addUpdateTerms() {
            console.log($("#sampleEditor").val());
            $scope.terms.TermsConditions = $("#sampleEditor").val();
            console.log($scope.terms);
            apiService.post('/api/data/addUpdateTerms', $scope.terms,
            addTermsSucceded,
            addTermsFailed);
        }

        function addTermsSucceded(response) {
            notificationService.displaySuccess('Updated successfully');
            $scope.terms = response.data;
        }

        function addTermsFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }
    }

})(angular.module('ace'));