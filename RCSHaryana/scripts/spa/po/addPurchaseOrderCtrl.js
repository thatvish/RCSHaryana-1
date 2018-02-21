(function (app) {
    'use strict';

    app.controller('addPurchaseOrderCtrl', addPurchaseOrderCtrl);

    addPurchaseOrderCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService', '$modal', '$sce'];

    function addPurchaseOrderCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService, $modal, $sce) {

        $scope.Terms = [];
        $scope.units = [];
        $scope.tt = [];
        $scope.po = {};

        $scope.po.POItems = new Array(3);
        $scope.po.POItems = [
            { "ID": 1, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": 0.00, "Discount": 0.00, "Amount": 0.00 },
            { "ID": 2, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": 0.00, "Discount": 0.00, "Amount": 0.00 },
            { "ID": 3, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": 0.00, "Discount": 0.00, "Amount": 0.00 }];

        $scope.po.AdditionalInfo = [
            { "ID": 1, "InfoLabel": "", "InfoPercent" : 0.00, "InfoValue": 0.00 }];

        $scope.po.TotalPrice = 0.00;
        $scope.Taxes = 0.00;
        $scope.po.POTaxes = [];

        $scope.AddPO = AddPO;
        $scope.AddMore = AddMore;
        $scope.RemoveItem = RemoveItem;

        $scope.AddMoreInfo = AddMoreInfo;
        $scope.RemoveInfo = RemoveInfo;

        $scope.calcAmount = calcAmount;
        $scope.prepareFiles = prepareFiles;
        $scope.openDatePicker = openDatePicker;
        $scope.openDatePicker2 = openDatePicker2;
        $scope.openDatePicker3 = openDatePicker3;
        $scope.openDatePicker4 = openDatePicker4;

        // Auto Complete events
        $scope.selectBeneficiary = selectBeneficiary;
        $scope.selectShipTo = selectShipTo;
        $scope.selectBillTo = selectBillTo;
        $scope.selectAI = selectAI;
        $scope.selectionChangedAI = selectionChangedAI;
        $scope.selectPOItem = selectPOItem;
        $scope.numToWords = numToWords();
        $scope.isEnabled = false;
        $scope.directMode = true;
        

        //$scope.addedItem
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};
        $scope.datepicker2 = {};
        $scope.datepicker3 = {};
        $scope.datepicker4 = {};

        var attcahedDoc = [];

        $scope.openEditDialog = openEditDialog;

        function selectBillTo($item) {
            //console.log($item);
            $scope.po.BillTo = $item.originalObject.Address;
        }

        function selectShipTo($item) {
            //console.log($item);
            $scope.po.ShipTo = $item.originalObject.Address;
        }

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

        function selectAI() {
        }

        function selectionChangedAI() {
        }

        function calcAmount(indx) {
            var list = $scope.po.POItems;

            if (indx < list.length) {
                list[indx].Amount = (list[indx].Quantity * list[indx].Rate) - ((list[indx].Discount / 100) * list[indx].Quantity * list[indx].Rate);

                $scope.po.SubTotal = 0.00;
                $scope.po.TotalPrice = 0.00;
                var addAmount = 0.00;
                var amount = 0.00;

                for (var ctr = 0; ctr < list.length; ctr++) {
                    if (list[ctr].ItemName.length > 0) {
                        amount += parseFloat(list[ctr].Amount);
                    }
                }

                var addlInfo = $scope.po.AdditionalInfo;
                for (var ctr = 0; ctr < addlInfo.length; ctr++) {
                    if (parseFloat(addlInfo[ctr].InfoPercent) > 0.00) {
                        addlInfo[ctr].InfoValue = parseFloat((addlInfo[ctr].InfoPercent / 100) * amount);
                    }
                    addAmount = addAmount + parseFloat(addlInfo[ctr].InfoValue);
                    //console.log(addAmount);
                }
                $scope.po.AdditionalInfo = addlInfo;
                $scope.po.SubTotal = parseFloat(amount).toFixed(2);
                $scope.po.TotalPrice = parseFloat(amount + addAmount).toFixed(2);
                numToWords();
            }
        }

        function numToWords() {
            apiService.get(apiURL + 'api/Data/numToWords?number=' + parseInt($scope.po.TotalPrice), null,
               function (result) {
                   $scope.po.PriceInWords = result.data;
               }, null);
        }

        function selectBeneficiary($item) {
            if ($item) {
                //console.log($item.originalObject.ID);
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
                //console.log($item2.originalObject.ID);
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
                //console.log($item2.originalObject.ID);
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
            var list = $scope.po.AdditionalInfo;
            //console.log($scope.po.AdditionalInfo);
            for (var c = 0; c < list.length; c++) {
                if ($scope.po.AdditionalInfo[c].InfoLabel.length == 0) {
                    $scope.po.AdditionalInfo[c].InfoLabel = result.data.InfoLabel;
                    $scope.po.AdditionalInfo[c].InfoPercent = result.data.InfoPercent;
                    $scope.po.AdditionalInfo[c].InfoValue = result.data.InfoValue;

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
            //console.log(result.data.QuotationRefNumber);
            //console.log(result.data.QuotationDate);
            $scope.po.BeneficiaryName = result.data.Name;
            $scope.po.BeneficiaryAddress = result.data.Address;
            $scope.po.BeneficiaryPhone = result.data.Phone;
            $scope.po.BeneficiaryMobile = result.data.Mobile;
            $scope.po.BeneficiaryAlternateNo = result.data.AlternateNo;
            $scope.po.BeneficiaryEmail = result.data.Email;
            $scope.po.QuotationNo = result.data.QuotationRefNumber;
            $scope.po.QuotationDate = result.data.QuotationDate;

            $scope.po.TermsConditionsConsignee = " ";
            $scope.po.TermsConditionsConsigner = " ";

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
            for (var c = 0; c < list.length; c++) {
                if ($scope.po.POItems[c].ItemName.length == 0) {
                    $scope.po.POItems[c].ItemCode = result.data.ItemCode;
                    $scope.po.POItems[c].ItemName = result.data.ItemName;
                    $scope.po.POItems[c].Description = result.data.Description;
                    $scope.po.POItems[c].Rate = result.data.Rate;
                    $scope.po.POItems[c].Unit = result.data.Unit;
                    break;
                }
                calcAmount(c);
                numToWords();
            }
        }

        // Item details FAIL
        function itemLoadFailed(response) {
            notificationService.displayError(response.data);
            $scope.isEnabled = false;
        }

        function AddPO() {
            console.log($scope.po);
            AddPOModel();
        }

        function AddMore() {
            var itemsLength = $scope.po.POItems.length;
            var id = itemsLength + 1;
            $scope.po.POItems.push({ "ID": id, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": "", "Discount": "", "Amount": "" });
        }

        function RemoveItem(idx) {
            $scope.po.POItems.splice(idx, 1);
            calcAmount(idx);
        }

        function AddMoreInfo() {
            var infoLength = $scope.po.AdditionalInfo.length;
            var id = infoLength + 1;
            $scope.po.AdditionalInfo.push({ "ID": id, "InfoLabel": "", "InfoPercent": 0.00, "InfoValue": 0.00 });
        }

        function RemoveInfo(idx) {
            $scope.po.AdditionalInfo.splice(idx, 1);
            calcAmount(idx);
        }

        function AddPOModel() {
            apiService.post(apiURL + 'api/data/addPO', $scope.po,
            addPOSucceded,
            addPOFailed);
        }

        function prepareFiles($files) {
            //attcahedDoc = $files;
            attcahedDoc.push($files);
        }

        function addPOSucceded(response) {

            $scope.po.ID = response.data.ID;
            notificationService.displaySuccess('PO sucessfully added');
            $scope.movie = response.data;

            if (attcahedDoc) {
                fileUploadService.uploadPO(attcahedDoc, $scope.po.ID, redirectToEdit);
            }
            else
                redirectToEdit();
        }

        function addPOFailed(response) {
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

        function openDatePicker4($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker4.opened = true;
        };

        function redirectToEdit() {
            notificationService.displayError('Unable to upload attachment.');
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
            console.log(data);
        }

        /*************** Added Later ***************/
        // Prefill details
        var qid = getUrlParameter('id');
        if (qid != "undefined") {
            if (qid > 0) {
                loadPODetails();
                $scope.directMode = false;
            }
        }

        function loadPODetails() {
            $scope.loadingInvoice = true;
            apiService.get(apiURL + 'api/Data/quotationDetails/' + getUrlParameter('id'), null,
            poLoadCompleted,
            poLoadFailed);
        }

        function poLoadCompleted(result) {
            //$scope.po = result.data;
            $scope.po.BeneficiaryName = result.data.BeneficiaryName;
            $scope.po.BeneficiaryAddress = result.data.BeneficiaryAddress;
            $scope.po.BillTo = result.data.BeneficiaryAddress;
            $scope.po.ShipTo = result.data.BeneficiaryAddress;
            $scope.po.BeneficiaryPhone = result.data.BeneficiaryPhone;
            $scope.po.BeneficiaryMobile = result.data.BeneficiaryMobile;
            $scope.po.BeneficiaryAlternateNo = result.data.BeneficiaryAlternateNo;
            $scope.po.BeneficiaryEmail = result.data.BeneficiaryEmail;
            $scope.po.QuotationNo = result.data.QuotationRefNumber;
            $scope.po.QuotationDate = result.data.QuotationDate;

            var date = new Date();
            $scope.po.PODate = date.getFullYear() + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + ('0' + date.getDate()).slice(-2);

            $scope.po.POItems = new Array(0);
            $scope.isEnabled = true;
            // Items
            for (var x = 0; x < result.data.QuotationItems.length; x++) {
                $scope.po.POItems.push({
                    "ID": x + 1,
                    "ItemCode": result.data.QuotationItems[x].ItemCode,
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
            $scope.po.AdditionalInfo = new Array(0);
            for (var x = 0; x < result.data.AdditionalInfo.length; x++) {
                $scope.po.AdditionalInfo.push({ "ID": x + 1, "InfoLabel": result.data.AdditionalInfo[x].InfoLabel, "InfoValue": result.data.AdditionalInfo[x].InfoValue });
            }
            calcAmount(0);
        }

        function poLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        loadUnits();
    }

})(angular.module('ace'));