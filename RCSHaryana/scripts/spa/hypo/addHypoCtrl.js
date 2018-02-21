(function (app) {
    'use strict';

    app.controller('addHypoCtrl', addHypoCtrl);

    addHypoCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService'];

    function addHypoCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService) {

        $scope.hypo = {};
        $scope.addHypo = addHypo;
        $scope.prepareFiles = prepareFiles;
        $scope.openDatePicker = openDatePicker;

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};

        var attcahedDoc = null;

        function loadhypoDetails() {
            $scope.loadinghypo = true;
           
            apiService.get(apiURL + 'api/Data/invoiceDetails/' + getUrlParameter('id'), null,
            hypoLoadCompleted,
            hypoLoadFailed);
        }

        function hypoLoadCompleted(result) {
            $scope.hypo = result.data;
            $scope.hypo.InvoiceID = getUrlParameter('id');
            $scope.loadinghypo = false;
        }

        function hypoLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function addHypo() {
            AddhypoModel();
        }

        function AddhypoModel() {
            var myurl = apiURL + "api/Data/addHypo/";
            apiService.post(myurl, $scope.hypo,
            addhypoSucceded,
            addhypoFailed);
        }

        function prepareFiles($files) {
            attcahedDoc.push($files);
        }

        function addhypoSucceded(response) {
            notificationService.displaySuccess('Hypothecation has been added');
           
            if (attcahedDoc) {
                fileUploadService.uploadHypo(attcahedDoc, $scope.hypo.ID, redirectToEdit);
            }
        }

        function addhypoFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

        function openDatePicker($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker.opened = true;
        };

        loadhypoDetails();
    }

})(angular.module('ace'));