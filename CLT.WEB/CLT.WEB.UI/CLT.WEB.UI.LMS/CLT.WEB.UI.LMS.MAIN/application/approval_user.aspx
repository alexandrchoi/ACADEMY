<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="approval_user.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.approval_user" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">
        function GoAppAdd()
        {	
        //    var txtSTART_DATE = opener.document.getElementById('ctl00_ContentPlaceHolderMainUp_txtSTART_DATE').value;
        //    var txtEND_DATE = opener.document.getElementById('ctl00_ContentPlaceHolderMainUp_txtEND_DATE').value;
        //    var ddlCourseType = opener.document.getElementById('ctl00_ContentPlaceHolderMainUp_ddlCourseType').value;
        //    var txtCourseNM = opener.document.getElementById('ctl00_ContentPlaceHolderMainUp_txtCourseNM').value;
        //    
            //lnkApprAdd.href = "/application/select_list.aspx?txt_start_date="+txtSTART_DATE+"&txt_end_date="+txtEND_DATE+"&ddl_course_type="+ddlCourseType+"&txt_course_nm="+encodeURI(txtCourseNM);
            //lnkApprAdd.href = '/application/select_list.aspx?search=<%=iSearch %>&MenuCode=<%=Session["MENU_CODE"]%>';
            //lnkApprAdd.click();
    
            openPopWindow('/application/select_user_list.aspx?search=<%=iSearch %>&MenuCode=<%=Session["MENU_CODE"]%>', "SelectUserForm", "1250", "860", "status=yes");
	        return false;
        }
        function GoCourseForm(opener_textbox_id_id, opener_textbox_nm_id)
        {	
            //open_course_id
            openPopWindow('/common/course_pop.aspx?opener_textbox_id='+opener_textbox_id_id+'&opener_textbox_nm='+opener_textbox_nm_id+'&MenuCode=<%=Session["MENU_CODE"]%>', "AppForm", "600", "650", "status=yes");
	        return false;
        }
        function fnValidateForm()
        {   
            var chkCnt = 1;
            var chks = new Array(); 
            var gridview = document.getElementById('<%=grdList.ClientID %>'); 
            chks = gridview.getElementsByTagName('input'); 
    
            for (var i = 0; i < chks.length; i++) { 
                //교육입과일때
                if (chks.item(i).className == "chkStartFlg")
                {   
                    var grdIds = chks.item(i).id.split('_');
                    var grdItemCtl = grdIds[0]+'_'+grdIds[1];
            
                    if(document.getElementById(grdItemCtl+'_chkStartFlg').checked)
                    {
                        //교육입과 일때 승인여부 확인   
                        if(!document.getElementById(grdItemCtl+'_chkApproval').checked)
                        {
                            alert('<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A122", new string[] { "'+(chkCnt)+'" }, new string[] { "'+(chkCnt)+'" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>');
                            return false;
                        }
                    }
                    //미승인시 교육불가사유확인
                    if(!document.getElementById(grdItemCtl+'_chkApproval').checked)
                    {   
                        if(isEmpty(document.getElementById(grdItemCtl+'_ddlNonApprovalCD'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A067", new string[] { "'+(chkCnt)+'번째 교육불가사유" }, new string[] { "'+(chkCnt)+'th Absent"}, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) 
                        {   
                            return false;
                        }
                    }
                    else{
                        //승인 경우 교육불가 사유입력 불가
                        if(isNotEmpty(document.getElementById(grdItemCtl+'_ddlNonApprovalCD'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A131", new string[] { "'+(chkCnt)+'" }, new string[] { "'+(chkCnt)+'"}, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) {   
                            return false;
                        }
                        else if(isNotEmpty(document.getElementById(grdItemCtl+'_txtNonApprovalRemark'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A131", new string[] { "'+(chkCnt)+'" }, new string[] { "'+(chkCnt)+'"}, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) {   
                            return false;
                        }
                        //본인취소인 경우 승인 불가
                        else if(document.getElementById(grdItemCtl+'_hdnApprovalFlag').value == "000005"){
                            alert('<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A132", new string[] { "'+(chkCnt)+'" }, new string[] { "'+(chkCnt)+'" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>');
                            return false;
                        }
                    }
                    chkCnt++;
                } 
            }
            return true;
        }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="수강신청/승인처리" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p></p>-->

        <!-- 검색 -->
        <div class="message-box default center">
            <asp:Label ID="lblStatus" runat="server" meta:resourcekey="lblStatus" CssClass="title" Text="승인상태" />
            <asp:DropDownList ID="ddlApprovalFlg" runat="server" CssClass="w50per" />
            <asp:Button ID = "btnRetrieve" runat ="server" Text="Search" CssClass="button-board-search" OnClick = "btnRetrieve_Click" />
        </div>
        
        <div class="button-box right">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button-default"  OnClientClick="return GoAppAdd();" meta:resourcekey="btnAddResource" />
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button-default blue" OnClick="btn_save_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnSaveResource" />
        </div>

        <!--<CLTWebControl:PageInfo ID="PageInfo1" runat="server" />-->
        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grdList" runat="server" AllowSorting="True" AllowColSizing="True" AutoGenerateColumns="False" OnItemDataBound="grdUserList_ItemDataBound" DataKeyField="KEYS" OnItemCreated="grdList_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1BoundColumn DataField="INS_DT" HeaderText="신청&lt;br/&gt;일자">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="INS_TIME" HeaderText="시간">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="USER_ID" HeaderText="사번">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="성명">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlkUserNMKor" runat="server"></asp:HyperLink>
                            <asp:HiddenField ID="hdnApprovalFlag" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "APPROVAL_FLG")%>'/>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="COMPANY_NM" HeaderText="회사명">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="PERSONAL_NO" HeaderText="주민등록&lt;br/&gt;번호">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="DEPT_NAME" HeaderText="부서명">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="STEP_NAME" HeaderText="직급">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="APPROVAL_FLG_NM" HeaderText="상태">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="승인">
                        <HeaderStyle />
                        <HeaderTemplate>
                            <asp:Label ID="lblchkApproval" runat="server" Text="승인" meta:resourcekey="lblConfirm"></asp:Label> <input type="checkbox" id="chk_all_Approval" name="chk_all_Approval" onclick="CheckAll(this, 'chkApproval');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" id="chkApproval" name="chkApproval" runat="server" />
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="훈련생&lt;br/&gt;구분">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlEmployedState" runat="server" Width="150px" Enabled="false">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="고용보험&lt;br/&gt;취득일자">
                        <ItemTemplate>
                            <nobr><asp:Label ID="lblEnterDT" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ENTER_DT")%>'></asp:Label></nobr>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="교육&lt;br/&gt;불가사유">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlNonApprovalCD" runat="server">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="불가&lt;br/&gt;사유">
                        <HeaderStyle/>
                        <HeaderTemplate>
                            <asp:Label ID="lblNonApprovalRemark" runat="server" Text="불가&lt;br/&gt;사유" meta:resourcekey="lblNonApprovalRemark"></asp:Label> <input type="checkbox" id="chk_all_Confirm" name="chk_all_Confirm" onclick="CheckAll(this, 'chkConfirm');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtNonApprovalRemark" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NON_APPROVAL_REMARK")%>' />
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="APPROVAL_DT" HeaderText="승인&lt;br/&gt;일자">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="입과">
                    <HeaderStyle />
                        <HeaderTemplate>
                            <asp:Label ID="lblchkStartFlg" runat="server" Text="입과" meta:resourcekey="lblchkStartFlg"></asp:Label> <input type="checkbox" id="chk_all_StartFlg" name="chk_all_StartFlg" onclick="CheckAll(this, 'chkStartFlg');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" id="chkStartFlg" name="chkStartFlg" runat="server" />
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle  Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>
        
        <!--div class="gm-paging">
            <CLTWebControl:PageNavigator ID="PageNavigator1" runat="server" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" />
        </div-->

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
