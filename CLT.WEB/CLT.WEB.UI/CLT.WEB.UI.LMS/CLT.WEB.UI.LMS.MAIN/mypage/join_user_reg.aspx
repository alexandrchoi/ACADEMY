<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="join_user_reg.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.join_user_reg" 
    Culture="auto" UICulture="auto" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.FX.UTIL" Assembly="CLT.WEB.UI.FX.UTIL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    
    <script type="text/javascript" language="javascript">
        document.domain = "academy.gmarineservice.com";
	    function fnValidateForm()
	    {
	        // 필수 입력값 체크    
            if (isEmpty(document.getElementById('<%=txtID.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblID.Text }, new string[] { lblID.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtPass.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPass.Text }, new string[] { lblPass.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtPassCheck.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPass.Text }, new string[] { lblPass.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtuser_nm_kor.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lbluser_nm_kor.Text }, new string[] { lbluser_nm_kor.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtuser_nm_eng_first.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lbluser_nm_eng.Text }, new string[] { lbluser_nm_eng.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtuser_nm_eng_last.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lbluser_nm_eng.Text }, new string[] { lbluser_nm_eng.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtBirth_dt.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblbirth_dt.Text }, new string[] { lblbirth_dt.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtZipCode.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblZipCode.Text }, new string[] { lblZipCode.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtAddr1.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAddr.Text }, new string[] { lblAddr.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtAddr2.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAddr.Text }, new string[] { lblAddr.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtDept.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblDept.Text }, new string[] { lblDept.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtMobilePhone.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblMobilePhone.Text }, new string[] { lblMobilePhone.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtEmail.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblEmail.Text }, new string[] { lblEmail.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        
            if (isSelect(document.getElementById('<%=ddlComapnyduty.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblDuty.Text }, new string[] { lblDuty.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        // 비밀번호 입력체크
	        if (isPassChk(document.getElementById('<%=txtPass.ClientID %>'), document.getElementById('<%=txtPassCheck.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { lblPass.Text }, new string[] { lblPass.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
		    // 메일 체크
	        if (CheckMail(document.getElementById('<%=txtEmail.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { lblEmail.Text }, new string[] { lblEmail.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            
	        return true;
	    }

        function jusoCallBack(roadFullAddr,roadAddrPart1,addrDetail,roadAddrPart2,engAddr,jibunAddr,zipNo){
	        document.getElementById('<%=txtAddr1.ClientID %>').value = roadAddrPart1 + " " + roadAddrPart2;
	        document.getElementById('<%=txtAddr2.ClientID %>').value = addrDetail;
            document.getElementById('<%=txtZipCode.ClientID %>').value = zipNo;
        }
    </script>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <asp:HiddenField ID="backURL" runat="server" />
    <asp:Label ID="lblZipCode" runat="server" Text="우편번호" meta:resourcekey="lblZipCode" Visible="False" />

    <!-- 서브 컨텐츠 시작 -->
    <div class="section-fix">
        <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="회원가입" meta:resourcekey="lblMenuTitle"></asp:Label>
            <!-- 모바일 뒤로 가기 -->
            <span class="goback">
			        <button onclick="goBack();return false;"><span class="off-screen">Go back</span></button>
			    </span>
        </h2>
        
        <section class="join-form user-register">
            <div class="notice-text right">필수입력<span class="required">필수입력</span></div>

            <div class="table-grid gm-des-input">
                <dl>
                    <dt><asp:Label ID="lblID" runat="server" Text="아이디" meta:resourcekey="lblIDResource" /><span class="required">필수입력</span></dt>
                    <dd>
                        <asp:TextBox ID="txtID" runat="server" imeMode="disabled" MaxLength="10" onkeyup="javascript:this.value = this.value.toLowerCase();" onKeypress="if ((event.keyCode > 32 && event.keyCode < 48) || (event.keyCode > 57 && event.keyCode < 65) || (event.keyCode > 90 && event.keyCode < 97)) event.returnValue = false;" meta:resourcekey="txtIDResource" />
                        <asp:Button ID="btnIDcheck" runat="server" Text="중복체크" OnClick="btnIDCheck_OnClick" CssClass="button-default blue" meta:resourcekey="btnIDcheck" />
                    </dd>
                    <dt><asp:Label ID="lblPass" runat="server" Text="비빌번호" meta:resourcekey="lblPass" /><span class="required">필수입력</span></dt>
                    <dd>
                        <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="w100per" meta:resourcekey="txtPassResource" />
                    </dd>
                    <dt><asp:Label ID="lblPassCheck" runat="server" Text="비밀번호 확인" meta:resourcekey="lblPassCheck" /><span class="required">필수입력</span></dt>
                    <dd>
                        <asp:TextBox ID="txtPassCheck" runat="server" TextMode="Password" CssClass="w100per" meta:resourcekey="txtPassCheckResource" />
                    </dd>
                    <dt><asp:Label ID="lbluser_nm_kor" runat="server" Text="성명(한글)" meta:resourcekey="lbluser_nm_kor" /><span class="required">필수입력</span></dt>
                    <dd>
                        <asp:TextBox ID="txtuser_nm_kor" runat="server" CssClass="w100per" placeholder="성명을 입력하세요" meta:resourcekey="txtuser_nm_korResource" />
                    </dd>
                    <dt><asp:Label ID="lbluser_nm_eng" runat="server" Text="성명(영문)" meta:resourcekey="lbluser_nm_eng" /><span class="required">필수입력</span></dt>
                    <dd>
                        <asp:Label ID="lbluser_nm_eng_first" runat="server" Text="성" Visible="False" meta:resourcekey="lbluser_nm_eng_first"  />
                        <asp:TextBox ID="txtuser_nm_eng_first" runat="server" CssClass="w30per" meta:resourcekey="txtuser_nm_eng_firstResource" placeholder="성"/>
                        <asp:Label ID="lbluser_nm_eng_last" runat="server" Text="이름" Visible="False" meta:resourcekey="lbluser_nm_eng_last" />
                        <asp:TextBox ID="txtuser_nm_eng_last" runat="server" CssClass="w30per" meta:resourcekey="txtuser_nm_eng_lastResource" placeholder="이름"/>
                    </dd>
                    <dt><asp:Label ID="lblbirth_dt" runat="server" Text="생년월일" meta:resourcekey="lblbirth_dt" /><span class="required">필수입력</span></dt>
                    <dd>
                        <asp:TextBox ID="txtBirth_dt" runat="server" CssClass="datepick" meta:resourcekey="txtBirth_dtResource" />
                    </dd>
                    
                    <dt><asp:Label ID="lblAddr" runat="server" Text="주소" meta:resourcekey="lblAddr" /><span class="required">필수입력</span></dt>
                    <dd class="join-addr-cell">

                        <asp:TextBox ID="txtZipCode" runat="server" CssClass="w80" meta:resourcekey="txtZipCodeResource" />
                        <input type="button" id="btnFindAddr" class="button-default blue zip" value="<%=GetLocalResourceObject("btnFindAddr.Text").ToString() %>"
                                                    onclick="openPopWindow('/common/zipcode.aspx?opener_textZipCode_id=<%=txtZipCode.ClientID %>&opener_Addr1_id=<%=txtAddr1.ClientID %>', 'zipcode', '500', '360');" /><br />
                        <asp:TextBox ID="txtAddr1" runat="server" CssClass="addr-box w100per" meta:resourcekey="txtAddr1Resource" /><br />
                        <asp:TextBox ID="txtAddr2" runat="server" CssClass="addr-box w100per" placeholder="나머지 주소를 입력하세요" meta:resourcekey="txtAddr2Resource" />

                    </dd>
                    
                    <dt><asp:Label ID="lblDept" runat="server" Text="부서" meta:resourcekey="lblDept" /><span class="required">필수입력</span></dt>
                    <dd>
                        <asp:TextBox ID="txtDept" runat="server" MaxLength="50" CssClass="w100per" placeholder="담당자가 속한 부서명을 입력하세요." meta:resourcekey="txtDeptResource" />
                    </dd>
                    <dt><asp:Label ID="lblDuty" runat="server" Text="직급" meta:resourcekey="lblDuty" /><span class="required">필수입력</span></dt>
                    <dd>
                        <asp:DropDownList ID="ddlComapnyduty" runat="server" meta:resourcekey="ddlComapnydutyResource" />
                    </dd>
                    <dt><asp:Label ID="lblMobilePhone" runat="server" Text="휴대폰" meta:resourcekey="lblMobilePhone" /><span class="required">필수입력</span></dt>
                    <dd>
                        <asp:TextBox ID="txtMobilePhone" runat="server" CssClass="w100per" placeholder="-를 포함해서 입력해주세요" meta:resourcekey="txtMobilePhoneResource" />
                    </dd>
                    <dt><asp:Label ID="lblPhone" runat="server" Text="전화번호" meta:resourcekey="lblPhone" /></dt>
                    <dd>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="w100per" placeholder="-를 포함해서 입력해주세요" meta:resourcekey="txtPhoneResource" />
                    </dd>
                    <dt><asp:Label ID="lblEmail" runat="server" Text="E-Mail" meta:resourcekey="lblEmail" /><span class="required">필수입력</span></dt>
                    <dd class="email-cell">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="w100per" meta:resourcekey="txtEmailResource" />
                    </dd>
                    <dt><asp:Label ID="lblAcquisition" runat="server" Text="고용보험취득일" meta:resourcekey="lblAcquisition" /></dt>
                    <dd>
                        <asp:TextBox ID="txtAcquisition" runat="server" MaxLength="10" CssClass="datepick" meta:resourcekey="txtAcquisitionResource" />
                    </dd>
                    <dt><asp:Label ID="lblUpload_photo" runat="server" Text="사진업로드" meta:resourcekey="lblUpload_photo" /></dt>
                    <dd>
                        <!-- 파일 첨부 인풋 -->
                        <div class="file-box">
                            <input class="upload-name" value="증명사진 3x4size" disabled="disabled" />
                            <label for="<%=fileUplaod.ClientID %>"><asp:Label ID="lblSearchFile" runat="server" Text="찾아보기" meta:resourcekey="lblSearchFile" /></label>
                            <asp:FileUpload ID="fileUplaod" runat="server" CssClass="upload-hidden" meta:resourcekey="fileUplaodResource" />
                        </div>
                        <!-- 파일 첨부 인풋 -->
                    </dd>
                    <dt><asp:Label ID="lblSMS_yn" runat="server" Text="SMS 서비스 수신여부" meta:resourcekey="lblSMS_yn" /><span class="required">필수입력</span></dt>
                    <dd>
                        <label class="radio-box">
                            <asp:RadioButton ID="rbSMS_y" GroupName="sms" runat="server" Checked="True" Text="예" meta:resourcekey="rbSMS_y" />
                            <span class="radiomark"></span>
                        </label>
                        <label class="radio-box">
                            <asp:RadioButton ID="rbSMS_n" GroupName="sms" runat="server" Text="아니오" meta:resourcekey="rbSMS_n" />
                            <span class="radiomark"></span>
                        </label>
                    </dd>
                    <dt><asp:Label ID="lblMail_yn" runat="server" Text="메일링서비스 수신여부" meta:resourcekey="lblMail_yn" /><span class="required">필수입력</span></dt>
                    <dd>
                        <label class="radio-box">
                            <asp:RadioButton ID="rbMail_y" GroupName="mail" runat="server" Checked="True" Text="예" meta:resourcekey="rbMail_y" />
                            <span class="radiomark"></span>
                        </label>
                        <label class="radio-box">
                            <asp:RadioButton ID="rbMail_n" GroupName="mail" runat="server" Text="아니오" meta:resourcekey="rbMail_n" />
                            <span class="radiomark"></span>
                        </label>
                    </dd>
                </dl>
            </div>

            <div class="button-group center">
                <asp:Button ID="btnSave" runat="server" Text="회원가입" CssClass="button-main-rnd xlg blue" OnClick="btnSave_OnClick" OnClientClick="return fnValidateForm();" meta:resourcekey="btnSaveResource" />
            </div>

        </section>


    </div>
    <!--// 서브 컨텐츠 끝 -->


</asp:Content>