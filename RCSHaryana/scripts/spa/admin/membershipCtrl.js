(function (app) {
    'use strict';

    app.controller('membershipCtrl', membershipCtrl);

    membershipCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService', '$rootScope'];

    function membershipCtrl($scope, $modal, apiService, notificationService, $rootScope) {

        $scope.loadingMembershipOption = true;
        $scope.loadingPackagetypeOption = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.membershipOptionList = [];
        $scope.packagetypeList = [];

        $scope.search = search;
        $scope.searchPackgetypes = searchPackgetypes;
        $scope.deleteMembershipOption = deleteMembershipOption;
        $scope.clearSearch = clearSearch;
       
        function deleteMembershipOption(kittyModel) {
            apiService.get(apiURL + 'api/kitty/delete/' + kitty.id, null,
            MembershipOptionLoadCompleted,
            MembershipOptionLoadFailed);
        }

        function searchPackgetypes() {
            
            $scope.loadingPackagetypeOption = true;

            var config = {
                params: {

                }
            };
            var myurl = apiURL + "api/admin/package-type/";
            apiService.get(myurl, null,
            packagetypeLoadCompleted,
            packagetypeLoadFailed);
        }

        function packagetypeLoadCompleted(result) {

         
            $scope.packagetypeList = result.data;

            $scope.totalCount = result.data.TotalCount;
            $scope.loadingPackagetypeOption = false;

            //if ($scope.filterMembershipOption && $scope.filterMembershipOption.length) {
            //    notificationService.displayInfo(result.data.Items.length + ' Membership Not found');
            //}
        }

        function packagetypeLoadFailed(response) {
            notificationService.displayError(response.data);
        }


        function search(page) {
            page = page || 0;

            $scope.loadingMembershipOption = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filterKitty
                }
            };
            var myurl = apiURL + "api/admin/search-mo/";
            apiService.get(myurl, config,
            MembershipOptionLoadCompleted,
            MembershipOptionLoadFailed);
        }

        function MembershipOptionLoadCompleted(result) {

            //alert(result.data.Items.length);
            $scope.membershipOptionList = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingMembershipOption = false;

            //if ($scope.filterMembershipOption && $scope.filterMembershipOption.length) {
            //    notificationService.displayInfo(result.data.Items.length + ' Membership Not found');
            //}
        }

        function MembershipOptionLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
           // $scope.filterMembershipOption = '';
            search();
        }

        $scope.search();
        $scope.searchPackgetypes();
    }

})(angular.module('ace'));