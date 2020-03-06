<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="lecturer_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.lecturer_edit" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script language="javascript" type="text/javascript">
    document.domain = "academy.gmarineservice.com";
    function fnValidateForm()
	{
	    // 필수 입력값 체크
	    // 강사명
	    if (isEmpty(document.getElementById('<%=txtLecturerNm.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblLecturerNm.Text }, new string[] { lblLecturerNm.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if (isEmpty(document.getElementById('<%=ddlGrade.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblGrade.Text }, new string[] { lblGrade.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if (isEmpty(document.getElementById('<%=txtJob.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblJob.Text }, new string[] { lblJob.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if (isEmpty(document.getElementById('<%=txtEducation.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblEducation.Text }, new string[] { lblEducation.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if (isEmpty(document.getElementById('<%=txtMajor.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblMajor.Text }, new string[] { lblMajor.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if (isEmpty(document.getElementById('<%=txtOrgNm.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblOrgNm.Text }, new string[] { lblOrgNm.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if (isEmpty(document.getElementById('<%=txtCompanyNm.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCompanyNm.Text }, new string[] { lblCompanyNm.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if (isEmpty(document.getElementById('<%=ddlDutyStep.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblDutyStep.Text }, new string[] { lblDutyStep.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    // 우편번호
	    //if (isEmpty(document.getElementById('<%=txtZipCode.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblZipCode.Text }, new string[] { lblZipCode.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    // 주소
	    //if (isEmpty(document.getElementById('<%=txtAddr1.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAddr.Text }, new string[] { lblAddr.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    //if (isEmpty(document.getElementById('<%=txtAddr2.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAddr.Text }, new string[] { lblAddr.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    // 전화번호
	    if (isEmpty(document.getElementById('<%=txtPhone.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPhone.Text }, new string[] { lblPhone.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    
	    if (isEmpty(document.getElementById('<%=txtMobile.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblMobile.Text }, new string[] { lblMobile.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

        // 길이 체크	    
        if(isMaxLenth(document.getElementById('<%=txtPhone.ClientID %>'),20,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { txtPhone.Text,"20","13" }, new string[] { txtPhone.Text,"20","13" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if(isMaxLenth(document.getElementById('<%=txtMobile.ClientID %>'),20,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] {txtMobile.Text,"20","13" }, new string[] {txtMobile.Text,"20","13" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
	    return true;
	}    
	
    function jusoCallBack(roadFullAddr,roadAddrPart1,addrDetail,roadAddrPart2,engAddr,jibunAddr,zipNo){
	    document.getElementById('<%=txtAddr1.ClientID %>').value = roadAddrPart1 + " " + roadAddrPart2;
	    document.getElementById('<%=txtAddr2.ClientID %>').value = addrDetail;
        document.getElementById('<%=txtZipCode.ClientID %>').value = zipNo;
    }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <asp:HiddenField ID="hidLecturerID" runat="server" /> 

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container Lecturer-register user-edit">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="강사 등록/조회" meta:resourcekey="lblMenuTitle" /></h3>
    
        
        <div class="notice-text right"><asp:Label ID="lblRequired" runat="server" Text="필수입력항목" meta:resourcekey="lblRequired" /><span class="required"></span></div>

        <div class="gm-table data-table read-type">
                
            <table>
                <colgroup>
                    <col width="15%">
                    <col width="35%">
                    <col width="15%">
                    <col width="35%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblLecturerNm" runat="server" Text="강사명" meta:resourcekey="lblLecturerNm" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtLecturerNm" runat="server" CssClass="w100per" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblLecturerEngNm" runat="server" Text="강사명(영문)" meta:resourcekey="lblLecturerEngNm" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtLecturerEngNm" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblBirthDt" runat="server" Text="생년월일" meta:resourcekey="lblBirthDt" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtBirthDt" runat="server" CssClass="datepick" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblGrade" runat="server" Text="강사등급" meta:resourcekey="lblGrade" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlGrade" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblJob" runat="server" Text="직업" meta:resourcekey="lblJob" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtJob" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblOrgNm" runat="server" Text="소속기관" meta:resourcekey="lblOrgNm" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtOrgNm" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblEducation" runat="server" Text="학력" meta:resourcekey="lblEducation" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtEducation" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblMajor" runat="server" Text="전공" meta:resourcekey="lblMajor" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMajor" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblCompanyNm" runat="server" Text="회사명" meta:resourcekey="lblCompanyNm" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtCompanyNm" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblDutyStep" runat="server" Text="직위" meta:resourcekey="lblDutyStep" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlDutyStep" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblPhone" runat="server" Text="전화번호(- 포함)" meta:resourcekey="lblPhone" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="w100per" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblMobile" runat="server" Text="휴대폰(- 포함)" meta:resourcekey="lblMobile" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblEmail" runat="server" Text="E-Mail" meta:resourcekey="lblEmail" />
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="w80per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblBank" runat="server" Text="은행명" meta:resourcekey="lblBank" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtBank" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblAccount" runat="server" Text="계좌번호" meta:resourcekey="lblAccount" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtAccount" runat="server" />
                    </td>
                </tr>

                <tr>
                    <th scope="row">
                        <asp:Label ID="lblZipCode" runat="server" Text="우편번호" meta:resourcekey="lblZipCode" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtZipCode" runat="server" />
                        <input type="button" id="btnFindAddr" class="button-default blue" value="<%=GetLocalResourceObject("btnFindAddr.Text").ToString() %>"
                                                    onclick="openPopWindow('/common/zipcode.aspx?opener_textZipCode_id=<%=txtZipCode.ClientID %>&opener_Addr1_id=<%=txtAddr1.ClientID %>', 'zipcode', '500', '360');" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblAddr" runat="server" Text="주소" meta:resourcekey="lblAddr" />
                        <span class="required"></span>
                    </th>
                    <td colspan="2">
                        <asp:TextBox ID="txtAddr1" runat="server" CssClass="w100per" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddr2" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblUploadResume" runat="server" Text="이력서첨부" meta:resourcekey="lblUploadResume" />
                    </th>
                    <td colspan="3">
                        <!-- 파일 첨부 인풋 -->
                        <div class="file-box">
                           <input class="upload-name" value="" disabled="disabled" />
                           <label for="<%=FileUpload1.ClientID %>"><asp:Label ID="lblSearchFile1" runat="server" Text="찾아보기" meta:resourcekey="lblSearchFile" /></label>
                           <asp:FileUpload ID="FileUpload1" runat="server" CssClass="upload-hidden" />
                        </div>
                        <!-- 파일 첨부 인풋 -->
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblUploadDoc" runat="server" Text="증빙서류첨부" meta:resourcekey="lblUploadDoc" />
                    </th>
                    <td colspan="3">
                        <!-- 파일 첨부 인풋 -->
                        <div class="file-box">
                           <input class="upload-name" value="" disabled="disabled" />
                           <label for="<%=FileUpload2.ClientID %>"><asp:Label ID="lblSearchFile2" runat="server" Text="찾아보기" meta:resourcekey="lblSearchFile" /></label>
                           <asp:FileUpload ID="FileUpload2" runat="server" CssClass="upload-hidden" />
                        </div>
                        <!-- 파일 첨부 인풋 -->
                    </td>
                </tr>
                </tbody>
            </table>
        
        </div>

        <!-- 버튼 -->
        <div class="button-group center">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button-main-rnd blue lg" OnClick="button_OnClick" OnClientClick="return fnValidateForm();" meta:resourcekey="btnSaveResource" />
        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>