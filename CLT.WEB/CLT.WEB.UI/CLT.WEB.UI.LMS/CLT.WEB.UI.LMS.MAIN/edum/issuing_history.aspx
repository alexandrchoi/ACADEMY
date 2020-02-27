﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="issuing_history.aspx.cs" Inherits="CLT.WEB.UI.LMS.EDUM.issuing_history" Culture="auto" UICulture="auto" MasterPageFile="/MasterSub.Master" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">

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
                <asp:Label ID="lblPeriod" CssClass="title" runat="server" meta:resourcekey="lblPeriod" />
                <asp:TextBox ID="txtSTART_DATE" runat="server" MaxLength="10" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtEND_DATE" runat="server" MaxLength="10" CssClass="datepick" />
            </div>

            <div class="search-group">
                <asp:Label ID="lblCourseType" CssClass="title" runat="server" meta:resourcekey="lblCourseType" />
                <asp:DropDownList ID="ddlCourseType" runat="server" />
            </div>
            
            <div class="search-group">
                <asp:Label ID="lblUserNMKor" CssClass="title" runat="server" meta:resourcekey="lblUserNMKor" />
                <asp:TextBox ID="txtUserNMKor" runat="server"  />
            </div>

            <div class="search-group">
                <asp:Label ID="lblCourseNM" CssClass="title" runat="server" meta:resourcekey="lblCourseNM" />
                <asp:TextBox ID="txtCourseNM" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />
        </div>

        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnExcel" runat="server" CssClass="button-default blue" OnClick="btnExcel_Click" Text="Excel" meta:resourcekey="btnExcelResource" />
        </div>

        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grdList" runat="server" AllowColSizing="True" AutoGenerateColumns="False" DataKeyField="KEYS" OnItemDataBound="grdList_ItemDataBound" OnItemCreated="grdList_ItemCreated">
                <Columns>

                    <C1WebGrid:C1TemplateColumn HeaderText="No.">
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1BoundColumn DataField="COURSE_TYPE_NM" HeaderText="교육유형">
                        <ItemStyle Width="5%"/>
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="COMPANY_NM" HeaderText="회사명">
                        <ItemStyle Width="10%"/>
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1TemplateColumn HeaderText="과정명">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlkCourseNM" runat="server"><%# DataBinder.Eval(Container.DataItem, "COURSE_NM")%></asp:HyperLink>
                        </ItemTemplate>
                        <ItemStyle CssClass="left" Width=""/>
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1BoundColumn DataField="CERTIFICATE_NO" HeaderText="증서번호">
                        <ItemStyle Width="7%"/>
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="USER_NM_KOR" HeaderText="성명">
                        <ItemStyle Width="7%"/>
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="USER_NM_ENG" HeaderText="영문성명">
                        <ItemStyle Width="10%"/>
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="BIRTH_DATE" HeaderText="생년월일">
                        <ItemStyle Width="7%"/>
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="COURSE_DATE"  HeaderText="교육기간">
                        <ItemStyle Width="10%"/>
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="ISSUE_DATE" HeaderText="발급일자">
                        <ItemStyle Width="5%"/>
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="REASON" HeaderText="발급사유">
                        <ItemStyle CssClass="left" Width="15%"/>
                    </C1WebGrid:C1BoundColumn>

                </Columns>

                <HeaderStyle />
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