<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="issuing_report.aspx.cs" Inherits="CLT.WEB.UI.LMS.EDUM.issuing_report" Culture="auto" UICulture="auto" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%
    DataRowCollection drCourseResult = IDtCourseResult.Rows;
    string xReportTypeID = "";
    string xCourseBeginDt = "";
    string xCourseEndDt = "";
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
    <%
    for (int i = 0; i < drCourseResult.Count; i++)
    {
        xReportTypeID = Convert.ToString(drCourseResult[i]["REPORT_TYPE_ID"]);
        xCourseBeginDt = Convert.ToString(drCourseResult[i]["course_begin_dt"]);
        xCourseEndDt = Convert.ToString(drCourseResult[i]["course_end_dt"]);
        if (xReportTypeID == "000001" || xReportTypeID == "000002" || xReportTypeID == "000003" || xReportTypeID == "000004" || xReportTypeID == "000005" || xReportTypeID == "000006")
        {
            %>

            <!--디자인1 /html/edum/certificate1-8.html -->
            <div class="cont-wrap">
            <div id="wrap">
                <div class="cert-no">
                    <span class="title">증서번호 :</span><span class="cert-no-cont"><%=drCourseResult[i]["CERTIFICATE_CODE"]%></span>
                    <p>(Certificate No.)</p>
                </div>

                <div class="cert-wrap block">

                    <!-- 한국어 -->
                    <div class="cert-box cert-ko">
                        <!-- 증서 제목 -->
                        <h1>교육수료증서</h1>

                        <!-- 수료자 정보 -->
                        <ul class="completer">
                            <li><span class="title">성<span class="block1"></span>명 :</span><span class="name-cont"><%=drCourseResult[i]["user_nm_kor"]%></span></li>
                            <li><span class="title">주민등록번호 :</span><span class="id-cont"><%=drCourseResult[i]["personal_no"]%></span></li>
                        </ul>

                        <!-- 수료 내용 -->
                        <div class="graduate-cont">
                            <%=drCourseResult[i]["report_desc_kor"]%>
                        </div>

                        <!-- 수료 날짜 -->
                        <ul class="graduate-date">
                            <li><span class="title">교육기간 :</span><span class="edu-date"><%=drCourseResult[i]["course_begin_dt"]%><span class="block2"></span>~<span class="block2"></span><%=drCourseResult[i]["course_end_dt"]%></span></li>
                            <li><span class="title">발급일자 :</span><span class="issue-date"><%=System.DateTime.Now.ToString("yyyy")+"년 "+Convert.ToInt32(System.DateTime.Now.ToString("MM"))+"월 "+Convert.ToInt32(System.DateTime.Now.ToString("dd"))+"일"%></span></li>
                        </ul>

                        <!-- 회사직인 -->
                        <footer><img src="/asset/images/report/stamp.gif"></footer>
                    </div>
                    <!--// 한국어 -->


                    <!-- 영어 -->
                    <div class="cert-box cert-en">
                        <!-- 증서 제목 -->
                        <h1>CERTIFICATE</h1>

                        <!-- 수료자 정보 -->
                        <div class="completer">
                            <ul>
                                <li><span class="title">Name :</span><span class="name-cont"><%=drCourseResult[i]["user_nm_eng_first"]%> <%=drCourseResult[i]["user_nm_eng_last"]%></span></li>
                                <li><span class="title">ID No. :</span><span class="id-cont"><%=drCourseResult[i]["personal_no"]%></span></li>
                            </ul>
                            <span class="picture">
					            <% if (!string.IsNullOrEmpty(Convert.ToString(drCourseResult[i]["pic_file_nm"])))
					                { %>
						            <img src='<%=drCourseResult[i]["pic_file_nm"] %>'/>
					            <% }
					                else
					                { %>
					                <img src="/asset/images/report/picture.jpg">
					            <% } %>
                            </span>
                        </div>

                        <!-- 수료 내용 -->
                        <div class="graduate-cont">
                            <%=drCourseResult[i]["report_desc_eng"]%>
                        </div>

                        <!-- 수료 날짜 -->
                        <ul class="graduate-date">
                            <li><span class="title">From</span><span class="edu-date"><%=drCourseResult[i]["course_begin_dt_en"]%> <span class="title">to</span><%=drCourseResult[i]["course_end_dt_en"]%></span></li>
                            <li><span class="title">Date of Issue :</span><span class="issue-date">
                                <%=System.DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture) + " " + Convert.ToInt32(System.DateTime.Now.ToString("dd")) + ", " + System.DateTime.Now.ToString("yyyy")%>
                            </span></li>
                        </ul>

                        <!-- 카피라이트 -->
                        <footer><img src="/asset/images/report/sign.gif" alt="The sign of G-Marine Service president"></footer>
                    </div>
                    <!--// 영어 -->

                </div>
            </div>
            </div>

            <%
        }
        else if (xReportTypeID == "000007" || xReportTypeID == "000008")
        {
            %>

            <!--디자인2 /html/edum/certificate9-10.html  -->
            <div class="cont-wrap">
            <div id="wrap">
                <div class="cert-no2">
                    <p><span class="title">Issue date:</span><span class="cert-no-cont">
                        <%=System.DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture) + " " + Convert.ToInt32(System.DateTime.Now.ToString("dd")) + ", " + System.DateTime.Now.ToString("yyyy")%>
                    </span></p>
                    <p><span class="title">Ref:</span><span class="cert-no-cont"><%=drCourseResult[i]["CERTIFICATE_CODE"]%></span></p>
                </div>

                <div class="jrc"><img src="/asset/images/report/jrc.gif" alt="JRC"></div>

                <div class="cert-wrap block">

                    <div class="cert-box2">
                        <h1>CERTIFICATE<span>of</span>ECDIS TYPE-SPECIFIC TRAINING</h1>
                        <p>This is to certify that</p>
                        <p class="comleter"><%=drCourseResult[i]["user_nm_eng_first"]%> <%=drCourseResult[i]["user_nm_eng_last"]%>(<%=drCourseResult[i]["user_nm_kor"]%>)</p>
                        <p>G-Marine Service Co.,Ltd.</p>
                        <p>has completed the inhouse-training as mentioned below,<br>held by</p>

                        <footer><img src="/asset/images/report/stamp.gif"></footer>
                    </div>

                    <div class="cert-box2 contents">
                        <div class="picture">
					        <% if (!string.IsNullOrEmpty(Convert.ToString(drCourseResult[i]["pic_file_nm"])))
					            { %>
						        <img src='<%=drCourseResult[i]["pic_file_nm"] %>'/>
					        <% }
					            else
					            { %>
					            <img src="/asset/images/report/picture.jpg">
					        <% } %>
                        </div>

                        <div class="contents-subject">
                            <p>The contents of onboard-training；</p>
                            <ul>
                                <li><span class="title">ECDIS Operation of model:</span><span class="subject-cont">
                                    <%=drCourseResult[i]["report_desc_eng"]%>
                                </span></li>
                                <li><span class="title">Place of Training:</span><span class="subject-cont">G-Marine Service Co., Ltd.</span></li>
                                <li><span class="title">Date of Training:</span><span class="subject-cont"><%=drCourseResult[i]["course_begin_dt_eng"]%><% if (xCourseBeginDt != xCourseEndDt) {%> ~ <%=drCourseResult[i]["course_end_dt_eng"]%><% } %></span></li>
                            </ul>
                        </div>

                        <footer><img src="/asset/images/report/sign2.gif"></footer>
                    </div>

                </div>

                <div class="remarks">Remarks: Certificate of ECDIS Type-Specific Training is valid when it comes with the copy of Certificate of ECDIS Type-Specific Trainer  attached with this certificate. JRC holds no responsibility of issuing this Certificate of ECDIS Type-Specific Training, except the certificate that JRC issues.</div>
            </div>
            </div>

            <%
        }
        else if (xReportTypeID == "000009" || xReportTypeID == "000010")
        {
            %>

            <!--디자인3 /html/edum/certificate11-13.html  -->
            <div class="cont-wrap">
            <div id="wrap" class="cert-form2">
                <div class="cert-no">
                    <span class="title">증서번호 :</span><span class="cert-no-cont"><%=drCourseResult[i]["CERTIFICATE_CODE"]%></span>
                    <p>(Certificate No.)</p>
                </div>

                <div class="cert-wrap block">

                    <!-- 한국어 -->
                    <div class="cert-box cert-ko">
                        <!-- 증서 제목 -->
                        <h1>교육수료증서</h1>

                        <!-- 수료자 정보 -->
                        <ul class="completer">
                            <li><span class="title">성<span class="block3"></span>명 :</span><span class="name-cont"><%=drCourseResult[i]["user_nm_kor"]%></span></li>
                            <li><span class="title">생년월일 :</span><span class="id-cont"><%=drCourseResult[i]["birth_dt"]%></span></li>
                        </ul>

                        <!-- 수료 내용 -->
                        <div class="graduate-cont">
                            <%=drCourseResult[i]["report_desc_kor"]%>
                        </div>

                        <!-- 수료 날짜 -->
                        <ul class="graduate-date">
					    <% if (Convert.ToString(drCourseResult[i]["COURSE_INOUT"]) == "000001" || xReportTypeID != "000010") // 사내
					        { %>
                            <li><span class="title">발급일자 :</span><span class="edu-date"><%=System.DateTime.Now.ToString("yyyy")+"년 "+Convert.ToInt32(System.DateTime.Now.ToString("MM"))+"월 "+Convert.ToInt32(System.DateTime.Now.ToString("dd"))+"일"%></span></li>
                            <li><span class="title">유효기간 :</span><span class="issue-date"><%=System.DateTime.Now.AddYears((xReportTypeID == "000009" ? 5 : 3)).ToString("yyyy")+"년 "+Convert.ToInt32(System.DateTime.Now.AddYears((xReportTypeID == "000009" ? 5 : 3)).ToString("MM"))+"월 "+Convert.ToInt32(System.DateTime.Now.AddYears((xReportTypeID == "000009" ? 5 : 3)).ToString("dd"))+"일"%></span></li>
					    <% }
					        else // 사외
					        { 
                                DateTime xEduDt = Convert.ToDateTime(drCourseResult[i]["course_end_dt"]);
                                %>
                            <li><span class="title">교육일자 :</span><span class="issue-date"><%=xEduDt.Year + "년 " + xEduDt.Month + "년 " + xEduDt.Day + "일"%></span></li>
                            <li><span class="title">발급일자 :</span><span class="edu-date"><%=System.DateTime.Now.ToString("yyyy")+"년 "+Convert.ToInt32(System.DateTime.Now.ToString("MM"))+"월 "+Convert.ToInt32(System.DateTime.Now.ToString("dd"))+"일"%></span></li>
					    <% } %>
                        </ul>

                        <!-- 회사직인 -->
                        <footer><img src="/asset/images/report/stamp.gif"></footer>
                    </div>
                    <!--// 한국어 -->


                    <!-- 영어 -->
                    <div class="cert-box cert-en">
                        <!-- 증서 제목 -->
                        <h1>CERTIFICATE</h1>

                        <!-- 수료자 정보 -->
                        <div class="completer">
                            <ul>
                                <li><span class="title">Name :</span><span class="name-cont"><%=drCourseResult[i]["user_nm_eng_first"]%> <%=drCourseResult[i]["user_nm_eng_last"]%></span></li>
                                <li><span class="title">Date of Birth :</span><span class="id-cont"><%=drCourseResult[i]["birth_dt_eng"]%></span></li>
                            </ul>
                            <span class="picture">
					            <% if (!string.IsNullOrEmpty(Convert.ToString(drCourseResult[i]["pic_file_nm"])))
					                { %>
						            <img src='<%=drCourseResult[i]["pic_file_nm"] %>'/>
					            <% }
					                else
					                { %>
					                <img src="/asset/images/report/picture.jpg">
					            <% } %>
                            </span>
                        </div>

                        <!-- 수료 내용 -->
                        <div class="graduate-cont">
                            <%=drCourseResult[i]["report_desc_eng"]%>
                        </div>

                        <!-- 수료 날짜 -->
                        <ul class="graduate-date2">
					    <% if (Convert.ToString(drCourseResult[i]["COURSE_INOUT"]) == "000001" || xReportTypeID != "000010") // 사내
					        { %>
                            <li><span class="title">Date of Issue :</span><span class="edu-date">
                                <%=System.DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture) + " " + Convert.ToInt32(System.DateTime.Now.ToString("dd")) + ", " + System.DateTime.Now.ToString("yyyy")%>
                            </span></li>
                            <li><span class="title">Date of Expire :</span><span class="issue-date">
                                <%=System.DateTime.Now.AddYears((xReportTypeID == "000009" ? 5 : 3)).ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture) + " " + Convert.ToInt32(System.DateTime.Now.AddYears((xReportTypeID == "000009" ? 5 : 3)).ToString("dd")) + ", " + System.DateTime.Now.AddYears((xReportTypeID == "000009" ? 5 : 3)).ToString("yyyy")%>
                            </span></li>
					    <% }
					        else // 사외
					        { %>
                            <li><span class="title">Date of Training :</span><span class="issue-date">
                                <%=drCourseResult[i]["course_end_dt_eng"]%>
                            </span></li>
                            <li><span class="title">Date of Issue :</span><span class="edu-date">
                                <%=System.DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture) + " " + Convert.ToInt32(System.DateTime.Now.ToString("dd")) + ", " + System.DateTime.Now.ToString("yyyy")%>
                            </span></li>
					    <% } %>
                        </ul>

                        <!-- 카피라이트 -->
                        <footer><img src="/asset/images/report/sign2.gif"></footer>
                    </div>
                    <!--// 영어 -->

                </div>
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
