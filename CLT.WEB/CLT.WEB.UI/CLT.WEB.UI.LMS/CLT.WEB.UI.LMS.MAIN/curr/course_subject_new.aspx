<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="course_subject_new.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.course_subject_new" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">
        function setLoad()
        {
            lstItems   = document.getElementById("<%= lstItemList.ClientID %>");
            hidLstItems = document.getElementById("<%= HidLstItems.ClientID %>");
        }
        function setDispDetail(gubun, datas)
        {
            hidLstItems.value = datas[0];
            //hidUseYN, hidLstItems  값을 화면에 Setting해줌
            <%= Page.GetPostBackEventReference(LnkBtnSetItem) %>;
        }
        function setSubject(datas)
        {
            <%= Page.GetPostBackEventReference(LnkBtnSubjectidChange) %>;
        }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <!-- 데이터를 선택해서 가져왔을때 필요한 값 저장 -->
    <asp:HiddenField runat="server" ID="HidLstItems" />
    <!-- Pop Up 창에서 선택한 데이터를 ListBox에 추가하여 보여줌-->
    <asp:LinkButton ID="LnkBtnSetItem" runat="server" OnClick="LnkBtnSetItem_Click" />
    <asp:LinkButton ID="LnkBtnSubjectidChange" runat="server" OnClick="LnkBtnSubjectidChange_Click" />
    

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p><asp:Label ID="lbl0" runat="server" meta:resourcekey="lblGuide0" Text="" />
           <asp:Label ID="lbl1" runat="server" meta:resourcekey="lblGuide1" Text="" /></p>-->


        <!-- 검색 -->
        <!--div class="message-box default center">
        </div>-->


        <!-- 내용-->
        <div class="notice-text right"><asp:Label ID="lblRequired" runat="server" Text="필수입력항목" meta:resourcekey="lblRequired" /><span class="required"></span></div>
        <div class="gm-table data-table read-type">
                
            <table>
                <colgroup>
                    <col width="20%">
                    <col width="80%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row" rowspan="2">
                        <asp:Label ID="lblCourseNm" runat="server" meta:resourcekey="lblCourseNm" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <input type="text" id="txtSubjectNm" runat="server" readonly="readonly" class="w50per" />
                        <input type="text" id="txtSubjectId" runat="server" readonly="readonly" class="w30per"  />
                        <a href="#" class="button-board-search" onclick="openPopWindow('/curr/course_pop_subject.aspx?subject_id=<%=txtSubjectId.ClientID %>&subject_nm=<%=txtSubjectNm.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>', 'course_pop_subject', '1024', '790');"></a>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        <asp:Label ID="lblItems" runat="server" Text ="Items" /> &nbsp;
                        <asp:Label ID="lblItemsCount" runat="server" Font-Bold="True" ForeColor="Blue" />
                        <asp:Button ID="btnUp" runat="server" Text="위로 이동" CssClass="button-up" OnClick="btnUp_Click" />
                        <asp:Button ID="btnDown" runat="server" Text="아래로 이동" CssClass="button-down" OnClick="btnDown_Click" />
                        <input type="button" value="Add Contents" class="button-default blue" onclick="javascript:openPopWindow('/curr/course_pop_contents.aspx?MenuCode=<%=Session["MENU_CODE"]%>','course_pop_contents', '1024', '790');" id="btnAdd" />
                        <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="button-default" OnClick="btnRemove_Click" />
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
