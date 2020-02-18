<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="user_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.MANAGE.user_edit" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script language="javascript" type="text/javascript">
    document.domain = "academy-t.gmarineservice.com";
    function chkJumin(fldId1, fldId2, error_msg) 
    {
        var jumin = fldId1.value.replace(/\s/gi, "") + '-' + fldId2.value.replace(/\s/gi, "");
        
        // check JuminNumber-type and sex_digit 
        fmt = /^\d{6}-[1234]\d{6}$/;
        if (!fmt.test(jumin)) 
        {
            alert(error_msg);
            fldId1.focus() ;
            return true;
        }

        // check date-type
        birthYear = (jumin.charAt(7) <= "2") ? "19" : "20";
        birthYear += jumin.substr(0, 2);
        birthMonth = jumin.substr(2, 2) - 1;
        birthDate = jumin.substr(4, 2);
        birth = new Date(birthYear, birthMonth, birthDate);

        if ( birth.getYear() % 100 != jumin.substr(0, 2) ||
        birth.getMonth() != birthMonth ||
        birth.getDate() != birthDate) 
        {
            alert(error_msg);
            fldId1.focus();     
            return true;
        }

        // Check Sum code
        buf = new Array(13);
        for (i = 0; i < 6; i++) buf[i] = parseInt(jumin.charAt(i));
        for (i = 6; i < 13; i++) buf[i] = parseInt(jumin.charAt(i + 1));

        multipliers = [2,3,4,5,6,7,8,9,2,3,4,5];
        for (i = 0, sum = 0; i < 12; i++) sum += (buf[i] *= multipliers[i]);

        if ((11 - (sum % 11)) % 10 != buf[12]) 
        {
            alert(error_msg);
            fldId1.focus();        
            return true;
        }

        return false;
    }
    
    function movenext1()
    {
        var x = document.getElementById('<%=txtPersonal_no1.ClientID %>').value;
        
        if(x.length == 6)
          document.getElementById('<%=txtPersonal_no2.ClientID %>').focus();
    }		
    
	function fnValidateForm()
	{
	    /*
          사용자별 정보 수정 후 저장 시 필수값 체크를 확인한다.
        - 법인사 관리자: 주민번호,PW 수정 가능, 필수 아님.
        - 법인사 수강자: 주민번호,pw 수정 가능, 필수.
        - 관리자 및 그외: 주민번호,pw 수정 불가능, 필수 아님.
        - 우편번호, 주소, 연락처, 휴대폰 필수값 제외.
        */
        if(document.getElementById('<%=ddlUserGroup.ClientID %>').value == "000008")  //법인사 수강자
        {
            if("<%=Request.QueryString["EDITMODE"].ToString() %>" == "EDIT")
            {
                if (isEmpty(document.getElementById('<%=txtID.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblID.Text }, new string[] { lblID.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) 
                    return false; // ID를 입력해 주세요!

                //법인사 수강자가 자기 정보 수정시에는 비번 입력해야함.
                //관리자 또는 법인사 관린자가 정보 수정시에는 비번 입력안해도 됨.                
                if("<%=Session["USER_GROUP"].ToString() %>" == "000008")
                {
	                if (isEmpty(document.getElementById('<%=txtPass.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPass.Text }, new string[] { lblPass.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) 
	                    return false; // 비밀번호를 입력해 주세요!
        	          
                    if(isEmpty(document.getElementById('<%=txtPassCheck.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPassCheck.Text }, new string[] { lblPassCheck.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) 
                        return false; // 비밀번호확인를 입력해 주세요!  
                }
            }
            //비밀번호와 비밀번호 확인의 값이 틀릴 경우
            if(document.getElementById('<%=txtPass.ClientID %>').value != document.getElementById('<%=txtPassCheck.ClientID %>').value)            
            {
                alert('<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { "비밀번호" }, new string[] { "Password" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>');
                return false;
            }
	            
            if (isEmpty(document.getElementById('<%=txtPersonal_no1.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPersonal_no.Text }, new string[] { lblPersonal_no.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // 주민번호 입력해 주세요!
     	    if (isEmpty(document.getElementById('<%=txtPersonal_no2.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPersonal_no.Text }, new string[] { lblPersonal_no.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // 주민번호 입력해 주세요!
	        if (chkJumin(document.getElementById('<%=txtPersonal_no1.ClientID %>'), document.getElementById('<%=txtPersonal_no2.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { lblPersonal_no.Text }, new string[] { lblPersonal_no.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;          
          
        }
        else if(document.getElementById('<%=ddlUserGroup.ClientID %>').value == "000007")  //법인사 관리자
        {
            if (isEmpty(document.getElementById('<%=txtID.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblID.Text }, new string[] { lblID.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) 
                    return false; // ID를 입력해 주세요!
                    
            //비밀번호와 비밀번호 확인의 값이 틀릴 경우
            if(document.getElementById('<%=txtPass.ClientID %>').value != document.getElementById('<%=txtPassCheck.ClientID %>').value)            
            {
                alert('<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { "비밀번호" }, new string[] { "Password" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>');
                return false;
            }
            
            //주민번호에 값이 있다면 정합성 체크
            if(document.getElementById('<%=txtPersonal_no1.ClientID %>').value !="" || document.getElementById('<%=txtPersonal_no2.ClientID %>').value !="")
            {
                if (chkJumin(document.getElementById('<%=txtPersonal_no1.ClientID %>'), document.getElementById('<%=txtPersonal_no2.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { lblPersonal_no.Text }, new string[] { lblPersonal_no.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;          
            }
        }
            
	    /****************************** 필수 입력값 체크 Start*****************************/
	    // 이름 체크
	    if (isEmpty(document.getElementById('<%=txtuser_nm_kor.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lbluser_nm_kor.Text }, new string[] { lbluser_nm_kor.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // 이름을 입력해 주세요!
	    
	    // 회사명 체크
	    if (isEmpty(document.getElementById('<%=txtCompany.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCompany.Text }, new string[] { lblCompany.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // 회사명을 입력해 주세요!
	    
	    // 직급 체크 
	    if (isSelect(document.getElementById('<%=ddlComapnyduty.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblDuty.Text }, new string[] { lblDuty.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
  	    
  	    // 사용자그룹 체크 
	    if (isSelect(document.getElementById('<%=ddlUserGroup.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblUserGroup.Text }, new string[] { lblUserGroup.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    
        /*
	    //연락처 번호가 있다면 유효성 체크
	    if(document.getElementById('<%=txtPhone.ClientID %>').value != "") 
	    {
	        if (fnPhoneChk(document.getElementById('<%=txtPhone.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { lblPhone.Text }, new string[] { lblPhone.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) 
	            return false;
	    }
	    //휴대폰 번호가 있다면 유효성 체크
        if (document.getElementById('<%=txtMobilePhone.ClientID %>').value != "") 
	    {
	        if (fnPhoneChk(document.getElementById('<%=txtMobilePhone.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { txtMobilePhone.Text }, new string[] { txtMobilePhone.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) 
	            return false;
        }
        */
	    //자사근로자이면 고용보험취득일 필수 입력!!
	    if(document.getElementById('<%=ddlTrainee.ClientID %>').value == "000001"  && document.getElementById('<%=txtAcquisition.ClientID %>').value == "")
	    {
	        alert('<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { "고용보험취득일" }, new string[] { "Acquisition Date" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>');
	        return false;
	    }
	    /****************************** 필수 입력값 체크 End*****************************/
	    
	    // 길이체크
   	    if(isMaxLenth(document.getElementById('<%=txtID.ClientID %>'),10,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A007", new string[] { lblID.Text,"10" }, new string[] { lblID.Text,"10" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;	    
	    if(isMaxLenth(document.getElementById('<%=txtPass.ClientID %>'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblPass.Text,"50","30" }, new string[] { lblPass.Text,"50","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if(isMaxLenth(document.getElementById('<%=txtPassCheck.ClientID %>'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblPass.Text,"50","30" }, new string[] { lblPass.Text,"50","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtuser_nm_kor.ClientID %>'),75,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lbluser_nm_kor.Text,"75","50" }, new string[] { lbluser_nm_kor.Text,"75","50" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtPassCheck.ClientID %>'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblPass.Text,"50","30" }, new string[] { lblPass.Text,"50","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtuser_nm_eng_first.ClientID %>'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lbluser_nm_eng_first.Text,"50","30" }, new string[] { lbluser_nm_eng_first.Text,"50","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtuser_nm_eng_last.ClientID %>'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lbluser_nm_eng_last.Text,"50","30" }, new string[] { lbluser_nm_eng_last.Text,"50","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
   	    if(isMaxLenth(document.getElementById('<%=txtAddr1.ClientID %>'),100,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblAddr.Text,"100","66" }, new string[] { lblAddr.Text,"100","66" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if(isMaxLenth(document.getElementById('<%=txtAddr2.ClientID %>'),100,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblAddr.Text,"100","66" }, new string[] { lblAddr.Text,"100","66" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if(isMaxLenth(document.getElementById('<%=txtPhone.ClientID %>'),20,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblPhone.Text,"20","13" }, new string[] { lblPhone.Text,"20","13" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtEmail.ClientID %>'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblEmail.Text,"50","30" }, new string[] { lblEmail.Text,"50","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtMobilePhone.ClientID %>'),20,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblMobilePhone.Text,"20","13" }, new string[] { lblMobilePhone.Text,"20","13" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
        if(isDateChk(document.getElementById('<%=txtAcquisition.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblAcquisition.Text }, new string[] { lblAcquisition.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;
	    
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
    <div class="pop-container user-edit">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="개인회원" meta:resourcekey="lblMenuTitle" /></h3>
    
        
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
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtID" runat="server" imeMode="disabled" MaxLength="10" onkeyup="javascript:this.value = this.value.toLowerCase();" onKeypress="if ((event.keyCode > 32 && event.keyCode < 48) || (event.keyCode > 57 && event.keyCode < 65) || (event.keyCode > 90 && event.keyCode < 97)) event.returnValue = false;" />
                        <asp:Button ID="btnIDcheck" runat="server" Text="중복체크" OnClick="btnIDCheck_OnClick" CssClass="button-default blue" meta:resourcekey="btnIDcheck" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblPass" runat="server" Text="비빌번호" meta:resourcekey="lblPass" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblPassCheck" runat="server" Text="비밀번호 확인" meta:resourcekey="lblPassCheck" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtPassCheck" runat="server" TextMode="Password" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lbluser_nm_kor" runat="server" Text="이름" meta:resourcekey="lbluser_nm_kor" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtuser_nm_kor" runat="server" MaxLength="50" />
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
                        <asp:Label ID="lblPersonal_no" runat="server" Text="주민번호" meta:resourcekey="lblPersonal_no" />
                        <!--span class="required"></span-->
                    </th>
                    <td>
                        <asp:TextBox ID="txtPersonal_no1" runat="server" onkeyup ="movenext1();" MaxLength="6" CssClass="w30per" />
                        <span class="gm-text2">-</span>
                        <asp:TextBox ID="txtPersonal_no2" runat="server" TextMode="Password" MaxLength="7" CssClass="w30per" />
                    </td>
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
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtCompany" runat="server" />
                        <input type="button" id="btnComapny" class="button-default blue"  value="<%=GetLocalResourceObject("btnFindCompany.Text").ToString() %>"
                            onclick="openPopWindow('/manage/company_select.aspx?opener_textCompany_NM=<%=txtCompany.ClientID %>&opener_Company_id=<%= hidCompany_id.ClientID %>&USERGROUP=<%=Session["USER_GROUP"]%>&COMPANY_ID=<%=Session["COMPANY_ID"] %>', 'company_select', '550', '600');" />
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
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" />
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
                        <asp:Label ID="lblUserGroup" runat="server" Text="사용자 그룹" meta:resourcekey="lblUserGroup" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlUserGroup" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUserGroup_SelectedIndexChanged" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblTrainee" runat="server" Text="훈련생구분" meta:resourcekey="lblTrainee" />
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlTrainee" runat="server" />
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
            <!--<asp:Button ID="btnRewrite" runat="server" Text="Rewrite" CssClass="button-default blue" OnClick="btnRewrite_OnClick" meta:resourcekey="btnRewriteResource" />-->
        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>