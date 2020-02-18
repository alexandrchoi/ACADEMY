<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="competency_appr.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPR.competency_appr" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">
    function GoAppForm(rSearch, gubun)
    {	
        //Inquiry^VessleType^ShipCode^Rank^app_keys^user_id^user_nm_kor
        openPopWindow('/appr/competency_detail.aspx?search='+rSearch+'&MenuCode=<%=Session["MENU_CODE"]%>', "AppForm", "1024", "780", "status=yes");
	    return false;
    }
</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="역량평가 결과조회" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    
    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblInquiry" runat="server" Text="" meta:resourcekey="lblInquiry" />
                <asp:DropDownList ID="ddlInquiry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlInquiry_SelectedIndexChanged" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblRank" runat="server" Text="" meta:resourcekey="lblRank" />
                <asp:DropDownList ID="ddlAppDutyStep" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblPeriod" runat="server" Text="" meta:resourcekey="lblPeriod" />
                <asp:TextBox ID="txtSTART_DATE" runat="server" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtEND_DATE" runat="server" CssClass="datepick" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblVessleType" runat="server" Text="" meta:resourcekey="lblVessleType" />
                <asp:DropDownList ID="ddlVslType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlVslType_SelectedIndexChanged" />
                <asp:DropDownList ID="ddlVslTypeC" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblShip" runat="server" Text="" meta:resourcekey="lblShip" />
                <asp:DropDownList ID="ddlShipCode" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblRePayYN" runat="server" Text="" meta:resourcekey="lblRePayYN" />
                <asp:DropDownList ID="ddlRePayYN" runat="server" Width="211px">
                    <asp:ListItem Selected="True">*</asp:ListItem>
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="Search" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />
        </div>

        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" meta:resourcekey="btnExcelResource" OnClick="btnExcel_Click" />
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grdList" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" DataKeyField="app_keys" OnItemDataBound="grdList_ItemDataBound" OnItemCreated="grdList_ItemCreated">
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
                        <ItemStyle Width="30px" />
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="app_dt" HeaderText="평가일자">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="vsl_cd" HeaderText="선박명">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="on_dt" HeaderText="승선일자">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="사번">
                        <ItemStyle />
                        <ItemTemplate>
                            <asp:HyperLink ID="hlkUserId" runat="server"><%# DataBinder.Eval(Container.DataItem, "user_id")%></asp:HyperLink>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="user_nm_kor" HeaderText="성명">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="duty_work_ename" HeaderText="직책">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="tot_score" HeaderText="총점수">
                        <ItemStyle CssClass="right"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="re_cnt" HeaderText="C,D항목수">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="app_nm" HeaderText="평가자 성명">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="app_duty_nm" HeaderText="평가자 직급">
                        <ItemStyle />
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