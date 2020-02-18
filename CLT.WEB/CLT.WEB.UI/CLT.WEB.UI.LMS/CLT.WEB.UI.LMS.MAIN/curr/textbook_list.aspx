<%@ Page Language="C#" AutoEventWireup="true" Codebehind="textbook_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.textbook_list" Culture="auto" UICulture="auto" MasterPageFile="/MasterSub.Master" %>

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
                <asp:Label ID="lblTextBookType" CssClass="title" runat="server" meta:resourcekey="lblTextBookType" />
                <asp:DropDownList ID="ddlTextBookType" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseGroup" CssClass="title" runat="server" meta:resourcekey="lblCourseGroup" />
                <asp:DropDownList ID="ddlCourseGroup" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseField" CssClass="title" runat="server" meta:resourcekey="lblCourseField" />
                <asp:DropDownList ID="ddlCourseField" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblTextBookLang" CssClass="title" runat="server" meta:resourcekey="lblTextBookLang" />
                <asp:DropDownList ID="ddlTextBookLang" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblTextBookNM" CssClass="title" runat="server" meta:resourcekey="lblTextBookNM" />
                <asp:TextBox ID="txtTextBookNM" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblPeriod" CssClass="title" runat="server" meta:resourcekey="lblPeriod" />
                <asp:TextBox ID="txtBeginDt" runat="server" MaxLength="10" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtEndDt" runat="server" MaxLength="10" CssClass="datepick" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />

        </div>

        <!-- 버튼 -->
        <div class="button-box right">
            <!-- 개발자 수정 영역 1 - 버튼 추가 영역 Start -->
            <input type="button" value="New" class="button-default blue" onclick="javascript:openPopWindow('/curr/textbook_edit.aspx?MenuCode=<%=Session["MENU_CODE"]%>','textbook_edit', '940', '500');" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" OnClick="btnExcel_Click" />
            <!-- 개발자 수정 영역 1 - 버튼 추가 영역 End -->
        </div>

        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" runat="server" AllowSorting="True"  AllowColSizing="True"  CssClass="grid_main" AutoGenerateColumns="false" DataKeyField="TEXTBOOK_ID" OnItemCreated="C1WebGrid1_ItemCreated">
                <Columns>

                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="textbook_type">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <a href="#" onclick="openPopWindow('/curr/textbook_edit.aspx?textbook_id=<%# DataBinder.Eval(Container.DataItem, "TEXTBOOK_ID")%>&MenuCode=<%=Session["MENU_CODE"]%>','textbook_edit_win', '940', '500');">
                                <%# DataBinder.Eval(Container.DataItem, "textbook_nm")%>
                            </a>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="author">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="publisher">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="textbook_lang">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="user_nm_kor">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="use_flg">
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
