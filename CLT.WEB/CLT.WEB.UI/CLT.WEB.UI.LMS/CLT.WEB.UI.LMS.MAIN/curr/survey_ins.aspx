<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="survey_ins.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.survey_ins"
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">
    function fnValidateForm()
    {
       // 필수 입력값 체크
       if (isEmpty(document.getElementById('<%=txtRes_SUB.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblRes_SUB.Text }, new string[] { lblRes_SUB.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // 설문제목을 입력해 주세요!
       if (isEmpty(document.getElementById('<%=txtRes_From.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblRes_FromTo.Text }, new string[] { lblRes_FromTo.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // 투표기한을 입력해 주세요!
       if (isEmpty(document.getElementById('<%=txtRes_To.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblRes_FromTo.Text }, new string[] { lblRes_FromTo.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // 투표기한을 입력해 주세요!
       if (isEmpty(document.getElementById('<%=txtRes_Obj.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblRes_Obj.Text }, new string[] { lblRes_Obj.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false; // 설문목적을 입력해 주세요!

          // 설문문항 체크
        if (isSelect(document.getElementById('<%=ddlResquecnt.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblResquecnt.Text }, new string[] { lblResquecnt.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

           // 길이체크
           if(isMaxLenth(document.getElementById('<%=txtRes_SUB.ClientID %>'),150,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblRes_SUB.Text,"50","150" }, new string[] { lblRes_SUB.Text,"50","150" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtRes_Obj.ClientID %>'),300,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblRes_Obj.Text,"100","300" }, new string[] { lblRes_Obj.Text,"100","300" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

    }
</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:HiddenField ID="HiddenCourseID" runat="server" />
    <asp:HiddenField ID="HiddenCourseNM" runat="server" />


<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="설문 등록" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
            <button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
        </span>
    </h2>

    <section class="section-board">

        <div class="notice-text right"><asp:Label ID="lblRequired" runat="server" Text="필수입력항목" meta:resourcekey="lblRequired" /><span class="required">필수입력</span></div>
        <div class="board-list gm-table data-table read-type">

            <!-- Master 설문 등록 시작 -->
            <table>
                <colgroup>
                    <col width="15%">
                    <col width="10%">
                    <col width="30%">
                    <col width="15%">
                    <col width="30%">
                </colgroup>
                <tbody>
                <tr>
                    <th id="TDTARGET" runat="server" scope="row" rowspan="3">
                        <asp:Label ID="lblParticipant" runat="server" Text="설문대상자" meta:resourcekey="lblParticipant" />
                    </th>
                    <th scope="row">
                        <asp:Label ID="lblSend" runat="server" Text="선박발송여부" meta:resourcekey="lblSend" />
                    </th>
                    <td>
                        <asp:CheckBox ID="chkSent_YN" Checked="true" MaxLength="50" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblSurveytype" runat="server" Text="설문 구문" meta:resourcekey="lblSurveytype" />
                    </th>
                    <td class="subject">
                        <label class="radio-box">
                            <asp:RadioButton ID="rbRes_Kind" GroupName="kind" runat="server" Checked="true" Text="일반설문" meta:resourcekey="rbRes_Kind" AutoPostBack="true" OnCheckedChanged="rbRes_Kind_OnCheckedChanged" />
                            <span class="radiomark"></span>
                        </label>

                        <label class="radio-box">
                            <asp:RadioButton ID="rbRes_KindCurr" GroupName="kind" runat="server" Checked="false" Text="과정설문" meta:resourcekey="rbRes_KindCurr" AutoPostBack="true" OnCheckedChanged="rbRes_KinkCurr_OnCheckedChanged" />
                            <span class="radiomark"></span>
                        </label>
                    </td>
                </tr>
                <tr id="TRUSER" runat="server">
                    <th scope="row">
                        <asp:Label ID="lblCompany" runat="server" Text="회사명" meta:resourcekey="lblCompany" />
                    </th>
                    <td>
                        <asp:ListBox ID="lboxCompany" SelectionMode="Multiple" Height="150" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblUserGorup" runat="server" Text="회원그룹별" meta:resourcekey="lblUserGorup" /><span class="required">필수입력</span>
                    </th>
                    <td>
                        <asp:ListBox ID="lboxUserGorup" SelectionMode="Multiple" Height="150" runat="server" />
                    </td>
                </tr>
                <tr id="TRDATE" runat="server">
                    <th scope="row">
                        <asp:Label ID="lblPeriod" runat="server" Text="조회기간" meta:resourcekey="lblPeriod" />
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtPeriodFrom" runat="server" MaxLength="10" CssClass="datepick w180" /><span class="gm-text2">~</span><asp:TextBox ID="txtPeriodTo" runat="server" MaxLength="10" CssClass="datepick w180" />
                    </td>
                </tr>
                <tr id="TRDUTY" runat="server">
                    <th scope="row">
                        <asp:Label ID="lblClassification" runat="server" Text="신분구분" meta:resourcekey="lblClassification" />
                    </th>
                    <td>
                        <asp:ListBox ID="lboxSocialpos" SelectionMode="Multiple" Height="150" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblGrade" runat="server" Text="직급" meta:resourcekey="lblGrade" />
                    </th>
                    <td>
                        <asp:ListBox ID="lboxDutystep" SelectionMode="Multiple" Height="150" runat="server" />
                    </td>
                </tr>
                <tr id="TRCUS" runat="server">
                    <th scope="row">
                        <asp:Label ID="lblCurrName" runat="server" Text="과정명" meta:resourcekey="lblCurrName" />
                        <asp:PlaceHolder ID="ph01" runat="server" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtCus_ID" ReadOnly="true" runat="server" />
                        <asp:TextBox ID="txtCus_NM" ReadOnly="true" runat="server" />
                        <input type="button" id="btnCus_Search" value="Search" class="button-default blue"
                            onclick="openPopWindow('/common/course_pop.aspx?opener_textbox_id=<%=HiddenCourseID.ClientID%>&opener_textbox_nm=<%=HiddenCourseNM.ClientID %>', 'course_pop_win', '600', '650');" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblEducationperiod" runat="server" Text="교육기간" meta:resourcekey="lblEducationperiod" />
                        <asp:PlaceHolder ID="ph02" runat="server" />
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlCus_Date" runat="server" />
                        <!--<asp:Button ID="btnSelect" CssClass="button-default" Text="Search" OnClick="btnSelect_OnClick" runat="server" meta:resourcekey="btnSearchResource" />-->
                    </td>
                </tr>
                <tr>
                    <th scope="row" colspan="2">
                        <asp:Label ID="lblRes_SUB" runat="server" Text="설문 제목" meta:resourcekey="lblRes_SUB" /><span class="required">필수입력</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtRes_SUB" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblResquecnt" runat="server" Text="설문 문항" meta:resourcekey="lblResquecnt" /><span class="required">필수입력</span>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlResquecnt" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" colspan="2">
                        <asp:Label ID="lblRes_FromTo" runat="server" Text="투표 기한" meta:resourcekey="lblRes_FromTo" /><span class="required">필수입력</span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtRes_From" runat="server" MaxLength="10" CssClass="datepick w180" /><span class="gm-text2">~</span><asp:TextBox ID="txtRes_To" runat="server" MaxLength="10" CssClass="datepick w180" />
                    </td>
                </tr>
                <tr>
                    <th scope="row" colspan="2">
                        <asp:Label ID="lblRes_Obj" runat="server" Text="설문 목적" meta:resourcekey="lblRes_Obj" /><span class="required">필수입력</span>
                    </th>
                    <td colspan="3" runat="server" id="TD4">
                        <asp:TextBox ID="txtRes_Obj" TextMode="MultiLine" Rows="5" runat="server" Height="98%" AutoCompleteType="Notes" />
                    </td>
                </tr>
                </tbody>
            </table>
            <!-- Master 설문 등록 끝 -->
            <!-- 등록된 Master 설문 보기 시작-->

            <!-- 등록된 Master 설문 보기 끝-->
            <br />

        </div>

        <!-- 버튼 -->
        <div class="button-group center">
            <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="button-default blue" meta:resourcekey="btnNextResource" OnClientClick="return fnValidateForm();" OnClick="btnNext_OnClick" />
        </div>


    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>