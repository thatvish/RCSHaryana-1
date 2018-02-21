(function (app) {
    'use strict';

    app.controller('interestCalcCtrl', interestCalcCtrl);

    interestCalcCtrl.$inject = ['$scope', 'apiService', 'notificationService'];

    function interestCalcCtrl($scope, apiService, notificationService) {

        $scope.kitty1 = [];
        $scope.kitty2 = [];
        $scope.kitty3 = [];
        $scope.kitty4 = [];

        $scope.calcinterestCalc = function (kitty) {
            console.log(kitty);
            var myurl = apiURL + "api/kitty/update-interestCalc/";
            apiService.post(myurl, kitty,
                updateinterestCalcSucceeded,
                updateinterestCalcFailed);
        }

        function updateinterestCalcSucceeded(response) {
            loadKitty(response.data.IntervalID);
        }

        function updateinterestCalcFailed(response) {
            notificationService.displayError(response.statusText);
        }

        $scope.monthChange = function () {
            bindGrids();
        }

        $scope.inputMonth = [
            {
                value: '11',
                label: 'November'
            },
            {
                value: '12',
                label: 'December'
            }
        ];
        $scope.selectedMonth = 0;

        function loadKitty(intervalID) {
            var config = {
                params: {
                    intervalId: intervalID,
                    month: $scope.selectedMonth
                }
            };

            var myurl = apiURL + "api/kitty/interestCalc-and-loss";
            apiService.get(myurl, config,
                kittyLoadCompleted,
                kittyLoadFailed);
        }

        function kittyLoadCompleted(response) {
            if (response.data.length > 0) {
                if (response.data[0].IntervalID === 1) {
                    $scope.kitty1 = response.data;
                }
                if (response.data[0].IntervalID === 2) {
                    $scope.kitty2 = response.data;
                }
                if (response.data[0].IntervalID === 3) {
                    $scope.kitty3 = response.data;
                }
                if (response.data[0].IntervalID === 4) {
                    $scope.kitty4 = response.data;
                }
            }
        }

        function kittyLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function bindGrids() {
            for (var i = 1; i <= 4; i++) {
                loadKitty(i);
            }
        }
    }
})(angular.module('ace'));