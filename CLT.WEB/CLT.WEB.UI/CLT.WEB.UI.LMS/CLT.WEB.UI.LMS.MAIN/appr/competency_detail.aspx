<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="competency_detail.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPR.competency_detail" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script language="javascript" type="text/javascript">
        function GoUserForm()
        {	
            //debugger;
            //Inquiry^VessleType^ShipCode^Rank
            openPopWindow('/appr/competency_user_list.aspx?search=<%=ddlInquiry.ClientID %>&bind_control=<%=hdnUserId.ClientID %>^<%=txtName.ClientID %>^<%=ddlAppDutyStep.ClientID %>^<%=ddlAppDutyStep.ClientID %>^<%=ddlShipCode.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>&app_cd=appr&step_gu='+document.getElementById('<%=ddlInquiry.ClientID %>').value, "UserListForm", "800", "480", "status=no");
	        document.getElementById('<%=hdnAppNo.ClientID %>').value = ''; 
	        return false;
        }
    
        function GoMgrForm()
        {
            //Inquiry^VessleType^ShipCode^Rank
            openPopWindow('/appr/competency_user_list.aspx?search=<%=ddlInquiry.ClientID %>&bind_control=<%=txtMgrID.ClientID %>^<%=txtMgrNm.ClientID %>^<%=txtMgrStep.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>&app_cd=appr1&step_gu='+document.getElementById('<%=ddlInquiry.ClientID %>').value, "UserListForm", "800", "480", "status=no");
	        return false;
        }
    
        function fnValidateForm()
        {
            if (isEmpty(document.getElementById('<%=hdnUserId.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A067", new string[] { lblName.Text }, new string[] { lblName.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtAppDate.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A067", new string[] { lblAppDate.Text }, new string[] { lblAppDate.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(!isDateChk(document.getElementById('<%=txtAppDate.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblAppDate.Text }, new string[] { lblAppDate.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; 
            if(isEmpty(document.getElementById('<%=ddlAppDutyStep.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A067", new string[] { lblRank.Text }, new string[] { lblRank.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlVslType.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A067", new string[] { lblVessleType.Text }, new string[] { lblVessleType.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlShipCode.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A067", new string[] { lblShip.Text }, new string[] { lblShip.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(!isNumber(document.getElementById('<%=txtTotScore.ClientID %>'),'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A108", new string[] { lblTotScore.Text }, new string[] { lblTotScore.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
            return true;
        }
        function totGrade()
        {
            var totScore = 0;
            var dropdowns = new Array(); 
            var gridview = document.getElementById('<%=grdApprItem.ClientID %>'); 
            dropdowns = gridview.getElementsByTagName('select'); 
        
            for (var i = 0; i < dropdowns.length; i++) { 
                if (dropdowns.item(i).className == "ddlGrade" && dropdowns.item(i).value != '')
                {   
                    //alert(dropdowns.item(i).value);
                    var opts = dropdowns[i].getElementsByTagName("option"); 
                    for(var j=0;j<opts.length;j++){ 
                        if(opts[j].selected) { 
                    
                             //alert(parseInt(opts[j].value))
                             //totScore += parseInt(opts[j].innerHTML);
                             totScore += parseInt(opts[j].value);
                        } 
                    } 
                } 
            }
            document.getElementById('<%=txtTotScore.ClientID %>').value = totScore;
        }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="평가결과 입력/조회" meta:resourcekey="lblMenuTitle" /></h3>

        <!-- 버튼 -->
        <div class="button-box right">
            <asp:Button ID="btnRetrieve" runat="server" Text="Retrieve" CssClass="button-default blue" OnClick="btnRetrieve_Click"  OnClientClick="return fnValidateForm();" meta:resourcekey="btnRetrieve" />
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button-default" OnClick="btnSave_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnSaveResource" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" OnClick="btnExcel_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnExcelResource" />
        </div>
    
        <div class="gm-table data-table read-type">
                
            <table>
                <colgroup>
                    <col width="15%">
                    <col width="35%">
                    <col width="15%">
                    <col width="35%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblInquiry" runat="server" Text="" meta:resourcekey="lblInquiry" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlInquiry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlInquiry_SelectedIndexChanged" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblMgrID" runat="server" Text="" meta:resourcekey="lblMgrID" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMgrNm" runat="server" CssClass="w180" />
                        <asp:HiddenField ID="txtMgrID" runat="server"/>
                        <asp:HiddenField ID="txtMgrStep" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblName" runat="server" Text="" meta:resourcekey="lblName" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" CssClass="w180" />
                        <a href="#" class="button-board-search" onclick="javascript:return GoUserForm();">Search</a>
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblAppDate" runat="server" Text="" meta:resourcekey="lblAppDate" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtAppDate" runat="server" MaxLength="10" CssClass="datepick w180" />
                        <asp:HiddenField ID="hdnUserId" runat="server"/>
                        <asp:HiddenField ID="hdnAppNo" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblRank" runat="server" Text="" meta:resourcekey="lblAppDate" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlAppDutyStep" runat="server" CssClass="w180" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblVessleType" runat="server" Text="" meta:resourcekey="lblVessleType" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlVslType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlVslType_SelectedIndexChanged" />
                        <asp:DropDownList ID="ddlVslTypeC" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblShip" runat="server" Text="" meta:resourcekey="lblShip" />
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlShipCode" runat="server" CssClass="w180" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblTotScore" runat="server" Text="" meta:resourcekey="lblTotScore" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtTotScore" runat="server" CssClass="w180" />
                    </td>
                </tr>
                </tbody>
            </table>
        
        </div>
        
        <!-- Data Table - List type -->
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grdApprItem" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="grdApprItem_ItemDataBound" DataKeyField="app_keys" OnItemCreated="grdApprItem_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1BoundColumn DataField="app_item_seq" HeaderText="No.">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="app_item_nm" HeaderText="역량명">
                        <ItemStyle CssClass="left"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="app_item_desc" HeaderText="역량정의">
                        <ItemStyle CssClass="left"  />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="app_case_seq" HeaderText="Seq">
                        <ItemStyle  CssClass="left" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="app_case_desc" HeaderText="행위사례">
                        <ItemStyle CssClass="left"  />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="평가">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddl_grade" runat="server" CssClass="ddlGrade" />
                        </ItemTemplate>
                        <ItemStyle  Width="70px"/>
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="Remark" HeaderText="비고">
                        <ItemStyle CssClass="left"  />
                    </C1WebGrid:C1BoundColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>


        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grdCode" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemCreated="grdCode_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1BoundColumn DataField="grade" HeaderText="등급">
                        <ItemStyle Width="50px" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="grade_nm" HeaderText="평가내용">
                        <ItemStyle Width="60px" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="grade_nm_eng" HeaderText="평가내용">
                        <ItemStyle Width="60px" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="score" HeaderText="점수">
                        <ItemStyle Width="50px" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="grade_desc" HeaderText="설명">
                        <ItemStyle CssClass="left"/>
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="grade_desc_eng" HeaderText="설명">
                        <ItemStyle CssClass="left"/>
                    </C1WebGrid:C1BoundColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>

        <!-- 버튼 -->
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>