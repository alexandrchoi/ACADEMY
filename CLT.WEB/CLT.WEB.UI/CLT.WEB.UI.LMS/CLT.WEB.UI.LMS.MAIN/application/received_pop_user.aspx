<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="received_pop_user.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.received_pop_user" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">
        function setSelectUser(user_id)
		{
		    hidUser = document.getElementById("<%= HidUser.ClientID %>");			
		    hidUser.value = user_id; 
		    <%= Page.GetPostBackEventReference(LnkBtnUser) %>;
	    }
	    
	    function OK()
		{		
			var datas = Array();
		
			datas[0] = "<%= iarrUser %>"; // 형태( 각각의 Row 콤마(凸) 로 구분)	
			opener.setUserAdd(datas);
			opener.focus();
			self.close();
		}
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <!-- 선택된 user -->
    <asp:HiddenField runat ="server" ID="HidUser" />
    <asp:LinkButton ID="LnkBtnUser" runat = "server" OnClick="LnkBtnUser_Click" /> 
    

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="교육접수" meta:resourcekey="lblMenuTitle" /></h3>
        <p><asp:Label ID="lbl0" runat="server" meta:resourcekey="lblGuide0" Text="사업주 훈련비 지원시 반드시 '훈련생 구분, 고용보험 취득일' 입력바랍니다." />
           <asp:Label ID="lbl1" runat="server" meta:resourcekey="lblGuide1" Text="'성명'을 클릭하면 개인회원정보 수정이 가능합니다." /></p>
    

        <!-- 검색 -->
        <div class="message-box default center">
            <asp:Label ID="lblName" runat="server" meta:resourcekey="lblName" CssClass="title" Text="선원검색" />
            <asp:TextBox ID="txtName" runat="server" CssClass="board-keyword" />
            <asp:Button ID = "btnRetrieve" runat ="server" Text="Search" CssClass="button-board-search" OnClick = "btnRetreive_Click" />
        </div>
        
        <div class="button-box right">
            <input type="button" id="btnNew" value="<%=GetLocalResourceObject("btnNewResource.Text").ToString() %>" class="button-default" title="소속 개인회원 등록"
                onclick="javascript:openPopWindow('/manage/user_edit.aspx?editmode=NEW&MenuCode=<%=Session["MENU_CODE"]%>','user_edit', '1024', '821');" />

            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button-default blue" OnClick="btnAdd_Click" ToolTip="교육 대상자 등록" meta:resourcekey="btnAddResource" />
        </div>

        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grd" runat="server" AllowSorting="True" AllowColSizing="True" AutoGenerateColumns="False" OnItemCreated="grd_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1BoundColumn DataField="user_id" Visible="false">
                        <ItemStyle Width="0%" />                                    
                    </C1WebGrid:C1BoundColumn>
                                    
                    <C1WebGrid:C1BoundColumn DataField="user_nm_kor" Visible="false">
                        <ItemStyle Width="0%" />                                    
                    </C1WebGrid:C1BoundColumn>                                       
                                
                    <C1WebGrid:C1TemplateColumn >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate> 
                        <ItemStyle Width="3%" />
                    </C1WebGrid:C1TemplateColumn>                                                                                                                             
                                    
                    <C1WebGrid:C1TemplateColumn >
                        <ItemTemplate>                                            
                            <a href="#" onclick="openPopWindow('/manage/user_edit.aspx?editmode=EDIT&user_id=<%# DataBinder.Eval(Container.DataItem, "user_id")%>&MenuCode=<%=Session["MENU_CODE"]%>','user_edit', '1024', '821');">
                            <%# DataBinder.Eval(Container.DataItem, "user_nm_kor")%></a> 
                        </ItemTemplate>
                        <ItemStyle Width="9%" CssClass ="left" />
                    </C1WebGrid:C1TemplateColumn>       
                                    
                    <C1WebGrid:C1BoundColumn DataField="user_nm_eng" >
                        <ItemStyle Width="13%" CssClass ="left"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>
                                    
                    <C1WebGrid:C1BoundColumn DataField="personal_no">
                        <ItemStyle Width="13%" />
                    </C1WebGrid:C1BoundColumn>
                                    
                    <C1WebGrid:C1BoundColumn DataField="duty_work" >
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>
                                    
                    <C1WebGrid:C1BoundColumn DataField="status" >
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>
                                    
                    <C1WebGrid:C1BoundColumn DataField="enter_dt" DataFormatString="{0:yyyy.MM.dd}">
                        <ItemStyle Width="13%" />
                    </C1WebGrid:C1BoundColumn>                                                                                                                          
                                    
                    <C1WebGrid:C1BoundColumn DataField="mobile_phone" >
                        <ItemStyle Width="12%" CssClass ="left"></ItemStyle>
                    </C1WebGrid:C1BoundColumn>
                                    
                    <C1WebGrid:C1BoundColumn DataField="email_id" >
                        <ItemStyle Width="13%" CssClass ="left" ></ItemStyle>
                    </C1WebGrid:C1BoundColumn>                                    
                                    
                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <input type="checkbox" id="chkHeader" name="chkHeader" onclick="CheckAll(this, 'chk');" title="교육생 등록"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" id="chk" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle  Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>
        
        <div class="gm-paging">
            <CLTWebControl:PageNavigator ID="PageNavigator1" runat="server" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" />
        </div>

        <!-- 버튼
        클래스명 button-group : 사방 20px 여백 포함됨.
        -->
        <!--div class="button-group center">
            <a href="#none" class="button-default blue">버튼1</a>
            <button type="button" class="button-default">버튼2</button>
            <input type="button" class="button-default" value="버튼3">
        </div-->
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>
