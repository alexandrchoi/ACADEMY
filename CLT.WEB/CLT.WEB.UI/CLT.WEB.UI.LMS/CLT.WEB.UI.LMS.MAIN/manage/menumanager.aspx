<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="menumanager.aspx.cs" Inherits="CLT.WEB.UI.LMS.MANAGE.menumanager" 
    Culture="auto" UICulture="auto" MaintainScrollPositionOnPostback="true" %>

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
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="메뉴권한" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblUserGroup" runat="server" Text="사용자 그룹" meta:resourcekey="lblUserGroup" />
                <asp:DropDownList ID="ddlCourseField" AutoPostBack="true" DataTextField="groupname" DataValueField="detailcode" runat="server" OnSelectedIndexChanged="ddlCourseFiled_OnSelectedIndexChanged" />
                &nbsp; &nbsp;
                <a style="font-size: medium; font-weight: bold;" href="#" onclick="saveMenuCD('2', '2', '0', '/manage/codedetail_list.aspx?openerEditMode=m_cd_edit&openerm_cd=0041');">☜ MASTER CODE :  0041</a>
            </div>
        </div>

        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button-default blue" OnClick="btnSave_Click" meta:resourcekey="btnSaveResource" />
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="C1WebGrid1_ItemDataBound" OnItemCreated="grd_ItemCreated">
                <Columns>
                    
                <C1WebGrid:C1BoundColumn DataField="menu1" HeaderText="대메뉴">
                    <ItemStyle Width="12%" CssClass="left"  />
                </C1WebGrid:C1BoundColumn>
                <C1WebGrid:C1BoundColumn DataField="menu2" HeaderText="중메뉴">
                    <ItemStyle Width="12%" CssClass="left"></ItemStyle>
                </C1WebGrid:C1BoundColumn>
                <C1WebGrid:C1BoundColumn  DataField="menu3" HeaderText="소메뉴">
                    <ItemStyle Width="13%" CssClass="left"></ItemStyle>
                </C1WebGrid:C1BoundColumn>
                <C1WebGrid:C1TemplateColumn HeaderText="조회권한">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkInquiry" AutoPostBack="true" OnCheckedChanged="chkInquiry_OnCheckedChanged" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="12%" HorizontalAlign="Center"></ItemStyle>
                </C1WebGrid:C1TemplateColumn>
                <C1WebGrid:C1TemplateColumn HeaderText="수정권한">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkEdit" AutoPostBack="true" OnCheckedChanged="chkEdit_OnCheckedChanged" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="12%" HorizontalAlign="Center"></ItemStyle>
                </C1WebGrid:C1TemplateColumn>
                <C1WebGrid:C1TemplateColumn HeaderText="삭제권한">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkDel" AutoPostBack="true" OnCheckedChanged="chkDel_OnCheckedChanged" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="12%" HorizontalAlign="Center"></ItemStyle>
                </C1WebGrid:C1TemplateColumn>
                <C1WebGrid:C1TemplateColumn HeaderText="관리자권한">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkAdmin" AutoPostBack="true" OnCheckedChanged="chkAdmin_OnCheckedChanged" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="12%" HorizontalAlign="Center"></ItemStyle>
                </C1WebGrid:C1TemplateColumn>
                
                <C1WebGrid:C1TemplateColumn HeaderText="menucode" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblmenu" runat="server" />
                    </ItemTemplate>
                </C1WebGrid:C1TemplateColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 End -->

    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>