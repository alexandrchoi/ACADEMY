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
using System.Windows.Forms;

// 필수 using 문
using System.Globalization;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using IAMVIRTUALAGENTCOMLib;
using System.Linq;

namespace CLT.WEB.UI.LMS.MYPAGE
{
    public partial class login : BasePage
    {
        private string _ibackUrl;
        public string iBackURL
        {
            get { return _ibackUrl; }
            set { _ibackUrl = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                iBackURL = "/";

                if (Request.QueryString["mode"] != null && Request.QueryString["mode"].ToString() == "logout")
                {
                    this.SignOut();
                    Session.Abandon();
                    Session.Clear();
                    Session["USER_GROUP"] = this.GuestUserID;
                    Response.Redirect("/");
                }
                if (Request.QueryString["backURL"] != null && Request.QueryString["backURL"].ToString() != string.Empty)
                {
                    iBackURL = Request.QueryString["backURL"];
                    if (iBackURL != "" && iBackURL.IndexOf("MenuCode") < 0)
                    {
                        iBackURL += (iBackURL.IndexOf("?") > 0 ? "&" : "?") + "MenuCode=" + Session["MENU_CODE"];
                    }
                }

                Session["MENU_CODE1"] = "0"; // 대분류 메뉴
                Session["MENU_CODE2"] = "0"; // 중분류 메뉴
                Session["MENU_CODE3"] = "1"; // 소분류 메뉴
                Session["MENU_CODE"] = Session["MENU_CODE1"] + "" + Session["MENU_CODE2"] + "" + Session["MENU_CODE3"];

                if (Request.Cookies["UserId"] != null)              // 쿠키가 있는지 확인해서 있으면 아이디 입력필드에 지정
                {                                                   // 체크박스는 체크상태로
                    if (!IsPostBack)
                    {
                        if (Request.Cookies["UserId"] != null)      // 쿠키가 있는지 확인해서 있으면 아이디 입력필드에 지정
                        {                                           // 체크박스는 체크상태로
                            txtUserID.Text = Request.Cookies["UserId"].Value;

                            idSaveCheck.Checked = true;

                            txtPassword.Focus();
                        }
                    }
                    txtUserID.Text = Request.Cookies["UserId"].Value;
                    idSaveCheck.Checked = true;
                    txtPassword.Focus();
                }

                backURL.Value = iBackURL;
            }
            else
            {
                iBackURL = backURL.Value;
            }

        }
        /************************************************************
        * Function name : btn_login_Click
        * Purpose       : 로그인 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btn_login_Click(object sender, EventArgs e)
        {
            if (idSaveCheck.Checked)
            {
                HttpCookie cookie = new HttpCookie("UserId", txtUserID.Text);

                cookie.Expires = DateTime.Now.AddDays(7);       // 7일간 쿠키저장

                Response.Cookies.Add(cookie);
            }
            else
            {
                HttpCookie cookie = new HttpCookie("UserId", txtUserID.Text);

                cookie.Expires = DateTime.Now.AddDays(-1);      // 쿠키 삭제

                Response.Cookies.Add(cookie);
            }
            
            try
            {
                BasePage iPage = (BasePage)this.Page;

                // ID 와 패드워드가 입력 되었는지 체크한다.
                if (!String.IsNullOrEmpty(this.txtUserID.Text) && !String.IsNullOrEmpty(this.txtPassword.Text))
                {
                    string[] xParams = new string[1];
                    xParams[0] = this.txtUserID.Text;

                    DataTable xDt = null;
                    // 먼저 입력된 ID를 조회해서 UserGroup을 체크한다.
                    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                      "GetUserGroup",
                                                     LMS_SYSTEM.MYPAGE,
                                                     "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);
                    string xUSE_YN = string.Empty;
                    string xStatus = string.Empty;
                    string xUser_group = string.Empty;
                    string xRtn = Boolean.TrueString;
                    string xPWD = string.Empty;

                    if (xDt.Rows.Count > 0)
                    {
                        if (xDt.Rows.Count == 1 && !string.IsNullOrEmpty(xDt.Rows[0]["use_yn"].ToString()))
                        {
                            xUSE_YN = xDt.Rows[0]["use_yn"].ToString();  // 육상사용자 사용여부(SSO 대상)
                        }

                        xStatus = xDt.Rows[0]["status"].ToString();  // 상태 
                        xUser_group = xDt.Rows[0]["user_group"].ToString();  // 사용자 그룹
                        xPWD = xDt.Rows[0]["pwd"].ToString();  // 암호
                    }

                    if (xUser_group == "000006") // 해상직원(그룹사 수강자)이면...
                    {
                        // 해상직원용 암호화 모듈로 비밀번호를 체크한다.
                        if (iPage.SignInSea(this.txtUserID.Text, this.txtPassword.Text))
                        {
                            if (Session["USER_STATUS"] != null && Session["USER_STATUS"].ToString() == "000003")
                            {
                                Response.Redirect(iBackURL);
                            }
                            else
                            {
                                this.SignOut();
                                ScriptHelper.Page_Alert(iPage, "입력하신 계정이 아직 승인되지 않았습니다.");
                            }
                        }
                        else
                        {
                            ScriptHelper.Page_Alert(iPage, "사용자의 인증 정보를 다시 입력해 주십시오.");
                        }
                    }
                    else  // 관리자, 법인사, 일반사용자등
                    {
                        bool xLogin = false;

                        // 육상직원이면
                        if (xUSE_YN == "Y")
                        {
                            string EnPWD = OpusCryptoLibrary.Cryptography.Encrypt(this.txtPassword.Text, this.txtPassword.Text);

                            // IP 대역폭을 체크한다.
                            //string strClientIP = Request.UserHostAddress;

                            ////string InitParam = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; 
                            //if (iPage.IsInternalIP(strClientIP))
                            //{

                                if (EnPWD != xPWD)  // SSO PWD Encoding = 입력 PWD 동일할 경우 SSO 페이지로 이동
                                {
                                    //xLogin = false;

                                    ScriptHelper.Page_Alert(iPage, "사용자의 인증 정보를 다시 입력해 주십시오");

                                    //Response.Redirect("/");
                                }
                                else
                                {
                                    xLogin = true;
                                }
                            //}
                            //else
                            //{
                            //    xLogin = false;
                            //}
                        }

                        // 인증이 성공되었지만, status값이 "000003"인 경우만 로그인 이전페이지로 이동한다.
                        // 사용자 계정 승인상태 (000001:미승인, 000002:사용안함, 000003:승인)
                        if (xLogin == true)
                        {
                            if (iPage.SignIn(this.txtUserID.Text, this.txtPassword.Text))
                            {
                                if (Session["USER_STATUS"] != null && Session["USER_STATUS"].ToString() == "000003")
                                {
                                    Response.Redirect(iBackURL);
                                }
                                else
                                {
                                    this.SignOut();
                                    ScriptHelper.Page_Alert(iPage, "입력하신 계정이 아직 승인되지 않았습니다.");
                                }
                            }
                            else
                            {
                                ScriptHelper.Page_Alert(iPage, "사용자의 인증 정보를 다시 입력해 주십시오.");
                            }
                        }
                        else
                        {
                            if (iPage.SignIn(this.txtUserID.Text, this.txtPassword.Text))
                            {
                                if (Session["USER_STATUS"] != null && Session["USER_STATUS"].ToString() == "000003")
                                {
                                    Response.Redirect(iBackURL);
                                }
                                else
                                {
                                    this.SignOut();
                                    ScriptHelper.Page_Alert(iPage, "입력하신 계정이 아직 승인되지 않았습니다.");
                                }
                            }
                            else
                            {
                                ScriptHelper.Page_Alert(iPage, "사용자의 인증 정보를 다시 입력해 주십시오.");
                            }
                        }

                    }

                }
                else if (String.IsNullOrEmpty(this.txtUserID.Text) && !String.IsNullOrEmpty(this.txtPassword.Text))
                {
                    ScriptHelper.Page_Alert(iPage, "ID를 입력해 주십시오.");
                }
                else if (!String.IsNullOrEmpty(this.txtUserID.Text) && String.IsNullOrEmpty(this.txtPassword.Text))
                {
                    ScriptHelper.Page_Alert(iPage, "Password를 입력해 주십시오.");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /************************************************************
        * Function name : ibtnLogout_Click
        * Purpose       : 로그아웃 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void ibtnLogout_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.SignOut();
                Session.Abandon();

                //Session["CERTIFY"] = "LOGOUT2";

                Response.Redirect("/");

                //Response.Redirect("/login.aspx");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
    }
}