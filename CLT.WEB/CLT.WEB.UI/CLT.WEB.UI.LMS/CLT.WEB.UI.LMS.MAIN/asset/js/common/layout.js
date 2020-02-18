$(document).ready(function(){

    headerDesign(); //헤더 디자인 변경 PC & 모바일
    gmScroll(); //스크롤시 헤더 디자인 변경 & 상단 이동 버튼 제어
    primaryMenu(); //메인메뉴
    allMenu(); //전체메뉴
    familySite(); //패밀리사이트 토글
    modalPopup(); //모달 팝업
    buttonChildren(); //버튼 자식체크
    buttonSibling(); //버튼 형제체크
    attachFiles(); //파일첨부 인풋 기능구현

});


//헤더 디자인 변경 PC & 모바일
function headerDesign() {
    $('body#sub').find('.js-gm-header').addClass('gm-header-dark');

    if($(window).width() <= 1280){
        $('body#main').find('.js-gm-header').addClass('gm-header-light');
    }
}

//스크롤시 헤더 디자인 변경 & 상단 이동 버튼 제어
function gmScroll() {
    $(window).scroll(function() {
        if($(this).scrollTop() > 300){
            $('.js-gm-header').addClass('header-fixed');
            $('body#sub .js-gm-header').removeClass('gm-header-dark');
            $('.move-top').fadeIn();
        } else {
            $('.js-gm-header').removeClass('header-fixed');
            $('body#sub .js-gm-header').addClass('gm-header-dark');
            $('.move-top').fadeOut();
        }
    });

    $( '.move-top' ).click( function() {
        $( 'html, body' ).animate( { scrollTop : 0 }, 400 );
        return false;
    } );
}

//메인메뉴
function primaryMenu() {
    $(".gnb > ul > li")
        .mouseenter(function() {
            var depHeight = $('.depth-1').height();
            $('.js-gm-header').addClass('open');
            $('.js-gm-header.open').height(120 + depHeight);
            $(this).addClass('current');
            $('.gnb .depth-1').show();
        })
        .mouseleave(function() {
            $('.js-gm-header').removeClass('open');
            $(this).removeClass('current');
            $('.gnb .depth-1').hide();
            /*
            if($(this).scrollTop() > 500) {
                $('#main .js-gm-header').removeClass('header-light');
            } else if($(this).scrollTop() < 500) {
                $('#main .js-gm-header').addClass('header-light');
            }
            */
            $('.js-gm-header').height(100);
        });


    /*20200101 GNB 2차 메뉴 높이 변경 */
    var $menuArray = new Array();
    $('.gnb > ul > li').each(function(){
        var $menuAmount = $(this).find('.depth-1 > li').length;
        $menuArray.push($menuAmount);
        var $menuMath = Math.max.apply(null, $menuArray);

        $('.gnb .depth-1').height($menuMath * 40);
    });
}

//전체메뉴
function allMenu() {
    $('.js-all-menu-ham').on('click', function(e) {
        e.preventDefault();
        $('.all-menu-box').fadeIn();
        $('body').addClass('fixed');
    });
    $('.button-close-all-menu').on('click', function(e) {
        $('.all-menu-box').fadeOut();
        $('body').removeClass('fixed');
    });

    $(window).on("load",function(){
        $(".all-menu-box").mCustomScrollbar();
    });

    $(window).resize(function () {
        if ($(window).width() > 768) {
            $(".main-menu").show();
        } else {
            $(".main-menu").hide();
        }
    });
}

//패밀리사이트 토글
function familySite() {
    $('.family-site').click(function(){
        $(this).toggleClass('opened');
        $('.family-link').toggle();
    });
}

//모달 팝업
function modalPopup() {
    var $body = $('#sub');
    var $popup = $('#modal');

    $('.show-popup').click(function(){
        if(!$body.hasClass('open-popup')) {
            $body.addClass('open-popup');
            $popup.fadeIn();
        } else {
            $body.removeClass('open-popup');
            $popup.fadeOut();
        }
    });
    $('.close-popup').click(function(){
        $body.removeClass('open-popup');
        $popup.fadeOut();
    });
}

//버튼 자식체크
function buttonChildren() {
    var $buttonChild = $('.button-box').children().length;
    if($buttonChild == 0) {
        $('.PageInfo').css('padding-bottom', '7px');
    }
}

//버튼 형제체크
function buttonSibling() {
    $('.table-option').prev('.button-box').css('margin-top', '0');
}

//파일첨부 인풋 기능구현
function attachFiles() {
    var fileTarget = $('.file-box .upload-hidden');
    fileTarget.on('change', function(){ // 값이 변경되면
        if(window.FileReader){ // modern browser
            var filename = $(this)[0].files[0].name;
        } else { // old IE
            var filename = $(this).val().split('/').pop().split('\\').pop(); // 파일명만 추출
        }

        // 추출한 파일명 삽입
        $(this).siblings('.upload-name').val(filename);
    });
}


