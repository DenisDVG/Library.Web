

var valueEdit;

$(document).ready(function () {
    $("#btnTypeButton").click(function () {
        validete();
    });
    $("#PublishingHousesIds").val("Error");
    

    $.when(
    $.ajax({
        type: "GET",
        url: '/Magazine/GetPublishingHousesForEdit?id=' + $("#itemId").val(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            valueEdit = data;
            console.log(valueEdit)
        },
        error: function (data) {

        }
    })

).then(function() {

    $("#orders").kendoMultiSelect({
        value: valueEdit,
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
                            setValue();
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

});


    $("#orders").change(function () {
        setValue();
    });



})


function setValue() {
    var multiselect = $("#orders").data("kendoMultiSelect");
    var dataItem = multiselect.dataItems();
    var iDs = [];
    for (var i = 0; i < dataItem.length; i++) {
        iDs.push(dataItem[i].Id);
    }
    console.log(iDs);
    $("#PublishingHousesIds").val(iDs);
}

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
            //k-input
            if (!lementsInput[i].classList.contains("k-input")) {
                isvalidate = false;
            }
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