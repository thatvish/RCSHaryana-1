//import { debug } from "util";

$(document).ready(function () {
    loadDataOfManagingCommitteMember();
});

$(document).ready(function () {
    
    $("#SocietyMemberDesignation").change(function () {
        var e = document.getElementById("SocietyMemberDesignation");
        var value = e.options[e.selectedIndex].value;
        if (value === "1" || value === "2") {
            $.getJSON("/Society/CheckPresidentValidation", { SocietyMemberDesignation: value },
                function (data) {
                if (data >= 1) {
                    if (value === "1") {
                        alert("You already select the president for member. You can only edit that member profile");
                        $("#SocietyMemberDesignation").val("");
                    }
                    if (value === "2") {
                        alert("You already select the vice president for member. You can only edit that member profile");
                        $("#SocietyMemberDesignation").val("");
                    }
                    if (value === "4") {
                        alert("You already select the cashier for member. You can only edit that member profile");
                        $("#SocietyMemberDesignation").val("");
                    }
                    if (value === "3") {
                        alert("You already select the Secretary for member. You can only edit that member profile");
                        $("#SocietyMemberDesignation").val("");
                    }
                }
            });
        }
    });
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
                html += '<td>' + item.RelationshipMemberName + '</td>';
                html += '<td>' + item.SocietyMemberDesignationName + '</td>';
                html += '<td id="tbMCM"><a id="editManagingCommitte" href="#" onclick="return getbySocietyMemberIDOfManagingCommittMembers(' + item.SocietyMemberID + ')">Edit</a> | <a href="#" id="deleteManagingCommitte" onclick="DeleleSocietyMemberIDOfManagingCommittMembers(' + item.SocietyMemberID + ')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}

//Add Data Function 
function AddManagingCommitteMember() {
    var res = ManagingCommitteMemberDetailsvalidate();
    if (res === false) {
        return false;
    }
    $('#dvLoading').fadeIn();
    var objMCM = {
        SocietyMemberID: $('#SocietyMemberID').val(),
        SocietyMemberName: $('#SocietyMemberName').val(),
        SocietyMemberDesignation: $('#SocietyMemberDesignation').val(),
        RelationshipMemberName: $('#RelationshipMemberName').val(),
        ManagingRelationshipName: $('#ManagingRelationshipName').val()
    };

    $.ajax({
        url: "/Society/AddManagingCommitteMember",
        data: JSON.stringify(objMCM),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadDataOfManagingCommitteMember();
            loadData();
            clearTextBoxOfManagingCommitteMember();
            $('#dvLoading').fadeOut();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            $('#dvLoading').fadeOut();
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
            var getbySocietyMemberID = result[0];
            $('#SocietyMemberID').val(SocietyMemberID);
            $('#SocietyMemberName').val(getbySocietyMemberID.SocietyMemberName);
            $('#SocietyMemberDesignation').val(getbySocietyMemberID.SocietyMemberDesignation);
            $('#RelationshipMemberName').val(getbySocietyMemberID.RelationshipMemberName);
            $('#ManagingRelationshipName').val(getbySocietyMemberID.ManagingRelationshipName);
            $('#myModal').modal('show');
            $('#btnUpdate11').show();
            $('#btnAdd11').hide();
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
    if (res === false) {
        return false;
    }
    $('#dvLoading').fadeIn();
    var objMCM = {
        SocietyMemberID: $('#SocietyMemberID').val(),
        SocietyMemberName: $('#SocietyMemberName').val(),
        SocietyMemberDesignation: $('#SocietyMemberDesignation').val(),
        RelationshipMemberName: $('#RelationshipMemberName').val(),
        ManagingRelationshipName: $('#ManagingRelationshipName').val()
    };
    $.ajax({
        url: "/Society/UpdateManagingCommitteMember",
        data: JSON.stringify(objMCM),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadDataOfManagingCommitteMember();
            loadData();
            clearTextBoxOfManagingCommitteMember();
            $('#dvLoading').fadeOut();
            $('#myModal').modal('hide');
            $('#SocietyMemberName').val("");
            $('#SocietyMemberDesignation').val("");
            $('#RelationshipMemberName').val("");
        },
        error: function (errormessage) {
            $('#dvLoading').fadeOut();
            alert(errormessage.responseText);
        }
    });
}

function DeleleSocietyMemberIDOfManagingCommittMembers(SocietyMemberID) {
    var status = $('#delete').val();
    var isValid = true;
    if (status === "1") {
        isValid = false;
        alert("You can't delete this entry .");
        return isValid;
    }
    var ans = confirm("The record shall be removed from Lisf of Members. Are you sure?");
    if (ans) {
        var objMCM = {
            SocietyMemberID: SocietyMemberID
        };
        $.ajax({
            url: "/Society/DeleteManagingCommitteMember",
            data: JSON.stringify(objMCM),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                loadDataOfManagingCommitteMember();
                loadData();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes
function clearTextBoxOfManagingCommitteMember() {
    $('#SocietyMemberName').val("");
    $('#SocietyMemberDesignation').val("");
    $('#RelationshipMemberName').val("");
    $('#ManagingRelationshipName').val("");
    $('#btnUpdate11').hide();
    $('#btnAdd11').show();
}

//Valdidation using jquery
function ManagingCommitteMemberDetailsvalidate() {
    var isValid = true;
    if ($('#SocietyMemberName').val().trim() === "") {
        $('#SocietyMemberName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#SocietyMemberName').css('border-color', 'green');
    }
    if ($('#ManagingRelationshipName').val().trim() === "") {
        $('#ManagingRelationshipName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ManagingRelationshipName').css('border-color', 'green');
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