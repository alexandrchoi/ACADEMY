<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="competency_user_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPR.competency_user_list" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">
        function SetUserId(userId, userNMKor, dutyStep, personalNo, dutyWork, shipCode, ctlUserId, ctlUserNMKor, ctlDutyStep, ctlPersonalNo, ctlShipCode)
        {   
            if(opener.document.getElementById(ctlUserId) != "undefined" && opener.document.getElementById(ctlUserId) != null)
                opener.document.getElementById(ctlUserId).value = userId;
            
            if(opener.document.getElementById(ctlUserNMKor) != "undefined" && opener.document.getElementById(ctlUserNMKor) != null)
                opener.document.getElementById(ctlUserNMKor).value = userNMKor;
            
            if(opener.document.getElementById(ctlDutyStep) != "undefined" && opener.document.getElementById(ctlDutyStep) != null){
                if('<%=iAppCD %>' == 'appr'){
                    
                    if('<%=iStepGu %>' == '000001'){
                        opener.document.getElementById(ctlDutyStep).value = dutyWork;
                    }
                    else if('<%=iStepGu %>' == '000002'){
                        opener.document.getElementById(ctlDutyStep).value = dutyStep;
                    }
                    
                }
                else {
                    opener.document.getElementById(ctlDutyStep).value = dutyStep;
                }
            }
            
            if(opener.document.getElementById(ctlPersonalNo) != "undefined" && opener.document.getElementById(ctlPersonalNo) != null)
                opener.document.getElementById(ctlPersonalNo).value = personalNo;
            
            if(opener.document.getElementById(ctlShipCode) != "undefined" && opener.document.getElementById(ctlShipCode) != null)
                opener.document.getElementById(ctlShipCode).value = shipCode;                
            
            //if(opener.document.getElementById("ddlAppDutyStep") != "undefined" && opener.document.getElementById("ddlAppDutyStep") != null)
            //    opener.document.getElementById("ddlAppDutyStep").value = "";
            
            self.close();
        }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="회원검색" meta:resourcekey="lblMenuTitle" /></h3>
    

        <!-- 검색 -->
        <div class="gm-table plane-table">
            <table>
                <colgroup>
                    <col width="15%">
                    <col width="35%">
                    <col width="15%">
                    <col width="35%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="col"><asp:Label ID="lblDeptCode" runat="server" meta:resourcekey="lblDeptCode" /></th>
                    <td><asp:DropDownList ID="ddlDeptCode" runat="server" /></td>
                    <th scope="col"><asp:Label ID="lblDutyStep" runat="server" meta:resourcekey="lblDutyStep" /></th>
                    <td><asp:DropDownList ID="ddlDutyStep" runat="server" /></td>
                </tr>
                <tr>
                    <th scope="col"><asp:Label ID="lblUserId" runat="server" meta:resourcekey="lblUserId" /></th>
                    <td><asp:TextBox runat="server" ID="txtUserId" MaxLength="50" /></td>
                    <th scope="col"><asp:Label ID="lblUserNMKor" runat="server" meta:resourcekey="lblUserNMKor" /></th>
                    <td><asp:TextBox runat="server" ID="txtUserNMKor" MaxLength="50" /></td>
                </tr>
                <tr>
                    <th scope="col"><asp:Label ID="lblPersonalNo" runat="server" meta:resourcekey="lblPersonalNo" /></th>
                    <td>
                        <asp:TextBox runat="server" ID="txtPersonalNo" MaxLength="50" />
                    <td colspan="2">
                        <asp:Button ID="btnRetrieve" runat="server" Text="Retrieve" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" />
                    </td>
                </tr>
                </tbody>
            </table>
        </div>


        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grdUserList" runat="server" AllowSorting="True" AllowColSizing="True" AutoGenerateColumns="False" DataKeyField="user_id" OnItemDataBound="grdUserList_ItemDataBound" OnItemCreated="grdUserList_ItemCreated" OnItemCommand="grdUserList_ItemCommand">
                <Columns>
                    
                    <C1WebGrid:C1TemplateColumn HeaderText="사번">
                        <ItemTemplate>
                            <asp:LinkButton ID="hlkUserId" runat="server" CommandName="UserId" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "user_id") + "^" +DataBinder.Eval(Container.DataItem, "user_nm_kor") + "^" +DataBinder.Eval(Container.DataItem, "duty_step") + "^" + DataBinder.Eval(Container.DataItem, "personal_no") + "^" + DataBinder.Eval(Container.DataItem, "duty_work")+ "^" + DataBinder.Eval(Container.DataItem, "dept_code") %>'><%# DataBinder.Eval(Container.DataItem, "user_id")%></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="user_nm_kor" HeaderText="성명">
                        <ItemStyle CssClass="left" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="dept_ename1" HeaderText="선박">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="step_name" HeaderText="직급">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="duty_work_name" HeaderText="직책">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="personal_no" HeaderText="주민번호">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle  Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>
        
        <div class="gm-paging">
            <CLTWebControl:PageNavigator ID="PageNavigator1" runat="server" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" />
        </div>

    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>
