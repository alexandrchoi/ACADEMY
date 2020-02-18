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
using CLT.WEB.UI.COMMON.BASE;
using System.Data.OleDb;
using System.IO;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.FX.AGENT;
using System.Threading;
using C1.Web.C1WebGrid;

namespace CLT.WEB.UI.LMS.APPR
{
    /// <summary>
    /// 1. 작업개요 : 역량평가항목 Class
    /// 
    /// 2. 주요기능 : LMS 역량평가항목
    ///				  
    /// 3. Class 명 : item_excel
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    ///
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class item_excel : BasePage
    {
        private DataTable iDtAppLang { get { return (DataTable)ViewState["appLang"]; } }
        private DataTable iDtVslTypeP { get { return (DataTable)ViewState["vslTypeP"]; } }
        private DataTable iDtVslTypeC { get { return (DataTable)ViewState["vslTypeC"]; } }
        private DataTable iDtStepGu { get { return (DataTable)ViewState["stepGu"]; } }
        private DataTable iDtAppDutyStep1 { get { return (DataTable)ViewState["appDutyStep1"]; } }
        private DataTable iDtAppDutyStep2 { get { return (DataTable)ViewState["appDutyStep2"]; } }

        private DataTable GetDtVslTypeC(string rVslTypeP)
        {
            DataTable xDt = new DataTable();
            try
            {
                xDt = iDtVslTypeC.Clone();
                DataRow[] drResults = iDtVslTypeC.Select("TYPE_P_SHORT_DESC='" + rVslTypeP + "' ");

                foreach (DataRow dr in drResults)
                {
                    object[] row = dr.ItemArray;
                    xDt.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
            return xDt;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "item_excel", xScriptMsg);
                    return;
                }

                base.pRender(this.Page, new object[,] {
                                                    { this.btnExcel, "I" },
                                                    { this.btnSend, "E" }
                                                    }, Convert.ToString(Request.QueryString["MenuCode"]));

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (!IsPostBack)
                    {
                        string[] xParams = new string[1];

                        // 구분
                        xParams[0] = "0052";
                        ViewState["stepGu"] = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                        "GetCommonCodeInfo",
                                                        LMS_SYSTEM.CAPABILITY,
                                                        "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                        // 평가대상
                        xParams[0] = "000001";
                        ViewState["appDutyStep1"] = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                                            "GetDtApprTarget",
                                                            LMS_SYSTEM.CAPABILITY,
                                                            "CLT.WEB.UI.LMS.APPR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                        xParams[0] = "000002";
                        ViewState["appDutyStep2"] = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                                            "GetDtApprTarget",
                                                            LMS_SYSTEM.CAPABILITY,
                                                            "CLT.WEB.UI.LMS.APPR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                        // 언어
                        xParams[0] = "0017";
                        ViewState["appLang"] = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                        "GetCommonCodeInfo",
                                                        LMS_SYSTEM.CAPABILITY,
                                                        "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                        // 선종
                        xParams[0] = "Y";
                        ViewState["vslTypeP"] = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                        "GetVCommonVslType",
                                                        LMS_SYSTEM.CAPABILITY,
                                                        "CLT.WEB.UI.LMS.CURR", (object)xParams);
                        //vslTypeC
                        xParams[0] = "Y";
                        ViewState["vslTypeC"] = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                            "GetVCommonVslTypeC",
                                                            LMS_SYSTEM.CAPABILITY,
                                                            "CLT.WEB.UI.LMS.APPR", (object)xParams);
                        this.btnSend.Visible = false;
                    }
                }
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
                string excelFileFullPath = Util.UploadGetFileName(this.fu_excel, "/file/excel");

                if (!string.IsNullOrEmpty(excelFileFullPath))
                {
                    DataTable dt = Util.GetDtExcelFile(excelFileFullPath, true);

                    //데이터 맵핑및 Validator 체크
                    string xResult = "true";
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{

                    //}

                    if (xResult == "true")
                    {
                        grdItem.DataSource = dt;
                        grdItem.DataBind();
                    }

                    Util.FileDel(excelFileFullPath, false);
                    this.fu_excel.Visible = false;
                    btnExcel.Visible = false;
                    this.btnSend.Visible = true;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                int xCntSel = 0;
                DataTable xDt = new DataTable();
                xDt.Columns.Add("app_base_dt");             //기준일8자

                xDt.Columns.Add("step_gu");                 //구분
                xDt.Columns.Add("app_duty_step");           //평가대상

                xDt.Columns.Add("vsl_typeP");               //선종
                xDt.Columns.Add("vsl_typeC");               //선종
                xDt.Columns.Add("app_lang");                //언어
                xDt.Columns.Add("app_item_seq");            //역량 No
                xDt.Columns.Add("app_item_nm");             //역량명

                xDt.Columns.Add("app_item_nm_eng");         //영문 역량명

                xDt.Columns.Add("app_item_desc");           //국문 역량정의
                xDt.Columns.Add("app_item_desc_eng");       //영문 역량정의
                xDt.Columns.Add("app_case_seq");            //SEQ
                xDt.Columns.Add("app_case_desc");           //국문 행위사례
                xDt.Columns.Add("app_case_desc_eng");       //영문 행위사례
                xDt.Columns.Add("course_ojt");              //OJT
                xDt.Columns.Add("course_lms");              //LMS
                xDt.Columns.Add("course_etc");              //Others
                xDt.Columns.Add("ins_id");

                for (int i = 0; i < this.grdItem.Items.Count; i++)
                {
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("chk_sel")).Checked)
                    {
                        xCntSel++;

                        DataRow xRow = xDt.NewRow();
                        xRow["app_base_dt"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtAppBaseDT")).Text;
                        xRow["step_gu"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("ddlStepGu")).SelectedValue;
                        xRow["app_duty_step"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("ddlAppDutyStep")).SelectedValue;
                        xRow["vsl_typeP"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("ddlVslTypeP")).SelectedValue;
                        xRow["vsl_typeC"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("ddlVslTypeC")).SelectedValue;
                        xRow["app_lang"] = "";// ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("ddlAppLang")).SelectedValue;
                        xRow["app_item_seq"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtAppItemSeq")).Text;
                        xRow["app_item_nm"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtAppItemNM")).Text;
                        xRow["app_item_nm_eng"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtAppItemNMEng")).Text;
                        xRow["app_item_desc"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtAppItemDesc")).Text;
                        xRow["app_item_desc_eng"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtAppItemDescEng")).Text;
                        xRow["app_case_seq"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtAppCaseSeq")).Text;
                        xRow["app_case_desc"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtAppCaseDesc")).Text;
                        xRow["app_case_desc_eng"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtAppCaseDescEng")).Text;
                        xRow["course_ojt"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("hdnCourseOJT")).Text;
                        xRow["course_lms"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("hdnCourseLMS")).Text;
                        xRow["course_etc"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtCourseEtc")).Text;
                        xRow["ins_id"] = Session["user_id"];
                        xDt.Rows.Add(xRow);
                    }
                }

                if (xCntSel > 0)
                {
                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                    "SetApprItemInsert",
                                    LMS_SYSTEM.CAPABILITY,
                                    "vp_a_appraisal_item_excel_wpg", xDt);

                    if (xRtn.ToUpper() == "TRUE")
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001", new string[] { "평가자" }, new string[] { "evaluator" }, Thread.CurrentThread.CurrentCulture));
                    else
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                }
                else
                {
                    ScriptHelper.Page_Alert(this, CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A047", new string[] { "" }, new string[] { "" }, System.Threading.Thread.CurrentThread.CurrentCulture));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void grdItem_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                DataRowView DRV = (DataRowView)e.Item.DataItem;

                DropDownList ddlStepGu = ((DropDownList)e.Item.FindControl("ddlStepGu"));
                WebControlHelper.SetDropDownList(ddlStepGu, iDtStepGu, "D_KNM", "D_CD", WebControlHelper.ComboType.NullAble);
                WebControlHelper.SetSelectText_DropDownList(ddlStepGu, DRV["구분"].ToString());

                TextBox txtAppBaseDT = ((TextBox)e.Item.FindControl("txtAppBaseDT"));

                DropDownList ddlAppDutyStep = ((DropDownList)e.Item.FindControl("ddlAppDutyStep"));
                if (DRV["구분"].ToString() == "현직")
                    WebControlHelper.SetDropDownList(ddlAppDutyStep, iDtAppDutyStep1, "D_KNM", "D_CD", WebControlHelper.ComboType.NullAble);
                else if (DRV["구분"].ToString() == "상위직")
                    WebControlHelper.SetDropDownList(ddlAppDutyStep, iDtAppDutyStep2, "D_KNM", "D_CD", WebControlHelper.ComboType.NullAble);
                WebControlHelper.SetSelectText_DropDownList(ddlAppDutyStep, DRV["평가대상"].ToString());

                //DropDownList ddlAppLang = ((DropDownList)e.Item.FindControl("ddlAppLang"));
                //WebControlHelper.SetDropDownList(ddlAppLang, iDtAppLang, "D_KNM", "D_CD", WebControlHelper.ComboType.NullAble);
                //ddlGrade.Attributes.Add("onchange", "javascript:totGrade();");
                //WebControlHelper.SetSelectText_DropDownList(ddlAppLang, DRV["언어"].ToString());

                DropDownList ddlVslTypeP = ((DropDownList)e.Item.FindControl("ddlVslTypeP"));
                WebControlHelper.SetDropDownList(ddlVslTypeP, iDtVslTypeP, "TYPE_P_SHORT_DESC", "TYPE_P_CD", WebControlHelper.ComboType.NullAble);
                WebControlHelper.SetSelectText_DropDownList(ddlVslTypeP, DRV["선종P"].ToString());

                DropDownList ddlVslTypeC = ((DropDownList)e.Item.FindControl("ddlVslTypeC"));
                DataTable xDt = GetDtVslTypeC(Convert.ToString(DRV["선종P"]));
                WebControlHelper.SetDropDownList(ddlVslTypeC, xDt, "TYPE_C_DESC", "TYPE_C_CD");
                WebControlHelper.SetSelectText_DropDownList(ddlVslTypeC, DRV["선종C"].ToString());

                TextBox hdnCourseOJT = ((TextBox)e.Item.FindControl("hdnCourseOJT"));
                TextBox txtCourseOJT = ((TextBox)e.Item.FindControl("txtCourseOJT"));
                ImageButton btnSearchOJT = ((ImageButton)e.Item.FindControl("btnSearchOJT"));
                btnSearchOJT.OnClientClick = "javascript:return GoCourseForm('" + hdnCourseOJT.ClientID + "','" + txtCourseOJT.ClientID + "', '000005');";

                TextBox hdnCourseLMS = ((TextBox)e.Item.FindControl("hdnCourseLMS"));
                TextBox txtCourseLMS = ((TextBox)e.Item.FindControl("txtCourseLMS"));
                ImageButton btnSearchLMS = ((ImageButton)e.Item.FindControl("btnSearchLMS"));
                btnSearchLMS.OnClientClick = "javascript:return GoCourseForm('" + hdnCourseLMS.ClientID + "','" + txtCourseLMS.ClientID + "', '');";
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void grdItem_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[1].Text = "구분";
                        e.Item.Cells[2].Text = "평가대상";
                        e.Item.Cells[3].Text = "선종";
                        e.Item.Cells[4].Text = "No";
                        e.Item.Cells[5].Text = "역량명";
                        e.Item.Cells[6].Text = "역량정의";
                        e.Item.Cells[7].Text = "SEQ";
                        e.Item.Cells[8].Text = "행위사례";
                        e.Item.Cells[9].Text = "기준일자";
                        e.Item.Cells[10].Text = "OJT";
                        e.Item.Cells[11].Text = "LMS";
                        e.Item.Cells[12].Text = "Others";
                    }
                    else
                    {
                        e.Item.Cells[1].Text = "Classification";
                        e.Item.Cells[2].Text = "Evalution";
                        e.Item.Cells[3].Text = "Vessle Type";
                        e.Item.Cells[4].Text = "No";
                        e.Item.Cells[5].Text = "Name of Competency";
                        e.Item.Cells[6].Text = "Definition of Competency";
                        e.Item.Cells[7].Text = "SEQ";
                        e.Item.Cells[8].Text = "Description";
                        e.Item.Cells[9].Text = "Date";
                        e.Item.Cells[10].Text = "OJT";
                        e.Item.Cells[11].Text = "LMS";
                        e.Item.Cells[12].Text = "Others";
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
                DropDownList ddlStepGu = (DropDownList)sender;
                C1.Web.C1WebGrid.C1GridItem gi = ((C1.Web.C1WebGrid.C1GridItem)((DropDownList)sender).Parent.Parent);
                DropDownList ddlAppDutyStep = (DropDownList)gi.FindControl("ddlAppDutyStep");

                if (ddlStepGu.SelectedValue == "000001")
                    WebControlHelper.SetDropDownList(ddlAppDutyStep, iDtAppDutyStep1, "D_KNM", "D_CD", WebControlHelper.ComboType.NullAble);
                else if (ddlStepGu.SelectedValue == "000002")
                    WebControlHelper.SetDropDownList(ddlAppDutyStep, iDtAppDutyStep2, "D_KNM", "D_CD", WebControlHelper.ComboType.NullAble);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void ddlVslTypeP_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlVslTypeP = (DropDownList)sender;
                C1.Web.C1WebGrid.C1GridItem gi = ((C1.Web.C1WebGrid.C1GridItem)((DropDownList)sender).Parent.Parent);
                DropDownList ddlVslTypeC = (DropDownList)gi.FindControl("ddlVslTypeC");
                WebControlHelper.SetDropDownList(ddlVslTypeC, GetDtVslTypeC(ddlVslTypeP.SelectedItem.Text), "TYPE_C_DESC", "TYPE_C_CD");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

    }
}