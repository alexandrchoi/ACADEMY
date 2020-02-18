<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="assess_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.assess_edit" Culture="auto" UICulture="auto" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">

    <script type="text/javascript" language="javascript">
    function fnValidation()
        {
            if(isEmpty(document.getElementById('<%=ddlClassification.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblClassification.Text }, new string[] { lblClassification.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlScore.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblScore.Text }, new string[] { lblScore.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlLang.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblLang.Text }, new string[] { lblLang.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlGroup.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblGroup.Text }, new string[] { lblGroup.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlField.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblField.Text }, new string[] { lblField.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtQuestion.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblQuestion.Text }, new string[] { lblQuestion.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtExplan.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblExplan.Text }, new string[] { lblExplan.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

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
        <div class="notice-text right"><asp:Label ID="lblRequired" runat="server" Text="필수입력항목" meta:resourcekey="lblRequired" /><span class="required"></span></div>
        <div class="gm-table data-table read-type">
                
            <table>
                <colgroup>
                    <col width="20%">
                    <col width="80%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row" >
                        <!-- 분류 -->
                        <asp:Label ID="lblClassification" runat="server" meta:resourcekey="lblClassification" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlClassification" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <!-- 배점 -->
                        <asp:Label ID="lblScore" runat="server" meta:resourcekey="lblScore" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlScore" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <!-- 언어 -->
                        <asp:Label ID="lblLang" runat="server" meta:resourcekey="lblLang" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlLang" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <!-- 과정그룹-->
                        <asp:Label ID="lblGroup" runat="server" meta:resourcekey="lblGroup" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlGroup" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <!-- 과정분야 -->
                        <asp:Label ID="lblField" runat="server" meta:resourcekey="lblField" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlField" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <!-- 과정 [ 2012.04.25 add]-->
                        <asp:Label ID="lblCourseNm" runat="server" meta:resourcekey="lblCourseNm" />
                    </th>
                    <td>
                        <input type="text" id="txtCourseNM" runat="server" readonly="readonly" style="width:35%;" />
                        <input type="text" id="txtCourseID" runat="server" readonly="readonly" style="width:15%;" />
                        <a href="#" class="button-board-search" onclick="openPopWindow('/common/course_pop.aspx?opener_textbox_id=<%=txtCourseID.ClientID %>&opener_textbox_nm=<%=txtCourseNM.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>', 'course_pop_win', '600', '650');"></a>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnAddCourse" runat="server" Text="Add" CssClass="button-icon plus" OnClick="btnAddCourse_Click" />
                        <asp:Button ID="btnRemoveCourse" runat="server" Text="Remove" CssClass="button-icon ex" OnClick="btnRemoveCourse_Click" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                    </th>
                    <td>
                        <asp:ListBox ID="lstCourseItemList" Height ="50px" Width = "100%" SelectionMode="Single" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <!-- 과목 [ 2012.04.25 add] -->
                        <asp:Label ID="lblSubjectNm" runat="server" meta:resourcekey="lblSubjectNm" />
                    </th>
                    <td>
                        <input type="text" id="txtSubjectNm" runat="server" readonly="readonly" style="width:35%;" />
                        <input type="text" id="txtSubjectId" runat="server" readonly="readonly" style="width:15%;"  />
                        <a href="#" class="button-board-search" onclick="openPopWindow('/curr/course_pop_subject.aspx?subject_id=<%=txtSubjectId.ClientID %>&subject_nm=<%=txtSubjectNm.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>', 'course_pop_subject', '1024', '790');"></a>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnAddSubject" runat="server" Text="Add" CssClass="button-icon plus" OnClick="btnAddSubject_Click" />
                        <asp:Button ID="btnRemoveSubject" runat="server" Text="Remove" CssClass="button-icon ex" OnClick="btnRemoveSubject_Click" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                    </th>
                    <td>
                        <asp:ListBox ID="lstSubjectItemList" Height ="50px" Width = "100%" SelectionMode="Single" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <!-- 시험유형 -->
                        <asp:Label ID="lblExamType" runat="server" meta:resourcekey="lblExamType" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:RadioButtonList ID="rdoExamType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" Width="100%"
                            OnSelectedIndexChanged="rdoExamType_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="1"> 주관식 &nbsp; &nbsp;  </asp:ListItem>
                            <asp:ListItem Value="2"> O,X &nbsp; &nbsp;  </asp:ListItem>
                            <asp:ListItem Value="3"> 다중선택 &nbsp; &nbsp;  </asp:ListItem>
                            <asp:ListItem Value="4"> 단답형 &nbsp; &nbsp;  </asp:ListItem>
                            <asp:ListItem Value="6"> 4지선다 </asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <!-- 질문 -->
                        <asp:Label ID="lblQuestion" runat="server" meta:resourcekey="lblQuestion" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtQuestion" Width = "100%" Height = "49px" runat="server" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <!-- ANSWER, EXAMPLE -->
                        <asp:Label ID="lblAnswer" runat="server" meta:resourcekey="lblAnswer" />
                        <span class="required"></span>
                        <asp:DataList ID="dtlQ" runat="server" ShowFooter="False" ShowHeader="False"
                            ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px" ItemStyle-HorizontalAlign="Center"
                            HorizontalAlign="Center" BorderStyle="None" CellPadding="0" CellSpacing="0" BorderWidth="0" >
                            <ItemTemplate>
                                <div align="center">
                                Q<%# Eval("num") %>
                                <asp:CheckBox ID="chkQ" runat="server" Checked='<%# Eval("check") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                    </th>
                    <td>
                        <br />
                        <asp:DataList ID="dtlExample" runat="server" ShowFooter="False" ShowHeader="False" 
                            ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px" ItemStyle-HorizontalAlign="Center"
                            HorizontalAlign="Center" BorderStyle="None" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%" >
                            <ItemTemplate>
                                <asp:TextBox ID="txtExample" runat="server" Height="25px" />
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <!-- 해설 -->
                        <asp:Label ID="lblExplan" runat="server" meta:resourcekey="lblExplan" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtExplan" Width = "100%" Height = "50px" runat="server" TextMode="MultiLine" />
                    </td>
                </tr>
                </tbody>
            </table>
        
        </div>


        <!-- 하단 버튼-->
        <div class="button-group center">  
            <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="button-main-rnd lg blue" OnClick="btnSend_Click" OnClientClick="return fnValidation();"  />
            <asp:Button ID="btnTemp" runat="server" Text="Temp Save" CssClass="button-main-rnd lg" OnClick="btnSend_Click" OnClientClick="return fnValidation();" />
        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>
