(function (app) {
    'use strict';

    app.controller('addCustCtrl', addCustCtrl);

    addCustCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService'];

    function addCustCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService) {

        $scope.cust = {};
        $scope.addCust = addCust;
        $scope.prepareFiles = prepareFiles;

        var attcahedDoc = null;

        function addCust() {
            AddcustModel();
        }

        function AddcustModel() {
            // Change made on 23 Oct 2017 by AJA
            var myurl = apiURL + "api/customers/Post/";
            //$scope.cust = {
            //    "ID": 1,
            //    "FirstName": "sample string 2",
            //    "LastName": "sample string 3",
            //    "Email": "sample string 4",
            //    "IdentityCard": "sample string 5",
            //    "UniqueKey": "ef85db60-cb76-4201-9471-029d5eed451b",
            //    "DateOfBirth": "2017-09-30T11:57:52.8206955+05:30",
            //    "Mobile": "sample string 8",
            //    "RegistrationDate": "2017-09-30T11:57:52.8206955+05:30",
            //    "Address": "sample string 10",
            //    "City": "sample string 11",
            //    "StateName": "sample string 12"
            //};

            apiService.post(myurl, $scope.cust,
                addCustSucceded,
                addCustFailed);
        }

        function prepareFiles($files) {
            attcahedDoc.push($files);
        }

        function addCustSucceded(response) {
            notificationService.displaySuccess('Customer has been added');

            if (attcahedDoc) {
                fileUploadService.uploadCust(attcahedDoc, $scope.cust.ID, redirectToEdit);
            }
        }

        function addCustFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }
    }

})(angular.module('ace'));