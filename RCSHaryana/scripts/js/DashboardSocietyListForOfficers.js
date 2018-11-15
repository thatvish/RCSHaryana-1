//Load Data in Table when documents is ready
$(document).ready(function () {
    var Role = document.getElementById("Role").value;
    if (Role === "2") {
        loadDataForARCSDashBoardSocietyList();
    }
    else if (Role === "3") {
        loadDataForInspectorDashBoardSocietyList();
    }
});

//Load Data function
function loadDataForARCSDashBoardSocietyList() {
    $.ajax({
        url: "/ARCS/ARCSDashBoardSocietyList",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td hidden="hidden">' + item.SocietyName + '</td>';
                html += '<td hidden="hidden">' + item.SocietyTransID + '</td>';
                html += '<td>' + item.SocietyName + '</td>';
                html += '<td>' + item.AreaOfOperation + '</td>';
                html += '<td>' + item.Mainobject1 + '</td>';
                html += '<td>' + item.NoOfMembers + '</td>';
                html += '<td>' + item.TypeofSociety + '</td>';
            });
            $('.SocietyListForOfficer').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadDataForInspectorDashBoardSocietyList() {
    $.ajax({
        url: "/Inspector/InspectorDashBoardSocietyList",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td hidden="hidden">' + item.SocietyName + '</td>';
                html += '<td hidden="hidden">' + item.SocietyTransID + '</td>';
                html += '<td>' + item.SocietyName + '</td>';
                html += '<td>' + item.AreaOfOperation + '</td>';
                html += '<td>' + item.Mainobject1 + '</td>';
                html += '<td>' + item.NoOfMembers + '</td>';
                html += '<td>' + item.TypeofSociety + '</td>';
            });
            $('.SocietyListForOfficer').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
