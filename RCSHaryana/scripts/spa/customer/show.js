(function ($) {
    "use strict";

    var cust = {
        //loadCustomers: function () {
        //    $('#list-customers').dataTable({
        //        destroy: true,
        //        //searching: false,
        //        //"paging": true,
        //        //"info": true,
        //        processing: true,
        //        serverSide: true,
        //        "aLengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],

        //        "ajax": {
        //            "url": apiURL + "api/customers/search",
        //            "type": "POST",
        //            "dataSrc": ""
        //        },
        //        "columns": [
        //            {
        //                "data": "FirstName", "sortable": true, "render": function (url, type, full) {
        //                    return '<a href="/customers/details?id=' + full.ID + '"><span class="text-info">' + full.FirstName + ' ' + full.LastName + '</span></a>';
        //                }
        //            },
        //            {
        //                "data": "Mobile", "sortable": false
        //            },
        //            {
        //                "data": "RegistrationDate",
        //                "render": function (data) {
        //                    return moment(data).format('DD/MM/YYYY');
        //                }
        //            },
        //            {
        //                "data": "City"
        //            },
        //            {
        //                "data": null, "sortable": false, "render": function (url, type, full) {
        //                    return '<a href="/customers/edit?id=' + full.ID + '"><i class="fa fa-edit fa-lg link-icon"></i></a>';
        //                }
        //            }
        //        ],
        //        "pagingType": "full_numbers"
        //    });
        //}
    }

    var generateCustomerTable = $("#list-customers")
        .dataTable({
            "processing": true,
            "serverSide": true,
            "ajax": {
                "url": apiURL + "api/customers",
                "method": "Get"
            },
            "columns": [
                { "data": "FirstName" },
                { "data": "LastName" },
                { "data": "City" }
            ],
            "ordering": true,
            "paging": true,
            "pagingType": "full_numbers",
            "pageLength": 3
        });

    
}(jQuery));




