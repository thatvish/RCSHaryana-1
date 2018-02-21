(function (app) {
    'use strict';

    app.controller('qAddCtrl', qAddCtrl);

    qAddCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService', '$modal', '$sce'];

    function qAddCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService, $modal, $sce) {

        $scope.units = [];
        $scope.tt = [];
        $scope.quotation = {};

        $scope.quotation.QuotationItems = new Array(3);
        $scope.quotation.QuotationItems = [
            { "ID": 1, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": "", "Discount": 0, "Amount": "" },
            { "ID": 2, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": "", "Discount": 0, "Amount": "" },
            { "ID": 3, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": "", "Discount": 0, "Amount": "" }];

        $scope.quotation.AdditionalInfo = [
            {"ID":1, "InfoLabel":"", "InfoValue":0}];

        $scope.Taxes = 0;
        $scope.quotation.QuotationTaxes = [];
        $scope.quotation.TotalPrice = 0;
        $scope.Terms = [];

        $scope.AddQ = AddQ;
        $scope.AddMore = AddMore;
        $scope.RemoveItem = RemoveItem;

        $scope.AddMoreInfo = AddMoreInfo;
        $scope.RemoveInfo = RemoveInfo;

        $scope.calcAmount = calcAmount;
        $scope.prepareFiles = prepareFiles;
        $scope.openDatePicker = openDatePicker;
        $scope.openDatePicker2 = openDatePicker2;
        $scope.getTT = getTT;

        // Auto Complete events
        $scope.selectBeneficiary = selectBeneficiary;
        $scope.selectionChanged = selectionChanged;
        $scope.isEnabled = false;

        $scope.selectAI = selectAI;
        $scope.selectionChangedAI = selectionChangedAI;

        $scope.selectQuotationItem = selectQuotationItem;
        $scope.selectionChangedQ = selectionChangedQ;
        $scope.numToWords = numToWords();

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

        //$scope.addedItem
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};
        $scope.datepicker2 = {};

        var attcahedDoc = [];

        function selectAI() {
        }

        function selectionChangedAI() {
        }

        function calcAmount(indx) {
            var list = $scope.quotation.QuotationItems;

            if (indx < list.length) {
                list[indx].Amount = (list[indx].Quantity * list[indx].Rate) - ((list[indx].Discount / 100) * list[indx].Quantity * list[indx].Rate);

                $scope.quotation.SubTotal = 0.00;
                $scope.quotation.TotalPrice = 0.00;
                var addAmount = 0;
                var amount = 0;

                for (var ctr = 0; ctr < list.length; ctr++) {
                    if (list[ctr].ItemName.length > 0) {
                        amount += parseFloat(list[ctr].Amount);
                    }
                }
               
                var addlInfo = $scope.quotation.AdditionalInfo;
                for (var ctr = 0; ctr < addlInfo.length; ctr++) {
                    if (parseFloat(addlInfo[ctr].InfoPercent) > 0.00) {
                        addlInfo[ctr].InfoValue = parseFloat((addlInfo[ctr].InfoPercent / 100) * amount);
                    }
                    addAmount = addAmount + parseFloat(addlInfo[ctr].InfoValue);
                }
                $scope.quotation.AdditionalInfo = addlInfo;
                $scope.quotation.SubTotal = parseFloat(amount).toFixed(2);
                $scope.quotation.TotalPrice = parseFloat(amount + addAmount).toFixed(2);
                numToWords();
            }
        }

        function numToWords() {
            apiService.get(apiURL + 'api/Data/numToWords?number=' + parseInt($scope.quotation.TotalPrice), null,
               function (result) {
                   //alert(result.data);
                   $scope.quotation.PriceInWords = result.data;
               }, null);
        }

        function selectBeneficiary($item) {
            if ($item) {
                console.log($item.originalObject.ID);
                $scope.quotation.BeneficiaryID = $item.originalObject.ID;

                // Get Beneficiary details by Beneficiary ID
                apiService.get(apiURL + 'api/Data/beneficiaryDetails/' + $scope.quotation.BeneficiaryID, null,
                beneficiaryLoadCompleted,
                beneficiaryLoadFailed);
            }
            else {
                $scope.isEnabled = false;
            }
        }

        function selectQuotationItem($item2) {
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

        function selectAI($item2) {
            if ($item2) {
                console.log($item2.originalObject.ID);
                var infoID = $item2.originalObject.ID;
                var infoLabel = $item2.originalObject.InfoLabel;
                //console.log(itemID);

                // Get Item details by Beneficiary ID
                apiService.get(apiURL + 'api/Data/additionalInfoDetails/' + infoID, null,
                aiLoadCompleted,
                aiLoadFailed);
            }
            else {
                //$scope.isEnabled = false;
            }
        }

        function aiLoadCompleted(result) {
            var list = $scope.quotation.AdditionalInfo;
            console.log($scope.quotation.AdditionalInfo);
            for (var c = 0; c < list.length; c++) {
                if ($scope.quotation.AdditionalInfo[c].InfoLabel.length == 0) {
                    $scope.quotation.AdditionalInfo[c].InfoLabel = result.data.InfoLabel;
                    $scope.quotation.AdditionalInfo[c].InfoValue = result.data.InfoValue;
                   
                    calcAmount(c);
                    break;
                }
            }
        }

        // Item details FAIL
        function aiLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        // Beneficiary details SUCCESS
        function beneficiaryLoadCompleted(result) {
            //$scope.quotation = result.data;
            $scope.quotation.BeneficiaryName = result.data.Name;
            $scope.quotation.BeneficiaryAddress = result.data.Address;
            $scope.quotation.BeneficiaryPhone = result.data.Phone;
            $scope.quotation.BeneficiaryMobile = result.data.Mobile;
            $scope.quotation.BeneficiaryAlternateNo = result.data.AlternateNo;
            $scope.quotation.BeneficiaryEmail = result.data.Email;

            $scope.quotation.InFavorOf = "MG Contractors Pvt Ltd";//result.data.Name;
            $scope.quotation.TermsConditionsConsignee = " ";
            $scope.quotation.TermsConditionsConsigner = " ";
            $scope.isEnabled = true;
        }

        // Beneficiary details FAIL
        function beneficiaryLoadFailed(response) {
            notificationService.displayError(response.data);
            $scope.isEnabled = false;
        }

        // Item details SUCCESS
        function itemLoadCompleted(result) {

            var list = $scope.quotation.QuotationItems;
            console.log($scope.quotation.QuotationItems);
            for (var c = 0; c < list.length; c++) {
                if ($scope.quotation.QuotationItems[c].ItemName.length == 0) {
                    $scope.quotation.QuotationItems[c].ItemCode = result.data.ItemCode;
                    $scope.quotation.QuotationItems[c].ItemName = result.data.ItemName;
                    $scope.quotation.QuotationItems[c].Description = result.data.Description;
                    $scope.quotation.QuotationItems[c].Rate = result.data.Rate;
                    $scope.quotation.QuotationItems[c].Unit = result.data.Unit;
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

        function AddQ() {
            AddQModel();
        }

        function AddMore() {
            var itemsLength = $scope.quotation.QuotationItems.length;
            var id = itemsLength + 1;
            $scope.quotation.QuotationItems.push({ "ID": id, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": "", "Discount": "", "Amount": "" });
        }

        function RemoveItem(idx) {
            $scope.quotation.QuotationItems.splice(idx, 1);
            calcAmount(idx);
        }

        function AddMoreInfo() {
            var infoLength = $scope.quotation.AdditionalInfo.length;
            var id = infoLength + 1;
            $scope.quotation.AdditionalInfo.push({ "ID": id, "InfoLabel": "", "InfoValue": 0 });
        }

        function RemoveInfo(idx) {
            $scope.quotation.AdditionalInfo.splice(idx, 1);
            calcAmount(idx);
        }

        function AddQModel() {
            apiService.post(apiURL + 'api/data/addQ', $scope.quotation,
            addQSucceded,
            addQFailed);
        }

        function prepareFiles($files) {
            //attcahedDoc = $files;
            attcahedDoc.push($files);
        }

        function addQSucceded(response) {
           
            $scope.quotation.ID = response.data.ID;
            notificationService.displaySuccess('Quotation sucessfully added');
            $scope.movie = response.data;

            if (attcahedDoc) {
                fileUploadService.uploadAttachment(attcahedDoc, $scope.quotation.ID, redirectToEdit);
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

        function redirectToEdit() {
            //notificationService.displayError('Unable to upload attachment.');
            //$location.url(apiURL + 'quotation/edit/' + $scope.movie.ID);
        }

        // Load Units
        function loadUnits() {
            apiService.get(apiURL +'api/data/searchUnits/', null,
            unitsLoadCompleted,
            unitsLoadFailed);
        }

        function unitsLoadCompleted(response) {
            $scope.units = response.data;
        }

        function unitsLoadFailed(response) {
            notificationService.displayError(response.data);
            console.log(data);
        }

        // Load TT
        function loadTT() {
            apiService.get(apiURL +'api/data/searchTaxMaster/', null,
            ttLoadCompleted,
            ttLoadFailed);
        }

        function ttLoadCompleted(response) {
            $scope.quotation.QuotationTaxes = response.data;
        }

        function ttLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function getTT() {
            //console.log($scope.quotation.Taxes.TaxName);
        }

        loadUnits();
        loadTT();
    }

})(angular.module('ace'));