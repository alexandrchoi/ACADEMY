<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="opencourse_pop_survey.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.opencourse_pop_survey" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">

    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="Label1" runat="server" Text="" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p></p>-->

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblResDt" runat="server" CssClass="title" meta:resourcekey="lblResDt" />
                <asp:TextBox ID="txtBeginDt" runat="server" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtEndDt" runat="server" CssClass="datepick" />
                <asp:TextBox ID="txtResNM" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblResNm" runat="server" CssClass="title" meta:resourcekey="lblResNm" />
            </div>
            <asp:Button ID = "btnRetrieve" runat ="server" Text="Search" CssClass="button-board-search" OnClick = "btnRetrieve_Click" />
        </div>
        
        <div class="button-box right">
        </div>


        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type mt10">
            <C1WebGrid:C1WebGrid ID="grd" runat="server" AllowSorting="True"  AllowColSizing="True"  CssClass="grid_main" AutoGenerateColumns="false" OnItemCreated="grd_ItemCreated">
                <Columns>

                    <C1WebGrid:C1BoundColumn DataField="res_NO" Visible="false" >
                        <ItemStyle Width="0%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="res_sub" Visible="false">
                        <ItemStyle Width="0%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1TemplateColumn >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1TemplateColumn >
                        <ItemTemplate>
                            <a href="#" onclick="setTextValOfOpener1(self, opener, '<%=ViewState["RES_NO"].ToString() %>', '<%# DataBinder.Eval(Container.DataItem, "res_no")%>', '<%=ViewState["RES_SUB"].ToString() %>', '<%# DataBinder.Eval(Container.DataItem, "res_sub")%>');">
                            <%# DataBinder.Eval(Container.DataItem, "res_sub")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="23%" CssClass ="left" />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1BoundColumn DataField="res_que_cnt" >
                        <ItemStyle Width="7%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                        <ItemStyle Width="11%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="res_date" >
                        <ItemStyle Width="24%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="notice_yn" >
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="res_sum_cnt" >
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="res_rec_cnt" >
                        <ItemStyle Width="7%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="res_nrec_cnt" >
                        <ItemStyle Width="7%" />
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

    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>
