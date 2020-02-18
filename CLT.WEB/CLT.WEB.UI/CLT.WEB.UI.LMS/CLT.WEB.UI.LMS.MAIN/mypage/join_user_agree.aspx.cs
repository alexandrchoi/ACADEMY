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
using System.Linq;

namespace CLT.WEB.UI.LMS.MYPAGE
{
    public partial class join_user_agree : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    Response.Redirect("/");
                }
            }
            catch { Response.Redirect("/"); }

            if (!IsPostBack)
            {
                if (!UrlReferrerCheck("/mypage/join.aspx"))
                {
                    string xScriptMsg = string.Format("<script>alert('잘못된 접근입니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "join", xScriptMsg);
                    return;
                }

                Session["MENU_CODE1"] = "0"; // 대분류 메뉴
                Session["MENU_CODE2"] = "0"; // 중분류 메뉴
                Session["MENU_CODE3"] = "2"; // 소분류 메뉴
                Session["MENU_CODE"] = Session["MENU_CODE1"] + "" + Session["MENU_CODE2"] + "" + Session["MENU_CODE3"];
            }

        }


        public bool UrlReferrerCheck(string pPageName)
        {
            bool bReturn = true;

            try
            {
                if (Request.UrlReferrer == null)
                {
                    bReturn = false;
                }
                else
                {
                    string refer = Request.UrlReferrer.ToString().ToLower();
                    string ServerPath = Request.Url.ToString();
                    ServerPath = ServerPath.Substring(0, ServerPath.LastIndexOf("/"));

                    if (refer.IndexOf(ServerPath) == -1)
                    {
                        bReturn = false;
                    }

                    if (!string.IsNullOrEmpty(pPageName))
                    {
                        if (refer.IndexOf(pPageName) == -1)
                        {
                            bReturn = false;
                        }
                    }
                }
            }
            catch
            {
                bReturn = false;
            }

            return bReturn;
        }
    }
}