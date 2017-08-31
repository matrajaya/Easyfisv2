// =====================================================
// Input for Number Field - Decimal and Digits (Numbers)
// =====================================================
$('input.numberField').on("blur", function (e) {
    nStr = this.value;
    nStr += '';

    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';

    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    var $this = $(this);

    var realValue = (x1 + x2);
    var thisValue = (x1 + x2).replace(/\,/g, '');
    var returnValue = 0;

    if (thisValue % 1 != 0) {
        // count the number of decimal digits
        if ((realValue.split('.')[1] || []).length == 1) {
            returnValue = parseFloat(thisValue).toFixed(2).toLocaleString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        } else {
            returnValue = realValue.toLocaleString(undefined, { maximumFractionDigits: 5 });
        }
    } else {
        returnValue = parseFloat(thisValue).toFixed(2).toLocaleString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    $this.val(returnValue);
});

// ========================================
// Input for Number Field - Key Restriction
// ========================================
$("input.numberField").keydown(function (e) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190, 189]) !== -1 ||
        // Allow: Ctrl+A
        (e.keyCode == 65 && e.ctrlKey === true) ||
        // Allow: Ctrl+C
        (e.keyCode == 67 && e.ctrlKey === true) ||
        // Allow: Ctrl+X
        (e.keyCode == 88 && e.ctrlKey === true) ||
        // Allow: home, end, left, right
        (e.keyCode >= 35 && e.keyCode <= 39)) {
        // let it happen, don't do anything
        return;
    }

    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
});
