﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edu_cmp_n_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.EDUM.edu_cmp_n_list" Culture="auto" UICulture="auto" MasterPageFile="/MasterSub.Master" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%--<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>--%>
<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
<script type="text/javascript" language="javascript">
    function fnValidateForm()
    {  
        if(!compareDate(document.getElementById('<%=txtSTART_DATE.ClientID %>'),document.getElementById('<%=txtEND_DATE.ClientID %>'))) return false;

        return true;
    }
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
                <asp:Label ID="lblPeriod" runat="server" CssClass="title" meta:resourcekey="lblPeriod"></asp:Label>
                <asp:TextBox ID="txtSTART_DATE" runat="server" MaxLength="10" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtEND_DATE" runat="server" MaxLength="10" CssClass="datepick"></asp:TextBox>
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseType" runat="server" CssClass="title" meta:resourcekey="lblCourseType"></asp:Label>
                <asp:DropDownList ID="ddlCourseType" runat="server" />
            </div>
            
            <div class="search-group">
                <asp:Label ID="lblNonApp" runat="server" CssClass="title" meta:resourcekey="lblNonApp"></asp:Label>
                <asp:DropDownList ID="ddlNonApp" runat="server" />
            </div>

            <div class="search-group">
                <asp:Label ID="lblNonPass" runat="server" CssClass="title" meta:resourcekey="lblNonPass"></asp:Label>
                <asp:DropDownList ID="ddlNonPass" runat="server" />
            </div>

            <div class="search-group">
                <asp:Label ID="lblCourseNM" runat="server" CssClass="title" meta:resourcekey="lblCourseNM"></asp:Label>
                <asp:TextBox ID="txtCourseNM" runat="server"></asp:TextBox>
            </div>

            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue"
                OnClick="btnRetrieve_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnRetrieveResource" />
            
        </div>
    
        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnExcel" runat="server" CssClass="button-default blue" OnClick="btnExcel_Click" Text="Excel" meta:resourcekey="btnExcelResource" />
        </div>



    <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grdList" runat="server" AllowColSizing="True" CssClass="grid_main" AutoGenerateColumns="False"
                                OnItemDataBound="grdList_ItemDataBound" OnItemCommand="grdList_ItemCommand" OnItemCreated="grdList_ItemCreated">
                <Columns>
                    <C1WebGrid:C1TemplateColumn HeaderText="No.">
                        <ItemStyle Width="3%"/>
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="user_id" HeaderText="사번">
                        <ItemStyle Width="5%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="step_name" HeaderText="직급">
                        <ItemStyle Width="5%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="user_nm_kor" HeaderText="성명">
                        <ItemStyle Width="9%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="course_nm" HeaderText="과정명">
                        <ItemStyle HorizontalAlign="Left" CssClass="grid_list_leftaling" Width="25%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="course_dt" HeaderText="교육기간">
                        <ItemStyle Width="11%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="non_approval_nm" HeaderText="교육불가사유">
                        <ItemStyle Width="11%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="non_approval_remark" HeaderText="불가사유">
                        <ItemStyle Width="9%" CssClass="left"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="non_pass_nm" HeaderText="미이수사유">
                        <ItemStyle Width="9%"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="non_pass_remark" HeaderText="비고">
                        <ItemStyle Width="9%"/>
                    </C1WebGrid:C1BoundColumn>
                </Columns>

                <HeaderStyle Font-Bold="true" />
                <ItemStyle  Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>

        <div class="gm-paging">
            <CLTWebControl:PageNavigator ID="PageNavigator1" runat="server" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" />
        </div>      
    </section>
</div>
</asp:Content>