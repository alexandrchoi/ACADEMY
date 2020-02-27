<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="consortium.aspx.cs" Inherits="CLT.WEB.UI.LMS.INTRO.consortium" 
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="컨소시엄훈련" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack()"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="consortium sub-design">
        <h3>컨소시엄 교육훈련 운영 개요</h3>
        <div class="content-box">
            <p class="mb30">재직근로자 및 채용예정자를 대상으로 한 직무능력 향상/양성 훈련 실시 지원</p>
            <ul class="dash-list">
                <li>재직근로자 : 고용보험 가입자</li>
                <li>채용예정자 : 고용보험 미가입자 (3개월내 가입예정자)</li>
            </ul>
            <figure class="center">
                <img src="/asset/images/sub/consortium-dia1.gif" alt="컨소시엄 교육훈련 운영 개요">
            </figure>
        </div>

        <h3>컨소시엄 협약배경</h3>
        <div class="content-box referral">
            <p class="headline black">해당분야 다수의 중소기업과 공동훈련 협약을 맺고 (컨소시엄 구성), 훈련시설/장비를 활용하여 중소 협약기업 근로자들에게 맞춤형 훈련을 제공할 수 있도록 정부가 공동훈련에 필요한 훈련인프라와 훈련비 등을 지원하는 인적자원개발(HRD)사업</p>
            <div class="process mb30">
                <div class="company">
                    <span class="logo"></span>
                    <span class="title">사업주</span>
                </div>
                <div class="mid-train">
                    <span class="logo"></span>
                    <span class="title">공동훈련센터</span>
                </div>
                <div class="gov">
                    <span class="logo"></span>
                    <span class="title">한국산업인력공단<br>고용노동부</span>
                </div>
                <div class="process-list">
                    <ol>
                        <li><span>교육훈련수료</span></li>
                        <li><span>교육신고</span></li>
                        <li><span>교육비환급</span></li>
                        <li><span>환급교육비지급(전달)</span></li>
                    </ol>
                </div>
            </div>
            <figure class="center">
                <img src="/asset/images/sub/consortium-dia2.gif" alt="컨소시엄 협약배경">
            </figure>
        </div>

        <h3>컨소시엄 협약 및 기대효과</h3>
        <div class="content-box">
            <ul class="bullet-list">
                <li class="mb30">
                    <p class="headline black">협약체결된 기업의 재직자 및 입사예정(채용예정자) 직원이 무료교육을 받게 됩니다.</p>
                    <p>고용노동부가 운영하는 중소기업 재직근로자의 직무능력 향상을 위한 사업으로서 협회를 중심으로 참여기업이 직업능력개발법에 의해 지원 받는 직업능력개발지원금 한도액 범위내에서 운영기관이 제공하는 교육과정을 전직원(재직자 및 채용예정자)이 무료로 수강할 수 있는 공동 훈련 사업입니다.</p>
                </li>
                <li class="mb30">
                    <p class="headline black">어떻게 협약기업이 될 수 있나요?</p>
                    <p class="mb10">고용보험을 납부하는 기업으로서 지마린아카데미 교육훈련과정 참여를 희망하면 협약서를 작성 및 제출 후 협약체결이 이루어집니다.</p>
                    <ol class="list-order">
                        <li>컨소시엄 협약신청</li>
                        <li>협약체결</li>
                        <li>과정개설 교육안내</li>
                        <li>교육참여 교육수료</li>
                    </ol>
                </li>
                <li class="mb30">
                    <p class="headline black">고용보험 사업주 훈련비로 교육이 이루어집니다.</p>
                    <p>고용노동부가 운영하는 중소기업 재직근로자의 직무능력 향상을 위한 사업으로서 공동훈련센터를 중심으로 참여기업이 직업능력개발법에 의해 지원받는 직업능력개발지원금 한도액 범위 내에서 교육비가 지원됩니다.</p>
                </li>
            </ul>
        </div>

        <h3>컨소시엄 협약 및 교육신청 절차</h3>
        <div class="content-box">
            <h4 class="center">컨소시엄 협약 절차</h4>
            <div class="process-step">
            <div class="step-1">
                <span class="title">지마린아카데미 홈페이지 접속</span>
                <p>협약관련문의 : 051.330.9386</p>
            </div>
            <div class="step-2">
                <span class="title">협약신청서 다운로드</span>
                <p><a href="/file/download/consortium_intent.hwp" target="_blank" class="button-underline">다운로드</a></p>
            </div>
            <div class="step-3">
                <span class="title">컨소시엄 가입신청</span>
                <p>trainingcenter@gmarineservice.com</p>
            </div>
            <div class="step-4">
                <span class="title">컨소시엄 협약완료</span>
                <p>담당자 확인 요청<br>051.330.9386</p>
            </div>
            </div>
            <h4 class="center">교육신청 절차</h4>
            <div class="process-step">
            <div class="step-1">
                <span class="title">지마린아카데미 홈페이지 접속</span>
                <p>교육신청문의 : 051.330.9386/9387</p>
            </div>
            <div class="step-2">
                <span class="title">교육신청서 다운로드</span>
                <a href="/file/download/consortium_apply.xlsx" target="_blank" class="button-underline">다운로드</a>
            </div>
            <div class="step-3">
                <span class="title">교육신청 이메일 발송</span>
                <p>trainingcenter@gmarineservice.com</p>
            </div>
            <div class="step-4">
                <span class="title">교육신청 완료</span>
                <p>담당자 확인 요청<br>051.330.9386/9387</p>
            </div>
            <div class="step-5">
                <span class="title">교육안내</span>
                <p>개별 문자 안내 및 교육 참여</p>
            </div>
            </div>
        </div>

        <h3>문의</h3>
        <div class="content-box proc-contact">
            <p>기타 궁금하신 사항은 아래로 문의 바랍니다.</p>
            <ul class="message-box default">
                <li><span class="icon-bg icon-tel">전화</span> <a href="tel:051-330-9387">051-330-9386/9387</a></li>
                <li><span class="icon-bg icon-mail">이메일</span> <a href="mailto:trainingcenter@gmarineservice.com">trainingcenter@gmarineservice.com</a></li>
            </ul>
            <div class="button-group center">
                <a href="/file/download/consortium_apply.xlsx" target="_blank" class="button-default lg">교육신청서식 다운로드</a>
                <a href="/file/download/consortium_intent.hwp" target="_blank" class="button-default lg">협약서 다운로드</a>
                <!--<a href="#none" target="_blank" class="button-default lg">컨소시엄 바로가기</a>-->
            </div>
        </div>
    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>