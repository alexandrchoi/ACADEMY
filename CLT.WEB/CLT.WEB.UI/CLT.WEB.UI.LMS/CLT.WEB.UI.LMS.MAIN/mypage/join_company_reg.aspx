<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="join_company_reg.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.join_company_reg" 
    Culture="auto" UICulture="auto" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.FX.UTIL" Assembly="CLT.WEB.UI.FX.UTIL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    
    <script type="text/javascript" language="javascript">
        document.domain = "academy-t.gmarineservice.com";
	    function fnValidateForm()
	    {
	        // 필수 입력값 체크    
	        if (isEmpty(document.getElementById('<%=txtCEOName.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCEOName.Text }, new string[] { lblCEOName.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtEmpoly_Ins.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblEmpoly_Ins.Text }, new string[] { lblEmpoly_Ins.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtBusi.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblBusi.Text }, new string[] { lblBusi.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtCompanyType.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCompanyType.Text }, new string[] { lblCompanyType.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if (isEmpty(document.getElementById('<%=txtPhone.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPhone.Text }, new string[] { lblPhone.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtFax.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblFax.Text }, new string[] { lblFax.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtEmail.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblEmail.Text }, new string[] { lblEmail.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtZipCode.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblZipCode.Text }, new string[] { lblZipCode.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtAddr1.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAddr.Text }, new string[] { lblAddr.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtAddr2.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAddr.Text }, new string[] { lblAddr.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtHomePage.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblHomePage.Text }, new string[] { lblHomePage.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        
            if (isEmpty(document.getElementById('<%=txtID.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblID.Text }, new string[] { lblID.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtPass.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPass.Text }, new string[] { lblPass.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtPassCheck.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPass.Text }, new string[] { lblPass.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtuser_nm_kor.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lbluser_nm_kor.Text }, new string[] { lbluser_nm_kor.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtuser_nm_eng_first.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lbluser_nm_eng.Text }, new string[] { lbluser_nm_eng.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtuser_nm_eng_last.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lbluser_nm_eng.Text }, new string[] { lbluser_nm_eng.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtDept.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblDept.Text }, new string[] { lblDept.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtMobilePhone.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblMobilePhone.Text }, new string[] { lblMobilePhone.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        if (isEmpty(document.getElementById('<%=txtEmail_Admin.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblEmail_Admin.Text }, new string[] { lblEmail_Admin.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

            if (isSelect(document.getElementById('<%=ddlCompanyKind.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCompanyKind.Text }, new string[] { lblCompanyKind.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if (isSelect(document.getElementById('<%=ddlCompanyScale.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCompanyScale.Text }, new string[] { lblCompanyScale.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
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

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            } else {
                return true;
            }      
}
    </script>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <asp:HiddenField ID="backURL" runat="server" />
    <asp:Label ID="lblRegNo" runat="server" Text="법인 등록번호" meta:resourcekey="lblRegNo" Visible="False" />
    <asp:TextBox ID="txtRegNo" runat="server" Visible="False" meta:resourcekey="txtRegNoResource1" />
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
            <p class="headline black center">기업회원가입은 회사 정보와 담당자 정보를 모두 입력해 주세요.</p>
            <div class="sub-tab">
                <a href="#none" class="current" data-tab="tab-1" id="tab_1">회사정보 입력</a>
                <a href="#none" data-tab="tab-2" id="tab_2">담당자정보 입력</a>
            </div>

            <!-- 회사 정보 입력 -->
            <div id="tab-1" class="tab-content current">
                <div class="notice-text right">필수입력<span class="required">필수입력</span></div>
                <div class="table-grid gm-des-input">
                    <dl>
                        <dt><asp:Label ID="lblComapnyName" runat="server" Text="회사명" meta:resourcekey="lblComapnyName" /></dt>
                        <dd>
                            <asp:TextBox ID="txtCompanyName" runat="server" ReadOnly="True" CssClass="w100per" meta:resourcekey="txtCompanyNameResource1" />
                        </dd>
                        <dt><asp:Label ID="lblCEOName" runat="server" Text="대표자 성명" meta:resourcekey="lblCEOName" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtCEOName" runat="server" CssClass="w100per" meta:resourcekey="txtCEONameResource1" />
                        </dd>
                        <dt><asp:Label ID="lblTex" runat="server" Text="사업자 등록번호" meta:resourcekey="lblTex" /></dt>
                        <dd>
                            <asp:TextBox ID="txtTex" runat="server" ReadOnly="True" CssClass="w100per" meta:resourcekey="txtTexResource1" />
                        </dd>
                        <dt><asp:Label ID="lblEmpoly_Ins" runat="server" Text="고용보험관리번호" meta:resourcekey="lblEmpoly_Ins" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtEmpoly_Ins" runat="server" CssClass="w100per" meta:resourcekey="txtEmpoly_InsResource1" />
                        </dd>
                        <dt>
                            <asp:Label ID="lblBusi" runat="server" Text="업태" meta:resourcekey="lblBusi" />
                            <asp:Label ID="lblCompanyType" runat="server" Text="종목" meta:resourcekey="lblCompanyType" />
                            <span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtBusi" runat="server" CssClass="w30per" meta:resourcekey="txtBusiResource1" />
                            <asp:TextBox ID="txtCompanyType" runat="server" CssClass="w30per" meta:resourcekey="txtCompanyTypeResource1" />
                        </dd>
                        <dt><asp:Label ID="lblCompanyKind" runat="server" Text="회사구분" meta:resourcekey="lblCompanyKind" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:DropDownList ID="ddlCompanyKind" runat="server" meta:resourcekey="ddlCompanyKindResource1" />
                        </dd>
                        <dt><asp:Label ID="lblCompanyScale" runat="server" Text="회사규모" meta:resourcekey="lblCompanyScale" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:DropDownList ID="ddlCompanyScale" runat="server" meta:resourcekey="ddlCompanyScaleResource1" />
                        </dd>
                        <dt><asp:Label ID="lblPhone" runat="server" Text="전화번호" meta:resourcekey="lblPhone" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="w100per" placeholder="-를 포함해서 입력해주세요" meta:resourcekey="txtPhoneResource1" />
                        </dd>
                        <dt><asp:Label ID="lblFax" runat="server" Text="팩스번호" meta:resourcekey="lblFax" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtFax" runat="server" CssClass="w100per" placeholder="-를 포함해서 입력해주세요" meta:resourcekey="txtFaxResource1" />
                        </dd>
                        <dt><asp:Label ID="lblEmail" runat="server" Text="대표이메일" meta:resourcekey="lblEmailResource1" /><span class="required">필수입력</span></dt>
                        <dd class="email-cell">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="w100per" meta:resourcekey="txtEmailResource1" />
                        </dd>
                        <dt><asp:Label ID="lblAddr" runat="server" Text="주소" meta:resourcekey="lblAddr" /><span class="required">필수입력</span></dt>
                        <dd class="join-addr-cell">

                            <asp:TextBox ID="txtZipCode" runat="server" CssClass="w80" meta:resourcekey="txtZipCodeResource1" />
                            <input type="button" id="btnFindAddr" class="button-default blue zip" value="<%=GetLocalResourceObject("btnFindAddr.Text").ToString() %>"
                                                        onclick="openPopWindow('/common/zipcode.aspx?opener_textZipCode_id=<%=txtZipCode.ClientID %>&opener_Addr1_id=<%=txtAddr1.ClientID %>', 'zipcode', '500', '360');" /><br />
                            <asp:TextBox ID="txtAddr1" runat="server" CssClass="addr-box w100per" meta:resourcekey="txtAddr1Resource1" /><br />
                            <asp:TextBox ID="txtAddr2" runat="server" CssClass="addr-box w100per" placeholder="나머지 주소를 입력하세요" meta:resourcekey="txtAddr2Resource1" />

                        </dd>
                        <dt><asp:Label ID="lblHomePage" runat="server" Text="홈페이지" meta:resourcekey="lblHomePage" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtHomePage" runat="server" CssClass="w100per" meta:resourcekey="txtHomePageResource1" />
                        </dd>
                        <dt><asp:Label ID="lblEmpCountVessel" runat="server" Text="근로자수(해상직원)" meta:resourcekey="lblEmpCountVessel" /></dt>
                        <dd>
                            <asp:TextBox ID="txtEmpCountVessel" runat="server" CssClass="w100per" onkeypress="return isNumberKey(event)" meta:resourcekey="txtEmpCountVessel" />
                        </dd>
                        <dt><asp:Label ID="lblEmpCountShore" runat="server" Text="근로자수(육상직원)" meta:resourcekey="lblEmpCountShore" /></dt>
                        <dd>
                            <asp:TextBox ID="txtEmpCountShore" runat="server" CssClass="w100per" onkeypress="return isNumberKey(event)" meta:resourcekey="txtEmpCountShore" />
                        </dd>
                    </dl>
                </div>
            </div>
            <!--// 회사 정보 입력 -->

            <!-- 담당자 정보 입력 -->
            <div id="tab-2" class="tab-content">
                <div class="notice-text right">필수입력<span class="required">필수입력</span></div>

                <div class="table-grid gm-des-input">
                    <dl>
                        <dt><asp:Label ID="lblID" runat="server" Text="아이디" meta:resourcekey="lblIDResource1" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtID" runat="server" imeMode="disabled" MaxLength="10" onkeyup="javascript:this.value = this.value.toLowerCase();" onKeypress="if ((event.keyCode > 32 && event.keyCode < 48) || (event.keyCode > 57 && event.keyCode < 65) || (event.keyCode > 90 && event.keyCode < 97)) event.returnValue = false;" meta:resourcekey="txtIDResource1" />
                            <asp:Button ID="btnIDcheck" runat="server" Text="중복체크" OnClick="btnIDCheck_OnClick" CssClass="button-default blue" meta:resourcekey="btnIDcheck" />
                        </dd>
                        <dt><asp:Label ID="lblPass" runat="server" Text="비빌번호" meta:resourcekey="lblPass" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="w100per" meta:resourcekey="txtPassResource1" />
                        </dd>
                        <dt><asp:Label ID="lblPassCheck" runat="server" Text="비밀번호 확인" meta:resourcekey="lblPassCheck" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtPassCheck" runat="server" TextMode="Password" CssClass="w100per" meta:resourcekey="txtPassCheckResource1" />
                        </dd>
                        <dt><asp:Label ID="lbluser_nm_kor" runat="server" Text="성명(한글)" meta:resourcekey="lbluser_nm_kor" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtuser_nm_kor" runat="server" CssClass="w100per" placeholder="성명을 입력하세요" meta:resourcekey="txtuser_nm_korResource1" />
                        </dd>
                        <dt><asp:Label ID="lbluser_nm_eng" runat="server" Text="성명(영문)" meta:resourcekey="lbluser_nm_eng" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:Label ID="lbluser_nm_eng_first" runat="server" Text="성" Visible="False" meta:resourcekey="lbluser_nm_eng_first"  />
                            <asp:TextBox ID="txtuser_nm_eng_first" runat="server" CssClass="w30per" meta:resourcekey="txtuser_nm_eng_firstResource1" placeholder="성"/>
                            <asp:Label ID="lbluser_nm_eng_last" runat="server" Text="이름" Visible="False" meta:resourcekey="lbluser_nm_eng_last" />
                            <asp:TextBox ID="txtuser_nm_eng_last" runat="server" CssClass="w30per" meta:resourcekey="txtuser_nm_eng_lastResource1" placeholder="이름"/>
                        </dd>
                        <dt><asp:Label ID="lblbirth_dt" runat="server" Text="생년월일" meta:resourcekey="lblbirth_dt" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtBirth_dt" runat="server" CssClass="datepick" meta:resourcekey="txtBirth_dtResource1" />
                        </dd>
                        <dt><asp:Label ID="lblDept" runat="server" Text="부서" meta:resourcekey="lblDept" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtDept" runat="server" MaxLength="50" CssClass="w100per" placeholder="담당자가 속한 부서명을 입력하세요." meta:resourcekey="txtDeptResource1" />
                        </dd>
                        <dt><asp:Label ID="lblDuty" runat="server" Text="직급" meta:resourcekey="lblDuty" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:DropDownList ID="ddlComapnyduty" runat="server" meta:resourcekey="ddlComapnydutyResource1" />
                        </dd>
                        <dt><asp:Label ID="lblMobilePhone" runat="server" Text="휴대폰" meta:resourcekey="lblMobilePhone" /><span class="required">필수입력</span></dt>
                        <dd>
                            <asp:TextBox ID="txtMobilePhone" runat="server" CssClass="w100per" placeholder="-를 포함해서 입력해주세요" meta:resourcekey="txtMobilePhoneResource1" />
                        </dd>
                        <dt><asp:Label ID="lblPhone_Admin" runat="server" Text="전화번호" meta:resourcekey="lblPhone" /></dt>
                        <dd>
                            <asp:TextBox ID="txtPhone_Admin" runat="server" CssClass="w100per" placeholder="-를 포함해서 입력해주세요" meta:resourcekey="txtPhone_AdminResource1" />
                        </dd>
                        <dt><asp:Label ID="lblEmail_Admin" runat="server" Text="E-Mail" meta:resourcekey="lblEmail" /><span class="required">필수입력</span></dt>
                        <dd class="email-cell">
                            <asp:TextBox ID="txtEmail_Admin" runat="server" CssClass="w100per" meta:resourcekey="txtEmail_AdminResource1" />
                        </dd>
                        <dt><asp:Label ID="lblAcquisition" runat="server" Text="고용보험취득일" meta:resourcekey="lblAcquisition" /></dt>
                        <dd>
                            <asp:TextBox ID="txtAcquisition" runat="server" MaxLength="10" CssClass="datepick" meta:resourcekey="txtAcquisitionResource1" />
                        </dd>
                        <dt><asp:Label ID="lblUpload_photo" runat="server" Text="사진업로드" meta:resourcekey="lblUpload_photo" /></dt>
                        <dd>
                            <!-- 파일 첨부 인풋 -->
                            <div class="file-box">
                               <input class="upload-name" value="증명사진 3x4size" disabled="disabled" />
                               <label for="<%=fileUplaod.ClientID %>"><asp:Label ID="lblSearchFile" runat="server" Text="찾아보기" meta:resourcekey="lblSearchFile" /></label>
                               <asp:FileUpload ID="fileUplaod" runat="server" CssClass="upload-hidden" meta:resourcekey="fileUplaodResource1" />
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
            </div>
            <!--// 담당자 정보 입력 -->

            <div class="button-group center">
                <asp:Button ID="btnSave" runat="server" Text="회원가입" CssClass="button-main-rnd xlg blue" OnClick="btnSave_OnClick" OnClientClick="return fnValidateForm();" meta:resourcekey="btnSaveResource1" />
            </div>

        </section>


    </div>
    <!--// 서브 컨텐츠 끝 -->


</asp:Content>