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

using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using System.Text;
using System.IO;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : 수강신청 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 수강신청 화면
    ///				  
    /// 3. Class 명 : courseapplication_detail
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02.
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    /// 
    /// </summary>
    public partial class courseapplication_detail : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 수강신청 페이지 Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "courseapplication_detail", xScriptMsg);

                    return;
                }

                this.Page.Form.DefaultButton = this.btnList.UniqueID; // Page Default Button Mapping

                if ((Request.QueryString["ropen_course_id"] != null && Request.QueryString["ropen_course_id"].ToString() != ""))
                {
                    if (!IsPostBack)
                    {

                        base.pRender(this.Page,
                                     new object[,] { { this.btnApplication, "E" },
                                           },
                                     Convert.ToString(Request.QueryString["MenuCode"]));


                        if (Request.QueryString["rapproval_code"] != null)
                            ViewState["rapproval_code"] = Request.QueryString["rapproval_code"].ToString();
                        else
                            ViewState["rapproval_code"] = string.Empty;

                        ViewState["course_yn"] = Boolean.FalseString;  // 필수직급 여부
                        BindData(Request.QueryString["ropen_course_id"]);
                    }
                }
                else
                {
                    return;
                    //string xScriptContent = "<script>alert('잘못된 경로를 통해 접근하였습니다.');self.close();</script>";
                    //ScriptHelper.ScriptBlock(this, "courseapplication_detail", xScriptContent);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : btnApplication_OnClick
        * Purpose       : 수강신청 Click 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnApplication_OnClick(object sender, EventArgs e)
        protected void btnApplication_OnClick(object sender, EventArgs e)
        {
            try
            {
                string xApplovalChk = Boolean.TrueString;

                // 직책 or 직급이 수강신청 대상에 해당하는 경우를 찾는다.
                string[] xChkParams = new string[2];
                xChkParams[0] = Request.QueryString["ropen_course_id"].ToString();
                xChkParams[1] = Session["USER_ID"].ToString();

                xApplovalChk = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPLICATION.vp_g_courseapplication_md",
                               "GetCourseStatus",
                               LMS_SYSTEM.APPLICATION,
                               "CLT.WEB.UI.LMS.APPLICATION", (object)xChkParams);

                if (xApplovalChk == Boolean.TrueString)  // 이미 수강신청이 되어있는 상태 체크
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A014",
                                  new string[] { "수강신청" },
                                  new string[] { "Course Registration" },
                                  Thread.CurrentThread.CurrentCulture
                                 ));
                    return;
                }

                //// 필수직급을 비교하여 신청대상인지 확인...
                //if (ViewState["course_yn"].ToString() == Boolean.FalseString)
                //{
                //    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A083",
                //                  new string[] { "수강신청" },
                //                  new string[] { "Course Registration" },
                //                  Thread.CurrentThread.CurrentCulture
                //                 ));
                //    return;
                //}




                DataTable xChkDt = null;
                xChkDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_g_courseapplication_md",
                                       "GetCoursePermissions",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION", (object)xChkParams);

                if (xChkDt == null || xChkDt.Rows.Count == 0)
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A137",
                                  new string[] { "수강신청" },
                                  new string[] { "Registration" },
                                  Thread.CurrentThread.CurrentCulture
                                 ));
                    return;
                }

                string xDuty_Work = Session["DUTY_WORK"].ToString();
                string xDuty_Step = Session["DUTY_STEP"].ToString();
                bool Chk = false;

                string[] xWork = xChkDt.Rows[0]["opt_duty_work"].ToString().Split(',');
                string[] xStep = xChkDt.Rows[0]["ess_duty_step"].ToString().Split(',');

                foreach (string xWorks in xWork)
                {
                    if (xWorks == xDuty_Work)
                    {
                        if (!string.IsNullOrEmpty(xWorks))
                            Chk = true;
                    }
                }

                foreach (string xSteps in xStep)
                {
                    if (xSteps == xDuty_Step)
                    {
                        if (!string.IsNullOrEmpty(xSteps))
                            Chk = true;
                    }
                }



                if (Chk == false)
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A137",
                                  new string[] { "수강신청" },
                                  new string[] { "Registration" },
                                  Thread.CurrentThread.CurrentCulture
                                 ));
                    return;
                }

                string[] xParams = new string[14];
                xParams[0] = Session["USER_ID"].ToString();  // 사용자 ID

                if (ViewState["open_course_id"] != null)
                    xParams[1] = ViewState["open_course_id"].ToString(); // 개설과정 ID

                xParams[2] = Session["COMPANY_ID"].ToString(); // 사용자 회사
                xParams[3] = Session["DEPT_CODE"].ToString(); // 사용자 부서

                xParams[4] = Session["DUTY_STEP"].ToString(); // 사용자 직급
                xParams[5] = string.Empty; //Session["USER_GU"].ToString(); // 재직, 채용예정자 구분
                xParams[6] = "000003"; // 승인여부  m_cd = '0019',  000001 : 승인, 000002 : 미승인, 000003 : 승인대기, 000004 : 과정취소, 000005 : 본인취소
                xParams[7] = "000004"; // 이수여부  m_cd = '0010',  000001 : 이수, 000002 : 재시험, 000003 : 재재시험, 000004 : 수강, 000005 : 미이수, 000006 : 미처리

                xParams[8] = "0";  // 진도율


                if (ViewState["open_course_id"] != null)
                    xParams[9] = ViewState["course_begin_applty_dt"].ToString(); // 학습 시작일자

                if (ViewState["course_end_applty_dt"] != null)
                    xParams[10] = ViewState["course_end_applty_dt"].ToString(); // 학습 종료일자

                if (ViewState["insurance_flg"] != null)
                {
                    xParams[11] = ViewState["insurance_flg"].ToString();  // 고용보험 여부

                    if (ViewState["insurance_flg"].ToString() == "Y")
                        xParams[12] = Convert.ToDateTime(Session["ENTER_DT"]).ToString("yyyy.MM.dd");  // 정규직을경우 입사일자는 피보험 취득일자가 된다.
                    else
                        xParams[12] = string.Empty;
                }
                xParams[13] = Session["USER_ID"].ToString();

                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;


                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPLICATION.vp_g_courseapplication_md",
                                         "SetCourseApplication",
                                         LMS_SYSTEM.APPLICATION,
                                         "CLT.WEB.UI.LMS.APPLICATION",
                                         (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A022",
                                  new string[] { "수강신청" },
                                  new string[] { "Course Registration" },
                                  Thread.CurrentThread.CurrentCulture
                                 ));
                    ViewState["rapproval_code"] = "000003";
                    //xScriptMsg = "<script>window.location.href='/application/courseapplication_list.aspx';</script>";
                    //ScriptHelper.ScriptBlock(this, "courseapplication_detail", xScriptMsg);
                }
                else
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101",
                                  new string[] { "수강신청" },
                                  new string[] { "Course Registration" },
                                  Thread.CurrentThread.CurrentCulture
                                 ));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : btnList_OnClick
        * Purpose       : 수강신청 List Click 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnList_OnClick(object sender, EventArgs e)
        protected void btnList_OnClick(object sender, EventArgs e)
        {
            try
            {
                string xScriptMsg = "<script>window.location.href='/application/courseapplication_list.aspx';</script>";
                ScriptHelper.ScriptBlock(this, "courseapplication_detail", xScriptMsg);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : BindData
        * Purpose       : 수강신청정보 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region public void BindData(string rOpen_Course_id)
        public void BindData(string rOpen_Course_id)
        {
            try
            {
                string[] xParams = new string[1];
                xParams[0] = rOpen_Course_id;

                DataTable xDt = null;
                DataTable xDtDutyWork = null;
                DataTable xDtDutyStep = null;

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_g_courseapplication_md",
                                       "GetCourseApplicationResult",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);

                xDtDutyStep = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_g_courseapplication_md",
                                                "GetDutyStep",
                                                LMS_SYSTEM.APPLICATION,
                                                "CLT.WEB.UI.LMS.APPLICATION", (object)xParams);

                xDtDutyWork = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_g_courseapplication_md",
                                                "GetDutyWork",
                                                LMS_SYSTEM.APPLICATION,
                                                "CLT.WEB.UI.LMS.APPLICATION", (object)xParams);

                this.lblCourseDate.Text = xDt.Rows[0]["course_Date"].ToString();
                this.lblApplyDate.Text = xDt.Rows[0]["apply_date"].ToString();
                this.lblSeq.Text = xDt.Rows[0]["course_seq"].ToString();
                this.lblcourse_nm.Text = xDt.Rows[0]["course_nm"].ToString();
                this.lblCourseDate_1.Text = xDt.Rows[0]["course_Date"].ToString();
                this.txtContent.Text = xDt.Rows[0]["course_intro"].ToString(); // 과정소개
                this.txtObjective.Text = xDt.Rows[0]["course_objective"].ToString(); // 학습목표


                string xDutyWork = string.Empty;
                string[] xDutyWorkCode = xDt.Rows[0]["opt_duty_work"].ToString().Split(',');

                string xSSDutyWork = (string)Session["DUTY_WORK"];

                for (int i = 0; i < xDutyWorkCode.Length; i++)
                {
                    foreach (DataRow rows in xDtDutyWork.Rows)
                    {
                        if (xDutyWorkCode[i] == rows["duty_work"].ToString())
                        {
                            if (IsSettingKorean())
                            {
                                if (xDutyWork.Length == 0)
                                    xDutyWork += rows["duty_work_name"].ToString();
                                else
                                    xDutyWork += ", " + rows["duty_work_name"].ToString();
                            }
                            else
                            {
                                if (xDutyWork.Length == 0)
                                    xDutyWork += rows["duty_work_ename"].ToString();
                                else
                                    xDutyWork += ", " + rows["duty_work_ename"].ToString();
                            }

                            if (xSSDutyWork == xDutyWork[i].ToString())
                                ViewState["course_yn"] = Boolean.TrueString;

                            continue;
                        }
                    }
                }

                this.txtTarget.Text = xDutyWork;
                if (IsSettingKorean())
                {
                    if (!string.IsNullOrEmpty(xDt.Rows[0]["std_progress_rate"].ToString()))
                        this.txtCompletedBy.Text += "진도율 : " + xDt.Rows[0]["std_progress_rate"].ToString() + "% ";

                    if (!string.IsNullOrEmpty(xDt.Rows[0]["std_final_exam"].ToString()))
                        this.txtCompletedBy.Text += "기말고사 : " + xDt.Rows[0]["std_final_exam"].ToString() + "% ";

                    if (!string.IsNullOrEmpty(xDt.Rows[0]["std_report"].ToString()))
                        this.txtCompletedBy.Text += "레포트 : " + xDt.Rows[0]["std_report"].ToString() + "% ";
                }
                else
                {
                    if (!string.IsNullOrEmpty(xDt.Rows[0]["std_progress_rate"].ToString()))
                        this.txtCompletedBy.Text += "Progress Rate : " + xDt.Rows[0]["std_progress_rate"].ToString() + "% ";

                    if (!string.IsNullOrEmpty(xDt.Rows[0]["std_final_exam"].ToString()))
                        this.txtCompletedBy.Text += "Final Example : " + xDt.Rows[0]["std_final_exam"].ToString() + "% ";

                    if (!string.IsNullOrEmpty(xDt.Rows[0]["std_report"].ToString()))
                        this.txtCompletedBy.Text += "Report : " + xDt.Rows[0]["std_report"].ToString() + "% ";

                }

                ViewState["open_course_id"] = xDt.Rows[0]["open_course_id"].ToString(); // 개설과정 ID
                string[] xCourseDate = xDt.Rows[0]["course_Date"].ToString().Split('~');

                for (int i = 0; i < xCourseDate.Length; i++)
                {
                    // 학습시작일자 , 종료일자가 입력되어야 하나 
                    // 종료일자에 시작일자가 입력됨...
                    if (i == 0)
                        ViewState["course_begin_applty_dt"] = xCourseDate[i].ToString().Trim();  // 학습 시작일자
                    else if (i == 1)
                        ViewState["course_end_applty_dt"] = xCourseDate[i].ToString().Trim();  // 학습 종료일자
                }

                ViewState["insurance_flg"] = xDt.Rows[0]["insurance_flg"].ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

    }
}
