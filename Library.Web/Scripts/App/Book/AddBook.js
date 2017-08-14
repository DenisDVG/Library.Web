$(document).ready(function () {

    $("#PublishingHousesIds").val("Error");
    $("#orders").kendoMultiSelect({
        placeholder: "Select Publishing Houses...",
        dataTextField: "Name",
        dataValueField: "Id",
        height: 520,
        dataSource: {
            type: "json",
            transport: {
                read: function (e) {
                    $.ajax({
                        type: "GET",
                        url: '/Book/GetPublishingHouses',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            e.success(data);
                        },
                        error: function (data) {
                            e.error("", "400", data);
                        }
                    });
                }
            },
            schema: {
                model: {
                    fields: {
                        Id: { type: "string" },
                        Name: { type: "string" }
                    }
                }
            },
            pageSize: 80,
            serverPaging: true,
            serverFiltering: true
        }
    });

    $("#orders").change(function () {
        var multiselect = $("#orders").data("kendoMultiSelect");
        var dataItem = multiselect.dataItems();
        var iDs = [];
        for (var i = 0; i < dataItem.length; i++) {
            iDs.push(dataItem[i].Id);
        }
        console.log(iDs);
        $("#PublishingHousesIds").val(iDs);
});



})
