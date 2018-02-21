(function (app) {
    'use strict';

    app.controller('rootCtrl', rootCtrl);

    rootCtrl.$inject = ['$scope','$location', 'membershipService','$rootScope'];
    function rootCtrl($scope, $location, membershipService, $rootScope) {

        $scope.userData = {};
        
        $scope.userData.displayUserInfo = displayUserInfo;
        $scope.logout = logout;


        function displayUserInfo() {
            $scope.userData.isUserLoggedIn = membershipService.isUserLoggedIn();
            if($scope.userData.isUserLoggedIn)
            {
                var loggedInUser = $rootScope.repository.loggedUser;
                if (loggedInUser)
                {
                    $scope.username = $rootScope.repository.loggedUser.username;
                }
            }
        }

        function logout() {

            membershipService.removeCredentials();

            // Add cookies manually
            var cookies = document.cookie.split(";");
            for (var i = 0; i < cookies.length; i++)
                eraseCookie(cookies[i].split("=")[0]);

            timedRedirect('/', 2000);
        }

        $scope.userData.displayUserInfo();
    }

})(angular.module('ace'));