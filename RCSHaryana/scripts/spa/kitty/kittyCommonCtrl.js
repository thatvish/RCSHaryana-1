

(function (app) {
    'use strict';

    app.controller('kittyCommonCtrl', kittyCommonCtrl);

    kittyCommonCtrl.$inject = ['$scope', 'apiService', 'notificationService'];

    function kittyCommonCtrl($scope, apiService, notificationService) {

        $scope.kitty = {};
        $scope.kittyID = getUrlParameter('id');
        $scope.loadDetails = function () {
            var config = {
                params: {
                    id: $scope.kittyID
                }
            };

            var myurl = apiURL + "api/kitty/details";
            apiService.get(myurl, config,
                detailsLoadCompleted,
                detailsLoadFailed);
        }

        function detailsLoadCompleted(response) {
            $scope.kitty = response.data;
        }

        function detailsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        // Load Kitty Info
        $scope.loadDetails();
    }
})(angular.module('ace'));