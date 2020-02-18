<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="study.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.study" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.FX.UTIL" Assembly="CLT.WEB.UI.FX.UTIL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    
   <script runat="server">protected void TreeView1_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e) { foo(e.Node, e.Node.Checked); }private void foo(TreeNode e, bool check) { e.Checked = check; foreach (TreeNode t in e.ChildNodes) { foo(t, check); } }</script>
 
   <script>function foo(){var o = window.event.srcElement;if (o.tagName == "INPUT" && o.type == "checkbox"){o.checked = o.defaultChecked;} }</script>

    <script type="text/javascript" language="javascript">
        function flash(value){
            tdStatus = document.getElementById("tdFlash");
            //pnlStatus = document.getElementById("pnlFlash");
            //alert(value); 
            
            var FlashVar; 
            
            //FlashVar = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="'+width+'" height="'+height+'">'; 
            FlashVar = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="98%" height="98%">'; 
            FlashVar += '<param name="movie" value="'+value+'">'; 
            FlashVar += '<param name="quality" value="high">'; 
            FlashVar += '<param name="wmode" value="transparent">'; 
            FlashVar += '<param name="menu" value="false">'; 
            //FlashVar += '<embed src="'+value+'" width="'+width+'" height="'+height+'" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" wmode="transparent" menu="false"></embed>'; 
            FlashVar += '<embed src="'+value+'" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" wmode="transparent" menu="false"></embed>'; 
            FlashVar += '</object>'; 
                                       
//	        document.writeln('<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="'+width+'" height="'+height+'">');
//	        document.writeln('<param name="movie" value="'+value+'">');
//	        document.writeln('<param name="quality" value="high">');
//	        document.writeln('<param name="wmode" value="transparent">');
//	        document.writeln('<param name="menu" value="false">');
//	        document.writeln('<embed src="'+value+'" width="'+width+'" height="'+height+'" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" wmode="transparent" menu="false"></embed>');
//	        document.writeln('</object>');
	        
	        tdStatus.innerHTML = FlashVar; 
	        //pnlStatus.innerHTML = FlashVar; 
        }
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
            <asp:Button ID="btnNotice" runat="server" Text="Notice" CssClass="button-default blue" OnClick="btnNotice_OnClick" meta:resourcekey="btnNoticeResource" />
            <asp:Button ID="btnData" runat="server" Text="Learning Data" CssClass="button-default" OnClick="btnData_OnClick" meta:resourcekey="btnDataResource" />
            <asp:Button ID="btnSurvey" runat="server" Text="Survey" CssClass="button-default" OnClick="btnSurvey_OnClick" meta:resourcekey="btnSurveyResource" />
            <asp:Button ID="btnExam" runat="server" Text="Exam" CssClass="button-default" OnClick="btnExam_OnClick" meta:resourcekey="btnExamResource" />
        </div>

        <table>
            <tr>
                <td width="25%">
                    <asp:Panel ID="EmpresasPanel" runat="server" Width="220px" Height="450px" ScrollBars="Auto" BorderColor="#E0E0E0" BorderStyle="Solid" BorderWidth="1px">
                        <asp:TreeView ID="TreeView" Width="200px" ShowCheckBoxes= "All" onclick="foo()" Height="430px" runat="server" BorderStyle="solid" BorderWidth="0" BorderColor="#dcdcdc" OnTreeNodeCheckChanged="TreeView_OnTreeNodeCheckChanged" OnSelectedNodeChanged="TreeView_SelectedNodeChanged"  AutoGenerateDataBindings="False" NodeWrap="True" >        
                            <SelectedNodeStyle Font-Bold="True" /> 
                            <HoverNodeStyle BackColor="#E0E0E0" /> 
                            </asp:TreeView>
                        </asp:Panel>
                        <asp:Panel ID="pnlText" runat = "server" Width="220px" Height="70px" ScrollBars="Auto"  BorderColor="#E0E0E0" BorderStyle="Solid" BorderWidth="1px"> 
                        <asp:DataList ID="dtlText" runat="server" Width="100%"  ShowFooter="False" ShowHeader="False" BorderStyle="None">
                                <ItemTemplate>                                                
                                    <asp:Button ID="btnText" runat="server" Text ="Down" CssClass="btn_save" OnClick="btnDown_Click" />
                                    <asp:Label ID="lblTextbookNm" runat="server" Style="height: 20px;padding-top: 7px" Text='<%# Eval("textbooknm") %>'></asp:Label>
                                    <asp:Label ID="lblTextbookid" runat="server" Visible="false" Text='<%# Eval("textbookid") %>'></asp:Label> 
                                    <asp:Label ID="lblTextbookFileNm" runat="server" Visible="false" Text='<%# Eval("textbookfilenm") %>'></asp:Label> 
                                </ItemTemplate>
                            </asp:DataList>                                           
                        </asp:Panel>
                </td>
                <td id="tdFlash" style="width:75%; height:100%; vertical-align:middle">
                    <!--<asp:Panel ID="pnlFlash" runat="server" Width="100%" Height="100%" ScrollBars="Auto" BorderColor="#E0E0E0" BorderStyle="Solid" BorderWidth="1px">
                        </asp:Panel>-->
                </td>
            </tr>
        </table>
        <!-- 버튼-->
        <div class="button-group center">
            
            <asp:Button ID="btnPrevious" runat="server" Text="Previous" CssClass="button-main-rnd lg" OnClick="btnPrev_Click" />
            <asp:TextBox ID="txtPage" runat = "server" ReadOnly ="true" />
            <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="button-main-rnd lg blue" OnClick="btnNext_Click" />    

        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>