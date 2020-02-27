<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="textbook_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.textbook_edit" 
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">
    function fnValidation()
    {
        //null check
       if(isEmpty(document.getElementById('<%=ddlTextBookType.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblTextBookType.Text }, new string[] { lblTextBookType.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
       if(isEmpty(document.getElementById('<%=ddlCourseGroup.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblCourseGroup.Text }, new string[] { lblCourseGroup.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
       if(isEmpty(document.getElementById('<%=ddlCourseField.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblCourseField.Text }, new string[] { lblCourseField.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
       if(isEmpty(document.getElementById('<%=ddlTextBookLang.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblTextBookLang.Text }, new string[] { lblTextBookLang.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
       if(isEmpty(document.getElementById('<%=txtTextBookNM.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblTextBookNM.Text }, new string[] { lblTextBookNM.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
       if(isEmpty(document.getElementById('<%=txtPublisher.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPublisher.Text }, new string[] { lblPublisher.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
       if(isEmpty(document.getElementById('<%=txtAuthor.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblAuthor.Text }, new string[] { lblAuthor.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
       if(isEmpty(document.getElementById('<%=txtPrice.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblPrice.Text }, new string[] { lblPrice.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
       if(isEmpty(document.getElementById('<%=txtFileNM.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblTextBookFile.Text }, new string[] { lblTextBookFile.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

      //size check
       if(isMaxLenth(document.getElementById('<%=txtTextBookNM.ClientID %>'), 300, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblTextBookNM.Text, "100", "200" }, new string[] { lblTextBookNM.Text, "100", "200" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
       if(isMaxLenth(document.getElementById('<%=txtPublisher.ClientID %>'), 80, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblPublisher.Text, "25", "50" }, new string[] { lblPublisher.Text, "25", "50" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
       if(isMaxLenth(document.getElementById('<%=txtAuthor.ClientID %>'), 80, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblAuthor.Text, "25", "50" }, new string[] { lblAuthor.Text, "25", "50" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
       if(isMaxLenth(document.getElementById('<%=txtPrice.ClientID %>'), 50, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblPrice.Text, "50" }, new string[] { lblPrice.Text, "50" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

      //number check
      if(isNumber(document.getElementById('<%=txtPrice.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblPrice.Text }, new string[] { lblPrice.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;

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
                    <col width="10%">
                    <col width="15%">
                    <col width="10%">
                    <col width="15%">
                    <col width="10%">
                    <col width="15%">
                    <col width="10%">
                    <col width="15%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row" >
                        <asp:Label ID="lblTextBookType" runat="server" meta:resourcekey="lblTextBookType" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlTextBookType" runat="server" />
                    </td>
                    <th scope="row" >
                        <asp:Label ID="lblCourseGroup" runat="server" meta:resourcekey="lblCourseGroup" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlCourseGroup" runat="server" />
                    </td>
                    <th scope="row" >
                        <asp:Label ID="lblCourseField" runat="server" meta:resourcekey="lblCourseField" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlCourseField" runat="server" />
                    </td>
                    <th scope="row" >
                        <asp:Label ID="lblTextBookLang" runat="server" meta:resourcekey="lblTextBookLang" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlTextBookLang" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <asp:Label ID="lblTextBookNM" runat="server" meta:resourcekey="lblTextBookNM" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtTextBookNM" runat="server" MaxLength="200" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblPubDt" runat="server" Text="출판일" meta:resourcekey="lblPubDt" />
                    </th>
                    <td style="text-wrap:none; overflow-wrap:break-word; word-wrap:break-word;">
                        <asp:TextBox ID="txtPubDt" runat="server" MaxLength="10" CssClass="datepick" Width="120" />
                    </td>
                    <th scope="row" >
                        <asp:Label ID="lblPublisher" runat="server" meta:resourcekey="lblPublisher" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtPublisher" runat="server" MaxLength="50" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <asp:Label ID="lblAuthor" runat="server" meta:resourcekey="lblAuthor" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtAuthor" runat="server" MaxLength="50" />
                    </td>
                    <th scope="row" >
                        <asp:Label ID="lblPrice" runat="server" meta:resourcekey="lblPrice" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtPrice" runat="server" MaxLength="50" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <asp:Label ID="lblTextBookIntro" runat="server" meta:resourcekey="lblTextBookIntro" />
                    </th>
                    <td colspan="7">
                        <asp:TextBox ID="txtTextBookIntro" runat="server" MaxLength="500" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <asp:Label ID="lblTextBookDesc" runat="server" meta:resourcekey="lblTextBookDesc" />
                    </th>
                    <td colspan="7">
                        <asp:TextBox ID="txtTextBookDesc" runat="server" MaxLength="500" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" >
                        <asp:Label ID="lblTextBookFile" runat="server" meta:resourcekey="lblTextBookFile" />
                        <span class="required"></span>
                    </th>
                    <td colspan="7">
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
            <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="button-main-rnd lg blue" OnClick="btnSend_Click" OnClientClick="return fnValidation();"  />
            <asp:Button ID="btnTemp" runat="server" Text="Temp Save" CssClass="button-main-rnd lg" OnClick="btnSend_Click" OnClientClick="return fnValidation();" />
        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>
