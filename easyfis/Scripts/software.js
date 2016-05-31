function btnNoRightsOnclick() {
    if (history.length > 0) {
        window.history.back();
    } else {
        window.location = '/Manage';
    }
}

function rightsAccessValidation(userId) {
    var path = window.location.pathname;
    var currentPage = path.split("/").pop();

    $.ajax({
        url: '/api/listByMstUserId/' + userId,
        cache: false,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        success: function (userResults) {

            var userForms = new Array();

            $.ajax({
                url: '/api/listUserFormByUserId/' + userResults.Id,
                cache: false,
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                success: function (userFormResults) {
                    var currentObject;
                    if (userFormResults.length > 0) {
                        for (i = 0; i < userFormResults.length; i++) {
                            userForms.push({
                                Form: userFormResults[i]["Form"],
                            });

                            currentObject = userFormResults[i].Form.indexOf(currentPage) >= 0;

                            if (currentObject == true) {
                                break;
                            }

                        }

                        if (currentObject == false) {
                            $("#noRights").modal({
                                show: true,
                                backdrop: 'static'
                            });
                        }
                    } else {
                        $("#noRights").modal({
                            show: true,
                            backdrop: 'static'
                        });
                    }
                }
            })
        }
    });
}


$(document).ready(function () {
    // number fields
    function RemoveRougeChar(convertString) {
        if (convertString.substring(0, 1) == ",") {
            return convertString.substring(1, convertString.length)
        }
        return convertString;
    }

    $('input.numberField').on("focus", function (e) {
        var $this = $(this);
        var num = $this.val().replace(/,/g, "");
        $this.val(num);
    }).on("blur", function (e) {
        var $this = $(this);
        var num = $this.val().replace(/[^0-9]+/g, '').replace(/,/gi, "").split("").reverse().join("");
        var num2 = RemoveRougeChar(num.replace(/(.{3})/g, "$1,").split("").reverse().join(""));
        $this.val(num2);
    });
});