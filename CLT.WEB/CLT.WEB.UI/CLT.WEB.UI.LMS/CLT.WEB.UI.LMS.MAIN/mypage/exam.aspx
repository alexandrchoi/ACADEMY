<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="exam.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.exam" 
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    
    <script type="text/javascript" language="javascript">

    </script>

</asp:Content>



<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <!-- 선택된 user -->

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="나의 강의실" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p><asp:Label ID="lbl0" runat="server" meta:resourcekey="lblGuide0" Text="" />
           <asp:Label ID="lbl1" runat="server" meta:resourcekey="lblGuide1" Text="" /></p>-->
    

        <!-- 검색 -->
        <!--div class="message-box default center">
            <asp:Button ID = "btnRetrieve" runat ="server" Text="Search" CssClass="button-board-search" />
        </div>-->
        
        <div class="button-box right">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button-default blue" OnClick="btnSave_Click" meta:resourcekey="btnSaveResource" />
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button-default" OnClick="btnSubmit_Click" meta:resourcekey="btnSubmitResource" />
        </div>
        
        <table>
            <tr style="height:20px">
                <td>
                    <asp:Label ID="lblExamExplain" runat="server" ForeColor="blue" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                        
                <td style ="width:100%;">                                
                    <asp:PlaceHolder ID="ph" runat="server"></asp:PlaceHolder>                               
                </td> 
                           
            </tr>
        </table>
        <!-- 버튼-->
        <div class="button-group center">
            

        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>