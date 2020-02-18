<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="stcw.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPR.stcw" 
    Culture="auto" UICulture="auto" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">
        
    </script>
</asp:Content>



<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="STCW" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack()"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    <p>정확한 역량진단과 단계적인 학습은 Global Leader가 되는 길입니다. STCW상 직책별 역량기준을 확인 할 수 있습니다.</p>

    <section class="sub-design stcw">
            <div class="sub-tab">
                <a href="#none" class="current" data-tab="tab-1" id="tab_1">선장&1항사(관리급)</a>
                <a href="#none" data-tab="tab-2" id="tab_2">2항사&3항사(운항급)</a>
                <a href="#none" data-tab="tab-3" id="tab_3">갑판부원(보조급)</a>
                <a href="#none" data-tab="tab-4" id="tab_4">기관장&1기사(관리급)</a>
                <a href="#none" data-tab="tab-5" id="tab_5">2기사&3기사(운항급)</a>
                <a href="#none" data-tab="tab-6" id="tab_6">기관부원(보조금)</a>
            </div>

            <!-- 선장&1항사(관리급) -->
            <div id="tab-1" class="tab-content current">
                <div class="stcw-box">
                    <h4><span>항해</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>항해계획과 항해 수행</li>
                            <li>선위결정과 일체의 방법에 의하여 구한 실측위치의 정밀도</li>
                            <li>컴퍼스 오차의 결정과 감안</li>
                            <li>수색과 구조작업의 조정</li>
                            <li>당직 근무 배치와 절차의 수립</li>
                            <li>레이더와 ARPA 및 회신 항해시스템의 사용을 통한 항해 안전 유지</li>
                            <li>기상예보와 해상 상태</li>
                            <li>항해상의 비상사태 대응</li>
                            <li>모든 상황에서의 선박 조종과 취급</li>
                            <li>추진플랜트, 기관시스템과 설비의 원격 제어 운전</li>
                        </ul>
                    </div>
                </div>
                
                <div class="stcw-box">
                    <h4><span>화물취급과 적부</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>화물의 안전한 적재/적부/결속 및 항해중 관리와 양화 계획 및 확보</li>
                            <li>화물구역과 해치커버 및 밸러스트탱크에 대하여 보고된 결함과 손상 평가 및 적절한 조치</li>
                            <li>위험화물 운송</li>
                        </ul>
                    </div>
                </div>

                <div class="stcw-box">
                    <h4><span>선박운항 통제와 선상의 인명관리</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>트림, 복원성, 응력의 관리</li>
                            <li>해상인명안전과 해양환경보호를 위한 법적 요건과 조치에 따른 감시와 관리</li>
                            <li>선박, 승무원, 여객의 편의와 안전 및 생존, 소화 및 기타 안전시스템의 운전조건 유지</li>
                            <li>비상과 손상제어 계획 개발과 비상상황 취급</li>
                            <li>승무원 조직과 관리</li>
                            <li>선내 의료제공 조직과 관리</li>
                        </ul>
                    </div>
                </div>
                
            </div>
            <!--// 선장&1항사(관리급) -->


            <!-- 2항사 & 3항사(운항급) -->
            <div id="tab-2" class="tab-content">
                <div class="stcw-box">
                    <h4><span>항해</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>항해계획과 수행 및 선위 결정</li>
                            <li>안전한 항해당직 유지</li>
                            <li>안전한 항해를 유지하기 위한 레이다와 ARPA 사용</li>
                            <li>비상대응</li>
                            <li>해상에서의 조난신호 대응</li>
                            <li>표준해사항해용어 사용과 영어 쓰기와 말하기</li>
                            <li>시각신호 방법에 의한 정보 송수신</li>
                            <li>선박조종</li>
                        </ul>
                    </div>
                </div>

                <div class="stcw-box">
                    <h4><span>선박운항 통제와 선상의 인명관리</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>오염방지 요건의 준수 확보</li>
                            <li>선박 감항성 유지</li>
                            <li>선내 방화, 화재제어 및 소화</li>
                            <li>구명설비 운용</li>
                            <li>선내 의료응급처치 적용</li>
                            <li>법적 강제사항 준수를 감시</li>
                        </ul>
                    </div>
                </div>

                <div class="stcw-box">
                    <h4><span>화물취급과 적부</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>화물의 선적, 적부, 결속, 양하의 감시 및 항해중 화물 관리</li>
                            <li>화물 구역, 해치 커버 및 밸러스트 탱크의 결함과 손상 검사 및 보고</li>
                        </ul>
                    </div>
                </div>

                <div class="stcw-box">
                    <h4><span>무선통신</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>GMDSS 하부시스템과 장치를 이용한 정보의 송신과 수신, 또한 GMDSS의 기능적 요건의 완수</li>
                            <li>비상시 무선서비스 제공</li>
                        </ul>
                    </div>
                </div>
            </div>
            <!--// 2항사 & 3항사(운항급) -->

            <!-- 갑판부원(보조급) -->
            <div id="tab-3" class="tab-content">
                <div class="stcw-box">
                    <h4><span>항해</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>선박의 조타와 영어를 포함한 조타명령 준수</li>
                            <li>시각 및 청각에 의한 적절한 경계 유지</li>
                            <li>안전당직의 감시와 관리를 위한 기여</li>
                            <li>비상장치 운전과 비상절차의 적용</li>
                        </ul>
                    </div>
                </div>
            </div>
            <!--// 갑판부원(보조급) -->

            <!-- 기관장 & 1기사(관리급) -->
            <div id="tab-4" class="tab-content">
                <div class="stcw-box">
                    <h4><span>선박기관공학</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>계획적 및 단계적 운전</li>
                            <li>관련 시스템을 포함한 주기와 보조기계의 시동과 정지</li>
                            <li>기관성능과 용량의 운전, 감시 및 평가</li>
                            <li>기관장치, 시스템 및 서비스 안전 유지</li>
                            <li>연료와 밸러스트 운전 관리</li>
                            <li>내부 통신 시스템 이용</li>
                        </ul>
                    </div>
                </div>

                <div class="stcw-box">
                    <h4><span>선박운항 통제와 선상의 인명관리</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>트림, 복원성 및 응력의 제어</li>
                            <li>해상인명안전과 해양환경보호를 위한 법적 요건과 조치에 따른 감시와 관리</li>
                            <li>선박, 승무원, 여객의 편의와 안전 및 생존, 소화 및 기타 안전시스템의 운전조건 유지<li>
                            <li>비상 및 손상제어 계획 개발과 비상상황 취급</li>
                            <li>승무원 조직과 관리</li>
                        </ul>
                    </div>
                </div>

                <div class="stcw-box">
                    <h4><span>보수관리와 수리</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>안전한 보수관리와 수리절차 조직</li>
                            <li>기계고장 탐지와 원인 확인 및 결함 수정</li>
                            <li>안전한 작업시행 확보</li>
                        </ul>
                    </div>
                </div>

                <div class="stcw-box">
                    <h4><span>전기전자 및 제어공학</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>전기 및 전자제어 장치 운전</li>
                            <li>전기 및 전자제어 장치 시험, 고장 검출과 보수관리 및 운전상태로의 복구</li>
                        </ul>
                    </div>
                </div>
            </div>
            <!--// 기관장 & 1기사(관리급) -->

            <!-- 2기사 & 3기사(운항급) -->
            <div id="tab-5" class="tab-content">
                <div class="stcw-box">
                    <h4><span>선박기관공학</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>조립과 수리를 위한 적절한 공구 사용</li>
                            <li>장치 분해, 보수관리, 수리 및 재조립을 위한 손공구와 측정자치 사용</li>
                            <li>고장발견, 보수관리와 수리작업을 위한 손공구, 전기와 전차측정 및 시험장치 사용</li>
                            <li>안전한 기관당직 유지</li>
                            <li>영어 쓰기와 말하기</li>
                            <li>주기, 보조기계 및 관련 제어시스템 운전</li>
                            <li>펌프시스템과 관련 제어시스템 운전</li>
                        </ul>
                    </div>
                </div>

                <div class="stcw-box">
                    <h4><span>선박운항 통제와 선상의 인명관리</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>오염방지 요건의 준수 확보</li>
                            <li>선박 감항성 유지</li>
                            <li>선내 방화, 화재제어 및 소화 구명설비 운용</li>
                        </ul>
                    </div>
                </div>

                <div class="stcw-box">
                    <h4><span>보수관리와 수리</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>안전한 보수관리와 수리절차 조직</li>
                            <li>기계고장 탐지와 원인 확인 및 결함 수정</li>
                            <li>안전한 작업시행 확보</li>
                        </ul>
                    </div>
                </div>

                <div class="stcw-box">
                    <h4><span>전기전자 및 제어공학</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>전기 및 전자제어 장치 운전</li>
                            <li>전기 및 전자제어 장치 시험, 고장 검출과 보수관리 및 운전상태로의 복구</li>
                        </ul>
                    </div>
                </div>
            </div>
            <!--// 2기사 & 3기사(운항급) -->

            <!-- 기관부원(보조급) -->
            <div id="tab-6" class="tab-content">
                <div class="stcw-box">
                    <h4><span>선박기관공학</span></h4>
                    <div class="message-box default">
                        <ul class="bullet-list">
                            <li>기관당직의 일부를 구성하는 부원의 임무에 적절한 통상적 당직의 수행</li>
                            <li>지시의 이해와 당직근무 임무에 관련되는 문제 이해</li>
                            <li>보일러 당직을 유지하기 위한 올바른 수위와 증기압력의 유지</li>
                            <li>비상장치 운전과 비상절차의 적용</li>
                        </ul>
                    </div>
                </div>
            </div>
            <!--// 기관부원(보조급) -->


        </section>

</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>