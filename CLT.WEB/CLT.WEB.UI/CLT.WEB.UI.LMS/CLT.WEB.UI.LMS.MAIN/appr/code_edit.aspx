<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="code_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPR.code_edit" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script language="javascript" type="text/javascript">
        function fnValidateForm()
        {   
            if(isEmpty(document.getElementById('<%=txtGrade.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblGrade.Text }, new string[] { lblGrade.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(!isUpperCase(document.getElementById('<%=txtGrade.ClientID %>'),'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblGrade.Text }, new string[] { lblGrade.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtGrade.ClientID %>'), 1, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A008", new string[] { lblGrade.Text, "1"}, new string[] { lblGrade.Text,"1"}, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtGradeNM.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblGradeNM.Text }, new string[] { lblGradeNM.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtGradeNM.ClientID %>'), 30, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblGradeNM.Text, "10", "20" }, new string[] { lblGradeNM.Text,"10", "20" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtScore.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblScore.Text }, new string[] { lblScore.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(!isNumber(document.getElementById('<%=txtScore.ClientID %>'),'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A108", new string[] { lblScore.Text }, new string[] { lblScore.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtGradeDesc.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblGradeDesc.Text }, new string[] { lblGradeDesc.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
            return true;
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
                    <col width="30%">
                    <col width="70%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblGrade" runat="server" Text="" meta:resourcekey="lblGrade" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtGrade" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblGradeNM" runat="server" Text="" meta:resourcekey="lblGradeNM" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtGradeNM" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblScore" runat="server" Text="" meta:resourcekey="lblScore" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtScore" runat="server" CssClass="w100per" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblGradeDesc" runat="server" Text="" meta:resourcekey="lblGradeDesc" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtGradeDesc" runat="server" CssClass="w100per" TextMode="MultiLine" Height="100px" />
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