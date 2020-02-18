<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="agreement_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.agreement_list" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">
       function OpenReport(strReportAddr)
        {
            var childWindow;
            
            childWindow = window.open(strReportAddr, "report", "menubar=0, status=0, toolbar=0, scrollbars=1, width=800, height=600");
            
            childWindow.focus();
        }
    </script>
</asp:Content>



<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="위탁훈련 계약서" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack()"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblCourseNm" CssClass="title" runat="server" meta:resourcekey="lblCourseNm" />
                <asp:TextBox ID="txtCourseNm" runat="server" />
            </div>
            <div class="search-group date-pick-box">
                <asp:Label ID="lblCourseDt" CssClass="title" runat="server" meta:resourcekey="lblCourseDt" />
                <asp:TextBox ID="txtBeginDt" runat="server" MaxLength="10" CssClass="datepick w180" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtEndDt" runat="server" MaxLength="10" CssClass="datepick w180" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="Retrieve" CssClass="button-default blue" OnClick="btnRetrieve_Click" />
        </div>

        <div class="button-box right">
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
    
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grd" runat="server" AutoGenerateColumns="false" OnItemCreated="grd_ItemCreated">
                <Columns>
            
                    <C1WebGrid:C1BoundColumn DataField="open_course_id" Visible="false">
                        <ItemStyle Width="0%" />                                    
                    </C1WebGrid:C1BoundColumn>     
           
                    <C1WebGrid:C1BoundColumn DataField="" Visible="false">
                        <ItemStyle Width="0%" />                                    
                    </C1WebGrid:C1BoundColumn>   
            
                    <C1WebGrid:C1TemplateColumn >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate> 
                        <ItemStyle Width="4%" />
                    </C1WebGrid:C1TemplateColumn> 
                
                    <C1WebGrid:C1BoundColumn DataField="course_nm" >
                        <ItemStyle Width="40%" />
                    </C1WebGrid:C1BoundColumn>                               
               
                    <C1WebGrid:C1BoundColumn DataField="course_dt" >
                        <ItemStyle Width="30%" />
                    </C1WebGrid:C1BoundColumn> 
               
                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <asp:Button ID="btn" Text ="Print" CssClass="button-default blue" runat = "server" OnClick ="grdbtn_Click"/>
                        </ItemTemplate>
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1TemplateColumn>   
               
                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <asp:Button ID="btn2" Text ="Print" CssClass="button-default blue" runat = "server" OnClick ="grdbtn2_Click"/>
                        </ItemTemplate>
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1TemplateColumn>    
                                              
                </Columns>
                <HeaderStyle />
                <ItemStyle  />
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