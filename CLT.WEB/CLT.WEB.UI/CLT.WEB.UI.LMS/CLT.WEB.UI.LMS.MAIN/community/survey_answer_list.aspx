<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="survey_answer_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.COMMUNITY.survey_answer_list" 
    Culture="auto" UICulture="auto" %>

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
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="설문조사" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <!-- 검색 -->
        <!--div class="search-box">
            <div class="search-group">
            </div>
        </div-->

        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnRetrieve" runat="server" Text="Search" CssClass="button-default blue" OnClick="button_OnClick" meta:resourcekey="btnRetrieveResource" />
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="grd_OnItemDataBound" DataKeyField="res_no" OnItemCreated="grd_ItemCreated">
                <Columns>
                    
                <C1WebGrid:C1BoundColumn DataField="res_date" HeaderText="설문기간">
                    <ItemStyle Width="19%" />
                </C1WebGrid:C1BoundColumn>                        
                <C1WebGrid:C1TemplateColumn HeaderText="설문제목">
                    <ItemTemplate>
                        <a id="res_sub1" href="/community/survey_answer_detail.aspx?rResNo=<%# DataBinder.Eval(Container.DataItem, "res_no")%>&rRes_sub=<%# HttpUtility.UrlEncode(DataBinder.Eval(Container.DataItem, "res_sub").ToString())%>&rRes_object=<%# HttpUtility.UrlEncode(DataBinder.Eval(Container.DataItem, "res_object").ToString())%>&rRes_date=<%# HttpUtility.UrlEncode(DataBinder.Eval(Container.DataItem, "res_date").ToString())%>&rAnswer_yn=N"><%# DataBinder.Eval(Container.DataItem, "res_sub")%></a>
                    </ItemTemplate>
                    <ItemStyle Width="27%" CssClass="left" />
                </C1WebGrid:C1TemplateColumn>
                <C1WebGrid:C1BoundColumn DataField="user_nm_kor" HeaderText="작성자">
                    <ItemStyle Width="10%" />
                </C1WebGrid:C1BoundColumn>      
                <C1WebGrid:C1BoundColumn DataField="res_object" Visible="false" HeaderText="설문목적">
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