<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="mail_detail.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.mail_detail" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">

</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="보낸편지함" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <div class="board-list gm-table data-table read-type">
            <table>
                <colgroup>
                    <col width="12%">
                    <col width="31%">
                    <col width="12%">
                    <col width="*">
                    <col width="12%">
                    <col width="31%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblSubjectTitle" runat="server" Text="제목" meta:resourcekey="lblSubjectTitle" />
                    </th>
                    <td colspan="5">
                        <asp:Label ID="lblSubject" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblFrom" runat="server" Text="발송자" meta:resourcekey="lblFrom" />
                    </th>
                    <td>
                        <asp:Label ID="lblFromName" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblMailaddress" runat="server" Text="발송자 메일 주소" meta:resourcekey="lblMailaddress" />
                    </th>
                    <td>
                        <asp:Label ID="lblFromEMail" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblSent" runat="server" Text="발송일자" meta:resourcekey="lblSent" />
                    </th>
                    <td>
                        <asp:Label ID="lblSendDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblTo" runat="server" Text="수신자" meta:resourcekey="lblTo" />
                    </th>
                    <td colspan="3">
                        <asp:Label ID="lblToEmail" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblClassification" runat="server" Text="메일구분" meta:resourcekey="lblClassification" />
                    </th>
                    <td>
                        <asp:Label ID="lblGubun" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblAttachFile" runat="server" Text="첨부파일" meta:resourcekey="lblAttachFile" />
                    </th>
                    <td colspan="5">

                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                            <tr runat="server" id="TRDown1" visible="false">
                                <td>
                                    <asp:Label ID="lblDown1" runat="server" />&nbsp
                                    <asp:Button ID="btnDown1" CssClass="button-default" Text="Save" OnClick="btnDown_Click" runat="server" meta:resourcekey="btnDownResource" />
                                </td>
                            </tr>
                            <tr id="TRDown2" runat="server"  visible="false">
                                <td>
                                    <asp:Label ID="lblDown2" runat="server" />&nbsp
                                    <asp:Button ID="btnDown2" CssClass="button-default" Text="Save" OnClick="btnDown_Click" runat="server" meta:resourcekey="btnDownResource" />
                                </td>
                            </tr>
                            <tr id="TRDown3" runat="server" visible="false">
                                <td>
                                    <asp:Label ID="lblDown3" runat="server" />&nbsp
                                    <asp:Button ID="btnDown3" CssClass="button-default" Text="Save" OnClick="btnDown_Click" runat="server" meta:resourcekey="btnDownResource" />
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <div class="read-content">
                            <asp:Label ID="txtContent" runat="server" Height="165px" TextMode="MultiLine" ReadOnly="true" />
                        </div>
                    </td>
                </tr>
                </tbody>
            </table>
        </div>

        <div class="button-box right">
            <asp:Button ID="btnNew" runat="server" Text="New" CssClass="button-default" OnClick="btnNew_Click" Visible="False" meta:resourcekey="btnNewResource" />
            <asp:Button ID="btnList" runat="server" Text="List" CssClass="button-default blue" OnClick="btnList_Click" meta:resourcekey="btnListResource" />
        </div>
    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>