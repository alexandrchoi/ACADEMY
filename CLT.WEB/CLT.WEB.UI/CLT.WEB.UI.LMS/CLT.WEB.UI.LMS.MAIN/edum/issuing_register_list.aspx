<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="issuing_register_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.EDUM.issuing_register_list" Culture="auto" UICulture="auto" MasterPageFile="/MasterSub.Master" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
<script type="text/javascript" language="javascript">
function fnValidateForm()
{  
    if(isEmpty(document.getElementById('<%=txtUserId.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblUserId.Text }, new string[] { lblUserId.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

    return true;
}
function GoAppForm(rSearch)
{	
    //open_course_id
    if(fnValidateForm())
    {
        openPopWindow('/edum/issuing_register.aspx?search=' + rSearch + '&MenuCode=<%=Session["MENU_CODE"]%>', "EduTraningForm", "780", "400", "status=yes");
	}
	return false;
}
function GoUserForm()
{	
    //Inquiry^VessleType^ShipCode^Rank
    openPopWindow('/appr/competency_user_list.aspx?search=&bind_control=<%=txtUserId.ClientID %>^<%=txtUserNMKor.ClientID %>^<%=ddlDutyStep.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>', "UserListForm", "820", "860", "status=no");
    return false;
}
</script> 
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="Label1" runat="server" Text="" meta:resourcekey="lblMenuTitle" />

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
                <asp:Label ID="lblUserNMKor" CssClass="title" runat="server" meta:resourcekey="lblUserNMKor" />
                <asp:TextBox ID="txtUserNMKor" runat="server" /> 
            </div>
            <div class="search-group">
                <asp:Label ID="lblUserId" CssClass="title" runat="server" meta:resourcekey="lblUserId" />
                <asp:TextBox ID="txtUserId" runat="server" />
                <asp:Button ID = "btnSearch" runat ="server" Text="Search" CssClass="button-board-search" OnClientClick="javascript:return GoUserForm();" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblDutyStep" CssClass="title" runat="server" meta:resourcekey="lblDutyStep" />
                <asp:DropDownList ID="ddlDutyStep" runat="server" />
            </div>
            <!--div class="search-group">
                <asp:Label ID="lblPersonalNo" CssClass="title" runat="server" meta:resourcekey="lblPersonalNo" Visible="false" />
                <asp:TextBox ID="txtPersonalNo" runat="server" Visible="false" />
            </div-->
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue"
                OnClick="btnRetrieve_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnRetrieveResource" />
        </div>

        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnNew" runat="server" CssClass="button-default blue" Text="New" meta:resourcekey="btnNewResource" OnClientClick="return GoAppForm(document.getElementById('ctl00_ContentPlaceHolderMain_txtUserId').value+'^'+'^'+document.getElementById('ctl00_ContentPlaceHolderMain_txtUserNMKor').value);"/>
            <asp:Button ID="btnDel" runat="server" CssClass="button-default" OnClick="btnDel_Click" Text="Delete" meta:resourcekey="btnDelResource" />
            <asp:Button ID="btnExcel" runat="server" CssClass="button-default" OnClick="btnExcel_Click" Text="Excel" meta:resourcekey="btnExcelResource" />
        </div>


        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grdList" runat="server" AllowColSizing="True" AutoGenerateColumns="False" 
               DataKeyField="KEYS" OnItemCreated="grdList_ItemCreated" OnItemDataBound="grdList_ItemDataBound">
                <Columns>

                    <C1WebGrid:C1TemplateColumn>
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_all_sel" name="chk_all_sel" onclick="CheckAll(this, 'chk_sel');" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" id="chk_sel" name="chk_sel" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="20px"/>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="No.">
                        <ItemStyle Width="80px" />
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="과정명">
                        <ItemStyle CssClass="left" />
                        <ItemTemplate>
                            <asp:HyperLink ID="hlkCourseNM" runat="server"></asp:HyperLink>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="교육기관">
                        <ItemStyle CssClass="left" Width="200px"/>
                        <ItemTemplate>
                            <asp:Label ID="lblLearningInstitution" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LEARNING_INSTITUTION")%>'></asp:Label>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="COURSE_BEGIN_DT" HeaderText="교육시작일">
                        <ItemStyle  Width="150px"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="COURSE_END_DT" HeaderText="교육종료일">
                        <ItemStyle Width="150px"/>
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
