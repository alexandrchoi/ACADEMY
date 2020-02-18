<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="CLT.WEB.UI.LMS.MAIN.error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>▒ Error Information ▒</title>
    <link href="/css/layout.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="/scripts/lmsCommon.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <table id="td1" runat="server" border="0" cellpadding="0" cellspacing="0" style="height:270px; width:302px">
            <tr align="left">
               <td style="width:100%; height:50px">
                <img src="/asset/images/error.png" width="302px" height="50px" alt="" />
               </td>
            </tr>
            <tr>
               <td style="width:100%;">
                <asp:TextBox ID="txtError" runat="server" Width = "100%" Height ="165px" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                <asp:TextBox ID="txtError2" runat="server" Width = "100%" Height = "200px" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
               </td> 
            </tr>
            <tr align="right">
               <td style="width:100%; height:30px">
                <asp:Button ID="btnDetail" OnClick="Button_OnClick" Text="Detail" CssClass="btn_search" runat="server" />
                <input type="button" value="Close" class="btn_close" onclick="javascript:self.close();" />
               </td>
            </tr>
        </table>
    </form>
</body>
</html>
