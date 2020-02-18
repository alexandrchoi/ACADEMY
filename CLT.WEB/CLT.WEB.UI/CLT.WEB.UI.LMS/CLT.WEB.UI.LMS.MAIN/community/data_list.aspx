<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="data_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.COMMUNITY.data_list" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">

</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:HiddenField ID="HiddenCourseID" runat="server" />
    <asp:HiddenField ID="HiddenCourseNM" runat="server" />
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="자료실" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnNew" runat="server" Text="New" CssClass="button-default blue" OnClick="btnNew_OnClick" meta:resourcekey="btnNewResource" />
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <div class="table-option">
            <asp:Label ID="lblDelYn" runat="server" Text="삭제유무" CssClass="title" meta:resourcekey="lblDelYn" />
            <asp:DropDownList ID="ddlDelYn" runat="server" OnSelectedIndexChanged="DdlDelYn_SelectedIndexChanged" AutoPostBack="True" />
        </div>
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" CurrentPageIndex="1" PageSize="15" TotalRecordCount="100" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="C1WebGrid1_ItemDataBound" DataKeyField="boa_no" OnItemCreated="grd_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1BoundColumn DataField="rnum" HeaderText="번호">
                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="제목">
                        <ItemTemplate>
                            <a id="boa_sub" href="/community/data_detail.aspx?seq=<%# DataBinder.Eval(Container.DataItem, "boa_no")%>&MenuCode=<%=Session["MENU_CODE"]%>&delYN=<%=ddlDelYn.SelectedValue %>"><%# DataBinder.Eval(Container.DataItem, "boa_sub")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="" HorizontalAlign="Left" CssClass="left" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <asp:Image ID="imgHeader" runat="server" CssClass="icon-attach" ImageUrl="/asset/Images/clip.png" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Image ID="imgItems" CssClass="icon-attach" runat="server"/>
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>                
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="user_nm_kor" HeaderText="작성자">
                        <ItemStyle Width="15%"  HorizontalAlign="Center"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>                
                    <C1WebGrid:C1BoundColumn DataField="insdt" HeaderText="작성일자">
                        <ItemStyle Width="15%"  HorizontalAlign="Center"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>                               
                    <C1WebGrid:C1BoundColumn DataField="hit_cnt" HeaderText="조회수">
                        <ItemStyle Width="9%"  HorizontalAlign="Center"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>                                                
                    <C1WebGrid:C1BoundColumn DataField="del_yn" HeaderText="삭제유무">
                        <ItemStyle Width="9%"  HorizontalAlign="Center"></ItemStyle>
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

        <!-- 검색 -->
        <div class="search-box at-bottom">
            <div class="search-group">
                <asp:DropDownList ID="ddlType" runat="server" />
            </div>
            <div class="search-group">
                <asp:TextBox ID="txtContent" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="Search" CssClass="button-default blue" OnClick="btnRetrieve_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnRetrieveResource" />

        </div>

    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>