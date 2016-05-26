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