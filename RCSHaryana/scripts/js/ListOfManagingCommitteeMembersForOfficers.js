//Load Data in Table when documents is ready

function BindAllSocietyDetailsForOfficers() {
    var e = document.getElementById("SocietyList");
    var SocietyTransID = e.options[e.selectedIndex].value;
    if (SocietyTransID === "") {
        alert("Kindly select society");
        document.getElementById("btnForword").disabled = true;
    }
    else {
        loadSocietyDetails();
        loadManagingCommitteeMembers();
        loadSocietyMembers();
        document.getElementById("btnForword").disabled = false;
    }
}

function loadSocietyDetails() {
    var e = document.getElementById("SocietyList");
    var SocietyTransID = e.options[e.selectedIndex].value;
    $.ajax({
        url: "/ARCS/getSocietyDetails/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: { SocietyTransID },
        success: function (result) {
            // alert(JSON.stringify(result))
            var getbySociety = result[0];
            //alert(getbySociety.DistrictForShowUSer);
            $('#NameofProposedSociety').val(getbySociety.SocietyName);
            $('#AreaofOperation').val(getbySociety.AreaofOperation);
            $('#ClassofSocietyandLiability').val(getbySociety.SocietyClassName);
            $('#SocietySubClassCode').val(getbySociety.SocietySubClassName);
            $('#RegisteredAddress').val(getbySociety.Address1);
            $('#HouseNoSectorNoRoad').val(getbySociety.Address2);
            $('#PostOffice').val(getbySociety.PostOffice);
            $('#PostalCode').val(getbySociety.Pin);
            //$('#AreaOfOperation').val(getbySociety.AreaOfOperation)
            $('#Mainobjects1').val(getbySociety.Mainobject1);
            $('#Mainobjects2').val(getbySociety.Mainobject2);
            $('#Mainobjects3').val(getbySociety.Mainobject3);
            $('#Mainobjects4').val(getbySociety.Mainobject4);
            $('#Noofmemberspresent').val(getbySociety.NoOfMembers);
            //$('#Occupation').val(getbySociety.OccupationName)
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
            $('#DistrictForUser1').val(getbySociety.DistrictForShowUSer);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadManagingCommitteeMembers() {
    var e = document.getElementById("SocietyList");
    var SocietyTransID = e.options[e.selectedIndex].value;
    $.ajax({
        url: "/ARCS/ManagingCommitteMembersList/",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: { SocietyTransID },
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td hidden="hidden">' + item.SocietyMemberID + '</td>';
                html += '<td>' + item.SocietyMemberName + '</td>';
                html += '<td>' + item.SocietyMemberDesignationName + '</td>';
                html += '<td>' + item.RelationshipMemberName + '</td>';
                html += '<td>' + item.RelationshipName + '</td>';
                html += '<td><a id="btnCommitteMembers" href="#" class="btn btn-block ink-reaction btn-flat btn-info" onclick="return getbySocietyManagingCommittMembers(' + item.SocietyMemberID + ')">View</a> ';
                html += '</tr>';
            });
            $('.tblBindCommitteMembers').html(html);
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}

function loadSocietyMembers() {
    var e = document.getElementById("SocietyList");
    var SocietyTransID = e.options[e.selectedIndex].value;
    $.ajax({
        url: "/ARCS/SocietyMembersList/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: { SocietyTransID },
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td hidden="hidden">' + item.MemberSNo + '</td>';
                html += '<td>' + item.MemberName + '</td>';
                html += '<td>' + item.FatherName + '</td>';
                html += '<td>' + item.Gender + '</td>';
                html += '<td>' + item.Age + '</td>';
                html += '<td>' + item.Mobile + '</td>';
                html += '<td><a id="MembersDetails" href="#" class="btn btn-block ink-reaction btn-flat btn-info" onclick="return getbySocietyMemberID(' + item.MemberSNo + ')">View</a>';
                html += '</tr>';
            });
            $('.tblBindMembers').html(html);
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

function getbySocietyMemberID(MemberSNo) {
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
            $('#GenderOfSocietyMember').val(getbySocietyMemberID.Gender);
            $('#Age').val(getbySocietyMemberID.Age);
            $('#OccupationOfMember').val(getbySocietyMemberID.OccupationVal);
            $('#Address1').val(getbySocietyMemberID.Address1);
            $('#Address2').val(getbySocietyMemberID.Address2);
            $('#PostOfficeOfSocietyMember').val(getbySocietyMemberID.PostOffice);
            $('#PostalCodeOfSocietyMember').val(getbySocietyMemberID.Pin);
            $('#DistrictOfMember').val(getbySocietyMemberID.DistrictName);
            $('#NoofSharesSubscribed').val(getbySocietyMemberID.NoOfShares);
            $('#NameofNominee').val(getbySocietyMemberID.NomineeName);            
            $('#NomineeAge').val(getbySocietyMemberID.NomineeAge);
            $('#RelationshipCodeOfSocietyMember').val(getbySocietyMemberID.RelationshipName);
            $('#RelationwithMember').val(getbySocietyMemberID.RelationOfMemberName);
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

function getbySocietyManagingCommittMembers(SocietyMemberID) {
    $.ajax({
        url: "/ARCS/getbyManagingCommitteeMemberID/",
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: { SocietyMemberID },
        success: function (result) {
            //alert(JSON.stringify(result));
            var getbySocietyMemberID = result[0];
            $('#SocietyMemberID').val(SocietyMemberID);
            $('#SocietyMemberName').val(getbySocietyMemberID.SocietyMemberName);
            $('#SocietyMemberDesignation').val(getbySocietyMemberID.SocietyMemberDesignationName);
            $('#RelationshipCode').val(getbySocietyMemberID.RelationshipName);
            $('#RelationshipMemberName').val(getbySocietyMemberID.RelationshipMemberName);
            $('#Address').val(getbySocietyMemberID.Address);
            $('#HouseNo').val(getbySocietyMemberID.HouseNo);
            $('#SectorStreet').val(getbySocietyMemberID.SectorStreet);
            $('#CommitteMemberDistrict').val(getbySocietyMemberID.DistrictName);
            $('#MobileNumber').val(getbySocietyMemberID.MobileNumber);
            $('#Email').val(getbySocietyMemberID.Email);
            $('#AadharNo').val(getbySocietyMemberID.AadharNo);
            $('#IsPresident').val(getbySocietyMemberID.IsPresident);
            $('#myModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//function loadManagingCommitteeMembers() {
//    var e = document.getElementById("SocietyList");
//    var SocietyTransID = e.options[e.selectedIndex].value;
//    $.ajax({
//        url: "/ARCS/ManagingCommitteMembersList/",
//        type: "GET",
//        contentType: "application/json;charset=UTF-8",
//        dataType: "json",
//        data: { SocietyTransID },
//        success: function (result) {
//            var html = '';
//            $.each(result, function (key, item) {
//                html += '<div class="col-lg-12"><div class="card"><div class="card-body">'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.SocietyMemberName + ' readonly> <label for="SocietyMemberName">1. Society Member Name</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.SocietyMemberDesignationName + ' readonly><label for="SocietyMemberDesignation">2. Society Member Designation</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.RelationshipMemberName + ' readonly> <label for="RelationshipMemberName">3. Guardian Name</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.RelationshipName + ' readonly> <label for="RelationshipCode">3.1 Relationship with Committee Member</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.Address + ' readonly> <label for="Address">4. Address</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.HouseNo + ' readonly> <label for="HouseNo">4.1 House No.</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.SectorStreet + ' readonly><label for="SectorStreet">4.2 Sector/Street</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.DistrictName + ' readonly> <label for="District">4.3 District</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.MobileNumber + ' readonly> <label for="Mobile">5. Mobile</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.Email + ' readonly><label for="Email">5.1 Email</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.AadharNo + ' readonly> <label for="AadharNo">6. AadharNo</label></div>'
//                html += '</div></div></div>'
//            });
//            $('#divCommitteMembers').html(html);
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });
//}

//function loadSocietyMembers() {
//    var e = document.getElementById("SocietyList");
//    var SocietyTransID = e.options[e.selectedIndex].value;
//    $.ajax({
//        url: "/ARCS/SocietyMembersList/",
//        type: "GET",
//        contentType: "application/json;charset=UTF-8",
//        dataType: "json",
//        data: { SocietyTransID },
//        success: function (result) {
//            var html = '';
//            $.each(result, function (key, item) {
//                html += '<div class="col-lg-12"><div class="card"><div class="card-body">'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.MemberName + ' readonly><label for="MemberName">1. Member Name</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.FatherName + ' readonly><label for="FatherName">2. Father Name</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.Gender + ' readonly><label for="SelectGender">3. Gender</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.Age + ' readonly> <label for="Age">4. Age</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.Mobile + ' readonly><label for="Mobile">5. Mobile</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.EmailId + ' readonly><label for="EmailId">6 Email</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.AadharNo + ' readonly><label for="AadharNo">7. AadharNo</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.OccupationName + ' readonly><label for="Occupationofmembers">8. Occupation of members</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.NoOfShares + ' readonly><label for="NoofSharesSubscribed">9. No. of Shares Subscribed</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.NomineeName + ' readonly <label for="NameofNominee">11. Name of Nominee</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.NomineeAge + ' readonly> <label for="NomineeAge">4. Nominee Age</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.RelationshipName + ' readonly><label for="RelationshipCode">10. Relationship with Nominee</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.Address1 + ' readonly><label for="RegisteredAddress">11. Place of Residence</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.Address2 + ' readonly><label for="HouseNoSectorNoRoad">11.1 House No. /Sector No. /Road</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.PostOffice + ' readonly><label for="PostOffice">11.2 Post Office</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.Pin + ' readonly> <label for="PostalCode">11.3 Postal Code</label></div>'
//                html += '<div class="col-md-6 form-group"> <input type="text" class="form-control" value=' + item.DistrictName + ' readonly><label for="District">11. District</label></div>'
//                html += '</div></div></div>'
//            });
//            $('#divSocietyMemberList').html(html);
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });
//}