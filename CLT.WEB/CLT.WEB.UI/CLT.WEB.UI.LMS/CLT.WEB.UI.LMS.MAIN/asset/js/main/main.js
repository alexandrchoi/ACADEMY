$(document).ready(function(){
    visHeight(); //비주얼 높이 측정

    // Top Ocean Leader 스크롤시 숫자 변경
    $(window).scroll(startCounter);
    function startCounter() {
        if ($(window).scrollTop() > 1500) {
            $(window).off("scroll", startCounter);
            $('.numeric').each(function () {
                var $this = $(this);
                $({ Counter: 0 }).animate({ Counter: $this.text() }, {
                    duration: 1000,
                    easing: 'swing',
                    step: function () {
                        $this.text(Math.ceil(this.Counter));
                    }
                });
            });
        }
    }
});

function visHeight() {
    var $contentHeight = $('.tab-content').height();
    var $boardHeight = $('.tab').height();

    $('.main-board-tab').height($contentHeight + $boardHeight);

    var $leftheight = $('.main-board').height();
    $('.main-board-box').height($leftheight + 40);
    $('.main-visual-box, .main-visual').height($leftheight + 220);
}