<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="issuing_report.aspx.cs" Inherits="CLT.WEB.UI.LMS.EDUM.vp_a_eduming_issuing_report" %>--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="issuing_report_old.aspx.cs" Inherits="CLT.WEB.UI.LMS.EDUM.issuing_report_old" Culture="auto" UICulture="auto" %>

<%@ Import Namespace="System.Data"  %>
<%@ Import Namespace="System.Collections.Generic"  %>
<% 
    DataRowCollection drCourseResult = IDtCourseResult.Rows;
    string xReportTypeID = "";
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<style type="text/css"> 
    body {
	    font-size: 12px;
        font-family: "맑은고딕", "돋음체", "굴림체", "Verdana";
    }
    table {  border-collapse:collapse; border: 1px black solid; width:850px; font-size:15px;overflow:auto; } 
    th { font-weight:bold; background-color:#dcdcdc; border: 1px black solid; padding: 6px; } 
    td { padding: 6px; }  
	.photo { width:133px; height:180px; border:1px black solid; border-top:2px black solid; text-align:center; padding:4px;}
    .cTitle { font-weight:bold; background-color:#dcdcdc; text-align:center; }
	.cTitle1 { font-weight:bold; background-color:#dcdcdc; width:12%; }
	.cTitle2 { font-weight:bold; background-color:#dcdcdc; width:12%; }
	.cSubject { font-weight:bold; background-color:#dcdcdc; }
	h4 { padding:0; margin:0; padding-bottom:3px;}
	p { padding:0; margin:0;}
	.pPad { padding:2px; }
	.cont { line-height:25px; padding: 10px; padding-bottom: 25px; }
	@media print {
        .print { display:none; }
    }
    input.btn_search
    {   
	    font-size: 8pt;
	    letter-spacing:-1;
	    padding-left:25px;
	    padding-top:2px;
	    padding-bottom:2px;
	    background-image:url(/images/btn_search.gif);
	    border:0px;
	    width:40px;
	    height:18px;
	    cursor:pointer;
    }
</style>  
<script type="text/javascript">window.onload = function(){ this.focus(); self.print(); }</script>
</head>
<body style="margin:0px;">
    <div class="print" style="padding:10px 30px 0 0; text-align:right;"><input value="Print" class="btn_search" onclick="javascript:window.location.reload();" /></div>
    <%  for (int i = 0; i < drCourseResult.Count; i++)
        {
            xReportTypeID = Convert.ToString(drCourseResult[i]["REPORT_TYPE_ID"]);
            if (xReportTypeID == "000011")
            { 
            
            %>
            <!--디자인-->
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
            
            <% }
               else if (xReportTypeID == "000012" || xReportTypeID == "000013" || xReportTypeID == "000014" || xReportTypeID == "000015" || xReportTypeID == "000016" || xReportTypeID == "000017" || xReportTypeID == "000018")
            {   
            %>
            
<style type="text/css"> 
    @page { size: landscape; }
    table {  border-collapse:collapse; border: 1px black solid; width:1495px; font-size:15px; } 
</style>    
            <table style="border:none; height:20px;">
            <tr>
                <td>
                </td>
            </tr>
            </table>
            <div id="print_content" style="border-style:solid;border-width:1px;padding:2px; width:1495px;height:950px">
                <div style="border-style:solid;border-width:3px; padding:2px;height:940px">    
                    <div style="border-style:solid;border-width:1px;height:938px"> 
                    <table style="border:none;width:1495px;height:938px">
                        <tr>
                            <td colspan="2" style="font-size:13pt;padding-left:20px;padding-top:20px;">증 서 번 호 : <%=drCourseResult[i]["CERTIFICATE_CODE"]%><br/>(Certificate No.)</td>
                        </tr>
                        <tr>
                            <td valign="top" style=" width: 770px;">
                                <table style="border:none; border-collapse:collapse; width:100%; height:200px;">
		                        <tr>
		                            <td valign="top" style="padding-left:10px; padding-right:10px; text-align:center; border:none;  font-weight: bold;">
		                                <img src="/report/cert_2/1.gif" width="300px"/>        
		                            </td>
		                        </tr>
		                        <tr>
		                            <td></td>
		                        </tr>
		                        <tr>
		                            <td style="font-size:15pt;font-weight:bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;성&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;명 : <%=drCourseResult[i]["user_nm_kor"]%></td>
		                        </tr>
		                        <tr>
		                            <td></td>
		                        </tr>
		                        <tr>
		                            <td style="font-size:15pt;font-weight:bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;주민등록번호 : <%=drCourseResult[i]["personal_no"]%></td>
		                        </tr>
		                        </table>
                            </td>
                            <td valign="top">
                                <table style="border:none;border-collapse:collapse; width:100%;height:200px;">
		                        <tr>
		                            <td valign="top" style="padding-right:30px; text-align:right; border:none;font-weight: bold;">
		                                <img src="/report/cert_2/2.gif" width="255px"/>
		                            </td>
		                            <td rowspan="6" valign="top" style="width:200px;">
		                                <% if (!string.IsNullOrEmpty(Convert.ToString(drCourseResult[i]["pic_file_nm"])))
                                           { %>
		                                    <img src='<%=drCourseResult[i]["pic_file_nm"] %>' width="150px"/>
		                                <% } %>
		                            </td>
		                        </tr>
		                        <tr>
		                            <td></td>
		                        </tr>
		                        <tr>
		                            <td style="font-size:15pt;font-weight:bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Name : <%=drCourseResult[i]["user_nm_eng_first"]%>, <%=drCourseResult[i]["user_nm_eng_last"]%></td>
		                        </tr>
		                        <tr>
		                            <td></td>
		                        </tr>
		                        <tr>
		                            <td style="font-size:15pt;font-weight:bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ID No : <%=drCourseResult[i]["personal_no"]%></td>
		                        </tr>
		                        </table>
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="2" style="text-align:center;"><img src="/report/cert_2/3_<%=xReportTypeID %>.gif" width="1150px"/></td>
                        </tr>
                        
                        <tr>
                            <td>
                                <table style="border:none; border-collapse:collapse; padding:10px; width:100%; height:100px;">
		                        <tr>
		                            <td style="font-size:15pt;font-weight:bold;padding-left:20px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;교육기간 : <%=drCourseResult[i]["course_begin_dt"]%> ~ <%=drCourseResult[i]["course_end_dt"]%></td>
		                        </tr>
		                        <tr>
		                            <td style="font-size:15pt;font-weight:bold;padding-left:20px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;발급일자 : <%=drCourseResult[i]["course_end_dt"]%></td>
		                        </tr>
		                        </table>
                            </td>
                            <td>
                                <table style="border:none; border-collapse:collapse; padding:10px; width:100%; height:100px;">
		                        <tr>
		                            <td style="font-size:15pt;font-weight:bold;padding-right:150px;text-align:right;">From: <%=drCourseResult[i]["course_begin_dt_eng"]%> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;To: <%=drCourseResult[i]["course_end_dt_eng"]%></td>
		                        </tr>
		                        <tr>
		                            <td style="font-size:15pt;font-weight:bold;text-align:right;padding-right:150px;">Date of Issue : <%=drCourseResult[i]["course_end_dt_eng"]%></td>
		                        </tr>
		                        </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <asp:Panel ID="pnl0" Visible="true" runat="server">
                            <td style="text-align:center;">
                                <img src="/report/cert_2/4.gif" width="450px"/>
                            </td>
                            <td style="text-align:center;">
                                <img src="/report/cert_2/5.gif" width="350px"/>
                                <img src="/report/cert_2/8.jpg" width="250px"/>  
                            </td>
                            </asp:Panel>
                            <asp:Panel ID="pnl1" Visible="true" runat="server">
                            <td style="text-align:center;">
                                <img src="/report/cert_2/4.gif" width="450px"/>
                            </td>
                            <td style="text-align:center;">
                                <img src="/report/cert_2/5.gif" width="350px"/>                           
                            </td>
                            </asp:Panel>
                        </tr>
                        <tr>
                            <td colspan="2" style="height:20px;"></td>
                        </tr>
                    </table>
                    </div>
                </div>
            </div>  
            <%
            }
            %>    
            
            <% if (drCourseResult.Count != i+1)
               { %>
                <!--디자인 끝-->
                <div id="resume" style="page-break-after: always;"><!--[if gte IE 7]><br style='height:0px; line-height:0px' /><![endif]--></div>
            <% } %>
    <%  } %>

</body>
</html>
