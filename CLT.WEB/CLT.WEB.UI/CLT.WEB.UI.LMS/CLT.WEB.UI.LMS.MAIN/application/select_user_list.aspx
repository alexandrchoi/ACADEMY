<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="select_user_list.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.select_user_list" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">
        function fnValidateForm()
        {   
            var chkCnt = 1;
            var chks = new Array(); 
            var gridview = document.getElementById('<%=grdList.ClientID %>'); 
            chks = gridview.getElementsByTagName('input'); 
            var xIsBreak = true;
        
        
            //2013.07.13
            //교육불가사휴 필수 체크제거 (문서영대리 요청)
            /*
            for (var i = 0; i < chks.length; i++) { 
            
                if (chks.item(i).className == "chkConfirm")
                {   
                    var grdIds = chks.item(i).id.split('_');
                    var grdItemCtl = grdIds[0]+'_'+grdIds[1];
                
                    //if(document.getElementById(grdItemCtl+'_chk_sel').checked)
                    //{ 
                
                        if(!document.getElementById(grdItemCtl+'_chkConfirm').checked)
                        {  
                            if(isEmpty(document.getElementById(grdItemCtl+'_ddlNonApproval'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A067", new string[] { "'+(chkCnt)+'번째 교육불가사유" }, new string[] { "'+(chkCnt)+'th Absent"}, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) 
                            {   
                                return false;
                            }
                        }
                        else
                        {
                            if(isNotEmpty(document.getElementById(grdItemCtl+'_ddlNonApproval'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A131", new string[] { "'+(chkCnt)+'" }, new string[] { "'+(chkCnt)+'"}, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) 
                            {   
                                return false;
                            }
                            else if(isNotEmpty(document.getElementById(grdItemCtl+'_txtNonApprovalRemark'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A131", new string[] { "'+(chkCnt)+'" }, new string[] { "'+(chkCnt)+'"}, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) 
                            {   
                                return false;
                            }
                        }
                    //}
                
                    chkCnt++;
                } 
            }
           */ 
            return true;
        }
    
        function GoUserForm()
        {	
            //Inquiry^VessleType^ShipCode^Rank
            openPopWindow('/appr/competency_user_list.aspx?search=<%=iSearch %>&bind_control=&MenuCode=<%=Session["MENU_CODE"]%>&app_cd=edu_user', "EduUserListForm", "820", "860", "status=no");
            return false;
        }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    
    <script type="text/javascript" language="javascript">
        function ChangeLayer(str) 
        {
            <%-- 
            var page1, grid1, pageNav1, btn1;
            page1 = document.getElementById("<%=PageInfo1.ClientID%>");
            grid1 = document.getElementById("<%=grdList.ClientID%>");
            pageNav1 = document.getElementById("<%=PageNavigator1.ClientID%>");
            btn1 = document.getElementById("<%=btnSave1.ClientID%>");
            var page2, grid2, pageNav2, btn2;
            page2 = document.getElementById("<%=PageInfo2.ClientID%>");
            grid2 = document.getElementById("<%=grdUserList.ClientID%>");
            pageNav2 = document.getElementById("<%=PageNavigator2.ClientID%>");
            btn2 = document.getElementById("<%=btnSave2.ClientID%>");
            
            page1.style.display = grid1.style.display = pageNav1.style.display = btn1.style.display = str == "1" ? "inline-block" : "none";
            page2.style.display = grid2.style.display = pageNav2.style.display = btn2.style.display = str == "2" ? "inline-block" : "none";
            --%>
            
        } 
    </script>

    <!-- 팝업 컨텐츠 시작 -->
    <div-- class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="교육대상자 선발" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p></p>-->

        <!-- 검색 -->
        <div class="search-box">
                
            <%--<div class="search-group" id="tabTr_1">
                <a href="./select_user_list.aspx?tab=1&search=<%=iSearch %>&MenuCode=<%=Session["MENU_CODE"]%>" onclick="ChangeLayer('1')" id="t_tab1">
                <asp:Label ID="lblTab1" runat="server" Text="교육대상자" /></a>
            </div>
            <div class="search-group" id="tabTr_2">
                <a href="./select_user_list.aspx?tab=2&search=<%=iSearch %>&MenuCode=<%=Session["MENU_CODE"]%>" onclick="ChangeLayer('2')" id="t_tab2">
                <asp:Label ID="lblTab2" runat="server" Text="전체해상직원" /></a>
            </div>--%>

            <div class="search-group">
                <asp:Label ID="lblNationality" runat="server" CssClass="title" Text="국적" meta:resourcekey="lblNationality" />
                <asp:DropDownList ID="ddllNationality" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblVacationFlg" runat="server" CssClass="title" Text="휴가여부" meta:resourcekey="lblVacationFlg" />
                
                <label class="radio-box">
                    <asp:RadioButton ID="rdoInsu_Y" GroupName="rdoInsu" runat="server" Checked="true" Text="Y" AutoPostBack="false" />
                    <span class="radiomark"></span>
                </label>
                <label class="radio-box">
                    <asp:RadioButton ID="rdoInsu_N" GroupName="rdoInsu" runat="server" Checked="false" Text="N" AutoPostBack="false" />
                    <span class="radiomark"></span>
                </label>
            </div>
            <div class="search-group">
                <asp:Label ID="lblRank" runat="server" CssClass="title" Text="직급" meta:resourcekey="lblRank" />
                <asp:DropDownList ID="ddlRank" runat="server" />
            </div>
            <asp:Button ID = "btnRetrieve" runat ="server" Text="Search" CssClass="button-board-search" OnClick = "btnRetrieve_Click" />
        </div>
        
        <div class="button-box right">
            <asp:Button ID="btnSave1" runat="server" Text="Save" CssClass="button-default blue" OnClick="btnSave1_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnSaveResource" />
            <asp:Button ID="btnSave2" runat="server" Text="Save" CssClass="button-default blue" OnClick="btnSave2_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnSaveResource" />
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button-default" OnClientClick="GoUserForm(); return false;" meta:resourcekey="btnAddResource" />
            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button-default" OnClick="btnExcel_Click" meta:resourcekey="btnExcelResource" />
        </div>

        <!-- div class="gm-grid column-2"-->

                <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
                <div class="gm-table data-table list-type mt10">
                    <C1WebGrid:C1WebGrid ID="grdList" runat="server" AllowSorting="True" AllowColSizing="True" AutoGenerateColumns="False" OnItemDataBound="grdList_ItemDataBound" DataKeyField="KEYS" OnItemCreated="grdList_ItemCreated">
                        <Columns>
                    
                            <C1WebGrid:C1TemplateColumn HeaderText="No.">
                                <ItemStyle Width="30px" />
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 + 100 * (this.PageNavigator1.CurrentPageIndex - 1)%>
                                </ItemTemplate>
                            </C1WebGrid:C1TemplateColumn>
                            <C1WebGrid:C1BoundColumn DataField="DEPT_NAME" HeaderText="부서명">
                                <ItemStyle CssClass="left" />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="STEP_NAME" HeaderText="직급">
                                <ItemStyle />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="USER_ID" HeaderText="사번">
                                <ItemStyle />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="USER_NM_KOR" HeaderText="성명">
                                <ItemStyle />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="ORD_FDATE" HeaderText="최종선박하선일">
                                <ItemStyle />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="INS_DT" HeaderText="교육신청일">
                                <ItemStyle />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="USER_COURSE_END_DT" HeaderText="이전이수일">
                                <ItemStyle />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1TemplateColumn HeaderText="확정">
                                <ItemStyle/>
                                <HeaderStyle/>
                                <HeaderTemplate>
                                    <asp:Label ID="lblGrdListConfirm" runat="server" Text="확정" meta:resourcekey="lblConfirm" /> <input type="checkbox" id="chk_all_Confirm" name="chk_all_Confirm" onclick="CheckAll(this, 'chkConfirm');" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input type="checkbox" id="chkConfirm" name="chkConfirm" runat="server" />
                                </ItemTemplate>
                            </C1WebGrid:C1TemplateColumn>
                            <C1WebGrid:C1TemplateColumn HeaderText="교육불가사유">
                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlNonApproval" runat="server">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </C1WebGrid:C1TemplateColumn>
                            <C1WebGrid:C1TemplateColumn HeaderText="불가사유">
                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNonApprovalRemark" runat="server" />
                                </ItemTemplate>
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
    
                <CLTWebControl:PageInfo ID="PageInfo2" runat="server" Visible="false" />
                <div class="gm-table data-table list-type mt10 off-screen">
                    <C1WebGrid:C1WebGrid ID="grdUserList" runat="server" AllowSorting="True" AllowColSizing="True" AutoGenerateColumns="False"
                        OnItemDataBound="grdUserList_ItemDataBound" DataKeyField="KEYS" OnItemCreated="grdUserList_ItemCreated"
                        Visible="false" >
                        <Columns>
                    
                            <C1WebGrid:C1TemplateColumn HeaderText="No.">
                                <ItemStyle Width="30px" />
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 + this.PageSize * (this.PageNavigator2.CurrentPageIndex - 1)%>
                                </ItemTemplate>
                            </C1WebGrid:C1TemplateColumn>
                            <C1WebGrid:C1BoundColumn DataField="DEPT_NAME" HeaderText="현재 부서명">
                                <ItemStyle CssClass="left"/>
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="STEP_NAME" HeaderText="직급">
                                <ItemStyle />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="USER_ID" HeaderText="사번">
                                <ItemStyle />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="USER_NM_KOR" HeaderText="성명">
                                <ItemStyle />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="ORD_FDATE" HeaderText="최종선박하선일">
                                <ItemStyle />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="INS_DT" HeaderText="교육신청일">
                                <ItemStyle />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="USER_COURSE_END_DT" HeaderText="이전이수일">
                                <ItemStyle />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1TemplateColumn HeaderText="확정">
                                <ItemStyle/>
                                <HeaderStyle/>
                                <HeaderTemplate>
                                    <asp:Label ID="lblgrdUserListConfirm" runat="server" Text="확정" meta:resourcekey="lblConfirm" /> <input type="checkbox" id="chk_all_Confirm" name="chk_all_Confirm" onclick="CheckAll(this, 'chkConfirm');" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input type="checkbox" id="chkConfirm" name="chkConfirm" runat="server" />
                                </ItemTemplate>
                            </C1WebGrid:C1TemplateColumn>

                        </Columns>
                        <HeaderStyle Font-Bold="true" />
                        <ItemStyle  Wrap="true"  />
                        <AlternatingItemStyle />
                    </C1WebGrid:C1WebGrid>
                </div>
        
                <div class="gm-paging">
                    <CLTWebControl:PageNavigator ID="PageNavigator2" runat="server" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" Visible="false" />
                </div>


    <!--/div-->
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>
