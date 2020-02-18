using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

// 필수 using 문
using System.Globalization;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using IAMVIRTUALAGENTCOMLib;
using System.Linq;

namespace CLT.WEB.UI.LMS.MAIN
{
    /// <summary>
    /// 1. 작업개요 : Sub Master Class
    /// 
    /// 2. 주요기능 : 웹사이트 레이아웃
    ///				  
    /// 3. Class 명 : MasterSub
    /// 
    /// 4. 작 업 자 : 
    /// </summary>
    public partial class MasterSub : BaseMasterPage
    {
        DataTable iDt = new DataTable();
        public static string iFileNm, iSubNm = "";
        public static string iPageTitle = "";

        /************************************************************
       * Function name : Page_Load
       * Purpose       : 페이지 로드될 때 처리
       * Input         : void
       * Output        : void
       *************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Lang"] != null && Request.QueryString["Lang"].ToString() != "")
            {
                Session["Language"] = Convert.ToString(Request.QueryString["Lang"]);
                SetDefaultCulture(new CultureInfo(Session["Language"].ToString() == "EN" ? "en-US" : "ko-KR"));
                var nvc = HttpUtility.ParseQueryString(Request.Url.Query);
                nvc.Remove("Lang");
                Response.Redirect(Request.Url.AbsolutePath + "?" + nvc.ToString());
            }
            else if (Session["Language"] != null && Session["Language"].ToString().Trim() != GetCultureName())
            {
                SetDefaultCulture(new CultureInfo(Session["Language"].ToString() == "EN" ? "en-US" : "ko-KR"));
                //Response.Redirect(Request.RawUrl.Replace("Lang=" + Session["Language"], ""));
            }

            if (Request.QueryString["MenuCode"] != null && Request.QueryString["MenuCode"].ToString() != "")
            {
                if (Request.QueryString["MenuCode"].ToString().Length == 3)
                {
                    Session["MENU_CODE"] = Request.QueryString["MenuCode"];
                    Session["MENU_CODE1"] = Session["MENU_CODE"].ToString().Substring(0, 1);
                    Session["MENU_CODE2"] = Session["MENU_CODE"].ToString().Substring(1, 1);
                    Session["MENU_CODE3"] = Session["MENU_CODE"].ToString().Substring(2, 1);
                }
                if (Session["MENU_CODE"].ToString().Length != 3) Session["MENU_CODE"] = "000";
            }

            try
            {
                iFileNm = getFileName();
                iSubNm = getSubName();
                //
                HtmlHead head = (System.Web.UI.HtmlControls.HtmlHead)this.Page.Header;
                head.Controls.Add(new LiteralControl("\r\n"));
                //
                HtmlLink[] xHtmlLnk = new HtmlLink[7];
                string[] xCssFiles = { "css/common/reset", "css/jquery-ui.min", "css/common/layout",
                    (iFileNm == "" && iSubNm == "" ? "css/main/main" : "css/sub/" + iSubNm)
                    , "css/common/responsive", "css/common/jquery.mCustomScrollbar"}; //, "fontawesome/css/all"
                for (int i = 0; i < 6; i++)
                {
                    xHtmlLnk[i] = new HtmlLink();
                    xHtmlLnk[i].Attributes.Add("rel", "stylesheet");
                    xHtmlLnk[i].Attributes.Add("type", "text/css");
                    xHtmlLnk[i].Attributes.Add("media", "all");
                    xHtmlLnk[i].Href = ResolveUrl("/asset/" + xCssFiles[i] + ".css"); ;
                    head.Controls.Add(xHtmlLnk[i]);
                    head.Controls.Add(new LiteralControl("\r\n"));
                }
                //
                HtmlGenericControl[] xHtmlGC = new HtmlGenericControl[6];
                string[] xJssFiles = { "jquery.min", "jquery-ui.min", "common/util", "common/layout", "common/jquery.mCustomScrollbar.concat.min",
                    (iFileNm == "" && iSubNm == "" ? "main/main" : "sub/sub")
                    };
                for (int i = 0; i < 6; i++)
                {
                    xHtmlGC[i] = new HtmlGenericControl();
                    xHtmlGC[i].TagName = "script";
                    xHtmlGC[i].Attributes.Add("type", "text/javascript");
                    //xHtmlGC[i].Attributes.Add("language", "javascript");
                    xHtmlGC[i].Attributes.Add("src", "/asset/js/" + xJssFiles[i] + ".js");
                    head.Controls.Add(xHtmlGC[i]);
                    head.Controls.Add(new LiteralControl("\r\n"));
                }
                //
                //string xLang = GetCultureName();
                //Session["Language"] = ibtnLang.CommandName = xLang;
                //ibtnLang.ImageUrl = "/asset/images/btn_" + (xLang == "EN" ? "english" : "korean") + "1.gif";
                

                //
                if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != this.GuestUserID)
                {
                    ltSign.CssClass = "gm-log-out";
                    lnkSignIn.Visible = lnkSignUp.Visible = lnkSignInA.Visible = lnkSignUpA.Visible = false;
                    lnkSignOut.Visible = lnkMyInfo.Visible = lnkSignOutA.Visible = lnkMyInfoA.Visible = true;
                }
                // 인증되지 않은 사용자의 경우
                else
                {
                    ltSign.CssClass = "gm-log-in";
                    lnkSignIn.Visible = lnkSignUp.Visible = lnkSignInA.Visible = lnkSignUpA.Visible = true;
                    lnkSignOut.Visible = lnkMyInfo.Visible = lnkSignOutA.Visible = lnkMyInfoA.Visible = false;

                    lnkSignIn.NavigateUrl = lnkSignInA.NavigateUrl = string.Format("/mypage/login.aspx?backURL={0}", HttpContext.Current.Server.UrlEncode(Request.RawUrl));
                    //lnkSignIn.NavigateUrl = lnkSignInA.NavigateUrl = string.Format("/mypage/login.aspx?backURL={0}", HttpContext.Current.Server.UrlEncode(Request.RawUrl.Replace("&MenuCode=" + Session["MENU_CODE"], "")));
                }
                Head1.Title = GetLocalResourceObject("lnkHomeResource.Text").ToString();
                //bottom 영문처리
                if (IsSettingKorean())
                {
                    Head1.Title = "지마린 서비스 아카데미";
                    lnkLang.NavigateUrl = lnkLangA.NavigateUrl = Request.RawUrl + (Request.RawUrl.IndexOf("?") > 0 ? "&" : "?") + "Lang=EN";
                    lnkLang.CssClass = lnkLangA.CssClass = "gm-lang-ko";
                    //lnkLang.Text = lnkLangA.Text = GetCultureName();// "English";

                    //LnkLogin.Text = LnkLoginAll.Text = "로그인";
                    //LnkJoinUs.Text = LnkJoinUsAll.Text = "회원가입";
                    //LnkLogout.Text = LnkLogoutAll.Text = "로그아웃";
                    //LnkMyInfo.Text = LnkMyInfoAll.Text = "정보수정";
                }
                else
                {
                    lnkLang.NavigateUrl = lnkLangA.NavigateUrl = Request.RawUrl + (Request.RawUrl.IndexOf("?") > 0 ? "&" : "?") + "Lang=KO";
                    lnkLang.CssClass = lnkLangA.CssClass = "gm-lang-en";
                    //lnkLang.Text = lnkLangA.Text = "한국어";

                    //LnkLogin.Text = LnkLoginAll.Text = "Sign In";
                    //LnkJoinUs.Text = LnkJoinUsAll.Text = "Sign Up";
                    //LnkLogout.Text = LnkLogoutAll.Text = "Sign Out";
                    //LnkMyInfo.Text = LnkMyInfoAll.Text = "My Info";
                }

                if (!IsPostBack)
                {
                    ltTopMenu.Text = ltTopMenuA.Text = "";

                    iDt = getMenu();

                    SetTopMenu();
                    SetTopMenu("All");
                    SetSubMenu();
                    
                }
                //
                if (Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {

                }
                else
                {

                }
                //
                string xJScripts = "\r\n"
                    + " function fnShowHide(id, show) " + "\r\n"
                    + " { " + "\r\n"
                    + "     var sView = document.getElementById(id).style.visibility == 'visible' ? 'hidden' : 'visible'; " + "\r\n"
                    + "     if (show != null) sView = show; " + "\r\n"
                    + "     document.getElementById(id).style.visibility = sView; " + "\r\n"
                    + " } " + "\r\n";
                xJScripts += " \r\n" + "\r\n"
                    // 주어진 배열에서 요소 1개를 랜덤하게 골라 반환하는 함수
                    + " function randomItem(a) { " + "\r\n"
                    + "     return a[Math.floor(Math.random() * a.length)]; " + "\r\n"
                    + " } " + "\r\n"
                    + "\r\n"
                    + " $(document).ready(function() { " + "\r\n";

                xJScripts += " \r\n" + "\r\n"
                    //Calendar (fold - 컨트롤 아래로 숨는문제, bounce 떨림 사용X)
                    + "     vAnim = new Array('show', 'slideDown', 'fadeIn', 'blind', 'clip', '', 'slide'); " + "\r\n"
                    + "     anim = randomItem(vAnim); " + "\r\n"
                    + "     $('.datepick').each(function() { " + "\r\n"
                    + "         $(this).datepicker({ " + "\r\n"
                    + "                 showOn: 'both', " + "\r\n" //button
                    + "                 changeMonth: true, " + "\r\n"
                    + "                 changeYear: true, " + "\r\n"
                    + "                 showButtonPanel: true, " + "\r\n"
                    + "                 buttonImage: '/asset/images/icon_cal.gif', " + "\r\n"
                    + "                 buttonImageOnly: true, " + "\r\n"
                    + "                 showMonthAfterYear: true, " + "\r\n"
                    + "                 dateFormat: 'yy.mm.dd', " + "\r\n"
                    + "                 showAnim: anim, " + "\r\n"
                    + "         }); " + "\r\n"
                    + "     }); " + "\r\n"
                    + " }); " + "\r\n"
                    + "\r\n";

                // <body> 태그 바로 밑에
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", xJScript, true);
                // </body> 태그 바로 위에
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "", xJScripts, true);
                //xJScripts += "\r\n"
                //    + " $(window).load(function() { "
                //    + "     $('input#" + HidMenuCode.ClientID + "').val('" + (Session["MENU_CODE"] == null ? "000" : Session["MENU_CODE"]) + "').css('type','text').css('display','block'); "
                //    + " }); "
                //+ "\r\n";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", xJScripts, true);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        private string getFileName()
        {
            string xFileNm = "";
            try
            {
                xFileNm = System.IO.Path.GetFileNameWithoutExtension(Request.RawUrl).ToLower();
                if (xFileNm == "index" || xFileNm == "default")
                    xFileNm = "";
            }
            catch { }
            return xFileNm;
        }
        private string getSubName()
        {

            string xSubNm = "";
            try
            {
                string xDir = System.IO.Path.GetDirectoryName(Request.RawUrl).ToLower();
                if (xDir.Length > 0)
                {
                    try
                    {
                        string[] arrDir = xDir.Split(new char[] { '\\' });

                        xSubNm = arrDir[arrDir.Length - 1];
                    }
                    catch
                    {
                        xSubNm = "";
                    }
                }
            }
            catch { }
            return xSubNm;
        }

        /************************************************************
        * Function name : SetTopMenu
        * Purpose       : Top 메뉴를 설정하는 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void SetTopMenu(string sKind = "")
        {
            try
            {
                if (iDt.Rows.Count == 0)
                    iDt = getMenu();

                DataTable xDt = sKind == "All" ? iDt : iDt.Select("SUBSTRING(menu_group, 1, 1) IN ('1', '3', '4', '6', '7')").CopyToDataTable();

                Literal xLtl = sKind == "All" ? ltTopMenuA : ltTopMenu;
                xLtl.Text = "\r\n\r\n\r\n\r\n";
                xLtl.Text += "<ul";
                if (sKind == "All") xLtl.Text += " class='all-menu' ";
                xLtl.Text += ">";


                for (int i = 0; i < xDt.Rows.Count; i++)
                {
                    if (xDt.Rows[i]["menu_group"].ToString().Substring(1, 2) == "00")
                    {
                        DataTable xSubMenu = new DataTable();

                        // 해당 대분류 메뉴에 해당하는 중분류 메뉴만 걸러내는 처리
                        DataRow[] xDrs = xDt.Select(string.Format("menu_group LIKE '{0}%' AND menu_group NOT LIKE '{0}0%' AND menu_group LIKE '%0' ", xDt.Rows[i]["menu_group"].ToString().Substring(0, 1)), "SEQ");

                        if (xDrs.Count() < 1) continue;

                        xSubMenu = xDrs.CopyToDataTable();

                        xLtl.Text += "<li><a href='#'>" + xDt.Rows[i]["MENU_NM" + (IsSettingKorean() ? "_KOR" : "_ENG")].ToString() + "</a>";
                        //xLtl.Text += string.Format(@"<a href='javascript:;' onclick=""saveMenuCD('{1}', '{2}', '{3}', '{4}');"">{0}</a>", xDt.Rows[i]["MENU_NM" + (IsSettingKorean() ? "_KOR" : "_ENG")].ToString(), xDt.Rows[i]["menu_group"].ToString().Substring(0, 1), xDt.Rows[i]["menu_group"].ToString().Substring(1, 1), xDt.Rows[i]["menu_group"].ToString().Substring(2, 1), xDt.Rows[i]["path"].ToString());

                        if (xSubMenu.Rows.Count > 0)
                        {
                            xLtl.Text += "<ul class='depth-1'>";

                            DataRow[] xSmallMenu = null;
                            foreach (DataRow submenu in xSubMenu.Rows)  // 중메뉴
                            {
                                xLtl.Text += "<li>";
                                xLtl.Text += string.Format(@"<a href='javascript:;' onclick=""saveMenuCD('{1}', '{2}', '{3}', '{4}');"">{0}</a>", submenu["MENU_NM" + (IsSettingKorean() ? "_KOR" : "_ENG")].ToString(), submenu["menu_group"].ToString().Substring(0, 1), submenu["menu_group"].ToString().Substring(1, 1), submenu["menu_group"].ToString().Substring(2, 1), submenu["path"].ToString());

                                if (sKind == "All")
                                {
                                    xSmallMenu = xDt.Select(string.Format("menu_group LIKE '{0}%' AND menu_group NOT LIKE '{0}0%' AND menu_group NOT LIKE  '%0' AND menu3 IS NOT NULL ", submenu["menu_group"].ToString().Substring(0, 2)));

                                    if (xSmallMenu.Length > 0)
                                    {
                                        xLtl.Text += "<ul class='depth-2'>";
                                        foreach (DataRow smallmenu in xSmallMenu)  // 소메뉴
                                        {
                                            xLtl.Text += string.Format(@"<li><a href='javascript:;' onclick=""saveMenuCD('{1}', '{2}', '{3}', '{4}');"">{0}</a></li>", smallmenu["MENU_NM" + (IsSettingKorean() ? "_KOR" : "_ENG")].ToString(), smallmenu["menu_group"].ToString().Substring(0, 1), smallmenu["menu_group"].ToString().Substring(1, 1), smallmenu["menu_group"].ToString().Substring(2, 1), smallmenu["path"].ToString());

                                            /*
                                            if (IsSettingKorean())
                                                xLtl.Text += string.Format(@"<li><a href=""{0}"">{1}</a></li>", xsmall["path"].ToString(), xsmall["menu_nm_kor"].ToString());
                                            else
                                                xLtl.Text += string.Format(@"<li><a href=""{0}"">{1}</a></li>", xsmall["path"].ToString(), xsmall["menu_nm_eng"].ToString());
                                            */
                                        }
                                        xLtl.Text += "</ul>";
                                    }
                                }

                                xLtl.Text += "</li>";
                            }

                            xLtl.Text += "</ul>";
                        }

                        xLtl.Text += "</li>";
                    }
                }

                xLtl.Text += "</ul>\r\n\r\n\r\n\r\n";
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        private DataTable getMenu()
        {
            try
            {
                string[] xParams = new string[1];
                xParams[0] = Session["USER_GROUP"].ToString();

                //xParams[0] = "000001";

                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MAIN.vp_m_main_md",
                                             "GetAllMenuInfo",
                                             LMS_SYSTEM.MAIN,
                                             "CLT.WEB.UI.LMS.MAIN.MAIN", (object)xParams);


                return xDt;
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        private void SetSubMenu()
        {
            try
            {
                if (iDt.Rows.Count == 0)
                    iDt = getMenu();
                
                DataRow[] xDrs = null;
                //
                // ltSubMenuTitle
                /*
                    <li><a href="#">기관소개</a></li>
                    <li><a href="#">조직정보</a></li>
                    <li class="current">연혁</li>
                */
                xDrs = iDt.Select(string.Format("menu_group = '{0}00' OR " +
                                                "menu_group = '{1}0' OR " +
                                                "menu_group = '{2}'"
                                                , Session["MENU_CODE"].ToString().Substring(0, 1)
                                                , Session["MENU_CODE"].ToString().Substring(0, 2)
                                                , Session["MENU_CODE"].ToString()));
                if (Session["MENU_CODE"].ToString().Substring(0, 1) == "0")
                {
                    switch(Session["MENU_CODE"].ToString().Substring(1, 2))
                    {
                        case "01":
                            ltSubMenuTitle.Text += string.Format(@"<li class='current'>{0}</li>", GetLocalResourceObject("lnkSignInResource.Text"));
                            break;
                        case "02":
                            ltSubMenuTitle.Text += string.Format(@"<li class='current'>{0}</li>", GetLocalResourceObject("lnkSignUpResource.Text"));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    foreach (DataRow xDr in xDrs)
                    {
                        if (Session["MENU_CODE"].ToString() == xDr["menu_group"].ToString())
                        {
                            ltSubMenuTitle.Text += string.Format(@"<li class='current'>{0}</li>", xDr["MENU_NM" + (IsSettingKorean() ? "_KOR" : "_ENG")].ToString());
                        }
                        else
                        {
                            ltSubMenuTitle.Text += string.Format(@"<li><a href='javascript:;' onclick=""saveMenuCD('{1}', '{2}', '{3}', '{4}');"">{0}</a></li>", xDr["MENU_NM" + (IsSettingKorean() ? "_KOR" : "_ENG")].ToString(), xDr["menu_group"].ToString().Substring(0, 1), xDr["menu_group"].ToString().Substring(1, 1), xDr["menu_group"].ToString().Substring(2, 1), xDr["path"].ToString());
                        }
                    }
                }
                //
                // ltSubMenuSib
                /*
                    <li class="current-sib"><a href="#none">연혁</a></li>
                    <li><a href="#none">조직현황</a></li>
                */
                xDrs = iDt.Select(string.Format(" substring(menu_group, 2, 2) <> '00' AND (" +
                                                (
                                                  Session["MENU_CODE"].ToString().Substring(2, 1) != "0" ?
                                                  "substring(menu_group, 3, 1) <> '0' AND menu_group LIKE '{1}%'" :
                                                  "substring(menu_group, 3, 1) =  '0' AND menu_group LIKE '{0}%'"
                                                ) + ")"
                                                , Session["MENU_CODE"].ToString().Substring(0, 1)
                                                , Session["MENU_CODE"].ToString().Substring(0, 2)));

                if (Session["MENU_CODE"].ToString().Substring(0, 1) == "0")
                {
                    switch (Session["MENU_CODE"].ToString().Substring(1, 2))
                    {
                        case "01":
                            ltSubMenuSib.Text += string.Format(@"<li class='current-sib'><a href='#'>{0}</a></li>", GetLocalResourceObject("lnkSignInResource.Text"));
                            ltSubMenuSib.Text += string.Format(@"<li><a href='/mypage/join.aspx'>{0}</a></li>", GetLocalResourceObject("lnkSignUpResource.Text"));
                            break;
                        case "02":
                            ltSubMenuSib.Text += string.Format(@"<li><a href='/mypage/login.aspx?backURL={1}'>{0}</a></li>", GetLocalResourceObject("lnkSignInResource.Text"), HttpContext.Current.Server.UrlEncode(Request.RawUrl));
                            ltSubMenuSib.Text += string.Format(@"<li class='current-sib'><a href='#'>{0}</a></li>", GetLocalResourceObject("lnkSignUpResource.Text"));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    foreach (DataRow xDr in xDrs)
                    {
                        if (Session["MENU_CODE"].ToString() == xDr["menu_group"].ToString())
                        {
                            ltSubMenuSib.Text += string.Format(@"<li class='current-sib'><a href='#'>{0}</a></li>", xDr["MENU_NM" + (IsSettingKorean() ? "_KOR" : "_ENG")].ToString());
                        }
                        else
                        {
                            ltSubMenuSib.Text += string.Format(@"<li><a href='javascript:;' onclick=""saveMenuCD('{1}', '{2}', '{3}', '{4}');"">{0}</a></li>", xDr["MENU_NM" + (IsSettingKorean() ? "_KOR" : "_ENG")].ToString(), xDr["menu_group"].ToString().Substring(0, 1), xDr["menu_group"].ToString().Substring(1, 1), xDr["menu_group"].ToString().Substring(2, 1), xDr["path"].ToString());
                        }
                    }
                }
                // ltSubMenuTab
                /*
                    <li class="item current"><a href="#none">연혁</a></li>
                    <li class="item"><a href="#none">조직현황</a></li>
                */
                if (Session["MENU_CODE"].ToString().Substring(2, 1) != "0")
                {
                    foreach (DataRow xDr in xDrs)
                    {
                        if (Session["MENU_CODE"].ToString() == xDr["menu_group"].ToString())
                        {
                            ltSubMenuTab.Text += string.Format(@"<li class='item current'><a href='#'>{0}</a></li>", xDr["MENU_NM" + (IsSettingKorean() ? "_KOR" : "_ENG")].ToString());
                        }
                        else
                        {
                            ltSubMenuTab.Text += string.Format(@"<li class='item'><a href='javascript:;' onclick=""saveMenuCD('{1}', '{2}', '{3}', '{4}');"">{0}</a></li>", xDr["MENU_NM" + (IsSettingKorean() ? "_KOR" : "_ENG")].ToString(), xDr["menu_group"].ToString().Substring(0, 1), xDr["menu_group"].ToString().Substring(1, 1), xDr["menu_group"].ToString().Substring(2, 1), xDr["path"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
    }
}
