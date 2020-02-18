<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="mail_send.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.mail_send" 
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
            if (isEmpty(document.getElementById('<%=txtTo.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblTo.Text }, new string[] { lblTo.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if (isEmpty(document.getElementById('<%=txtSubject.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblSubject.Text }, new string[] { lblSubject.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            //if (isEmpty(document.getElementById('<%=txtCONTENT.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblContents.Text }, new string[] { lblContents.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            
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
    
    <asp:HiddenField ID="HiddenCourseID" runat="server" />
    <asp:HiddenField ID="HiddenCourseNM" runat="server" />

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="메일 보내기" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board sms-send">

        <!-- 메일 발송항목 부분 시작 !-->
        <div class="notice-text right"><asp:Label ID="lblRequired" runat="server" Text="필수입력항목" meta:resourcekey="lblRequired" /><span class="required"></span></div>
        <div class="board-list gm-table data-table read-type">

            <table>
                <caption>표정보 - 대상조건, 받는사람, 제목, 내용 등을 제공합니다.</caption>
                <colgroup>
                    <col width="20%">
                    <col width="20%">
                    <col width="*">
                </colgroup>

                <tbody>
                <tr>
                    <th scope="row" rowspan="3" colspan="1"><asp:Label ID="lblSelectterms" runat="server" Text="대상조건 선택" meta:resourcekey="lblSelectterms" /></th>
                    <th scope="row"><asp:Label ID="lblPeriod" Text="조회기간" runat="server" meta:resourcekey="lblPeriod" /></th>
                    <td>
                        <asp:TextBox ID="txtCus_From" runat="server" MaxLength="10" CssClass="datepick w180" />
                        <span class="gm-text2">~</span>
                        <asp:TextBox ID="txtCus_To" runat="server" MaxLength="10" CssClass="datepick w180" />
                        <asp:Button ID="btnSearch" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnSearch_Click" OnClientClick="return setWebControls();" meta:resourcekey="btnSearchResource" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblCourseName" Text="과정명" runat="server" meta:resourcekey="lblCourseName" /></th>
                    <td>
                        <asp:DropDownList ID="ddlCus_NM" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCus_NM_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblCusDate" runat="server" Text="교육기간" meta:resourcekey="lblCusDate" /></th>
                    <td>
                        <asp:DropDownList ID="ddlCus_Date" runat="server" />
                        <asp:Button ID="btnAdd" runat="server" CssClass="button-underline" Text="List Add" OnClick="btnAdd_OnClick" OnClientClick="return setWebControls();" meta:resourcekey="btnAddResource" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblTo" runat="server" Text="받는사람" meta:resourcekey="lblTo" /></th>
                    <td colspan="2">
                        <asp:TextBox ID="txtTo" Width="85%" TextMode="MultiLine" Rows="3" runat="server" placeholder="수신자의 메일주소를 입력하세요" CssClass="w50per" />
                        <label class="checkbox-box">
                            <asp:Label ID="lblBcc" Text="숨은참조" runat="server" meta:resourcekey="cbBcc" />
                            <asp:CheckBox ID="cbBcc" runat="server" Checked="false" />
                            <span class="checkmark"></span>
                        </label>
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblSubject" runat="server" Text="제목" meta:resourcekey="lblSubject" /></th>
                    <td colspan="2">
                        <asp:TextBox ID="txtSubject" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblContents" runat="server" Text="내용" meta:resourcekey="lblContents" /></th>
                    <td colspan="2">
                        <div class="board-editor">
		                    <%-- html Editor를 적용할 Textarea를 만듬 --%>
		                    <asp:TextBox ID="txtCONTENT" name="txtCONTENT" runat="server" TextMode="MultiLine" Width="100%" Height="300px" />
                            <div style="display:none;"><asp:TextBox ID="txt_content" runat="server" /></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th scope="row" rowspan="2"><asp:Label ID="lblAttachfile" runat="server" Text="첨부파일" meta:resourcekey="lblAttachfile" /></th>
                    <td colspan="2">
                        (Max 4mb)
                        &nbsp
                        <div class="file-box">
                           <input class="upload-name" value="File Name" disabled="disabled">
                           <label for="<%=FileUpload1.ClientID %>"><asp:Label ID="lblSearchFile" runat="server" Text="찾아보기" meta:resourcekey="lblSearchFile" /></label>
                           <asp:FileUpload ID="FileUpload1" runat="server" CssClass="upload-hidden" />
                        </div>
                        <!-- 파일 첨부 인풋 -->
                        <asp:Button ID="Button1" CssClass="button-icon plus" OnClick="btnUpload_OnClick" Text="Add" runat="server" OnClientClick="return setWebControls();" meta:resourcekey="btnUploadResource" />
                        <asp:Button ID="Button2" CssClass="button-icon ex" OnClick="btnRemove_OnClick" Text="Remove" runat="server" OnClientClick="return setWebControls();" meta:resourcekey="btnRemoveResource" />   
                    </td>
                </tr>
                <tr>
                   <td class="pop_right" colspan="2">
                       <asp:ListBox ID="lbSentlist" runat="server" Width="100%" />
                       <asp:ListBox ID="lbDeleteList" runat="server" Visible="False" Width="98%" Height="0px" />
                   </td>                    
                </tr>  
                </tbody>
            </table>

        </div>
        <!-- 메일 발송항목 부분 끝 !-->

        <div class="button-group right">
		    <asp:Button ID="btnMailSend" runat="server" CssClass="button-default blue" Text="Send" OnClick="btnMailSend_OnClick" OnClientClick="return fnValidateForm();" meta:resourcekey="btnMailSendResource" />
		    <asp:Button ID="btnCancel" runat="server" CssClass="button-default" Text="Cancel" OnClick="btnCancel_Click" meta:resourcekey="btnCancelResource" />
        </div>
    </section>
</div>
<!--// 서브 컨텐츠 끝 -->
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