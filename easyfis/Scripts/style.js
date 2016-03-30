$(document).ready(function() {
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

});