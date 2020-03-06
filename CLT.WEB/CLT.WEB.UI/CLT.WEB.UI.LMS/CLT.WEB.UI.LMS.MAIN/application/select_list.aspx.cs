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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading;

namespace CLT.WEB.UI.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : 교육대상자선발 조회 Class
    /// 
    /// 2. 주요기능 : LMS 교육대상자선발 조회 화면
    ///				  
    /// 3. Class 명 : select_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class select_list : BasePage
    {
        //txt_start_date=&txt_end_date=&ddl_course_type=&txt_course_nm= 
        private string iTxtStartDate { get { return Util.Request("txt_start_date"); } }
        private string iTxtEndDate { get { return Util.Request("txt_end_date"); } }
        private string iDdlCourseType { get { return Util.Request("ddl_course_type"); } }
        private string iTxtCourseNM { get { return Util.Request("txt_course_nm"); } }
        private DataTable iDutyStep
        {
            get { return (DataTable)ViewState["DutyStep"]; }
            set { ViewState["DutyStep"] = value; }
        }
        private DataTable iDutyWork
        {
            get { return (DataTable)ViewState["DutyWork"]; }
            set { ViewState["DutyWork"] = value; }
        }

        #region 인터페이스 그룹

        #endregion

        #region 기타 프로시저 그룹 [Core Logic]
        private DataSet GetDsGrdList(string rGubun)
        {
            DataSet xDs = null;
            try
            {
                string[] xParams = new string[7];
                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = Util.ForbidText(txtSTART_DATE.Text);
                xParams[3] = Util.ForbidText(txtEND_DATE.Text);
                xParams[4] = ddlCourseType.SelectedValue;
                xParams[5] = ddlCourseLang.SelectedValue;
                xParams[6] = Util.ForbidText(txtCourseNM.Text);

                xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                       "GetEduSelectList",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION",
                                       (object)xParams,
                                       rGubun, Thread.CurrentThread.CurrentCulture);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDs;
        }
        private void BindGrdList(string rGubun)
        {
            try
            {
                DataSet xDs = GetDsGrdList(rGubun);
                DataTable xDtTotalCnt = xDs.Tables[0];
                DataTable xDt = xDs.Tables[1];

                if (Convert.ToInt32(xDtTotalCnt.Rows[0][0]) < 1)
                {
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageInfo1.TotalRecordCount = 0;
                    this.PageNavigator1.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageInfo1.TotalRecordCount = Convert.ToInt32(xDtTotalCnt.Rows[0][0]);
                    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDtTotalCnt.Rows[0][0]) - 1;
                }
                grdList.DataSource = xDt;
                grdList.DataBind();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        #region 초기화 그룹
        private void InitControl()
        {
            try
            {
                this.txtSTART_DATE.Attributes.Add("onkeyup", "ChkDate(this);");
                this.txtEND_DATE.Attributes.Add("onkeyup", "ChkDate(this);");

                this.txtSTART_DATE.Text = DateFormat(GetFirstDayofMonth());
                this.txtEND_DATE.Text = DateFormat(GetLastDayofMonth());

                string[] xParams = new string[1];
                string xSql = string.Empty;
                DataTable xDt = null;

                //course type 
                xParams = new string[2];
                xParams[0] = "0006";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlCourseType, xDt);

                xParams = new string[2];
                xParams[0] = "0017";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlCourseLang, xDt);

                //txt_start_date=&txt_end_date=&ddl_course_type=&txt_course_nm=
                if (!Util.IsNullOrEmptyObject(iTxtStartDate))
                    txtSTART_DATE.Text = iTxtStartDate;
                if (!Util.IsNullOrEmptyObject(iTxtEndDate))
                    txtEND_DATE.Text = iTxtEndDate;
                if (!Util.IsNullOrEmptyObject(iDdlCourseType))
                    ddlCourseType.SelectedValue = iDdlCourseType;
                if (!Util.IsNullOrEmptyObject(iTxtCourseNM))
                    txtCourseNM.Text = iTxtCourseNM;

                if (!String.IsNullOrEmpty(Convert.ToString(Session["USER_GROUP"])) && Convert.ToString(Session["USER_GROUP"]) != "000009")
                    BindGrdList("");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID;

                base.pRender(this.Page, new object[,] {
                                                        { this.btnRetrieve, "I" }
                                                      });

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (!IsPostBack)
                    {
                        InitControl();
                    }
                }
                else
                {
                    //return;
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "select_list", xScriptMsg);
                }

            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        #region 화면 컨트롤 이벤트 핸들러 그룹
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrdList("");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void grdList_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                DataRowView DRV = (DataRowView)e.Item.DataItem;
                HyperLink hlkUserId = ((HyperLink)e.Item.FindControl("hlkCourseNM"));
                hlkUserId.NavigateUrl = "javascript:;";
                //open_course_id
                hlkUserId.Attributes.Add("onclick", "javascript:GoAppForm('" + DRV["course_id"].ToString() + "^" + DRV["open_course_id"].ToString() + "'); return false;");

                HyperLink hlkMan = ((HyperLink)e.Item.FindControl("hlkMAN"));
                hlkMan.NavigateUrl = "javascript:;";
                //open_course_id
                hlkMan.Attributes.Add("onclick", "javascript:GoConfirmForm('" + DRV["course_id"].ToString() + "^" + DRV["open_course_id"].ToString() + "'); return false;");

                if (!Util.IsNullOrEmptyObject(Convert.ToString(DRV["ESS_DUTY_STEP"])) || !Util.IsNullOrEmptyObject(Convert.ToString(DRV["OPT_DUTY_WORK"])))
                {
                    if (iDutyStep == null)
                    {
                        iDutyStep = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                                "GetDutyStep",
                                                                LMS_SYSTEM.APPLICATION,
                                                                "CLT.WEB.UI.LMS.APPLICATION", null);
                    }
                    if (iDutyWork == null)
                    {
                        iDutyWork = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                                "GetDutyWork",
                                                                LMS_SYSTEM.APPLICATION,
                                                                "CLT.WEB.UI.LMS.APPLICATION", null);
                    }

                    string[] dutySteps = Convert.ToString(DRV["ESS_DUTY_STEP"]).Split(',');
                    Label lblEssStepName = ((Label)e.Item.FindControl("lblEssStepName"));
                    string dutyStep = "";
                    for (int i = 0; i < dutySteps.Length; i++)
                    {
                        DataRow[] dr = iDutyStep.Select("duty_step='" + dutySteps[i] + "'");

                        if (!string.IsNullOrEmpty(dutyStep) && dr.Length > 0)
                            dutyStep += ",";

                        if (dr.Length > 0)
                        {
                            //국영문 구분에 따른 처리
                            if (Thread.CurrentThread.CurrentCulture.Name.ToLower() == "ko-kr")
                                dutyStep += Convert.ToString(iDutyStep.Select("duty_step='" + dutySteps[i] + "'")[0]["step_name"]);
                            else
                                dutyStep += Convert.ToString(iDutyStep.Select("duty_step='" + dutySteps[i] + "'")[0]["STEP_ENAME"]);
                        }
                    }
                    lblEssStepName.Text = dutyStep;

                    string[] dutyWorks = Convert.ToString(DRV["OPT_DUTY_WORK"]).Split(',');
                    string dutyWork = "";
                    for (int i = 0; i < dutyWorks.Length; i++)
                    {
                        DataRow[] dr = iDutyWork.Select("duty_work='" + dutyWorks[i] + "'");

                        if (!string.IsNullOrEmpty(dutyWork) && dr.Length > 0)
                            dutyWork += ", ";

                        if (dr.Length > 0)
                        {
                            //국영문 구분에 따른 처리
                            if (Thread.CurrentThread.CurrentCulture.Name.ToLower() == "ko-kr")
                                dutyWork += Convert.ToString(iDutyWork.Select("duty_work='" + dutyWorks[i] + "'")[0]["duty_work_name"]);
                            else
                                dutyWork += Convert.ToString(iDutyWork.Select("duty_work='" + dutyWorks[i] + "'")[0]["DUTY_WORK_ENAME"]);
                        }
                    }
                    lblEssStepName.Text += dutyWork;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void PageNavigator1_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        {
            try
            {
                this.CurrentPageIndex = e.PageIndex;
                this.PageInfo1.CurrentPageIndex = e.PageIndex;
                this.PageNavigator1.CurrentPageIndex = e.PageIndex;
                this.BindGrdList("");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        #region protected void grdList_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        protected void grdList_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "과정명";
                        e.Item.Cells[2].Text = "차수";
                        e.Item.Cells[3].Text = "교육기간";
                        e.Item.Cells[4].Text = "일수";
                        e.Item.Cells[5].Text = "확정인원";
                        e.Item.Cells[6].Text = "유효기간(년)";
                        e.Item.Cells[7].Text = "대상";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Course Name";
                        e.Item.Cells[2].Text = "SEQ";
                        e.Item.Cells[3].Text = "Period";
                        e.Item.Cells[4].Text = "Days";
                        e.Item.Cells[5].Text = "Confirm";
                        e.Item.Cells[6].Text = "Expired(year)";
                        e.Item.Cells[7].Text = "Target";
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
