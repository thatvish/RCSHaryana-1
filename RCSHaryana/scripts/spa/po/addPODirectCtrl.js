(function (app) {
    'use strict';

    app.controller('addPODirectCtrl', addPODirectCtrl);

    addPODirectCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService', '$modal'];

    function addPODirectCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService, $modal) {

        $scope.units = [];
        $scope.tt = [];
        $scope.po = {};

        $scope.po.POItems = new Array(3);
        $scope.po.POItems = [
            { "ID": 1, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": "", "Discount": 0, "Amount": "" },
            { "ID": 2, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": "", "Discount": 0, "Amount": "" },
            { "ID": 3, "ItemCode": "", "ItemName": "", "Description": "", "Quantity": 1, "Unit": "", "Rate": "", "Discount": 0, "Amount": "" }];

        $scope.po.AdditionalInfo = [
            { "ID": 1, "InfoLabel": "", "InfoValue": 0 }];

        $scope.po.TotalPrice = 0;
        $scope.Taxes = 0;
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
        $scope.getTT = getTT;

        // Auto Complete events
        $scope.selectBeneficiary = selectBeneficiary;
        $scope.selectionChanged = selectionChanged;
        $scope.isEnabled = false;

        $scope.selectAI = selectAI;
        $scope.selectionChangedAI = selectionChangedAI;

        $scope.selectPOItem = selectPOItem;
        $scope.selectionChangedQ = selectionChangedQ;
        $scope.numToWords = numToWords();

        //$scope.addedItem
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};
        $scope.datepicker2 = {};
        $scope.datepicker3 = {};

        var attcahedDoc = [];

        function selectAI() {
        }

        function selectionChangedAI() {
        }

        function calcAmount(indx) {
            var list = $scope.po.POItems;

            if (indx < list.length) {
                list[indx].Amount = (list[indx].Quantity * list[indx].Rate) - ((list[indx].Discount / 100) * list[indx].Quantity * list[indx].Rate);

                $scope.po.SubTotal = 0;
                $scope.po.TotalPrice = 0;
                //var taxAmount = 0;
                var addAmount = 0;
                var amount = 0;

                for (var ctr = 0; ctr < list.length; ctr++) {
                    if (list[ctr].ItemName.length > 0) {
                        amount += parseInt(list[ctr].Amount);
                    }
                }

                //console.log('Amount='+amount);
                //var txs = $scope.po.POTaxes;
                //for (var ctr = 0; ctr < txs.length; ctr++) {
                //    taxAmount = taxAmount + ((txs[ctr].TaxPercent / 100) * amount);
                //}

                var addlInfo = $scope.po.AdditionalInfo;
                for (var ctr = 0; ctr < addlInfo.length; ctr++) {
                    addAmount = addAmount + ((addlInfo[ctr].InfoValue / 100) * amount);
                }

                $scope.po.SubTotal = amount;
                $scope.po.TotalPrice = $scope.po.SubTotal + addAmount;
                numToWords();
            }
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
            var list = $scope.po.AdditionalInfo;
            console.log($scope.po.AdditionalInfo);
            for (var c = 0; c < list.length; c++) {
                if ($scope.po.AdditionalInfo[c].InfoLabel.length == 0) {
                    $scope.po.AdditionalInfo[c].InfoLabel = result.data.InfoLabel;
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
            //$scope.po = result.data;
            $scope.po.BeneficiaryName = result.data.Name;
            $scope.po.BeneficiaryAddress = result.data.Address;
            $scope.po.BeneficiaryPhone = result.data.Phone;
            $scope.po.BeneficiaryMobile = result.data.Mobile;
            $scope.po.BeneficiaryAlternateNo = result.data.AlternateNo;
            $scope.po.BeneficiaryEmail = result.data.Email;

            //$scope.po.InFavorOf = "MG Contractors Pvt Ltd";//result.data.Name;
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
            $scope.po.AdditionalInfo.push({ "ID": id, "InfoLabel": "", "InfoValue": 0 });
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
                fileUploadService.uploadAttachment(attcahedDoc, $scope.po.ID, redirectToEdit);
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

        function redirectToEdit() {
            //notificationService.displayError('Unable to upload attachment.');
            //$location.url(apiURL + 'po/edit/' + $scope.movie.ID);
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

        // Load TT
        function loadTT() {
            apiService.get(apiURL + 'api/data/searchTaxMaster/', null,
            ttLoadCompleted,
            ttLoadFailed);
        }

        function ttLoadCompleted(response) {
            $scope.po.POTaxes = response.data;
        }

        function ttLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function getTT() {
            //console.log($scope.po.Taxes.TaxName);
        }

        loadUnits();
        loadTT();
    }

})(angular.module('ace'));