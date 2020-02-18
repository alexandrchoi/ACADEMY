<%@ Page Language="C#" AutoEventWireup="true" Codebehind="content_mix.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.content_mix" Culture="auto" UICulture="auto" MasterPageFile="/MasterSub.Master" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">
    function fnValidateForm()
    {
        if(isEmpty(document.getElementById('<%=txtCourseID.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblCourseNM.Text }, new string[] { lblCourseNM.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

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
                <asp:Label ID="lblCourseNM" CssClass="title" runat="server" meta:resourcekey="lblCourseNM"/>
                <asp:TextBox ID="txtCourseNM" runat="server" ReadOnly="true"/>
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourseID" CssClass="title" runat="server" meta:resourcekey="lblCourseID"/>
                <input type="text" id="txtCourseID" runat="server" readonly="readonly" />
                <a href="#" class="button-board-search" onclick="openPopWindow('/common/course_pop.aspx?opener_textbox_id=<%=txtCourseID.ClientID %>&opener_textbox_nm=<%=txtCourseNM.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>', 'course_pop_win', '600', '650');"></a>
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnRetrieveResource" />

        </div>

        <!-- 버튼 -->
        <!--<div class="button-box right">
        </div>-->

        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" runat="server" AllowSorting="True"  AllowColSizing="True"  CssClass="grid_main" AutoGenerateColumns="false" OnItemCreated="C1WebGrid1_ItemCreated">
                <Columns>

                    <C1WebGrid:C1BoundColumn DataField="subject_seq">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="subject_nm">
                        <ItemStyle Width="30%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="contents_nm">
                        <ItemStyle Width="40%" CssClass ="left"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="contents_file_nm">
                        <ItemStyle Width="20%" />
                    </C1WebGrid:C1BoundColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>

    
    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>
