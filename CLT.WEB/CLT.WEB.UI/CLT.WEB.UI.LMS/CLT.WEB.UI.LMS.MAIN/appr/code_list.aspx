<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="code_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPR.code_list" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">

</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="역량평가코드" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnRetrieve" runat="server" Text="Retrieve" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />
            <asp:Button ID="btnNew" runat="server" Text="New" CssClass="button-default blue" meta:resourcekey="btnNewResource" Visible="False" />
            <asp:Button ID="btnDel" runat="server" Text="Delete" CssClass="button-default" meta:resourcekey="btnDelResource" OnClick="btnDel_Click" Enabled="False" Visible="False" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" meta:resourcekey="btnExcelResource" OnClick="btnExcel_Click" />
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grdList" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="grdList_ItemDataBound" DataKeyField="GRADE" OnItemCreated="grd_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <input type="checkbox" id="chk_all_sel" name="chk_all_sel" onclick="CheckAll(this, 'chk_sel');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" id="chk_sel" name="chk_sel" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="20px" />
                    </C1WebGrid:C1TemplateColumn >
                    <C1WebGrid:C1BoundColumn DataField="RNUM">
                        <ItemStyle Width="50px" />
                    </C1WebGrid:C1BoundColumn>                                
                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <a href="#" onclick="javascript:openPopWindow('/appr/code_edit.aspx?GRADE=<%# DataBinder.Eval(Container.DataItem, "GRADE")%>','code_edit_win', '580', '480');"><%# DataBinder.Eval(Container.DataItem, "GRADE")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="50px"/>
                    </C1WebGrid:C1TemplateColumn> 
                    <C1WebGrid:C1BoundColumn DataField="GRADE_NM">
                        <ItemStyle Width="5%"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="SCORE">
                        <ItemStyle Width="5%"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="GRADE_DESC">
                        <ItemStyle CssClass="left"></ItemStyle>
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