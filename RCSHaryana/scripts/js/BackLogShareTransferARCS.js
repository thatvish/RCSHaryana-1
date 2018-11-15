
//$(document).ready(function () {
//    loadData();
//});

//function BindSocietyMemberOccupation() {
//    $.getJSON("/Society/GetSocietyMemberOccupation",
//    function (data) {
//        var select = $("#OccupationOfMember");
//        select.empty();
//        select.append($('<option/>', {
//            value: 0,
//            text: "Select"
//        }));
//        $.each(data, function (index, itemData) {
//            select.append($('<option/>', {
//                value: itemData.Value,
//                text: itemData.Text
//            }));
//        });
//    });
//}

//function BindSocietyMemberRelationshipWithNominee() {
//    $.getJSON("/Society/BindSocietyMemberRelationshipWithNominee",
//    function (data) {
//        var select = $("#RelationshipCodeOfSocietyMember");
//        select.empty();
//        select.append($('<option/>', {
//            value: 0,
//            text: "Select"
//        }));
//        $.each(data, function (index, itemData) {
//            select.append($('<option/>', {
//                value: itemData.Value,
//                text: itemData.Text
//            }));
//        });
//    });
//}

////Load Data function
//function loadData() {
//    //debugger;
//    var FindLength = 0;

//    $.ajax({
//        url: "/BackLog/ShareMembersList",
//        type: "GET",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        success: function (result) {        
//            var NoofMember = result.TotalCount;
//            FindLength = result.getRecord.length;
//            var html = '';
//            var i = 0;
//            $.each(result.getRecord, function (key, item) {
//                i = i + 1;
//                html += '<tr>';
//                html += '<td hidden="hidden">' + item.MemberSNo + '</td>';
//                html += '<td>' + i + '</td>';
//                //html += '<td><img src="' + item.fullpath + '"/></td>';
//                html += '<td>' + item.MemberName + '</td>';
//                html += '<td>' + item.FatherName + '</td>';
//                html += '<td>' + item.ShareTransferApprovalDate + '</td>';            
//                html += '<td><a href="#" onclick="return getbyShareTransferD(' + item.ShareTransferID + ')">Edit</a> | <a href="#" onclick="Delele(' + item.ShareTransferID + ')">Delete</a></td>';
//                html += '</tr>';
//            });
//            $('.tbody1').html(html);
//            //if (FindLength === NoofMember) {
//            //    $("#AddMemberDetails").hide();
//            //    $('#AddManagingCommitteMembersShareTrans').hide();
//            //    $("#div1").addClass("alert alert-warning");

//            //    //$("#div1").Removeclass("alert alert-success");
//            //}
//            //else {
//            //    $("#AddMemberDetails").show();
//            //    $('#AddManagingCommitteMembersShareTrans').show();
//            //    $("#div1").removeClass("alert alert-warning");
//            //    $("#div1").addClass("alert alert-success");
//            //}
//            //$("#div1").html("Total Added Member " + FindLength);
//            //refreshPage();
//            //loadDataOfManagingCommitteMember();
//        },
//        error: function (errormessage) {
//            //alert(errormessage.responseText);
//        }
//    });
//}

//var base64;
//function returnBase64() {
//    var selectedFile = document.getElementById('files_10').files;
//    var fileToLoad = selectedFile[0];
//    var fileReader = new FileReader();

//    var filesubstring;
//    fileReader.onload = function (fileLoadedEvent) {
//        base64 = fileLoadedEvent.target.result;
//        alert(base64);
//    };
//    fileReader.readAsDataURL(fileToLoad);
//}

//Add Data Function 




//function Add() {
//    var res = ShareValidate();
//    if (res === false) {
//        //alert("Please Fill the mandatory field first");
//        return false;
//    }
//    var date = $('#datepicker').datepicker('getDate');
//    var date2 = $('#datepicker').val();  
//    if (date === "" || date === null) {
//        $('#datepicker').val("");
//    }
//    else {
//        var getdate = date.getDate();
//        var month = date.getMonth() + 1;
//        var fulldate = getdate + '/' + month + '/' + date.getFullYear();
//    }
//    var objMFD = {
//        //dob: $('#datepicker').val(),
//        dob:$('#datepicker').val(),
//        flfile: $('#files_10').val(),
//        Fullpath: $('#files_10').text(),
//        MemberName: $('#MemberName').val(),
//        FatherName: $('#FatherName').val(),
//        Gender: $('#GenderOfSocietyMember').val(),
//        Age: $('#Age').val(),
//        OccupationVal: $('#OccupationOfMember').val(),
//        Address1: $('#Address1').val(),
//        Address2: $('#Address2').val(),
//        PostOffice: $('#PostOfficeOfSocietyMember').val(),
//        Pin: $('#PostalCodeOfSocietyMember').val(),
//        DistCode: $('#DistrictOfMember').val(),
//        NoOfShares: $('#NoofSharesSubscribed').val(),
//        NomineeName: $('#NameofNominee').val(),
//        NomineeAge: $('#NomineeAge').val(),
//        RelationshipCode: $('#RelationshipCodeOfSocietyMember').val(),
//        Mobile: $('#MobileNumberOfSocietyMember').val(),
//        AadharNo: $('#AadharNo1').val(),
//        EmailId: $('#EmailId').val(),
//        ShareTransferApprovalDate: $('#Approvaldatetime').val(),
//        ShareTransferAppLetterNo: $('#ApprovalLetterNo').val(),
//        MemberId: $('#MemberId').val(),
//        NewMemberName: $('#NEWMemberName').val(),
//        ExistingMemberName: $("#MemberId :selected").text()
        
//    };
//    $.ajax({
//        url: "/BackLog/SaveShareTransfer",
//        data: JSON.stringify(objMFD),
//        type: "POST",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            loadData();
//            clearTextBoxforshare();
//            $('#myModal').modal('hide');
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });
//}

//Function for getting the Data Based upon Employee ID


function getbyShareTransferD(ShareTransferID) {
    $.ajax({
        url: "/BackLog/getbyShareTransferMemberID",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: { ShareTransferID },
        success: function (result) {
            //alert(JSON.stringify(result));
            var getbySocietyMemberID = result[0];
            // alert(getbySocietyMemberID.imgsrc);
            if (getbySocietyMemberID.dob === "") {
                $('#datepicker').val("");
            }
            else {
                //    var newvalue = getbySocietyMemberID.dob;
                //var date = new Date(parseInt(newvalue.substr(6)));
                //var month = date.getMonth() + 1;
                //var fulldate = date + '/' + month + '/' + date.getFullYear();
                $('#datepicker').val(getbySocietyMemberID.Dob);

            }
            //debugger;   
            $('#showCOR1').css('display', 'block');
            $('#MemberSNo').val(getbySocietyMemberID.MemberSNo);
            $('#MemberName').val(getbySocietyMemberID.MemberName);
            $('#FatherName').val(getbySocietyMemberID.FatherName);
            $('#GenderOfSocietyMember').val(getbySocietyMemberID.Gender);
            $('#img').attr('src', getbySocietyMemberID.Fullpath);
            $('#files_101').text(getbySocietyMemberID.Fullpath);
            $('#img1').val(getbySocietyMemberID.imgg);
            // $('#files_10').val(getbySocietyMemberID.fullpath)
            //$('#files_10').val(getbySocietyMemberID.flfile);
            var Age = getbySocietyMemberID.Age;
            if (Age === 0) {
                $('#Age').val("");
            }
            else {
                $('#Age').val(Age);
            }
            $('#dateofresolution').val(getbySocietyMemberID.DateofResolution);
            $('#FirstShareTrans').val(getbySocietyMemberID.FirstShareTrans);
            if (getbySocietyMemberID.FirstShareTrans == "true") {
                $('#FirstShareTrans').prop('checked', true);
                $("#ApprovalLetterNo").val("");
                $("#Approvaldatetime").val("");
                $("#ApprovalLetterNo").attr("readonly", "readonly");
                $("#ApprovalLetterNo").attr("Placeholder", "No Need To Fill");
                $("#Approvaldatetime").datepicker("destroy");
                $("#Approvaldatetime").attr("Placeholder", "No Need To Fill");
            }
            else {
                $('#FirstShareTrans').prop('checked', false);
            }
            $('#files_15').text(getbySocietyMemberID.CopyOfResolution); 
            var OccupationOfMember = getbySocietyMemberID.OccupationVal;
            if (OccupationOfMember === 0) {
                $('#OccupationOfMember').val("");
            }
            else {
                $('#OccupationOfMember').val(OccupationOfMember);
            }

            $('#Address1').val(getbySocietyMemberID.Address1);
            $('#Address2').val(getbySocietyMemberID.Address2);
            $('#PostOfficeOfSocietyMember').val(getbySocietyMemberID.PostOffice);
            var PostalCodeOfSocietyMember = getbySocietyMemberID.Pin;
            if (PostalCodeOfSocietyMember === 0) {
                $('#PostalCodeOfSocietyMember').val("");
            }
            else {
                $('#PostalCodeOfSocietyMember').val(PostalCodeOfSocietyMember);
            }
            $('#DistrictOfMember').val(getbySocietyMemberID.DistCode);
            var NoofSharesSubscribed = getbySocietyMemberID.NoOfShares;
            if (NoofSharesSubscribed === 0) {
                $('#NoofSharesSubscribed').val("");
            }
            else {
                $('#NoofSharesSubscribed').val(NoofSharesSubscribed);
            }
            $('#NameofNominee').val(getbySocietyMemberID.NomineeName);
            var NomineeAge = getbySocietyMemberID.NomineeAge;
            if (NomineeAge === 0) {
                $('#NomineeAge').val("");
            }
            else {
                $('#NomineeAge').val(NomineeAge);
            }
            var RelationshipCodeOfSocietyMember = getbySocietyMemberID.RelationshipCode;
            if (RelationshipCodeOfSocietyMember === 0) {
                BindSocietyMemberRelationshipWithNominee();
            }
            else {
                $('#RelationshipCodeOfSocietyMember').val(RelationshipCodeOfSocietyMember);
            }
            $('#MobileNumberOfSocietyMember').val(getbySocietyMemberID.Mobile);

            $('#ExistingMemberName').val(getbySocietyMemberID.ExistingMemberName);      
            $('#Approvaldatetime').val(getbySocietyMemberID.ShareTransferApprovalDate);
            $('#ApprovalLetterNo').val(getbySocietyMemberID.ShareTransferAppLetterNo);
            $('#MemberId').val(getbySocietyMemberID.OldMemberId);
            $('#sharetransferId').val(getbySocietyMemberID.ShareTransferID);

            $('#AadharNo1').val(getbySocietyMemberID.AadharNo);
            $('#EmailId').val(getbySocietyMemberID.EmailId);
            $('#myModal').modal('show');
            $('#btnUpdate22').show();
            $('#btnAdd22').hide();
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
        
    });
    return false;
}

//function for updating employee's record
function UpdatebyARCS() {   
    //debugger;
   
    var ShareTransferID1 = $('#sharetransferId').val();
    var res = ShareValidate();
    if (res === false) {
        alert("Please Fill the mandatory field first");
        return false;
    }
    var date = $('#datepicker').datepicker('getDate');
    // alert(date);
    if (date === "" || date === null) {
        $('#datepicker').val("");
    }
    else {
        var getdate = date.getDate();
        var month = date.getMonth() + 1;
        var fulldate = getdate + '/' + month + '/' + date.getFullYear();
    }
    
    if ($('#files_101').val() === "") {
        var objMFD = {
            dob: fulldate,
            flfile: $('#files_101').text(),
            Fullpath: $('#files_101').text(),
            ShareTransferID: $('#sharetransferId').val(),
            MemberName: $('#MemberName1').val(),
            FatherName: $('#FatherName1').val(),
            Gender: $('#GenderOfSocietyMember1').val(),
            Age: $('#Age1').val(),
            OccupationVal: $('#OccupationOfMember1').val(),
            Address1: $('#Address11').val(),
            Address2: $('#Address22').val(),
            PostOffice: $('#PostOfficeOfSocietyMember1').val(),
            Pin: $('#PostalCodeOfSocietyMember1').val(),
            DistCode: $('#DistrictOfMember1').val(),
            NoOfShares: $('#NoofSharesSubscribed1').val(),
            NomineeName: $('#NameofNominee1').val(),
            NomineeAge: $('#NomineeAge1').val(),
            RelationshipCode: $('#RelationshipCodeOfSocietyMember1').val(),
            Mobile: $('#MobileNumberOfSocietyMember1').val(),
            AadharNo: $('#AadharNo11').val(),
            EmailId: $('#EmailId1').val(),
            ShareTransferApprovalDate: $('#Approvaldatetime').val(),
            ShareTransferAppLetterNo: $('#ApprovalLetterNo').val(),
            FirstShareTrans: $('#FirstShareTrans').val(),
            DateofResolution: $('#dateofresolution').val(),
            MemberId: $('#MemberId').val()
        };
    }
    else {     
        objMFD = {
            dob: fulldate,
            flfile: $('#files_101').val(),
            imgg: document.getElementById("b64").innerHTML,
            ShareTransferID: $('#sharetransferId').val(),
            MemberName: $('#MemberName1').val(),
            FatherName: $('#FatherName1').val(),
            Gender: $('#GenderOfSocietyMember1').val(),
            Age: $('#Age1').val(),
            OccupationVal: $('#OccupationOfMember1').val(),
            Address1: $('#Address11').val(),
            Address2: $('#Address22').val(),
            PostOffice: $('#PostOfficeOfSocietyMember1').val(),
            Pin: $('#PostalCodeOfSocietyMember1').val(),
            DistCode: $('#DistrictOfMember1').val(),
            NoOfShares: $('#NoofSharesSubscribed1').val(),
            NomineeName: $('#NameofNominee1').val(),
            NomineeAge: $('#NomineeAge1').val(),
            RelationshipCode: $('#RelationshipCodeOfSocietyMember1').val(),
            Mobile: $('#MobileNumberOfSocietyMember1').val(),
            AadharNo: $('#AadharNo11').val(),
            EmailId: $('#EmailId1').val(),
            ShareTransferApprovalDate: $('#Approvaldatetime').val(),
            ShareTransferAppLetterNo: $('#ApprovalLetterNo').val(),
            FirstShareTrans: $('#FirstShareTrans').val(),
            DateofResolution: $('#dateofresolution').val(),
            MemberId: $('#MemberId').val()
        };
    }
    $.ajax({
        url: "/BackLog/SaveShareTransfer",
        data: JSON.stringify(objMFD),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            clearTextBoxforshare();
            $('#myModal11').modal('hide');
            alert("Share Transfer Data has been updated")
            Bck_loadShareTransferMembers();
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}

function clearTextBoxforshare() {
    $('#MemberName1').val("");
    $('#FatherName1').val("");
    $('#GenderOfSocietyMember1').val("");
    $('#Age1').val("");
    $('#MobileNumberOfSocietyMember1').val("");
    $('#EmailId1').val("");
    $('#AadharNo11').val("");
    $('#OccupationOfMember1').val("");
    $('#NoofSharesSubscribed1').val("");
    $('#NameofNominee1').val("");
    $('#NomineeAge1').val("");
    $('#RelationshipCodeOfSocietyMember1').val("");
    $('#Address11').val("");
    $('#Address22').val("");
    $('#PostOfficeOfSocietyMember').val("");
    $('#datepicker').val("");
    $('#FirstShareTrans').val(""),
    $('#dateofresolution').val(""),
    $('#files_15').val(""),
    $('#ApprovalLetterNo').val("");
    $('#PostalCodeOfSocietyMember1').val("");
    $('#DistrictOfMember1').val("");
    $('#img1').attr('src', '');
    $('#files_101').val("");
    $('#showCOR1').css('display', 'none');
}
function ShareValidate() {
    var isValid = true;
    if ($('#img1').attr('src') === "") {
        $('#files_101').css('border-color', 'Red');
        isValid = false;
    }
    else {
        //alert($('#files_10').val().trim());
        $('#files_101').css('border-color', 'green');
    }
    if ($('#files_15').text() === "" && $('#files_15').val() === "") {
        $('#files_15').css('border-color', 'Red');
        isValid = false;
    }
    else {
        //alert($('#files_10').val().trim());
        $('#files_15').css('border-color', 'green');
    }
    var date = $('#datepicker').datepicker('getDate');
    // alert(date);
    if (date === "" || date === null) {
        $('#datepicker').val("");
        //$('#datepicker').css('border-color', 'Red');
        //isValid = false;
    }
    else {
        $('#datepicker').css('border-color', 'green');
    }


    if ($('#MemberName1').val().trim() === "") {
        $('#MemberName1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#MemberName1').css('border-color', 'green');
    }

    if ($('#FatherName1').val().trim() === "") {
        $('#FatherName1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#FatherName1').css('border-color', 'green');
    }

    if ($('#GenderOfSocietyMember1 :selected').text() === "Select Gender" || $('#GenderOfSocietyMember1 :selected').text() === "") {
        $('#GenderOfSocietyMember1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#GenderOfSocietyMember1').css('border-color', 'green');
    }

    if ($('#Age1').val().trim() === "") {
        $('#Age1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Age1').css('border-color', 'green');
    }

    if ($('#MobileNumberOfSocietyMember1').val().trim() === "") {
        $('#MobileNumberOfSocietyMember1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#MobileNumberOfSocietyMember1').css('border-color', 'green');
    }
    var name = $('#AadharNo11').val().trim();
    if ($('#AadharNo11').val().trim() === "") {
        $('#AadharNo11').css('border-color', 'green');
        //    isValid = false;
    }
    else if (name.length >= 12) {
        var url = "/Society/ValidateAadharCard";
        $.get(url, { input: name }, function (data) {
            if (data !== "true") {

                $('#AadharNo11').css('border-color', 'Red');
                alert("Aadhar card is not valid");
                isValid = false;
            }
            else {
                // alert("3");
                $('#AadharNo11').css('border-color', 'green');
            }
        });
    }
    else {
        $('#AadharNo11').css('border-color', 'Red');
        alert("Please fill 12 digit of aadhar card");
        $('#AadharNo11').val('');
        $('#AadharNo11').css('border-color', 'green');
        isValid = false;
    }
    if ($('#OccupationOfMember1').val().trim() === "") {
        $('#OccupationOfMember1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#OccupationOfMember1').css('border-color', 'green');
    }

    if ($('#NoofSharesSubscribed1').val().trim() === "") {
        $('#NoofSharesSubscribed1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NoofSharesSubscribed1').css('border-color', 'green');
    }

    if ($('#NameofNominee1').val().trim() === "") {
        $('#NameofNominee1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NameofNominee1').css('border-color', 'green');
    }

    if ($('#NomineeAge1').val().trim() === "") {
        $('#NomineeAge1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NomineeAge1').css('border-color', 'green');
    }

    if ($('#RelationshipCodeOfSocietyMember1 :selected').text() === "Select" || $('#RelationshipCodeOfSocietyMember1 :selected').text() === "") {
        $('#RelationshipCodeOfSocietyMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#RelationshipCodeOfSocietyMember1').css('border-color', 'green');
    }

    if ($('#Address11').val().trim() === "") {
        $('#Address11').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Address11').css('border-color', 'green');
    }

    if ($('#Address22').val().trim() === "") {
        $('#Address22').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Address22').css('border-color', 'green');
    }



    if ($('#PostOfficeOfSocietyMember1').val().trim() === "") {
        $('#PostOfficeOfSocietyMember1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#PostOfficeOfSocietyMember1').css('border-color', 'green');
    }
    var totallength = $('#PostalCodeOfSocietyMember1').val().trim();

    if ($('#PostalCodeOfSocietyMember1').val().trim() === "" || totallength.length < 6) {
        $('#PostalCodeOfSocietyMember1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#PostalCodeOfSocietyMember1').css('border-color', 'green');
    }

    if ($('#DistrictOfMember1 :selected').text() === "Select" || $('#DistrictOfMember1 :selected').text() === "") {
        $('#DistrictOfMember1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#DistrictOfMember1').css('border-color', 'green');
    }
    if ($('#MemberId :selected').text() === "Select" || $('#MemberId :selected').text() === "") {
        $('#MemberId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#MemberId').css('border-color', 'green');
    }
    var Approvaldatetime = $('#Approvaldatetime').datepicker('getDate');
    // alert(date);
    if ($("#FirstShareTrans").val() == "true") {
        $('#Approvaldatetime').css('border-color', 'green');
    }
    else {
        if (Approvaldatetime === "" || Approvaldatetime === null) {
            $('#Approvaldatetime').val("");
            $('#Approvaldatetime').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#Approvaldatetime').css('border-color', 'green');
        }
    }
    var dateofresolution = $('#dateofresolution').datepicker('getDate');
    // alert(date);
    if (dateofresolution === "" || dateofresolution === null) {
        $('#dateofresolution').val("");
        $('#dateofresolution').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#dateofresolution').css('border-color', 'green');
    }
    if ($("#FirstShareTrans").val() == "true") {
        $('#ApprovalLetterNo').css('border-color', 'green');
    }
    else {
        if ($('#ApprovalLetterNo').val().trim() === "" || totallength.length < 6) {
            $('#ApprovalLetterNo').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#ApprovalLetterNo').css('border-color', 'green');
        }
    }
    if (isValid == true) {
        $('#btnUploadCpy').attr("disabled", false)
        $('#btnUploadCpy').trigger('click');

    }
    return isValid;
}