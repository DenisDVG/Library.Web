var validete;
$(document).ready(function () {

    fillFilds();
    $("ul.nav-sidebar li").each(function () {
        $(this).removeClass("active");
    });
    var lementsLi = document.querySelectorAll('ul.nav-sidebar li');
    var listOfObjectsElementClass = [];

    function elementClass()
    {
        this.baseURI;
        this.clientHeight;
    }
    function elementClassProto() {
        this.clientHeight;
    }
    elementClass.prototype.__proto__ = elementClassProto;

    for (var lementLi in lementsLi) {
        if (!lementsLi.hasOwnProperty(lementLi)) {
            continue;
        }
        var elementClassItem = new elementClass();
        elementClassItem.baseURI = lementsLi[lementLi].baseURI;
        elementClassItem.clientHeight = lementsLi[lementLi].clientHeight;
        listOfObjectsElementClass.push(elementClassItem);
    }

    $("#generalActionLink").addClass("active");
    $(".bookShow").each(function () {
        $(this).hide();
    });
    $(".brochureShow").each(function () {
        $(this).hide();
    });
    $(".magazineShow").each(function () {
        $(this).hide();
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
    $("#typePublication").change(function () {
        setShowInputs();
        setValidateClass();
    });

})

validete = function () {
    var isvalidate = true;
    var isPublishingYear = isYearofBirthValid($("#PublishingYear").val());
    var isPublicationName = isEmpty($("#PublicationName").val());
    var lementsInput = document.querySelectorAll('input');
    console.log(lementsInput);

    for (var i = 0; i < lementsInput.length; i++) {
        if (!isEmpty(lementsInput[i].value))
        {
            lementsInput[i].className = lementsInput[i].className.replace(/errorClass/, "");
            lementsInput[i].className += " errorClass";
            if (!lementsInput[i].classList.contains('notValidate'))
            {
                isvalidate = false;
            }
        }
        if (isEmpty(lementsInput[i].value)) {
            lementsInput[i].className = lementsInput[i].className.replace(/errorClass/, "");
        }
    }
    if (!isPublishingYear)
    {
        document.getElementById("PublishingYear").classList.remove('errorClass');
        document.getElementById("PublishingYear").classList.add('errorClass');
    }
    if (isPublishingYear) {
        document.getElementById("PublishingYear").classList.remove('errorClass');

    }

    if (isvalidate && isPublishingYear)
    {
        $("#btnTypeSubmit").click();
    }
    
    
}

function setValidateClass()
{
    var lementsInput = document.querySelectorAll('input');
    for (var i = 0; i < lementsInput.length; i++) {



        if ($("#typePublication").val() == "1") { // Book
            if (lementsInput[i].id == "Author" || lementsInput[i].id == "NumberPages" || lementsInput[i].id == "PublishingYear") {
                lementsInput[i].classList.remove('notValidate');
            }
            if (lementsInput[i].id == "MagazineNumber" || lementsInput[i].id == "PublicationDate" || lementsInput[i].id == "TomFiling") {
                lementsInput[i].classList.add('notValidate');
            }
        }
        if ($("#typePublication").val() == "2") { // Magazine
            if (lementsInput[i].id == "MagazineNumber" || lementsInput[i].id == "PublicationDate") {
                lementsInput[i].classList.remove('notValidate');
            }
            if (lementsInput[i].id == "Author" || lementsInput[i].id == "NumberPages" || lementsInput[i].id == "PublishingYear" || lementsInput[i].id == "TomFiling") {
                lementsInput[i].classList.add('notValidate');
            }
        }
        if ($("#typePublication").val() == "3") { // Magazine
            if (lementsInput[i].id == "NumberPages" || lementsInput[i].id == "PublishingYear"|| lementsInput[i].id == "PublicationDate" || lementsInput[i].id == "TomFiling") {
                lementsInput[i].classList.remove('notValidate');
            }
            if (lementsInput[i].id == "Author" || lementsInput[i].id == "MagazineNumber") {
                lementsInput[i].classList.add('notValidate');
            }
        }
    }
}

function setShowInputs() {
    if ($("#typePublication").val() == "0") // None
    {
        $(".brochureShow").each(function () {
            $(this).hide();
        });
        $(".magazineShow").each(function () {
            $(this).hide();
        });
        $(".bookShow").each(function () {
            $(this).hide();
        });
    }
    if ($("#typePublication").val() == "1") // Book
    {
        $(".brochureShow").each(function () {
            $(this).hide();
        });
        $(".magazineShow").each(function () {
            $(this).hide();
        });
        $(".bookShow").each(function () {
            $(this).show();
        });
    }
    if ($("#typePublication").val() == "2") // Magazine
    {
        $(".bookShow").each(function () {
            $(this).hide();
        });
        $(".brochureShow").each(function () {
            $(this).hide();
        });
        $(".magazineShow").each(function () {
            $(this).show();
        });
    }
    if ($("#typePublication").val() == "3") // Brochure
    {
        $(".bookShow").each(function () {
            $(this).hide();
        });
        $(".magazineShow").each(function () {
            $(this).hide();
        });
        $(".brochureShow").each(function () {
            $(this).show();
        });
    }
}




function isYearofBirthValid(birth) {
    if (parseInt(birth) < 1400 || parseInt(birth) > new Date().getFullYear() + 1) {
        return false;
    }
    return true;
}

function isEmpty(value) {
    if (value == null || value == "") {
        return false;
    }
    return true;
}

function fillFilds() {

    //$("input:text").each(function () {
    //    $(this).val(" ");
    //});
    //$("input[type=number]").each(function () {
    //    $(this).val(0);
    //});
    var currdate = new Date();
    var currdate = (currdate.getMonth() + 1) + '/' + currdate.getDate() + '/' + currdate.getFullYear();
    var currdateOnlyYear = new Date();
    var currdateOnlyYear = currdateOnlyYear.getFullYear();
    $("#PublishingYear").val(currdateOnlyYear);
    $("#PublicationDate").val(currdate);
}