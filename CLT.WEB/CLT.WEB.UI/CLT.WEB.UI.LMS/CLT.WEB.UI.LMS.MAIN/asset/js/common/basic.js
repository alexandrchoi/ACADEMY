$(document).ready(function() {


    // 전체 메뉴 토글
    $('.js-all-menu-ham').click(function () {
        $('.all-menu-box').slideToggle();
    });

    $(window).resize(function () {
        if ($(window).width() > 768) {
            $(".main-menu").show();
        } else {
            $(".main-menu").hide();
        }
    });

    // 상단 메뉴 고정
    var $header = $("header");
    $(window).scroll(function () {
        if ($(this).scrollTop() > 0) {
            $header.addClass('sticky');
        } else {
            $header.removeClass('sticky');
        }
    });

    console.log ('basic');











});