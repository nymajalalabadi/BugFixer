$(function () {
    $('.profile-page .user-information .main-tabs li a').click(function(){
        $('.profile-page .user-information .main-tabs li').removeClass('active');
        $('.profile-page .tab-content-layer').css('display','none');
        $(this).parent('li').addClass('active');
        var target = $(this).attr('class');
        $('#'+target).fadeIn('fast');
    });
});