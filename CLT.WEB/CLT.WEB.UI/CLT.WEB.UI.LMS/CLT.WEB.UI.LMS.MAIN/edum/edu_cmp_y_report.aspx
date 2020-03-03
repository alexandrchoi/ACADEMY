<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edu_cmp_y_report.aspx.cs" Inherits="CLT.WEB.UI.LMS.EDUM.edu_cmp_y_report" %>
<%@ Import Namespace="System.Data"  %>
<%@ Import Namespace="System.Collections.Generic"  %>
<% 
    DataRowCollection drCourseResult = IDtCourseResult.Rows;
%>
<!doctype html>
<html lang="ko">
<head runat="server">
    <title>교육수료증서</title>
    <meta http-equiv="Content-Type" content="text/html" charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge">
    <link rel="stylesheet" href="/asset/css/report/certificate.css" type="text/css" media="all">
    <style type="text/css">
        @page { size: landscape; }
    </style>
    <script type="text/javascript">
        window.onload = function () {
            this.focus();
            self.print();
        }
    </script>
</head>
<body>
    <!--div class="print" style="padding:10px 30px 0 0; text-align:right;"><input value="Print" class="btn_search" onclick="javascript:window.location.reload();" /></div-->
    <%  for (int i = 0; i < drCourseResult.Count; i++)
        {   
    %>
        <!--디자인 /html/edum/certificate14.html  -->
        <div class="cont-wrap">
            <div id="wrap" class="cert-form3">
                <div class="cert-wrap">
                    <!-- 증서 제목 -->
                    <h1><span>수료증</span>Certificate</h1>

                    <!-- 수료자 정보 -->
                    <ul class="completer">
                        <li><span class="title">성<span class="block4"></span>명(Name) :</span><span class="name-cont"><%=Convert.ToString(drCourseResult[i]["USER_NM_KOR"])%>(<%=Convert.ToString(drCourseResult[i]["USER_NM_ENG_FIRST"])%> <%=Convert.ToString(drCourseResult[i]["USER_NM_ENG_LAST"])%>)</span></li>
                        <li><span class="title space1">생년월일(ID No.) :</span><span class="id-cont"><%=Convert.ToString(drCourseResult[i]["BIRTH_DT"])%></span></li>
                        <li><span class="title">훈련과정명(Course) :</span><span class="course-cont"><%=Convert.ToString(drCourseResult[i]["COURSE_NM"])%></span></li>
                        <li><span class="title">훈련기관(Period) :</span><span class="period-cont"><%=Convert.ToString(drCourseResult[i]["COURSE_BEGIN_DT_ENG"])%> ~ <%=Convert.ToString(drCourseResult[i]["COURSE_END_DT_ENG"])%></span></li>
                    </ul>

                    <!-- 수료 내용 -->
                    <div class="graduate-cont">
                        <p>위 사람은 지마린서비스가 실시한 훈련과정을 수료하였음을 증명합니다.</p>
                        <p>This is to certify That the above mentioned person has successfully completed<br>the training course, applicable to issue of this certificate.</p>
                    </div>

                    <!-- 수료 날짜 -->
                    <div class="graduate-date">
                        <span class="title">교육수료일(Date of Issue) :</span><span class="edu-date"><%=Convert.ToString(drCourseResult[i]["APPROVAL_DT_ENG"])%></span>
                    </div>

                    <!-- 회사직인 -->
                    <footer class="block"><span class="stamp"><img src="/asset/images/report/stamp.gif"></span><span class="sign"><img src="/asset/images/report/sign2.gif"></span></footer>

                </div>
            </div>
        </div>
    <%  } %>

</body>
</html>
