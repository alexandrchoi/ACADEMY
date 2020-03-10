<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="received_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.received_edit" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">
        function setUserAdd(datas)
		{
		    hidUser = document.getElementById("<%=HidUserAdd.ClientID %>");             
		    hidUser.value = datas[0]; 			
		    <%= Page.GetPostBackEventReference(LnkBtnUserAdd) %>;
	    }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <!-- User 정보 선택 했을때 가져온 hidden 필드 및 link 필드 -->
    <asp:HiddenField runat ="server" ID="HidUserAdd" />   
    <asp:LinkButton ID="LnkBtnUserAdd" runat = "server" OnClick="LnkBtnUserAdd_Click" /> 
    
    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="교육접수" meta:resourcekey="lblMenuTitle" /></h3>
    

        <!-- 검색 -->
        <div class="gm-table data-table read-type">
            <table>
                <colgroup>
                    <col width="30%">
                    <col width="*">
                </colgroup>
                <tbody>
                <tr>
                    <!-- 과정명 -->
                    <th scope="col"><asp:Label ID="lblCourseNm" runat="server" meta:resourcekey="lblCourseNm" /></th>
                    <td><asp:TextBox runat="server" ID="txtCourseNm" MaxLength="50" ReadOnly="true" /></td>
                </tr>
                <tr>
                    <!-- 교육기간 -->
                    <th scope="col"><asp:Label ID="lblCourseDt" runat="server" meta:resourcekey="lblCourseDt" /></th>
                    <td><asp:TextBox ID="txtCourseDt" runat="server" ReadOnly="true" /></td>
                </tr>
                <tr>
                    <!-- 접수인원 -->
                    <th scope="col"><asp:Label ID="lblCount" runat="server" meta:resourcekey="lblCount" /></th>
                    <td><asp:TextBox ID="txtCount" runat="server" ReadOnly="true" /></td>
                </tr>
                </tbody>
            </table>
        </div>

        <div class="button-box right">
            <input type="button" id="btnNew" value="<%=GetLocalResourceObject("btnNewResource.Text").ToString() %>" class="button-default"
                onclick="javascript:openPopWindow('/application/received_pop_user.aspx?open_course_id=<%=ViewState["OPEN_COURSE_ID"]%>&MenuCode=<%=Session["MENU_CODE"]%>&ManTotCnt=<%=ViewState["ManTotCnt"] %>','received_pop_user', '1200', '755');" /> 
            <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button-default blue"  OnClick="btnSave_Click" meta:resourcekey="btnSaveResource" /> 
            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button-default" OnClick="btnDelete_Click" meta:resourcekey="btnDeleteResource" />
        </div>

        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grd" runat="server" AllowSorting="True" AllowColSizing="True" AutoGenerateColumns="False" OnItemCreated="grd_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1BoundColumn DataField="user_id" Visible="false">
                        <ItemStyle Width="0%" /> 
                    </C1WebGrid:C1BoundColumn>
                                    
                    <C1WebGrid:C1BoundColumn DataField="MOBILE_PHONE" Visible="false">
                        <ItemStyle Width="0%" />
                    </C1WebGrid:C1BoundColumn>   
                                
                    <C1WebGrid:C1TemplateColumn >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate> 
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn>    
                                    
                    <C1WebGrid:C1BoundColumn DataField="personal_no">
                        <ItemStyle Width="15%" />
                    </C1WebGrid:C1BoundColumn>
                                    
                    <C1WebGrid:C1BoundColumn DataField="user_nm_kor" >
                        <ItemStyle Width="20%" />
                    </C1WebGrid:C1BoundColumn>
                                    
                    <C1WebGrid:C1BoundColumn DataField="duty_work" >
                        <ItemStyle Width="15%" />
                    </C1WebGrid:C1BoundColumn>
                                    
                    <C1WebGrid:C1BoundColumn DataField="status" >
                        <ItemStyle Width="15%" />
                    </C1WebGrid:C1BoundColumn>
                                    
                    <C1WebGrid:C1BoundColumn DataField="enter_dt" Visible="false">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>               
                                    
                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <input type="checkbox" id="chkHeader" name="chkHeader" onclick="CheckAll(this, 'chk');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" id="chk" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="10%" />
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
