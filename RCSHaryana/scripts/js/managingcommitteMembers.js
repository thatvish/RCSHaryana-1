/// <reference path="jquery-1.9.1.intellisense.js" />
//Load Data in Table when documents is ready
$(document).ready(function () {
    loadDataOfManagingCommitteMember();
});

//Load Data function
function loadDataOfManagingCommitteMember() {
    $.ajax({
        url: "/Society/ManagingCommitteMembersList",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td hidden="hidden">' + item.SocietyMemberID + '</td>';
                html += '<td>' + item.SocietyMemberName + '</td>';
                html += '<td>' + item.MobileNumber + '</td>';
                html += '<td><a href="#" onclick="return getbySocietyMemberIDOfManagingCommittMembers(' + item.SocietyMemberID + ')">Edit</a> | <a href="#" onclick="DeleleSocietyMemberIDOfManagingCommittMembers(' + item.SocietyMemberID + ')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Add Data Function 
function AddManagingCommitteMember() {
    var res = ManagingCommitteMemberDetailsvalidate();
    if (res == false) {
        return false;
    }
    var objMCM = {
        SocietyMemberID: $('#SocietyMemberID').val(),
        SocietyMemberName: $('#SocietyMemberName').val(),
        SocietyMemberDesignation: $('#SocietyMemberDesignation').val(),
        RelationshipCode: $('#RelationshipCode').val(),
        RelationshipMemberName: $('#RelationshipMemberName').val(),
        Address: $('#Address').val(),
        HouseNo: $('#HouseNo').val(),
        SectorStreet: $('#SectorStreet').val(),
        District: $('#CommitteMemberDistrict').val(),
        MobileNumber: $('#MobileNumber').val(),
        Email: $('#Email').val(),
        AadharNo: $('#AadharNo').val(),
        IsPresident: $('#IsPresident').val(),
    };
    $.ajax({
        url: "/Society/AddManagingCommitteMember",
        data: JSON.stringify(objMCM),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadDataOfManagingCommitteMember();
            clearTextBoxOfManagingCommitteMember();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Function for getting the Data Based upon Employee ID
function getbySocietyMemberIDOfManagingCommittMembers(SocietyMemberID) {
    $.ajax({
        url: "/Society/getbyManagingCommitteeMemberID/",
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: { SocietyMemberID },
        success: function (result) {
            //alert(JSON.stringify(result));
            var getbySocietyMemberID = result[0];
            $('#SocietyMemberID').val(SocietyMemberID)
            $('#SocietyMemberName').val(getbySocietyMemberID.SocietyMemberName)
            $('#SocietyMemberDesignation').val(getbySocietyMemberID.SocietyMemberDesignation)
            $('#RelationshipCode').val(getbySocietyMemberID.RelationshipCode)
            $('#RelationshipMemberName').val(getbySocietyMemberID.RelationshipMemberName)
            $('#Address').val(getbySocietyMemberID.Address)
            $('#HouseNo').val(getbySocietyMemberID.HouseNo)
            $('#SectorStreet').val(getbySocietyMemberID.SectorStreet)
            $('#CommitteMemberDistrict').val(getbySocietyMemberID.District)
            $('#MobileNumber').val(getbySocietyMemberID.MobileNumber)
            $('#Email').val(getbySocietyMemberID.Email)
            $('#AadharNo').val(getbySocietyMemberID.AadharNo)
            $('#IsPresident').val(getbySocietyMemberID.IsPresident)
            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//function for updating employee's record
function UpdateManagingCommitteMember() {
    var res = ManagingCommitteMemberDetailsvalidate();
    if (res == false) {
        return false;
    }
    var objMCM = {
        SocietyMemberID: $('#SocietyMemberID').val(),
        SocietyMemberName: $('#SocietyMemberName').val(),
        SocietyMemberDesignation: $('#SocietyMemberDesignation').val(),
        RelationshipCode: $('#RelationshipCode').val(),
        RelationshipMemberName: $('#RelationshipMemberName').val(),
        Address: $('#Address').val(),
        HouseNo: $('#HouseNo').val(),
        SectorStreet: $('#SectorStreet').val(),
        District: $('#CommitteMemberDistrict').val(),
        MobileNumber: $('#MobileNumber').val(),
        Email: $('#Email').val(),
        AadharNo: $('#AadharNo').val(),
        IsPresident: $('#IsPresident').val(),
    };
    $.ajax({
        url: "/Society/UpdateManagingCommitteMember",
        data: JSON.stringify(objMCM),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadDataOfManagingCommitteMember();
            clearTextBoxOfManagingCommitteMember();
            $('#myModal').modal('hide');
            //$('#SocietyMemberID').val("");
            $('#SocietyMemberName').val("")
            $('#SocietyMemberDesignation').val("")
            $('#RelationshipCode').val("")
            $('#RelationshipMemberName').val("")
            $('#Address').val("")
            $('#HouseNo').val("")
            $('#SectorStreet').val("")
            $('#CommitteMemberDistrict').val("")
            $('#MobileNumber').val("")
            $('#Email').val("")
            $('#AadharNo').val("")
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//function for deleting employee's record
//function Delele(SocietyMemberID) {
//    debugger;
//    var ans = confirm("Are you sure you want to delete this Record?");
//    if (ans) {
//        var url = "/Society/DeleteManagingCommitteMember";
//        $.get(url, { SocietyMemberID }, function (data) {
//            alert(data);
//            if (data > 0) {
//                loadDataOfManagingCommitteMember();
//            }
//            else {
//                alert("Member not deleted.");
//            }
//        });
//    }
//}

function DeleleSocietyMemberIDOfManagingCommittMembers(SocietyMemberID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        var objMCM = {
            SocietyMemberID: SocietyMemberID,
        };
        $.ajax({
            url: "/Society/DeleteManagingCommitteMember",
            data: JSON.stringify(objMCM),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                loadDataOfManagingCommitteMember();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes
function clearTextBoxOfManagingCommitteMember() {
    $('#SocietyMemberName').val("")
    $('#SocietyMemberDesignation').val("")
    $('#RelationshipCode').val("")
    $('#RelationshipMemberName').val("")
    $('#Address').val("")
    $('#HouseNo').val("")
    $('#SectorStreet').val("")
    $('#CommitteMemberDistrict').val("")
    $('#MobileNumber').val("")
    $('#Email').val("")
    $('#AadharNo').val("")
    $('#IsPresident').checked = false;
    $('#btnUpdate').hide();
    $('#btnAdd').show();
}
//Valdidation using jquery
function ManagingCommitteMemberDetailsvalidate() {
    var isValid = true;
    if ($('#SocietyMemberName').val().trim() == "") {
        $('#SocietyMemberName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#SocietyMemberName').css('border-color', 'green');
    }

    if ($('#SocietyMemberDesignation').val().trim() == "") {
        $('#SocietyMemberDesignation').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#SocietyMemberDesignation').css('border-color', 'green');
    }

    if ($('#RelationshipCode').val().trim() == "") {
        $('#RelationshipCode').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#RelationshipCode').css('border-color', 'green');
    }

    if ($('#RelationshipMemberName').val().trim() == "") {
        $('#RelationshipMemberName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#RelationshipMemberName').css('border-color', 'green');
    }

    if ($('#Address').val().trim() == "") {
        $('#Address').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Address').css('border-color', 'green');
    }

    if ($('#HouseNo').val().trim() == "") {
        $('#HouseNo').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#HouseNo').css('border-color', 'green');
    }

    if ($('#SectorStreet').val().trim() == "") {
        $('#SectorStreet').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#SectorStreet').css('border-color', 'green');
    }

    if ($('#CommitteMemberDistrict').val().trim() == "") {
        $('#CommitteMemberDistrict').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#CommitteMemberDistrict').css('border-color', 'green');
    }

    if ($('#MobileNumber').val().trim() == "") {
        $('#MobileNumber').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#MobileNumber').css('border-color', 'green');
    }

    if ($('#Email').val().trim() == "") {
        $('#Email').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Email').css('border-color', 'green');
    }

    if ($('#AadharNo').val().trim() == "") {
        $('#AadharNo').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#AadharNo').css('border-color', 'green');
    }

    if ($('#IsPresident').val().trim() == "") {
        $('#IsPresident').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#IsPresident').css('border-color', 'green');
    }

    return isValid;
}