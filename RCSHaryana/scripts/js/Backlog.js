function BacklogAuditValidate() {
    var isValid = true;
    if ($('#CommunityofSociety').val().trim() === "") {
        $('#CommunityofSociety').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#CommunityofSociety').css('border-color', 'green');
    }

    return isValid;
}
function AddBacklogAuditDetail() {
    var res = BacklogAuditValidate();
    ///debugger;
    if (res === false) {
        //alert("Please Fill the mandatory field first");
        return false;
    }
    $('#dvLoading').fadeIn();
    var objMFD = {
        //dob: $('#datepicker').val(),
        SocietyName: $('#backlogSocietyName').val(),
        DateofRegistration: $('#backlogDateofReg').val(),
        RegId: $('#backlogRID').val(),
        CommunityOfSocietyId: $('#CommunityofSociety').val(),
        KindOfSocietyId: $('#SubClassOfSociety').val()
    };
    $.ajax({
        url: "/BackLog/AddBackLogData",
        data: JSON.stringify(objMFD),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {    
            $('#dvLoading').fadeOut();
        },
        error: function (errormessage) {
            $('#dvLoading').fadeOut();
            //alert(errormessage.responseText);
        }
    });
}
function AddElectionDate() {
    var value = $('#ElectionDatetime').datepicker('getDate');
    if (value === "" || value === null) {
        $('#ElectionDatetime').val("");
    }
    else {
        var getdate = value.getDate();
        var month = value.getMonth() + 1;
        var fulldate = getdate + '/' + month + '/' + value.getFullYear();
    }
    $('#dvLoading').fadeIn();
    var objdata = {
        electionDate: fulldate
    };
    $.ajax({
        url: "/BackLog/SaveElectionDate",
        data: JSON.stringify(objdata),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            alert("Election Date has been Saved.");
            $('#dvLoading').fadeOut();
        },
        error: function (errormessage) {
            $('#dvLoading').fadeOut();
            alert(errormessage.responseText);
        }
    });
}

function AddElectionDateByARCS() {
    var value = $('#ElectionDatetime').datepicker('getDate');
    if (value === "" || value === null) {
        $('#ElectionDatetime').val("");
    }
    else {
        var getdate = value.getDate();
        var month = value.getMonth() + 1;
        var fulldate = getdate + '/' + month + '/' + value.getFullYear();
    }

    var objdata = {
        electionDate: fulldate
    };
    $('#dvLoading').fadeIn();
    $.ajax({
        url: "/ARCS/SaveElectionDate",
        data: JSON.stringify(objdata),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#dvLoading').fadeOut();
            alert("Election Date has been Saved.");
        },
        error: function (errormessage) {
            $('#dvLoading').fadeOut();
            alert(errormessage.responseText);
        }
    });
}


function SaveElectionCommitteMember() {
    // var res = ManagingCommitteMemberDetailsvalidate();
    //if (res == false) {
    //    return false;
    //}
    var objMCM = {
        SocietyMemberID: $('#SocietyMemberID').val(),
        SocietyMemberName: $('#SocietyMemberName').val(),
        SocietyMemberDesignation: $('#SocietyMemberDesignation').val(),
        //RelationshipCode: $('#RelationshipCode').val(),
        RelationshipMemberName: $('#RelationshipMemberName').val()
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
        url: "/BackLog/ElectionCommitteMember",
        data: JSON.stringify(objMCM),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            alert("Data Saved Successfully");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}



