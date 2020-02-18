<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="company_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.MANAGE.company_edit" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script language="javascript" type="text/javascript">
    document.domain = "academy-t.gmarineservice.com";
    function checkBizID(bizID, error_msg)  //사업자등록번호 체크 
    { 
        // bizID는 숫자만 10자리로 해서 문자열로 넘긴다. 
        var checkID = new Array(1, 3, 7, 1, 3, 7, 1, 3, 5, 1); 
        var tmpBizID, i, chkSum=0, c2, remander; 
         bizID = bizID.replace(/-/gi,''); 

         for (i=0; i<=7; i++) chkSum += checkID[i] * bizID.charAt(i); 
         c2 = "0" + (checkID[8] * bizID.charAt(8)); 
         c2 = c2.substring(c2.length - 2, c2.length); 
         chkSum += Math.floor(c2.charAt(0)) + Math.floor(c2.charAt(1)); 
         remander = (10 - (chkSum % 10)) % 10 ; 

        if (Math.floor(bizID.charAt(9)) == remander)
        {
            
            return false ; // OK! 
        }
        else
        {
            alert(error_msg);
            return true; 
        }
    }       
    
    function fnValidateForm()
	{
	
	    // 필수 입력값 체크    
	    
	    // 회사코드
	    if (isEmpty(document.getElementById('<%=txtCompanyCode.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCompanyCode.Text }, new string[] { lblCompanyCode.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    // 회사명

	    if (isEmpty(document.getElementById('<%=txtCompanyName.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCompanyName.Text }, new string[] { lblCompanyName.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        // 사업자 등록번호	    
	    if (isEmpty(document.getElementById('<%=txtTex1.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblTex.Text }, new string[] { lblTex.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if (isEmpty(document.getElementById('<%=txtTex2.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblTex.Text }, new string[] { lblTex.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if (isEmpty(document.getElementById('<%=txtTex3.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblTex.Text }, new string[] { lblTex.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    // 대표자 성명
	    if (isEmpty(document.getElementById('<%=txtCEOName.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCEOName.Text }, new string[] { lblCEOName.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    // 우편번호
	    if (isEmpty(document.getElementById('<%=txtZipCode.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblZipCode.Text }, new string[] { lblZipCode.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    // 주소
	    if (isEmpty(document.getElementById('<%=txtAddr1.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAddr.Text }, new string[] { lblAddr.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if (isEmpty(document.getElementById('<%=txtAddr2.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAddr.Text }, new string[] { lblAddr.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    // 전화번호
	    if (isEmpty(document.getElementById('<%=txtPhone.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPhone.Text }, new string[] { lblPhone.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    
   	    // 회사규모 체크 
	    if (isSelect(document.getElementById('<%=ddlCompanyScale.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblCompanyScale.Text }, new string[] { lblCompanyScale.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    
	    // 회사구분 체크
	    if (isSelect(document.getElementById('<%=ddlCompanyKind.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblCompanyKind.Text }, new string[] { lblCompanyKind.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    
	    // 전화번호 체크
	    if (fnPhoneChk(document.getElementById('<%=txtPhone.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { lblPhone.Text }, new string[] { lblPhone.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    
	    
	    //2014.03.20 seojw 회사코드는 반드시 3자리로
        if(document.getElementById('<%=txtCompanyCode.ClientID %>').value.length != 3)
        {
            alert('<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A008", new string[] { lblCompanyCode.Text,"3" }, new string[] { lblCompanyCode.Text,"3" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>');
            return false;
        }

        // 길이 체크	    
        if(isMaxLenth(document.getElementById('<%=txtCompanyCode.ClientID %>'),3,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A007", new string[] { lblCompanyCode.Text,"3" }, new string[] { lblCompanyCode.Text,"3" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtCompanyName.ClientID %>'),100,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblCompanyName.Text,"100","66" }, new string[] { lblCompanyName.Text,"100","66" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if(isMaxLenth(document.getElementById('<%=txtRegNo.ClientID %>'),13,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblRegNo.Text,"13","8" }, new string[] { lblRegNo.Text,"13","8" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if(isMaxLenth(document.getElementById('<%=txtEmpoly_Ins.ClientID %>'),20,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblEmpoly_Ins.Text,"20","13" }, new string[] { lblEmpoly_Ins.Text,"20","13" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if(isMaxLenth(document.getElementById('<%=txtCEOName.ClientID %>'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblCEOName.Text,"50","33" }, new string[] { lblCEOName.Text,"50","33" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if(isMaxLenth(document.getElementById('<%=txtHomePage.ClientID %>'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblHomePage.Text,"50","33" }, new string[] { lblHomePage.Text,"50","33" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if(isMaxLenth(document.getElementById('<%=txtBusi.ClientID %>'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblBusi.Text,"50","33" }, new string[] { lblBusi.Text,"50","33" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtCompanyType.ClientID %>'),60,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblCompanyType.Text,"20","30" }, new string[] { lblCompanyType.Text,"20","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;	    
	    if(isMaxLenth(document.getElementById('<%=txtAddr1.ClientID %>'),100,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblAddr.Text,"100","66" }, new string[] { lblAddr.Text,"100","66" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if(isMaxLenth(document.getElementById('<%=txtAddr2.ClientID %>'),100,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblAddr.Text,"100","66" }, new string[] { lblAddr.Text,"100","66" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if(isMaxLenth(document.getElementById('<%=txtPhone.ClientID %>'),20,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { txtPhone.Text,"20","13" }, new string[] { txtPhone.Text,"20","13" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    if(isMaxLenth(document.getElementById('<%=txtFax.ClientID %>'),20,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblFax.Text,"20","13" }, new string[] { lblFax.Text,"20","13" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtCompanyEngName.ClientID %>'),100,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblCompanyEngName.Text,"100","66" }, new string[] { lblCompanyEngName.Text,"100","66" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
        var taxno = document.getElementById('<%=txtTex1.ClientID %>').value + document.getElementById('<%=txtTex2.ClientID %>').value + document.getElementById('<%=txtTex3.ClientID %>').value;
  	    
  	    
  	    // 사업자 등록번호 체크
	    if (checkBizID(taxno, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { lblTex.Text }, new string[] { lblTex.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

        
	    return true;
	}    
	
    function movenext1()
    {
        var x = document.getElementById('<%=txtTex1.ClientID %>').value;
        
        if(x.length == 3)
          document.getElementById('<%=txtTex2.ClientID %>').focus();
    }		

    function movenext2()
    {
        var x = document.getElementById('<%=txtTex2.ClientID %>').value;
        
        if(x.length == 2)
          document.getElementById('<%=txtTex3.ClientID %>').focus();
    }
    
    function jusoCallBack(roadFullAddr,roadAddrPart1,addrDetail,roadAddrPart2,engAddr,jibunAddr,zipNo){
	    document.getElementById('<%=txtAddr1.ClientID %>').value = roadAddrPart1 + " " + roadAddrPart2;
	    document.getElementById('<%=txtAddr2.ClientID %>').value = addrDetail;
        document.getElementById('<%=txtZipCode.ClientID %>').value = zipNo;
    }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <asp:Label ID="lblRegNo" runat="server" Text="법인 등록번호" meta:resourcekey="lblRegNo" Visible="false" />
    <asp:TextBox ID="txtRegNo" runat="server" Visible="false" />

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container company-register user-edit">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="회원사" meta:resourcekey="lblMenuTitle" /></h3>
    
        
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
                        <asp:Label ID="lblCompanyCode" runat="server" Text="회사코드" meta:resourcekey="lblCompanyCode" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtCompanyCode" runat="server" imeMode="disabled" MaxLength="3" onkeyup="javascript:this.value = this.value.toUpperCase();" CssClass="w100per" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblCompanyName" runat="server" Text="회사명" meta:resourcekey="lblCompanyName" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblCompanyEngName" runat="server" Text="회사명(영문)" meta:resourcekey="lblCompanyEngName" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtCompanyEngName" runat="server" CssClass="w100per" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblCEOName" runat="server" Text="대표자 성명" meta:resourcekey="lblCEOName" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtCEOName" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblTex" runat="server" Text="사업자 등록번호" meta:resourcekey="lblTex" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtTex1" runat="server" CssClass="w80" MaxLength="3" onkeyup ="movenext1();" />
                        <span class="gm-text2">-</span>
                        <asp:TextBox ID="txtTex2" runat="server" CssClass="w80" MaxLength="2" onkeyup ="movenext2();" />
                        <span class="gm-text2">-</span>
                        <asp:TextBox ID="txtTex3" runat="server" CssClass="w80" MaxLength="5" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblEmpoly_Ins" runat="server" Text="고용 보험번호" meta:resourcekey="lblEmpoly_Ins" />
                        <!--span class="required"></span-->
                    </th>
                    <td>
                        <asp:TextBox ID="txtEmpoly_Ins" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblBusi" runat="server" Text="업태" meta:resourcekey="lblBusi" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtBusi" runat="server" CssClass="w100per" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblCompanyType" runat="server" Text="종목" meta:resourcekey="lblCompanyType" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtCompanyType" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                    
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblCompanyKind" runat="server" Text="회사구분" meta:resourcekey="lblCompanyKind" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlCompanyKind" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblCompanyScale" runat="server" Text="회사규모" meta:resourcekey="lblCompanyScale" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlCompanyScale" runat="server" />
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
                        <asp:Label ID="lblFax" runat="server" Text="팩스번호(- 포함)" meta:resourcekey="lblFax" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtFax" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                    
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblHomePage" runat="server" Text="홈페이지" meta:resourcekey="lblHomePage" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtHomePage" runat="server" CssClass="w100per" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblEmail" runat="server" Text="E-Mail" meta:resourcekey="lblEmail" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="w100per" />
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
                </tbody>
            </table>
        
        </div>

        <!-- 버튼 -->
        <div class="button-group center">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button-main-rnd blue lg" OnClick="button_OnClick" OnClientClick="return fnValidateForm();" meta:resourcekey="btnSaveResource" />
            <!--<asp:Button ID="btnRewrite" runat="server" Text="Rewrite" CssClass="button-default blue" OnClick="button_OnClick" meta:resourcekey="btnRewriteResource" />-->
        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>