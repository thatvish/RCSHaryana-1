(function (app) {
    'use strict';

    app.controller('drawKittyCtrl', drawKittyCtrl);

    drawKittyCtrl.$inject = ['$scope', '$routeParams', 'apiService', 'notificationService'];

    function drawKittyCtrl($scope, $routeParams, apiService, notificationService) {

        $scope.kitty = {};
        $scope.drawKitty = drawKitty;

        function drawKitty() {
            drawKittyModel();
        }

        function drawKittyModel() {
            console.log($routeParams);
            var myurl = apiURL + "api/kitty/draw";
            apiService.post(myurl, $scope.kitty,
                drawKittySucceded,
                drawKittyFailed);
        }

        function drawKittySucceded(response) {
            notificationService.displaySuccess('Draw has been saved.');
            
            window.location.href = '/kitty/draws?id=' + getUrlParameter('kittyId');
        }

        function drawKittyFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

        function loadCustomers() {

            var config = {
                params: {
                    kittyId: getUrlParameter('kittyId')
                }
            };
            apiService.get(apiURL + 'api/kitty/members/', config,
                custLoadCompleted,
                custLoadFailed);
        }

        function custLoadCompleted(response) {
            $scope.customers = response.data;
        }

        function custLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        loadCustomers();
    }

})(angular.module('ace'));