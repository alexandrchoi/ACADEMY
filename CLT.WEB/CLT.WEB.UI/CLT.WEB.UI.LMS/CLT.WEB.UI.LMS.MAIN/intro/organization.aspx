<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="organization.aspx.cs" Inherits="CLT.WEB.UI.LMS.INTRO.organization" 
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">

    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="조직현황" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack()"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="organization">
        <div class="gm-table data-table list-type gm-staff">
            <table>
                <caption><asp:Label ID="lblCaption" runat="server" Text="조직현황 - 조직의 담당자와 담당업무, 연락처 등을 제공합니다." meta:resourcekey="lblCaptionResource" /></caption>

                <thead>
                <tr>
                    <th scope="col"><asp:Label ID="lblJobTitle" runat="server" Text="직책" meta:resourcekey="lblJobTitleResource" /></th>
                    <th scope="col"><asp:Label ID="lblName" runat="server" Text="성명" meta:resourcekey="lblNameResource" /></th>
                    <th scope="col"><asp:Label ID="lblDuty" runat="server" Text="담당업무" meta:resourcekey="lblDutyResource" /></th>
                    <th scope="col"><asp:Label ID="lblContact" runat="server" Text="연락처" meta:resourcekey="lblContactResource" /></th>
                </tr>
                </thead>
                <tbody>
                <tr>
                    <td><asp:Label ID="lblJob1" runat="server" Text="원장" meta:resourcekey="lblJob1Resource" /></td>
                    <td><asp:Label ID="lblName1" runat="server" Text="신상일" meta:resourcekey="lblName1Resource" /></td>
                    <td><asp:Label ID="lblDuty1" runat="server" Text="업무 총괄" meta:resourcekey="lblDuty1Resource" /></td>
                    <td class="left">
                        <div class="org-contact">
                            <span class="org-info">
                                <span class="icon-bg icon-tel">전화</span>
                                <span class="org-content">051-330-9380</span>
                            </span>
                            <span class="org-info">
                                <span class="icon-bg icon-mail">이메일</span>
                                <span class="org-content">sishin@gmarineservice.com</span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblJob2" runat="server" Text="책임매니저" meta:resourcekey="lblJob2Resource" /></td>
                    <td><asp:Label ID="lblName2" runat="server" Text="장진욱" meta:resourcekey="lblName2Resource" /></td>
                    <td><asp:Label ID="lblDuty2" runat="server" Text="교육 기획" meta:resourcekey="lblDuty2Resource" /></td>
                    <td class="left">
                        <div class="org-contact">
                            <span class="org-info">
                                <span class="icon-bg icon-tel">전화</span>
                                <span class="org-content">051-330-9382</span>
                            </span>
                            <span class="org-info">
                                <span class="icon-bg icon-mail">이메일</span>
                                <span class="org-content">jwkjang@gmarineservice.com</span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblJob3" runat="server" Text="책임매니저" meta:resourcekey="lblJob3Resource" /></td>
                    <td><asp:Label ID="lblName3" runat="server" Text="정인교" meta:resourcekey="lblName3Resource" /></td>
                    <td><asp:Label ID="lblDuty3" runat="server" Text="교육 개발(컨소시엄 사업)" meta:resourcekey="lblDuty3Resource" /></td>
                    <td class="left">
                        <div class="org-contact">
                            <span class="org-info">
                                <span class="icon-bg icon-tel">전화</span>
                                <span class="org-content">051-330-9383</span>
                            </span>
                            <span class="org-info">
                                <span class="icon-bg icon-mail">이메일</span>
                                <span class="org-content">ikjung@gmarineservice.com</span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblJob4" runat="server" Text="매니저" meta:resourcekey="lblJob4Resource" /></td>
                    <td><asp:Label ID="lblName4" runat="server" Text="박경민" meta:resourcekey="lblName4Resource" /></td>
                    <td><asp:Label ID="lblDuty4" runat="server" Text="교육 운영(컨소시엄 사업)" meta:resourcekey="lblDuty4Resource" /></td>
                    <td class="left">
                        <div class="org-contact">
                            <span class="org-info">
                                <span class="icon-bg icon-tel">전화</span>
                                <span class="org-content">051-330-9386</span>
                            </span>
                            <span class="org-info">
                                <span class="icon-bg icon-mail">이메일</span>
                                <span class="org-content">kminpark@gmarineservice.com</span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblJob5" runat="server" Text="사원" meta:resourcekey="lblJob5Resource" /></td>
                    <td><asp:Label ID="lblName5" runat="server" Text="설지현" meta:resourcekey="lblName5Resource" /></td>
                    <td><asp:Label ID="lblDuty5" runat="server" Text="교육 운영(컨소시엄 사업)" meta:resourcekey="lblDuty5Resource" /></td>
                    <td class="left">
                        <div class="org-contact">
                            <span class="org-info">
                                <span class="icon-bg icon-tel">전화</span>
                                <span class="org-content">051-330-9387</span>
                            </span>
                            <span class="org-info">
                                <span class="icon-bg icon-mail">이메일</span>
                                <span class="org-content">jhseol@gmarineservice.com</span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblJob6" runat="server" Text="교수" meta:resourcekey="lblJob6Resource" /></td>
                    <td><asp:Label ID="lblName6" runat="server" Text="박쌍식" meta:resourcekey="lblName6Resource" /></td>
                    <td><asp:Label ID="lblDuty6" runat="server" Text="항해 교육" meta:resourcekey="lblDuty6Resource" /></td>
                    <td class="left">
                        <div class="org-contact">
                            <span class="org-info">
                                <span class="icon-bg icon-tel">전화</span>
                                <span class="org-content">051-330-9381</span>
                            </span>
                            <span class="org-info">
                                <span class="icon-bg icon-mail">이메일</span>
                                <span class="org-content">ssbag@gmarineservice.com</span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblJob7" runat="server" Text="전임강사" meta:resourcekey="lblJob7Resource" /></td>
                    <td><asp:Label ID="lblName7" runat="server" Text="김한결" meta:resourcekey="lblName7Resource" /></td>
                    <td><asp:Label ID="lblDuty7" runat="server" Text="항해 교육" meta:resourcekey="lblDuty7Resource" /></td>
                    <td class="left">
                        <div class="org-contact">
                            <span class="org-info">
                                <span class="icon-bg icon-tel">전화</span>
                                <span class="org-content">051-330-9384</span>
                            </span>
                            <span class="org-info">
                                <span class="icon-bg icon-mail">이메일</span>
                                <span class="org-content">kimhg@gmarineservice.com</span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblJob8" runat="server" Text="전임강사" meta:resourcekey="lblJob8Resource" /></td>
                    <td><asp:Label ID="lblName8" runat="server" Text="박동혁" meta:resourcekey="lblName8Resource" /></td>
                    <td><asp:Label ID="lblDuty8" runat="server" Text="기관 교육" meta:resourcekey="lblDuty8Resource" /></td>
                    <td class="left">
                        <div class="org-contact">
                            <span class="org-info">
                                <span class="icon-bg icon-tel">전화</span>
                                <span class="org-content">051-330-9385</span>
                            </span>
                            <span class="org-info">
                                <span class="icon-bg icon-mail">이메일</span>
                                <span class="org-content">dh-park@gmarineservice.com</span>
                            </span>
                        </div>
                    </td>
                </tr>
                </tbody>
            </table>
        </div>

    </section>

</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>