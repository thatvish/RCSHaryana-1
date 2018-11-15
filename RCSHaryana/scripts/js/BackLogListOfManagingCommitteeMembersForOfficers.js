//Load Data in Table when documents is ready

function BindAllBackLogDetailsForOfficers() {
    var e = document.getElementById("SocietyList");
    var SocietyTransID = e.options[e.selectedIndex].value;
    if (SocietyTransID === "") {
        alert("Kindly select society");
        document.getElementById("btnForword").disabled = true;       
        document.getElementById("UpdateGeneralDetail").disabled = true;
        document.getElementById("SubmitDateInARCS").disabled = true;
    }
    else {
        Bck_BindHistoryView();
        Bck_ChangesByInspector();
        loadBackLogDetails();
        Bck_loadElectionDetail();
        Bck_loadDataOfManagingCommitteMember();
        Bck_loadApprovedLForm();
        Bck_loadShareTransferMembers();
        document.getElementById("btnForword").disabled = false;
        document.getElementById("UpdateGeneralDetail").disabled = false;
        document.getElementById("btnUpdateShareTransfer").disabled = false;
        document.getElementById("SubmitDateInARCS").disabled = false;
        document.getElementById("btnUpdateInARCS").disabled = false;
        document.getElementById("btnUpdateLForm").disabled = false;
        document.getElementById("SubClassOfSociety").disabled = false;
        document.getElementById("CommunityofSociety").disabled = false;
        //document.getElementById("btnForwordFreeze").disabled = false;
    }
}

function Bck_Pending_BindAllBackLogDetailsForOfficers() {
    var e = document.getElementById("SocietyList");
    var SocietyTransID = e.options[e.selectedIndex].value;
    if (SocietyTransID === "") {
        alert("Kindly select society");
        document.getElementById("btnForwordFreeze").disabled = true;
        document.getElementById("UpdateGeneralDetail").disabled = true;
        document.getElementById("btnUpdateShareTransfer").disabled = true;
        document.getElementById("SubmitDateInARCS").disabled = true;
        document.getElementById("chk1").disabled = true;
    }
    else {
        BindDivId();
        Bck_BindHistoryView();
        loadBackLogDetails();
        Bck_loadElectionDetail();
        Bck_loadDataOfManagingCommitteMember();
        Bck_loadApprovedLForm();
        Bck_loadShareTransferMembers();
        document.getElementById("btnForwordFreeze").disabled = false;
        Bck_ChangesByInspector();
        document.getElementById("UpdateGeneralDetail").disabled = false;
        document.getElementById("SubmitDateInARCS").disabled = false;
        document.getElementById("btnUpdateInARCS").disabled = false;
        document.getElementById("btnUpdateLForm").disabled = false;
        document.getElementById("btnUpdateShareTransfer").disabled = false;
        document.getElementById("chk1").disabled = false;
        document.getElementById("SubClassOfSociety").disabled = false;
        document.getElementById("CommunityofSociety").disabled = false;
        //document.getElementById("btnForwordFreeze").disabled = false;
    }
}

function BindDivId()
{
        var divIdBindHistory = window.localStorage.lastname
        //var FinalId = "\'" + divIdBindHistory + "\'";
        var e = document.getElementById("SocietyList");
        var SocietyTransID = e.options[e.selectedIndex].value;
        if (SocietyTransID != null) {
            if (SocietyTransID === '') {
            }
            else {
                $.ajax({
                    url: "/BackLogOfficer/GetInspectorChange/",
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    data: { SocietyTransID },
                    success: function (result) {
                        var GetCount = result.GetCount;
                        if (GetCount >= 1) {
                            $('#BindHistoryDivInspector').css("display", "block");
                        }
                        else {
                            $('#BindHistoryDivInspector').css("display", "none");
                        }
                    },
                    error: function (errormessage) {
                        //alert(errormessage.responseText);
                    }
                });
            }
        }
}

function loadBackLogDetails() {
    var e = document.getElementById("SocietyList");
    var SocietyTransID = e.options[e.selectedIndex].value;
    $.ajax({
        url: "/ARCS/getBackLogDetails/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: { SocietyTransID },
        success: function (result) {
            var getbySociety = result[0];   
            $("#backlogSocietyName").val(getbySociety.SocietyName);
            $("#backlogRID").val(getbySociety.RegId);
            $("#backlogDateofReg").val(getbySociety.Createdate);
            $("#backlogSocietyTrans").val(getbySociety.SocietyTransId);
            $("#backlogAreaopt").val(getbySociety.AreaOfOperation);
            $("#lastauditdatetime").val(getbySociety.LastDateAudit);
            $("#datetimeInspection").val(getbySociety.LastDateInspection);
            $("#backlogBodyMeeting").val(getbySociety.GeneralBodyMeeting);
            $("#backlogAmountFee").val(getbySociety.AmountOfAuditFees);
            $("#CommunityofSociety").val(getbySociety.CommunityOfSocietyId);
            $("#SubClassOfSociety").val(getbySociety.KindOfSocietyId);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Bck_ChangesByInspector() {
    var e = document.getElementById("SocietyList");
    var SocietyTransID = e.options[e.selectedIndex].value;
    $.ajax({
        url: "/BackLogOfficer/GetInspectorChange/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: { SocietyTransID },
        success: function (result) {
            //debugger;
            var RoleId = result.RoleId;
            var GetCount = result.GetCount;
            var GetChecked = result.GetChecked;
            if (RoleId ==3)
            {

            }
            if (RoleId ==2) {
                if (GetCount > 0 && GetChecked !=true) {

                    alert("Inspector has done some changes. Please check Verify in History Tab For Proceed Further.")
                    document.getElementById("btnForwordFreeze").disabled = true;
                }
                if (GetChecked == true)
                {
                    document.getElementById("btnForwordFreeze").disabled = false;
                    document.getElementById("chk1").disabled = true;
                }
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function Bck_BindHistoryView() {
    var e = document.getElementById("SocietyList");
    var SocietyTransID = e.options[e.selectedIndex].value;
    $.ajax({
        url: "/BackLogOfficer/BindHistoryView/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: { SocietyTransID },
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                //var getDate = item.Date
                //var date = new Date(parseInt(getDate.substr(6)));
                //var month = date.getMonth() + 1;
                html += '<tr>';
                html += '<td hidden="hidden">' + item.ColumnName + '</td>';
                html += '<td>' + item.ColumnName + '</td>';
                html += '<td>' + item.OldValue + '</td>';
                html += '<td>' + item.NewVAlue + '</td>';
                html += '<td>' + item.FirstName + '</td>';
                html += '<td>' + item.SocietyName + '</td>';
                html += '<td>' + item.OldRedgNo + '</td>';
                html += '<td>' + item.SocietyTransID + '</td>';
                html += '</tr>';
            });
            $('.Bck_HistoryView').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function Bck_loadElectionDetail() {
    var e = document.getElementById("SocietyList");
    var SocietyTransID = e.options[e.selectedIndex].value;
    $.ajax({
        url: "/BackLogOfficer/ElectionDetails/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: { SocietyTransID },
        success: function (GetElectionDate) {           
            $("#ElectionDatetime").val(GetElectionDate);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function Bck_loadShareTransferMembers() {
    var e = document.getElementById("SocietyList");
    var SocietyTransID = e.options[e.selectedIndex].value;
    $.ajax({
        url: "/ARCS/ShareMembersList",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var NoofMember = result.TotalCount;
            FindLength = result.getRecord.length;
            var html = '';
            var i = 0;
            $.each(result.getRecord, function (key, item) {            
                i = i + 1;
                html += '<tr>';
                html += '<td hidden="hidden">' + item.MemberSNo + '</td>';
                html += '<td>' + i + '</td>';
                //html += '<td><img src="' + item.fullpath + '"/></td>';             
                html += '<td>' + item.OldMemberName + '</td>';
                html += '<td>' + item.MemberName + '</td>';
                html += '<td>' + item.ShareTransferApprovalDate + '</td>';
                html += '<td><a id="Bck_ShareMembersDetails" href="#" class="btn btn-block ink-reaction btn-flat btn-info" onclick="return Bck_SharegetbySocietyMemberID(' + item.ShareTransferID + ')">View</a>';
                html += '</tr>';
            });
            $('.Bck_Share_tblBindMembers').html(html);
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}
function Download() {
    $.ajax({
        url: "/ARCS/downLoadLForm/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
    return false;
}
function Bck_getbySocietyMemberID(MemberSNo) {
        $.ajax({
            url: "/ARCS/getbySocietyMemberID/",
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            data: { MemberSNo },
            success: function (result) {
                //alert(JSON.stringify(result));
                var getbySocietyMemberID = result[0];
                $('#MemberSNo').val(MemberSNo);
                $('#MemberName').val(getbySocietyMemberID.MemberName);
                $('#FatherName').val(getbySocietyMemberID.FatherName);
                $('#BckManagingMemberRelationship').val(getbySocietyMemberID.ManagingMemberRelationship);
                $('#GenderOfSocietyMember').val(getbySocietyMemberID.Gender);
                $('#Age').val(getbySocietyMemberID.Age);
                $('#OccupationOfMember').val(getbySocietyMemberID.OccupationName);
                $('#Address1').val(getbySocietyMemberID.Address1);
                $('#Address2').val(getbySocietyMemberID.Address2);
                $('#PostOfficeOfSocietyMember').val(getbySocietyMemberID.PostOffice);
                $('#PostalCodeOfSocietyMember').val(getbySocietyMemberID.Pin);
                $('#DistrictOfMember').val(getbySocietyMemberID.DistrictName);
                $('#NoofSharesSubscribed').val(getbySocietyMemberID.NoOfShares);
                $('#NameofNominee').val(getbySocietyMemberID.NomineeName);
                $('#NomineeAge').val(getbySocietyMemberID.NomineeAge);
                $('#RelationshipCodeOfSocietyMember').val(getbySocietyMemberID.RelationshipName);
                $('#MobileNumberOfSocietyMember').val(getbySocietyMemberID.Mobile);
                $('#AadharNo1').val(getbySocietyMemberID.AadharNo);
                $('#EmailId').val(getbySocietyMemberID.EmailId);
                $('#myModal1').modal('show');
            },
            error: function (errormessage) {
                //alert(errormessage.responseText);
            }
        });
        return false;
    }
function Bck_SharegetbySocietyMemberID(ShareTransferID) {

    $.ajax({
        url: "/ARCS/getbyShareTransferMemberID",
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
                $('#datepicker').val(getbySocietyMemberID.Dob);
            }
            $('#showCOR1').css('display', 'block');
            $('#MemberSNo').val(getbySocietyMemberID.MemberSNo);
            $('#MemberName1').val(getbySocietyMemberID.MemberName);
            $('#FatherName1').val(getbySocietyMemberID.FatherName);
            $('#GenderOfSocietyMember1').val(getbySocietyMemberID.Gender);
            $('#img1').attr('src', getbySocietyMemberID.Fullpath);
            $('#files_101').text(getbySocietyMemberID.Fullpath);
            $('#img').val(getbySocietyMemberID.imgg);
            // $('#files_10').val(getbySocietyMemberID.fullpath)
            //$('#files_10').val(getbySocietyMemberID.flfile);
            var Age = getbySocietyMemberID.Age;
            if (Age === 0) {
                $('#Age1').val("");
            }
            else {
                $('#Age1').val(Age);
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
                $('#OccupationOfMember1').val("");
            }
            else {
                $('#OccupationOfMember1').val(OccupationOfMember);
            }
            $('#Address11').val(getbySocietyMemberID.Address1);
            $('#Address22').val(getbySocietyMemberID.Address2);
            $('#PostOfficeOfSocietyMember1').val(getbySocietyMemberID.PostOffice);
            var PostalCodeOfSocietyMember = getbySocietyMemberID.Pin;
            if (PostalCodeOfSocietyMember === 0) {
                $('#PostalCodeOfSocietyMember1').val("");
            }
            else {
                $('#PostalCodeOfSocietyMember1').val(PostalCodeOfSocietyMember);
            }
            $('#DistrictOfMember1').val(getbySocietyMemberID.DistCode);
            var NoofSharesSubscribed = getbySocietyMemberID.NoOfShares;
            if (NoofSharesSubscribed === 0) {
                $('#NoofSharesSubscribed1').val("");
            }
            else {
                $('#NoofSharesSubscribed1').val(NoofSharesSubscribed);
            }
            $('#NameofNominee1').val(getbySocietyMemberID.NomineeName);
            var NomineeAge = getbySocietyMemberID.NomineeAge;
            if (NomineeAge === 0) {
                $('#NomineeAge1').val("");
            }
            else {
                $('#NomineeAge1').val(NomineeAge);
            }
            var RelationshipCodeOfSocietyMember = getbySocietyMemberID.RelationshipCode;
            if (RelationshipCodeOfSocietyMember === 0) {
                BindSocietyMemberRelationshipWithNominee();
            }
            else {
                $('#RelationshipCodeOfSocietyMember1').val(RelationshipCodeOfSocietyMember);
            }
            $('#MobileNumberOfSocietyMember1').val(getbySocietyMemberID.Mobile);

            $('#ExistingMemberName').val(getbySocietyMemberID.ExistingMemberName);
            $('#Approvaldatetime').val(getbySocietyMemberID.ShareTransferApprovalDate);
            $('#ApprovalLetterNo').val(getbySocietyMemberID.ShareTransferAppLetterNo);
            $('#MemberId').val(getbySocietyMemberID.OldMemberId);
            $('#sharetransferId').val(getbySocietyMemberID.ShareTransferID);

            $('#AadharNo11').val(getbySocietyMemberID.AadharNo);
            $('#EmailId1').val(getbySocietyMemberID.EmailId);
            $('#myModal11').modal('show');
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }

    });
    return false;
}

/*
Todays Function - 12-07-2018
 */
function Bck_loadApprovedLForm() {
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
                html += '<td><a id="Bck_loadApprovedLform" href="#" class="btn btn-block ink-reaction btn-flat btn-info" onclick="return Bck_loadgetbySocietyMemberID(' + item.MemberSNo + ')">View</a>';               
                html += '</tr>';
            });
            $('.Bck_Approved_tblBindMembers').html(html);
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
            // $("#div1").html("Total Added Member " + FindLength);
            //refreshPage();
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}
function Bck_loadgetbySocietyMemberID(MemberSNo) {
    $.ajax({
        url: "/Society/getbySocietyMemberID/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: { MemberSNo },
        success: function (result) {
            var getbySocietyMemberID = result[0];
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
            $('#MemberSNo').val(getbySocietyMemberID.MemberSNo);
            $('#MemberName').val(getbySocietyMemberID.MemberName);
            $('#FatherName').val(getbySocietyMemberID.FatherName);
            $('#BckManagingMemberRelationship').val(getbySocietyMemberID.ManagingMemberRelationship);
            $('#GenderOfSocietyMember').val(getbySocietyMemberID.Gender);
            $('#img').attr('src', getbySocietyMemberID.Fullpath);
            $('#files_10L').text(getbySocietyMemberID.Fullpath);
            $('#img').val(getbySocietyMemberID.imgg);
            // $('#files_10').val(getbySocietyMemberID.fullpath)
            //$('#files_10').val(getbySocietyMemberID.flfile);
            var Age = getbySocietyMemberID.Age;
            if (Age === 0) {
                $('#Age').val("");
            }
            else {
                $('#Age').val(Age);
            }

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
            $('#AadharNo1').val(getbySocietyMemberID.AadharNo);
            $('#EmailId').val(getbySocietyMemberID.EmailId);
            $('#myModalApprovedLForm').modal('show');
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }

    });
    return false;
}
function Bck_loadDataOfManagingCommitteMember() {
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
                html += '<td>' + item.RelationshipMemberName + '</td>';  
                html += '<td><a id="Bck_MenberCommitteeform" href="#" class="btn btn-block ink-reaction btn-flat btn-info" onclick="return Bck_getbySocietyMemberIDOfManagingCommittMembers(' + item.SocietyMemberID + ')">View</a>';
                html += '</tr>';
            });
            $('.Bck_tblMembercommittee').html(html);
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}
function Bck_getbySocietyMemberIDOfManagingCommittMembers(SocietyMemberID) {
    //debugger;
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
            $('#RelationshipMemberName').val(getbySocietyMemberID.RelationshipMemberName);
            $('#BckManagingRelationshipName').val(getbySocietyMemberID.ManagingRelationshipName);
            $('#myModalLFormARCS').modal('show');            
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}


