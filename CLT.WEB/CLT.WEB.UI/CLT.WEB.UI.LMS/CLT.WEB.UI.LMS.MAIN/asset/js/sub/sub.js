$(document).ready(function() {

    subMenuSize(); //서브메뉴 개수에 따른 사이즈 변경
    breadcrumbToggle(); //로케이션 토글
    cmtZero(); //게시판 댓글 0일 때 삭제
    subTab(); //서브페이지 탭구현

});

//서브메뉴 개수에 따른 사이즈 변경
function subMenuSize() {
    var $subMenu = $('.sub-menu > ul li').length;
    $('.sub-menu > ul').addClass('column-' + $subMenu);
}

//로케이션 토글
function breadcrumbToggle() {
    var $currentMenu = $('.breadcrumb .current');
    $currentMenu.click(function(){
        $('.menu-siblings').toggle();
        $(this).toggleClass('current-open');
    });
}

//조직현황 모바일에서 디자인 변경
function mobileOrg() {
    if($(window).width() < 1024) {
        var $gmStaff = $('.gm-staff .org-contact .org-info');
        $('.org-content').hide();

        $gmStaff
            .mouseenter(function(){
                var $staffData = $(this).find('.org-icon').attr('data-rate');
                $(this).stop().append("<div class='org-data'>" + $staffData + "</div>");
            }) .mouseleave(function(){
            $('.org-data').remove(".org-data");
        });
    }
}

//게시판 댓글 0일 때 삭제
function cmtZero() {
    $('.gm-table tr').each(function(){
        var $cmt = $(this).find('.cmt-amount');
        var $cmtTxt = $cmt.text();
        if($cmtTxt > 0) {
            $cmt.css('display', 'inline-block');
        }
    });
}

//서브페이지 탭구현
function subTab() {
    $('.sub-tab > a').click(function(){
        var $tabId =$(this).attr('data-tab');
        $('.sub-tab > a').removeClass('current');
        $('.tab-content').removeClass('current');
        $(this).addClass('current');
        $('#'+ $tabId).addClass('current');
    });
}






