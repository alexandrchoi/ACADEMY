<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.report" %>

<%@ Register Assembly="C1.Web.C1WebReport.2, Version=2.5.20063.223, Culture=neutral, PublicKeyToken=79882d576c6336da"
    Namespace="C1.Web.C1WebReport" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:C1WebReport ID="C1WebReport1" runat="server">
            <NavigationBar Align="Center" HasGotoPageButton="True" HasExportButton="True" Style-BackColor="#E0E0E0" Style-BorderColor="Silver" Style-BorderStyle="Solid" Style-BorderWidth="1px" Style-Font-Names="Tahoma" Style-Font-Size="10pt" />
            <Cache Enabled="True" />
        </cc1:C1WebReport>    
    </div>
    </form>
</body>
</html>
