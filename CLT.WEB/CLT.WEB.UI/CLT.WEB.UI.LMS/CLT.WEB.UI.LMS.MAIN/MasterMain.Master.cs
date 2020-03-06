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
    /// 1. 작업개요 : MAIN Master Class
    /// 
    /// 2. 주요기능 : 웹사이트 레이아웃
    ///				  
    /// 3. Class 명 : MasterMain
    /// 
    /// 4. 작 업 자 : 
    /// </summary>
    public partial class MasterMain : BaseMasterPage
    {
        DataTable iDt = new DataTable();
        String iFileNm, iSubNm = "";

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
                Response.Redirect("/");
            }
            else if (Session["Language"] != null && Session["Language"].ToString().Trim() != GetCultureName())
            {
                SetDefaultCulture(new CultureInfo(Session["Language"].ToString() == "EN" ? "en-US" : "ko-KR"));
                //Response.Redirect(Request.RawUrl);
            }
            
            try
            {
                iFileNm = getFileName();
                iSubNm = getSubName();
                //
                HtmlHead head = (System.Web.UI.HtmlControls.HtmlHead)this.Page.Header;
                head.Controls.Add(new LiteralControl("\r\n"));
                //
                HtmlLink[] xHtmlLnk = new HtmlLink[6];
                string[] xCssFiles = { "css/common/reset", "css/common/layout", 
                    (iFileNm == "" && iSubNm == "" ? "css/main/main" : "css/sub/" + iSubNm)
                    , "css/common/responsive", "css/common/jquery.mCustomScrollbar"}; //, "fontawesome/css/all"
                for (int i = 0; i < 5; i++)
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
                HtmlGenericControl[] xHtmlGC = new HtmlGenericControl[5];
                string[] xJssFiles = { "jquery.min", "common/util", "common/layout", "common/jquery.mCustomScrollbar.concat.min",
                    (iFileNm == "" && iSubNm == "" ? "main/main" : "sub/sub")
                    };
                for (int i = 0; i < 5; i++)
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
                //Session["Language"] = xLang;
                //ibtnLang.ImageUrl = "/asset/images/btn_" + (xLang == "EN" ? "english" : "korean") + "1.gif";
                
                // By KMK SESSION 에 있는 메뉴코드 가져오는 부분
                string xMenuCode = string.Empty;
                if (Session["MENU_CODE1"] != null)
                    xMenuCode += Session["MENU_CODE1"];
                if (Session["MENU_CODE2"] != null)
                    xMenuCode += Session["MENU_CODE2"];
                if (Session["MENU_CODE3"] != null)
                    xMenuCode += Session["MENU_CODE3"];
                Session["MENU_CODE"] = xMenuCode;
                
                //
                if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != GuestUserID)
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
                }
                Head1.Title = GetLocalResourceObject("lnkHomeResource.Text").ToString();
                //bottom 영문처리
                if (IsSettingKorean())
                {
                    Head1.Title = "지마린 서비스 아카데미";
                    lnkLang.NavigateUrl = lnkLangA.NavigateUrl = "?Lang=EN";
                    lnkLang.CssClass = lnkLangA.CssClass = "gm-lang-ko";
                    //lnkLang.Text = lnkLangA.Text = GetCultureName();// "English";

                    //LnkLogin.Text = LnkLoginAll.Text = "로그인";
                    //LnkJoinUs.Text = LnkJoinUsAll.Text = "회원가입";
                    //LnkLogout.Text = LnkLogoutAll.Text = "로그아웃";
                    //LnkMyInfo.Text = LnkMyInfoAll.Text = "정보수정";
                }
                else
                {
                    lnkLang.NavigateUrl = lnkLangA.NavigateUrl = "?Lang=KO";
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

                }
                //
                if (Session["USER_GROUP"].ToString() == this.GuestUserID || string.IsNullOrEmpty(Session["USER_GROUP"].ToString()))
                {
                    Session["USER_GROUP"] = this.GuestUserID;
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
        
    }
}
