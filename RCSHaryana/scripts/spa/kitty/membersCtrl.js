(function (app) {
    'use strict';

    app.controller('membersCtrl', membersCtrl);

    membersCtrl.$inject = ['$scope', 'apiService', 'notificationService'];

    function membersCtrl($scope, apiService, notificationService) {

        $scope.customers = [];
        $scope.kittyID = getUrlParameter('id');
        $scope.totalCount = 0;

        // Load Members
        $scope.loadMembers = function () {
            var config = {
                params: {
                    kittyId: $scope.kittyID
                }
            };

            var myurl = apiURL + "api/kitty/members";
            apiService.get(myurl, config,
                memberLoadCompleted,
                memberLoadFailed);
        }

        function memberLoadCompleted(response) {
            $scope.customers = response.data;
            $scope.totalCount = $scope.customers.length;
        }

        function memberLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        // Delete subscription
        $scope.deleteSubsc = function (id) {
            var result = confirm('Are you sure you want to delete this subscription?');

            if (result === true) {
                apiService.post(apiURL + 'api/subscription/delete?id=' + id, null,
                    deleteSubscCompleted,
                    deleteSubscFailed);
            }
        }

        function deleteSubscCompleted(response) {
            notificationService.displaySuccess('Subscription has been cancelled');
            $scope.loadMembers();
        }

        function deleteSubscFailed(response) {
            notificationService.displayError(response);
        }


        // Load Kitty Members
        $scope.loadMembers();
    }
})(angular.module('ace'));