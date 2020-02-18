<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="course_pop_contents.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.course_pop_contents" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">
            function OK(gubun)
            {
                var datas = Array();

                datas[0] = "<%= listItems %>"; // 형태( 각각의 Row 콤마(凸) 로 구분)

                opener.setDispDetail(gubun, datas);
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
                <!--Name-->
                <asp:Label ID="lblContentsNm" CssClass="title" runat="server" meta:resourcekey="lblContentsNm" />
                <asp:TextBox ID="txtContentsNM" runat="server" />
            </div>
            <div class="search-group">
                <!--Field-->
                <asp:Label ID="lblField" CssClass="title" runat="server" meta:resourcekey="lblField" />
                <asp:DropDownList ID="ddlContentsType" runat="server" />
            </div>
            <div class="search-group">
                <!--언어 -->
                <asp:Label ID="lblLang" CssClass="title" runat="server" meta:resourcekey="lblLang" />
                <asp:DropDownList ID="ddlContentsLang" runat="server" />
            </div>
            <div class="search-group">
                <!--비고 -->
                <asp:Label ID="lblRemark" CssClass="title" runat="server" meta:resourcekey="lblRemark" />
                <asp:TextBox ID="txtRemark" runat="server" />
            </div>
            <asp:Button ID = "btnRetrieve" runat ="server" Text="Search" CssClass="button-board-search" OnClick = "btnRetrieve_Click" />
        </div>
        
        <div class="button-box right">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button-default blue" OnClick="btnAdd_Click" />
        </div>


        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type mt10">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" runat="server" AllowSorting="True"  AllowColSizing="True"  CssClass="grid_main" AutoGenerateColumns="false"
                DataKeyField="CONTENTS_ID" OnItemCreated="C1WebGrid1_ItemCreated">
                <Columns>

                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="contents_nm">
                        <ItemStyle Width="25%" CssClass ="left" />
                    </C1WebGrid:C1BoundColumn>
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
                        <ItemStyle Width="25%" CssClass ="left" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
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

                    <C1WebGrid:C1BoundColumn DataField="contents_id" Visible="false">
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
