<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="training.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.training" 
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">
    function OpenReport(strReportAddr)
    {
        var childWindow;
            
        childWindow = window.open(strReportAddr, "report", "menubar=0, status=0, toolbar=0, scrollbars=1, width=1024, height=870");
            
        childWindow.focus();
    }
</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    
<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="수료확인" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblCourseType" runat="server" meta:resourcekey="lblCourseType" />
                <asp:DropDownList ID="ddlCourseType" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblPass" runat="server" meta:resourcekey="lblPass" />
                <asp:DropDownList ID="ddlPass" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />
        </div>

        <!-- Data Table - List type -->
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grd" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="grd_ItemDataBound" OnItemCreated="grd_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1BoundColumn DataField="open_course_id" Visible="false">
                        <ItemStyle Width="0%" />                                    
                    </C1WebGrid:C1BoundColumn>     
           
                    <C1WebGrid:C1BoundColumn DataField="pass_flg_cd" Visible="false">
                        <ItemStyle Width="0%" />                                    
                    </C1WebGrid:C1BoundColumn>   
            
                    <C1WebGrid:C1TemplateColumn >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate> 
                        <ItemStyle Width="4%" />
                    </C1WebGrid:C1TemplateColumn> 
                                                                     
                    <C1WebGrid:C1BoundColumn DataField="course_nm">
                        <ItemStyle Width="25%" CssClass ="left" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="course_dt" >
                        <ItemStyle Width="20%" />
                    </C1WebGrid:C1BoundColumn>
               
                   <C1WebGrid:C1BoundColumn DataField="expired_period" >
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn> 
               
                   <C1WebGrid:C1BoundColumn DataField="educational_org" >
                        <ItemStyle Width="20%" />
                    </C1WebGrid:C1BoundColumn>  
               
                   <C1WebGrid:C1BoundColumn DataField="pass_flg" >
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>   
                
                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                           <asp:Button ID="btnPrint" CssClass="button-default" Text="Print" runat="server" OnClick ="grdbtn_Click"/>
                        </ItemTemplate>
                        <ItemStyle Width="15%" />
                    </C1WebGrid:C1TemplateColumn>

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