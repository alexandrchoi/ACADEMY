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

namespace CLT.WEB.UI.LMS.APPR
{
    /// <summary>
    /// 1. 작업개요 : 사용자 조회 Class
    /// 
    /// 2. 주요기능 : LMS 사용자 조회 화면
    ///				  
    /// 3. Class 명 : competency_user_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class competency_user_list : BasePage
    {
        public string iBindControl { get { return Util.Request("bind_control"); } }
        public string iAppCD { get { return Util.Request("app_cd"); } }
        public string iStepGu { get { return Util.Request("step_gu"); } }
        private string iSearch { get { return Util.Request("search"); } }

        #region 인터페이스 그룹

        #endregion

        #region 기타 프로시저 그룹 [Core Logic]
        private void BindUserList()
        {
            try
            {
                string[] xParams = new string[8];
                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = Util.ForbidText(txtUserId.Text);
                xParams[3] = ddlDeptCode.SelectedValue;
                xParams[4] = Util.ForbidText(txtUserNMKor.Text).ToUpper();
                xParams[5] = Util.ForbidText(txtPersonalNo.Text);
                xParams[6] = ddlDutyStep.SelectedValue;
                xParams[7] = iAppCD;

                DataSet xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                       "GetUserList",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPR",
                                       (object)xParams, Thread.CurrentThread.CurrentCulture);
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

                grdUserList.DataSource = xDt;
                grdUserList.DataBind();
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
                base.pRender(this.Page,
                                 new object[,] {
                                { this.btnRetrieve, "I" }
                             },
                                 Convert.ToString(Request.QueryString["MenuCode"]));

                string[] xParams = new string[1];
                string xSql = string.Empty;
                DataTable xDt = null;

                //부서코드 가져오기
                xParams = new string[2];
                xParams[0] = "B10','B20','B30";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                     "GetVHDeptCode",
                                     LMS_SYSTEM.MANAGE,
                                     "CLT.WEB.UI.LMS.APPR",
                                     xParams,
                                     " ORDER BY dept_ename1 ");
                WebControlHelper.SetDropDownList(ddlDeptCode, xDt, "dept_ename1", "dept_code");

                if (iAppCD == "appr")
                {
                    // 직책(직급)코드 Dutystep
                    xParams = new string[3];
                    xParams[1] = "Y";
                    xParams[2] = "M";
                }
                else
                {
                    // 직책(직급)코드 Dutystep
                    xParams = new string[2];
                    xParams[1] = "Y";
                }
                DataTable xDtDutyStep = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                     "GetDutystepCodeInfo",
                                     LMS_SYSTEM.MANAGE,
                                     "CLT.WEB.UI.LMS.APPR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlDutyStep, xDtDutyStep, "step_name", "duty_step");

                this.SetGridClear(this.grdUserList, this.PageNavigator1, this.PageInfo1);
                //if (!String.IsNullOrEmpty(Convert.ToString(Session["USER_GROUP"])) && Convert.ToString(Session["USER_GROUP"]) != "000009")
                //    BindUserList();
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
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "competency_user_list", xScriptMsg);

                    return;
                }

                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping

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
                BindUserList();
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
                this.BindUserList();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void grdUserList_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                //string[] iBindControls = Util.Split(iBindControl, "^", 4);
                //string xCtlUserId = iBindControls[0];
                //string xCtlUserNMKor = iBindControls[1];
                //string xCtlAppDutyStep = iBindControls[2];
                //string xCtlPersonalNo = iBindControls[3];

                ////승선여부 체크
                ////DataRowView DRV = (DataRowView)e.Item.DataItem;
                ////HyperLink hlkUserId = ((HyperLink)e.Item.FindControl("hlkUserId"));
                ////hlkUserId.NavigateUrl = "javascript:SetUserId('" + DRV["user_id"].ToString() + "','" + DRV["user_nm_kor"].ToString() + "','" + DRV["duty_step"].ToString() + "','" + DRV["personal_no"].ToString() + "','" + xCtlUserId + "','" + xCtlUserNMKor + "','" + xCtlAppDutyStep + "','" + xCtlPersonalNo + "');";

                ////DataRowView DRV = (DataRowView)e.Item.DataItem;
                ////LinkButton hlkUserId = ((LinkButton)e.Item.FindControl("hlkUserId"));
                ////hlkUserId.Comm

            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        protected void grdUserList_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "사번";
                        e.Item.Cells[1].Text = "성명";
                        e.Item.Cells[2].Text = "부서";
                        e.Item.Cells[3].Text = "직급";
                        e.Item.Cells[4].Text = "직책";
                        e.Item.Cells[5].Text = "주민번호";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "ID";
                        e.Item.Cells[1].Text = "Name";
                        e.Item.Cells[2].Text = "Dept";
                        e.Item.Cells[3].Text = "Grade";
                        e.Item.Cells[4].Text = "Position";
                        e.Item.Cells[5].Text = "Personal No";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void grdUserList_ItemCommand(object sender, C1CommandEventArgs e)
        {
            try
            {
                if (iAppCD == "edu_user")
                {
                    if (e.CommandName == "UserId")
                    {
                        int xCntSel = 0;
                        DataTable xDt = new DataTable();
                        xDt.Columns.Add("keys");
                        xDt.Columns.Add("confirm");
                        xDt.Columns.Add("non_approval_cd");
                        xDt.Columns.Add("non_approval_remark");

                        // R.COURSE_ID || '^' || R.OPEN_COURSE_ID || '^' || U.USER_ID||'^'|| R.COURSE_RESULT_SEQ
                        DataRow xRow = xDt.NewRow();
                        xRow["keys"] = iSearch + "^" + grdUserList.DataKeys[e.Item.ItemIndex].ToString();
                        xRow["confirm"] = "1";
                        xRow["non_approval_cd"] = "";
                        xRow["non_approval_remark"] = "";
                        xDt.Rows.Add(xRow);

                        string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                                        "SetEduList",
                                                        LMS_SYSTEM.MANAGE,
                                                        "CLT.WEB.UI.LMS.APPR",
                                                        xDt, "");
                        if (xRtn.ToUpper() == "TRUE")
                        {
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001", new string[] { "교육대상자 선발" }, new string[] { "Educational object selection" }, Thread.CurrentThread.CurrentCulture));
                        }
                        else
                        {
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                        }
                    }
                }
                else
                {
                    if (e.CommandName == "UserId")
                    {
                        bool xIsSave = true;
                        if (iAppCD == "appr")
                        {
                            string[] xParams = new string[1];
                            xParams[0] = grdUserList.DataKeys[e.Item.ItemIndex].ToString();
                            //승선여부 체크
                            DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                           "GetVHOrderdetKind",
                                                           LMS_SYSTEM.MANAGE,
                                                           "CLT.WEB.UI.LMS.APPR",
                                                           (object)xParams);
                            bool isOrderKind = false;
                            if (xDt.Rows.Count > 0)
                            {
                                //'AF6','AN2','AH6', 승선,복직,파견면 일때 승선
                                if (Convert.ToString(xDt.Rows[0]["ORD_KIND"]) == "AF6" || Convert.ToString(xDt.Rows[0]["ORD_KIND"]) == "AN2" || Convert.ToString(xDt.Rows[0]["ORD_KIND"]) == "AH6")
                                    isOrderKind = true;
                            }
                            /*사용자 요청 - 승선한 사람도 역량평가 가능하도록 2013.03.19 Seo.jw*/
                            /*
                            if (isOrderKind)
                            {
                                ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A127",
                                                                  new string[] { "" },
                                                                  new string[] { "" },
                                                                  Thread.CurrentThread.CurrentCulture
                                                                 ));
                                xIsSave = false;
                            }
                            else
                            {
                                //승선이 아닐때
                                xIsSave = true;
                            }
                            */
                            xIsSave = true;
                        }

                        if (xIsSave)
                        {
                            string[] iBindControls = Util.Split(iBindControl, "^", 5);
                            string xCtlUserId = iBindControls[0];
                            string xCtlUserNMKor = iBindControls[1];
                            string xCtlAppDutyStep = iBindControls[2];
                            string xCtlPersonalNo = iBindControls[3];
                            string xCtlShipCode = iBindControls[4];

                            string[] xUsers = Util.Split(e.CommandArgument.ToString(), "^", 6);
                            ScriptHelper.ScriptStartup(this.Page, Guid.NewGuid().ToString(), "<script>SetUserId('" + xUsers[0] + "','" + xUsers[1] + "','" + xUsers[2] + "','" + xUsers[3] + "','" + xUsers[4] + "','" + xUsers[5] + "','" + xCtlUserId + "','" + xCtlUserNMKor + "','" + xCtlAppDutyStep + "','" + xCtlPersonalNo + "','" + xCtlShipCode + "');</script>");
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

    }
}
