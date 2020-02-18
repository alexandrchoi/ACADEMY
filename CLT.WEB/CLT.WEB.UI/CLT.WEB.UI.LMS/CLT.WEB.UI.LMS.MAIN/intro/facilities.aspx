<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="facilities.aspx.cs" Inherits="CLT.WEB.UI.LMS.INTRO.facilities" 
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="교육시설" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack()"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    <section class="training">
        <h3><asp:Label ID="lblFacility" runat="server" Text="교육훈련시설" meta:resourcekey="lblFacilityResource" /></h3>
        <div class="classroom">
            <ul class="gm-grid column-2">
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/classroom1.jpg">
                        <figcaption><asp:Label ID="lblPCTC" runat="server" Text="PCTC RM(101)" meta:resourcekey="lblPCTCResource" /></figcaption>
                    </figure>
                </li>
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/classroom2.jpg">
                        <figcaption><asp:Label ID="lblENGINE" runat="server" Text="ENGINE RM(102)" meta:resourcekey="lblENGINEResource" /></figcaption>
                    </figure>
                </li>
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/classroom3.jpg">
                        <figcaption><asp:Label ID="lblSHS" runat="server" Text="SHS RM(103)" meta:resourcekey="lblSHSResource" /></figcaption>
                    </figure>
                </li>
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/classroom4.jpg">
                        <figcaption><asp:Label ID="lblCBT" runat="server" Text="CBT RM(104)" meta:resourcekey="lblCBTResource" /></figcaption>
                    </figure>
                </li>
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/classroom5.jpg">
                        <figcaption><asp:Label ID="lblGMDSS" runat="server" Text="GMDSS RM(105)" meta:resourcekey="lblGMDSSResource" /></figcaption>
                    </figure>
                </li>
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/classroom6.jpg">
                        <figcaption><asp:Label ID="lblRestArea" runat="server" Text="휴게실" meta:resourcekey="lblRestAreaResource" /></figcaption>
                    </figure>
                </li>
            </ul>
        </div>

        <h3><asp:Label ID="lblTrEq" runat="server" Text="교육훈련장비" meta:resourcekey="lblTrEqResource" /></h3>
        <div class="equipments">
            <ul class="gm-grid column-2">
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/equip1.jpg">
                        <figcaption><asp:Label ID="lblSSPS" runat="server" Text="선박모의조종 시뮬레이터" meta:resourcekey="lblSSPSResource" /></figcaption>
                    </figure>
                </li>
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/equip2.jpg">
                        <figcaption><asp:Label ID="lblECS" runat="server" Text="전자해도시스템" meta:resourcekey="lblECSResource" /></figcaption>
                    </figure>
                </li>
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/equip3.jpg">
                        <figcaption><asp:Label ID="lblRAMPSim" runat="server" Text="RAMP 시뮬레이터" meta:resourcekey="lblRAMPSimResource" /></figcaption>
                    </figure>
                </li>
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/equip4.jpg">
                        <figcaption><asp:Label ID="DeckLFSim" runat="server" Text="DECK LIFTER 시뮬레이터" meta:resourcekey="DeckLFSimResource" /></figcaption>
                    </figure>
                </li>
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/equip5.jpg">
                        <figcaption><asp:Label ID="lblEngSim" runat="server" Text="기관실 시뮬레이터" meta:resourcekey="lblEngSimResource" /></figcaption>
                    </figure>
                </li>
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/equip6.jpg">
                        <figcaption><asp:Label ID="lblOws" runat="server" Text="유수분리장치" meta:resourcekey="lblOwsResource" /></figcaption>
                    </figure>
                </li>
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/equip7.jpg">
                        <figcaption><asp:Label ID="lblPpc" runat="server" Text="주기관 공압 제어장치" meta:resourcekey="lblPpcResource" /></figcaption>
                    </figure>
                </li>
                <li class="item">
                    <figure>
                        <img src="/asset/images/sub/equip8.jpg">
                        <figcaption><asp:Label ID="lblCo2" runat="server" Text="고정식 CO2 소화장치" meta:resourcekey="lblCo2Resource" /></figcaption>
                    </figure>
                </li>
            </ul>
        </div>


    </section>
</div>
<!--// 서브 컨텐츠 끝 -->

</asp:Content>