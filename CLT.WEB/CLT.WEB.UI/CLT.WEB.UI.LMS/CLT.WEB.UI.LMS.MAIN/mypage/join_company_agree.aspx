﻿<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="join_company_agree.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.join_company_agree" 
    Culture="auto" UICulture="auto" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.FX.UTIL" Assembly="CLT.WEB.UI.FX.UTIL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    
    <script type="text/javascript" language="javascript">
		function GoJoinForm()
        {
            if (!document.getElementById("agree_chk").checked) {
                alert("이용약관에 동의하셔야 가입이 가능합니다");
                return false;
            }
            else if (!document.getElementById("agree_chk2").checked) {
                alert("개인정보 수집 및 이용에 동의하셔야 가입이 가능합니다");
                return false;
            }
            else {
                location.href="join_company_reg.aspx?CompanyName=<%=Request.QueryString["CompanyName"]%>&RegNo=<%=Request.QueryString["RegNo"]%>";
            }
            return true;
        }

        var all_allow = function(el){
	        var check = $(el).is(':checked');
            $('#agree_chk').prop('checked',check);
            $('#agree_chk2').prop('checked',check);
            $('#ad_type').prop('checked',check);
        };
    </script>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <asp:HiddenField ID="backURL" runat="server" />

    <!-- 서브 컨텐츠 시작 -->
    <div class="section-fix">
        <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="회원가입" meta:resourcekey="lblMenuTitle"></asp:Label>
            <!-- 모바일 뒤로 가기 -->
            <span class="goback">
			        <button onclick="goBack();return false;"><span class="off-screen">Go back</span></button>
			    </span>
        </h2>
        

        <section class="join-agree">
            <div class="agree-box">
                <h3 class="title">이용약관</h3>
                <div class="message-box white box-hide-y">
                    <h3>제1장 총칙</h3>
                    <h4>제1조 목적</h4>
                    <p>이 약관은 지마린서비스(이하 지마린아카데미)가 제공하는 모든 서비스의 이용조건 및 절차에 관한 사항과 기타 필요한 사항을 전기통신사업법 및 동법 시행령이 정하는 대로 준수하고 규정함을 목적으로 합니다.</p>
                    <h4>제2조 약관의 효력과 변경</h4>
                    <ol class="list-order-sm">
                        <li>이 약관은 이용자에게 공지사항을 통해 공시함으로써 효력을 발생합니다.</li>
                        <li>지마린아카데미는 운영상 중요 사유가 있을 때 약관을 변경할 수 있으며, 변경된 약관은 전항과 같은 방법으로 효력을 발생합니다.</li>
                    </ol>
                    <h4>제3조 약관 외 준칙</h4>
                    <p>이 약관에 명시되지 않은 사항이 관계 법령에 규정되어 있을 경우에는 그 규정에 따릅니다.</p>
                    <h3>제2장 회원 가입과 서비스 이용</h3>
                    <h4>제1조 서비스 이용 계약의 성립</h4>
                    <ol class="list-order-sm">
                        <li>이용 계약은 이용자의 이용 신청에 대한 지마린아카데미의 이용 승낙과 이용자의 약관 내용에 대한 동의로 성립됩니다.</li>
                        <li>회원에 가입하여 서비스를 이용하고자 하는 희망자(기업)는 지마린아카데미에서 요청하는 개인 신상정보를 제공해야 합니다.</li>
                        <li>지마린아카데미는 다음 각 호에 해당하는 이용계약 신청에 대하여는 이를 승낙하지 아니 합니다.
                            <ul class="dash-list">
                                <li>다른 사람의 명의를 사용하여 신청하였을 때</li>
                                <li>본인의 실명으로 신청하지 않았을 때</li>
                                <li>이용 계약 신청서의 내용을 허위로 기재하였을 때</li>
                                <li>사회의 안녕과 질서 혹은 미풍양속을 저해할 목적으로 신청하였을 때</li>
                            </ul>
                        </li>
                    </ol>
                    <h4>제2조 서비스 이용 및 제한</h4>
                    <ol class="list-order-sm">
                        <li>서비스 이용은 지마린아카데미 업무상 또는 기술상 특별한 지장이 없는 한 연중무휴, 1일 24시간을 원칙으로 합니다.</li>
                        <li>전항의 서비스 이용시간은 시스템 정기점검 등 지마린아카데미가 필요한 경우, 회원에게 사전 통지 없이, 제한할 수도 있습니다.</li>
                    </ol>
                    <h3>제3장 의무</h3>
                    <h4>제 1조 지마린아카데미의 의무</h4>
                    <ol class="list-order-sm">
                        <li>지마린아카데미는 특별한 사정이 없는 한 회원이 신청한 서비스 제공 개시일에 서비스를 이용할 수 있도록 합니다.</li>
                        <li>지마린아카데미는 이 약관에서 정한 바에 따라 계속적, 안정적으로 서비스를 제공할 의무가 있습니다.</li>
                        <li>지마린아카데미는 회원으로부터 소정의 절차에 의해 제기되는 의견에 대해서는 적법한 절차를 거쳐 지원할 수 있습니다.</li>
                        <li>지마린아카데미는 회원의 정보를 철저히 보안 유지하며, 양질의 서비스를 운영하거나 개선하는 데에만 사용하고, 이외의 다른 목적으로 타 기관 및 개인에게 양도하지 않습니다.</li>
                    </ol>
                    <h4>제 2조 회원의 의무</h4>
                    <ol class="list-order-sm">
                        <li>ID와 비밀번호에 관한 모든 관리의 책임은 회원에게 있습니다.</li>
                        <li>자신의 ID가 부정하게 사용된 경우, 회원은 반드시 지마린아카데미에게 그 사실을 통보해야 합니다.</li>
                        <li>회원은 이 약관 및 관계 법령에서 규정한 사항을 준수하여야 합니다.</li>
                    </ol>
                    <h3>제4장 계약 해지 및 서비스 이용 제한</h3>
                    <ol class="list-order-sm">
                        <li>회원이 이용 계약을 해지하고자 하는 때에는 회원 본인이 직접 지마린아카데미 해지 서비스를 이용하여 서비스 해지 신청을 요청해야 합니다.</li>
                        <li>이름, 주민등록번호, ID를 입력하여 본인임을 확인한 후 , 해지 확인을 선택하면 자동으로 가입 해지됩니다.</li>
                        <li>가입 해지 여부는 기존의 ID,비밀번호로 로그인이 되지 않으면 해지된 것이며, 한번 해지된 ID는 기존 사용자라도 사용할 수 없습니다.</li>
                        <li>지마린아카데미는 회원이 다음 사항에 해당하는 행위를 하였을 경우, 사전 통지 없이 이용 계약을 해지하거나 또는 서비스 이용을 중지할 수 있습니다.
                            <ul class="dash-list">
                                <li>공공 질서 및 미풍 양속에 반하는 경우</li>
                                <li>범죄적 행위에 관련되는 경우</li>
                                <li>국익 또는 사회적 공익을 저해할 목적으로 서비스 이용을 계획 또는 이용 할 경우</li>
                                <li>타인의 ID 및 비밀번호를 도용한 경우</li>
                                <li>타인의 명예를 손상시키거나 불이익을 주는 경우</li>
                                <li>같은 사용자가 다른 ID로 이중 등록을 한 경우</li>
                                <li>서비스에 해를 가하는 등 건전한 이용을 저해하는 경우</li>
                                <li>기타 관련 법령이나 박물관이 정한 이용조건에 위배되는 경우</li>
                            </ul>
                        </li>
                        <li>지마린아카데미는 긴급하게 이용을 중지해야 할 필요가 있을 경우 그 사유에 대한 통지절차 없이 서비스 이용을 제한할 수 있습니다.</li>
                    </ol>
                    <h3>제5장 회원의 게시물 관리</h3>
                    <p>지마린아카데미는 서비스에 회원이 게시하거나 등록한 내용물이 다음 사항에 해당된다고 판단되는 경우에 사전 통지 없이 삭제할 수 있습니다.</p>
                    <ol class="list-order-sm">
                        <li>타인을 비방하거나 중상모략으로 개인 및 단체의 명예를 손상시키는 내용인 경우</li>
                        <li>공공질서 및 미풍양속에 위반되는 내용인 경우</li>
                        <li>범죄적 행위에 부합된다고 인정되는 내용인 경우</li>
                        <li>타인의 저작권 등 기타의 권리를 침해하는 내용인 경우</li>
                        <li>기타 관계 법령이나 회사에서 정한 규정에 위배되는 내용인 경우</li>
                    </ol>
                </div>
                <div class="agree right">
                    <label class="checkbox-box">[필수] 이용약관에 동의합니다.
                        <input type="checkbox" name="agree_chk" id="agree_chk" value="1">
                        <span class="checkmark"></span>
                    </label>
                </div>
            </div>

            <div class="agree-box">
                <h3 class="title">개인정보 수집 및 이용</h3>
                <div class="message-box white box-hide-y">
                    <ol class="list-order">
                        <li>개인정보의 수집•이용 목적
                            <ul class="dash-list">
                                <li>민원신청, 교육예약, 교육이수현황 조회 등 서비스 제공에 관련한 목적으로 개인정보를 처리(수집•이용)합니다.</li>
                            </ul>
                        </li>
                        <li>수집•이용하려는 개인정보의 항목
                            <ul class="dash-list">
                                <li>필수 : 회사명, 대표자명, 사업자등록번호, 고용보험관리번호, 업종, 회사구분, 회사규모, 전화번호, 팩스번호, 대표이메일, 주소, 홈페이지, 담당자 성명(한글), 담당자 성명(영문), 부서, 직급, 휴대전화번호, 이메일, 아이디, 비밀번호</li>
                                <li>선택 : 근로자수, 담당자 전화번호, 담당자 고용보험취득일, 증명사진업로드</li>
                            </ul>
                        </li>
                        <li>개인정보의 보유 및 이용 기간
                            <ul class="dash-list">
                                <li>개인정보파일명 : 홈페이지 회원정보</li>
                                <li>보유 및 이용 기간 : 회원 탈퇴 시 까지</li>
                            </ul>
                        </li>
                        <li>이용자 개인정보보호를 위하여 수집된 개인정보는 암호화되어 처리됩니다.
                            <ul class="dash-list">
                                <li>이용자는 해당 개인정보 수집 및 이용 동의에 대한 거부 권리가 있습니다.(단, 개인정보 수집•이용에 대한 동의를 하지 않으실 경우에는 민원신청•교육예약 및 교육이수현황조회 불가 등의 불이익이 발생하게 됩니다.)
                                </li>
                                <li>선택 항목은 보다 나은 홈페이지 서비스 제공을 위한 통계정보로 활용되며, 선택항목을 제공하지 않는다는 이유로 그 어떤 불이익도 없음을 알려드립니다.</li>
                            </ul>
                        </li>
                    </ol>
                </div>
                <div class="agree right">
                    <label class="checkbox-box">[필수] 개인정보 수집 및 이용에 동의합니다.
                    <input type="checkbox" name="agree_chk2" id="agree_chk2" value="1">
                    <span class="checkmark"></span>
                    </label>
                </div>
            </div>

            <div class="agree-box">
                <h3 class="title">개인정보 제3자 제공</h3>
                <div class="message-box white box-hide-y">
                    <ol class="list-order">
                        <li>지마린아카데미는 이용자의 개인정보를 원칙적으로 외부에 제공하지 않습니다. 다만, 다음의 경우에는 예외로 개인정보를 제공하는 것이 가능합니다.
                            <ul class="dash-list">
                                <li>이용자들이 사전에 동의한 경우</li>
                                <li>법률에 특별한규정이 있거나 법령상 의무를 준수하기 위하여 불가피한 경우</li>
                                <li>통계작성/학술연구 또는 시장조사를 위하여 필요한 경우로서 특정 개인을 알아볼 수 없는 형태로 가공하여 제공하는 경우</li>
                                <li>서비스제공에 따른 요금정산을 위하여 필요한 경우</li>
                            </ul>
                        </li>
                        <li>지마린아카데미는 연계 또는 제공에 의하여 개인정보를 제공받은 기관이 지마린아카데미의 동의 없이 개인정보를 제3자에게 제공하여 제3자가 이용하고 있는 것을 인지한 즉시 아래의 사항을 지체없이 이행할 것입니다.
                            <ul class="dash-list">
                                <li>해당기관에 대해 동의없이 제공한 사실을 확인</li>
                                <li>해당 개인정보의 제공 중단과 이용 금지 요구</li>
                                <li>동의없이 제공한 행위에 대한 소명 요청</li>
                                <li>필요한 경우 정보주체에 대하여 해당 사항을 통지</li>
                            </ul>
                        </li>
                        <li>지마린아카데미는 이용자가 교육서비스 등을 이용할 경우, 정부의 직업능력개발훈련제도 운영 및 개인정보 보호법에 따라 다음과 같이 개인정보를 제3자에게 제공하고 있습니다.</li>
                    </ol>

                    <h3>개인정보 제3자 제공 현황</h3>
                    <div class="gm-table data-table">
                        <table>
                            <colgroup>
                                <col width="15%">
                                <col width="15%">
                                <col width="15%">
                                <col width="15%">
                                <col width="25%">
                                <col width="15%">
                            </colgroup>
                            <thead>
                            <tr>
                                <th scope="col">개인정보파일명</th>
                                <th scope="col">정보제공기관</th>
                                <th scope="col">제공근거</th>
                                <th scope="col">이용목적</th>
                                <th scope="col">개인정보항목</th>
                                <th scope="col">보유 및 이용기간</th>
                            </tr>
                            </thead>
                            <tbody>
                            <tr>
                                <td class="center">교육생정보</td>
                                <td class="center">고용노동부(HRD-net)</td>
                                <td class="center">고용보험법 근로자직업능력개발법</td>
                                <td class="center">교육비 고용보험 환급, 심사</td>
                                <td class="center">성명, 주민등록번호, 교육과정, 전화번호, 이메일, 출결현황 등</td>
                                <td class="center">3년</td>
                            </tr>
                            <tr>
                                <td class="center">교육생정보</td>
                                <td class="center">한국고용정보원(HRD-net)</td>
                                <td class="center">고용보험법 근로자직업능력개발법</td>
                                <td class="center">교육비 고용보험 환급, 심사</td>
                                <td class="center">성명, 주민등록번호, 교육과정, 전화번호, 이메일, 출결현황 등</td>
                                <td class="center">3년</td>
                            </tr>
                            <tr>
                                <td class="center">교육생정보</td>
                                <td class="center">한국산업인력공단(HRD-net)</td>
                                <td class="center">고용보험법 근로자직업능력개발법</td>
                                <td class="center">교육비 고용보험 환급, 심사</td>
                                <td class="center">성명, 주민등록번호, 교육과정, 전화번호, 이메일, 출결현황 등</td>
                                <td class="center">3년</td>
                            </tr>
                            <tr>
                                <td class="center">회사정보</td>
                                <td class="center">고용노동부(HRD-net)</td>
                                <td class="center">고용보험법 근로자직업능력개발법</td>
                                <td class="center">교육비 고용보험 환급, 심사</td>
                                <td class="center">회사명, 사업자등록번호, 대표자명, 교육과정, 전화번호, 이메일 등</td>
                                <td class="center">3년</td>
                            </tr>
                            <tr>
                                <td class="center">회사정보</td>
                                <td class="center">한국고용정보원(HRD-net)</td>
                                <td class="center">고용보험법 근로자직업능력개발법</td>
                                <td class="center">교육비 고용보험 환급, 심사</td>
                                <td class="center">회사명, 사업자등록번호, 대표자명, 교육과정, 전화번호, 이메일 등</td>
                                <td class="center">3년</td>
                            </tr>
                            <tr>
                                <td class="center">회사정보</td>
                                <td class="center">한국산업인력공단(HRD-net)</td>
                                <td class="center">고용보험법 근로자직업능력개발법</td>
                                <td class="center">교육비 고용보험 환급, 심사</td>
                                <td class="center">회사명, 사업자등록번호, 대표자명, 교육과정, 전화번호, 이메일 등</td>
                                <td class="center">3년</td>
                            </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div class="agree right">
                    <label class="checkbox-box">[선택] 개인정보 제3자 제공에 동의합니다.
                        <input type="checkbox" name="ad_type" id="ad_type" value="0">
                        <span class="checkmark"></span>
                    </label>
                </div>
            </div>

            <div class="agree-box center">
                <div class="agree mb30">
                    <label class="checkbox-box">[선택] 위의 필수 및 선택 항목에 관한 내용을 모두 확인하였으며 이에 모두 동의합니다.
                        <input type="checkbox" id="agree_all" class="gt-checkbox" onchange="all_allow(this)">
                        <span class="checkmark"></span>
                    </label>
                </div>
                <a href="javascript:" onclick="return GoJoinForm();" class="button-main-rnd lg blue">확인</a>
            </div>

        </section>


    </div>
    <!--// 서브 컨텐츠 끝 -->


</asp:Content>