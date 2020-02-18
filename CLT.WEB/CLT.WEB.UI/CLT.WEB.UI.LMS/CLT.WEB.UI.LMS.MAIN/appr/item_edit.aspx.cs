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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.APPR
{
    /// <summary>
    /// 1. 작업개요 : 역량평가항목 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 역량평가항목
    ///				  
    /// 3. Class 명 : item_edit
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    /// 
    public partial class item_edit : BasePage
    {
        bool IsNew = false;

        /************************************************************
        * Function name : Page_Load
        * Purpose       : 페이지 로드될 때 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "item_edit", xScriptMsg);

                    return;
                }
                base.pRender(this.Page, new object[,] {
                                                        { this.btnSend, "E" }
                                                      }, Convert.ToString(Request.QueryString["MenuCode"]));

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    this.txtAppBaseDt.Attributes.Add("onkeyup", "ChkDate(this);");

                    if (!IsPostBack)
                    {
                        if (Request.QueryString["app_item_no"] != null && Request.QueryString["app_item_no"].ToString() != "")
                        {
                            this.BindDropDownList();

                            if (Request.QueryString["app_item_no"].ToString() == "new")
                            {
                                this.IsNew = true;
                            }
                            else
                            {
                                ViewState["app_item_no"] = Request.QueryString["app_item_no"].ToString();
                                if (!String.IsNullOrEmpty(Convert.ToString(Session["USER_GROUP"])) && Convert.ToString(Session["USER_GROUP"]) != "000009")
                                {
                                    this.BindData();
                                }
                            }
                        }
                        if (IsNew)
                            this.txtAppBaseDt.Text = System.DateTime.Now.ToString("yyyy.MM.dd");
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        private void BindDDLApproval()
        {
            try
            {
                // 평가대상
                if (!Util.IsNullOrEmptyObject(ddlStepGu.SelectedValue))
                {
                    string[] xParams = new string[1];
                    xParams[0] = ddlStepGu.SelectedValue;

                    DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                                     "GetDtApprTarget",
                                                     LMS_SYSTEM.CAPABILITY,
                                                     "CLT.WEB.UI.LMS.APPR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                    WebControlHelper.SetDropDownList(this.ddlAppDutyStep, xDt, "D_KNM", "D_CD", WebControlHelper.ComboType.NullAble);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

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

        /************************************************************
        * Function name : BindDropDownList
        * Purpose       : DropDownList 데이터 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void BindDropDownList()
        private void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[1];
                string xSql = string.Empty;
                DataTable xDt = null;

                // 구분
                xParams[0] = "0052";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlStepGu, xDt, WebControlHelper.ComboType.NullAble);

                // 언어
                //xParams[0] = "0017";
                //xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                //                             "GetCommonCodeInfo",
                //                             LMS_SYSTEM.CAPABILITY,
                //                             "CLT.WEB.UI.LMS.CURR", (object)xParams);
                //WebControlHelper.SetDropDownList(this.ddlAppLang, xDt, WebControlHelper.ComboType.NullAble);

                // 선종
                xParams[0] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetVCommonVslType",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams);
                WebControlHelper.SetDropDownList(this.ddlVslType, xDt, "TYPE_P_SHORT_DESC", "TYPE_P_CD", WebControlHelper.ComboType.NullAble);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion 

        /******************************************************************************************
        * Function name : BindData
        * Purpose       : 넘겨받은 grade에 해당하는 데이터를 페이지의 컨트롤에 바인딩 처리
        * Input         : void
        * Output        : void
        ******************************************************************************************/
        private void BindData()
        {
            try
            {
                string[] xParams = new string[1];
                DataTable xDt = null;

                xParams[0] = ViewState["app_item_no"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                                 "GetApprItemInfo",
                                                 LMS_SYSTEM.CAPABILITY,
                                                 "CLT.WEB.UI.LMS.APPR", (object)xParams);

                if (xDt != null && xDt.Rows.Count > 0)
                {
                    DataRow dr = xDt.Rows[0];

                    WebControlHelper.SetSelectItem_DropDownList(this.ddlStepGu, dr["step_gu"].ToString());
                    BindDDLApproval();
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlAppDutyStep, dr["app_duty_step"].ToString());
                    //WebControlHelper.SetSelectItem_DropDownList(this.ddlAppLang, dr["app_lang"].ToString());
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlVslType, dr["vsl_type"].ToString());
                    BindDDLTypeC();
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlVslTypeC, dr["TYPE_C_CD"].ToString());

                    this.txtAppItemNo.Text = dr["app_item_no"].ToString();
                    this.txtAppBaseDt.Text = dr["app_base_dt"].ToString();
                    this.txtAppItemSeq.Text = dr["app_item_seq"].ToString();
                    this.txtAppItemNm.Text = dr["app_item_nm"].ToString();
                    this.txtAppItemNmEng.Text = dr["app_item_nm_eng"].ToString();
                    this.txtAppItemDesc.Text = dr["app_item_desc"].ToString();
                    this.txtAppItemDescEng.Text = dr["app_item_desc_eng"].ToString();
                    this.txtAppCaseSeq.Text = dr["app_case_seq"].ToString();
                    this.txtAppCaseDesc.Text = dr["app_case_desc"].ToString();
                    this.txtAppCaseDescEng.Text = dr["app_case_desc_eng"].ToString();
                    this.txtCourseOJT.Text = dr["course_ojt_nm"].ToString();
                    this.hdnCourseOJT.Text = dr["course_ojt"].ToString();
                    this.txtCourseLMS.Text = dr["course_lms_nm"].ToString();
                    this.hdnCourseLMS.Text = dr["course_lms"].ToString();
                    this.txtCourseETC.Text = dr["course_etc"].ToString();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /******************************************************************************************
        * Function name : btnSend_Click
        * Purpose       : 수정, 추가된 항목 저장 후 선박으로 발송
        * Input         : void
        * Output        : void
        ******************************************************************************************/
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;

                // 필수입력항목 체크
                //if (string.IsNullOrEmpty(this.txtGrade.Text))
                //{
                //    xScriptMsg = "<script>alert('등급을 입력하여 주세요!');</script>";
                //    ScriptHelper.ScriptBlock(this, "code_edit", xScriptMsg);
                //    this.txtGrade.Focus();
                //    return;
                //}

                string[] xParams = new string[19];

                xParams[1] = this.txtAppBaseDt.Text;
                xParams[2] = this.ddlStepGu.SelectedItem.Value;
                xParams[3] = this.ddlAppDutyStep.SelectedItem.Value;
                xParams[4] = "";//this.ddlAppLang.SelectedItem.Value;
                xParams[5] = this.ddlVslType.SelectedValue;
                xParams[6] = this.txtAppItemSeq.Text;
                xParams[7] = this.txtAppItemNm.Text;
                xParams[8] = this.txtAppItemNmEng.Text;
                xParams[9] = this.txtAppItemDesc.Text;
                xParams[10] = this.txtAppItemDescEng.Text;
                xParams[11] = this.txtAppCaseSeq.Text;
                xParams[12] = this.txtAppCaseDesc.Text;
                xParams[13] = this.txtAppCaseDescEng.Text;
                xParams[14] = this.hdnCourseOJT.Text;
                xParams[15] = this.hdnCourseLMS.Text;
                xParams[16] = this.txtCourseETC.Text;
                xParams[17] = Session["USER_ID"].ToString();
                xParams[18] = ddlVslTypeC.SelectedValue;

                this.btnSend.Enabled = false;
                if (IsNew)   // 추가
                {
                    xParams[0] = "";
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                             "SetApprItemInsert",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.APPR",
                                             (object)xParams);

                    if (xRtn.ToUpper() == "TRUE")
                    {
                        ScriptHelper.Page_AlertClose(this.Page, MsgInfo.GetMsg("A001", new string[] { "역량평가항목" }, new string[] { "Competence evaluation items" }, Thread.CurrentThread.CurrentCulture));
                    }
                    else
                    {
                        ScriptHelper.Page_AlertClose(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                    }
                }
                else // 수정
                {
                    xParams[0] = this.txtAppItemNo.Text;
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                             "SetApprItemInsert",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.APPR",
                                             (object)xParams);

                    if (xRtn.ToUpper() == "TRUE")
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001", new string[] { "역량평가항목" }, new string[] { "Competence evaluation items" }, Thread.CurrentThread.CurrentCulture));
                    }
                    else
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void ddlStepGu_SelectedIndexChanged(object sender, EventArgs e)
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
