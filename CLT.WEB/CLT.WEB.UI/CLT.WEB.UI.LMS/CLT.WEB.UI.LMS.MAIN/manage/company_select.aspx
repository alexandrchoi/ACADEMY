<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="company_select.aspx.cs" Inherits="CLT.WEB.UI.LMS.MANAGE.company_select" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script language="javascript" type="text/javascript">
		function chkSubmit()
		{	
			if(event.keyCode == 13)
			{	
				keyConfirm();
			}
		}
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <asp:HiddenField ID="hidCompany_id" runat="server" /> 
    

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">

        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="회사명 검색" meta:resourcekey="lblMenuTitle" /></h3>
        <p><asp:Label ID="lbl0" runat="server" meta:resourcekey="lblGuide0" Text="회사명을 입력 후 검색 버튼을 누르세요." /></p>

        <!-- 검색 -->
        <div class="message-box default center">
            <asp:Label ID="lblDong" runat="server" meta:resourcekey="lblDong" CssClass="title" Text="회사명" />
            <asp:TextBox ID="txtZipcode" runat="server" CssClass="board-keyword" OnTextChanged="txtZipcode_OnTextChanged" />
            <asp:Button ID = "btnRetrieve" runat ="server" Text="Search" CssClass="button-board-search" OnClick = "search_OnClick" />
        </div>

        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" runat="server" AllowSorting="True" AllowColSizing="False" AutoGenerateColumns="False" DataKeyField="company_id">
                <Columns>
                    
                    <C1WebGrid:C1TemplateColumn HeaderText="회사명">
                        <ItemTemplate>
                            <a href="#" onclick="setTextValOfOpener1(self, opener, '<%=ViewState["opener_textCompany_NM"].ToString() %>', '<%# DataBinder.Eval(Container.DataItem, "company_nm")%>' , '<%= ViewState["opener_Company_id"] %>', '<%# DataBinder.Eval(Container.DataItem, "company_id")%>' );">
                            <%# DataBinder.Eval(Container.DataItem, "company_nm")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="100%" />
                    </C1WebGrid:C1TemplateColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle  Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>
        
        <div class="gm-paging">

        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>