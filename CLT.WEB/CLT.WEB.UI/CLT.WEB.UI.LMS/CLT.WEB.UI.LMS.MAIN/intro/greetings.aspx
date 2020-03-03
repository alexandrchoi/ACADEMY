<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="greetings.aspx.cs" Inherits="CLT.WEB.UI.LMS.INTRO.greetings" 
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-cover greeting">
    <h2 class="page-title off-screen"><asp:Label ID="lblMenuTitle" runat="server" Text="인사말" meta:resourcekey="lblMenuTitle" />
    </h2>

    <section class="sub-design">
        <div class="greeting-cont">
            <p class="headline">
                Global Ocean Leader Training Center<br>
                <strong>Your Best HRD Partner!</strong>
            </p>
            <p class="welcome-text">지마린 아카데미에 오신 것을 환영합니다.</p>
            <p>
                고품질 해운 서비스의 지속적인 창출은 우수한 해기역량을 보유한 승무원에 의해서만 가능합니다.<br>
                체계화된 실무 중심의 교육훈련으로 핵심 해기역량을 보유한 승무원이야 말로 선박과 인명의 안전, 그리고 소중한 고객의 화물을 위험으로 부터 지켜내는 안전운항의 원동력입니다.<br><br>
                이러한 해기역량을 갖춘 인재 확보는 지속 가능 경영의 초석이 되어, 우리 미래를 굳건히 받쳐 줄 것입니다.<br>
                지마린아카데미는 고객사 여러분의 가치와 성과를 위한 실무중심의 교육, 열린 리더십을 바탕으로 사람을 통한 꿈을 이루어내는 해기전문 HRD 파트너로 항상 함께 할 것입니다.<br><br>
                한곳에 머무르지 않는 바다처럼 끊임없는 변화와 혁신, 사회와 기업이 필요로 하는 인재들을 Global Ocean Leader로 육성함에 지마린 아카데미가 최선을 다 할 것을 약속드립니다.<br><br>
                감사합니다.
            </p>
        </div>
        <div class="signature"><span class="off-screen">대표 황창국 서명</span></div>
    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>