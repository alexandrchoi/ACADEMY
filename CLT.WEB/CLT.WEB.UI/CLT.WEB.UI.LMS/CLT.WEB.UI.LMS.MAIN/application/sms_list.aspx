<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="sms_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.sms_list" 
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">
    function fnValidateForm()
    {
        if(!isDateChk(document.getElementById('<%=txtSMS_From.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { txtCreated.Text }, new string[] { txtCreated.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(!isDateChk(document.getElementById('<%=txtSMS_To.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { txtCreated.Text }, new string[] { txtCreated.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
        return true;
    }
</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="SMS 전송결과/예약조회" meta:resourcekey="lblMenuTitle" />

        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblSentType" runat="server" CssClass="title" Text="조회할 결과 선택" meta:resourcekey="lblSentType" />
                
                <label class="radio-box">
                    <asp:RadioButton ID="rbSent" runat="server" GroupName="SMSSent" Checked="true" Text="전송결과 확인" AutoPostBack="false" meta:resourcekey="rbSent" />
                    <span class="radiomark"></span>
                </label>
                <label class="radio-box">
                    <asp:RadioButton ID="rbBooking" runat="server" GroupName="SMSSent" Checked="false" Text="예약메시지 확인" AutoPostBack="false" meta:resourcekey="rbBooking" />
                    <span class="radiomark"></span>
                </label>
            </div>
            <div class="search-group date-pick-box">
                <asp:Label ID="txtCreated" runat="server" Text="조회 기간" meta:resourcekey="txtCreated" />
                <asp:TextBox ID="txtSMS_From" runat="server" MaxLength="10" CssClass="datepick" />
                <span class="gm-text2">~</span>
                <asp:TextBox ID="txtSMS_To" runat="server" MaxLength="10" CssClass="datepick" />
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnRetrieve_Click" OnClientClick="return fnValidateForm();;" meta:resourcekey="btnRetrieveResource" />
        </div>

        <div class="button-box right">
            <asp:Button ID="btnNew" runat="server" Text="New" CssClass="button-default blue" OnClick="btnNew_OnClick" meta:resourcekey="btnNewResource" />
        </div>

        <!-- Data Table - List type -->
        
        <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" DataKeyField="sms_group_no" OnItemCreated="grd_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1TemplateColumn HeaderText="제목">
                        <ItemTemplate>
                            <a id="res_sub1" href="#" onclick="javascript:openPopWindow('/application/sms_detail.aspx?rSMSGroupNo=<%# DataBinder.Eval(Container.DataItem, "sms_group_no")%>','sms_detail', '650', '750');"><%# DataBinder.Eval(Container.DataItem, "sent_sms_title")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="40%" CssClass="left" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="sent_dt" HeaderText="전송일시">
                        <ItemStyle Width="30%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="totalcount" HeaderText="수신처">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="sentcount" HeaderText="성공">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn  DataField="failcount" HeaderText="실패">
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