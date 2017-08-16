var record = 0;

$(document).ready(function () {

    $("#MyGrid").kendoGrid({
        dataSource: {
            pageSize: 10,
            transport: {
                read: function (e) {
                    $.ajax({
                        type: "GET",
                        url: '/Home/GetPublications',
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
            title: "#",
            template: "#= ++record #"
        }, {
            field: "Name",
            title: "Name"
        }, {
            field: "Type",
            title: "Type"
        }],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

})






$('#homeMy a').click(function (e) {
    e.preventDefault()
    $(this).tab('show')
})
$('#profileMy a').click(function (e) {
    e.preventDefault()
    $(this).tab('show')
})