(function (app) {
    'use strict';

    app.controller('addPOCtrl', addPOCtrl);

    addPOCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService', '$modal'];

    function addPOCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService, $modal) {

        $scope.units = [];
        $scope.tt = [];
        $scope.po = {};
        $scope.po.POItems = [];
        $scope.Terms = [];

        $scope.Taxes = 0;
        $scope.po.AdditionalInfo = [];

        $scope.AddPO = AddPO;
        $scope.AddMore = AddMore;
        $scope.RemoveItem = RemoveItem;
        $scope.calcAmount = calcAmount;
        $scope.prepareFiles = prepareFiles;
        $scope.openDatePicker = openDatePicker;
        $scope.getTT = getTT;

        // Auto Complete events
        $scope.selectBeneficiary = selectBeneficiary;
        $scope.selectionChanged = selectionChanged;
        $scope.isEnabled = false;

        $scope.selectPOItem = selectPOItem;
        $scope.selectionChangedQ = selectionChangedQ;

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};
        $scope.datepicker2 = {};
        $scope.datepicker3 = {};

        var attcahedDoc = null;

        $scope.openEditDialog = openEditDialog;

        function openEditDialog(termType) {
            $scope.tt = termType;
            $modal.open({
                templateUrl: '/scripts/spa/quotation/addConsignerModal.html',
                controller: 'addConsignerCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                //clearSearch();
            }, function () {
            });
        }

        function calcAmount(indx) {
            //console.log(indx);
            var list = $scope.po.POItems;
            list[indx].Amount = (list[indx].Quantity * list[indx].Rate) - ((list[indx].Discount / 100) * list[indx].Quantity * list[indx].Rate);

            $scope.po.SubTotal = 0;
            $scope.po.TotalPrice = 0;
            var taxAmount = 0;
            var amount = 0;

            for (var ctr = 0; ctr < list.length; ctr++) {
                amount += list[ctr].Amount;
            }

            var txs = $scope.po.AdditionalInfo;
           
            for (var ctr = 0; ctr < txs.length; ctr++) {
                taxAmount = taxAmount + ((txs[ctr].InfoValue / 100) * amount);
            }
            $scope.po.SubTotal = amount;
            console.log(taxAmount);
            $scope.po.TotalPrice = $scope.po.SubTotal + taxAmount;
            numToWords();
        }

        function numToWords() {
            apiService.get(apiURL + 'api/Data/numToWords?number=' + $scope.po.TotalPrice, null,
               function (result) {
                   //alert(result.data);
                   $scope.po.PriceInWords = result.data;
               }, null);
        }

        function selectBeneficiary($item) {
            if ($item) {
                console.log($item.originalObject.ID);
                $scope.po.BeneficiaryID = $item.originalObject.ID;

                // Get Beneficiary details by Beneficiary ID
                apiService.get(apiURL + 'api/Data/beneficiaryDetails/' + $scope.po.BeneficiaryID, null,
                beneficiaryLoadCompleted,
                beneficiaryLoadFailed);
            }
            else {
                $scope.isEnabled = false;
            }
        }

        function selectPOItem($item2) {
            if ($item2) {
                console.log($item2.originalObject.ID);
                var itemID = $item2.originalObject.ID;
                var itemCode = $item2.originalObject.ItemCode;
                //console.log(itemID);

                // Get Item details by Beneficiary ID
                apiService.get(apiURL + 'api/Data/itemDetails/' + itemID, null,
                itemLoadCompleted,
                itemLoadFailed);
            }
            else {
                //$scope.isEnabled = false;
            }
        }

        // Beneficiary details SUCCESS
        function beneficiaryLoadCompleted(result) {
            //$scope.po = result.data;
            $scope.po.BeneficiaryName = result.data.Name;
            $scope.po.BeneficiaryAddress = result.data.Address;
            $scope.po.BeneficiaryPhone = result.data.Phone;
            $scope.po.BeneficiaryMobile = result.data.Mobile;
            $scope.po.BeneficiaryAlternateNo = result.data.AlternateNo;
            $scope.po.BeneficiaryEmail = result.data.Email;

            $scope.isEnabled = true;
        }

        // Beneficiary details FAIL
        function beneficiaryLoadFailed(response) {
            notificationService.displayError(response.data);
            $scope.isEnabled = false;
        }

        // Item details SUCCESS
        function itemLoadCompleted(result) {

            var list = $scope.po.POItems;
            console.log($scope.po.POItems);
            for (var c = 0; c < list.length; c++) {
                if ($scope.po.POItems[c].ItemName.length == 0) {
                    $scope.po.POItems[c].ItemCode = result.data.ItemCode;
                    $scope.po.POItems[c].ItemName = result.data.ItemName;
                    $scope.po.POItems[c].Description = result.data.Description;
                    $scope.po.POItems[c].Rate = result.data.Rate;
                    $scope.po.POItems[c].Unit = result.data.Unit;
                    calcAmount(c);
                    break;
                }
            }

        }

        // Item details FAIL
        function itemLoadFailed(response) {
            notificationService.displayError(response.data);
            $scope.isEnabled = false;
        }

        function selectionChanged($item) {
        }

        function selectionChangedQ($item) {
        }

        function AddPO() {
            AddPOModel();
        }

        function AddMore() {
            $scope.po.POItems.push({ "Item": "", "Grade": "", "Description": "", "Size": "", "Quantity": "", "Unit": "", "Rate": "", "Amount": "" });
        }

        function RemoveItem(idx) {
            $scope.po.POItems.splice(idx, 1);
        }

        function AddPOModel() {
            apiService.post(apiURL + 'api/data/addPO', $scope.po,
            addQSucceded,
            addQFailed);
        }

        function prepareFiles($files) {
            attcahedDoc = $files;
        }

        function addQSucceded(response) {
            notificationService.displaySuccess('Added successfully');
            $scope.movie = response.data;

            if (attcahedDoc) {
                fileUploadService.uploadPO(attcahedDoc, $scope.po.ID, redirectToEdit);
            }
            else
                redirectToEdit();
        }

        function addQFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

        function openDatePicker($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker.opened = true;
        };

        function openDatePicker2($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker2.opened = true;
        };

        function openDatePicker3($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker3.opened = true;
        };

        function redirectToEdit() {
            $location.url(apiURL + 'po/edit/' + $scope.movie.ID);
        }

        // Load Units
        function loadUnits() {
            apiService.get(apiURL + 'api/data/searchUnits/', null,
            unitsLoadCompleted,
            unitsLoadFailed);
        }

        function unitsLoadCompleted(response) {
            $scope.units = response.data;
        }

        function unitsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        // Load TT
        function loadTT() {
            apiService.get(apiURL + 'api/data/searchTaxMaster/', null,
            ttLoadCompleted,
            ttLoadFailed);
        }

        function ttLoadCompleted(response) {
            $scope.po.AdditionalInfo = response.data;
        }

        function ttLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function getTT() {
            //console.log($scope.po.Taxes.TaxName);
        }

        // Auto fill PO details from quotation details
        function loadPODetails() {
            $scope.loadingInvoice = true;
            apiService.get(apiURL + 'api/Data/quotationDetails/' + getUrlParameter('id'), null,
            poLoadCompleted,
            poLoadFailed);
        }

        function poLoadCompleted(result) {
            $scope.po = result.data;
            $scope.po.BillTo = result.data.BeneficiaryAddress;
            $scope.po.ShipTo = result.data.BeneficiaryAddress;
            $scope.po.Taxes = 0;

            $scope.po.POItems = new Array(0);
            $scope.isEnabled = true;
            // Items
            for (var x = 0; x < result.data.QuotationItems.length; x++) {
                $scope.po.POItems.push({
                    "ID": x + 1, "ItemCode": result.data.QuotationItems[x].ItemCode,
                    "ItemName": result.data.QuotationItems[x].ItemName,
                    "Description": result.data.QuotationItems[x].Description,
                    "Quantity": result.data.QuotationItems[x].Quantity,
                    "Unit": result.data.QuotationItems[x].Unit,
                    "Rate": result.data.QuotationItems[x].Rate,
                    "Discount": result.data.QuotationItems[x].Discount,
                    "Amount": result.data.QuotationItems[x].Amount
                });
            }

            // Additional Info
            //for (var x = 0; x < result.data.AdditionalInfo.length; x++) {
            //    $scope.po.AdditionalInfo.push({ "ID": x + 1, "InfoLabel": result.data.AdditionalInfo[x].InfoLabel, "InfoValue": result.data.AdditionalInfo[x].InfoValue });
            //}
            //calcAmount(0);
            console.log(result.data);
            //alert($scope.po.POItems.length);
        }

        function poLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        var qid = getUrlParameter('id');
        if (qid != "undefined") {
            loadPODetails();
        }


        loadUnits();
        loadTT();
    }

})(angular.module('ace'));