<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="item_excel.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPR.item_excel" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">
    function fnValidateForm()
    {   
        var chkCnt = 1;
        var chks = new Array(); 
        var gridview = document.getElementById('<%=grdItem.ClientID %>'); 
        chks = gridview.getElementsByTagName('input'); 
        
        for (var i = 0; i < chks.length; i++) { 
            if (chks.item(i).className == "chk_sel" && chks.item(i).checked)
            {   
                var grdIds = chks.item(i).id.split('_');
                var grdItemCtl = grdIds[0]+'_'+grdIds[1];
                
                if(isEmpty(document.getElementById(grdItemCtl+'_txtAppBaseDT'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "기준일자" }, new string[] { "Date" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(!isDateChk(document.getElementById(grdItemCtl+'_txtAppBaseDT'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { "기준일자" }, new string[] { "Date" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; 
                if(isEmpty(document.getElementById(grdItemCtl+'_ddlStepGu'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "구분" }, new string[] { "Classification" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(isEmpty(document.getElementById(grdItemCtl+'_ddlAppDutyStep'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "평가대상" }, new string[] { "Evalution" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(isEmpty(document.getElementById(grdItemCtl+'_ddlVslTypeP'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "선종" }, new string[] { "Vessle Type" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                
                if(isEmpty(document.getElementById(grdItemCtl+'_txtAppItemSeq'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "역량No" }, new string[] { "No" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(!isNumber(document.getElementById(grdItemCtl+'_txtAppItemSeq'),'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A108", new string[] { "역량No" }, new string[] { "No" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(isMaxLenth(document.getElementById(grdItemCtl+'_txtAppItemSeq'),3,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { "역량No", "1", "2" }, new string[] { "No", "1", "2" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
                if(isEmpty(document.getElementById(grdItemCtl+'_txtAppItemNM'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "역량명" }, new string[] { "Name of Competency" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(isMaxLenth(document.getElementById(grdItemCtl+'_txtAppItemNM'),90, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { "역량명", "30", "60" }, new string[] { "Name of Competency", "30", "60" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(isMaxLenth(document.getElementById(grdItemCtl+'_txtAppItemNM'),90, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { "역량명", "30", "60" }, new string[] { "Name of Competency", "30", "60" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
                if(isEmpty(document.getElementById(grdItemCtl+'_txtAppItemDesc'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "역량정의" }, new string[] { "Definition of Competency" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(isEmpty(document.getElementById(grdItemCtl+'_txtAppCaseSeq'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "SEQ" }, new string[] { "SEQ" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(!isNumber(document.getElementById(grdItemCtl+'_txtAppCaseSeq'),'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A108", new string[] { "SEQ" }, new string[] { "SEQ" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(isMaxLenth(document.getElementById(grdItemCtl+'_txtAppCaseSeq'),3, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { "SEQ", "1", "2" }, new string[] { "SEQ","1", "2" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                
                if(isEmpty(document.getElementById(grdItemCtl+'_txtAppCaseDesc'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "행위사례" }, new string[] { "Descrition" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                
                chkCnt++;
            } 
            else if(chks.item(i).className == "chk_sel")
            {
                chkCnt++;
            }
        }
        
        return true;
    }
    function GoCourseForm(opener_textbox_id, opener_textbox_nm, course_type)
    {	
        openPopWindow('/common/course_pop.aspx?opener_textbox_id='+opener_textbox_id+'&opener_textbox_nm='+opener_textbox_nm+'&course_type='+course_type+'&MenuCode=<%=Session["MENU_CODE"]%>', "CourseForm", "600", "721", "status=no");
	    return false;
    }
</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    
    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container company-register user-edit">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="회원사" meta:resourcekey="lblMenuTitle" /></h3>
    
        
        <!-- 검색 -->
        <div class="message-box default center">
            
            <table>
                <tr>
                    <td>

                        <!-- 파일 첨부 인풋 -->
                        <div class="file-box">
                            <input class="upload-name" value="" disabled="disabled" />
                            <label for="<%=fu_excel.ClientID %>"><asp:Label ID="lblSearchFile" runat="server" Text="찾아보기" meta:resourcekey="lblSearchFile" /></label>
                            <asp:FileUpload ID="fu_excel" runat="server" CssClass="upload-hidden" />
                        </div>
                        <!-- 파일 첨부 인풋 -->

                    </td>
                </tr>
            </table>
                
            </div>

            <!-- 버튼 -->
            <div class="button-box right">
                <asp:Button ID="btnExcel" runat="server" Text="Upload" CssClass="button-default blue" meta:resourcekey="btnExcelResource" OnClick="btnExcel_Click" />
                <asp:Button ID="btnSend" runat="server" Text="Notice" CssClass="button-default" meta:resourcekey="btnUploadResource" OnClick="btnSend_Click" OnClientClick="return fnValidateForm();" />
                <input type="button" name="btnExcelTemplate" value="Excel Template" onclick="location.href='/file/download/item_list.xls';" id="btnExcelTemplate" class="button-default" />
            </div>

            <!-- Data Table - List type -->
        
            <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
            <div class="gm-table data-table list-type">
                <C1WebGrid:C1WebGrid ID="grdItem" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemDataBound="grdItem_ItemDataBound" OnItemCreated="grdItem_ItemCreated">
                    <Columns>
                    
                        <C1WebGrid:C1TemplateColumn>
                            <HeaderTemplate>
                                <input type="checkbox" id="chk_all_sel" name="chk_all_sel" onclick="CheckAll(this, 'chk_sel');" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" id="chk_sel" name="chk_sel" runat="server" class="chk_sel"/>
                            </ItemTemplate>
                            <ItemStyle Width="25px" />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1TemplateColumn HeaderText="구분">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlStepGu" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStepGu_SelectedIndexChanged" />
                            </ItemTemplate>
                            <ItemStyle />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1TemplateColumn HeaderText="평가대상">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlAppDutyStep" runat="server" />
                            </ItemTemplate>
                            <ItemStyle />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1TemplateColumn HeaderText="선종">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlVslTypeP" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlVslTypeP_SelectedIndexChanged" /><br/>
                                <asp:DropDownList ID="ddlVslTypeC" runat="server" />
                            </ItemTemplate>
                            <ItemStyle />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAppItemSeq" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "No")%>' CssClass="right" />
                            </ItemTemplate>
                            <ItemStyle />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1TemplateColumn HeaderText="역량명">
                            <ItemTemplate>
                                <nobr>국문 : <asp:TextBox ID="txtAppItemNM" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "역량명")%>' /></nobr>
                                <br />
                                <nobr>영문 : <asp:TextBox ID="txtAppItemNMEng" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "영문 역량명")%>' /></nobr>
                            </ItemTemplate>
                            <ItemStyle CssClass="left" />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1TemplateColumn HeaderText="역량정의">
                            <ItemTemplate>
                                <nobr>국문 : <asp:TextBox ID="txtAppItemDesc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "역량정의")%>' /></nobr>
                                <br />
                                <nobr>영문 : <asp:TextBox ID="txtAppItemDescEng" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "영문 역량정의")%>' /></nobr>
                            </ItemTemplate>
                            <ItemStyle CssClass="left" />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1TemplateColumn HeaderText="SEQ">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAppCaseSeq" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SEQ")%>' CssClass="right" />
                            </ItemTemplate>
                            <ItemStyle />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1TemplateColumn HeaderText="행위사례">
                            <ItemTemplate>
                                <nobr>국문 : <asp:TextBox ID="txtAppCaseDesc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "행위사례")%>' /></nobr>
                                <br />
                                <nobr>영문 : <asp:TextBox ID="txtAppCaseDescEng" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "영문 행위사례")%>' /></nobr>
                            </ItemTemplate>
                            <ItemStyle CssClass="left" />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1TemplateColumn HeaderText="기준일자">
                            <ItemTemplate>
                                <nobr><asp:TextBox ID="txtAppBaseDT" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container.DataItem, "기준일자")%>' CssClass="datepick" /></nobr>
                            </ItemTemplate>
                            <ItemStyle />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1TemplateColumn HeaderText="OJT">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCourseOJT" runat="server"
                                    ReadOnly="true" /><br />
                                <nobr><asp:TextBox ID="hdnCourseOJT" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OJT")%>' />
                                      <asp:Button ID="btnSearchOJT" runat ="server" Text="Search" CssClass="button-board-search" /> 
                                </nobr>
                            </ItemTemplate>
                            <ItemStyle CssClass="left" Width="90px" />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1TemplateColumn HeaderText="LMS">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCourseLMS" runat="server" ReadOnly="true" /><br />
                                <nobr><asp:TextBox ID="hdnCourseLMS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LMS")%>' Width="60px"/>
                                      <asp:Button ID="btnSearchLMS" runat ="server" Text="Search" CssClass="button-board-search" /> 
                                </nobr>
                            </ItemTemplate>
                            <ItemStyle CssClass="left" Width="90px" />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1TemplateColumn HeaderText="Others">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCourseEtc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Others")%>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="left" />
                        </C1WebGrid:C1TemplateColumn>

                    </Columns>
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle Wrap="true"  />
                    <AlternatingItemStyle />
                </C1WebGrid:C1WebGrid>
            </div>
            <!-- 개발자 수정 영역 3 - Grid 추가 영역 End -->
        
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>