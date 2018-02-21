/// <reference path="jquery-1.9.1.intellisense.js" />
//Load Data in Table when documents is ready
$(document).ready(function () {
    loadData();
});

function BindSocietyMemberOccupation() {
    $.getJSON("/Society/GetSocietyMemberOccupation",
    function (data) {
        var select = $("#OccupationOfMember");
        select.empty();
        select.append($('<option/>', {
            value: 0,
            text: "Select"
        }));
        $.each(data, function (index, itemData) {
            select.append($('<option/>', {
                value: itemData.Value,
                text: itemData.Text
            }));
        });
    });
}

function BindSocietyMemberRelationshipWithNominee() {
    $.getJSON("/Society/BindSocietyMemberRelationshipWithNominee",
    function (data) {
        var select = $("#RelationshipCodeOfSocietyMember");
        select.empty();
        select.append($('<option/>', {
            value: 0,
            text: "Select"
        }));
        $.each(data, function (index, itemData) {
            select.append($('<option/>', {
                value: itemData.Value,
                text: itemData.Text
            }));
        });
    });
}

//Load Data function
function loadData() {
    $.ajax({
        url: "/Society/SocietyMembersList",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td hidden="hidden">' + item.MemberSNo + '</td>';
                html += '<td>' + item.MemberName + '</td>';
                html += '<td>' + item.FatherName + '</td>';
                html += '<td>' + item.Mobile + '</td>';
                html += '<td>' + item.NomineeName + '</td>';
                html += '<td><a href="#" onclick="return getbySocietyMemberID(' + item.MemberSNo + ')">Edit</a> | <a href="#" onclick="Delele(' + item.MemberSNo + ')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbody1').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Add Data Function 
function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var objMFD = {
        MemberName: $('#MemberName').val(),
        FatherName: $('#FatherName').val(),
        Gender: $('#GenderOfSocietyMember').val(),
        Age: $('#Age').val(),
        OccupationCode: $('#OccupationOfMember').val(),
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
    };
    $.ajax({
        url: "/Society/AddSocietyMember",
        data: JSON.stringify(objMFD),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            clearTextBox();
            $('#myModal1').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Function for getting the Data Based upon Employee ID
function getbySocietyMemberID(MemberSNo) {
    $.ajax({
        url: "/Society/getbySocietyMemberID/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: { MemberSNo },
        success: function (result) {
            //alert(JSON.stringify(result));
            var getbySocietyMemberID = result[0];
            $('#MemberSNo').val(MemberSNo)
            $('#MemberName').val(getbySocietyMemberID.MemberName)
            $('#FatherName').val(getbySocietyMemberID.FatherName)
            $('#GenderOfSocietyMember').val(getbySocietyMemberID.Gender)
            var Age = getbySocietyMemberID.Age
            if (Age == 0) {
                $('#Age').val("")
            }
            else {
                $('#Age').val(Age)
            }
            var OccupationOfMember = getbySocietyMemberID.OccupationCode
            if (OccupationOfMember == 0) {
                BindSocietyMemberOccupation()
            }
            else {
                $('#OccupationOfMember').val(OccupationOfMember)
            }

            $('#Address1').val(getbySocietyMemberID.Address1)
            $('#Address2').val(getbySocietyMemberID.Address2)
            $('#PostOfficeOfSocietyMember').val(getbySocietyMemberID.PostOffice)
            var PostalCodeOfSocietyMember = getbySocietyMemberID.Pin
            if (PostalCodeOfSocietyMember == 0) {
                $('#PostalCodeOfSocietyMember').val("")
            }
            else {
                $('#PostalCodeOfSocietyMember').val(PostalCodeOfSocietyMember)
            }
            $('#DistrictOfMember').val(getbySocietyMemberID.DistCode)
            var NoofSharesSubscribed = getbySocietyMemberID.NoOfShares
            if (NoofSharesSubscribed == 0) {
                $('#NoofSharesSubscribed').val("")
            }
            else {
                $('#NoofSharesSubscribed').val(NoofSharesSubscribed)
            }
            $('#NameofNominee').val(getbySocietyMemberID.NomineeName)
            var NomineeAge = getbySocietyMemberID.NomineeAge
            if (NomineeAge == 0) {
                $('#NomineeAge').val("")
            }
            else {
                $('#NomineeAge').val(NomineeAge)
            }
            var RelationshipCodeOfSocietyMember = getbySocietyMemberID.RelationshipCode
            if (RelationshipCodeOfSocietyMember == 0) {
               BindSocietyMemberRelationshipWithNominee()
            }
            else {
                $('#RelationshipCodeOfSocietyMember').val(RelationshipCodeOfSocietyMember)
            }
            $('#MobileNumberOfSocietyMember').val(getbySocietyMemberID.Mobile)
            $('#AadharNo1').val(getbySocietyMemberID.AadharNo)
            $('#EmailId').val(getbySocietyMemberID.EmailId)
            $('#myModal1').modal('show');
            $('#btnUpdate1').show();
            $('#btnAdd1').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//function for updating employee's record
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var objMFD = {
        MemberSNo: $('#MemberSNo').val(),
        MemberName: $('#MemberName').val(),
        FatherName: $('#FatherName').val(),
        Gender: $('#GenderOfSocietyMember').val(),
        Age: $('#Age').val(),
        OccupationCode: $('#OccupationOfMember').val(),
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
    };
    $.ajax({
        url: "/Society/UpdateSocietyMemberDetail",
        data: JSON.stringify(objMFD),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //alert(result);
            loadData();
            clearTextBox();
            $('#myModal1').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);

        }
    });
}

//function for deleting employee's record
function Delele(MemberSNo) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        var objMFD = {
            MemberSNo: MemberSNo,
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
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes
function clearTextBox() {
    $('#MemberName').val("")
    $('#FatherName').val("")
    $('#GenderOfSocietyMember').val("")
    $('#Age').val("")
    $('#MobileNumberOfSocietyMember').val("")
    $('#EmailId').val("")
    $('#AadharNo1').val("")
    $('#OccupationOfMember').val("")
    $('#NoofSharesSubscribed').val("")
    $('#NameofNominee').val("")
    $('#NomineeAge').val("")
    $('#RelationshipCodeOfSocietyMember').val("")
    $('#Address1').val("")
    $('#Address2').val("")
    $('#PostOfficeOfSocietyMember').val("")
    $('#PostalCodeOfSocietyMember').val("")
    $('#DistrictOfMember').val("")
    $('#btnUpdate1').hide();
    $('#btnAdd1').show();
}

//Valdidation using jquery
function validate() {
    var isValid = true;
    debugger;
    if ($('#MemberName').val().trim() == "") {
        $('#MemberName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#MemberName').css('border-color', 'green');
    }

    if ($('#FatherName').val().trim() == "") {
        $('#FatherName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#FatherName').css('border-color', 'green');
    }

    if ($('#GenderOfSocietyMember :selected').text() == "Select Gender" || $('#GenderOfSocietyMember :selected').text() == "") {
        $('#GenderOfSocietyMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#GenderOfSocietyMember').css('border-color', 'green');
    }

    if ($('#Age').val().trim() == "") {
        $('#Age').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Age').css('border-color', 'green');
    }

    if ($('#MobileNumberOfSocietyMember').val().trim() == "") {
        $('#MobileNumberOfSocietyMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#MobileNumberOfSocietyMember').css('border-color', 'green');
    }

    if ($('#EmailId').val().trim() == "") {
        $('#EmailId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#EmailId').css('border-color', 'green');
    }

    if ($('#AadharNo1').val().trim() == "") {
        $('#AadharNo1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#AadharNo1').css('border-color', 'green');
    }

    if ($('#OccupationOfMember :selected').text() == "Select" || $('#OccupationOfMember :selected').text() == "") {
        $('#OccupationOfMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#OccupationOfMember').css('border-color', 'green');
    }

    if ($('#NoofSharesSubscribed').val().trim() == "") {
        $('#NoofSharesSubscribed').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NoofSharesSubscribed').css('border-color', 'green');
    }

    if ($('#NameofNominee').val().trim() == "") {
        $('#NameofNominee').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NameofNominee').css('border-color', 'green');
    }

    if ($('#NomineeAge').val().trim() == "") {
        $('#NomineeAge').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NomineeAge').css('border-color', 'green');
    }

    if ($('#RelationshipCodeOfSocietyMember :selected').text() == "Select" || $('#RelationshipCodeOfSocietyMember :selected').text() == "") {
        $('#RelationshipCodeOfSocietyMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#RelationshipCodeOfSocietyMember').css('border-color', 'green');
    }

    if ($('#Address1').val().trim() == "") {
        $('#Address1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Address1').css('border-color', 'green');
    }

    if ($('#Address2').val().trim() == "") {
        $('#Address2').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Address2').css('border-color', 'green');
    }

    if ($('#PostOfficeOfSocietyMember').val().trim() == "") {
        $('#PostOfficeOfSocietyMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#PostOfficeOfSocietyMember').css('border-color', 'green');
    }

    if ($('#PostalCodeOfSocietyMember').val().trim() == "") {
        $('#PostalCodeOfSocietyMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#PostalCodeOfSocietyMember').css('border-color', 'green');
    }

    if ($('#DistrictOfMember :selected').text() == "Select" || $('#DistrictOfMember :selected').text() == "") {
        $('#DistrictOfMember').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#DistrictOfMember').css('border-color', 'green');
    }
    return isValid;
}