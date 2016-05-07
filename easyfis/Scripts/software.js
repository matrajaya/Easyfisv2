// ===========
// read Cookie
// ===========
function readCookieBranch(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) {
                return c.substring(nameEQ.length, c.length);
            }
        }
    }
    return null;
}

// =========================
// Diplay company and branch
// =========================
function getCompanyAndBranch() {
    document.getElementById('currentCompanyName').innerHTML = readCookieBranch("company");
    document.getElementById('currentBranchName').innerHTML = readCookieBranch("branch");
}


$(document).ready(function () {
    getCompanyAndBranch();

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

    //$('.nav a').click(function () {
    //    //Toggle Class
    //    $(".active").removeClass("active");
    //    $(this).closest('li').addClass("active");
    //    var theClass = $(this).attr("class");
    //    $('.' + theClass).parent('li').addClass('active');
    //    //Animate
    //    $('html, body').stop().animate({
    //        scrollTop: $($(this).attr('href')).offset().top - 50
    //    }, 400);
    //    return false;
    //});
    //$('.scrollTop a').scrollTop();

    //var slideLeft = new Menu({
    //    wrapper: '#o-wrapper',
    //    type: 'slide-left',
    //    menuOpenerClass: '.c-button',
    //    maskId: '#c-mask'
    //});

    //var slideLeftBtn = document.querySelector('#c-button--slide-left');
    //slideLeftBtn.addEventListener('click', function(e) {
    //    e.preventDefault;
    //    slideLeft.open();
    //});

});