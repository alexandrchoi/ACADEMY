<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="qna_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.COMMUNITY.qna_edit" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" src="/asset/smarteditor/js/service/HuskyEZCreator.js"></script>
    <script type="text/javascript" language="javascript">
        function fnValidateForm()
        {
            //  필수 입력값 체크
            if (isEmpty(document.getElementById('<%=txtSubject.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblSubject.Text }, new string[] { lblSubject.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // ID를 입력해 주세요!
            
	        oEditors.getById["<%= txtCONTENT.ClientID %>"].exec("UPDATE_CONTENTS_FIELD", []);	// 에디터의 내용이 textarea에 적용됩니다.
            document.getElementById("<%=txt_content.ClientID %>").value = document.getElementById("<%= txtCONTENT.ClientID %>").value;

            return true;
        }
        // 포스트백상황시 호출
        function setWebControls() {
	        oEditors.getById["<%= txtCONTENT.ClientID %>"].exec("UPDATE_CONTENTS_FIELD", []);	// 에디터의 내용이 textarea에 적용됩니다.
            document.getElementById("<%=txt_content.ClientID %>").value = document.getElementById("<%= txtCONTENT.ClientID %>").value;
            return true;
        }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="Q&A" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board board-write">
        <div class="notice-text right">필수입력<span class="required">필수입력</span></div>
        <div class="board-list gm-table data-table read-type">
            <table>
                <colgroup>
                    <col width="15%">
                    <col width="35%">
                    <col width="15%">
                    <col width="*">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblSubject" runat="server" Text="제목" meta:resourcekey="lblSubject" />
                        <span class="required essential">필수입력</span>
                    </th>
                    <td class="subject" colspan="3">
                        <asp:TextBox ID="txtSubject" Width="100%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblContent" runat="server" Text="내용" meta:resourcekey="lblContent" />
                        <span class="required essential">필수입력</span>
                    </th>
                    <td class="contents" colspan="3">
                        <div class="board-editor">
		                    <%-- html Editor를 적용할 Textarea를 만듬 --%>
		                    <asp:TextBox ID="txtCONTENT" name="txtCONTENT" runat="server" TextMode="MultiLine" Width="100%" Height="300px" />
                            <div style="display:none;"><asp:TextBox ID="txt_content" runat="server" /></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th scope="row" rowspan="2">
                        <asp:Label ID="lblAttach" runat="server" Text="첨부파일" meta:resourcekey="lblAttach" /><br />
                        <asp:Label ID="lblAttach2" runat="server" Text="(최대 4메가)" meta:resourcekey="lblAttach2" />
                    </th>      
                    <td colspan="3" >

                        <!-- 파일 첨부 인풋 -->
                        <div class="file-box">
                           <input class="upload-name" value="File Name" disabled="disabled">
                           <label for="<%=FileUpload1.ClientID %>">찾아보기</label>
                           <asp:FileUpload ID="FileUpload1" runat="server" CssClass="upload-hidden" />
                       </div>
                       <!--// 파일 첨부 인풋 -->

                        <asp:Button ID="btnUpload" CssClass="button-icon plus" OnClick="btnUpload_OnClick" Text="Add" runat="server" meta:resourcekey="btnUploadResource" OnClientClick="return setWebControls();" />
                        <asp:Button ID="btnRemove" CssClass="button-icon ex" OnClick="btnRemove_OnClick" Text="Remove" runat="server" meta:resourcekey="btnRemoveResource" OnClientClick="return setWebControls();" />
                    </td>
                </tr>
                <tr>
                   <td class="pop_right" colspan="3">
                       <asp:ListBox ID="lbSentlist" runat="server" Width="100%" />
                       <asp:ListBox ID="lbDeleteList" runat="server" Visible="False" Width="98%" Height="0px" />
                   </td>                    
                </tr>  
                </tbody>
            </table>
        </div>

        <div class="button-group right">
		    <asp:Button ID="btnSave" CssClass="button-default blue" Text="Save" OnClick="Button_Click" runat="server" OnClientClick="return fnValidateForm();" meta:resourcekey="btnSaveResource" />
		    <asp:Button ID="btnList" CssClass="button-default" Text="List" OnClick="Button_Click" runat="server" meta:resourcekey="btnListResource" />
		    <!--<asp:Button ID="btnRewrite" CssClass="button-default" Text="Rewrite" OnClick="Button_Click" runat="server" meta:resourcekey="btnRewriteResource" />-->
		    <asp:Button ID="btnCancel" CssClass="button-default" Text="Cancel" OnClick="Button_Click" runat="server" meta:resourcekey="btnCancelResource" />
        </div>
    </section>
</div>
	<%-- 
		스마트 에디터용 자바 스크립트 추가
	--%>
	<script type="text/javascript">
        var oEditors = [];

        var sLang = "ko_KR";	// 언어 (ko_KR/ en_US/ ja_JP/ zh_CN/ zh_TW), default = ko_KR
    
        nhn.husky.EZCreator.createInIFrame({
            oAppRef: oEditors,
            elPlaceHolder: "<%=txtCONTENT.ClientID%>",
            sSkinURI: "/asset/smarteditor/SmartEditor2Skin.html",
            htParams: {
                bUseToolbar: true,				// 툴바 사용 여부 (true:사용/ false:사용하지 않음)
                bUseVerticalResizer: true,		// 입력창 크기 조절바 사용 여부 (true:사용/ false:사용하지 않음)
                bUseModeChanger: true,			// 모드 탭(Editor | HTML | TEXT) 사용 여부 (true:사용/ false:사용하지 않음)
                //bSkipXssFilter : true,		// client-side xss filter 무시 여부 (true:사용하지 않음 / 그외:사용)
                //aAdditionalFontList : aAdditionalFontSet,		// 추가 글꼴 목록
                fOnBeforeUnload: function () {
                    //alert("완료!");
                },
                I18N_LOCALE: sLang
            }, //boolean
            fOnAppLoad: function () {
                //예제 코드
                //oEditors.getById["ir1"].exec("PASTE_HTML", ["로딩이 완료된 후에 본문에 삽입되는 text입니다."]);
            },
            fCreator: "createSEditor2"
        });
	</script>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>