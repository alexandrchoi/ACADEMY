<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="course_pop_subject.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.course_pop_subject" 
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
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p></p>-->

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <!--언어-->
                <asp:Label ID="lblLang" CssClass="title" runat="server" meta:resourcekey="lblLang" />
                <asp:DropDownList ID="ddlLang" runat="server" />
            </div>
            <div class="search-group">
                <!--분류-->
                <asp:Label ID="lblClassification" CssClass="title" runat="server" meta:resourcekey="lblClassification" />
                <asp:DropDownList ID="ddlClassification" runat="server" />
            </div>
            <div class="search-group">
                <!--과목명 -->
                <asp:Label ID="lblSubject" CssClass="title" runat="server" meta:resourcekey="lblSubject" />
                <asp:TextBox ID="txtSubject" runat="server" />
            </div>
            <div class="search-group">
                <!--강사명 -->
                <asp:Label ID="lblInstructor" CssClass="title" runat="server" meta:resourcekey="lblInstructor" />
                <asp:TextBox ID="txtInstructor" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat ="server" Text="Search" CssClass="button-board-search" OnClick = "btnRetrieve_Click" />
        </div>


        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type mt10">

            <C1WebGrid:C1WebGrid ID="grd" runat="server" AllowSorting="True"  AllowColSizing="True"  CssClass="grid_main" AutoGenerateColumns="false" OnItemCreated="grd_ItemCreated">
                <Columns>

                    <C1WebGrid:C1TemplateColumn >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="subject_lang">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="subject_kind" >
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <a href="#" onclick="setTextValOfOpener1(self, opener, '<%=ViewState["SUBJECT_ID"].ToString() %>', '<%# DataBinder.Eval(Container.DataItem, "subject_id")%>', '<%=ViewState["SUBJECT_NM"].ToString() %>', '<%# DataBinder.Eval(Container.DataItem, "subject_nm")%>'); opener.setSubject( '<%# DataBinder.Eval(Container.DataItem, "subject_id")%>'); ">
                            <%# DataBinder.Eval(Container.DataItem, "subject_nm")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="31%" CssClass ="left" />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1BoundColumn DataField="learning_time" >
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="instructor" >
                        <ItemStyle Width="15%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="use_flg" >
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="temp_save_flg" >
                        <ItemStyle Width="8%" />
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
