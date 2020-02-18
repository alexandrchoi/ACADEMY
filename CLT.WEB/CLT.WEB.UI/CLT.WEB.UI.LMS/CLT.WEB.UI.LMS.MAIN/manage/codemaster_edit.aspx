<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="codemaster_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.MANAGE.codemaster_edit" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script language="javascript" type="text/javascript">
    function fnValidateForm()
    {
        // 필수입력값 체크
        if (isEmpty(document.getElementById('<%=txtMasterCode_NM.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblMasterCode_NM.Text }, new string[] { lblMasterCode_NM.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // 코드명을 입력해 주세요!
        if (isEmpty(document.getElementById('<%=txtCodeMaster_ENM.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCodeMaster_ENM.Text }, new string[] { lblCodeMaster_ENM.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // 영문명을 입력해 주세요!
        if (isEmpty(document.getElementById('<%=txtCodeMaster_KNM.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCodeMaster_KNM.Text }, new string[] { lblCodeMaster_KNM.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // 한글명을 입력해 주세요!
        
        // 길이체크
        if(isMaxLenth(document.getElementById('<%=txtMasterCode.ClientID %>'),4,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A007", new string[] { lblMasterCode.Text,"4" }, new string[] { lblMasterCode.Text,"4" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtMasterCode_NM.ClientID %>'),500,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblMasterCode_NM.Text,"500","300" }, new string[] { lblMasterCode_NM.Text,"500","300" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtCodeMaster_ENM.ClientID %>'),60,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblCodeMaster_ENM.Text,"60","40" }, new string[] { lblCodeMaster_ENM.Text,"60","40" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtCodeMaster_KNM.ClientID %>'),60,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblCodeMaster_KNM.Text,"60","40" }, new string[] { lblCodeMaster_KNM.Text,"60","40" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        
        return true;
    }
    </script>
   <%-- <script language="javascript" type="text/javascript">
     function getFileNM(filepath){
      var xControl = document.getElementById("<%= txtFileNM.ClientID %>");
      var xFileNameSector = filepath.split("\\");
      var xFileName = xFileNameSector[xFileNameSector.length - 1];
      xControl.value = xFileName;
     }
    </script>--%> 
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container">
        <h3><asp:Label ID="lblMenuTitle" runat="server" meta:resourcekey="lblMenuTitle" /></h3>
    
        
        <div class="notice-text right"><asp:Label ID="lblRequired" runat="server" Text="필수입력항목" meta:resourcekey="lblRequired" /><span class="required"></span></div>

        <div class="gm-table data-table read-type">
                
            <table>
                <caption>표정보 - 번호, 제목 등을 제공합니다.</caption>
                <colgroup>
                    <col width="30%">
                    <col width="*">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblMasterCode" runat="server" Text="코드" meta:resourcekey="lblMasterCode" />
                        <span class="required"></span>

                    </th>
                    <td>
                        <asp:TextBox ID="txtMasterCode" runat="server" MaxLength="200" ReadOnly="True" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblMasterCode_NM" runat="server" Text="코드명" meta:resourcekey="lblMasterCode_NM" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMasterCode_NM" runat="server" MaxLength="200" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblCodeMaster_ENM" runat="server" Text="영문명" meta:resourcekey="lblCodeMaster_ENM" />
                        <span class="required"></span>

                    </th>
                    <td>
                        <asp:TextBox ID="txtCodeMaster_ENM" runat="server" MaxLength="200" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblCodeMaster_KNM" runat="server" Text="한글명" meta:resourcekey="lblCodeMaster_KNM" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtCodeMaster_KNM" runat="server" MaxLength="200" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblUse_yn" runat="server" Text="사용유무" meta:resourcekey="lblUse_yn" />
                    </th>
                    <td>
                        <asp:CheckBox ID="chkUse_yn" runat="server" MaxLength="200" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblUser_Code_YN" runat="server" Text="사용자코드유무" meta:resourcekey="lblUser_Code_YN" />
                    </th>
                    <td>
                        <asp:CheckBox ID="chkUser_Code_YN" runat="server" Checked="false" MaxLength="200" />
                    </td>
                </tr>
                </tbody>
            </table>
        
        </div>

        <!-- 버튼 -->
        <div class="button-group center">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button-default blue" OnClick="btnSend_Click" meta:resourcekey="btnSaveResource" />
        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>