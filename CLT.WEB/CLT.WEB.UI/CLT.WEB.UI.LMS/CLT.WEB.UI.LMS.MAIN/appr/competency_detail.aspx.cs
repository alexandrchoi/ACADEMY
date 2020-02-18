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
    /// 1. 작업개요 : 역량평가 입력/조회  Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 역량평가 입력/조회 
    ///				  
    /// 3. Class 명 : competency_detail
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    /// 
    public partial class competency_detail : BasePage
    {
        private bool iIsAppComplete = false;
        private DataTable iDt
        {
            get { return (DataTable)ViewState["iDt"]; }
            set { ViewState["iDt"] = value; }
        }
        private bool iIsView = false;
        private int iTotScore = 0;

        #region 인터페이스 그룹

        #endregion

        #region 기타 프로시저 그룹 [Core Logic]
        public int GetScore(string rGrade)
        {
            int xScore = 0;
            DataRow[] rows = iDt.Select("grade = '" + rGrade + "' ");
            if (rows.Length > 0)
                xScore = Convert.ToInt32((string.IsNullOrEmpty(Convert.ToString(rows[0]["score"]))) ? "0" : rows[0]["score"]);

            return xScore;
        }
        private void BindView()
        {
            btnSave.Visible = false;
            //btnUserForm.Visible = false;
            btnRetrieve.Visible = false;
            //btnCal.Attributes.Add("style", "visibility:hidden;");
            txtName.Enabled = false;
            txtAppDate.Enabled = false;
            ddlInquiry.Enabled = false;
            ddlAppDutyStep.Enabled = false;
            ddlShipCode.Enabled = false;
            ddlVslType.Enabled = false;
            txtTotScore.Enabled = false;
            btnSave.Visible = false;
            ddlVslTypeC.Enabled = false;
            txtMgrNm.Enabled = false;
            //btnMgrForm.Visible = false;
        }
        private DataTable GetDtApprCode()
        {
            DataTable xDt = null;
            try
            {
                if (iDt != null)
                {
                    xDt = iDt;
                }
                else
                {
                    string[] xParams = new string[1];
                    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                           "GetApprCode_Excel",
                                           LMS_SYSTEM.CAPABILITY,
                                           "CLT.WEB.UI.LMS.APPR",
                                           (object)xParams);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }
        private DataTable GetDtApprItem()
        {
            DataTable xDt = null;
            try
            {
                string[] xParams = new string[9];
                xParams[0] = ddlInquiry.SelectedValue;
                xParams[1] = ddlVslType.SelectedValue;
                xParams[2] = ddlShipCode.SelectedValue;
                xParams[3] = Util.ForbidText(txtName.Text);
                xParams[4] = ddlAppDutyStep.SelectedValue;
                xParams[5] = Util.ForbidText(txtAppDate.Text);
                xParams[6] = hdnAppNo.Value;
                xParams[7] = hdnUserId.Value;
                xParams[8] = ddlVslTypeC.SelectedValue;

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                                 "Get_Appr_Detail",
                                                 LMS_SYSTEM.CAPABILITY,
                                                 "CLT.WEB.UI.LMS.APPR",
                                                 (object)xParams,
                                                 (string.IsNullOrEmpty(xParams[6]) ? "new" : "edit"),
                                                 Thread.CurrentThread.CurrentCulture);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xDt;
        }
        private void BindgrdApprItem()
        {
            try
            {
                txtTotScore.Text = "";
                iDt = GetDtApprCode();

                DataTable xDt = GetDtApprItem();
                if (xDt.Rows.Count > 0)
                {
                    txtTotScore.Text = Convert.ToString(xDt.Rows[0]["tot_score"]);
                    //완료여부 체크
                    iIsAppComplete = false;
                    btnSave.Visible = true;
                    if (Convert.ToString(xDt.Rows[0]["app_complete"]) == "1")
                    {
                        iIsAppComplete = true;
                        BindView();
                    }
                }

                grdApprItem.DataSource = xDt;
                grdApprItem.DataBind();

                C1Column col_0 = (C1Column)this.grdApprItem.Columns[0];
                C1Column col_1 = (C1Column)this.grdApprItem.Columns[1];
                C1Column col_2 = (C1Column)this.grdApprItem.Columns[2];

                col_0.Visible = true;
                col_0.RowMerge = RowMergeEnum.Free;
                col_0.GroupInfo.Position = GroupPositionEnum.None;

                col_1.Visible = true;
                col_1.RowMerge = RowMergeEnum.Free;
                col_1.GroupInfo.Position = GroupPositionEnum.None;

                col_2.Visible = true;
                col_2.RowMerge = RowMergeEnum.Free;
                col_2.GroupInfo.Position = GroupPositionEnum.None;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        private void BindGrdCode()
        {
            try
            {
                //string[] xParams = new string[1];
                //iDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                //                       "GetApprCode_Excel",
                //                       LMS_SYSTEM.CAPABILITY,
                //                       "CLT.WEB.UI.LMS.APPR",
                //                       (object)xParams);
                //iDt = GetDtApprCode();

                grdCode.DataSource = GetDtApprCode();
                grdCode.DataBind();
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

                WebControlHelper.SetDropDownList(this.ddlAppDutyStep, xDt, "D_KNM", "D_CD", WebControlHelper.ComboType.NullAble);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        private void InitControl()
        {
            //this.Page.Title = "▒ EUSUSM - LMS ▒";

            try
            {
                base.pRender(this.Page, new object[,] {
                                                        { this.btnRetrieve, "I" },
                                                        { this.btnSave, "E" },
                                                        { this.btnExcel, "I" }
                                                      }, Convert.ToString(Request.QueryString["MenuCode"]));

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (Util.Request("MenuCode") == "331")
                    {
                        iIsView = true;
                    }

                    if (!IsPostBack)
                    {
                        this.txtAppDate.Attributes.Add("onkeyup", "ChkDate(this);");

                        //this.txtAppDate.Text = System.DateTime.Now.ToString("yyyy.MM.dd");
                        //ddlInquiry.Attributes.Add("onchange", "if (!fnValidateForm()){ return };");
                        //ddlVslType.Attributes.Add("onchange", "if (!fnValidateForm()){ return };");
                        //ddlShipCode.Attributes.Add("onchange", "if (!fnValidateForm()){ return };");
                        //ddlAppDutyStep.Attributes.Add("onchange", "if (!fnValidateForm()){ return };");

                        string[] xParams = new string[1];
                        string xSql = string.Empty;
                        DataTable xDt = null;

                        //현직/상위직

                        xParams = new string[2];
                        xParams[0] = "0052";
                        xParams[1] = "Y";
                        xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.APPR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                        WebControlHelper.SetDropDownList(ddlInquiry, xDt, "d_knm", "d_cd", WebControlHelper.ComboType.NotNullAble);

                        // 선종
                        xParams[0] = "Y";
                        xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                     "GetVCommonVslType",
                                                     LMS_SYSTEM.CAPABILITY,
                                                     "CLT.WEB.UI.LMS.APPR", (object)xParams);
                        WebControlHelper.SetDropDownList(this.ddlVslType, xDt, "TYPE_P_SHORT_DESC", "TYPE_P_CD", WebControlHelper.ComboType.NullAble);

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
                        WebControlHelper.SetDropDownList(ddlShipCode, xDt, "dept_ename1", "dept_ename1", WebControlHelper.ComboType.NullAble);

                        //평가대상

                        //xParams = new string[1];
                        //xParams[0] = "0053";
                        //xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                        //                             "GetCommonCodeInfo",
                        //                             LMS_SYSTEM.CAPABILITY,
                        //                             "CLT.WEB.UI.LMS.APPR", (object)xParams);
                        //WebControlHelper.SetDropDownList(this.ddlAppDutyStep, xDt, WebControlHelper.ComboType.NullAble);

                        //Inquiry^VessleType^ShipCode^Rank^app_keys^user_id^user_nm_kor
                        //000001^000004^HJHP^000002^APPR12020006^0915010^법인사 수강자

                        string[] rSearch = Util.Split(Util.Request("search"), "^", 12);
                        ddlInquiry.SelectedValue = rSearch[0];
                        BindDDLApproval();
                        ddlVslType.SelectedValue = rSearch[1];
                        ddlShipCode.SelectedValue = rSearch[2];
                        ddlAppDutyStep.SelectedValue = rSearch[3];
                        hdnAppNo.Value = rSearch[4];
                        hdnUserId.Value = rSearch[5];
                        txtName.Text = rSearch[6];
                        if (!Util.IsNullOrEmptyObject(rSearch[8]))
                            txtMgrNm.Text = rSearch[8];
                        else
                            txtMgrNm.Text = Convert.ToString(Session["USER_KNM"]);
                        if (!Util.IsNullOrEmptyObject(rSearch[7]))
                            txtAppDate.Text = rSearch[7];

                        if (!Util.IsNullOrEmptyObject(rSearch[9]))
                            txtMgrStep.Value = rSearch[9];
                        else
                            txtMgrStep.Value = Convert.ToString(Session["DUTY_STEP"]);

                        if (!Util.IsNullOrEmptyObject(rSearch[10]))
                            txtMgrID.Value = rSearch[10];
                        else
                            txtMgrID.Value = Convert.ToString(Session["USER_ID"]);

                        BindDDLTypeC();

                        /*ddlVslTypeC - 이전 Page에서 Parameter 받아서 Setting 하는것으로 변경 2013.03.15 Seo.jw*/
                        ddlVslTypeC.SelectedValue = rSearch[11];

                        BindGrdCode();

                        if (rSearch[0] != "*" && rSearch[1] != "*" && rSearch[2] != "*" && rSearch[3] != "*" && rSearch[0] != "" && rSearch[1] != "" && rSearch[2] != "" && rSearch[3] != "")
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(Session["USER_GROUP"])) && Convert.ToString(Session["USER_GROUP"]) != "000009")
                                BindgrdApprItem();
                        }

                        if (Util.IsNullOrEmptyObject(hdnAppNo.Value))
                            btnExcel.Visible = false;
                        else
                            btnExcel.Visible = true;

                        if (iIsView)
                        {
                            BindView();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //ddlInquiry.SelectedIndexChanged += new EventHandler(ddlInquiry_SelectedIndexChanged);
                    //ddlVslType.SelectedIndexChanged += new EventHandler(ddlVslType_SelectedIndexChanged);
                    //ddlShipCode.SelectedIndexChanged += new EventHandler(ddlShipCode_SelectedIndexChanged);
                    //ddlAppDutyStep.SelectedIndexChanged += new EventHandler(ddlAppDutyStep_SelectedIndexChanged);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "competency_detail", xScriptMsg);

                    return;
                }

                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping

                if (!IsPostBack)
                {
                    InitControl();
                }

                //현직
                if (ddlInquiry.SelectedValue == "000001")
                {
                    if (this.IsSettingKorean())
                        lblRank.Text = "직책";
                    else
                        lblRank.Text = "Duty Work";
                }
                else if (ddlInquiry.SelectedValue == "000002")
                {
                    if (this.IsSettingKorean())
                        lblRank.Text = "직급";
                    else
                        lblRank.Text = "Duty Step";
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
                BindgrdApprItem();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;

                string[] xParams = new string[15];
                xParams[0] = hdnUserId.Value;
                xParams[1] = ddlInquiry.SelectedValue;
                xParams[2] = ddlAppDutyStep.SelectedValue;
                xParams[3] = ddlVslType.SelectedValue;
                xParams[4] = ddlShipCode.SelectedValue;
                xParams[5] = "";//승선일자
                xParams[6] = Util.ForbidText(txtAppDate.Text);
                xParams[7] = txtMgrID.Value;   //Session["USER_ID"].ToString();    // 평가자 사번
                xParams[8] = txtMgrNm.Text; //Convert.ToString(Session["USER_KNM"]);          // 평가자 성명
                xParams[9] = txtMgrStep.Value; //Convert.ToString(Session["DUTY_STEP"]);          // 평가자 직책
                xParams[10] = Util.ForbidText(txtTotScore.Text);
                xParams[11] = hdnAppNo.Value;
                xParams[12] = ""; //dateKeys
                xParams[13] = ddlVslTypeC.SelectedValue;
                xParams[14] = Session["USER_ID"].ToString();   //로긴 사용자 사번

                DataTable xDt = new DataTable();
                xDt.Columns.Add("app_no");
                xDt.Columns.Add("app_item_no");
                xDt.Columns.Add("grade");
                xDt.Columns.Add("score");

                if (this.grdApprItem.Items.Count == 0)
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A138", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    btnSave.Enabled = true;
                    return;
                }

                for (int i = 0; i < this.grdApprItem.Items.Count; i++)
                {
                    DataRow xRow = xDt.NewRow();
                    string[] dateKeys = Util.Split(grdApprItem.DataKeys[i].ToString(), "^", 2);
                    xRow["app_no"] = dateKeys[0];
                    xRow["app_item_no"] = dateKeys[1];
                    xRow["grade"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdApprItem.Items[i]).FindControl("ddl_grade")).SelectedItem.Text;
                    xRow["score"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdApprItem.Items[i]).FindControl("ddl_grade")).SelectedValue;
                    xDt.Rows.Add(xRow);

                    xParams[12] = dateKeys[0];
                }

                string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                       "SetApprResultDetail",
                                       LMS_SYSTEM.CAPABILITY,
                                       "CLT.WEB.UI.LMS.APPR",
                                       (object)xParams,
                                       xDt,
                                       (string.IsNullOrEmpty(xParams[11]) ? "new" : "edit"));

                string xScriptMsg = "";

                if (xRtn != string.Empty)
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001", new string[] { "역량평가" }, new string[] { "Evaluation results" }, Thread.CurrentThread.CurrentCulture));
                    //BindgrdApprItem();

                    xScriptMsg = "<script>";
                    xScriptMsg += "  opener.__doPostBack('ctl00$ContentPlaceHolderMain$btnRetrieve','');";
                    //xScriptMsg += "  self.close(); ";
                    xScriptMsg += "</script>";
                    ScriptHelper.ScriptBlock(this, "competency_detail", xScriptMsg);

                    hdnAppNo.Value = xRtn.ToString();
                }
                else if (xRtn.ToUpper() == "SEARCH")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A128", new string[] { "역량평가 항목" }, new string[] { "Evaluation Item" }, Thread.CurrentThread.CurrentCulture));
                }
                else if (xRtn.ToUpper() == "DLP")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A135", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                }
                else
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                }

                btnSave.Enabled = true;
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
                DataTable xDt = GetDtApprItem();

                int xColumnCnt = 7;
                string[] xExcelHeader = new string[xColumnCnt];
                if (this.IsSettingKorean())
                {
                    xExcelHeader[0] = "No";
                    xExcelHeader[1] = "역량명";
                    xExcelHeader[2] = "역량정의";
                    xExcelHeader[3] = "Seq";
                    xExcelHeader[4] = "행위사례";
                    xExcelHeader[5] = "평가";
                    xExcelHeader[6] = "비고";
                }
                else
                {
                    xExcelHeader[0] = "No.";
                    xExcelHeader[1] = "Name of Competency";
                    xExcelHeader[2] = "Definition of Competency";
                    xExcelHeader[3] = "Seq";
                    xExcelHeader[4] = "Description";
                    xExcelHeader[5] = "Grade";
                    xExcelHeader[6] = "Remark";
                }

                string[] xDtHeader = new string[xColumnCnt];
                xDtHeader[0] = "app_item_seq";
                xDtHeader[1] = "app_item_nm";
                xDtHeader[2] = "app_item_desc";
                xDtHeader[3] = "app_case_seq";
                xDtHeader[4] = "app_case_desc";
                xDtHeader[5] = "grade";
                xDtHeader[6] = "remark";

                this.GetExcelFile(xDt, xExcelHeader, xDtHeader, "0");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void grdApprItem_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                DataRowView DRV = (DataRowView)e.Item.DataItem;

                string grad = Convert.ToString(DRV["grade"]);
                DropDownList ddlGrade = ((DropDownList)e.Item.FindControl("ddl_grade"));
                WebControlHelper.SetDropDownList(ddlGrade, iDt, "grade", "score", WebControlHelper.ComboType.NullAble);
                ddlGrade.Attributes.Add("onchange", "javascript:totGrade();");
                WebControlHelper.SetSelectText_DropDownList(ddlGrade, grad);

                if (iIsAppComplete)
                    ddlGrade.Enabled = !iIsAppComplete;
                else
                    ddlGrade.Enabled = !(!string.IsNullOrEmpty(Convert.ToString(DRV["grade"])) && Convert.ToString(DRV["grade"]) != "C" && Convert.ToString(DRV["grade"]) != "D");


                if (iIsView)
                    ddlGrade.Enabled = false;

                iTotScore += GetScore(Convert.ToString(DRV["grade"]));

                txtTotScore.Text = iTotScore.ToString();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
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
        protected void grdApprItem_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "역량명";
                        e.Item.Cells[2].Text = "역량정의";
                        e.Item.Cells[3].Text = "Seq";
                        e.Item.Cells[4].Text = "행위사례";
                        e.Item.Cells[5].Text = "평가";
                        e.Item.Cells[6].Text = "비고";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Name of Competency";
                        e.Item.Cells[2].Text = "Definition of Competency";
                        e.Item.Cells[3].Text = "Seq";
                        e.Item.Cells[4].Text = "Description";
                        e.Item.Cells[5].Text = "Grade";
                        e.Item.Cells[6].Text = "Remark";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void grdCode_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "등급";
                        e.Item.Cells[1].Text = "평가내용";
                        e.Item.Cells[2].Text = "영문<br/>평가내용";
                        e.Item.Cells[3].Text = "점수";
                        e.Item.Cells[4].Text = "설명";
                        e.Item.Cells[5].Text = "영문 설명";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "Grade";
                        e.Item.Cells[1].Text = "Level";
                        e.Item.Cells[2].Text = "Level Eng";
                        e.Item.Cells[3].Text = "Point";
                        e.Item.Cells[4].Text = "Criterion";
                        e.Item.Cells[5].Text = "Criterion Eng";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
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
        #endregion

    }
}
