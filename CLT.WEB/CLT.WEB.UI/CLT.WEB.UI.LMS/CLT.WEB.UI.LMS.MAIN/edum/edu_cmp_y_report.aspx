<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edu_cmp_y_report.aspx.cs" Inherits="CLT.WEB.UI.LMS.EDUM.edu_cmp_y_report" %>
<%@ Import Namespace="System.Data"  %>
<%@ Import Namespace="System.Collections.Generic"  %>
<% 
    DataRowCollection drCourseResult = IDtCourseResult.Rows;
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
    @media print {
        .print { display:none; }
    }
</style>  
<script type="text/javascript">window.onload = function(){ this.focus(); self.print(); }</script>
</head>
<body style="margin:0px;">
    <div class="print" style="padding:10px 30px 0 0; text-align:right;"><input value="Print" class="btn_search" onclick="javascript:window.location.reload();" /></div>
    <%  for (int i = 0; i < drCourseResult.Count; i++)
        {   
    %>
        <div id="print_content" style="border-style:solid;border-width:1px;padding:2px;width:850px;height:1290px">
            <div style="border-style:solid;border-width:1px; padding:2px;width:843px;height:1284px">    
            <table style="border:none; border-collapse:collapse; width:100%;">
                <tr>
                    <td style="height:50px;"></td>
                </tr>
                <tr>
                    <td style="height:200px;font-size:40pt; font-weight: bold;text-align:center;">수&nbsp;&nbsp;&nbsp;료&nbsp;&nbsp;&nbsp;증</td>
                </tr>
                <tr>
                    <td style="height:30px;"></td>
                </tr>
                <tr>
                    <td style="padding-left:40px;">
                        <table style="border:none;width:100%; height:250px;font-weight: bold;font-size:18pt; ">
		                <tr>
			                <td style="padding-left:10px;width:35%">
			                    성&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;명&nbsp;&nbsp;&nbsp;:
			                </td>
			                <td style="padding-left:20px;padding-right:5px; border:none;">
			                    <%=Convert.ToString(drCourseResult[i]["USER_NM_KOR"])%>
			                </td>
		                </tr>
		                <tr>
			                <td style="padding-left:10px; padding-right:10px; border:none;">
			                    주 민 등 록 번 호&nbsp;&nbsp;&nbsp;:
			                </td>
			                <td style="padding-left:20px; padding-right:5px;border:none;">
			                    <%=Convert.ToString(drCourseResult[i]["PERSONAL_NO"])%>
			                </td>
		                </tr>
		                <tr>
			                <td style="padding-left:10px; padding-right:10px; border:none;">
			                    사&nbsp;&nbsp;&nbsp;업&nbsp;&nbsp;&nbsp;장&nbsp;&nbsp;&nbsp;명&nbsp;&nbsp;&nbsp;:
			                </td>
			                <td style="padding-left:20px; padding-right:5px; text-align:left; border:none;">
			                    <%=Convert.ToString(drCourseResult[i]["COMPANY_NM"])%>
			                </td>
		                </tr>
		                <tr>
			                <td style="padding-left:10px; padding-right:10px; border:none;">
			                    훈&nbsp;&nbsp;련&nbsp;&nbsp;과&nbsp;&nbsp;정&nbsp;&nbsp;명&nbsp;&nbsp;:
			                </td>
			                <td style="padding-left:20px; padding-right:5px; border:none;">
			                    <%=Convert.ToString(drCourseResult[i]["COURSE_NM"])%>
			                </td>
		                </tr>
		                <tr>
			                <td style="padding-left:10px; padding-right:10px;border:none;">
			                    훈&nbsp;&nbsp;&nbsp;련&nbsp;&nbsp;&nbsp;기&nbsp;&nbsp;&nbsp;간&nbsp;&nbsp;&nbsp;:
			                </td>
			                <td style="padding-left:20px; padding-right:5px; border:none;">
			                    <%=Convert.ToString(drCourseResult[i]["COURSE_BEGIN_END_DT"])%>
			                </td>
		                </tr>
		                </table>
                    </td>
                </tr>
                <tr>
                    <td style="height:100px;"></td>
                </tr>
                <tr>
                    <td style="border:none;width:100%; height:200px;font-weight: bold;font-size:17pt; line-height:55px; padding-left:45px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;위 사람은 한진해운 운항훈련원에서 실시한 상기 교육<br/>훈련과정을 수료하였으므로 이 증서를 수여합니다.</td>
                </tr>
                <tr>
                    <td style="height:70px;"></td>
                </tr>
                <tr>
                    <td style="border:none;width:100%; height:80px;font-weight: bold;font-size:17pt;text-align:right;padding-right:220px;"><%=Convert.ToString(drCourseResult[i]["AGREE_DATETIME"])%></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td style="border:none;width:100%; height:50px;font-weight: bold;font-size:25pt;text-align:right; vertical-align:middle; padding-right:100px;">한진해운 운항훈련원 원장<img src="/report/sign01.jpg" style="vertical-align:middle;"/></td>
                </tr>
            </table>
            </div>
        </div>  
    <%  } %>

</body>
</html>
