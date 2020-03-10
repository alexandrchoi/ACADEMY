<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="approval_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.approval_list" 
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">
    function fnValidateForm()
    {  
        if(!compareDate(document.getElementById('<%=txtSTART_DATE.ClientID %>'),document.getElementById('<%=txtEND_DATE.ClientID %>'))) return false;

        return true;
    }
    function GoAppForm(rSearch)
    {	
        //open_course_id
        openPopWindow('/application/approval_user.aspx?search='+rSearch+'&MenuCode=<%=Session["MENU_CODE"]%>', "AppForm", screen.width, "800", "status=yes");
	    return false;
    }
</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="수강신청/승인처리" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    <p>관리자가 개인 수강신청/기업 교육접수 및 승인 처리 할 수 있습니다.</p>

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblPeriod" runat="server" meta:resourcekey="lblPeriod" />
                <asp:TextBox ID="txtSTART_DATE" runat="server" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtEND_DATE" runat="server" CssClass="datepick" />
            </div>
            <div class="search-group date-pick-box">
                <asp:Label ID="lblCourseNM" runat="server" meta:resourcekey="lblCourseNM" />
                <asp:TextBox ID="txtCourseNM" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseType" runat="server" meta:resourcekey="lblCourseType" />
                <asp:DropDownList ID="ddlCourseType" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnRetrieveResource" />
        </div>

        <div class="button-box right">
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" OnClick="btnExcel_Click" meta:resourcekey="btnExcelResource" /> 
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grdList" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="grdList_ItemDataBound" DataKeyField="KEYS" OnItemCreated="grdList_ItemCreated" OnItemCommand="grdList_ItemCommand">
                <Columns>
                    
                    <C1WebGrid:C1TemplateColumn>
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_all_sel" name="chk_all_sel" onclick="CheckAll(this, 'chk_sel');" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" id="chk_sel" name="chk_sel" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="3%"/>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="No.">
                        <ItemStyle VerticalAlign="Middle" Width="5%" />
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    
                    <C1WebGrid:C1TemplateColumn HeaderText="교육구분">
                        <ItemTemplate>
                            <asp:Label ID="lblCourseType" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="12%" />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1TemplateColumn HeaderText="과정명">
                        <ItemStyle CssClass="left" Width="30%"/>
                        <ItemTemplate>
                            <asp:HyperLink ID="hlkCourseNM" runat="server"><%# DataBinder.Eval(Container.DataItem, "COURSE_NM")%></asp:HyperLink>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="COURSE_SEQ" HeaderText="차수">
                        <ItemStyle Width="5%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="교육기간">
                        <ItemStyle Width="12%"/>
                        <ItemTemplate>
                            <asp:Label ID="lblCourseDate" runat="server"><%# DataBinder.Eval(Container.DataItem, "COURSE_DATE")%></asp:Label>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="CLASS_MAN_COUNT" HeaderText="정원">
                        <ItemStyle Width="6%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="CNT_APP" HeaderText="신청">
                        <ItemStyle Width="6%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="CNT_RCV" HeaderText="접수">
                        <ItemStyle Width="6%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="CNT_APPR" HeaderText="승인">
                        <ItemStyle Width="6%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="CNT_NONAPPR" HeaderText="미승인">
                        <ItemStyle Width="6%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="CNT_IN" HeaderText="입과">
                        <ItemStyle Width="6%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="실시신고" Visible="false">
                        <ItemStyle Width="8%"/>
                        <ItemTemplate>
                            <asp:Button ID="btn_down" runat="server" Text="Down" CssClass="button-underline" CommandName="Excel"/>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>

                </Columns>
                <HeaderStyle />
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