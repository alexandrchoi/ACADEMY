<%@ Page Language="C#" MasterPageFile="/MasterMain.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CLT.WEB.UI.LMS.MAIN.Default" 
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">
	    //교육안내
        function eduNoticeUrl(rseq){		
	        saveMenuCD('6', '1', '0', '/community/edu_notice_detail.aspx?rseq='+rseq);
        }
        function eduNoticeUrlList(){		
	        saveMenuCD('6', '1', '0', '/community/edu_notice_list.aspx?BIND=BIND');
        }
	    //공지사항
        function noticeUrl(rseq){		
	        saveMenuCD('6', '2', '0', '/community/notice_detail.aspx?rseq='+rseq);
        }
        function noticeUrlList(){		
	        saveMenuCD('6', '2', '0', '/community/notice_list.aspx?BIND=BIND');
        }
        function courseOpenUrl(rseq){		
	        saveMenuCD('4', '2', '1', '/application/courseapplication_detail.aspx?ropen_course_id='+rseq);
        }
        function courseOpenUrlList(){	
            <% if(Convert.ToString(Session["USER_ID"]) == "" || Convert.ToString(Session["USER_GROUP"]) == this.GuestUserID) {%>	
	                alert('<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A123", new string[] { "" }, new string[] { "" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>');
	        <% } else { %>
	                saveMenuCD('4', '2', '1', '/application/courseapplication_list.aspx');
	        <% } %>
        }
    </script>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
<!-- 메인 컨텐츠 -->
    
    <h2 class="off-screen"><asp:Label ID="startContent" runat="server" text="본문 시작" meta:resourcekey="startContentResource" /></h2>

    <div class="main-top block section-wrap">

        <div class="main-board-box">
            <div class="main-board">

                <!-- 배너 3개 -->
                <div class="main-board-card">
                    <h3><asp:Label ID="schedEdu" runat="server" text="교육일정" meta:resourcekey="schedEduResource" /></h3>
                    <ul class="main-board-card-list block">
                        <li class="schedule">
                            <a href="javascript:" onclick="saveMenuCD('4', '4', '0', '/asset/FullCalendar/');"><asp:Label ID="schedEdu1" runat="server" CssClass="title" text="교육일정" meta:resourcekey="schedEduResource" /></a>
                        </li>
                        <li class="curriculum">
                            <a href="javascript:" onclick="saveMenuCD('3', '2', '0', '/curr/course.aspx');"><asp:Label ID="classEdu" runat="server" CssClass="title" text="교육과정" meta:resourcekey="classEduResource" /></a>
                        </li>
                        <li class="apply">
                            <a href="javascript:" onclick="saveMenuCD('4', '2', '1', '/application/courseapplication_list.aspx');"><asp:Label ID="appyEdu" runat="server" CssClass="title" text="교육신청" meta:resourcekey="appyEduResource" /></a>
                        </li>
                    </ul>
                </div>

                <!-- 게시판 2개 탭 -->
                <div class="main-board-tab">
                    <div class="tabs">
                        <div class="tab">
                            <input type="radio" name="tabs" id="tab-1" checked class="tab-switch">
                            <label for="tab-1" class="tab-label"><asp:Label ID="tab_1" runat="server" text="교육안내" meta:resourcekey="tab_1Resource" /></label>
                            <div class="tab-content">
                                <ul>
                                    <asp:Literal ID="EduNoticeList" runat="server" />
                                    <!--
                                    <li>
                                    <span class="date">
                                        <span class="date-month">dec</span>
                                        <span class="date-day">28</span>
                                    </span>
                                        <a href="#none">게시글이 어쩌고 저쩌고 미리보기 80byte 까지 잘라주세요. 이 텍스트는 공백 포함 약 40자 정...</a>
                                    </li>
                                    <li>
                                    <span class="date">
                                        <span class="date-month">dec</span>
                                        <span class="date-day">28</span>
                                    </span>
                                        <a href="#none">게시글이 어쩌고 저쩌고 미리보기 80byte 까지 잘라주세요. 이 텍스트는 공백 포함 약 40자 정...</a>
                                    </li>
                                    <li>
                                    <span class="date">
                                        <span class="date-month">dec</span>
                                        <span class="date-day">28</span>
                                    </span>
                                        <a href="#none">게시글이 어쩌고 저쩌고 미리보기 80byte 까지 잘라주세요. 이 텍스트는 공백 포함 약 40자 정...</a>
                                    </li>-->
                                </ul>
                            </div>
                        </div>

                        <div class="tab">
                            <input type="radio" name="tabs" id="tab-2" class="tab-switch">
                            <label for="tab-2" class="tab-label"><asp:Label ID="tab_2" runat="server" text="공지사항" meta:resourcekey="tab_2Resource" /></label>
                            <div class="tab-content">
                                <ul>
                                    <asp:Literal ID="NoticeList" runat="server" />
                                    <!--
                                    <li>
                                    <span class="date">
                                        <span class="date-month">dec</span>
                                        <span class="date-day">28</span>
                                    </span>
                                        <a href="#none">게시글이 어쩌고 저쩌고 미리보기 80byte 까지 잘라주세요. 이 텍스트는 공백 포함 약 40자 정...</a>
                                    </li>
                                    <li>
                                    <span class="date">
                                        <span class="date-month">dec</span>
                                        <span class="date-day">28</span>
                                    </span>
                                        <a href="#none">게시글이 어쩌고 저쩌고 미리보기 80byte 까지 잘라주세요. 이 텍스트는 공백 포함 약 40자 정...</a>
                                    </li>
                                    <li>
                                    <span class="date">
                                        <span class="date-month">dec</span>
                                        <span class="date-day">28</span>
                                    </span>
                                        <a href="#none">게시글이 어쩌고 저쩌고 미리보기 80byte 까지 잘라주세요. 이 텍스트는 공백 포함 약 40자 정...</a>
                                    </li>-->
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <!-- 메인 비주얼 -->
        <div class="main-visual-box">
            <div class="main-visual">
                <div class="visual-copy">
                    <p class="key-copy"><asp:Label ID="bhpTitle" runat="server" Text="Best HRD Provider" meta:resourcekey="bhpTitleResource" /></p>
                    <p>
                        <asp:Label ID="bhpDesc" runat="server" text="Global Ocean Leader Training Center Your Best HRD Partner! G-MARINE ACADEMY" meta:resourcekey="bhpDescResource" />
                    </p>
                </div>
            </div>
        </div>
    </div>

    <div class="main-banners-box section-fix">
        <div class="main-banners gm-grid column-3">
            <div class="item">
                <div class=" banner-1">
                    <img src="/asset/images/main/main-icon1.png" alt="아이콘">
                    <h3><asp:Label ID="entrTitle" runat="server" Text="사업주위탁훈련" meta:resourcekey="entrTitleResource" /></h3>
                    <p><asp:Label ID="entrDesc" runat="server" Text="사업소개, 신청방법, 과정안내를 확인하세요." meta:resourcekey="entrDescResource" /></p>
                    <asp:HyperLink ID="lnkEntr" runat="server" CssClass="button-main-rnd lg" Text="자세히보기" NavigateUrl="/intro/consignment.aspx" meta:resourcekey="lnkResource" />
                </div>
            </div>

            <div class="item">
                <div class=" banner-2">
                    <img src="/asset/images/main/main-icon2.png" alt="아이콘">
                    <h3><asp:Label ID="consTitle" runat="server" Text="국가인적자원개발 컨소시엄" meta:resourcekey="consTitleResource" /></h3>
                    <p><asp:Label ID="consDesc" runat="server" Text="사업소개, 신청방법, 과정안내를 확인하세요." meta:resourcekey="consDescResource" /></p>
                    <asp:HyperLink ID="lnkCons" runat="server" CssClass="button-main-rnd lg" Text="자세히보기" NavigateUrl="/intro/consortium.aspx" meta:resourcekey="lnkResource" />
                </div>
            </div>
            <div class="item">
                <div class=" banner-3">
                    <img src="/asset/images/main/main-icon3.png" alt="아이콘">
                    <h3><asp:Label ID="refundTitle" runat="server" Text="고용보험환급" meta:resourcekey="refundTitleResource" /></h3>
                    <p><asp:Label ID="refundDesc" runat="server" Text="고용보험환급 신청방법을 자세히 알아보세요." meta:resourcekey="refundDescResource" /></p>
                    <asp:HyperLink ID="lnkRefund" runat="server" CssClass="button-main-rnd lg" Text="자세히보기" NavigateUrl="/intro/refund.aspx" meta:resourcekey="lnkResource" />
                </div>
            </div>
        </div>
    </div>


    <div class="fixed-bg section-wrap">
        <div class="fixed-box">
            <h3 class="title"><span>best hrd</span><br>provider</h3>
            <p><asp:Label ID="bhpDesc1" runat="server" text="2019년도 인적자원개발 우수기관으로 선정" meta:resourcekey="bhpDesc1Resource" /></p>
            <asp:HyperLink ID="lnkbhp" runat="server" CssClass="button-underline lt" Text="자세히보기" NavigateUrl="/community/notice_detail.aspx?rseq=237&MenuCode=620&delYN=N" meta:resourcekey="lnkResource" />
        </div>
    </div>

    <div class="section-fix gm-value-box section-wrap">
        <h3>Top Ocean Leader</h3>
        <p><asp:Label ID="tolDesc" runat="server" text="지마린 아카데미에서는 다양하고 체계적인 훈련 커리큘럼을 통해 선원의 역량과 자질을 높은 수준으로 유지하고 있습니다." meta:resourcekey="tolDescResource" /></p>
        <div class="gm-value gm-grid column-4">
            <div class="item">
                <span class="numeric" data-rate="8">8</span>
                <span class="title-en">Primary course</span>
                <asp:Label ID="pcDesc" runat="server" CssClass="title-ko" text="8개 기본 교육코스" meta:resourcekey="pcDescResource" />
            </div>
            <div class="item">
                <span class="numeric" data-rate="41">41</span>
                <span class="title-en">Job competency course</span>
                <asp:Label ID="jcDesc" runat="server" CssClass="title-ko" text="41개 직무역량코스" meta:resourcekey="jcDescResource" />
            </div>
            <div class="item">
                <span class="numeric" data-rate="3">3</span>
                <span class="title-en">Leadership course</span>
                <asp:Label ID="lcDesc" runat="server" CssClass="title-ko" text="3개 리더십코스" meta:resourcekey="lcDescResource" />
            </div>
            <div class="item">
                <span class="numeric" data-rate="6">6</span>
                <span class="title-en">Selected course</span>
                <asp:Label ID="scDesc" runat="server" CssClass="title-ko" text="6개 해양수산부 지정 교육과정" meta:resourcekey="scDescResource" />
            </div>
        </div>
        <asp:HyperLink ID="lnkEduList" runat="server" CssClass="button-main-rnd lg" Text="자세히보기" NavigateUrl="/curr/curriculum.aspx" meta:resourcekey="lnkResource" />
    </div>

</asp:Content>
