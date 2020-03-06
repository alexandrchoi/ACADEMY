<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="select_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.select_list" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">
        function fnValidateForm()
        {  
        if(!compareDate(document.getElementById('<%=txtSTART_DATE.ClientID %>'),document.getElementById('<%=txtEND_DATE.ClientID %>'))) return false;
        
        return true;
        }
        function GoAppForm(rSearch, gubun)
        {	
            //open_course_id
            openPopWindow('/application/select_user_list.aspx?search='+rSearch+'&MenuCode=<%=Session["MENU_CODE"]%>', "UserList", "1250", "860", "status=yes");
	        return false;
        }
        function GoConfirmForm(rSearch, gubun)
        {	
            //open_course_id
            openPopWindow('/application/select_user_result_list.aspx?search='+rSearch+'&MenuCode=<%=Session["MENU_CODE"]%>', "UserResultList", "1000", "860", "status=yes");
	        return false;
        }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    
<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="교육대상자 선발" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    <!--<p></p>-->


    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblPeriod" runat="server" CssClass="title" meta:resourcekey="lblPeriod" />
                <asp:TextBox ID="txtSTART_DATE" runat="server" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtEND_DATE" runat="server" CssClass="datepick" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseType" runat="server" CssClass="title" meta:resourcekey="lblCourseType" />
                <asp:DropDownList ID="ddlCourseType" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseLang" runat="server" CssClass="title" Text="" meta:resourcekey="lblCourseLang" />
                <asp:DropDownList ID="ddlCourseLang" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseNM" runat="server" CssClass="title" Text="" meta:resourcekey="lblCourseNM" />
                <asp:TextBox ID="txtCourseNM" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat ="server" Text="Search" CssClass="button-default blue" OnClick = "btnRetrieve_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnRetrieveResource" />
        </div>
        
        <!--div class="button-box right">
        </div-->

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type mt10">
            <C1WebGrid:C1WebGrid ID="grdList" runat="server" AllowSorting="True" AllowColSizing="True" AutoGenerateColumns="False" OnItemDataBound="grdList_ItemDataBound" DataKeyField="COURSE_ID" OnItemCreated="grdList_ItemCreated" HorizontalAlign="Center" Width="100%">
                <Columns>
                    
                    <C1WebGrid:C1TemplateColumn HeaderText="No.">
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="과정명">
                        <ItemStyle CssClass="left" Width="25%" />
                        <ItemTemplate>
                            <asp:HyperLink ID="hlkCourseNM" runat="server"><%# DataBinder.Eval(Container.DataItem, "COURSE_NM")%></asp:HyperLink>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="COURSE_SEQ" HeaderText="차수">
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="교육년월">
                        <ItemStyle Width="14%" Wrap="false" />
                        <ItemTemplate>
                            <asp:Label ID="lblCourseDate" runat="server"><%# DataBinder.Eval(Container.DataItem, "COURSE_DATE")%></asp:Label>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="COURSE_DAY" HeaderText="일수">
                        <ItemStyle Width="5%"/>
                    </C1WebGrid:C1BoundColumn>
                    
                    <C1WebGrid:C1TemplateColumn HeaderText="확정인원">
                        <ItemStyle Width="7%" />
                        <ItemTemplate>
                            <asp:HyperLink ID="hlkMAN" runat="server"><%# DataBinder.Eval(Container.DataItem, "MAN")%></asp:HyperLink>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1BoundColumn DataField="expired_period" HeaderText="유효기간(년)">
                        <ItemStyle Width="7%"/>
                        <HeaderStyle Wrap="false" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="대상직급" >
                        <ItemStyle CssClass="left word-break" Wrap="true" Width="32%" />
                        <ItemTemplate>
                            <asp:Label ID="lblEssStepName" runat="server" />
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle  Wrap="true"  />
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
