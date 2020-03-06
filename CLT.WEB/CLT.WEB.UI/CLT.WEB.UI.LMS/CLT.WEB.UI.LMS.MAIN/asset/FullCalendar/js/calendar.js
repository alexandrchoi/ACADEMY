/**
 * Created by leehakje on 2016. 5. 3..
 */
( function( $ ) {
    $(document).ready(function() {

        var vColor = new Array('#D25565', '#9775fa', '#ffa94d', '#f7e2dd', '#f06595', '#63e6be', '#a9e34b', '#4d638c', '#495057');

        function randomItem(a) {
            return a[Math.floor(Math.random() * a.length)];
        }

        if ( $( "input[name=calendar_day]" ).length > 0 ) {
            $( "input[name=calendar_day]" ).datepicker({
                dateFormat: 'yy-mm-dd'
            });
        }

        if ( $( "#calendar" ).length > 0 ) {

            var kr = [];
            if(calendar.lang=="kr") {
                kr['monthNames'] = ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'];
                kr['dayNamesShort'] = ['일', '월', '화', '수', '목', '금', '토'];
                kr['views'] = {
                    month: {
                        titleFormat: 'YYYY년 M월'
                    },
                    basicWeek: {
                        titleFormat: 'YYYY년 M월 D일'
                    },
                    basicDay: {
                        titleFormat: 'YYYY년 M월 D일'
                    }
                }
            }
            $('#calendar').fullCalendar({
                header: {
                    left: 'prev',
                    center: 'title',
                    right: 'next month basicWeek today' //basicDay
                },
                monthNames: kr['monthNames'] ,
                dayNamesShort: kr['dayNamesShort'],
                views: kr['views'],
                //defaultDate: new Date(2016, 11),
                defaultDate: new Date(),
                editable: false,
                eventLimit: true, // allow "more" link when too many events
                events: function(start, end, timezone, callback) {
                    $.ajax({
                        url: calendar.ajaxurl,
                        dataType: 'json',
                        type: 'post',
                        data: {
                            'date'          : moment($('#calendar').fullCalendar('getDate')).format('YYYY-MM'),
                            //'param1'        : calendar.param1,
                        },
                        success: function (data) {
                            $.each(data, function (index) {
                                //if (data[index].id == "16020013") {
                                //    alert(data[index].backgroundColor + "\r\n" + calendar.backgroundColor);
                                //    data[index].backgroundColor = calendar.backgroundColor;

                                    //data[index].backgroundColor = randomItem(vColor);

                                //}
                            });
                            callback(data);
                        },
                        complete: function (data) {

                            //
                        },
                        error: function (xhr, status, error) {

                            //alert("에러발생");
                        }
                    });
                }, eventRender: function(event, element) {
                    element.attr("type",event.type);
                    element.attr("id", event.id);
                    element.attr("description", event.description);
                    element.attr("objective", event.objective);
                    element.attr("place", event.place);
                    element.attr("approval_code", event.approval_code);
                    element.attr("backgroundColor", calendar.backgroundColor);


                }, eventClick: function(event, element) {
                    if( event['type'] == "edu") {
                        var url = "";
                        url = calendar.edu_url + "?ropen_course_id=" + event['id'] + "&approval_code=" + event['approval_code'] + "&MenuCode=431";
                    } else// if( event['type'] == "event")
                    {
                        url = calendar.event_url + "?rseq=" + event['seq'] +"&MenuCode=610";
                    }
                    //window.open(url, "_blank");
                    location.href = url;
                    return false;
                }
                /*
                , eventMouseover: function (data, event, view) {
                    var tooltip = "<a class='tooltipevent fc-day-grid-event fc-h-event fc-event fc-start fc-end fc-noticein fc-draggable fc-resizable'><ul>"
                        + "<li>" + $(this).html() + "</li>"
                    if (data["description"] != null)
                        tooltip += "<li style='margin-left:5px; margin-right:15px;'>" + data["description"] + "</li>";
                    if (data["description"] != data["objective"] && data["objective"] != null)
                        tooltip += "<li style='margin-left:5px; margin-right:15px;'>" + data["objective"] + "</li>"
                    tooltip += "</ul></a>";
                    $("body").append(tooltip);
                    $(this).mouseover(function(e) {
                        $('.tooltipevent').fadeIn('500');
                    }).mousemove(function(e) {
                        $('.tooltipevent').css('top', e.pageY + 10);
                        $('.tooltipevent').css('left', e.pageX + 20);
                    }).mouseleave(function(e) {
                         $('.tooltipevent').remove();
                    });
                }
                */
                //, eventOrder: "eventOrder"
                //, eventOrder: "-id"
                , eventOrder: "-type, id, cid, seq, title"

            });

            /*
             $(document).on('mouseenter', ".fc-event", function(e) {
                 $( this ).css("position" , "absolute");
             });

            $(document).on('mouseleave', ".fc-event", function(e) {
              $( this ).css("position" , "");
           });
           */

        }
    });
} )( jQuery );