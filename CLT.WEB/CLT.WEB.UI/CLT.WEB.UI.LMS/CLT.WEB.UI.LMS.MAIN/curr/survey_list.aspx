<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="survey_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.survey_list"
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">
    function fnValidateForm()
    {
        if(!isDateChk(document.getElementById('<%=txtRes_From.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblSurvayPeriod.Text }, new string[] { lblSurvayPeriod.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(!isDateChk(document.getElementById('<%=txtRes_To.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblSurvayPeriod.Text }, new string[] { lblSurvayPeriod.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

        return true;
    }

    var deletemsg = '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A013", new string[] { "설문조사" }, new string[] { "Survey" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>';
</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">


<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="설문 등록/조회" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
            <button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
        </span>
    </h2>

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblSurvayPeriod" runat="server" Text="설문기간" CssClass="title" meta:resourcekey="lblSurvayPeriod" />
                <asp:TextBox ID="txtRes_From" runat="server" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtRes_To" runat="server" CssClass="datepick" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblSurveytitle" runat="server" Text="설문제목" CssClass="title" meta:resourcekey="lblSurveytitle" />
                <asp:TextBox ID="txtRes_NM" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="Search" CssClass="button-default blue" OnClick="btnRetrieve_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnRetrieveResource" />
        </div>

        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" CssClass="button-default blue" meta:resourcekey="btnNewResource" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button-default" meta:resourcekey="btnDeleteResource" OnClientClick="return confirm(deletemsg);" OnClick="btnDelete_Click" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" meta:resourcekey="btnExcelResource" OnClick="btnExcel_Click" />
            <asp:Button ID="btnNotice" runat="server" Text="Notice" CssClass="button-default" meta:resourcekey="btnNoticeResource" OnClick="btnNotice_Click" />
        </div>

        <!-- Data Table - List type -->

        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="grd_OnItemDataBound" DataKeyField="res_no" OnItemCreated="grd_ItemCreated">
                <Columns>

                <C1WebGrid:C1TemplateColumn>
                    <HeaderTemplate>
                        <input type="checkbox" id="chkHeader" name="chkHeader" onclick="CheckAll(this, 'chkEdit');" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" id="chkEdit" name="chkEdit" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" />
                </C1WebGrid:C1TemplateColumn >
                <C1WebGrid:C1TemplateColumn HeaderText="설문제목">
                    <ItemTemplate>
                        <a id="res_sub1" href="/curr/survey_insdetail.aspx?rMode=edit&rResNo=<%# DataBinder.Eval(Container.DataItem, "res_no")%>&rResQueCnt=<%# DataBinder.Eval(Container.DataItem, "res_que_cnt")%>&rResSub=<%# HttpUtility.UrlEncode(DataBinder.Eval(Container.DataItem, "res_sub").ToString())%>&rResDate=<%# HttpUtility.UrlEncode(DataBinder.Eval(Container.DataItem,"res_date").ToString()) %>&rNoticeyn=<%# DataBinder.Eval(Container.DataItem,"notice_yn") %>&MenuCode=<%=Session["MENU_CODE"]%>"><%# DataBinder.Eval(Container.DataItem, "res_sub")%></a>
                    </ItemTemplate>
                    <ItemStyle Width="" CssClass ="left" />
                </C1WebGrid:C1TemplateColumn>
                <C1WebGrid:C1BoundColumn DataField="res_que_cnt" HeaderText="항목수">
                    <ItemStyle Width="7%" />
                </C1WebGrid:C1BoundColumn>
                <C1WebGrid:C1BoundColumn DataField="ins_dt" HeaderText="설문등록일">
                    <ItemStyle Width="10%" />
                </C1WebGrid:C1BoundColumn>
                <C1WebGrid:C1BoundColumn DataField="res_date" HeaderText="응답기간">
                    <ItemStyle Width="17%" />
                </C1WebGrid:C1BoundColumn>
                <C1WebGrid:C1BoundColumn  DataField="notice_yn" HeaderText="게시여부">
                    <ItemStyle Width="7%"  />
                </C1WebGrid:C1BoundColumn>
                <C1WebGrid:C1BoundColumn DataField="res_sum_cnt" HeaderText="총대상자">
                    <ItemStyle Width="7%" />
                </C1WebGrid:C1BoundColumn>
                <C1WebGrid:C1BoundColumn DataField="res_rec_cnt" HeaderText="응답수">
                    <ItemStyle Width="7%" />
                </C1WebGrid:C1BoundColumn>
                <C1WebGrid:C1BoundColumn DataField="res_nrec_cnt" HeaderText="미응답수">
                    <ItemStyle Width="7%" />
                </C1WebGrid:C1BoundColumn>
                <C1WebGrid:C1BoundColumn Visible="false" DataField="res_sub" HeaderText="설문제목">
                    <ItemStyle Width="0%" CssClass ="left" />
                </C1WebGrid:C1BoundColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>
        <div class="gm-paging">
            <CLTWebControl:PageNavigator ID="PageNavigator1" runat="server" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" />
        </div>
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 End -->

    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>