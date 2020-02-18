<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="courseapplication_detail.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.courseapplication_detail" 
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
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="개인수강신청" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">
        
        <!-- Data Table - List type -->
        <div class="gm-table data-table list-type">
            <table>
                <colgroup>
                    <col width="25%">
                    <col width="25%">
                    <col width="25%">
                    <col width="25%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblEducationPeriod" runat="server" Text="교육기간" meta:resourcekey="lblEducationPeriod" />
                    </th>
                    <th scope="row">
                        <asp:Label ID="lblApplicationPeriod" runat="server" Text="수강신청기간" meta:resourcekey="lblApplicationPeriod" />
                    </th>
                    <th scope="row">
                        <asp:Label ID="lblSEQTitle" runat="server" Text="차수" meta:resourcekey="lblSEQTitle" />
                    </th>
                    <th scope="row">
                        <asp:Label ID="lblApply" runat="server" Text="수강신청" meta:resourcekey="lblApply" />
                    </th>
                </tr>
                <tr>
                    <td>
					    <asp:Label ID="lblCourseDate" runat="server" />
                    </td>
                    <td>
					    <asp:Label ID="lblApplyDate" runat="server" />
                    </td>
                    <td>
					    <asp:Label ID="lblSeq" runat="server" />
                    </td>
                    <td>
				        <asp:Button ID="btnApplication" CssClass="button-default blue" Text="Apply" OnClick="btnApplication_OnClick" meta:resourcekey="btnApplicationResource" runat="server" />
                    </td>
                </tr>
                </tbody>
            </table>
        </div>
        <!--// Data Table - List type -->

        <!-- Data Table - Read type -->
        <div class="gm-table data-table read-type">
            <table>
                <caption>표정보 - 과정소개, 학습목표, 수료기준 등을 제공합니다.</caption>
                <colgroup>
                    <col width="10%">
                    <col width="40%">
                    <col width="10%">
                    <col width="40%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblCourseName" runat="server" Text="과정명" meta:resourcekey="lblCourseName" />
                    </th>
                    <td>
                        <asp:Label ID="lblcourse_nm" Width="98%" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblEducationPeriod1" runat="server" Text="교육기간" meta:resourcekey="lblEducationPeriod" />
                    </th>
                    <td>
                        <asp:Label ID="lblCourseDate_1" Width="98%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblIntroduce" runat="server" Text="과정소개" meta:resourcekey="lblIntroduce" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtContent" TextMode="MultiLine" ReadOnly="true" rows="10" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblLearningGoal" runat="server" Text="학습목표" meta:resourcekey="lblLearningGoal" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtObjective" TextMode="MultiLine" ReadOnly="true" rows="10" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblEducationTarget" runat="server" Text="학습대상" meta:resourcekey="lblEducationTarget" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtTarget" TextMode="MultiLine" ReadOnly="true" rows="10" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblCompletionStandard" runat="server" Text="수료기준" meta:resourcekey="lblCompletionStandard" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtCompletedBy" TextMode="MultiLine" ReadOnly="true" rows="10" runat="server" />
                    </td>
                </tr>
                </tbody>
            </table>
        </div>

        <div class="button-box right">
            <asp:Button ID="btnList" runat="server" Text="List" CssClass="button-default blue" OnClick="btnList_OnClick" meta:resourcekey="btnListResource" />
        </div>
    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>