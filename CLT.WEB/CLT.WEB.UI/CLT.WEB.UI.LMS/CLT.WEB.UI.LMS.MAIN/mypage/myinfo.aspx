<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="myinfo.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.myinfo" 
    Culture="auto" UICulture="auto" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.FX.UTIL" Assembly="CLT.WEB.UI.FX.UTIL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script language="javascript" type="text/javascript">
        document.domain = "academy-t.gmarineservice.com";

	    function fnValidateForm()
	    {
	        // 필수 입력값 체크    
	        if (isEmpty(document.getElementById('<%=txtPass.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPass.Text }, new string[] { lblPass.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // 비밀번호를 입력해 주세요!
	        if (isEmpty(document.getElementById('<%=txtuser_nm_eng_first.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lbluser_nm_eng.Text }, new string[] { lbluser_nm_eng.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtuser_nm_eng_last.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lbluser_nm_eng.Text }, new string[] { lbluser_nm_eng.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            //if (isEmpty(document.getElementById('<%=txtZipCode.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblZipCode.Text }, new string[] { lblZipCode.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        //if (isEmpty(document.getElementById('<%=txtAddr1.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAddr.Text }, new string[] { lblAddr.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        //if (isEmpty(document.getElementById('<%=txtAddr2.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAddr.Text }, new string[] { lblAddr.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtEmail.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblEmail.Text }, new string[] { lblEmail.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtMobilePhone.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblMobilePhone.Text }, new string[] { lblMobilePhone.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        
  	        // 직급선택 체크 
	        if (isSelect(document.getElementById('<%=ddlComapnyduty.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblDuty.Text }, new string[] { lblDuty.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    	    
	        // 비밀번호 입력체크
	        if (isPassChk(document.getElementById('<%=txtNewPass.ClientID %>'), document.getElementById('<%=txtNewPassCheck.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { lblNewPass.Text }, new string[] { lblNewPass.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    
	        // 전화번호 체크
	        if (fnPhoneChk(document.getElementById('<%=txtPhone.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { lblPhone.Text }, new string[] { lblPhone.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    
		    // 메일 체크
	        if (CheckMail(document.getElementById('<%=txtEmail.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { lblEmail.Text }, new string[] { lblEmail.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

	        // 길이 체크
	        if(isMaxLenth(document.getElementById('<%=txtNewPass.ClientID %>'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblNewPass.Text,"50","30" }, new string[] { lblNewPass.Text,"50","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if(isMaxLenth(document.getElementById('<%=txtNewPassCheck.ClientID %>'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblNewPass.Text,"50","30" }, new string[] { lblNewPass.Text,"50","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if(isMaxLenth(document.getElementById('<%=txtAddr1.ClientID %>'),100,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblAddr.Text,"100","66" }, new string[] { lblAddr.Text,"100","66" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if(isMaxLenth(document.getElementById('<%=txtAddr2.ClientID %>'),100,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblAddr.Text,"100","66" }, new string[] { lblAddr.Text,"100","66" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if(isMaxLenth(document.getElementById('<%=txtPhone.ClientID %>'),20,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { txtPhone.Text,"20","13" }, new string[] { txtPhone.Text,"20","13" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if(isMaxLenth(document.getElementById('<%=txtMobilePhone.ClientID %>'),20,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblMobilePhone.Text,"20","13" }, new string[] { lblMobilePhone.Text,"20","13" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if(isMaxLenth(document.getElementById('<%=txtEmail.ClientID %>'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblEmail.Text,"50","30" }, new string[] { lblEmail.Text,"50","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        
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
    
    <asp:HiddenField ID="hidCompany_id" runat="server" /> 
    

    <!-- 팝업 컨텐츠 시작 -->
    <div class="section-fix user-edit">
        <h2 class="page-title"><asp:Label ID="Label1" runat="server" Text="개인정보수정" meta:resourcekey="lblMenuTitle"></asp:Label>
            <!-- 모바일 뒤로 가기 -->
            <span class="goback">
			        <button onclick="goBack();return false;"><span class="off-screen">Go back</span></button>
			    </span>
        </h2>
        
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
                        <asp:Label ID="lblID" runat="server" Text="ID" meta:resourcekey="lblID" />
                    </th>
                    <td colspan="3">
                        <asp:Label ID="txtID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblPass" runat="server" Text="비밀번호" meta:resourcekey="lblPass" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />
                    </td>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblNewPass" runat="server" Text="새 비밀번호" meta:resourcekey="lblNewPass" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtNewPass" runat="server" TextMode="Password" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblNewPassCheck" runat="server" Text="비밀번호 확인" meta:resourcekey="lblNewPassCheck" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtNewPassCheck" runat="server" TextMode="Password" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lbluser_nm_kor" runat="server" Text="이름" meta:resourcekey="lbluser_nm_kor" />
                    </th>
                    <td>
                        <asp:Label ID="txtuser_nm_kor" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lbluser_nm_eng" runat="server" Text="영문이름" meta:resourcekey="lbluser_nm_eng" />
                    </th>
                    <td>
                        <asp:Label ID="lbluser_nm_eng_first" runat="server" Text="First" Visible="false" meta:resourcekey="lbluser_nm_eng_first" />
                        <asp:TextBox ID="txtuser_nm_eng_first" runat="server" CssClass="w30per" placeholder="First" />
                        <asp:Label ID="lbluser_nm_eng_last" runat="server" Text="Last" Visible="false" meta:resourcekey="lbluser_nm_eng_last" />
                        <asp:TextBox ID="txtuser_nm_eng_last" runat="server" CssClass="w30per" placeholder="Last" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblbirth_dt" runat="server" Text="생년월일" meta:resourcekey="lblbirth_dt" />
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtBirth_dt" runat="server" CssClass="datepick" />
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
                        <asp:Label ID="lblCompany" runat="server" Text="회사명" meta:resourcekey="lblCompany" />
                    </th>
                    <td>
                        <asp:Label ID="txtCompany" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblAcquisition" runat="server" Text="고용보험취득일" meta:resourcekey="lblAcquisition" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtAcquisition" runat="server" MaxLength="10" CssClass="datepick" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblDept" runat="server" Text="부서" meta:resourcekey="lblDept" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtDept" runat="server" MaxLength="50" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblDuty" runat="server" Text="직급" meta:resourcekey="lblDuty" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlComapnyduty" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblPhone" runat="server" Text="연락처" meta:resourcekey="lblPhone" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblMobilePhone" runat="server" Text="휴대폰" meta:resourcekey="lblMobilePhone" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMobilePhone" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblEmail" runat="server" Text="E-Mail" meta:resourcekey="lblEmail" />
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtEmail" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblUpload_photo" runat="server" Text="사진업로드" meta:resourcekey="lblUpload_photo" />
                    </th>
                    <td colspan="3">
                        
                        <asp:Image ID="img_pic_file" runat="server" ImageUrl='/asset/images/blank.gif' /> <!--# DataBinder.Eval(Container.DataItem, "PIC_FILE")-->
                        <!-- 파일 첨부 인풋 -->
                        <div class="file-box">
                           <input class="upload-name" value="증명사진 3x4size" disabled="disabled" />
                           <label for="<%=fileUplaod.ClientID %>"><asp:Label ID="lblSearchFile" runat="server" Text="찾아보기" meta:resourcekey="lblSearchFile" /></label>
                           <asp:FileUpload ID="fileUplaod" runat="server" CssClass="upload-hidden" />
                        </div>
                        <!-- 파일 첨부 인풋 -->

                    </td>
                </tr>

                <tr>
                    <th scope="row">
                        <asp:Label ID="lblSMS_yn" runat="server" Text="SMS 서비스 수신여부" meta:resourcekey="lblSMS_yn" />
                    </th>
                    <td>
                        <label class="radio-box">
                            <asp:RadioButton ID="rbSMS_y" GroupName="sms" runat="server" Checked="true" Text="예" meta:resourcekey="rbSMS_y" AutoPostBack="false" />
                            <span class="radiomark"></span>
                        </label>
                        <label class="radio-box">
                            <asp:RadioButton ID="rbSMS_n" GroupName="sms" runat="server" Checked="false" Text="아니오" meta:resourcekey="rbSMS_n" AutoPostBack="false" />
                            <span class="radiomark"></span>
                        </label>
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblMail_yn" runat="server" Text="메일링서비스 수신여부" meta:resourcekey="lblMail_yn" />
                    </th>
                    <td>
                        <label class="radio-box">
                            <asp:RadioButton ID="rbMail_y" GroupName="mail" runat="server" Checked="true" Text="예" meta:resourcekey="rbMail_y" AutoPostBack="false" />
                            <span class="radiomark"></span>
                        </label>
                        <label class="radio-box">
                            <asp:RadioButton ID="rbMail_n" GroupName="mail" runat="server" Checked="false" Text="아니오" meta:resourcekey="rbMail_n" AutoPostBack="false" />
                            <span class="radiomark"></span>
                        </label>
                    </td>
                </tr>
                </tbody>
            </table>
        
        </div>

        <!-- 버튼 -->
        <div class="button-group center">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button-main-rnd blue lg" OnClick="btnSave_OnClick" OnClientClick="return fnValidateForm();" meta:resourcekey="btnSaveResource" />
        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>