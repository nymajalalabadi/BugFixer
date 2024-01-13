$(function(){

    $('.accordion-layer article h2').click(function(){
        var $this = $(this);
        var $status = $(this).parents().closest('article');
        if($status.hasClass('open')){
            $status.removeClass('open');
            $this.find('i').addClass('icon-plus-sign');
            $this.find('i').removeClass('icon-minus');
            $this.parents().closest('article').find('.text').slideUp();
        }
        else{
            $status.addClass('open');
            $this.find('i').addClass('icon-minus');
            $this.find('i').removeClass('icon-plus-sign');
            $this.parents().closest('article').find('.text').slideDown();
        }
    });

});
