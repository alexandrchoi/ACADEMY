<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="user_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.MANAGE.user_list" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">
    function GoExcelForm()
    {	
        openPopWindow('/manage/user_excel.aspx?MenuCode=<%=Session["MENU_CODE"]%>', "ExcelForm", "1350", "850", "status=no");
        return false;
    }

    function GoUserForm()
    {	
        openPopWindow('/manage/user_edit.aspx?EDITMODE=NEW&MenuCode=<%=Session["MENU_CODE"]%>', 'user_edit', '1280', '821');
        return false;
    }
</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="개인회원" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblCompany" runat="server" Text="업체명" meta:resourcekey="lblCompany" />
                <asp:TextBox ID="txtCompany" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblDutyStep" runat="server" Text="직급" meta:resourcekey="lblDutyStep" />
                <asp:DropDownList ID="ddlDutyStep" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblUserGroup" runat="server" Text="사용자 그룹" meta:resourcekey="lblUserGroup" />
                <asp:DropDownList ID="ddlUserGroup" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblUserName" runat="server" Text="이름" meta:resourcekey="lblUserName" />
                <asp:TextBox ID="txtUserName" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblPersonal_No" runat="server" Text="주민번호" meta:resourcekey="lblPersonal_No" />
                <asp:TextBox ID="txtPersonal_No" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblStatus" runat="server" Text="상태" meta:resourcekey="lblStatus" />
                <asp:DropDownList ID="ddlStatus" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="Search" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />
        </div>

        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnNew" runat="server" Text="New" CssClass="button-default blue" OnClientClick="javascript:return GoUserForm();" meta:resourcekey="btnNewResource" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button-default" OnClick="btnDelete_Click" meta:resourcekey="btnDeleteResource" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" OnClick="btnExcel_Click" meta:resourcekey="btnExcelResource" />
            <asp:Button ID="btnApploval" runat="server" Text="Approval" CssClass="button-default" OnClick="btnApploval_Click" meta:resourcekey="btnApplovalResource" />
            <asp:Button ID="btnUseage" runat="server" Text="Usage" CssClass="button-default" OnClick="btnUseage_Click" meta:resourcekey="btnUseageResource" />
            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="button-default" OnClick="btnReject_Click" meta:resourcekey="btnRejectResource" />
            <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="button-default" OnClientClick="javascript:return GoExcelForm();" meta:resourcekey="btnUploadResource" />
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" DataKeyField="user_id" OnItemDataBound="grd_OnItemDataBound" OnItemCreated="grd_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <input type="checkbox" id="chkHeader" name="chkHeader" onclick="CheckAll(this, 'chkEdit');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" id="chkEdit" name="chkEdit" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn >
                
                    <C1WebGrid:C1BoundColumn DataField="user_id" HeaderText="ID">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>                                                
                    <C1WebGrid:C1TemplateColumn HeaderText="성명">
                        <ItemTemplate>
                            <a href="#" onclick="javascript:openPopWindow('/manage/user_edit.aspx?EDITMODE=EDIT&USER_ID=<%# DataBinder.Eval(Container.DataItem, "user_id")%>&MenuCode=<%=Session["MENU_CODE"]%>','user_edit', '1280', '821');"><%# DataBinder.Eval(Container.DataItem, "user_nm_kor")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="9%" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="personal_no" HeaderText="주민번호">
                        <ItemStyle Width="9%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="user_group" HeaderText="사용자그룹">
                        <ItemStyle Width="7%" />
                    </C1WebGrid:C1BoundColumn>                                                
                    <C1WebGrid:C1BoundColumn DataField="company_nm" HeaderText="회사명">
                        <ItemStyle Width="9%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="dept_nm" HeaderText="부서명">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>    
                    <C1WebGrid:C1BoundColumn DataField="dutystep" HeaderText="직급">
                        <ItemStyle Width="7%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="mobile_phone" HeaderText="휴대폰">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="email_id" HeaderText="이메일">
                        <ItemStyle Width="13%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn  DataField="edu_cnt" HeaderText="수강횟수">
                        <ItemStyle Width="5%"  />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn  DataField="ins_dt" HeaderText="등록일">
                        <ItemStyle Width="6%"  />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn  DataField="status" HeaderText="상태">
                        <ItemStyle Width="5%"  />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn Visible="false"  DataField="statusvalue" HeaderText="상태값">
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