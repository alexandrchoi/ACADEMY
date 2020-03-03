<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="consignment.aspx.cs" Inherits="CLT.WEB.UI.LMS.INTRO.consignment" 
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">
    
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="사업주위탁훈련" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack()"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="referral sub-design">
        <h3>사업목적</h3>

        <p class="content-box headline black">사업주가 노동자 등을 대상으로 자체 또는 위탁으로 직업능력개발훈련을 실시할 경우 훈련비 일부를 지원해 인적자원개발 및 기업 경쟁력 제고</p>
        <h3>사업내용</h3>
        <div class="content-box">
            <ul class="bullet-list">
                <li>사업주가 비용을 전적으로 부담하여 소속 근로자를 대상으로 직업능력개발훈련을 실시(자체 또는 위탁)했을 때 정해진 수료기준을 충족한 근로자에게 소요된 비용에 한해서 훈련비 단가를 적용하여 일부 비용 환급 지원</li>
                <li>지원대상 : 고용보험가입 사업주</li>
                <li>지원내용</li>
            </ul>
            <div class="gm-table data-table list-type">
                <table>
                    <colgroup>
                        <col width="20%">
                        <col width="40%">
                        <col width="40%">
                    </colgroup>
                    <thead>
                    <tr>
                        <th scope="row">지원내용</th>
                        <th scope="row">지원요건</th>
                        <th scope="row">지원수준</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td>훈련비</td>
                        <td>1일 8시간(대기업 2일 16시간) 이상의 훈련실시 (집체훈련 기준)</td>
                        <td>
                            <ul class="bullet-list left">
                                <li>우선지원 대상기업 100% 지원(위탁훈련 90% 지원)</li>
                                <li>상시노동자 1,000인 미만 기업 60% 지원(원격훈련 80% 지원)</li>
                                <li>상시노동자 1,000인 이상 기업 40% 지원 *단 외국어 과정은 각 수준별 지원금액의 50% 지원</li>
                            </ul>
                        </td>
                    </tr>
                    </tbody>
                </table>
            </div>
        </div>

        <h3>사업추진체계</h3>
        <div class="content-box">
            <div class="referral-org">
                <div class="referral-sides company">사업주/위탁훈련기관</div>
                <div class="steps">
                    <ol class="list-referral">
                        <li><span>훈련과정 인정신청</span></li>
                        <li><span>훈련과정 인정요건 검토 ㆍ 통지</span></li>
                        <li><span>훈련실시 및 수료자 보고, 훈련비용 신청</span></li>
                        <li><span>훈련비용 지원</span></li>
                    </ol>
                </div>
                <div class="referral-sides gov">한국산업 인력공단</div>
            </div>
        </div>

        <h3>지원절차</h3>
        <div class="content-box">
            <p class="headline black">사업주는 훈련비를 훈련기관에 납부하여 재직근로자 등을 대상으로 직업훈련을 실시하는 경우 고용노동부에서 훈련비를 지원합니다.</p>
            <div class="process">
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
        </div>

        <h3>진행절차</h3>
        <div class="content-box process-step">
            <div class="step-1">
                <div>
                    <span class="title">회원사등록</span>
                    <p>유선/이메일/사이트를 통해 회원사 등록<br>교육문의 : 051.330.9382</p>
                </div>
            </div>

            <div class="step-2">
                <div>
                    <span class="title">수강신청</span>
                    <p>참여 훈련과정을 선택후, 유선/이메일/사이트를 통해 수강신청</p>
                </div>
            </div>

            <div class="step-3">
                <div>
                    <span class="title">위탁계약서 작성</span>
                    <p>참여 훈련과정에 대한 훈련위탁계약서 작성후 이메일 발송(1부 사업주 보관, 1부 교육기관 제출)</p>
                </div>
            </div>

            <div class="step-4">
                <div>
                    <span class="title">훈련진행</span>
                    <p>훈련과정 참여자에게 문자 등 입과안내, 교육과정 진행</p>
                </div>
            </div>

            <div class="step-5">
                <div>
                    <span class="title">평가</span>
                    <p>과정종료시 평가 및 피드백 시행</p>
                </div>
            </div>

            <div class="step-6">
                <div>
                    <span class="title">수료</span>
                    <p>교육과정 80%이상 이수시 수료 처리</p>
                </div>
            </div>
        </div>

        <h3>문의</h3>
        <div class="content-box proc-contact">
            <p>기타 궁금하신 사항은 아래로 문의 바랍니다.</p>
            <ul class="message-box default">
                <li><span class="icon-bg icon-tel">전화</span> <a href="tel:051-330-9382">051-330-9382</a></li>
                <li><span class="icon-bg icon-mail">이메일</span> <a href="email:trainingcenter@gmarineservice.com">trainingcenter@gmarineservice.com</a></li>
            </ul>
            <div class="button-group center">
                <a href="/file/download/consignment_contract.xlsx" target="_blank" class="button-main-rnd lg blue">위탁계약서 다운로드</a>
            </div>
        </div>
    </section>

</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>