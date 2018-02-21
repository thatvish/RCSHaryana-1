(function (app) {
    'use strict';

    app.controller('addInvoiceCtrl', addInvoiceCtrl);

    addInvoiceCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService', '$modal', '$sce'];

    function addInvoiceCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService, $modal, $sce) {

        $scope.Terms = [];
        $scope.units = [];
        $scope.tt = [];
        $scope.invoice = {};

        $scope.invoice.InvoiceItems = new Array(3);
        $scope.invoice.InvoiceItems = [
            { "ID": 1, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": 0.00, "Discount": 0.00, "Amount": 0.00 },
            { "ID": 2, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": 0.00, "Discount": 0.00, "Amount": 0.00 },
            { "ID": 3, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": 0.00, "Discount": 0.00, "Amount": 0.00 }];


        $scope.invoice.TotalPrice = 0.00;
        $scope.Taxes = 0.00;
        $scope.invoice.InvoiceTaxes = [];

        $scope.AddInvoice = AddInvoice;
        $scope.AddMore = AddMore;
        $scope.RemoveItem = RemoveItem;

        $scope.AddMoreInfo = AddMoreInfo;
        $scope.RemoveInfo = RemoveInfo;

        $scope.calcAmount = calcAmount;
        $scope.prepareFiles = prepareFiles;

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

        var attcahedDoc = [];

        $scope.openEditDialog = openEditDialog;

        function selectBillTo($item) {
            //console.log($item);
            $scope.invoice.BillTo = $item.originalObject.Address;
        }

        function selectShipTo($item) {
            //console.log($item);
            $scope.invoice.ShipTo = $item.originalObject.Address;
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
            var list = $scope.invoice.InvoiceItems;

            if (indx < list.length) {
                list[indx].Amount = (list[indx].Quantity * list[indx].Rate) - ((list[indx].Discount / 100) * list[indx].Quantity * list[indx].Rate);

                $scope.invoice.SubTotal = 0.00;
                $scope.invoice.TotalPrice = 0.00;
                var addAmount = 0.00;
                var amount = 0.00;

                for (var ctr = 0; ctr < list.length; ctr++) {
                    if (list[ctr].ItemName.length > 0) {
                        amount += parseFloat(list[ctr].Amount);
                    }
                }

                $scope.invoice.SubTotal = parseFloat(amount).toFixed(2);
                $scope.invoice.TotalPrice = parseFloat(amount + addAmount).toFixed(2);
                numToWords();
            }
        }

        function numToWords() {
            apiService.get(apiURL + 'api/Data/numToWords?number=' + parseInt($scope.invoice.TotalPrice), null,
                function (result) {
                    $scope.invoice.PriceInWords = result.data;
                }, null);
        }

        function selectBeneficiary($item) {
            if ($item) {
                //console.log($item.originalObject.ID);
                $scope.invoice.BeneficiaryID = $item.originalObject.ID;

                // Get Beneficiary details by Beneficiary ID
                apiService.get(apiURL + 'api/Data/beneficiaryDetails/' + $scope.invoice.BeneficiaryID, null,
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
            var list = $scope.invoice.AdditionalInfo;
            //console.log($scope.invoice.AdditionalInfo);
            for (var c = 0; c < list.length; c++) {
                if ($scope.invoice.AdditionalInfo[c].InfoLabel.length == 0) {
                    $scope.invoice.AdditionalInfo[c].InfoLabel = result.data.InfoLabel;
                    $scope.invoice.AdditionalInfo[c].InfoPercent = result.data.InfoPercent;
                    $scope.invoice.AdditionalInfo[c].InfoValue = result.data.InfoValue;

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
            $scope.invoice.BeneficiaryName = result.data.Name;
            $scope.invoice.BeneficiaryAddress = result.data.Address;
            $scope.invoice.BeneficiaryPhone = result.data.Phone;
            $scope.invoice.BeneficiaryMobile = result.data.Mobile;
            $scope.invoice.BeneficiaryAlternateNo = result.data.AlternateNo;
            $scope.invoice.BeneficiaryEmail = result.data.Email;
            $scope.invoice.QuotationNo = result.data.QuotationRefNumber;
            $scope.invoice.QuotationDate = result.data.QuotationDate;

            $scope.invoice.TermsConditionsConsignee = " ";
            $scope.invoice.TermsConditionsConsigner = " ";

            $scope.isEnabled = true;
        }

        // Beneficiary details FAIL
        function beneficiaryLoadFailed(response) {
            notificationService.displayError(response.data);
            $scope.isEnabled = false;
        }

        // Item details SUCCESS
        function itemLoadCompleted(result) {
            var list = $scope.invoice.InvoiceItems;
            for (var c = 0; c < list.length; c++) {
                if ($scope.invoice.InvoiceItems[c].ItemName.length == 0) {
                    $scope.invoice.InvoiceItems[c].ItemCode = result.data.ItemCode;
                    $scope.invoice.InvoiceItems[c].ItemName = result.data.ItemName;
                    $scope.invoice.InvoiceItems[c].Description = result.data.Description;
                    $scope.invoice.InvoiceItems[c].Rate = result.data.Rate;
                    $scope.invoice.InvoiceItems[c].Unit = result.data.Unit;
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

        function AddInvoice() {
            console.log($scope.invoice);
            AddInvoiceModel();
        }

        function AddMore() {
            var itemsLength = $scope.invoice.InvoiceItems.length;
            var id = itemsLength + 1;
            $scope.invoice.InvoiceItems.push({ "ID": id, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": "", "Discount": "", "Amount": "" });
        }

        function RemoveItem(idx) {
            $scope.invoice.InvoiceItems.splice(idx, 1);
            calcAmount(idx);
        }

        function AddMoreInfo() {
            var infoLength = $scope.invoice.AdditionalInfo.length;
            var id = infoLength + 1;
            $scope.invoice.AdditionalInfo.push({ "ID": id, "InfoLabel": "", "InfoPercent": 0.00, "InfoValue": 0.00 });
        }

        function RemoveInfo(idx) {
            $scope.invoice.AdditionalInfo.splice(idx, 1);
            calcAmount(idx);
        }

        function AddInvoiceModel() {
            apiService.invoicest(apiURL + 'api/data/addPO', $scope.invoice,
                addPOSucceded,
                addPOFailed);
        }

        function prepareFiles($files) {
            //attcahedDoc = $files;
            attcahedDoc.push($files);
        }

        function addPOSucceded(response) {

            $scope.invoice.ID = response.data.ID;
            notificationService.displaySuccess('PO sucessfully added');
            $scope.movie = response.data;

            if (attcahedDoc) {
                fileUploadService.uploadPO(attcahedDoc, $scope.invoice.ID, redirectToEdit);
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
            //$scope.invoice = result.data;
            $scope.invoice.BeneficiaryName = result.data.BeneficiaryName;
            $scope.invoice.BeneficiaryAddress = result.data.BeneficiaryAddress;
            $scope.invoice.BillTo = result.data.BeneficiaryAddress;
            $scope.invoice.ShipTo = result.data.BeneficiaryAddress;
            $scope.invoice.BeneficiaryPhone = result.data.BeneficiaryPhone;
            $scope.invoice.BeneficiaryMobile = result.data.BeneficiaryMobile;
            $scope.invoice.BeneficiaryAlternateNo = result.data.BeneficiaryAlternateNo;
            $scope.invoice.BeneficiaryEmail = result.data.BeneficiaryEmail;
            $scope.invoice.QuotationNo = result.data.QuotationRefNumber;
            $scope.invoice.QuotationDate = result.data.QuotationDate;

            var date = new Date();
            $scope.invoice.InvoiceDate = date.getFullYear() + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + ('0' + date.getDate()).slice(-2);

            $scope.invoice.InvoiceItems = new Array(0);
            $scope.isEnabled = true;
            // Items
            for (var x = 0; x < result.data.QuotationItems.length; x++) {
                $scope.invoice.InvoiceItems.push({
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
            $scope.invoice.AdditionalInfo = new Array(0);
            for (var x = 0; x < result.data.AdditionalInfo.length; x++) {
                $scope.invoice.AdditionalInfo.push({ "ID": x + 1, "InfoLabel": result.data.AdditionalInfo[x].InfoLabel, "InfoValue": result.data.AdditionalInfo[x].InfoValue });
            }
            calcAmount(0);
        }

        function poLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        loadUnits();
    }

})(angular.module('ace'));