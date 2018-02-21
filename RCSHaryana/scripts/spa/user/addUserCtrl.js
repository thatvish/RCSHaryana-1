(function (app) {
    'use strict';

    app.controller('addUserCtrl', addUserCtrl);

    addUserCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'membershipService'];

    function addUserCtrl($scope, $location, $routeParams, apiService, notificationService, membershipService) {

        // Check if user is autheticated
        if (!membershipService.isUserLoggedIn()) {
            window.location.href = "/Home/Login?unauthenticated";
        }

        $scope.user = {};
        $scope.addUser = addUser;
        $scope.RoleID = 1;
        $scope.roles = ["Admin", "Finance", "Purchase"];

        function addUser() {
            AdduserModel();
        }

        function AdduserModel() {
            var myurl = apiURL + "api/Account/register/";
            apiService.post(myurl, $scope.user,
            adduserSucceded,
            adduserFailed);
        }

        function adduserSucceded(response) {
            notificationService.displaySuccess('User has been added');
        }

        function adduserFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }
    }

})(angular.module('ace'));