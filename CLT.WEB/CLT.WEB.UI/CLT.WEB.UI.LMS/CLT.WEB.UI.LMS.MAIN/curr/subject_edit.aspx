<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="subject_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.subject_edit" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">
    function fnValidation()
    {
        //null check
        if(isEmpty(document.getElementById('<%=txtSubject.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblSubject.Text }, new string[] { lblSubject.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=ddlClass.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblClass.Text }, new string[] { lblClass.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=ddlType.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblType.Text }, new string[] { lblType.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=ddlLang.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblLang.Text }, new string[] { lblLang.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

        //size check
        if(isMaxLenth(document.getElementById('<%=txtSubject.ClientID %>'), 300, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblSubject.Text, "100", "200" }, new string[] { lblSubject.Text, "100", "200" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtTime.ClientID %>'), 5, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblTime.Text, "5" }, new string[] { lblTime.Text, "5" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtInstructor.ClientID %>'), 80, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblInstructor.Text, "25", "50" }, new string[] { lblInstructor.Text, "25", "50" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtContents.ClientID %>'), 300, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblContents.Text, "100", "200" }, new string[] { lblContents.Text, "100", "200" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtPoint.ClientID %>'), 300, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblPoint.Text, "100", "200" }, new string[] { lblPoint.Text, "100", "200" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

        //number check
        if(isNumber(document.getElementById('<%=txtTime.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblTime.Text }, new string[] { lblTime.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;

        return true;
    }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="Label1" runat="server" meta:resourcekey="lblMenuTitle" /></h3>
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
                        <!--과목명-->
                        <asp:Label ID="lblSubject" runat="server" meta:resourcekey="lblSubject" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtSubject" runat="server" MaxLength="200" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <!--교육시간-->
                        <asp:Label ID="lblTime" runat="server" meta:resourcekey="lblTime" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtTime" runat="server" MaxLength="5" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <!--강사명1-->
                        <asp:Label ID="lblInstructor" runat="server" meta:resourcekey="lblInstructor" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtInstructor" runat="server" MaxLength="50" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <!--강사명2-->
                        <asp:Label ID="lblInstructor1" runat="server" meta:resourcekey="lblInstructor1" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtInstructor1" runat="server" MaxLength="50" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <!--교육목표-->
                        <asp:Label ID="lblPoint" runat="server" meta:resourcekey="lblPoint" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtPoint" runat="server"  MaxLength="200" Height = "50px" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <!--교육내용-->
                        <asp:Label ID="lblContents" runat="server" meta:resourcekey="lblContents" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtContents" runat="server" MaxLength="200" Height ="50px" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <!--분류-->
                        <asp:Label ID="lblClass" runat="server" meta:resourcekey="lblClass" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlClass" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <!--과목유형-->
                        <asp:Label ID="lblType" runat="server" meta:resourcekey="lblType" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <!--과목언어-->
                        <asp:Label ID="lblLang" runat="server" meta:resourcekey="lblLang" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlLang" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <!-- usage -->
                        <asp:Label ID="lblUsage" runat="server" meta:resourcekey="lblUsage" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:RadioButtonList ID="rdoUsage" runat="server" Width="15%" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                            <asp:ListItem Value="N">No</asp:ListItem>
                        </asp:RadioButtonList>
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
