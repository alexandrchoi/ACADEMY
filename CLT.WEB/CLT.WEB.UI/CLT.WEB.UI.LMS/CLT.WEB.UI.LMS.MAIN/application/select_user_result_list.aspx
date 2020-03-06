<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="select_user_result_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.select_user_result_list" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">

    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="교육대상자 확정 명단" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p></p>-->

        <div class="button-box right">
            <asp:Button ID = "btnRetrieve" runat ="server" Text="Search" CssClass="button-default blue" OnClick = "btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" OnClick="btnExcel_Click" meta:resourcekey="btnExcelResource" />
        </div>

        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type mt10">
            <C1WebGrid:C1WebGrid ID="grdList" runat="server" AllowSorting="True" AllowColSizing="True" AutoGenerateColumns="False" OnItemDataBound="grdList_ItemDataBound" DataKeyField="KEYS" OnItemCreated="grdList_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1TemplateColumn HeaderText="No.">
                        <ItemStyle Width="30px" />
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + 100 * (this.PageNavigator1.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="DEPT_NAME" HeaderText="부서명">
                        <ItemStyle CssClass="left" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="STEP_NAME" HeaderText="직급">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="USER_ID" HeaderText="사번">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="USER_NM_KOR" HeaderText="성명">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="ORD_FDATE" HeaderText="최종선박하선일">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="INS_DTIME" HeaderText="교육신청일시">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="USER_COURSE_END_DT" HeaderText="이전이수일">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle  Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>
        
        <div class="gm-paging">
            <CLTWebControl:PageNavigator ID="PageNavigator1" runat="server" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" />
        </div>
    
    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>
