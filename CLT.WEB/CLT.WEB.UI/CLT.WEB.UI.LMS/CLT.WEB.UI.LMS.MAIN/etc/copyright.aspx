<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="copyright.aspx.cs" Inherits="CLT.WEB.UI.LMS.ETC.copyright" 
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="개인정보 보호정책" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack()"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    
    <section class="privacy">
        <p class="headline">
            우리 헌법은 국민의 기본권인 사생활 비밀의 자유와 통신의 자유를 보장하고 있습니다.<br>
            이러한 기본적 권리를 철저히 보장하기 위해 TRAINING CENTER(HANJIN CREW TRAINING CENTER)(이하 회사라함)는 회원의 프라이버시를 보호하여 정보화 사회에서 통신의 자유를 누구나 장애적 요소 없이 누리게 하는데 일조 하고자 아래와 같이 개인정보 보호정책을 명시합니다.
        </p>

        <strong>1. 개인정보의 수집목적 및 이용</strong>
        <p>
            회사가 회원등록과 로그온(Log on) 자료 등을 통해 입수하게 된 회원 정보 는 외부에 공개하지 않음을 원칙으로 합니다.
            다만, 주문한 사진이나 상품 또는 기념품 등의 발송을 위해 사용되거나, 회원 개개인의 기호에 맞는 보다 차별화된
            서비스를 제공하기 위하여 사용되는 경우는 예외로 합니다.
        </p>
        <strong>2. 개인정보 항목 및 수집 방법</strong>
        <p>
            회사는 먼저 회원으로 가입하실 때 필수적으로 개인정보를 얻고 있습니다.
            이 때 기입하시는 개인정보로는 성명, 주민등록번호, 주소 등이 있으며, 이는 주문한 사진이나, 상품 또는 기념품 등의
            발송을 위하여 사용됩니다.
            어떠한 경우에도 사전에 이용자에게 밝힌 목적 외에는 이용자의 개인정보를 사용하지 않습니다.
        </p>
        <strong>3. 세션의 운용 및 활용</strong>
        <p>
            회사는 회원에게 보다 적절하고 유용한 서비스를 제공하기 위하여 세션을 사용하여 회원이 로그인(Log in)하여
            TRAINING CENTER(HANJIN CREW TRAINING CENTER) 사이트를 이용 하시는 동안 회원의 아이디(ID)에 관한 정보를
            알아냅니다.
            세션이란 쿠키와 비슷한 성격과 기능을 가지는데 쿠키와 구별되는 중요한 차이점은 쿠키가 사용자의 컴퓨터에
            저장된다면 세션은 서버 쪽에 세션데이터를 두기 때문에 보안의 측면에서 쿠키보다는 더욱 안정성이 있습니다.
        </p>
        <strong>4. 개인정보의 열람, 정정 및 삭제</strong>
        <p>
            회사는 회원의 입력하신 개인정보를 '정보수정' 란에서 언제든지 열람, 변경, 정정할 수 있도록 하고 있으며,
            개인정보 수집을 반대하는 회원에겐 개인정보 수집에 대한동의를 철회할 수 있게 하고 있습니다.
            회원탈퇴, 즉 아이디(ID)의 삭제를 원하시면, 회원가입해지의 의사를 저희 회사 에 통지하시면 됩니다.
        </p>
        <strong>5. 개인정보의 제공 및 공유</strong>
        <p>
            회사는 본인의 동의 없이 개별적인 신상 정보를 다른 개인이나 기업, 기관과 공유하지 않는 것을 원칙으로 하고
            있습니다.
            다만, 회원이 허락한 경우나 상거래 등의 이행을 위해 필요한 경우, 또는 이용 약관을 위반한 회원 에게 법적인 제재를
            주기 위한 경우에는 정보를 공개할 수 있습니다.
            하지만, 이러한 경우라도 프라이버시에 대한 충분한 검토를 거친 후에 하게 됩니다.
        </p>
        <strong>6. 개인정보의 보유 및 폐기</strong>
        <p>
            TRAINING CENTER(HANJIN CREW TRAINING CENTER) 사이트 회원으로서 회사가 제공하는 서비스를 받는 동안
            회원의 개인정보는 회사에서 계속 보유하며 서비스 제공을 위해 이용하게 됩니다.
            다만, 회원 탈퇴, 즉 ID(아이디) 삭제가 이루어진 경우 또는 개인정보의 수집목적이 달성된 경우, 관련법규의 규정에
            의한 보존의무가 없는 한 회사에서 온라인 또는 오프라인 상에 보존된 모든 개인정보 는 완전 폐기되어 사용할 수
            없도록 하고 있습니다.
        </p>
        <strong>7. 개인정보 보호</strong>
        <p>
            회사는 회원 개인의 정보와 비밀을 안전하게 지킬 수 있도록 항상 모든 기술적 수단과 노력을 다하고 있습니다.
            그러나 회원의 아이디(ID) 및 비밀 번호의 보안은 기본적으로 회원개개인의 책임 하에 있습니다.
            회사에서 개인정보에 접근할 수 있는 방법은 오직 회원의 아이디(ID) 및 비밀번호로 인한 로그인(Log in)에 의한
            방법이며, 회사는 e-mail이나 전화, 그 어떠한 방법을 통해서도 회원의 비밀번호를 묻는 경우는 없으므로
            (ID/비밀번호 분실로 인한 회원님의 요청 시 제외), 회원 본인이 보안을 위해 비밀번호를 자주 바꾸어주시기 바랍니다.
            TRAINING CENTER(HANJIN CREW TRAINING CENTER) 사이트를 이용하신 후에는 반드시 로그아웃 (Log out)을 해
            주시고, 컴퓨터를 공유하거나 공공장소에서 컴퓨터를 사용하는 경우에는 이용 후 반드시 웹 브라우저의 창을
            닫아주는 등 개인정보 유출을 막기 위해 각별히 노력을 기울여주시기 바랍니다.
        </p>

    </section>

</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>