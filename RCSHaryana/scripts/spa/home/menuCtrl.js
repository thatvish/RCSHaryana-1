(function (app) {
    'use strict';

    app.controller('menuCtrl', menuCtrl);

    menuCtrl.$inject = ['$scope', 'apiService', 'notificationService'];
    function menuCtrl($scope, apiService, notificationService) {

        $scope.menuData = {};//[{ 'menuItem': 'Home', 'url': '#' }, { 'menuItem': 'Request', 'url': 'client/addrequest' }, { 'menuItem': 'Profile', 'url': '#' }];

        var config = {
            params: {
                username: $scope.username
            }
        };

        function loadMenu() {
            apiService.get(apiURL + 'api/account/get-menu/', config,
               menuLoadCompleted,
               menuLoadFailed);
        }

        function menuLoadCompleted(result) {
            $scope.menuData = result.data.Items;
            //alert($scope.menuData[1].Title);
        }

        function menuLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        loadMenu();
    }

})(angular.module('ace'));