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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Linq;

namespace CLT.WEB.UI.LMS.MYPAGE
{
    public partial class join_company_check : BasePage
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

        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCompanyName.Text))
                {
                    //Response.Write("<script language='javascript'>alert('회사명을 입력하여 주세요!');</script>");
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "회사명" }, new string[] { "Company Name" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                if (string.IsNullOrEmpty(txtRegNo1.Text))
                {
                    //Response.Write("<script language='javascript'>alert('사업자등록번호를 입력하여 주세요!');</script>");
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "사업자 등록번호" }, new string[] { "Business Registration Number" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                if (string.IsNullOrEmpty(txtRegNo2.Text))
                {
                    //Response.Write("<script language='javascript'>alert('사업자등록번호를 입력하여 주세요!');</script>");
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "사업자 등록번호" }, new string[] { "Business Registration Number" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                if (string.IsNullOrEmpty(txtRegNo3.Text))
                {
                    //Response.Write("<script language='javascript'>alert('사업자등록번호를 입력하여 주세요!');</script>");
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "사업자 등록번호" }, new string[] { "Business Registration Number" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                string xRegNo = this.txtRegNo1.Text + this.txtRegNo2.Text + this.txtRegNo3.Text;
                if (xRegNo.Length < 10)
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A033", new string[] { "사업자 등록번호", "10자리" }, new string[] { "Business Registration Number", "10 Length" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                string[] xParams = new string[2];
                
                xParams[0] = this.txtCompanyName.Text.Replace("'", "''");
                xParams[1] = this.txtRegNo1.Text.Trim().Replace("'", "''") + this.txtRegNo2.Text.Trim().Replace("'", "''") + this.txtRegNo3.Text.Trim().Replace("'", "''");
                
                // 등록된 Company가 있는지 체크
                DataTable xDtCompany = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                                 "GetCompany",
                                                 LMS_SYSTEM.MANAGE,
                                                 "CLT.WEB.UI.LMS.MANAGE",
                                                 (object)xParams);
                
                if (xDtCompany.Rows.Count > 0)
                {
                    //사업자번호가 존재합니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A026", new string[] { "사업자번호" }, new string[] { "Register No" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                Response.Redirect(string.Format("/mypage/join_company_agree.aspx?CompanyName={0}&RegNo={1}", xParams[0], xParams[1]));

                //string xScriptContent = string.Format("<script>document.location.href='/mypage/join_company_reg.aspx?CompanyName={0}&RegNo={1}';</script>", xParams[0], xParams[1]);
                //ScriptHelper.ScriptBlock(this, "join_company_check", xScriptContent);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }

        }

        public bool NullCheck(string rString)
        {
            bool xChk = false;
            try
            {
                if (string.IsNullOrEmpty(rString))
                    xChk = false;
                else
                    xChk = true;

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xChk;
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