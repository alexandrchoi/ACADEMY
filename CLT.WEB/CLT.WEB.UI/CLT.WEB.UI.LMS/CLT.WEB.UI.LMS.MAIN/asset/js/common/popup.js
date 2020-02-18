$(document).ready(function(){

    buttonChildren(); //버튼 자식체크
    buttonSibling(); //버튼 형제체크
    attachFiles(); //파일첨부 인풋 기능구현
    cmtZero(); //게시판 댓글 0일 때 삭제
    subTab(); //서브페이지 탭구현
});



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



