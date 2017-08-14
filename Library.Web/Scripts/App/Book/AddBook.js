$(document).ready(function () {

    //$("#orders").kendoMultiSelect({
    //    placeholder: "Select addresses...",
    //    dataTextField: "ShipName",
    //    dataValueField: "OrderID",
    //    height: 520,
    //    virtual: {
    //        itemHeight: 26,
    //        valueMapper: function (options) {
    //            $.ajax({
    //                url: "https://demos.telerik.com/kendo-ui/service/Orders/ValueMapper",
    //                type: "GET",
    //                dataType: "jsonp",
    //                data: convertValues(options.value),
    //                success: function (data) {
    //                    options.success(data);
    //                }
    //            })
    //        }
    //    },
    //    dataSource: {
    //        type: "odata",
    //        transport: {
    //            read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Orders"
    //        },
    //        schema: {
    //            model: {
    //                fields: {
    //                    OrderID: { type: "number" },
    //                    Freight: { type: "number" },
    //                    ShipName: { type: "string" },
    //                    OrderDate: { type: "date" },
    //                    ShipCity: { type: "string" }
    //                }
    //            }
    //        },
    //        pageSize: 80,
    //        serverPaging: true,
    //        serverFiltering: true
    //    }
    //});
    $("#PublishingHousesIds").val("Error");
    $("#orders").kendoMultiSelect({
        placeholder: "Select Publishing Houses...",
        dataTextField: "Name",
        dataValueField: "Id",
        height: 520,
        //virtual: {
        //    itemHeight: 26,
        //    valueMapper: function (options) {
        //        $.ajax({
        //            url: "https://demos.telerik.com/kendo-ui/service/Orders/ValueMapper",
        //            type: "GET",
        //            dataType: "jsonp",
        //            data: convertValues(options.value),
        //            success: function (data) {
        //                options.success(data);
        //            }
        //        })
        //    }
        //},
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
        // get data items for the selected options.
        var dataItem = multiselect.dataItems();
        var iDs = [];
        for (var i = 0; i < dataItem.length; i++) {
            iDs.push(dataItem[i].Id);
        }
        console.log(iDs);
        $("#PublishingHousesIds").val(iDs);
});



})

function convertValues(value) {
    var data = {};

    value = $.isArray(value) ? value : [value];

    for (var idx = 0; idx < value.length; idx++) {
        data["values[" + idx + "]"] = value[idx];
    }

    return data;
}