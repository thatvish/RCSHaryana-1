﻿
//Load Data in Table when documents is ready
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

//        url: "/Society/SocietyMembersList",
//        type: "GET",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            //debugger;
//            var NoofMember = result.TotalCount;
//            FindLength = result.getRecord.length;
//            var html = '';
//            var i = 0;
//            $.each(result.getRecord, function (key, item) {
//                i = i + 1;
//                html += '<tr>';
//                html += '<td hidden="hidden">' + item.MemberSNo + '</td>';
//                html += '<td>' + i + '</td>';
//                html += '<td><img src="' + item.Fullpath + '"/></td>';
//                html += '<td>' + item.MemberName + '</td>';
//                html += '<td>' + item.FatherName + '</td>';
//                html += '<td>' + item.Mobile + '</td>';
//                html += '<td>' + item.NomineeName + '</td>';
//                html += '<td><a href="#" onclick="return getbySocietyMemberID(' + item.MemberSNo + ')">Edit</a> | <a href="#" onclick="Delele(' + item.MemberSNo + ')">Delete</a></td>';
//                html += '</tr>';
//            });
//            $('.tbody1').html(html);
//            //if (FindLength === NoofMember) {
//            //    $("#AddMemberDetails").hide();
//            //    $('#AddManagingCommitteMembersBck').hide();
//            //    $("#div1").addClass("alert alert-warning");

//            //    //$("#div1").Removeclass("alert alert-success");
//            //}
//            //else {
//            //    $("#AddMemberDetails").show();
//            //    $('#AddManagingCommitteMembersBck').show();
//            //    $("#div1").removeClass("alert alert-warning");
//            //    $("#div1").addClass("alert alert-success");
//            //}
//          // $("#div1").html("Total Added Member " + FindLength);
//            //refreshPage();
//            loadDataOfManagingCommitteMember();
//        },
//        error: function (errormessage) {
//            //alert(errormessage.responseText);
//        }
//    });
//}

//var base64;
////function returnBase64() {
////    var selectedFile = document.getElementById('files_10').files;
////    var fileToLoad = selectedFile[0];
////    var fileReader = new FileReader();

////    var filesubstring;
////    fileReader.onload = function (fileLoadedEvent) {
////        base64 = fileLoadedEvent.target.result;
////        alert(base64);
////    };
////    fileReader.readAsDataURL(fileToLoad);
////}

////Add Data Function 
//function Add() {

//    //debugger;
//    var res = validate();
//    if (res === false) {
//        //alert("Please Fill the mandatory field first");
//        return false;
//    }
//    var date = $('#datepicker').datepicker('getDate');

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

//        dob: fulldate,
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
//        EmailId: $('#EmailId').val()
//    };
//    $.ajax({
//        url: "/Society/AddSocietyMember",
//        data: JSON.stringify(objMFD),
//        type: "POST",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            loadData();
//            clearTextBoxFormL();
//            $('#myModal1').modal('hide');
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });
//}
////Function for getting the Data Based upon Employee ID
//function getbySocietyMemberID(MemberSNo) {
//   // debugger;
   
//    $.ajax({
//        url: "/Society/getbySocietyMemberID/",
//        type: "GET",
//        contentType: "application/json;charset=UTF-8",
//        dataType: "json",
//        data: { MemberSNo },
//        success: function (result) {
//            var getbySocietyMemberID = result[0];          
//            if (getbySocietyMemberID.dob === "") {
//                $('#datepicker').val("");
//            }
//            else {
//                //    var newvalue = getbySocietyMemberID.dob;
//                //var date = new Date(parseInt(newvalue.substr(6)));
//                //var month = date.getMonth() + 1;
//                //var fulldate = date + '/' + month + '/' + date.getFullYear();
//                $('#datepicker').val(getbySocietyMemberID.Dob);
//            }
           
//            $('#MemberSNo').val(getbySocietyMemberID.MemberSNo);
//            $('#MemberName').val(getbySocietyMemberID.MemberName);
//            $('#FatherName').val(getbySocietyMemberID.FatherName);
//            $('#GenderOfSocietyMember').val(getbySocietyMemberID.Gender);
//            $('#img').attr('src', getbySocietyMemberID.Fullpath);
//            $('#files_10').text(getbySocietyMemberID.Fullpath);
//            $('#img').val(getbySocietyMemberID.imgg);
//            // $('#files_10').val(getbySocietyMemberID.fullpath)
//            //$('#files_10').val(getbySocietyMemberID.flfile);
//            var Age = getbySocietyMemberID.Age;
//            if (Age === 0) {
//                $('#Age').val("");
//            }
//            else {
//                $('#Age').val(Age);
//            }

//            var OccupationOfMember = getbySocietyMemberID.OccupationVal;
//            if (OccupationOfMember === 0) {
//                $('#OccupationOfMember').val("");
//            }
//            else {
//                $('#OccupationOfMember').val(OccupationOfMember);
//            }

//            $('#Address1').val(getbySocietyMemberID.Address1);
//            $('#Address2').val(getbySocietyMemberID.Address2);
//            $('#PostOfficeOfSocietyMember').val(getbySocietyMemberID.PostOffice);
//            var PostalCodeOfSocietyMember = getbySocietyMemberID.Pin;
//            if (PostalCodeOfSocietyMember === 0) {
//                $('#PostalCodeOfSocietyMember').val("");
//            }
//            else {
//                $('#PostalCodeOfSocietyMember').val(PostalCodeOfSocietyMember);
//            }
//            $('#DistrictOfMember').val(getbySocietyMemberID.DistCode);
//            var NoofSharesSubscribed = getbySocietyMemberID.NoOfShares;
//            if (NoofSharesSubscribed === 0) {
//                $('#NoofSharesSubscribed').val("");
//            }
//            else {
//                $('#NoofSharesSubscribed').val(NoofSharesSubscribed);
//            }
//            $('#NameofNominee').val(getbySocietyMemberID.NomineeName);
//            var NomineeAge = getbySocietyMemberID.NomineeAge;
//            if (NomineeAge === 0) {
//                $('#NomineeAge').val("");
//            }
//            else {
//                $('#NomineeAge').val(NomineeAge);
//            }
//            var RelationshipCodeOfSocietyMember = getbySocietyMemberID.RelationshipCode;
//            if (RelationshipCodeOfSocietyMember === 0) {
//                BindSocietyMemberRelationshipWithNominee();
//            }
//            else {
//                $('#RelationshipCodeOfSocietyMember').val(RelationshipCodeOfSocietyMember);
//            }
//            $('#MobileNumberOfSocietyMember').val(getbySocietyMemberID.Mobile);
//            $('#AadharNo1').val(getbySocietyMemberID.AadharNo);
//            $('#EmailId').val(getbySocietyMemberID.EmailId);
//            $('#myModal1').modal('show');
//            $('#btnUpdate11').show();
//            $('#btnAdd11').hide();
//        },
//        error: function (errormessage) {
//            //alert(errormessage.responseText);
//        }
        
//    });
//    return false;
//}

//function for updating employee's record
function LFormUpdatebyARCS() {
   // debugger;
    var res = Bckvalidate();
    if (res === false) {
        //alert("Please Fill the mandatory field first");
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
    
    if ($('#files_10L').val() === "") {
        //alert("enter");
        var objMFD = {
            dob: fulldate,
            flfile: $('#files_10L').text(),
            Fullpath: $('#files_10L').text(),
             //imgg: document.getElementById("b64").innerHTML,
            //today comment - img: $('#img').attr('src'),
            //img: ('#img').attr('src'),
            //imgg: $('#img').val(),
            MemberSNo: $('#MemberSNo').val(),
            MemberName: $('#MemberName').val(),
            FatherName: $('#FatherName').val(),
            ManagingMemberRelationship: $('#BckManagingMemberRelationship').val(),           
            Gender: $('#GenderOfSocietyMember').val(),
            Age: $('#Age').val(),
            OccupationVal: $('#OccupationOfMember').val(),
            Address1: $('#Address1').val(),
            Address2: $('#Address2').val(),
            PostOffice: $('#PostOfficeOfSocietyMember').val(),
            Pin: $('#PostalCodeOfSocietyMember').val(),
            DistCode: $('#DistrictOfMember').val(),
            NoOfShares: $('#NoofSharesSubscribed').val(),
            NomineeName: $('#NameofNominee').val(),
            NomineeAge: $('#NomineeAge').val(),
            RelationshipCode: $('#RelationshipCodeOfSocietyMember').val(),
            Mobile: $('#MobileNumberOfSocietyMember').val(),
            AadharNo: $('#AadharNo1').val(),
            EmailId: $('#EmailId').val()
        };
    }
    else {     
        objMFD = {
            dob: fulldate,
            flfile: $('#files_10L').val(),
            imgg: document.getElementById("b64").innerHTML,
            //imgg: $('#img').val(),
            //img: $('#img').attr('src'),
            MemberSNo: $('#MemberSNo').val(),
            MemberName: $('#MemberName').val(),
            FatherName: $('#FatherName').val(),
            ManagingMemberRelationship: $('#BckManagingMemberRelationship').val(),
            Gender: $('#GenderOfSocietyMember').val(),
            Age: $('#Age').val(),
            OccupationVal: $('#OccupationOfMember').val(),
            Address1: $('#Address1').val(),
            Address2: $('#Address2').val(),
            PostOffice: $('#PostOfficeOfSocietyMember').val(),
            Pin: $('#PostalCodeOfSocietyMember').val(),
            DistCode: $('#DistrictOfMember').val(),
            NoOfShares: $('#NoofSharesSubscribed').val(),
            NomineeName: $('#NameofNominee').val(),
            NomineeAge: $('#NomineeAge').val(),
            RelationshipCode: $('#RelationshipCodeOfSocietyMember').val(),
            Mobile: $('#MobileNumberOfSocietyMember').val(),
            AadharNo: $('#AadharNo1').val(),
            EmailId: $('#EmailId').val(),
            Fullpath: $('#files_10').text()
        };
    }
    //alert(objMFD.dob);
    $.ajax({
        url: "/Society/UpdateSocietyMemberDetail",
        data: JSON.stringify(objMFD),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            clearTextBoxFormL();
            $('#myModalApprovedLForm').modal('hide');
            Bck_loadApprovedLForm();
            Bck_loadDataOfManagingCommitteMember();
            Bck_loadShareTransferMembers();
            alert("Approved L Form Data has been updated")
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);

        }
    });
}

//function for deleting employee's record
function Delele(MemberSNo) {
    var status = $('#offdelete').val();
    var isValid = true;
    if (status === "1") {
        isValid = false;
        alert("You can't delete this entry .");
        return isValid;
    }
    var ans = confirm("The record shall be removed from Lisf of Members. Are you sure?");
    if (ans) {
        var objMFD = {
            MemberSNo: MemberSNo
        };
        $.ajax({
            url: "/Society/DeleteSocietyMember",
            data: JSON.stringify(objMFD),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                loadData();
            },
            error: function (errormessage) {
                //alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes
function clearTextBoxFormL() {
    $('#MemberName').val("");
    $('#FatherName').val("");
    $('#GenderOfSocietyMember').val("");
    $('#Age').val("");
    $('#MobileNumberOfSocietyMember').val("");
    $('#BckManagingMemberRelationship').val("");
    $('#EmailId').val("");
    $('#AadharNo1').val("");
    $('#OccupationOfMember').val("");
    $('#NoofSharesSubscribed').val("");
    $('#NameofNominee').val("");
    $('#NomineeAge').val("");
    $('#RelationshipCodeOfSocietyMember').val("");
    $('#Address1').val("");
    $('#Address2').val("");
    $('#PostOfficeOfSocietyMember').val("");
    $('#datepicker').val("");
    $('#PostalCodeOfSocietyMember').val("");
    $('#DistrictOfMember').val("");
    $('#img').attr('src', '');
    $('#files_10').val("");
}

//Valdidation using jquery
function Bckvalidate() {
    //debugger;
    var isValid = true;
    if ($('#img').attr('src') === "" || $('#img').attr('src') === "0") {
        $('#files_10L').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#files_10L').css('border-color', 'green');
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

    if ($('#BckManagingMemberRelationship').val() != null) {
        if ($('#BckManagingMemberRelationship').val().trim() === "") {
            $('#BckManagingMemberRelationship').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#BckManagingMemberRelationship').css('border-color', 'green');
        }
    }
    else {
        $('#BckManagingMemberRelationship').css('border-color', 'Red');
        isValid = false;
    }
    if ($('#MemberName').val().trim() === "") {
        $('#MemberName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#MemberName').css('border-color', 'green');
    }

    if ($('#FatherName').val().trim() === "") {
        $('#FatherName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#FatherName').css('border-color', 'green');
    }

    if ($('#GenderOfSocietyMember :selected').text() === "Select Gender" || $('#GenderOfSocietyMember :selected').text() === "") {
        $('#GenderOfSocietyMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#GenderOfSocietyMember').css('border-color', 'green');
    }

    if ($('#Age').val().trim() === "") {
        $('#Age').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Age').css('border-color', 'green');
    }

    if ($('#MobileNumberOfSocietyMember').val().trim() === "") {
        $('#MobileNumberOfSocietyMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#MobileNumberOfSocietyMember').css('border-color', 'green');
    }

    //if ($('#EmailId').val().trim() == "") {
    //    $('#EmailId').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#EmailId').css('border-color', 'green');
    //}

    var name = $('#AadharNo1').val().trim();
    if ($('#AadharNo1').val().trim() === "") {
        $('#AadharNo1').css('border-color', 'green');
        //    isValid = false;
    }
    else if (name.length >= 12) {
        var url = "/Society/ValidateAadharCard";
        $.get(url, { input: name }, function (data) {
            if (data !== "true") {

                $('#AadharNo1').css('border-color', 'Red');
                alert("Aadhar card is not valid");
                isValid = false;
            }
            else {
                // alert("3");
                $('#AadharNo1').css('border-color', 'green');
            }
        });
    }
    else {
        $('#AadharNo1').css('border-color', 'Red');
        alert("Please fill 12 digit of aadhar card");
        $('#AadharNo1').val('');
        $('#AadharNo1').css('border-color', 'green');
        isValid = false;
    }
    //if ($('#AadharNo1').val().trim() == "") {
    //    $('#AadharNo1').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#AadharNo1').css('border-color', 'green');
    //}
    if ($('#OccupationOfMember').val().trim() === "") {
       
        $('#OccupationOfMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#OccupationOfMember').css('border-color', 'green');
    }

    //if ($('#OccupationOfMember :selected').text() == "Select" || $('#OccupationOfMember :selected').text() == "") {
    //    $('#OccupationOfMember').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#OccupationOfMember').css('border-color', 'green');
    //}

    if ($('#NoofSharesSubscribed').val().trim() === "") {
        $('#NoofSharesSubscribed').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NoofSharesSubscribed').css('border-color', 'green');
    }

    if ($('#NameofNominee').val().trim() === "") {
        $('#NameofNominee').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NameofNominee').css('border-color', 'green');
    }

    if ($('#NomineeAge').val().trim() === "") {
        $('#NomineeAge').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NomineeAge').css('border-color', 'green');
    }

    if ($('#RelationshipCodeOfSocietyMember :selected').text() === "Select" || $('#RelationshipCodeOfSocietyMember :selected').text() === "") {
        $('#RelationshipCodeOfSocietyMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#RelationshipCodeOfSocietyMember').css('border-color', 'green');
    }

    if ($('#Address1').val().trim() === "") {
        $('#Address1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Address1').css('border-color', 'green');
    }

    if ($('#Address2').val().trim() === "") {
        $('#Address2').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Address2').css('border-color', 'green');
    }



    if ($('#PostOfficeOfSocietyMember').val().trim() === "") {
        $('#PostOfficeOfSocietyMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#PostOfficeOfSocietyMember').css('border-color', 'green');
    }
    var totallength = $('#PostalCodeOfSocietyMember').val().trim();

    if ($('#PostalCodeOfSocietyMember').val().trim() === "" || totallength.length < 6) {
        $('#PostalCodeOfSocietyMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#PostalCodeOfSocietyMember').css('border-color', 'green');
    }

    if ($('#DistrictOfMember :selected').text() === "Select" || $('#DistrictOfMember :selected').text() === "") {
        $('#DistrictOfMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#DistrictOfMember').css('border-color', 'green');
    }
    return isValid;
}
function ManagingCommitteMemberDetailsvalidate() {
    var isValid = true;
    if ($('#SocietyMemberName').val().trim() === "") {
        $('#SocietyMemberName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#SocietyMemberName').css('border-color', 'green');
    }

    if ($('#SocietyMemberDesignation').val().trim() === "") {
        $('#SocietyMemberDesignation').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#SocietyMemberDesignation').css('border-color', 'green');
    }

    if ($('#RelationshipMemberName').val().trim() === "") {
        $('#RelationshipMemberName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#RelationshipMemberName').css('border-color', 'green');
    }

    return isValid;
}

function UpdateManagingCommitteMemberARCS() {
    var res = ManagingCommitteMemberDetailsvalidate();
    if (res === false) {
        return false;
    }
    var objMCM = {
        SocietyMemberID: $('#SocietyMemberID').val(),
        SocietyMemberName: $('#SocietyMemberName').val(),
        SocietyMemberDesignation: $('#SocietyMemberDesignation').val(),
        //RelationshipCode: $('#RelationshipCode').val(),
        RelationshipMemberName: $('#RelationshipMemberName').val(),
        ManagingRelationshipName: $('#BckManagingRelationshipName').val()
        //Address: $('#Address').val(),
        //HouseNo: $('#HouseNo').val(),
        //SectorStreet: $('#SectorStreet').val(),
        //District: $('#CommitteMemberDistrict').val(),
        //MobileNumber: $('#MobileNumber').val(),
        //Email: $('#Email').val(),
        //AadharNo: $('#AadharNo').val(),
        //IsPresident: $('#IsPresident').val(),
    };
    $.ajax({
        url: "/Society/UpdateManagingCommitteMember",
        data: JSON.stringify(objMCM),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            clearTextBoxOfManagingCommitteMember();
            $('#myModalLFormARCS').modal('hide');
            alert("Data has been successfully updated");
            Bck_loadDataOfManagingCommitteMember();
            Bck_loadApprovedLForm();
            Bck_loadShareTransferMembers();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function clearTextBoxOfManagingCommitteMember() {
    $('#SocietyMemberName').val("");
    $('#SocietyMemberDesignation').val("");
    $('#RelationshipMemberName').val("");
    $('#BckManagingRelationshipName').val("");
}