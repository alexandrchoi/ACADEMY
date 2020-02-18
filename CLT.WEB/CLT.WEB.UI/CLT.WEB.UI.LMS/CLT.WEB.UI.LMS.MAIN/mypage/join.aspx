<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="join.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.join" 
    Culture="auto" UICulture="auto" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.FX.UTIL" Assembly="CLT.WEB.UI.FX.UTIL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    
    <script type="text/javascript" language="javascript">

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

        <section class="join">
            <p class="headline center mb30">유형에 따라 가입 절차가 다르니 해당하는 유형을 선택해 주세요.</p>
            <div class="join-box">
                <div class="join-personal">
                    <h4>개인회원 이용서비스</h4>
                    <ol class="list-order">
                        <li>약관동의</li>
                        <li>개인정보 입력</li>
                        <li>회원가입 신청완료</li>
                    </ol>
                    <a href="/mypage/join_user_agree.aspx" class="button-main-rnd flex100 green">개인회원 가입하기</a>
                </div>
                <div class="join-business">
                    <h4>기업회원 이용서비스</h4>
                    <ol class="list-order">
                        <li>회원중복체크</li>
                        <li>약관동의</li>
                        <li>회사/담당자정보 입력</li>
                        <li>회원가입 신청완료</li>
                    </ol>
                    <a href="/mypage/join_company_check.aspx" class="button-main-rnd flex100 pink">기업회원 가입하기</a>
                </div>
            </div>
        </section>




    </div>
    <!--// 서브 컨텐츠 끝 -->


</asp:Content>