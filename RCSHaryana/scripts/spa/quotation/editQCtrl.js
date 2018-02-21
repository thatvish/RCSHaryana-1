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
            { "ID": 1, "InfoLabel": "", "InfoPercent" : 0.00, "InfoValue": 0.00 }];

        $scope.Taxes = 0;
        $scope.quotation.QuotationTaxes = [];
        $scope.quotation.TotalPrice = 0;
        $scope.Terms = [];

        $scope.editQ = editQ;
        $scope.AddMore = AddMore;
        $scope.RemoveItem = RemoveItem;

        $scope.AddMoreInfo = AddMoreInfo;
        $scope.RemoveInfo = RemoveInfo;

        $scope.calcAmount = calcAmount;
        $scope.prepareFiles = prepareFiles;
        $scope.openDatePicker = openDatePicker;
        $scope.openDatePicker2 = openDatePicker2;

        // Auto Complete events
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
            $scope.quotation.ID = result.data.ID;
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

        function editQ() {
            editQModel();
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
            $scope.quotation.AdditionalInfo.push({ "ID": id, "InfoLabel": "", "InfoPercent":0.00, "InfoValue": 0.00});
        }

        function RemoveInfo(idx) {
            $scope.quotation.AdditionalInfo.splice(idx, 1);
            calcAmount(idx);
        }

        function editQModel() {
            apiService.post(apiURL + 'api/data/editQ', $scope.quotation,
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
            //console.log(data);
        }

        // Prefill details
        var qid = getUrlParameter('id');
        if (qid != "undefined") {
            if (qid > 0) {
                loadQDetails();
                console.log(qid);
            }
        }

        function loadQDetails() {
            apiService.get(apiURL + 'api/Data/quotationDetails/' + getUrlParameter('id'), null,
            qLoadCompleted,
            qLoadFailed);
        }

        function qLoadCompleted(result) {
            console.log(result.data)
            $scope.quotation.ID = result.data.ID;
            $scope.quotation.BeneficiaryName = result.data.BeneficiaryName;
            $scope.quotation.BeneficiaryAddress = result.data.BeneficiaryAddress;
            $scope.quotation.BeneficiaryPhone = result.data.BeneficiaryPhone;
            $scope.quotation.BeneficiaryMobile = result.data.BeneficiaryMobile;
            $scope.quotation.BeneficiaryAlternateNo = result.data.BeneficiaryAlternateNo;
            $scope.quotation.BeneficiaryEmail = result.data.BeneficiaryEmail;
            $scope.quotation.QuotationRefNumber = result.data.QuotationRefNumber;
            $scope.quotation.QuotationDate = result.data.QuotationDate;
            $scope.quotation.InFavorOf = result.data.InFavorOf;
            $scope.quotation.QuotationDueDate = result.data.QuotationDueDate;
            $scope.quotation.TermsConditionsConsignee = result.data.TermsConditionsConsignee;
            $scope.quotation.TermsConditionsConsigner = result.data.TermsConditionsConsigner;
            document.getElementById('Consignee').innerHTML = result.data.TermsConditionsConsignee;
            document.getElementById('Consigner').innerHTML = result.data.TermsConditionsConsigner;

            $scope.quotation.QuotationItems = new Array(0);
            $scope.isEnabled = true;
            // Items
            for (var x = 0; x < result.data.QuotationItems.length; x++) {
                $scope.quotation.QuotationItems.push({
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
            $scope.quotation.AdditionalInfo = new Array(0);
            for (var x = 0; x < result.data.AdditionalInfo.length; x++) {
                $scope.quotation.AdditionalInfo.push({
                    "ID": x + 1, "InfoLabel": result.data.AdditionalInfo[x].InfoLabel,
                    "InfoPercent": result.data.AdditionalInfo[x].InfoPercent,
                    "InfoValue": result.data.AdditionalInfo[x].InfoValue
                });
            }
            calcAmount(0);
        }

        function qLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        loadUnits();
    }

})(angular.module('ace'));