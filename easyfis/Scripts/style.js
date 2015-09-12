$(document).ready(function() {
	
	// ====================
	// PAGE SCROLL FUNCTION
	// ====================
	$('a').click(function() {
	    $('html, body').animate({
	        scrollTop: $( $(this).attr('href') ).offset().top
	    }, 500);
	    return false;
	});

	// ==================================
	// TRANSPARENT NAVBAR MENU, SCROLLSPY
	// ==================================
	$(window).scroll(function() {
		// check if window scroll for more than 430px. May vary as per the height of your main banner.
		if($(this).scrollTop() > 50) { 
			$('.navbar').addClass('opaque'); // adding the opaque class
		} 
		else {
			$('.navbar').removeClass('opaque'); // removing the opaque class
		}
	});

});