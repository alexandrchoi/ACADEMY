<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="survey_results_detail.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.survey_results_detail"
    Culture="auto" UICulture="auto" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="C1.Web.C1WebChart.2" Namespace="C1.Web.C1WebChart" TagPrefix="C1WebChart" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register Assembly="C1.Web.C1WebChart.2, Version=2.0.20063.16228, Culture=neutral, PublicKeyToken=360971499c5cdc04" Namespace="C1.Web.C1WebChart" TagPrefix="C1WebChart" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">

</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">


<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="설문 결과" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
            <button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
        </span>
    </h2>

    <section class="section-board">

        <div class="board-list gm-table data-table read-type survey-input">
            <asp:PlaceHolder ID="ph01" runat="server" />
        </div>

        <!-- 버튼 -->
        <div class="button-group center">
            <asp:Button ID="btnList" CssClass="button-main-rnd lg" Text="List" OnClick="btnList_OnClick" runat="server" meta:resourcekey="btnListResource" />
        </div>
    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>