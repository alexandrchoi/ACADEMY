<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CLT.WEB.UI.LMS.MAIN.Schedule" 
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    
    <!--link rel='stylesheet' id='mize-style-css' href='/asset/FullCalendar/css/style.css?ver=4.8.11' type='text/css' media='all' /-->

</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

<script type='text/javascript' src='/asset/FullCalendar/js/jquery.js?ver=1.12.4'></script>

<link rel='stylesheet' href='/asset/FullCalendar/css/style.css' type='text/css' media='all'>

<!-- 메인 컨텐츠 -->
    <div class="section-fix">
        <div  class="page-template-default page page-id-95 page-child parent-pageid-89 group-blog wpb-js-composer js-comp-ver-4.11.2.1 vc_responsive">

        <!-- Contents -->
        <div class="contents no-side">
            <div class="article-wrap">

                <article id="post-95" class="post-95 page type-page status-publish hentry page_tag-148" role="article">


                    <div class="entry-content group">

                        <div id='calendar'></div>

                    </div>


                </article>
            </div>
        </div>
        <!-- //Contents -->



        <link rel='stylesheet' id='full-calendar-css' href='/asset/FullCalendar/css/fullcalendar.min.css?ver=1.0.0' type='text/css' media='all' />


        <script type='text/javascript' src='/asset/FullCalendar/js/moment.min.js?ver=1'></script>
        <script type='text/javascript' src='/asset/FullCalendar/js/fullcalendar.min.js?ver=1'></script>
        <script type='text/javascript'>

            var vColor = new Array('#D25565', '#9775fa', '#ffa94d', '#74c0fc', '#f06595', '#63e6be', '#a9e34b', '#4d638c', '#495057');

            function randomItem(a) {
                return a[Math.floor(Math.random() * a.length)];
            }

            var calendar = {
                "ajaxurl": "\/asset\/FullCalendar\/get_edu_list.aspx",
                "edu_url": "\/application/courseapplication_detail.aspx",
                "event_url": "\/community\/edu_notice_detail.aspx",
                "backgroundColor": randomItem(vColor),
                "lang": "kr",
            };

        </script>
        <script type='text/javascript' src='/asset/FullCalendar/js/calendar.js?ver=1'></script>

        </div>
    </div>

</asp:Content>
