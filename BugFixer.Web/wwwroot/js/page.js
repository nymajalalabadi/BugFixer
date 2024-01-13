$(function () {
    $('.post-blog-layer header .category-dropdown').click(function(){
        var $this = $(this);
        $(this).find('ul:first').slideToggle();        
    });
});