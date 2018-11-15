function BindAllSocietyDetails() {
    loadSocietyDetails();
}
function BindArcsCode(divCode) {
    $.getJSON("/Account/BindARCSOffice", { DistrictCode: divCode },
       function (data) {
           var select = $("#ARCSOffice");
           select.empty();
           $.each(data, function (index, itemData) {
               select.append($('<option/>', {
                   value: itemData.Value,
                   text: itemData.Text
               }));
           });
       });
}

function SetArcsValue2(ARCSCode) {
    $.getJSON("/Society/BindARCSOfficeById", { ARCSCode: ARCSCode },
           function (data) {
               var select = $("#ARCSOffice");
               select.empty();
               $.each(data, function (index, itemData) {
                   select.append($('<option/>', {
                       value: itemData.Value,
                       text: itemData.Text
                   }));
               });
           });
}
function loadSocietyDetails() {
    //debugger;
    var e = document.getElementById("SocietyList");
    var SocietyTransID = e.options[e.selectedIndex].value;
    $.ajax({
        url: "/Society/getSocietyDetails/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: { SocietyTransID },
        success: function (result) {
            var getbySociety = result[0];
            var status = $('#delete').val();
            if (status === "1") {
                $('#Step1').attr('disabled', 'disabled');
            }
            else
            {
                $('#Step1').removeAttr("disabled");
            }         
            $('#Step1').html("UPDATE");
            $('#District').val(getbySociety.DivCode);
            SetArcsValue2(getbySociety.ARCSCode);
            $('#NameofProposedSociety').val(getbySociety.SocietyName);
            var value = result[0].AreaOfOperation;
            $('#AreaofOperation').val(value);
            $('#ClassofSocietyandLiability').val(getbySociety.SocietyClassName);
            $('#SubClassOfSociety').val(getbySociety.SocietySubClassName);
            $('#RegisteredAddress').val(getbySociety.Address1);
            $('#HouseNoSectorNoRoad').val(getbySociety.Address2);
            $('#PostOffice').val(getbySociety.PostOffice);
            $('#PostalCode').val(getbySociety.Pin);
            $('#Mainobjects1').val(getbySociety.Mainobject1);
            $('#Mainobjects2').val(getbySociety.Mainobject2);
            $('#Mainobjects3').val(getbySociety.Mainobject3);
            $('#Mainobjects4').val(getbySociety.Mainobject4);
            $('#Noofmemberspresent').val(getbySociety.NoOfMembers);
            $('#Categoryofsociety').val(getbySociety.CateOfSociety);
            $('#Estimatedunsecureddebtsofmembers').val(getbySociety.DebtsOfMembers);
            $('#AreaMortgagedbymembers').val(getbySociety.AreaMortgaged);
            $('#detailsofshares').val(getbySociety.DetailsOfShares);
            $('#Valueofashare').val(getbySociety.ValueOfShare);
            $('#ModeofPayment').val(getbySociety.ModeOfPayment);
            $('#Name1').val(getbySociety.Name1);
            $('#FatherName1').val(getbySociety.FatherName1);
            $('#Mobile1').val(getbySociety.Mobile1);
            $('#Email1').val(getbySociety.Email1);
            $('#Address3').val(getbySociety.Address3);
            $('#HouseNoSectorNoRoad1').val(getbySociety.HouseNoSectorNoRoad1);
            $('#PostOffice1').val(getbySociety.PostOffice1);
            $('#PostalCode1').val(getbySociety.PostalCode1);
            $('#DistrictForUser1').val(getbySociety.DistrictForUser1);
            $('#ARCSOffice').val(getbySociety.ARCSCode);
            $('#CustodianMemberId').val(getbySociety.MemberSNo);

            if (getbySociety.ShareMoney !== 0 && getbySociety.AdmissionFees !== 0)
            {
                $('#btnAddFee').html("UPDATE");
            }
            $('#ShareMoney').val(getbySociety.ShareMoney);
            $('#AdmissionFee').val(getbySociety.AdmissionFees);
            $('#Deposits').val(getbySociety.Deposits);

            $('#datepicker1').val(getbySociety.MeetingDate);
            $('#Bank').val(getbySociety.BankName);
            if (getbySociety.Bank !== "Select Bank") {
                $('#btnSaveFormC1').html("UPDATE");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}