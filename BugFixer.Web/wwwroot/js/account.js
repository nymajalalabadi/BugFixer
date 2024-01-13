$(function(){

    $('#description-field').keyup(function () {
        var max = 500;
        var len = $(this).val().length;
        if (len >= max) {
            $('#charNum').text('تعداد کاراکترها به حد نساب رسیده است');
        } else {
            var char = max - len;
            $('#charNum').text(char + ' کاراکتر مجاز ');
        }
    });

    $('.skills-layer-scroll > .inner').slimScroll({
        railVisible: true,
        position: 'left',
        size: '3px',
        height: '100%',
        distance: '4px',
        railColor: '#000',
        railOpacity: 0,
        alwaysVisible: false
    });
    $('.project-category-layer-scroll > .inner').slimScroll({
        railVisible: true,
        position: 'left',
        size: '3px',
        height: '100%',
        distance: '4px',
        railColor: '#000',
        railOpacity: 0,
        alwaysVisible: false
    });

    $('[data-toggle="tooltip"]').tooltip();

    $('.account-layer .right-side .sidebar > header').click(function(){
        var $this = $(this);
        var $status = $(this).parent('.sidebar');
        if($status.hasClass('open')){
            $status.removeClass('open');
            $this.find('i').addClass('icon-chevron-down');
            $this.find('i').removeClass('icon-chevron-up');
            $this.parent('.sidebar').find('.inner').slideDown();
        }
        else{
            $status.addClass('open');
            $this.find('i').addClass('icon-chevron-up');
            $this.find('i').removeClass('icon-chevron-down');
            $this.parent('.sidebar').find('.inner').slideUp();
        }
    });
    $('.account-layer .right-side .sidebar > header').click(function(){
        var $this = $(this);
        var $status = $(this).parent('.sidebar');
        if($status.hasClass('close-inner')){
            $status.removeClass('close-inner');
            $this.find('i').addClass('icon-chevron-down');
            $this.find('i').removeClass('icon-chevron-up');
            $this.parent('.sidebar').find('.inner').slideDown();
        }
        else{
            $status.addClass('close-inner');
            $this.find('i').addClass('icon-chevron-up');
            $this.find('i').removeClass('icon-chevron-down');
            $this.parent('.sidebar').find('.inner').slideUp();
        }
    });
    $('.employer-project-status .heading').click(function(){
        var $this = $(this);
        var $status = $(this).parent('.employer-project-status');
        if($status.hasClass('close-inner')){
            $status.removeClass('close-inner');
            $this.find('.close-status i').addClass('icon-chevron-down');
            $this.find('.close-status i').removeClass('icon-chevron-up');
            $this.parent('.employer-project-status').find('.status-bar').slideDown();
        }
        else{
            $status.addClass('close-inner');
            $this.find('.close-status i').addClass('icon-chevron-up');
            $this.find('.close-status i').removeClass('icon-chevron-down');
            $this.parent('.employer-project-status').find('.status-bar').slideUp();
        }
    });

    $('.all-skills-items-layer > .inner > ul > li > a').click(function(){
        var $this = $(this);
        var $status = $(this).parent('li');
        if($status.hasClass('close-inner')){
            $status.removeClass('close-inner');
            $this.find('span i').addClass('icon-minus');
            $this.find('span i').removeClass('icon-plus-sign');
            $this.parent('li').find('ul:first').slideDown();
        }
        else{
            $status.addClass('close-inner');
            $this.find('span i').addClass('icon-plus-sign');
            $this.find('span i').removeClass('icon-minus');
            $this.parent('li').find('ul:first').slideUp();
        }
    });

    $('.all-category-items-layer > .inner > ul > li > a').click(function(){
        var $this = $(this);
        var $status = $(this).parent('li');
        if($status.hasClass('close-inner')){
            $status.removeClass('close-inner');
            $this.find('span i').addClass('icon-minus');
            $this.find('span i').removeClass('icon-plus-sign');
            $this.parent('li').find('ul:first').slideDown();
        }
        else{
            $status.addClass('close-inner');
            $this.find('span i').addClass('icon-plus-sign');
            $this.find('span i').removeClass('icon-minus');
            $this.parent('li').find('ul:first').slideUp();
        }
    });

    $('.notification-row .message-layer .text p').click(function(){
        var $this = $(this);
        var message = $this.parents().closest('.notification-row').find('.message-text');
        $(message).slideToggle();
    });

    $( '.educational-documents-layer .educational-item' ).on( 'keydown', function ( event ) {
        var $this = $(this);
        var $educational = $('.educational-result').html()
        if( event.keyCode == 13 && $this.val().length != 0 )
            {
                $('.educational-result').append('<div class="input-item"> <a href="javascript:void(0)"><i class="icon-delete"></i></a> <label> <input type="checkbox" name="educational[]" value="'+$this.val()+'" checked="checked" > '+$this.val()+' </label> </div>');
                $this.val('');
            }
    }); 

    $(document).on("click",".educational-result .input-item a",function(){
        $(this).parent().remove();
    });

    $( '.favorites-layer .favorites-item' ).on( 'keydown', function ( event ) {
        var $this = $(this);
        var $educational = $('.favorites-result').html()
        if ( event.keyCode == 13 && $this.val().length != 0 )
            {
                $('.favorites-result').append('<div class="input-item"> <a href="javascript:void(0)"><i class="icon-delete"></i></a> <label> <input type="checkbox" name="favorites[]" value="'+$this.val()+'" checked="checked" > '+$this.val()+' </label> </div>');
                $this.val('');
            }
    }); 

    $(document).on("click",".favorites-result .input-item a",function(){
        $(this).parent().remove();
    });

    $( '.tags-layer .tags-item' ).on( 'keydown', function ( event ) {
        var $this = $(this);
        var $educational = $('.tags-result').html()
        if ( event.keyCode == 13 && $this.val().length != 0 )
            {
                $('.tags-result').append('<div class="input-item"> <a href="javascript:void(0)"><i class="icon-delete"></i></a> <label> <input type="checkbox" name="tags[]" value="'+$this.val()+'" checked="checked" > '+$this.val()+' </label> </div>');
                $this.val('');
            }
    }); 

    $(document).on("click",".tags-result .input-item a",function(){
        $(this).parent().remove();
    });

    $( '.technology-layer .technology-item' ).on( 'keydown', function ( event ) {
        var $this = $(this);
        var $educational = $('.technology-result').html()
        if ( event.keyCode == 13 && $this.val().length != 0 )
            {
                $('.technology-result').append('<div class="input-item"> <a href="javascript:void(0)"><i class="icon-delete"></i></a> <label> <input type="checkbox" name="technology[]" value="'+$this.val()+'" checked="checked" > '+$this.val()+' </label> </div>');
                $this.val('');
            }
    }); 

    $(document).on("click",".technology-result .input-item a",function(){
        $(this).parent().remove();
    });

    $('.all-category-items-layer > .inner > ul > li ul li label').click(function(){
        var $this = $(this);
        var $text = $this.html();
        var $for = $this.attr('for');
        $('.project-category-result').append('<div class="input-item"> <a href="javascript:void(0)"><i class="icon-delete"></i></a> <label> <input type="checkbox" name="technology[]" value="'+$for+'" checked="checked" > '+$text+' </label> </div>');
    })
    $(document).on("click",".project-category-result .input-item a",function(){
        var a = $(this).parents().closest('.input-item').find('input').val();
        $('.all-category-items-layer #'+a).prop('checked', false);
        $(this).parent().remove();
    });

    $('.all-skills-items-layer > .inner > ul > li ul li label').click(function(){
        var $this = $(this);
        var $text = $this.html();
        var $for = $this.attr('for');
        $('.overlay').fadeIn();
        $('.skills-popup').fadeIn();
        $('.skills-popup .inner .skill-percent span').html('').html($text);
        $('.skills-popup .inner input[name="select-category-id"]').val($for);
    })

    $(document).on("click",".skills-popup .inner .submit-button",function(){
        var $categoryID = $('.skills-popup .inner input[name="select-category-id"]').val();
        var $categoryName = $('.skills-popup .inner .skill-percent span').html();
        var $percent = $('.skills-popup .inner .skill-percent .percent-layer .percent-bar .number input:checked').val();
        $('.skills-result').append('<div class="input-item"> <a href="javascript:void(0)"><i class="icon-delete"></i></a> <label> <input type="checkbox" name="skills[]" value="'+$categoryID+'" checked="checked" > '+$categoryName+' </label> <span> %'+$percent+'</span> </div>');
        $('.overlay').fadeOut();
        $('.skills-popup').fadeOut();
        $('.skills-popup .inner .skill-percent .percent-layer .percent-bar .number input').prop('checked', false);
        $('.skills-popup .inner .skill-percent .percent-layer .percent-bar .number #percent-a').prop('checked', true);
    });

    $(document).on("click",".skills-result .input-item a",function(){
        var a = $(this).parents().closest('.input-item').find('input').val();
        $('.all-skills-items-layer #'+a).prop('checked', false);
        $(this).parent().remove();
    });

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
    /*progress*/
    var ProgressRange = $('#progress').attr('data-range');
    $('#progress').circleProgress({
      value: ProgressRange,
      size: 80,
      thickness :10,
      fill: {
        gradient: ["#7DD841", "#5C9E31"]
      }
    });

    $('.account-layer .right-side .sidebar .inner > ul > li').each(function(){
        if($(this).find('ul').length > 0) {
            $(this).append('<label class="sign-dropdown"><i class="icon-chevron-down"></i></label>');
        } 
    });
    
    $('.account-layer .right-side .sidebar .inner ul li > span').click(function(){
        var $this = $(this);
        var $parent = $(this).parent('li');
        if($parent.hasClass('open')){
            $parent.removeClass('open');
            $parent.find('.sign-dropdown i').addClass('icon-chevron-up');
            $parent.find('.sign-dropdown i').removeClass('icon-chevron-down');
            $parent.find('ul:first').slideUp();
        }
        else{
            $parent.addClass('open');
            $parent.find('.sign-dropdown i').addClass('icon-chevron-down');
            $parent.find('.sign-dropdown i').removeClass('icon-chevron-up');
            $parent.find('ul:first').slideDown();
        }
    });

    $('.account-layer .right-side .sidebar .inner ul li > .sign-dropdown').click(function(){
        var $this = $(this);
        var $parent = $(this).parent('li');
        if($parent.hasClass('open')){
            $parent.removeClass('open');
            $parent.find('.sign-dropdown i').addClass('icon-chevron-up');
            $parent.find('.sign-dropdown i').removeClass('icon-chevron-down');
            $parent.find('ul:first').slideUp();
        }
        else{
            $parent.addClass('open');
            $parent.find('.sign-dropdown i').addClass('icon-chevron-down');
            $parent.find('.sign-dropdown i').removeClass('icon-chevron-up');
            $parent.find('ul:first').slideDown();
        }
    });

});
