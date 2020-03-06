<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="received_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.received_list" 
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">

</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="기업교육접수" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    <p>과정명 클릭시 기업 교육접수 할 수 있습니다.</p>

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblCourseDt" runat="server" meta:resourcekey="lblCourseDt" />
                <asp:TextBox ID="txtBeginDt" runat="server" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtEndDt" runat="server" CssClass="datepick" />
            </div>
            <div class="search-group date-pick-box">
                <asp:Label ID="lblCourseNm" runat="server" meta:resourcekey="lblCourseNm" />
                <asp:TextBox ID="txtCourseNm" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseType" runat="server" meta:resourcekey="lblCourseType" />
                <asp:DropDownList ID="ddlCourseType" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />
        </div>

        <div class="button-box right">
            <input type="button" name="btnDnConsignment" value="사업주위탁훈련계약서" onclick="location.href='/file/download/consignment_contract.xlsx';" id="btnDnConsignment" class="button-default" />
            <input type="button" name="btnDnConsortium" value="컨소시엄 수강신청서" onclick="location.href='/file/download/consortium_apply.xlsx';" id="btnDnConsortium" class="button-default" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" OnClick="btnExcel_Click" meta:resourcekey="btnExcelResource" /> 
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="C1WebGrid1_ItemDataBound" DataKeyField="open_course_id" OnItemCreated="grd_ItemCreated">
                <Columns>

                    <C1WebGrid:C1BoundColumn DataField="open_course_id" Visible="false">
                        <ItemStyle Width="0%" />                                    
                    </C1WebGrid:C1BoundColumn>     
           
                    <C1WebGrid:C1BoundColumn DataField="" Visible="false">
                        <ItemStyle Width="0%" />                                    
                    </C1WebGrid:C1BoundColumn>   
            
                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate> 
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn> 
                
                    <C1WebGrid:C1TemplateColumn HeaderText="교육유형">
                        <ItemTemplate>
                            <asp:Label ID="lblCourseType" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="12%" />
                    </C1WebGrid:C1TemplateColumn>
                              
                   <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate> 
                        <a href="#" onclick="javascript:openPopWindow('/application/received_edit.aspx?open_course_id=<%# DataBinder.Eval(Container.DataItem, "OPEN_COURSE_ID")%>&MenuCode=<%=Session["MENU_CODE"]%>&ManTotCnt=<%# DataBinder.Eval(Container.DataItem, "CLASS_MAN_COUNT")%>','received_edit_win', '800', '740');"><%# DataBinder.Eval(Container.DataItem, "COURSE_NM")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="" CssClass="left"/>
                    </C1WebGrid:C1TemplateColumn>
               
                   <C1WebGrid:C1BoundColumn DataField="course_seq" >
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>  
                
                    <C1WebGrid:C1BoundColumn DataField="apply_dt" >
                        <ItemStyle Width="15%" />
                    </C1WebGrid:C1BoundColumn>
               
                   <C1WebGrid:C1BoundColumn DataField="course_dt" >
                        <ItemStyle Width="15%" />
                    </C1WebGrid:C1BoundColumn> 
               
                   <C1WebGrid:C1BoundColumn DataField="course_day" >
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn> 
               
                   <C1WebGrid:C1BoundColumn DataField="class_man_count" >
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>  

                </Columns>
                <HeaderStyle />
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