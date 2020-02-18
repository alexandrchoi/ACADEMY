<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="sms_detail.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.sms_detail" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.BASE" Assembly="CLT.WEB.UI.COMMON.BASE" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">

    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">

        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="SMS 전송결과/예약조회" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p></p>-->

        <div class="message-box default">
            <asp:Literal ID="txtContent" runat="server" />
        </div>

        <div class="gm-table data-table list-type" style="min-width:600px">

            <C1WebGrid:C1WebGrid ID="C1WebGrid1" runat="server" AllowSorting="True" AllowColSizing="True" AutoGenerateColumns="False" DataKeyField="seq" OnItemCreated="grd_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1BoundColumn DataField="rec_nm" HeaderText="수신자명">
                        <ItemStyle Width="16%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="rec_mobile_phone" HeaderText="수신번호">
                        <ItemStyle Width="19%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="sent_dt" HeaderText="전송일시">
                        <ItemStyle Width="28%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="result" HeaderText="결과">
                        <ItemStyle Width="7%" />
                    </C1WebGrid:C1BoundColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle  Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>

        </div>

    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>
