<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="pass.aspx.cs" Inherits="CLT.WEB.UI.LMS.EDUM.pass" Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">

    <script type="text/javascript" language="javascript">
    //이수체크박스 전체선택/해제 
    function CheckAll1(checkAllBox, chkName)                         
    {   
        var ChkState = checkAllBox.checked;
    
        var gridview = document.getElementById('<%=grdList.ClientID %>'); 
        chks = gridview.getElementsByTagName('input'); 
        
        for (var i = 0; i < chks.length; i++) 
        { 
            if (chks.item(i).className == chkName)
            {   
                //var grdIds = chks.item(i).id.split('_');
                //var grdItemCtl = ""; //grdIds[0]+'_'+grdIds[1];
                var grdItemCtl = chks.item(i).id.toString().substr(0, chks.item(i).id.toString().lastIndexOf('_'));
                
                //이수구분이 수강일때만 이수 전체선택/해제 적용
                if(document.getElementById(grdItemCtl+'_ddlPassFlg').value == "000004")
                    document.getElementById(grdItemCtl+'_' + chkName).checked = ChkState;
            }
        }
    }
    //발령체크박스 전체선택/해제 
    function CheckAll2(checkAllBox, chkName)                         
    {   
        var ChkState = checkAllBox.checked;
    
        var gridview = document.getElementById('<%=grdList.ClientID %>'); 
        chks = gridview.getElementsByTagName('input'); 
        
        for (var i = 0; i < chks.length; i++) 
        { 
            if (chks.item(i).className == chkName)
            {   
                //var grdIds = chks.item(i).id.split('_');
                //var grdItemCtl = grdIds[0]+'_'+grdIds[1];
                var grdItemCtl = chks.item(i).id.toString().substr(0, chks.item(i).id.toString().lastIndexOf('_'));

                //이수구분이 수강이고 이수체크박스에 체크된 데이터만 발령 전체선택/해제 적용,또는 이수구분이 이수인것만 발령 전체선택/해제 적용
                if((document.getElementById(grdItemCtl+'_ddlPassFlg').value == "000004" && document.getElementById(grdItemCtl+'_chkPassFlg').checked == true) ||(document.getElementById(grdItemCtl+'_ddlPassFlg').value == "000001"))
                    document.getElementById(grdItemCtl+'_' + chkName).checked = ChkState;
                
            }
        }
        
    }
    function fnValidateForm()
    {   
        var chkCnt = 1;
        var chks = new Array(); 
        var gridview = document.getElementById('<%=grdList.ClientID %>'); 
        chks = gridview.getElementsByTagName('input'); 
        
        for (var i = 0; i < chks.length; i++) { 
            //교육입과일때
            if (chks.item(i).className == "chkPassFlg")
            {   
                var grdIds = chks.item(i).id.split('_');
                var grdItemCtl = grdIds[0]+'_'+grdIds[1];
                
                //미이수시 미이수 사유 꼭입력
                if(!((document.getElementById(grdItemCtl+'_ddlPassFlg').value == "000004" && document.getElementById(grdItemCtl+'_chkPassFlg').checked == true) ||(document.getElementById(grdItemCtl+'_ddlPassFlg').value == "000001")))
                {   
                    if(isEmpty(document.getElementById(grdItemCtl+'_ddlNonPassCD'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A067", new string[] { "'+(chkCnt)+'번째 미이수사유" }, new string[] { "'+(chkCnt)+'th Non Pass"}, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) 
                    {   
                        return false;
                    }
                    //미이수시 발령 금지
                    else if(document.getElementById(grdItemCtl+'_chkOrderFlg').checked)
                    {
                        alert('<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A135", new string[] { "'+(chkCnt)+'" }, new string[] { "'+(chkCnt)+'" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>');
                        return false;
                    }
                }
                else{
                    //이수시 미이수 사유 입력 불가
                    if(isNotEmpty(document.getElementById(grdItemCtl+'_ddlNonPassCD'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A134", new string[] { "'+(chkCnt)+'" }, new string[] { "'+(chkCnt)+'"}, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) {   
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
        <h3><asp:Label ID="lblMenuTitle" runat="server" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p><asp:Label ID="lbl0" runat="server" meta:resourcekey="lblGuide0" Text="" />
           <asp:Label ID="lbl1" runat="server" meta:resourcekey="lblGuide1" Text="" /></p>-->

        <!-- 검색 -->
        <!--div class="message-box default center">
        </div>-->


        <!-- 상단 버튼-->
        <div class="button-box right">
        </div>


        <!-- 내용-->
        <section class="section-board">

            <CLTWebControl:PageInfo ID="PageInfo1" runat="server" Visible="false" />
            <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grdList" runat="server" AllowColSizing="True"
                AutoGenerateColumns="False" OnItemDataBound="grdUserList_ItemDataBound" DataKeyField="KEYS"
                OnItemCreated="grdList_ItemCreated">
                <Columns>

                    <C1WebGrid:C1BoundColumn DataField="USER_ID" HeaderText="사번">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="USER_NM_KOR" HeaderText="성명">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="COMPANY_NM" HeaderText="회사명">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="DEPT_NAME" HeaderText="부서명">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="STEP_NAME" HeaderText="직급">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="PROGRESS_RATE" HeaderText="진도">
                        <ItemStyle HorizontalAlign="Right" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="ASSESS_SCORE" HeaderText="기말">
                        <ItemStyle HorizontalAlign="Right" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="REPORT_SCORE" HeaderText="과제">
                        <ItemStyle HorizontalAlign="Right" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="TOTAL_SCORE" HeaderText="총점">
                        <ItemStyle HorizontalAlign="Right" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="COURSE_BEGIN_DT" HeaderText="교육<br/>시작일">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="COURSE_END_DT" HeaderText="교육<br/>종료일">
                        <ItemStyle />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="이수&lt;br/&gt;구분">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlPassFlg" runat="server">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="이수">
                        <HeaderStyle VerticalAlign="Middle"/>
                        <HeaderTemplate>
                            <asp:Label ID="lblchkPassFlg" runat="server" Text="이수" meta:resourcekey="lblchkPassFlg"></asp:Label>
                            <input type="checkbox" id="chk_all_Confirm" name="chk_all_Confirm" onclick="CheckAll1(this, 'chkPassFlg');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" id="chkPassFlg" name="chkPassFlg" runat="server" class="chkPassFlg"/>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="발령">
                        <HeaderStyle VerticalAlign="Middle"/>
                        <HeaderTemplate>
                            <asp:Label ID="lblchkOrderFlg" runat="server" Text="발령" meta:resourcekey="lblchkOrderFlg"></asp:Label>
                            <input type="checkbox" id="chk_all_Confirm" name="chk_all_Confirm" onclick="CheckAll2(this, 'chkOrderFlg');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" id="chkOrderFlg" name="chkOrderFlg" runat="server" class="chkOrderFlg"/><asp:HiddenField
                                ID="hdnOrderFlg" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ORDER_FLG")%>'/>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="미이수&lt;br/&gt;사유">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlNonPassCD" runat="server"></asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1TemplateColumn HeaderText="Remark">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRemark" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NON_PASS_REMARK")%>'></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle  Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>
        
        <div class="gm-paging">
            <CLTWebControl:PageNavigator ID="PageNavigator1" runat="server" Visible="false" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" />
        </div>


        <!-- 하단 버튼-->
        <div class="button-group center">  
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button-main-rnd lg blue" OnClick="btn_save_Click" OnClientClick="return fnValidateForm();" />
        </div>

        </section>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>
