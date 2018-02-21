(function (app) {
    'use strict';

    app.controller('detailsCtrl', detailsCtrl);

    detailsCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$filter', '$modal'];

    function detailsCtrl($scope, apiService, notificationService, $filter, $modal) {

        $scope.page = 0;
        $scope.pagesCount = 0;

        $scope.customerID = getUrlParameter('id');
        $scope.kittySubs = {};
        $scope.gymSubs = {};
        $scope.customer = {};
        $scope.loadCustomer = loadCustomer;
        $scope.bindKitty = loadKitty;
        $scope.membershipOptions = loadMO;
        $scope.packageType = loadPT;
        $scope.addSubsKitty = addSubsKitty;
        $scope.addSubsGym = addSubsGym;
        $scope.kittySubs.ProductID = 1;
        $scope.theProductID = 0;

        // Delete subscription
        $scope.deleteSubsc = function (id) {
            var result = confirm('Are you sure you want to delete this subscription?');

            if (result === true) {
                apiService.post(apiURL + 'api/subscription/delete?id='+id, null,
                    deleteSubscCompleted,
                    deleteSubscFailed);
            }
        }

        function deleteSubscCompleted(response) {
            notificationService.displaySuccess('Subscription has been cancelled');
            loadKitty();
            loadSubscriptions();
            loadKittySubscriptions();
        }

        function deleteSubscFailed(response) {
            notificationService.displayError(response);
        }

        // Load Kitty Batches
        $scope.loadBatch = function () {
            apiService.get(apiURL + 'api/kitty/batches', null,
                loadBatchCompleted,
                loadBatchFailed);
        }

        function loadBatchCompleted(response) {
            $scope.batch = response.data;
            $scope.selectedBatch = response.data.data['15'];
            loadKitty();
        }

        function loadBatchFailed(response) {
            notificationService.displayError(response);
        }

        // Batch change
        $scope.reloadKitty = function () {
            $scope.kittySubs = {};
            loadKitty();
        }

        $scope.kittyChange = function () {

            angular.forEach($scope.bindKitty, function (s) {
                if (s.ProductID == $scope.kittySubs.ProductID) {
                    $scope.kittySubs.ActualStartDate = s.StartDate;
                    $scope.kittySubs.MonthlyInstalment = s.MonthlyInstalment;
                    $scope.kittySubs.TenureInMonths = s.TenureInMonths;
                }
            });

            $scope.$watch('kittySubs.ActualStartDate', function (newValue) {
                $scope.kittySubs.ActualStartDate = $filter('date')(newValue, 'MM/dd/yyyy');
            });
        }

        $scope.subsKittyChange = function (theProductID) {
            loadPaymentSchedule(theProductID);
        }

        $scope.openEditDialog = openEditDialog;

        function openEditDialog(dueDate, instAmnt, status, interest) {
            $scope.installmentAmnt = instAmnt - interest;
            $scope.installmentDate = dueDate;
            $scope.interest = interest;

            if (status !== 'Paid') {
                $modal.open({
                    templateUrl: '/scripts/spa/customer/paymentSummary.html?0.1',
                    controller: 'paymentSummaryCtrl',
                    scope: $scope
                }).result.then(function ($scope) {
                    //clearSearch();
                }, function () {
                });
            }
        }

        // Kitty
        function loadKitty() {
            var config = {
                params: {
                    batch: $scope.selectedBatch
                }
            };
            apiService.get(apiURL + 'api/kitty/GetKittyList', config,
                loadKittyCompleted,
                loadKittyFailed);
        }

        function loadKittyCompleted(response) {
            $scope.bindKitty = response.data;
        }

        function loadKittyFailed(response) {
            notificationService.displayError(response.data);
        }

        // Customer
        function loadCustomer() {
            apiService.get(apiURL + 'api/customers/details/' + $scope.customerID, null,
                loadCustomerCompleted,
                loadCustomerFailed);
        }

        function loadCustomerCompleted(response) {
            $scope.customer = response.data;
        }

        function loadCustomerFailed(response) {
            notificationService.displayError(response.data);
        }

        // Subscription
        function loadSubscriptions() {
            apiService.get(apiURL + 'api/customers/subscriptions/' + $scope.customerID, null,
                loadSubCompleted,
                loadSubFailed);
        }

        function loadSubCompleted(response) {
            $scope.subscriptions = response.data;
        }

        function loadSubFailed(response) {
            notificationService.displayError(response.data);
        }

        // Kitties Subscribed
        function loadKittySubscriptions() {
            apiService.get(apiURL + 'api/customers/kitty-subscriptions/' + $scope.customerID, null,
                loadKittySubCompleted,
                loadKittySubFailed);
        }

        function loadKittySubCompleted(response) {
            $scope.kittySubscriptions = response.data;
           
            if (response.data.length > 0) {
                $scope.theProductID = response.data[0].ProductID;

                // Load payment schedule
                loadPaymentSchedule($scope.theProductID);
            }
        }

        function loadKittySubFailed(response) {
            notificationService.displayError(response.data);
        }

        // PaymentSchedule
        function loadPaymentSchedule(productID) {

            var config = {
                params: {
                    productID: productID,
                    customerID: $scope.customerID 
                }
            };

            apiService.get(apiURL + 'api/subscription/schedule', config,
                loadPSCompleted,
                loadPSFailed);
        }

        function loadPSCompleted(response) {
            $scope.schedule = response.data;
        }

        function loadPSFailed(response) {
            notificationService.displayError(response.data);
        }

        // Membership Option
        function loadMO(page) {
            page = page || 0;
            var config = {
                params: {
                    page: page,
                    pageSize: 100,
                    filter: ''
                }
            };
            apiService.get(apiURL + 'api/admin/search-mo', config,
                loadMOCompleted,
                loadMOFailed);
        }

        function loadMOCompleted(response) {
            $scope.membershipOptions = response.data.Items;
            $scope.gymSubs.ProductID = $scope.membershipOptions[0].value;
        }

        function loadMOFailed(response) {
            notificationService.displayError(response.data);
        }

        // Package Type
        function loadPT() {
            apiService.get(apiURL + 'api/admin/package-type', null,
                loadPTCompleted,
                loadPTFailed);
        }

        function loadPTCompleted(response) {
            $scope.packageType = response.data;
        }

        function loadPTFailed(response) {
            notificationService.displayError(response.data);
        }

        // Add Kitty as Subs
        function addSubsKitty() {
            var myurl = apiURL + "api/subscription/post";

            $scope.kittySubs.ProductType = 1; // 1 - kitty, 2 - gym
            $scope.kittySubs.CustomerID = $scope.customer.ID;

            apiService.post(myurl, $scope.kittySubs,
                addKittySubsSucceeded,
                addKittySubsFailed);
        }

        function addKittySubsSucceeded(response) {
            notificationService.displaySuccess('Subscription has been added');
            window.location.href = '/customers/show?id=' + $scope.customer.ID;
            //loadKitty();
            //loadSubscriptions();
            //loadKittySubscriptions();
        }

        function addKittySubsFailed(response) {
            notificationService.displayError(response.statusText);
        }

        // Add Gym as Subs
        function addSubsGym() {
            var myurl = apiURL + "api/subscription/post";

            $scope.gymSubs.ProductType = 2; // 1 - kitty, 2 - gym
            $scope.gymSubs.CustomerID = $scope.customer.ID;

            apiService.post(myurl, $scope.gymSubs,
                addSubsGymSucceeded,
                addSubsGymFailed);
        }

        function addSubsGymSucceeded(response) {
            notificationService.displaySuccess('Subscription has been added');
            window.location.href = '/customers/show?id=' + $scope.customer.ID;
            //loadKitty();
            //loadSubscriptions();
            //loadKittySubscriptions();
        }

        function addSubsGymFailed(response) {
            notificationService.displayError(response.statusText);
        }

        $scope.loadBatch();
        loadMO();
        loadCustomer();
       
        loadPT();
        loadSubscriptions();
        loadKittySubscriptions();
    }

})(angular.module('ace'));