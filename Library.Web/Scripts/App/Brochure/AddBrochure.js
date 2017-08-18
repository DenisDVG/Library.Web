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
                        url: '/Brochure/GetPublishingHouses',
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
    var isPublishingYear = isYearofBirthValid($("#PublishingYear").val());
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

    if (!isPublishingYear) {
        document.getElementById("PublishingYear").classList.remove('errorClass');
        document.getElementById("PublishingYear").classList.add('errorClass');
    }
    if (isPublishingYear) {
        document.getElementById("PublishingYear").classList.remove('errorClass');
    }

    if (isvalidate && isPublishingYear) {
        $("#btnTypeSubmit").click();
    }


}
function isEmpty(value) {
    if (value == null || value == "") {
        return false;
    }
    return true;
}
function isYearofBirthValid(birth) {
    if (parseInt(birth) < 1400 || parseInt(birth) > new Date().getFullYear() + 1 || birth == "") {
        return false;
    }
    return true;
}