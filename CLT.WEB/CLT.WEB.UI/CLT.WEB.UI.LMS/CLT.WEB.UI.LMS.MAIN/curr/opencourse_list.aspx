<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="opencourse_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.opencourse_list" Culture="auto" UICulture="auto" MasterPageFile="/MasterSub.Master" %>

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
                <asp:Label ID="lblCoursePeriod" CssClass="title" runat="server" meta:resourcekey="lblCoursePeriod" />
                <asp:TextBox ID="txtBeginDt" runat="server" MaxLength="10" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtEndDt" runat="server" MaxLength="10" CssClass="datepick" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseType" CssClass="title" runat="server" meta:resourcekey="lblCourseType" />
                <asp:DropDownList ID="ddlCourseType" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseNm" CssClass="title" runat="server" meta:resourcekey="lblCourseNm" />
                <asp:TextBox ID="txtCourseNM" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />

        </div>

        <!-- 버튼 -->
        <div class="button-box right">
            <!-- 개발자 수정 영역 1 - 버튼 추가 영역 Start -->
            <input type="button" value="New" class="button-default blue" onclick="javascript:openPopWindow('/curr/opencourse_edit.aspx?open_course_id=NEW&MenuCode=<%=Session["MENU_CODE"]%>','opencourse_edit', '1024', '800');" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" OnClick="btnExcel_Click" />
            <!-- 개발자 수정 영역 1 - 버튼 추가 영역 End -->
        </div>

        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grd" runat="server" AllowSorting="True" GridLines="None" AllowColSizing="True" AutoGenerateColumns="false" OnItemDataBound="C1WebGrid1_ItemDataBound" OnItemCreated="grd_ItemCreated">
                <Columns>

                    <C1WebGrid:C1TemplateColumn >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                        <ItemStyle Width="4%" />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1BoundColumn DataField="course_year">
                        <ItemStyle Width="4%"/>
                    </C1WebGrid:C1BoundColumn>
                    
                    <C1WebGrid:C1TemplateColumn HeaderText="교육구분">
                        <ItemTemplate>
                            <asp:Label ID="lblCourseType" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="12%" />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                        <a href="#" onclick="javascript:openPopWindow('/curr/opencourse_edit.aspx?open_course_id=<%# DataBinder.Eval(Container.DataItem, "OPEN_COURSE_ID")%>&MenuCode=<%=Session["MENU_CODE"]%>','opencourse_edit_win', '1024', '800');"><%# DataBinder.Eval(Container.DataItem, "COURSE_NM")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="13%" CssClass ="left" />
                    </C1WebGrid:C1TemplateColumn>

                   <C1WebGrid:C1BoundColumn DataField="course_seq" >
                        <ItemStyle Width="8%"/>
                    </C1WebGrid:C1BoundColumn>
                   <C1WebGrid:C1BoundColumn DataField="course_apply_date" >
                        <ItemStyle Width="17%"/>
                    </C1WebGrid:C1BoundColumn>
                   <C1WebGrid:C1BoundColumn DataField="course_date" >
                        <ItemStyle Width="17%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                        <ItemStyle Width="7%"/>
                    </C1WebGrid:C1BoundColumn>
                   <C1WebGrid:C1BoundColumn DataField="use_flg" >
                        <ItemStyle Width="7%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="manager" >
                        <ItemStyle Width="9%"/>
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