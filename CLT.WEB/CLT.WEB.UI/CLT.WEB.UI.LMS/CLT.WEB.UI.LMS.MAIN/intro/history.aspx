<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="history.aspx.cs" Inherits="CLT.WEB.UI.LMS.INTRO.history" 
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

    <!-- 서브 컨텐츠 시작 -->
    <div class="section-fix">

        <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="연혁" meta:resourcekey="lblMenuTitle" />
            <!-- 모바일 뒤로 가기 -->
            <span class="goback">
			    <button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
			</span>
        </h2>

        <section class="history sub-design">
            <ol class="block">
                <li>
                    <div>
                        <div class="month">2019</div>
                        <ol>
                            <li><asp:Label ID="lblYear20191" runat="server" Text="국가인적자원개발컨소시엄 사업 선정(10.1일부)" meta:resourcekey="lblYear20191Resource" /></li>
                            <li><asp:Label ID="lblYear20192" runat="server" Text="인적자원개발 우수기관(BEST HRD) 선정" meta:resourcekey="lblYear20192Resource" /></li>
                        </ol>
                    </div>
                </li>

                <li>
                    <div>
                        <div class="month">2018</div>
                        <ol>
                            <li><asp:Label ID="lblYear20181" runat="server" Text="직업능력개발훈련기관 인증평가 3년인증(우수기관)" meta:resourcekey="lblYear20181Resource" /></li>
                            <li><asp:Label ID="lblYear20182" runat="server" Text="7.2일부 사옥 이전, 교육장 이전 설비" meta:resourcekey="lblYear20182Resource" /></li>
                        </ol>
                    </div>
                </li>

                <li>
                    <div>
                        <div class="month">2017</div>
                        <ol>
                            <li><asp:Label ID="lblYear20171" runat="server" Text="일학습병행제 사업 참여(총무인사_L3)" meta:resourcekey="lblYear20171Resource" /></li>
                            <li><asp:Label ID="lblYear20172" runat="server" Text="현대자동차그룹 계열사 인수합병(9. 1일부), 사명((주)유수에스엠 → (주)지마린서비스) 변경" meta:resourcekey="lblYear20172Resource" /></li>
                            <li><asp:Label ID="lblYear20173" runat="server" Text="고용노동부장관 표창(직업능력개발의 달)" meta:resourcekey="lblYear20173Resource" /></li>
                        </ol>
                    </div>
                </li>

                <li>
                    <div>
                        <div class="month">2016</div>
                        <ol>
                            <li><asp:Label ID="lblYear20161" runat="server" Text="청년취업아카데미 사업 참여기업/훈련기관(2011~2016년)" meta:resourcekey="lblYear20161Resource" /></li>
                            <li><asp:Label ID="lblYear20162" runat="server" Text="인적자원개발 우수기관(BEST HRD) 선정" meta:resourcekey="lblYear20162Resource" /></li>
                        </ol>
                    </div>
                </li>

                <li>
                    <div>
                        <div class="month">2014</div>
                        <ol>
                            <li><asp:Label ID="lblYear20141" runat="server" Text="국가직무능력표준(NCS)개발 주도(항해, 선박기관운전)" meta:resourcekey="lblYear20141Resource" /></li>
                        </ol>
                    </div>
                </li>

                <li>
                    <div>
                        <div class="month">2013</div>
                        <ol>
                            <li><asp:Label ID="lblYear20131" runat="server" Text="국토해양부 해양안전심판원 준해양사고 우수선사 선정" meta:resourcekey="lblYear20131Resource" /></li>
                        </ol>
                    </div>
                </li>

                <li>
                    <div>
                        <div class="month">2012</div>
                        <ol>
                            <li><asp:Label ID="lblYear20121" runat="server" Text="외국 교육기관 설립 및 운영(필리핀, 미얀마, 인도네시아)" meta:resourcekey="lblYear20121Resource" /></li>
                        </ol>
                    </div>
                </li>

                <li>
                    <div>
                        <div class="month">2011</div>
                        <ol>
                            <li><asp:Label ID="lblYear20111" runat="server" Text="청년취업아카데미 사업 참여기업/훈련기관(1차년도)" meta:resourcekey="lblYear20111Resource" /></li>
                        </ol>
                    </div>
                </li>

                <li>
                    <div>
                        <div class="month">2010</div>
                        <ol>
                            <li><asp:Label ID="lblYear20101" runat="server" Text="운항훈련원 설립, 고용노동부 사업주 위탁훈련 시작" meta:resourcekey="lblYear20101Resource" /></li>
                        </ol>
                    </div>
                </li>

                <li>
                    <div>
                        <div class="month">2008</div>
                        <ol>
                            <li><asp:Label ID="lblYear20081" runat="server" Text="국토해양부 지정교육기관 지정" meta:resourcekey="lblYear20081Resource" /></li>
                        </ol>
                    </div>
                </li>

                <li>
                    <div>
                        <div class="month">2006</div>
                        <ol>
                            <li><asp:Label ID="lblYear20061" runat="server" Text="(주)한진에스엠 창립" meta:resourcekey="lblYear20061Resource" /></li>
                        </ol>
                    </div>
                </li>

            </ol>
        </section>

    </div>
    <!--// 서브 컨텐츠 끝 -->


</asp:Content>