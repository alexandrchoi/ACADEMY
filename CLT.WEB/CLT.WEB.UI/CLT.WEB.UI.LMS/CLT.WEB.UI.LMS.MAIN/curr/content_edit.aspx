<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="content_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.content_edit" 
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">
        function fnValidation()
        {
            //null check
            if(isEmpty(document.getElementById('<%=ddlContentsType.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblContentsType.Text }, new string[] { lblContentsType.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlLang.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblLang.Text }, new string[] { lblLang.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtContentsNM.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblContentsNM.Text }, new string[] { lblContentsNM.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtFileNM.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblContentsFile.Text }, new string[] { lblContentsFile.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

            //size check
            if(isMaxLenth(document.getElementById('<%=txtContentsNM.ClientID %>'), 300, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblContentsNM.Text, "100", "200" }, new string[] { lblContentsNM.Text, "100", "200" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtRemark.ClientID %>'), 300, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblRemark.Text, "100", "200" }, new string[] { lblRemark.Text, "100", "200" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtFileNM.ClientID %>'), 200, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblContentsFile.Text, "60", "120" }, new string[] { lblContentsFile.Text, "60", "120" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

            return true;
        }
        
        function setTxtFileNM(filepath)
        {
  
          var xControl = document.getElementById('<%=txtFileNM.ClientID %>');
          var xFileNameSector = filepath.split("\\");
          var xFileName = xFileNameSector[xFileNameSector.length - 1];
          xControl.value = xFileName;
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
                    <col width="30%">
                    <col width="20%">
                    <col width="30%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblContentsType" runat="server" meta:resourcekey="lblContentsType" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlContentsType" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblLang" runat="server" meta:resourcekey="lblLang" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlLang" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblContentsNM" runat="server" meta:resourcekey="lblContentsNM" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtContentsNM" runat="server" MaxLength="200" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblRemark" runat="server" meta:resourcekey="lblRemark" />
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblContentsFile" runat="server" meta:resourcekey="lblContentsFile" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <!-- 파일 첨부 인풋 -->
                        <div class="file-box">
                           <input class="upload-name" value="File Name" disabled="disabled">
                           <label for="<%=FileUpload1.ClientID %>">찾아보기</label>
                           <input type="file" id="FileUpload1" runat="server" class="upload-hidden" onchange="setTxtFileNM(this.value);" />
                       </div>
                        <!--// 파일 첨부 인풋 -->
                        <input type="text" id="txtFileNM" runat="server" style="width:38%; background-color:#dcdcdc" readonly="readonly" />
                    </td>
                </tr>
                </tbody>
            </table>
        
        </div>


        <!-- 하단 버튼-->
        <div class="button-group center">  
            <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="button-main-rnd lg blue" OnClick="btnSend_Click" OnClientClick="return fnValidation();" />
            <asp:Button ID="btnTemp" runat="server" Text="Temp Save" CssClass="button-main-rnd lg" OnClick="btnSend_Click" OnClientClick="return fnValidation();" />
        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>
