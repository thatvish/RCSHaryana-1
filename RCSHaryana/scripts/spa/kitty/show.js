(function ($) {
    "use strict";

    var cust = {
        
    }

    var generateCustomerTable = $("#list-kitty")
        .dataTable({
            "processing": true,
            "serverSide": true,
            "ajax": {
                "url": apiURL + "api/kitty",
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




