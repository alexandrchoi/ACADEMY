<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.login" 
    Culture="auto" UICulture="auto" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.FX.UTIL" Assembly="CLT.WEB.UI.FX.UTIL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    
    <script type="text/javascript" language="javascript">

        function fnLogin()
        {   
            if(isEmpty(document.getElementById('<%=txtUserID.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { "아이디" }, new string[] { "User ID" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtPassword.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { "패스워드" }, new string[] { "Password" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            return true;
        }
    </script>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <asp:HiddenField ID="backURL" runat="server" />

    <!-- 서브 컨텐츠 시작 -->
    <div class="section-fix">
        <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="로그인" meta:resourcekey="lblMenuTitle"></asp:Label>
            <!-- 모바일 뒤로 가기 -->
            <span class="goback">
			        <button onclick="goBack();return false;"><span class="off-screen">Go back</span></button>
			    </span>
        </h2>
        <section class="login gm-des-input">
            <div class="login-box">
                    <div class="idpw">
                        <label for="<%=txtUserID.ClientID %>"><%=GetLocalResourceObject("lblUserID") %><!--아이디--></label>
                        <asp:TextBox ID="txtUserID" runat="server"></asp:TextBox>
                        <label for="<%=txtPassword.ClientID %>"><%=GetLocalResourceObject("lblPassword") %><!--비밀번호--></label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                    </div>

                    <div class="button-box tight">
                        <asp:Button ID="btn_login" runat="server" CssClass="button-login" OnClientClick="return fnLogin()"  OnClick="btn_login_Click" meta:resourcekey="btn_loginResource" Text="로그인" />
                    </div>

                    <div class="on-off-slider save-id">
                        <asp:Label ID="txtSaveID" runat="server" CssClass="label-title" meta:resourcekey="txtSaveIDResource" /><!--아이디저장-->
                        <label class="switch">
                            <asp:CheckBox ID="idSaveCheck" runat="server" Checked="true" />
                            <span class="slider round"></span>
                        </label>
                    </div>

                <!-- 2019.12.30 아이디찾기, 비밀번호찾기 주석처리
                <div class="log-link">
                    <a href="#none"><%=GetLocalResourceObject("txtFindID") %>아이디찾기</a>
                    <a href="#none"><%=GetLocalResourceObject("txtFindPW") %>비밀번호찾기</a>
                    <a href="#none"><%=GetLocalResourceObject("txtSignUp") %>회원가입</a>
                </div-->
                
            </div>
        </section>
    </div>
    <!--// 서브 컨텐츠 끝 -->


</asp:Content>