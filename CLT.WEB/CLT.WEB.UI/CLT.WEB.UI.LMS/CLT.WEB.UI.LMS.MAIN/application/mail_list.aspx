<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="mail_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.mail_list" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">
    function fnValidateForm()
    {
        if(!isDateChk(document.getElementById('<%=txtMail_From.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblSendPeriod.Text }, new string[] { lblSendPeriod.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(!isDateChk(document.getElementById('<%=txtMail_To.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblSendPeriod.Text }, new string[] { lblSendPeriod.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
        return true;
    }
</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="보낸편지함" meta:resourcekey="lblMenuTitle" />

        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group date-pick-box">
                <asp:Label ID="lblSendPeriod" runat="server" Text="발송기간" meta:resourcekey="lblSendPeriod" />
                <asp:TextBox ID="txtMail_From" runat="server" MaxLength="10" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtMail_To" runat="server" MaxLength="10" CssClass="datepick" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />
        </div>

        <div class="button-box right">
            <asp:Button ID="btnNew" runat="server" Text="New" CssClass="button-default blue" OnClick="btnNew_OnClick" meta:resourcekey="btnNewResource" />
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="C1WebGrid1_ItemDataBound" DataKeyField="seq" OnItemCreated="grd_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1BoundColumn DataField="rnum" HeaderText="순번">
                        <ItemStyle Width="7%" />
                    </C1WebGrid:C1BoundColumn>           
                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <asp:Image ID="imgHeader" runat="server" CssClass="icon-attach" ImageUrl="/asset/Images/clip.png" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Image ID="imgItems" runat="server"/>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn> 
                    <C1WebGrid:C1BoundColumn DataField="user_id" HeaderText="보낸사람">
                        <ItemStyle Width="20%" />
                    </C1WebGrid:C1BoundColumn>                
                    <C1WebGrid:C1TemplateColumn HeaderText="제목">
                        <ItemTemplate>
                            <a id="mail_sub" href="/application/mail_detail.aspx?rseq=<%# DataBinder.Eval(Container.DataItem, "seq")%>&MenuCode=<%=Session["MENU_CODE"]%>"><%# DataBinder.Eval(Container.DataItem, "mail_sub")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="45%" CssClass="left" />
                    </C1WebGrid:C1TemplateColumn>     
                    <C1WebGrid:C1BoundColumn DataField="sent_dt"  HeaderText="발송일시">
                        <ItemStyle Width="25%" />
                    </C1WebGrid:C1BoundColumn>                
                    <C1WebGrid:C1BoundColumn DataField="attcount" Visible="false" HeaderText="첨부파일 갯수">
                        <ItemStyle Width="25%"  />
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