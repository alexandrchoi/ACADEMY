<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="codemaster_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.MANAGE.codemaster_list" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">
    function GoCodeForm()
    {
        openPopWindow('/manage/codemaster_edit.aspx?openerEditMode=m_cd_new','codemaster_list', '580', '460');
        return false;
    }
</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="공통코드 그룹 조회/등록" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblCode" runat="server" Text="코드" meta:resourcekey="lblCode" />
                <asp:TextBox ID="txtM_CD" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCodeName" runat="server" Text="코드명" meta:resourcekey="lblCodeName" />
                <asp:TextBox ID="txtM_DESC" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblENM" runat="server" Text="영문명" meta:resourcekey="lblENM" />
                <asp:TextBox ID="txtM_ENM" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblKNM" runat="server" Text="한글명" meta:resourcekey="lblKNM" />
                <asp:TextBox ID="txtM_KNM" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="Search" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button-default" OnClick="btnClear_OnClick" meta:resourcekey="btnClearResource" />
        </div>

        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnNew" runat="server" Text="New" CssClass="button-default blue" OnClientClick="javascript:return GoCodeForm();" meta:resourcekey="btnNewResource" />
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button-default" OnClick="btnSave_Click" meta:resourcekey="btnSaveResource" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" OnClick="btnExcel_Click" meta:resourcekey="btnExcelResource" />
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" CurrentPageIndex="1" PageSize="15" TotalRecordCount="100" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="C1WebGrid1_ItemDataBound" DataKeyField="M_CD" OnItemCreated="grd_ItemCreated">
                <Columns>

                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <input type="checkbox" id="chkHeader" name="chkHeader" onclick="CheckAll(this, 'chkEdit');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" id="chkEdit" name="chkEdit" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="3%" HorizontalAlign="Center"></ItemStyle>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="코드">
                        <ItemTemplate>
                            <a href="codedetail_list.aspx?openerEditMode=m_cd_edit&openerm_cd=<%# DataBinder.Eval(Container.DataItem, "m_cd")%>"><%# DataBinder.Eval(Container.DataItem, "m_cd")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="6%" HorizontalAlign="Center" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="코드명">
                        <ItemTemplate>
                            <a href="#" onclick="javascript:openPopWindow('/manage/codemaster_edit.aspx?openerEditMode=m_cd_edit&openerm_cd=<%# DataBinder.Eval(Container.DataItem, "m_cd")%>','codemaster_list', '580', '460');"><%# DataBinder.Eval(Container.DataItem, "m_desc")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="22%" HorizontalAlign="left" CssClass="left" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="m_enm" HeaderText="영문명">
                        <ItemStyle Width="16%" HorizontalAlign="Left" CssClass="left"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>               
                    <C1WebGrid:C1BoundColumn DataField="m_knm" HeaderText="한글명">
                        <ItemStyle Width="16%" HorizontalAlign="Left" CssClass="left"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>               
                    <C1WebGrid:C1TemplateColumn  HeaderText="사용유무">
                        <ItemTemplate>
                            <style type="text/css">
                                .HiddenText label {display:none;}
                            </style>
                            <input type="checkbox" id="chkuse_yn" name="chkuse_yn" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "m_cd") %>' class="HiddenText" />
                        </ItemTemplate>
                        <ItemStyle Width="8%" HorizontalAlign="Center"></ItemStyle>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="전송유무">
                        <ItemTemplate>
                            <asp:Label ID="lblsent_yn" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="7%" HorizontalAlign="Center" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="sent_dt" HeaderText="선박전송일자">
                        <ItemStyle Width="11%" HorizontalAlign="Center"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>   
                    <C1WebGrid:C1TemplateColumn HeaderText="등록">
                        <ItemTemplate>
                            <input type="button" class="button-underline" value="Create" onclick="javascript:openPopWindow('/manage/codedetail_edit.aspx?openerEditMode=d_cd_new&openerm_cd=<%# DataBinder.Eval(Container.DataItem, "m_cd")%>&openeruser_code_nm=<%# DataBinder.Eval(Container.DataItem, "user_code_yn")%>','codemaster_new', '580', '440');" />
                        </ItemTemplate>
                        <ItemStyle Width="8%" HorizontalAlign="Center"></ItemStyle>
                    </C1WebGrid:C1TemplateColumn>
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