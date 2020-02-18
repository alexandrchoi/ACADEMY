<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="courseapplication_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.courseapplication_list" 
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">
    function fnValidateForm()
    {
        if(!isDateChk(document.getElementById('<%=txtCurr_From.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { "기간" }, new string[] { "Date" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(!isDateChk(document.getElementById('<%=txtCurr_To.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { "기간" }, new string[] { "Date" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
        return true;
    }
</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="개인수강신청" meta:resourcekey="lblMenuTitle" />

        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    <p>과정명을 클릭하여 개인 수강 신청</p>

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblCourse" class="title" runat="server" Text="과정" meta:resourcekey="lblCourse" />
                <asp:TextBox ID="txtCus_ID" runat="server" />
                <asp:TextBox ID="txtCus_NM" runat="server" />
                <a href="#" class="button-board-search" onclick="openPopWindow('/common/course_pop.aspx?opener_textbox_id=<%=txtCus_ID.ClientID%>&opener_textbox_nm=<%=txtCus_NM.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>', 'course_pop_win', '600', '650');">
                검색</a>
            </div>
            <div class="search-group date-pick-box">
                <asp:DropDownList ID="ddlDate" runat="server" />
                <asp:TextBox ID="txtCurr_From" runat="server" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtCurr_To" runat="server" CssClass="datepick" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseType" runat="server" Text="과정유형" meta:resourcekey="lblCourseType" />
                <asp:DropDownList ID="ddlCourseType" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnRetrieveResource" />

        </div>


        <div class="button-box right">
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="C1WebGrid1_ItemDataBound" DataKeyField="open_course_id" OnItemCreated="grd_ItemCreated">
                <Columns>
                    <C1WebGrid:C1BoundColumn DataField="rnum" HeaderText="순번">
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="교육유형">
                        <ItemTemplate>
                            <asp:Label ID="lblCourseType" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="12%" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="과정명">
                        <ItemTemplate>
                            <a id="res_sub1" href="courseapplication_detail.aspx?ropen_course_id=<%# DataBinder.Eval(Container.DataItem, "open_course_id")%>&rapproval_code=<%# DataBinder.Eval(Container.DataItem, "approval_code")%>&MenuCode=<%=Session["MENU_CODE"]%>"><%# DataBinder.Eval(Container.DataItem, "course_nm")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="" CssClass="left" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="course_seq" HeaderText="차수">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="apply_date" HeaderText="수강신청기간">
                        <ItemStyle Width="15%" />
                    </C1WebGrid:C1BoundColumn>                
                    <C1WebGrid:C1BoundColumn DataField="course_Date" HeaderText="교육기간">
                        <ItemStyle Width="15%" />
                    </C1WebGrid:C1BoundColumn>            
                    <C1WebGrid:C1BoundColumn  DataField="course_day" HeaderText="교육일수">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>                                    
                    <C1WebGrid:C1BoundColumn  DataField="approval_flg" HeaderText="상태">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>   
                    <C1WebGrid:C1BoundColumn Visible="false"  DataField="course_type" HeaderText="교육유형">
                        <ItemStyle Width="0%"  />
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