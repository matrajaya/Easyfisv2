$(document).ready(function () {
    // Collapse 1 in set ups
    $('#collapse1').on('shown.bs.collapse', function () {
        $(".fa-drop1").addClass('fa-chevron-up').removeClass('fa-chevron-down');
    });

    $('#collapse1').on('hidden.bs.collapse', function () {
        $(".fa-drop1").addClass('fa-chevron-down').removeClass('fa-chevron-up');
    });

    // collapse 2 in systmes
    $('#collapse2').on('shown.bs.collapse', function () {
        $(".fa-drop2").addClass('fa-chevron-up').removeClass('fa-chevron-down');
    });

    $('#collapse2').on('hidden.bs.collapse', function () {
        $(".fa-drop2").addClass('fa-chevron-down').removeClass('fa-chevron-up');
    });

    // collapse 3 in activity
    $('#collapse3').on('shown.bs.collapse', function () {
        $(".fa-drop3").addClass('fa-chevron-up').removeClass('fa-chevron-down');
    });

    $('#collapse3').on('hidden.bs.collapse', function () {
        $(".fa-drop3").addClass('fa-chevron-down').removeClass('fa-chevron-up');
    });

    // collapse 4 in reports
    $('#collapse4').on('shown.bs.collapse', function () {
        $(".fa-drop4").addClass('fa-chevron-up').removeClass('fa-chevron-down');
    });

    $('#collapse4').on('hidden.bs.collapse', function () {
        $(".fa-drop4").addClass('fa-chevron-down').removeClass('fa-chevron-up');
    });

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