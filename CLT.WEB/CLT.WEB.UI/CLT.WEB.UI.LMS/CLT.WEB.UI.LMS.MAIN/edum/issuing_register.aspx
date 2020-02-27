<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="issuing_register.aspx.cs" Inherits="CLT.WEB.UI.LMS.EDUM.issuing_register" Culture="auto" UICulture="auto" MasterPageFile="/MasterWin.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">
    function fnValidateForm()
    {   
        //if(isEmpty(document.getElementById('<%=lblUserId.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblUserId.Text }, new string[] { lblUserId.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
        if(isEmpty(document.getElementById('<%=txtSTART_DATE.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCourseBeginDt.Text }, new string[] { lblCourseBeginDt.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(!isDateChk(document.getElementById('<%=txtSTART_DATE.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblCourseBeginDt.Text }, new string[] { lblCourseBeginDt.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; 
        if(isEmpty(document.getElementById('<%=txtEND_DATE.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCourseEndDt.Text }, new string[] { lblCourseEndDt.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(!isDateChk(document.getElementById('<%=txtEND_DATE.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblCourseEndDt.Text }, new string[] { lblCourseEndDt.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; 
        if(!compareDate(document.getElementById('<%=txtSTART_DATE.ClientID %>'),document.getElementById('<%=txtEND_DATE.ClientID %>'))) return false;
        
        if(isEmpty(document.getElementById('<%=txtCourseNM.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCourseNM.Text }, new string[] { lblCourseNM.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtCourseNM.ClientID %>'),300, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblCourseNM.Text, "100", "200" }, new string[] { lblCourseNM.Text, "100", "200" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=ddlInstitution.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblLearningInstitution.Text }, new string[] { lblLearningInstitution.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
        return true;
    }
    
    function GoUserForm()
    {	
        //Inquiry^VessleType^ShipCode^Rank
        openPopWindow('/appr/competency_user_list.aspx?search=&bind_control=<%=lblUserId.ClientID %>^<%=lblUserNMKor.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>', "UserListForm", "820", "860", "status=no");
	    return false;
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
        <div class="notice-text right"><asp:Label ID="lblRequired" runat="server" Text="필수입력항목" meta:resourcekey="lblRequired" /><span class="required"></span></div>
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
                    <th scope="row" >
                        <asp:Label ID="titleUserId" runat="server" meta:resourcekey="lblUserId" Text="사번" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:Label ID="lblUserId" runat="server" />
                        <!--a href="#" class="button-board-search" onclick="javascript:return GoUserForm();" style="visibility:hidden"></a-->
                    </td>
                    <th scope="row" >
                        <asp:Label ID="titleUserNMKor" runat="server" meta:resourcekey="lblUserNMKor" Text="이름" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:Label ID="lblUserNMKor" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <asp:Label ID="lblCourseBeginDt" runat="server" meta:resourcekey="lblCourseBeginDt" Text="교육시작일" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtSTART_DATE" runat="server" MaxLength="10" CssClass="datepick w180" />
                    </td>
                    <th scope="row" >
                        <asp:Label ID="lblCourseEndDt" runat="server" meta:resourcekey="lblCourseEndDt" Text="교육종료일" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtEND_DATE" runat="server" MaxLength="10" CssClass="datepick w180" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <asp:Label ID="lblCourseNM" runat="server" meta:resourcekey="lblCourseNM" Text="과정명" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <input type="text" id="txtCourseNM" runat="server" readonly="readonly" style="width:60%; background-color:#dcdcdc" />
                        <input type="text" id="txtCourseID" runat="server" readonly="readonly" style="width:20%;  background-color:#dcdcdc" />
                        <a href="#" class="button-board-search" onclick="openPopWindow('/common/course_pop.aspx?opener_textbox_id=<%=txtCourseID.ClientID %>&opener_textbox_nm=<%=txtCourseNM.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>', 'course_pop_win', '600', '650');"></a>
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <asp:Label ID="lblLearningInstitution" runat="server" meta:resourcekey="lblLearningInstitution" Text="교육기관" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlInstitution" runat="server" />
                    </td>
                </tr>
                </tbody>
            </table>
        
        </div>

        <!-- 하단 버튼-->
        <div class="button-group center">
            <asp:Button ID="btnSave" runat="server" CssClass="button-main-rnd lg blue" OnClick="btnSave_Click" OnClientClick="return fnValidateForm();" Text="Save" meta:resourcekey="btnSaveResource" /> 
        </div>
    </div>

</asp:Content>
