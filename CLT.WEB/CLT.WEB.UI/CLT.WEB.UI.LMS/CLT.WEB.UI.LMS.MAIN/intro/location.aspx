<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="location.aspx.cs" Inherits="CLT.WEB.UI.LMS.INTRO.location" 
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <!-- 서브 컨텐츠 시작 -->
    <div class="section-fix">

        <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" meta:resourcekey="lblMenuTitle" /><!--오시는길-->
            <!-- 모바일 뒤로 가기 -->
            <span class="goback">
			    <button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
			</span>
        </h2>

        <section class="map-box">
            <dl class="bullet-list">
                <dt><asp:Label ID="txtAddr" runat="server" Text="주소" meta:resourcekey="txtAddrResource" /></dt>
                <dd><asp:Label ID="txtAddress" runat="server" meta:resourcekey="txtAddressResource" /><!--부산시 동구 중앙대로 331, 메리츠타워 13층--></dd>
                <dt><asp:Label ID="txtSupport" runat="server" Text="교육 및 협약 문의" meta:resourcekey="txtSupportResource" /></dt>
                <dd>
                    <span><span class="icon-bg icon-tel">전화</span><asp:Label ID="txtTel" runat="server" Text="051-330-9387" meta:resourcekey="txtTelResource" /></span>
                    <span><span class="icon-bg icon-mail">이메일</span><asp:Label ID="txtEmail" runat="server" Text="yejikim@gmarineservice.com" meta:resourcekey="txtEmailResource" /></span>
                </dd>
            </dl>

            <div class="map">
                <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3263.2308742467935!2d129.04310971524362!3d35.12591168032754!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3568ebc388a4d7a7%3A0x8215e8b5cccfe383!2s331%20Jungang-daero%2C%20Choryang%203(sam)-dong%2C%20Dong-gu%2C%20Busan!5e0!3m2!1sen!2skr!4v1576739006946!5m2!1sen!2skr" width="100%" height="450" frameborder="0" style="border:0;" allowfullscreen=""></iframe>
            </div>
        </section>

    </div>
    <!--// 서브 컨텐츠 끝 -->

</asp:Content>