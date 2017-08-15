    $(document).ready(function () {
        $("ul.nav-sidebar li").each(function () {
            $(this).removeClass("active");
        });
        $("#magazineActionLink").addClass("active");

        $("#MyGrid").kendoGrid({
            dataSource: {
                pageSize: 10,
                transport: {
                    read: function (e) {
                        $.ajax({
                            type: "GET",
                            url: '/Magazine/GetMagazines',
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
                field: "Type",
                title: "Type"
            }, {
                field: "Name",
                title: "Name"
            }, {
                field: "MagazineNumber",
                title: "Magazine Number"
            }, {
                field: "Actions",
                title: "Actions",
                template: "<a href='/Magazine/Edit/${Id}'>Edit</a> | <a class='deleteLink' href='/Magazine/Delete/${Id}'>Delete</a>"
            }]
        });
    })



