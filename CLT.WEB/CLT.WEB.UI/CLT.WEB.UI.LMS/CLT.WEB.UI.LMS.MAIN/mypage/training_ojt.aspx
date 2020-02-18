<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="training_ojt.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.training_ojt" 
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">

</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    
<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="OJT 결과 조회" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblOjtNm" runat="server" meta:resourcekey="lblOjtNm" />
                <asp:TextBox ID="txtOjtNm" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblOjtDt" runat="server" meta:resourcekey="lblOjtDt" />
                <asp:TextBox ID="txtOjtBeginDt" runat="server" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtOjtEndDt" runat="server" CssClass="datepick" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />
        </div>

        <!-- Data Table - List type -->
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grd" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemCreated="grd_ItemCreated">
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
                                                                     
                    <C1WebGrid:C1BoundColumn DataField="course_nm">
                        <ItemStyle Width="25%" CssClass ="left"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="ojt_dt" >
                        <ItemStyle Width="20%" />
                    </C1WebGrid:C1BoundColumn>
               
                   <C1WebGrid:C1BoundColumn DataField="ojt_learning_time" >
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn> 
               
                   <C1WebGrid:C1BoundColumn DataField="ojt_cd" >
                        <ItemStyle Width="20%" />
                    </C1WebGrid:C1BoundColumn>  
               
                   <C1WebGrid:C1BoundColumn DataField="pass_flg" >
                        <ItemStyle Width="10%" />
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