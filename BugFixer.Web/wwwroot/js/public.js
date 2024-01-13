$(function () {
    $('.main-header .clientarea-layer .loggedin-status > span').click(function(){
        $('.main-header nav .inner > ul').fadeOut();
        $('.main-header .clientarea-layer .loggedin-status .sublayer').slideToggle();
    });
    $('.main-header nav > span').click(function(){
        $('.main-header nav .inner > ul').slideToggle();
        $('.main-header .clientarea-layer .loggedin-status .sublayer').fadeOut();
    });

});