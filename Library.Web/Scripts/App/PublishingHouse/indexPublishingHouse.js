$(document).ready(function () {


    $("#MyGrid").kendoGrid({
        dataSource: {
            pageSize: 10,
            transport: {
                read: function (e) {
                    $.ajax({
                        type: "GET",
                        url: '/PublishingHouse/GetPublishingHouses',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            e.success(data);
                            console.log(data);
                        },
                        error: function (data) {
                            e.error("", "400", data);
                        }
                    });
                }
            }
        },
        height: 300,
        sortable: true,
        pageable: true,
        filterable: true,
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        columns: [{
            field: "PublishingHouseName",
            title: "Name"
        }, {
            field: "Actions",
            title: "Actions",
            template: "<a href='/PublishingHouse/Edit/${Id}'>Edit</a> | <a class='deleteLink' href='/PublishingHouse/Delete/${Id}'>Delete</a>"
        }]
    });
})

