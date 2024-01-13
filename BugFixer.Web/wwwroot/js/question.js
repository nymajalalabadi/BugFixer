$(function(){
    $(".question-content-layer article .detail-layer > a, .answers-layer .answer-item .detail-layer > a").click(function (){
        $('html, body').animate({
            scrollTop: $("#submit-comment").offset().top
        }, 1000);
    });

    $('.question-content-layer article .detail-layer .share-layer > span').click(function(){
        $('.question-content-layer article .detail-layer .share-layer .share-detail').fadeToggle();
    });

    $("html,body").click(function(e) {
        if (!$(e.target).parents().hasClass('share-layer')) {
            $('.question-content-layer article .detail-layer .share-layer .share-detail').fadeOut('fast');
        }
    });

});
