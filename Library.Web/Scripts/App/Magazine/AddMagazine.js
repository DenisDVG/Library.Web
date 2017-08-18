var a = 1;
function b() {
    a = 10;
    return;
    function a() { }
}
b();
console.log(a); // 1



$(document).ready(function () {
    $("#btnTypeButton").click(function () {
        validete();
    });
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
                        url: '/Magazine/GetPublishingHouses',
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


    function validete() {
    var isvalidate = true;
    var isPublicationDate = validateDate($("#PublicationDate").val());
    var isPublicationName = isEmpty($("#PublicationName").val());
    var lementsInput = document.querySelectorAll('input');
    console.log(lementsInput);

    for (var i = 0; i < lementsInput.length; i++) {
        if (!isEmpty(lementsInput[i].value)) {
            lementsInput[i].className = lementsInput[i].className.replace(/errorClass/, "");
            lementsInput[i].className += " errorClass";
            isvalidate = false;
        }
        if (isEmpty(lementsInput[i].value)) {
            lementsInput[i].className = lementsInput[i].className.replace(/errorClass/, "");
        }
    }

    if (!isPublicationDate) {
        document.getElementById("PublicationDate").classList.remove('errorClass');
        document.getElementById("PublicationDate").classList.add('errorClass');
    }
    if (isPublicationDate) {
        document.getElementById("PublicationDate").classList.remove('errorClass');
    }

    if (isvalidate && isPublicationDate) {
        $("#btnTypeSubmit").click();
    }


}


function isEmpty(value) {
    if (value == null || value == "") {
        return false;
    }
    return true;
}

function validateDate(testdate) {
    var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
    return date_regex.test(testdate);
}