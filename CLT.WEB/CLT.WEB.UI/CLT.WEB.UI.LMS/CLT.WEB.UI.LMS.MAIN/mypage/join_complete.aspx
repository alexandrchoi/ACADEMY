<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="join_complete.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.join_complete" 
    Culture="auto" UICulture="auto" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.FX.UTIL" Assembly="CLT.WEB.UI.FX.UTIL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    
    <script type="text/javascript" language="javascript">

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

        <section class="join-complete">
            <p class="headline">회원가입신청이 성공적으로 완료되었습니다.</p>
            <p>관리자 승인 후(1~2일 소요) 서비스를 이용하실 수 있습니다.<br>문의 : 051-330-9384</p>
            <div class="button-group">
                <a href="/" class="button-main-rnd lg blue">HOME</a>
            </div>
        </section>

    </div>
    <!--// 서브 컨텐츠 끝 -->


</asp:Content>