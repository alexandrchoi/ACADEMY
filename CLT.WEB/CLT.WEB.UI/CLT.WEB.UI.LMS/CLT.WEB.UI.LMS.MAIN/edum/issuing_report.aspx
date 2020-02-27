<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="issuing_report.aspx.cs" Inherits="CLT.WEB.UI.LMS.EDUM.issuing_report" Culture="auto" UICulture="auto" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%
    DataRowCollection drCourseResult = IDtCourseResult.Rows;
    string xReportTypeID = "";
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
        body {
              margin: 0;
             }
    </style>
    <style type="text/css" media="print">
        .landScape {
            width: 100%;
            height: 100%;
            margin: 0% 0% 0% 0%;
            filter: progid:DXImageTransform.Microsoft.BasicImage(Rotation=3);
        }
    </style>
    <script type="text/javascript">
        window.onload = function () {
            this.focus();
            self.print();
        }
    </script>
</head>
<body class="landScape">
    <%
    for (int i = 0; i < drCourseResult.Count; i++)
    {
        xReportTypeID = Convert.ToString(drCourseResult[i]["REPORT_TYPE_ID"]);
        if (xReportTypeID == "000001")
        {
            %>

            <!--디자인1-->
            <table style="height:1320px">
                <tr>
                    <td style="padding-left:35px; padding-top:25px; font-size:13pt; font-weight: bold;">
                        Certificate No : <%=drCourseResult[i]["CERTIFICATE_CODE"]%>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left:35px; padding-top:5px; font-size:13pt; font-weight: bold;">
                        Date of Issue : <%=System.DateTime.Now.ToString("dd") %>th <%=System.DateTime.Now.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture)%>, <%=System.DateTime.Now.ToString("yyyy") %>
                    </td>
                </tr>
                <tr>
                    <td style="height:10px;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="border:none; border-collapse:collapse; padding:10px; width:100%; height:200px;">
		                <tr>
			                <td valign="middle" style="padding-left:10px; padding-right:10px; text-align:right; border:none;">
			                    <img src="/report/cert_1/1.gif" width="500px"/>        
			                </td>
			                <td valign="top" style="padding-left:20px; padding-right:5px; text-align:left; border:none; width:180px;">
			                    <% if (!string.IsNullOrEmpty(Convert.ToString(drCourseResult[i]["pic_file_nm"])))
                                    { %>
			                        <img src='<%=drCourseResult[i]["pic_file_nm"] %>' width="150px"/>        
			                    <%} %>
			                </td>
		                </tr>
		                </table>
                    </td>
                </tr>
                <tr>
                    <td style="height:20px;">
                    </td>
                </tr>
                
                <tr>
                    <td style="padding-left:35px;">
                        <table style="border:none;border-collapse:collapse; padding-left:10px; width:100%; height:auto;">
		                <tr>
			                <td style="width:250px">
			                    <span style="font-size:13pt; font-weight: bold;">Name :</span> &nbsp;&nbsp;<span style="font-size:13pt;"><%=drCourseResult[i]["user_nm_eng_last"]%>, <%=drCourseResult[i]["user_nm_eng_first"]%></span>
			                </td>
			                <td style="font-size:13pt;width:70px;font-weight:bold; padding-left:20px;">
			                    Rank : 
			                </td>
			                <td style="width:150px;font-size:13pt;width:200px;">
			                    <%=drCourseResult[i]["step_name"]%>
			                </td>
			                <td>
			                    
			                </td>
			                <td>
			                    
			                </td>
		                </tr>
		                <tr>
			                <td>
			                    <span style="font-weight: bold;font-size:13pt;">Date of Birth :</span> <span style="font-size:13pt;"><%=Convert.ToString(drCourseResult[i]["birth"]) %></span>
			                </td>
			                <td style="font-size:13pt;font-weight:bold;padding-left:20px;">
			                    ID No :
			                </td>
			                <td>
			                    <span style="font-size:13pt;"><%=drCourseResult[i]["personal_no"]%></span>
			                </td>
			                <td style="font-size:13pt; font-weight: bold; w">
			                    Nationality : 
			                </td>
			                <td style="width:150px;font-size:13pt;">
			                    <%=drCourseResult[i]["COUNTRY_KIND_NM"]%>
			                </td>
		                </tr>
		                </table>
                    </td>
                </tr>
                <tr>
                    <td style="height:20px;">
                    </td>
                </tr>
                <tr>
                    <td style="padding-left:35px;">
                        <img src="/report/cert_1/2.gif" width="600px" />        
                    </td>
                </tr>
                <tr>
                    <td style="height:20px;">
                    </td>
                </tr>
                <tr>
                    <td valign="bottom" style="border:none; border-collapse:collapse; padding-left:25px; width:auto; height:auto;text-align:left;">
                        <table style="border:none; border-collapse:collapse;"><tr><td style="width:330px;"><img src="/report/cert_1/3.gif" width="350px" style="vertical-align: bottom;"/></td><td><span style="width:auto; height:auto;text-align:left;font-size:14pt;font-weight: bold;text-decoration:underline;"><%=drCourseResult[i]["course_begin_dt_eng"]%></span> &nbsp;<span style="width:auto; height:auto;text-align:left;font-size:11pt;font-weight: bold;text-decoration:underline;">to</span>&nbsp; <span style="width:auto; height:auto;text-align:left;font-size:14pt;font-weight: bold;text-decoration:underline;"><%=drCourseResult[i]["course_end_dt_eng"]%></span></td></tr></table>       
                    </td>
                </tr>
                <tr>
                    <td style="border:none; border-collapse:collapse; padding:10px; width:auto; height:auto;text-align:center;">
                        <img src="/report/cert_1/4.gif" width="750px"/>        
                    </td>
                </tr>
                <tr>
                    <td style="height:20px;">
                    </td>
                </tr>
                <tr>
                    <td style="border:none; border-collapse:collapse; padding:10px; width:auto; height:auto;text-align:center;">
                        <table style="border:none; border-collapse:collapse; padding:10px; width:100%; height:250px;">
		                <tr>
		                    <td valign="top" style="padding-left:10px; padding-right:10px; text-align:right; border:none;  font-weight: bold;">
		                        <img src="/report/cert_1/5.gif" width="300px"/>        
		                    </td>
			                <td valign="bottom" style="padding-left:10px; padding-right:10px; text-align:left; border:none;  font-weight: bold;">
			                    <img src="/report/cert_1/6.gif" width="300px"/>        
			                </td>
		                </tr>
		                </table>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;">
                    </td>
                </tr>
            </table>

            <%
        }
        else if (xReportTypeID == "000002" || xReportTypeID == "000003" || xReportTypeID == "000004" || xReportTypeID == "000005" ||
                 xReportTypeID == "000006" || xReportTypeID == "000007" || xReportTypeID == "000008")
        {
            %>

            <!--디자인2-->
            <div id="wrap">
                <div class="cert-no">
                    <span class="title">No.</span><span class="cert-no-cont"><%=drCourseResult[i]["CERTIFICATE_CODE"]%></span>
                </div>

                <div class="cert-wrap block">

                    <!-- 한국어 -->
                    <div class="cert-box cert-ko">
                        <!-- 증서 제목 -->
                        <h1>교육수료증서</h1>

                        <!-- 수료자 정보 -->
                        <ul class="completer">
                            <li><span class="title">성명 :</span><span class="name-cont"><%=drCourseResult[i]["user_nm_kor"]%></span></li>
                            <li><span class="title">주민등록번호 :</span><span class="id-cont"><%=drCourseResult[i]["personal_no"]%></span></li>
                        </ul>

                        <!-- 수료 내용 -->
                        <div class="graduate-cont">
				            <%=xReportTypeID %>
                            위 사람은 지마린서비스 아카데미 운항훈련원에서 실시한 <strong class="subject">기관실자원관리(ERM)</strong> 교육을 수료하였음을 증명합니다. 위 사람은 지마린서비스 아카데미 운항훈련원에서 실시한 <strong class="subject">기관실자원관리(ERM)</strong> 교육을 수료하였음을 증명합니다. 위 사람은 지마린서비스 아카데미 운항훈련원에서 실시한 <strong class="subject">기관실자원관리(ERM)</strong> 교육을 수료하였음을 증명합니다.
                        </div>

                        <!-- 수료 날짜 -->
                        <ul class="graduate-date">
                            <li><span class="title">교육기간 :</span><span class="edu-date"><%=drCourseResult[i]["course_begin_dt"]%> ~ <%=drCourseResult[i]["course_end_dt"]%></span></li>
                            <li><span class="title">발급일자 :</span><span class="issue-date"><%=drCourseResult[i]["course_end_dt"]%></span></li>
                        </ul>

                        <!-- 원장 직인 -->
                        <span class="stamp"><img src="/asset/images/report/stamp.png" alt="원장 직인"></span>
                        <footer>지마린아카데미 운항훈련원장</footer>
                    </div>
                    <!--// 한국어 -->


                    <!-- 영어 -->
                    <div class="cert-box cert-en">
                        <!-- 증서 제목 -->
                        <h1>CERTIFICATE</h1>

                        <!-- 수료자 정보 -->
                        <div class="completer">
                            <ul>
                                <li><span class="title">Name :</span><span class="name-cont"><%=drCourseResult[i]["user_nm_eng_first"]%>, <%=drCourseResult[i]["user_nm_eng_last"]%></span></li>
                                <li><span class="title">ID No. :</span><span class="id-cont"><%=drCourseResult[i]["personal_no"]%></span></li>
                            </ul>
                            <span class="picture">
					            <% if (!string.IsNullOrEmpty(Convert.ToString(drCourseResult[i]["pic_file_nm"])))
					                { %>
						            <img src='<%=drCourseResult[i]["pic_file_nm"] %>' width="150px"/>
					            <% }
					                else
					                { %>
					                <img src="/asset/images/report/picture.jpg">
					            <% } %>
				            </span>
                        </div>

                        <!-- 수료 내용 -->
                        <div class="graduate-cont">
                            This is to certify that the above <strong class="subject">BLA BLA BLAHHH</strong> Bla blahhhh blah.
                        </div>

                        <!-- 수료 날짜 -->
                        <ul class="graduate-date">
                            <li><span class="title">From :</span><span class="edu-date"><%=drCourseResult[i]["course_begin_dt_eng"]%> <span class="title">To :</span><%=drCourseResult[i]["course_end_dt_eng"]%></span></li>
                            <li><span class="title">Date of issue :</span><span class="issue-date"><%=drCourseResult[i]["course_end_dt_eng"]%></span></li>
                        </ul>

                        <!-- 해수부 로고 -->
                        <span class="mof"><img src="/asset/images/report/logo-mof.png" alt="원장 직인"></span>
                        <!-- 카피라이트 -->
                        <footer>
                            <span class="title">President</span>
                            <span class="sign"><img src="/asset/images/report/signature.png" alt="대표 서명"></span>
                            <p class="copyright">
                                G-Marine Service Co., Ltd.
                                <span class="designed">Designated by Ministry of Oceans and Fisheries Republic of Korea.</span>
                            </p>
                        </footer>
                    </div>
                    <!--// 영어 -->

                </div>
            </div>

            <%
        }

        if (drCourseResult.Count != i+1)
        {
            %>

            <!--디자인 끝-->
            <div id="resume" style="page-break-after: always;"><!--[if gte IE 7]><br style='height:0px; line-height:0px' /><![endif]--></div>

            <%
        }
    }
    %>
</body>
</html>
