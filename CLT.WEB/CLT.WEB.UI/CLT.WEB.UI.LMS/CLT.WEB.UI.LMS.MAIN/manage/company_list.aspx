<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="company_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.MANAGE.company_list" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">

    function fnValidateForm()
    {
        if(!isDateChk(document.getElementById('<%=txtCreated_From.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { txtCreated.Text }, new string[] { txtCreated.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(!isDateChk(document.getElementById('<%=txtCreated_To.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { txtCreated.Text }, new string[] { txtCreated.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
        return true;
    }
    
    /*******************************************
    *  기능 :  날짜 체크, 날짜 컨트롤에서 사용 *
    *  parameter : obj                         *
    ********************************************/
    function ChkNum(obj)
    {
        alert(obj);
        if(event.keyCode == 8)
        {
            return;
        }

        var str = obj.value;

        // Only number input
        if (str.search( /[^0-9]/ ) != -1)
        {
            obj.value = str.substr(0, str.length - 1);
        }
    }
    
    /*******************************************
    *  기능 :  날짜 체크, 날짜 컨트롤에서 사용 *
    *  parameter : obj                         *
    ********************************************/
    function ChkNum1(obj)
    {
        var str = obj.value;
        alert
        // Only number input
        if (str.search( /[^0-9]/ ) != -1)
        {
                
            obj.value = str.substr(0, str.length - 1);
            return;
        }    
    }

    function GoCompanyForm()
    {	
        openPopWindow('/manage/company_edit.aspx?EDITMODE=NEW&MenuCode=<%=Session["MENU_CODE"]%>', 'company_edit', '1024', '721');
        return false;
    }
</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="회원사" meta:resourcekey="lblMenuTitle" />
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
                <asp:Label ID="lblStatus" runat="server" Text="승인여부" meta:resourcekey="lblStatus" />
                <asp:DropDownList ID="ddlStatus" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="txtCreated" runat="server" Text="가입기간" meta:resourcekey="txtCreated" />
                <asp:TextBox runat="server" ID="txtCreated_From" MaxLength="10" CssClass="datepick" />
                &nbsp;~&nbsp;
                <asp:TextBox runat="server" ID="txtCreated_To" MaxLength="10" CssClass="datepick" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="Search" CssClass="button-default blue" OnClick="button_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnRetrieveResource" />
        </div>

        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnNew" runat="server" Text="New" CssClass="button-default blue" OnClientClick="javascript:return GoCompanyForm();" meta:resourcekey="btnNewResource" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button-default" OnClick="button_Click" meta:resourcekey="btnDeleteResource" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" OnClick="button_Click" meta:resourcekey="btnExcelResource" />
            <asp:Button ID="btnApploval" runat="server" Text="Approval" CssClass="button-default" OnClick="button_Click" meta:resourcekey="btnApplovalResource" />
            <asp:Button ID="btnUseage" runat="server" Text="Usage" CssClass="button-default" OnClick="button_Click" meta:resourcekey="btnUseageResource" />
            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="button-default" OnClick="button_Click" meta:resourcekey="btnRejectResource" />
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" DataKeyField="company_id" OnItemCreated="grd_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <input type="checkbox" id="chkHeader" name="chkHeader" onclick="CheckAll(this, 'chkEdit');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" id="chkEdit" name="chkEdit" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="3%" />
                    </C1WebGrid:C1TemplateColumn >
                    <C1WebGrid:C1BoundColumn DataField="rnum" HeaderText="번호">
                        <ItemStyle Width="3%" />
                    </C1WebGrid:C1BoundColumn>                
                    <C1WebGrid:C1TemplateColumn HeaderText="회사명">
                        <ItemTemplate>
                            <a href="#" onclick="javascript:openPopWindow('/manage/company_edit.aspx?EDITMODE=EDIT&COMPANY_ID=<%# DataBinder.Eval(Container.DataItem, "company_id")%>&MenuCode=<%=Session["MENU_CODE"]%>','company_edit', '1024', '721');"><%# DataBinder.Eval(Container.DataItem, "company_nm")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="company_ceo" HeaderText="대표자명">
                        <ItemStyle Width="7%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="kindknm" HeaderText="회사규모">
                        <ItemStyle Width="7%" />
                    </C1WebGrid:C1BoundColumn>                
                    <C1WebGrid:C1BoundColumn DataField="user_nm" HeaderText="담당자명">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>                                
                    <C1WebGrid:C1BoundColumn DataField="tel_no" HeaderText="담당자연락처">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>                                                
                    <C1WebGrid:C1BoundColumn DataField="emp_cnt" HeaderText="근로자수">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>                                                
                    <C1WebGrid:C1BoundColumn DataField="company_dt" HeaderText="등록일">
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>                                                
                    <C1WebGrid:C1BoundColumn DataField="statusknm" HeaderText="승인">
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1BoundColumn> 
                    <C1WebGrid:C1BoundColumn Visible="false" DataField="companystatus" HeaderText="">
                        <ItemStyle Width="0%" />
                    </C1WebGrid:C1BoundColumn>
                                
                    <C1WebGrid:C1BoundColumn Visible="false" DataField="code" HeaderText="">
                        <ItemStyle Width="0%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn Visible="false" DataField="company_kind" HeaderText="">
                        <ItemStyle Width="0%" />
                    </C1WebGrid:C1BoundColumn>           
                    <C1WebGrid:C1BoundColumn Visible="false" DataField="zip_code" HeaderText="">
                        <ItemStyle Width="0%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn Visible="false" DataField="company_addr" HeaderText="">
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