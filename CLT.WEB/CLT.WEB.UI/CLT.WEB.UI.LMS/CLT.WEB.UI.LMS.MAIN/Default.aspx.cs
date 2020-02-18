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
//
using System.Globalization;
using System.Threading;

// 필수 using 문

using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.MAIN
{
    public partial class Default : BasePage
    {
        private string iNewIcon = "<img src='/images/icon_new.gif' style='width:10px;height:9px;vertical-align:middle;'/>";

        // Inner Exception 보기 설정
        protected string DEBUG_MODE
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DebugMode"].ToString();
            }
        }

        #region 초기화 그룹
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BasePage iPage = (BasePage)this.Page;
                    
                        if (!Request.IsSecureConnection)
                        {
                            if (DEBUG_MODE == Boolean.FalseString.ToUpper())
                            {
                                Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
                            }
                        }

                    this.BindGrid();
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        #region 기타 프로시저 그룹 [Core Logic]

        private void BindGrid()
        {
            try
            {
                // OpenCourse 관련 DataBinding 처리
                string[] xParams = new string[4];
                xParams[0] = "3"; // Count
                xParams[1] = "7"; // new icon 일자
                xParams[2] = Convert.ToString(Session["company_kind"]); // 
                xParams[3] = Convert.ToString(Session["user_group"]);

                DataSet xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.MAIN.vp_m_main_md",
                                                 "GetRecentOpenCourseInfoCountBy",
                                                 LMS_SYSTEM.MAIN,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                SetGrid(xDs.Tables[1], EduNoticeList);
                SetGrid(xDs.Tables[0], NoticeList);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }


        /************************************************************
        * Function name : SetGrid
        * Purpose       : Top 메뉴를 설정하는 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void SetGrid(DataTable xDt, Literal xLtl)
        {
            try
            {
                DateTime xDte;
                string xMMM, xDD, xSubject, xLink;
                xLtl.Text = "";
                /*
                <li>
                <span class="date">
                    <span class="date-month">dec</span>
                    <span class="date-day">28</span>
                </span>
                    <a href="#none">게시글이 어쩌고 저쩌고 미리보기 80byte 까지 잘라주세요. 이 텍스트는 공백 포함 약 40자 정...</a>
                </li>
                */
                foreach(DataRow xDr in xDt.AsEnumerable())
                {
                    xLtl.Text += "<li>";
                    xLtl.Text += "    <span class='date'>";

                    xDte = DateTime.ParseExact(xDr["NOTICE_BEGIN_DT"].ToString(), "yyyy.MM.dd", CultureInfo.InvariantCulture);
                    xMMM = xDte.ToString("MMM", CultureInfo.GetCultureInfo("en-US"));
                    xDD = xDte.ToString("dd");

                    xLtl.Text += "        <span class='date-month'>" + xMMM + "</span>";
                    xLtl.Text += "        <span class='date-day'>" + xDD + "</span>";
                    xLtl.Text += "    </span>";

                    xSubject = StringHelper.CutTitle(Convert.ToString(xDr["NOT_SUB"]), 40);

                    if (xLtl.ID == "EduNoticeList")
                        xLink = "javascript:eduNoticeUrl('" + Convert.ToString(xDr["NOT_NO"] + "');");
                    else
                        xLink = "javascript:noticeUrl('" + Convert.ToString(xDr["NOT_NO"] + "');");

                    xLtl.Text += "    <a href=\"" + xLink + "\">" + xSubject + "</a>";
                    xLtl.Text += "</li>";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        #endregion

        #region 화면 컨트롤 이벤트 핸들러 그룹

        protected void GVNotice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HyperLink hlk_subject = (HyperLink)e.Row.FindControl("hlk_subject");
                    hlk_subject.Text = StringHelper.CutTitle(Convert.ToString(((DataRowView)e.Row.DataItem)["NOT_SUB"]), 25);
                    hlk_subject.NavigateUrl = "javascript:noticeUrl('" + Convert.ToString(((DataRowView)e.Row.DataItem)["NOT_NO"] + "');");
                    if (((DataRowView)e.Row.DataItem)["NEW_ICON"].ToString() == "Y")
                    {
                        Literal ltr_icon = (Literal)e.Row.FindControl("ltr_icon");
                        ltr_icon.Text = "&nbsp;" + iNewIcon;
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void GVNoticeE_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HyperLink hlk_subject = (HyperLink)e.Row.FindControl("hlk_subject");
                    hlk_subject.Text = StringHelper.CutTitle(Convert.ToString(((DataRowView)e.Row.DataItem)["NOT_SUB"]), 25);
                    hlk_subject.NavigateUrl = "javascript:noticeUrl('" + Convert.ToString(((DataRowView)e.Row.DataItem)["NOT_NO"] + "');");
                    if (((DataRowView)e.Row.DataItem)["NEW_ICON"].ToString() == "Y")
                    {
                        Literal ltr_icon = (Literal)e.Row.FindControl("ltr_icon");
                        ltr_icon.Text = "&nbsp;" + iNewIcon;
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void GVCourseOpen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HyperLink hlk_course_nm = (HyperLink)e.Row.FindControl("hlk_course_nm");
                    hlk_course_nm.Text = StringHelper.CutTitle(Convert.ToString(((DataRowView)e.Row.DataItem)["course_nm"]), 20);

                    if (Convert.ToString(Session["USER_ID"]) == "" || Convert.ToString(Session["USER_GROUP"]) == this.GuestUserID)
                        hlk_course_nm.NavigateUrl = "javascript:alert('" + CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A123", new string[] { "" }, new string[] { "" }, System.Threading.Thread.CurrentThread.CurrentCulture) + "');";
                    else
                        hlk_course_nm.NavigateUrl = "javascript:courseOpenUrl('" + Convert.ToString(((DataRowView)e.Row.DataItem)["open_course_id"] + "');");

                    if (((DataRowView)e.Row.DataItem)["NEW_ICON"].ToString() == "Y")
                    {
                        Literal ltr_icon = (Literal)e.Row.FindControl("ltr_icon");
                        ltr_icon.Text = "&nbsp;" + iNewIcon;
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        #endregion
    }
}
