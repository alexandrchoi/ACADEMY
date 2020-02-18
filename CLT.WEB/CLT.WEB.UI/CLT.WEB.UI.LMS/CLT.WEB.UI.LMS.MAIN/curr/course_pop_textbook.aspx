<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="course_pop_textbook.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.course_pop_textbook" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">
            function OK()
            {
                var datas = Array();

                datas[0] = "<%= textbookItems %>"; // 형태( 각각의 Row 콤마(凸) 로 구분)

                opener.setTextbookAdd(datas);
                opener.focus();
                self.close();
            }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p></p>-->

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblTextBookType" runat="server" meta:resourcekey="lblTextBookType" />
                <asp:DropDownList ID="ddlTextBookType" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseGroup" runat="server" meta:resourcekey="lblCourseGroup" />
                <asp:DropDownList ID="ddlCourseGroup" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseField" runat="server" meta:resourcekey="lblCourseField" />
                <asp:DropDownList ID="ddlCourseField" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblTextBookLang" runat="server" meta:resourcekey="lblTextBookLang" />
                <asp:DropDownList ID="ddlTextBookLang" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblTextBookNM" runat="server" meta:resourcekey="lblTextBookNM" />
                <asp:TextBox ID="txtTextBookNM" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblPeriod" runat="server" meta:resourcekey="lblPeriod" />
                <asp:TextBox ID="txtBeginDt" runat="server" MaxLength="10" CssClass="datepick w180" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtEndDt" runat="server" MaxLength="10" CssClass="datepick w180" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="Search" CssClass="button-default blue" OnClick="btnRetrieve_Click" />
        </div>

        <div class="button-box right">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button-default blue" OnClick="btnAdd_Click" />
        </div>


        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type mt10">
            <C1WebGrid:C1WebGrid ID="grd" runat="server" AllowSorting="True"  AllowColSizing="True" CssClass="grid_main" AutoGenerateColumns="false"
                DataKeyField="TEXTBOOK_ID" OnItemCreated="grd_ItemCreated">
                <Columns>

                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="textbook_type">
                        <ItemStyle Width="7%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="textbook_nm">
                        <ItemStyle Width="20%" CssClass ="left"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="author">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="publisher">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="textbook_lang">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="user_nm_kor">
                        <ItemStyle Width="12%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="use_flg">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="temp_save_flg" >
                            <ItemStyle Width="7%" />
                        </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <asp:CheckBox ID="chk" runat = "server"/>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1BoundColumn DataField="textbook_id" Visible ="false" >
                        <ItemStyle Width="0%" />
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
