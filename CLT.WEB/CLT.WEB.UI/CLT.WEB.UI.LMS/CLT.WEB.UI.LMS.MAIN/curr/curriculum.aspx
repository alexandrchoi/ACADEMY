<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="curriculum.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.curriculum"
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">


<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">

    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="교육체계도" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
            <button onclick="goBack()"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
        </span>
    </h2>

    <section class="curriculum">
        <p class="headline black center">다양하고 체계적인 교육훈련 커리큘럼을 통해 해기인력 역량과 자질 향상을 위해 노력합니다.</p>
        <div class="category-1st-box">
            <ul class="gm-grid column-3">
                <li class="item">
                    <div class="category-1st leadership">
                        <div class="title">Leadership Course</div>
                        <div class="cont"><span>3</span>개 리더십코스</div>
                    </div>
                </li>
                <li class="item">
                    <div class="category-1st primary">
                        <div class="title">Primary Course</div>
                        <div class="cont"><span>8</span>개 기본교육코스</div>
                    </div>
                </li>
                <li class="item">
                    <div class="category-1st job">
                        <div class="title">Job Competency Course</div>
                        <div class="cont"><span>41</span>개 직무역량코스</div>
                    </div>
                </li>
            </ul>
        </div>

        <div class="category-2nd-box center">
            <div class="title">지정교육</div>
            <div class="category-2nd">
                <span>해수부지정</span>
                <span>고용노동부 인정</span>
            </div>
        </div>

        <div class="category-3rd-box left">
            <ul class="gm-grid column-7">
                <li class="item">
                    <div>
                    <ul>
                        <li>자동차운반선직무</li>
                        <li>자동차운반선화재안전</li>
                        <li>자동차운반선장비운용</li>
                        <li>선박조종기술숙련</li>
                        <li>항해계획운용실무</li>
                        <li>전자해도장치운용실무</li>
                        <li>선박 위험물 운송 관리(IMDG)</li>
                        <li>전자 제어 엔진 시스템</li>
                        <li>선상 의료 관리와 응급처치</li>
                        <li>선박 사고 조사자 전문</li>
                        <li>선교 도선 관리 기술 (SHS 특화)</li>
                    </ul>
                    </div>
                </li>
                <li class="item"><div>ECDIS (Electronic Chart Display & Information System)</div></li>
                <li class="item"><div>ERS (Engine Room Simulator)</div></li>
                <li class="item"><div>SHS (Ship Handling Simulator)</div></li>
                <li class="item"><div>리더십 및 관리기술직무 교육 (Leadership & Managerial Skill)</div></li>
                <li class="item"><div>리더십 및 팀워크 (Leadership $ Teamwork)</div></li>
                <li class="item"><div>SHS & BTM (선박모의조종 및 선교팀 워크교육) *IMO Model Course 1,22</div></li>
            </ul>
        </div>
    </section>

</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>