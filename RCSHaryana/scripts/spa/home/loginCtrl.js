(function (app) {
    'use strict';

    app.controller('loginCtrl', loginCtrl);

    loginCtrl.$inject = ['$scope', 'membershipService', 'notificationService', '$rootScope', '$location'];

    function loginCtrl($scope, membershipService, notificationService, $rootScope, $location) {
        $scope.login = login;
        $scope.user = {};

        function login() {
            console.log('hello');
            membershipService.login($scope.user, loginCompleted)
        }

        function loginCompleted(result) {
            console.log(result);
            if (result.data.success) {
                membershipService.saveCredentials($scope.user, result.data.roles);
                notificationService.displaySuccess('Hello ' + $scope.user.username+'. Redirecting to Dashboard.');

                window.location.href = "/Admin/Index";
            }
            else {
                notificationService.displayError('Login failed. Try again.');
            }
        }
    }

})(angular.module('common.core'));