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
    /// 1. 작업개요 : 역량평가결과 Class
    /// 
    /// 2. 주요기능 : LMS 역량평가결과
    ///				  
    /// 3. Class 명 : competency_appr
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    ///
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class competency_appr : BasePage
    {
        #region 인터페이스 그룹
        #endregion

        string xMenuGubun = string.Empty;    //사용자 그룹에 대한 화면 Menu 권한

        #region 기타 프로시저 그룹 [Core Logic]
        private void BindDDLTypeC()
        {
            try
            {
                // 평가대상
                string[] xParams = new string[2];
                xParams[0] = "Y";
                xParams[1] = ddlVslType.SelectedValue;

                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                 "GetVCommonVslTypeC",
                                                 LMS_SYSTEM.CAPABILITY,
                                                 "CLT.WEB.UI.LMS.APPR", (object)xParams);

                WebControlHelper.SetDropDownList(this.ddlVslTypeC, xDt, "TYPE_C_DESC", "TYPE_C_CD");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        private DataSet GetDsGrdList(string rGubun)
        {
            DataSet xDs = null;
            try
            {
                string[] xParams = new string[14];
                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = Util.ForbidText(txtSTART_DATE.Text);
                xParams[3] = Util.ForbidText(txtEND_DATE.Text);
                xParams[4] = ddlInquiry.SelectedValue;
                xParams[5] = ddlVslType.SelectedValue;
                xParams[6] = ddlShipCode.SelectedValue;
                xParams[7] = "";
                xParams[8] = ddlAppDutyStep.SelectedValue;
                xParams[9] = ddlRePayYN.SelectedValue;
                xParams[10] = Convert.ToString(Session["USER_ID"]);
                xParams[11] = Convert.ToString(Session["USER_GROUP"]);
                xParams[12] = ddlVslTypeC.SelectedValue;
                xParams[13] = xMenuGubun;

                xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                       "GetApprList",
                                       LMS_SYSTEM.CAPABILITY,
                                       "CLT.WEB.UI.LMS.APPR",
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
        private void BindDDLApproval()
        {
            try
            {
                // 평가대상
                string[] xParams = new string[1];
                xParams[0] = ddlInquiry.SelectedValue;

                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                 "GetDtApprTarget",
                                 LMS_SYSTEM.CAPABILITY,
                                 "CLT.WEB.UI.LMS.APPR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlAppDutyStep, xDt, "D_KNM", "D_CD", WebControlHelper.ComboType.All);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        private void InitContorl()
        {
            try
            {
                base.pRender(this.Page, new object[,] {
                                                        { this.btnRetrieve, "I" },
                                                        { this.btnExcel, "I" }
                                                      });
                
                this.txtSTART_DATE.Attributes.Add("onkeyup", "ChkDate(this);");
                this.txtEND_DATE.Attributes.Add("onkeyup", "ChkDate(this);");

                this.txtSTART_DATE.Text = DateFormat(GetFirstDayofMonth());
                this.txtEND_DATE.Text = DateFormat(GetLastDayofMonth());

                string[] xParams = new string[1];
                string xSql = string.Empty;
                DataTable xDt = null;

                //현직/상위직
                xParams = new string[2];
                xParams[0] = "0052";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                     "GetCommonCodeInfo",
                                     LMS_SYSTEM.MANAGE,
                                     "CLT.WEB.UI.LMS.APPR"
                                     , (object)xParams
                                     , Thread.CurrentThread.CurrentCulture
                                     );
                WebControlHelper.SetDropDownList(ddlInquiry, xDt, "d_knm", "d_cd", WebControlHelper.ComboType.NotNullAble);

                //평가대상 바인딩
                BindDDLApproval();

                xParams[0] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetVCommonVslType",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams);
                WebControlHelper.SetDropDownList(this.ddlVslType, xDt, "TYPE_P_SHORT_DESC", "TYPE_P_CD", WebControlHelper.ComboType.All);

                //선명
                xParams = new string[2];
                xParams[0] = "B20";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                     "GetVHDeptCode",
                                     LMS_SYSTEM.CAPABILITY,
                                     "CLT.WEB.UI.LMS.APPR",
                                     xParams,
                                     " ORDER BY dept_ename1 ");
                WebControlHelper.SetDropDownList(ddlShipCode, xDt, "dept_ename1", "dept_ename1");

                BindDDLTypeC();
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
                DataTable xMenu = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                                 "GetMenuAuthority",
                                                 LMS_SYSTEM.CAPABILITY,
                                                 "CLT.WEB.UI.APPR",
                                                 Convert.ToString(Session["MENU_CODE"]),
                                                 Convert.ToString(Session["USER_GROUP"]));

                foreach (DataRow xMenuDr in xMenu.Rows)
                {
                    xMenuGubun = xMenu.Rows[0]["ADMIN_YN"].ToString();
                }

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (!IsPostBack)
                    {
                        this.InitContorl();
                    }
                }
                else
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "competency_appr", xScriptMsg);
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
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable xDt = GetDsGrdList("all").Tables[1];

                int xColumnCnt = 10;
                string[] xExcelHeader = new string[xColumnCnt];
                if (this.IsSettingKorean())
                {
                    xExcelHeader[0] = "평가일자";
                    xExcelHeader[1] = "부서";
                    xExcelHeader[2] = "승선일자";
                    //xExcelHeader[3] = "하선일자";
                    xExcelHeader[3] = "사번";
                    xExcelHeader[4] = "성명";
                    xExcelHeader[5] = "직책";
                    xExcelHeader[6] = "총점수";
                    xExcelHeader[7] = "C,D항목수";
                    xExcelHeader[8] = "평가자 성명";
                    xExcelHeader[9] = "평가자 직급";
                }
                else
                {
                    xExcelHeader[0] = "Date";
                    xExcelHeader[1] = "Dept";
                    xExcelHeader[2] = "Date of Embarkation";
                    //xExcelHeader[3] = "Date of Disembarkation";
                    xExcelHeader[3] = "ID";
                    xExcelHeader[4] = "Name";
                    xExcelHeader[5] = "Grade";
                    xExcelHeader[6] = "Total Point";
                    xExcelHeader[7] = "Number of C,D";
                    xExcelHeader[8] = "Evaluator Name";
                    xExcelHeader[9] = "Evaluator Grade";
                }

                string[] xDtHeader = new string[xColumnCnt];
                xDtHeader[0] = "app_dt";
                xDtHeader[1] = "vsl_cd";
                xDtHeader[2] = "on_dt";
                //xDtHeader[3] = "off_dt";
                xDtHeader[3] = "user_id";
                xDtHeader[4] = "user_nm_kor";
                xDtHeader[5] = "duty_work_ename";
                xDtHeader[6] = "tot_score";
                xDtHeader[7] = "re_cnt";
                xDtHeader[8] = "app_nm";
                xDtHeader[9] = "app_duty_nm";

                this.GetExcelFile(xDt, xExcelHeader, xDtHeader, "1");
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
                HyperLink hlkUserId = ((HyperLink)e.Item.FindControl("hlkUserId"));
                hlkUserId.NavigateUrl = "javascript:void(0);";
                //Inquiry^VessleType^ShipCode^Rank^app_keys^user_id^user_nm_kor^app_dt
                //hlkUserId.Attributes.Add("onclick", "javascript:GoAppForm('" + DRV["step_gu"].ToString() + "^" + DRV["vsl_type"].ToString() + "^" + DRV["vsl_cd"].ToString() + "^" + DRV["user_duty_step"].ToString() + "^" + DRV["app_keys"].ToString() + "^" + DRV["user_id"].ToString() + "^" + DRV["user_nm_kor"].ToString() + "^" + DRV["app_dt"].ToString() + "'); return false;");
                if (ddlInquiry.SelectedValue == "000001")
                    hlkUserId.Attributes.Add("onclick", "javascript:GoAppForm('" + DRV["step_gu"].ToString() + "^" + DRV["vsl_type"].ToString() + "^" + DRV["vsl_cd"].ToString() + "^" + DRV["user_duty_work"].ToString() + "^" + DRV["app_keys"].ToString() + "^" + DRV["user_id"].ToString() + "^" + DRV["user_nm_kor"].ToString() + "^" + DRV["app_dt"].ToString() + "^" + DRV["app_nm"].ToString() + "^" + DRV["app_duty_step"].ToString() + "^" + DRV["app_user_id"].ToString() + "'); return false;");
                else if (ddlInquiry.SelectedValue == "000002")
                    hlkUserId.Attributes.Add("onclick", "javascript:GoAppForm('" + DRV["step_gu"].ToString() + "^" + DRV["vsl_type"].ToString() + "^" + DRV["vsl_cd"].ToString() + "^" + DRV["user_duty_step"].ToString() + "^" + DRV["app_keys"].ToString() + "^" + DRV["user_id"].ToString() + "^" + DRV["user_nm_kor"].ToString() + "^" + DRV["app_dt"].ToString() + "^" + DRV["app_nm"].ToString() + "^" + DRV["app_duty_step"].ToString() + "^" + DRV["app_user_id"].ToString() + "'); return false;");
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
        protected void grdList_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[1].Text = "No.";
                        e.Item.Cells[2].Text = "평가일자";
                        e.Item.Cells[3].Text = "부서";
                        e.Item.Cells[4].Text = "승선일자";
                        //e.Item.Cells[5].Text = "하선일자";
                        e.Item.Cells[5].Text = "사번";
                        e.Item.Cells[6].Text = "성명";
                        e.Item.Cells[7].Text = "직책";
                        e.Item.Cells[8].Text = "총점수";
                        e.Item.Cells[9].Text = "C,D항목수";
                        e.Item.Cells[10].Text = "평가자 성명";
                        e.Item.Cells[11].Text = "평자가 직급";
                    }
                    else
                    {
                        e.Item.Cells[1].Text = "No.";
                        e.Item.Cells[2].Text = "Date";
                        e.Item.Cells[3].Text = "Dept";
                        e.Item.Cells[4].Text = "Date of Embarkation";
                        //e.Item.Cells[5].Text = "Date of Disembarkation";
                        e.Item.Cells[5].Text = "ID";
                        e.Item.Cells[6].Text = "Name";
                        e.Item.Cells[7].Text = "Grade";
                        e.Item.Cells[8].Text = "Total Point";
                        e.Item.Cells[9].Text = "Number of C,D";
                        e.Item.Cells[10].Text = "Evaluator Name";
                        e.Item.Cells[11].Text = "Evaluator Grade";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        protected void ddlInquiry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDDLApproval();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void ddlVslType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDDLTypeC();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

    }
}