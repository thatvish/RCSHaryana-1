(function (app) {
    'use strict';

    app.factory('membershipService', membershipService);

    membershipService.$inject = ['apiService', 'notificationService','$http', '$base64', '$cookieStore', '$rootScope'];

    function membershipService(apiService, notificationService, $http, $base64, $cookieStore, $rootScope) {

        var service = {
            login: login,
            register: register,
            saveCredentials: saveCredentials,
            removeCredentials: removeCredentials,
            isUserLoggedIn: isUserLoggedIn
        }

        function login(user, completed) {
            apiService.post(apiURL + 'api/account/authenticate', user,
            completed,
            loginFailed);
        }

        function register(user, completed) {
            apiService.post(apiURL + 'api/account/register', user,
            completed,
            registrationFailed);
        }

        function saveCredentials(user, roles) {
            var membershipData = $base64.encode(user.username + ':' + user.password);
            console.log(roles);
            $rootScope.repository = {
                loggedUser: {
                    username: user.username,
                    authdata: membershipData,
                    role: roles
                }
            };

            $http.defaults.headers.common['Authorization'] = 'Basic ' + membershipData;
            $cookieStore.put('repository', $rootScope.repository);
            $cookieStore.put('uname', $rootScope.repository.loggedUser.username);
        }

        function removeCredentials() {
            $rootScope.repository = {};
            $cookieStore.remove('repository');
            $cookieStore.remove('uname');
            $http.defaults.headers.common.Authorization = '';

            notificationService.displaySuccess('Logging out, please wait...');
        };

        function loginFailed(response) {
            notificationService.displayError(response.data);
        }

        function registrationFailed(response) {

            notificationService.displayError('Registration failed. Try again.');
        }

        function isUserLoggedIn() {
            return $rootScope.repository.loggedUser !== null;
        }

        return service;
    }



})(angular.module('common.core'));