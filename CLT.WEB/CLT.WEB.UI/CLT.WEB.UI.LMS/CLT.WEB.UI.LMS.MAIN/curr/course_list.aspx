<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="course_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.course_list" Culture="auto" UICulture="auto" MasterPageFile="/MasterSub.Master" %>

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
            <!--과정그룹-->
                <asp:Label ID="lblGroup" CssClass="title" runat="server" meta:resourcekey="lblGroup" />
                <asp:DropDownList ID="ddlGroup" runat="server" />
            </div>
            <div class="search-group">
            <!--과정분야-->
                <asp:Label ID="lblField" CssClass="title" runat="server" meta:resourcekey="lblField" />
                <asp:DropDownList ID="ddlField" runat="server" />
            </div>
            <div class="search-group">
            <!--과정유형-->
                <asp:Label ID="lblType" CssClass="title" runat="server" meta:resourcekey="lblType" />
                <asp:DropDownList ID="ddlType" runat="server" />
            </div>
            <div class="search-group">
            <!--선종-->
                <asp:Label ID="lblVslType" CssClass="title" runat="server" meta:resourcekey="lblVslType" />
                <asp:DropDownList ID="ddlVslType" runat="server" />
            </div>
            <div class="search-group">
            <!--과정명-->
                <asp:Label ID="lblCourseNm" CssClass="title" runat="server" meta:resourcekey="lblCourseNm" />
                <asp:TextBox ID="txtCourseNm" runat="server" />
            </div>
            <div class="search-group">
            <!--언어-->
                <asp:Label ID="lblLang" CssClass="title" runat="server" meta:resourcekey="lblLang" />
                <asp:DropDownList ID="ddlLang" runat="server" />
            </div>
            <div class="search-group">
            <!--Usage-->
                <asp:Label ID="lblUse" CssClass="title" runat="server" meta:resourcekey="lblUse" />
                <asp:DropDownList ID="ddlUse" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />

        </div>
    
        <!-- 버튼 -->
        <div class="button-box right">
            <!-- 개발자 수정 영역 1 - 버튼 추가 영역 Start -->
            <input type="button" value="New" class="button-default blue" onclick="javascript:location.href='/curr/course_edit.aspx?temp_flg=Y&MenuCode=<%=Session["MENU_CODE"]%>'" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" OnClick="btnExcel_Click" />
            <!-- 개발자 수정 영역 1 - 버튼 추가 영역 End -->
        </div>

        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grd" runat="server" AllowSorting="True"  AllowColSizing="True" CssClass="grid_main" AutoGenerateColumns="false" OnItemCreated="grd_ItemCreated">
                <Columns>

                    <C1WebGrid:C1TemplateColumn >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="course_type">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>
                   <C1WebGrid:C1BoundColumn DataField="course_field" >
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>

                   <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <a href="#" onclick="javascript:location.href='/curr/course_edit.aspx?course_id=<%# DataBinder.Eval(Container.DataItem, "COURSE_ID")%>&MenuCode=<%=Session["MENU_CODE"]%>'">
                            <%# DataBinder.Eval(Container.DataItem, "course_nm")%>
                            </a>
                        </ItemTemplate>
                        <ItemStyle Width="" CssClass ="left"/>
                    </C1WebGrid:C1TemplateColumn>

                   <C1WebGrid:C1BoundColumn DataField="expired_period" >
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>

                   <C1WebGrid:C1BoundColumn DataField="vsl_type" >
                        <ItemStyle Width="15%" CssClass ="left"/>
                    </C1WebGrid:C1BoundColumn>

                   <C1WebGrid:C1BoundColumn DataField="use_flg">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>

                   <C1WebGrid:C1BoundColumn DataField="manager" >
                        <ItemStyle Width="8%"/>
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>

                   <C1WebGrid:C1BoundColumn DataField="temp_save_flg" Visible="false">
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
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 End -->

    
    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>