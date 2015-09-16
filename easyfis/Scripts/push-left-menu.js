var pushLeft = new Menu({
    wrapper: '#wrapper',
    type: 'push-left',
    menuOpenerClass: '.c-button',
    maskId: '#c-mask'
});


var pushLeftBtn = document.querySelector('#c-button--push-left');
pushLeftBtn.addEventListener('click', function (e) {
    e.preventDefault;
    pushLeft.open();
});