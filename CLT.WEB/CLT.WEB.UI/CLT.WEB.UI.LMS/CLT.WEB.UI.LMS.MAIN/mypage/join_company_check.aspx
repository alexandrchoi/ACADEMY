<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="join_company_check.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.join_company_check" 
    Culture="auto" UICulture="auto" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.FX.UTIL" Assembly="CLT.WEB.UI.FX.UTIL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    
    <script type="text/javascript" language="javascript">
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
	        var taxno = document.getElementById('<%=txtRegNo1.ClientID %>').value + document.getElementById('<%=txtRegNo2.ClientID %>').value + document.getElementById('<%=txtRegNo3.ClientID %>').value;
	
	        if(isEmpty(document.getElementById('<%=txtCompanyName.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCompanyNameTitle.Text }, new string[] { lblCompanyNameTitle.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        else if(isMaxLenth(document.getElementById('<%=txtCompanyName.ClientID %>'),100,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblCompanyNameTitle.Text,"60","100" }, new string[] { lblCompanyNameTitle.Text,"60","100" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    
	        else if(!isNumber(document.getElementById('<%=txtRegNo1.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A108", new string[] { "" }, new string[] { "" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        else if(!isNumber(document.getElementById('<%=txtRegNo2.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A108", new string[] { "" }, new string[] { "" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	        else if(!isNumber(document.getElementById('<%=txtRegNo3.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A108", new string[] { "" }, new string[] { "" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    
	        // 사업자 등록번호 체크
	        if (checkBizID(taxno, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { lblRegNo.Text }, new string[] { lblRegNo.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	    
	        return true;
	    }
	
        function movenext1()
        {
            var x = document.getElementById('<%=txtRegNo1.ClientID %>').value;
        
            if(x.length == 3)
              document.getElementById('<%=txtRegNo2.ClientID %>').focus();
        }		
    
        function movenext2()
        {
            var x = document.getElementById('<%=txtRegNo2.ClientID %>').value;
        
            if(x.length == 2)
              document.getElementById('<%=txtRegNo3.ClientID %>').focus();
        }	
    </script>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <asp:HiddenField ID="backURL" runat="server" />

    <!-- 서브 컨텐츠 시작 -->
    <div class="section-fix">
        <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="회원가입" meta:resourcekey="lblMenuTitle"></asp:Label>
            <!-- 모바일 뒤로 가기 -->
            <span class="goback">
			        <button onclick="goBack();return false;"><span class="off-screen">Go back</span></button>
			    </span>
        </h2>
        
        <section class="join-check center">
            <p class="headline mb30 black">회사명과 사업자 등록번호를 입력해 주세요.</p>

            <div class="table-grid gm-des-input">
                <dl>
                    <dt><asp:Label ID="lblCompanyNameTitle" runat="server" Text="회사명" meta:resourcekey="lblCompanyNameTitle" /></dt>
                    <dd><asp:TextBox ID="txtCompanyName" runat="server" CssClass="w100per" /></dd>
                    <dt><asp:Label  ID="lblRegNo" runat="server" Text="사업자등록번호" meta:resourcekey="lblRegNo" /></dt>
                    <dd>
                        <asp:TextBox ID="txtRegNo1" runat="server" MaxLength="3" onkeyup ="movenext1();" CssClass="w180" />
                        <span class="gm-text2">-</span>
				        <asp:TextBox ID="txtRegNo2" runat="server" MaxLength="2" onkeyup ="movenext2();" CssClass="w80" />
				        <span class="gm-text2">-</span>
				        <asp:TextBox ID="txtRegNo3" runat="server" MaxLength="5" CssClass="w180" />
                    </dd>
                </dl>
            </div>
            <div class="button-group center">
                <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="button-main-rnd xlg blue" OnClick="btnNext_Click" OnClientClick="return fnValidateForm();" />
            </div>
        </section>


    </div>
    <!--// 서브 컨텐츠 끝 -->


</asp:Content>