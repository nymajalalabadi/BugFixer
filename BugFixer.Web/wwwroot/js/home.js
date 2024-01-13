$(function () {
    $('.top-menu .clientarea-layer .loggedin-status > span').click(function(){
        $('.top-menu.responsive nav ul').fadeOut();
        $('.top-menu .clientarea-layer .loggedin-status .sublayer').slideToggle();
    });
    $('.top-menu nav > span').click(function(){
        $('.top-menu nav ul').slideToggle();
        $('.top-menu .clientarea-layer .loggedin-status .sublayer').fadeOut();
    });

    function checkMenu() {
        if (window.matchMedia('(max-width: 991px)').matches) {
            $('.top-menu').addClass('responsive');
            $('.top-menu nav ul').css('display','none');
        } else {
            $('.top-menu nav ul').css('display','block');
            $('.top-menu').removeClass('responsive');
        }
    }

    checkMenu();
    
    $(window).resize(function(event) { 
        event.preventDefault();
        checkMenu();
    });
});