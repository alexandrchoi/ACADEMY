<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="course_subject_sort.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.course_subject_sort" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">

    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p><asp:Label ID="lbl0" runat="server" meta:resourcekey="lblGuide0" Text="" />
           <asp:Label ID="lbl1" runat="server" meta:resourcekey="lblGuide1" Text="" /></p>-->


        <!-- 검색 -->
        <!--div class="message-box default center">
        </div>-->


        <!-- 내용-->
        <div class="gm-table data-table read-type">
                
            <table>
                <colgroup>
                    <col width="20%">
                    <col width="80%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                    </th>
                    <td>
                        <!-- SUBJECT item 선택 -->
                        <asp:Label ID="lblItems" runat="server" Text ="Items" /> &nbsp;
                        <asp:Label ID="lblItemsCount" runat="server" Font-Bold="True" ForeColor="Blue" />
                        <asp:Button ID="btnUp" runat="server" Text="▲" BackColor="White" Font-Bold="true" Font-Size="Large" OnClick="btnUp_Click" />
                        <asp:Button ID="btnDown" runat="server" Text="▼" BackColor="White" Font-Bold="true" Font-Size="Large" OnClick="btnDown_Click" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblItemsList" runat="server" meta:resourcekey="lblItemsList" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:ListBox ID="lstItemList" Height="300px" Width="100%" SelectionMode="Single" runat="server" />
                    </td>
                </tr>
                </tbody>
            </table>
        
        </div>


        <!-- 하단 버튼-->
        <div class="button-group center">
            <asp:Button ID="btnTemp" runat="server" Text="Temp Save" CssClass="button-main-rnd lg blue" OnClick="btnTemp_Click" />
        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>

