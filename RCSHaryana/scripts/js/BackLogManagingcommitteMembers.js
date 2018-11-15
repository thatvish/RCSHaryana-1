
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
                //html += '<td>' + item.RelationshipName + '</td>';
                html += '<td>' + item.RelationshipMemberName + '</td>';
                html += '<td>' + item.SocietyMemberDesignationName + '</td>';
                html += '<td id="tbMCM"><a id="editManagingCommitte" href="#" onclick="return getbySocietyMemberIDOfManagingCommittMembers(' + item.SocietyMemberID + ')">Edit</a> | <a href="#" id="deleteManagingCommitte" onclick="DeleleSocietyMemberIDOfManagingCommittMembers(' + item.SocietyMemberID + ')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
            //loadData();
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}
function loadData() {
    //debugger;
    var FindLength = 0;

    $.ajax({

        url: "/Society/SocietyMembersList",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            var NoofMember = result.TotalCount;
            FindLength = result.getRecord.length;
            var html = '';
            var i = 0;
            $.each(result.getRecord, function (key, item) {
                i = i + 1;
                html += '<tr>';
                html += '<td hidden="hidden">' + item.MemberSNo + '</td>';
                html += '<td>' + i + '</td>';
                html += '<td><img src="' + item.Fullpath + '" width="52px;" height="75px;"/></td>';
                html += '<td>' + item.MemberName + '</td>';
                html += '<td>' + item.FatherName + '</td>';
                html += '<td>' + item.Mobile + '</td>';
                html += '<td>' + item.NomineeName + '</td>';
                html += '<td><a href="#" onclick="return getbySocietyMemberID(' + item.MemberSNo + ')">Edit</a> | <a href="#" onclick="Delele(' + item.MemberSNo + ')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbody1').html(html);
            //if (FindLength === NoofMember) {
            //    $("#AddMemberDetails").hide();
            //    $('#AddManagingCommitteMembersBck').hide();
            //    $("#div1").addClass("alert alert-warning");

            //    //$("#div1").Removeclass("alert alert-success");
            //}
            //else {
            //    $("#AddMemberDetails").show();
            //    $('#AddManagingCommitteMembersBck').show();
            //    $("#div1").removeClass("alert alert-warning");
            //    $("#div1").addClass("alert alert-success");
            //}
            //$("#div1").html("Total Added Member " + FindLength);
            //refreshPage();
            loadDataOfManagingCommitteMember();
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
        url: "/Society/AddManagingCommitteMember",
        data: JSON.stringify(objMCM),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadDataOfManagingCommitteMember();
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
            //alert(JSON.stringify(result));
            var getbySocietyMemberID = result[0];
            $('#SocietyMemberID').val(SocietyMemberID);
            $('#SocietyMemberName').val(getbySocietyMemberID.SocietyMemberName);
            $('#SocietyMemberDesignation').val(getbySocietyMemberID.SocietyMemberDesignation);
            //$('#RelationshipCode').val(getbySocietyMemberID.RelationshipCode)
            $('#RelationshipMemberName').val(getbySocietyMemberID.RelationshipMemberName);
            $('#BckManagingRelationshipName').val(getbySocietyMemberID.ManagingRelationshipName);
            //$('#Address').val(getbySocietyMemberID.Address);
            //$('#HouseNo').val(getbySocietyMemberID.HouseNo)
            //$('#SectorStreet').val(getbySocietyMemberID.SectorStreet)
            //$('#CommitteMemberDistrict').val(getbySocietyMemberID.District)
            //$('#MobileNumber').val(getbySocietyMemberID.MobileNumber)
            //$('#Email').val(getbySocietyMemberID.Email)
            //$('#AadharNo').val(getbySocietyMemberID.AadharNo)
            //$('#IsPresident').val(getbySocietyMemberID.IsPresident)
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
    if (res === false) {
        return false;
    }
    $('#dvLoading').fadeIn();
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
            loadDataOfManagingCommitteMember();
            clearTextBoxOfManagingCommitteMember();
            $('#dvLoading').fadeOut();
            $('#myModal').modal('hide');
            //$('#SocietyMemberID').val("");
            $('#SocietyMemberName').val("");
            $('#SocietyMemberDesignation').val("");
            //$('#RelationshipCode').val("")
            $('#RelationshipMemberName').val("");
            $('#BckManagingRelationshipName').val("");
            
            //$('#Address').val("")
            //$('#HouseNo').val("")
            //$('#SectorStreet').val("")
            //$('#CommitteMemberDistrict').val("")
            //$('#MobileNumber').val("")
            //$('#Email').val("")
            //$('#AadharNo').val("")
        },
        error: function (errormessage) {
            $('#dvLoading').fadeOut();
            //alert(errormessage.responseText);
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
    //debugger;
    var status = $('#offdelete').val();
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
            },
            error: function (errormessage) {
                //alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes
function clearTextBoxOfManagingCommitteMember() {
    $('#SocietyMemberName').val("");
    $('#SocietyMemberDesignation').val("");
    //$('#RelationshipCode').val("")
    $('#RelationshipMemberName').val("");
    $('#BckManagingMemberRelationship').val("");
    //$('#Address').val("")
    //$('#HouseNo').val("")
    //$('#SectorStreet').val("")
    //$('#CommitteMemberDistrict').val("")
    //$('#MobileNumber').val("")
    //$('#Email').val("")
    //$('#AadharNo').val("")
    //$('#IsPresident').checked = false;
    $('#btnUpdate').hide();
    $('#btnAdd').show();
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
    if ($('#BckManagingRelationshipName').val() != null) {
        if ($('#BckManagingRelationshipName').val().trim() === "") {
            $('#BckManagingRelationshipName').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#BckManagingRelationshipName').css('border-color', 'green');
        }
    }
    else {
        $('#BckManagingRelationshipName').css('border-color', 'Red');
        isValid = false;
    }
    if ($('#SocietyMemberDesignation').val().trim() === "") {
        $('#SocietyMemberDesignation').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#SocietyMemberDesignation').css('border-color', 'green');
    }

    //if ($('#RelationshipCode').val().trim() == "") {
    //    $('#RelationshipCode').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#RelationshipCode').css('border-color', 'green');
    //}

    if ($('#RelationshipMemberName').val().trim() === "") {
        $('#RelationshipMemberName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#RelationshipMemberName').css('border-color', 'green');
    }

    //if ($('#Address').val().trim() == "") {
    //    $('#Address').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#Address').css('border-color', 'green');
    //}

    //if ($('#HouseNo').val().trim() == "") {
    //    $('#HouseNo').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#HouseNo').css('border-color', 'green');
    //}

    //if ($('#SectorStreet').val().trim() == "") {
    //    $('#SectorStreet').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#SectorStreet').css('border-color', 'green');
    //}

    //if ($('#CommitteMemberDistrict').val().trim() == "") {
    //    $('#CommitteMemberDistrict').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#CommitteMemberDistrict').css('border-color', 'green');
    //}

    //if ($('#MobileNumber').val().trim() == "") {
    //    $('#MobileNumber').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#MobileNumber').css('border-color', 'green');
    //}

    //if ($('#Email').val().trim() == "") {
    //    $('#Email').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#Email').css('border-color', 'green');
    //}

    //var aadharnoLenght = $('#AadharNo').lenght;
    //if (aadharnoLenght >= 12) {
    //    $('#AadharNo').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#AadharNo').css('border-color', 'green');
    //}

    //if ($('#IsPresident').val().trim() == "") {
    //    $('#IsPresident').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#IsPresident').css('border-color', 'green');
    //}

    return isValid;
}