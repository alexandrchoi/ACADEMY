<%@ Page Language="C#" AutoEventWireup="true" Codebehind="content_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.content_list" Culture="auto" UICulture="auto" MasterPageFile="/MasterSub.Master" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">

</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="" meta:resourcekey="lblMenuTitle" />

        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    <!--p>설명</p-->

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblContentsNm" CssClass="title" runat="server" meta:resourcekey="lblContentsNm" />
                <asp:TextBox ID="txtContentsNM" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblField" CssClass="title" runat="server" meta:resourcekey="lblField" />
                <asp:DropDownList ID="ddlContentsType" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblLang" CssClass="title" runat="server" meta:resourcekey="lblLang" />
                <asp:DropDownList ID="ddlContentsLang" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblRemark" CssClass="title" runat="server" meta:resourcekey="lblRemark" />
                <asp:TextBox ID="txtRemark" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />

        </div>

        <!-- 버튼 -->
        <div class="button-box right">
            <!-- 개발자 수정 영역 1 - 버튼 추가 영역 Start -->
            <input type="button" value="New" class="button-default blue" onclick="javascript:openPopWindow('/curr/content_edit.aspx?contents_id=NEW&MenuCode=<%=Session["MENU_CODE"]%>','content_edit', '700', '420');" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" OnClick="btnExcel_Click" />
            <!-- 개발자 수정 영역 1 - 버튼 추가 영역 End -->
        </div>

        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" runat="server" AllowSorting="True"  AllowColSizing="True"  CssClass="grid_main" AutoGenerateColumns="false" DataKeyField="CONTENTS_ID" OnItemCreated="C1WebGrid1_ItemCreated">
                <Columns>

                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <a href="#" onclick="openPopWindow('/curr/content_edit.aspx?contents_id=<%# DataBinder.Eval(Container.DataItem, "CONTENTS_ID")%>&MenuCode=<%=Session["MENU_CODE"]%>','content_edit_win', '700', '420');">
                                <%# DataBinder.Eval(Container.DataItem, "contents_nm")%>
                            </a>
                        </ItemTemplate>
                        <ItemStyle Width="25%" CssClass="left" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="contents_file_nm">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="contents_type">
                        <ItemStyle Width="7%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="contents_lang">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="contents_remark">
                        <ItemStyle Width="35%" CssClass="left" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                        <ItemStyle Width="10%" />
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
