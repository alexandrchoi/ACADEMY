<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="refund.aspx.cs" Inherits="CLT.WEB.UI.LMS.INTRO.refund" 
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">
    
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="고용 보험 환급" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack()"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    
    <section class="refund sub-design">
        <h3>고용보험이란?</h3>
        <p class="content-box">근로자와 사업주에 대한 사회 안전망의 하나로서 진행되고 있으며, 실업보험의 성격과 함께 사전적인 실업예방, 재취업 촉진, 근로자의 직업능력 향상을 위한 인적자원 개발을 수행하는 종합적인 인력정책 수단으로서의 특성을 갖고 있습니다.</p>
        <h3>교육비환급</h3>
        <p class="content-box">고용보험 피보험자로 등록된 교육생의 교육비 전액을 사업주가 부담한 경우 교육비의 일부금액을 사업주 앞으로 한국산업인력공단이 지원해 주는 제도입니다.</p>
        <h3>환급율</h3>
        <div class="content-box">
            <p>훈련직종별 NCS단가에 훈련시간과 환급비율을 곱하여 산출한 금액(환급비율은 기업규모별 비율 적용)이 지급됩니다.</p>
            <div class="message-box default center">
                <span class="headline black">계산식 :</span> <span class="headline">NCS기준단가 x 교육시간 x 기업규모별환급율(%) + 식비 + 숙박비</span>
            </div>

            <div class="refund-graph">
                <dl>
                    <dt>100%</dt>
                    <dd>50인 미만 우선지원</dd>
                    <dt>90%</dt>
                    <dd>50인 이상 우선지원</dd>
                    <dt>60%</dt>
                    <dd>1,000인 미만 대규모</dd>
                    <dt>40%</dt>
                    <dd>1,000인 이상 대규모 (교육비의 20% 내외)</dd>
                </dl>
            </div>
        </div>
        <h3>환급절차</h3>
        <div class="content-box referral">
            <div class="message-box default center mb30">
                <p class="headline black">2017년 1월 1일 이후 환급과정은 <span class="headline">사업주 별도 신청절차 없이 훈련기관이 신청 및 환급</span></p>
                <p>적용과정 : 2017년 1월 1일 인정기준 과정(전면시행)</p>
            </div>
            <div class="process mb30">
                <div class="company">
                    <span class="logo"></span>
                    <span class="title">사업주</span>
                </div>
                <div class="mid-train">
                    <span class="logo"></span>
                    <span class="title">교육기관</span>
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

            <dl class="bullet-list">
                <dt>환급대상</dt>
                <dd>교육비 전액 납부 후 수료자에 한해 환급되며, 위탁계약서 기재된 비용수급 사업장의 법인통장으로 입금됩니다. (입금자명 : 지마린 아카데미 수강생 이름)<br>* 단, 체납사업장 및 지원금(환급금) 소진 사업장 제외</dd>
                <dt>환급시기</dt>
                <dd>과정종료 후 회차별로 환급<br>*위 사항은 교육비 납입여부에 따라 지연될 수 있음</dd>
            </dl>
        </div>
    </section>

</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>