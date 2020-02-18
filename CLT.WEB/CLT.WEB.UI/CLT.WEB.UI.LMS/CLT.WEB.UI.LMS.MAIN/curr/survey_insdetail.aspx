<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="survey_insdetail.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.survey_insdetail"
    Culture="auto" UICulture="auto" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">

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
                    <col width="35%">
                    <col width="15%">
                    <col width="35%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblRes_SUB" runat="server" Text="설문 제목" meta:resourcekey="lblRes_SUB" /><span class="required">필수입력</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtRes_SUB" runat="server" ReadOnly="true" BackColor="#dcdcdc" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblRes_FromTo" runat="server" Text="투표 기한" meta:resourcekey="lblRes_FromTo" /><span class="required">필수입력</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtRes_From" runat="server" MaxLength="10" CssClass="datepick w180" /><span class="gm-text2">~</span><asp:TextBox ID="txtRes_To" runat="server" MaxLength="10" CssClass="datepick w180" />
                    </td>
                </tr>
                </tbody>
            </table>
            <!-- Master 설문 등록 끝 -->
        </div>

        <div class="board-list gm-table data-table read-type survey-input">

            <asp:PlaceHolder ID="ph01" runat="server"></asp:PlaceHolder>
            <!-- 반복 -->
            <!--
            <table>
                <colgroup>
                    <col width="15%">
                    <col width="15%">
                    <col width="*">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row" rowspan="4">1번 설문</th>
                    <th scope="row">질문</th>
                    <td><input type="text"></td>
                </tr>
                <tr>
                    <th scope="row">보기형태</th>
                    <td>
                        <label class="radio-box">단일선택
                            <input type="radio" checked="checked" name="radio">
                            <span class="radiomark"></span>
                        </label>

                        <label class="radio-box">다중선택
                            <input type="radio" name="radio">
                            <span class="radiomark"></span>
                        </label>

                        <label class="radio-box">서술형
                            <input type="radio" name="radio">
                            <span class="radiomark"></span>
                        </label>
                        <label class="radio-box">단일선택(혼합형)
                            <input type="radio" name="radio">
                            <span class="radiomark"></span>
                        </label>
                        <label class="radio-box">다중선택(혼합형)
                            <input type="radio" name="radio">
                            <span class="radiomark"></span>
                        </label>
                        <label class="radio-box">순서배열
                            <input type="radio" name="radio">
                            <span class="radiomark"></span>
                        </label>
                    </td>
                </tr>
                <tr>
                    <th scope="row">보기항목수</th>
                    <td>
                        <select>
                            <option>1번</option>
                            <option>2번</option>
                            <option>3번</option>
                            <option>4번</option>
                            <option>5번</option>
                            <option>6번</option>
                            <option>7번</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <th scope="row">보기1</th>
                    <td><input type="text"></td>
                </tr>
                </tbody>
            </table>
            -->
            <!-- 반복 -->
        </div>

        <!-- 버튼 -->
        <div class="button-group center">
            <asp:Button ID="btnSave" CssClass="button-main-rnd blue lg" Text="Save" OnClick="btnSave_OnClick" runat="server" meta:resourcekey="btnSaveResource" />
            <asp:Button ID="btnList" CssClass="button-main-rnd lg" Text="List" OnClick="btnList_OnClick" runat="server" meta:resourcekey="btnListResource" />
        </div>
    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>