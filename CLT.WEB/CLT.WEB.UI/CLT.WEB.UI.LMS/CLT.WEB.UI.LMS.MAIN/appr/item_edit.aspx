<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="item_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPR.item_edit" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script language="javascript" type="text/javascript">
        function fnValidateForm()
        {   
            if(isEmpty(document.getElementById('<%=txtAppBaseDt.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAppBaseDt.Text }, new string[] { lblAppBaseDt.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(!isDateChk(document.getElementById('<%=txtAppBaseDt.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblAppBaseDt.Text }, new string[] { lblAppBaseDt.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; 
            if(isEmpty(document.getElementById('<%=ddlStepGu.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblStepGu.Text }, new string[] { lblStepGu.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlAppDutyStep.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAppDutyStep.Text }, new string[] { lblAppDutyStep.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlVslType.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblVslType.Text }, new string[] { lblVslType.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtAppItemSeq.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAppItemSeq.Text }, new string[] { lblAppItemSeq.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(!isNumber(document.getElementById('<%=txtAppItemSeq.ClientID %>'),'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A108", new string[] { lblAppItemSeq.Text }, new string[] { lblAppItemSeq.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtAppItemSeq.ClientID %>'),3,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblAppItemSeq.Text, "1", "2" }, new string[] { lblAppItemSeq.Text, "1", "2" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
            if(isEmpty(document.getElementById('<%=txtAppItemNm.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAppItemNm.Text }, new string[] { lblAppItemNm.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtAppItemNm.ClientID %>'),90, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblAppItemNm.Text, "30", "60" }, new string[] { lblAppItemNm.Text, "30", "60" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtAppItemNmEng.ClientID %>'),90, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblAppItemNmEng.Text, "30", "60" }, new string[] { lblAppItemNmEng.Text, "30", "60" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
            if(isEmpty(document.getElementById('<%=txtAppItemDesc.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAppItemDesc.Text }, new string[] { lblAppItemDesc.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtAppCaseSeq.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAppCaseSeq.Text }, new string[] { lblAppCaseSeq.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(!isNumber(document.getElementById('<%=txtAppCaseSeq.ClientID %>'),'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A108", new string[] { lblAppCaseSeq.Text }, new string[] { lblAppCaseSeq.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtAppCaseSeq.ClientID %>'),3, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblAppCaseSeq.Text, "1", "2" }, new string[] { lblAppCaseSeq.Text,"1", "2" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtAppCaseDesc.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAppCaseDesc.Text }, new string[] { lblAppCaseDesc.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
            return true;
        }
    
        function GoCourseForm(opener_textbox_id, opener_textbox_nm, course_type)
        {	
            openPopWindow('/common/course_pop.aspx?opener_textbox_id='+opener_textbox_id+'&opener_textbox_nm='+opener_textbox_nm+'&course_type='+course_type+'&MenuCode=<%=Session["MENU_CODE"]%>', "CoursePOP", "600", "721", "status=no");
	        return false;
        }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <asp:Label ID="lblRegNo" runat="server" Text="제목" meta:resourcekey="lblRegNo" Visible="false" />
    <asp:TextBox ID="txtRegNo" runat="server" Visible="false" />

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="역량평가항목" meta:resourcekey="lblMenuTitle" /></h3>
    
        
        <div class="notice-text right"><asp:Label ID="lblRequired" runat="server" Text="필수입력항목" meta:resourcekey="lblRequired" /><span class="required"></span></div>

        <div class="gm-table data-table read-type">
                
            <table>
                <colgroup>
                    <col width="25%">
                    <col width="25%">
                    <col width="25%">
                    <col width="25%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblAppBaseDt" runat="server" Text="회사코드" meta:resourcekey="lblAppBaseDt" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtAppBaseDt" runat="server" CssClass="datepick w180" />
                        <asp:TextBox ID="txtAppItemNo" runat="server" CssClass="datepick" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblStepGu" runat="server" Text="" meta:resourcekey="lblStepGu" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlStepGu" runat="server" OnSelectedIndexChanged="ddlStepGu_SelectedIndexChanged" AutoPostBack="True" CssClass="w100per" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblAppDutyStep" runat="server" Text="" meta:resourcekey="lblAppDutyStep" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlAppDutyStep" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblVslType" runat="server" Text="" meta:resourcekey="lblVslType" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlVslType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlVslType_SelectedIndexChanged" />
                        <asp:DropDownList ID="ddlVslTypeC" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblAppItemSeq" runat="server" Text="" meta:resourcekey="lblAppItemSeq" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtAppItemSeq" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblAppItemNm" runat="server" Text="" meta:resourcekey="lblAppItemNm" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtAppItemNm" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblAppItemNmEng" runat="server" Text="" meta:resourcekey="lblAppItemNmEng" />
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtAppItemNmEng" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblAppItemDesc" runat="server" Text="" meta:resourcekey="lblAppItemDesc" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtAppItemDesc" runat="server" CssClass="w100per" TextMode="MultiLine" Height="50px" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblAppItemDescEng" runat="server" Text="" meta:resourcekey="lblAppItemDescEng" />
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtAppItemDescEng" runat="server" CssClass="w100per" TextMode="MultiLine" Height="50px" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblAppCaseSeq" runat="server" Text="" meta:resourcekey="lblAppCaseSeq" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtAppCaseSeq" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblAppCaseDesc" runat="server" Text="" meta:resourcekey="lblAppCaseDesc" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtAppCaseDesc" runat="server" CssClass="w100per" TextMode="MultiLine" Height="50px" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblAppCaseDescEng" runat="server" Text="" meta:resourcekey="lblAppCaseDescEng" />
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtAppCaseDescEng" runat="server" CssClass="w100per" TextMode="MultiLine" Height="50px" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblCourseOJT" runat="server" Text="" meta:resourcekey="lblCourseOJT" />
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtCourseOJT" runat="server" CssClass="w50per" ReadOnly="true" />
                        <asp:TextBox ID="hdnCourseOJT" runat="server" CssClass="w30per" ReadOnly="true" />
                        <a href="#" class="button-board-search" onclick="GoCourseForm('<%=hdnCourseOJT.ClientID %>','<%=txtCourseOJT.ClientID %>','000005');">Search</a>
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblCourseLMS" runat="server" Text="" meta:resourcekey="lblCourseLMS" />
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtCourseLMS" runat="server" CssClass="w50per" ReadOnly="true" />
                        <asp:TextBox ID="hdnCourseLMS" runat="server" CssClass="w30per" ReadOnly="true" /><asp:HiddenField ID="HiddenField2" runat="server" />
                        <a href="#" class="button-board-search" onclick="GoCourseForm('<%=hdnCourseLMS.ClientID %>','<%=txtCourseLMS.ClientID %>','');">Search</a>
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblCourseETC" runat="server" Text="" meta:resourcekey="lblCourseETC" />
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtCourseETC" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                </tbody>
            </table>
        
        </div>

        <!-- 버튼 -->
        <div class="button-group center">
            <asp:Button ID="btnSend" runat="server" Text="Save" CssClass="button-main-rnd blue lg" OnClick="btnSend_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnSendResource" />
        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>