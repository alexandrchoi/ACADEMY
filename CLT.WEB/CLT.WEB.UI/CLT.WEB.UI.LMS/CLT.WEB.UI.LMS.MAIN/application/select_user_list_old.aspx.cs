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
    /// 3. Class 명 : select_user_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class select_user_list_old : BasePage
    {
        private DataTable iDtNonApproval;
        private DataTable iDtNationality;
        private DataTable iDtRank;
        private DataTable iDtYN;
        public string iSearch { get { return Util.Request("search"); } }
        public string iTab { get { return string.IsNullOrEmpty(Util.Request("tab")) ? "1" : Util.Request("tab"); } }
        public string iCourseID { get { return Util.Split(Util.Request("search"), "^", 2)[0]; } }
        public string iOpenCourseID { get { return Util.Split(Util.Request("search"), "^", 2)[1]; } }

        #region 인터페이스 그룹

        #endregion

        #region 기타 프로시저 그룹 [Core Logic]
        private void GrdItemBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                DataRowView DRV = (DataRowView)e.Item.DataItem;
                HtmlInputCheckBox chkConfirm = ((HtmlInputCheckBox)e.Item.FindControl("chkConfirm"));

                chkConfirm.Checked = false;
                DropDownList ddlNonApproval = ((DropDownList)e.Item.FindControl("ddlNonApproval"));
                TextBox txtNonApprovalRemark = ((TextBox)e.Item.FindControl("txtNonApprovalRemark"));

                //if (!Util.IsNullOrEmptyObject(ddlNonApproval) && !Util.IsNullOrEmptyObject(txtNonApprovalRemark))
                //{
                //    ddlNonApproval.Visible = false;
                //    txtNonApprovalRemark.Visible = false;
                //}

                if (DRV["confirm"].ToString() == "1")
                {
                    chkConfirm.Checked = true;
                }

                if (!Util.IsNullOrEmptyObject(ddlNonApproval) && !Util.IsNullOrEmptyObject(txtNonApprovalRemark))
                {
                    //ddlNonApproval.Visible = true;
                    //txtNonApprovalRemark.Visible = true;

                    WebControlHelper.SetDropDownList(ddlNonApproval, iDtNonApproval, WebControlHelper.ComboType.NullAble);
                    ddlNonApproval.SelectedValue = Convert.ToString(DRV["non_approval_cd"]);
                    txtNonApprovalRemark.Text = Convert.ToString(DRV["NON_APPROVAL_REMARK"]);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        //교육대상자 New
        private void BindGrid()
        {
            try
            {
                DataTable xDt = null;

                string[] xParams = new string[7];
                xParams[0] = "100";
                xParams[1] = PageNavigator1.CurrentPageIndex.ToString();
                string[] rSearch = Util.Split(Util.Request("search"), "^", 2);
                xParams[2] = rSearch[0];
                xParams[3] = rSearch[1];
                xParams[4] = ddllNationality.SelectedValue;
                xParams[5] = rdoInsu_Y.Checked ? "Y" : "N";
                xParams[6] = ddlRank.SelectedValue;

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                             "GetEduListNew",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);


                this.grdList.DataSource = xDt;
                this.grdList.DataBind();

                if (xDt.Rows.Count < 1)
                {
                    /*
                    this.PageInfo1.TotalRecordCount = 0;
                    this.PageInfo1.PageSize = 100;
                    this.PageNavigator1.TotalRecordCount = 0;
                     */
                    InitialPage();
                }
                else
                {
                    /*
                    this.PageInfo1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    this.PageInfo1.PageSize = 100;
                    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    */
                    this.PageInfo1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    this.PageInfo1.PageSize = 100;
                    this.PageNavigator1.PageSize = 100;
                    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);

                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        private void InitialPage()
        {
            this.PageInfo1.TotalRecordCount = 0;
            this.PageInfo1.PageSize = 100;
            this.PageNavigator1.TotalRecordCount = 0;

            this.PageInfo2.TotalRecordCount = 0;
            this.PageInfo2.PageSize = 100;
            this.PageNavigator2.TotalRecordCount = 0;
        }

        private DataSet GetDsGrdList(string rGubun)
        {
            DataSet xDs = null;
            try
            {
                string[] xParams = new string[7];
                xParams[0] = "100";
                xParams[1] = PageNavigator1.CurrentPageIndex.ToString();
                string[] rSearch = Util.Split(Util.Request("search"), "^", 2);
                xParams[2] = rSearch[0];
                xParams[3] = rSearch[1];
                xParams[4] = ddllNationality.SelectedValue;
                xParams[5] = rdoInsu_Y.Checked ? "Y" : "N";
                xParams[6] = ddlRank.SelectedValue;

                xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                       "GetEduList",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION",
                                       (object)xParams,
                                       rGubun);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDs;
        }

        //교육대상자
        private void BindGrdList(string rGubun)
        {
            try
            {
                InitControl();

                if (!String.IsNullOrEmpty(Convert.ToString(Session["USER_GROUP"])) && Convert.ToString(Session["USER_GROUP"]) != "000009")
                {
                    DataSet xDs = GetDsGrdList(rGubun);
                    DataTable xDtTotalCnt = xDs.Tables[0];
                    DataTable xDt = xDs.Tables[1];

                    if (Convert.ToInt32(xDtTotalCnt.Rows[0][0]) < 1)
                    {
                        this.PageInfo1.PageSize = 100;
                        this.PageInfo1.TotalRecordCount = 0;
                        this.PageNavigator1.TotalRecordCount = 0;
                    }
                    else
                    {
                        this.PageInfo1.PageSize = 100;
                        this.PageInfo1.TotalRecordCount = Convert.ToInt32(xDtTotalCnt.Rows[0][0]);
                        this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDtTotalCnt.Rows[0][0]) - 1;
                    }
                    grdList.DataSource = xDt;
                    grdList.DataBind();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        //전체해상직원
        private DataSet GetDsGrdUserList(string rGubun)
        {
            //ScriptHelper.ScriptStartup(this, "vp_a_appraisal_competency_detail_wpg", "<script>ChangeLayer('2');</script>");

            DataSet xDs = null;
            try
            {
                string[] xParams = new string[4];
                xParams[0] = "100";
                xParams[1] = this.PageNavigator2.CurrentPageIndex.ToString();
                xParams[2] = iCourseID;
                xParams[3] = iOpenCourseID;

                xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                       "GetEduUserList",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION",
                                       (object)xParams,
                                       rGubun);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xDs;
        }
        private void BindGrdUserList(string rGubun)
        {
            try
            {
                InitControl();

                DataSet xDs = GetDsGrdUserList(rGubun);
                DataTable xDtTotalCnt = xDs.Tables[0];
                DataTable xDt = xDs.Tables[1];

                if (Convert.ToInt32(xDtTotalCnt.Rows[0][0]) < 1)
                {
                    this.PageInfo2.PageSize = 100;
                    this.PageInfo2.TotalRecordCount = 0;
                    this.PageNavigator2.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo2.PageSize = 100;
                    this.PageInfo2.TotalRecordCount = Convert.ToInt32(xDtTotalCnt.Rows[0][0]);
                    this.PageNavigator2.TotalRecordCount = Convert.ToInt32(xDtTotalCnt.Rows[0][0]) - 1;
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
        private void SetGrdList(string gubun)
        {
            try
            {
                int xCntSel = 0;
                DataTable xDt = new DataTable();
                xDt.Columns.Add("keys");
                xDt.Columns.Add("confirm");
                xDt.Columns.Add("non_approval_cd");
                xDt.Columns.Add("non_approval_remark");

                if (gubun == "1")
                {
                    for (int i = 0; i < this.grdList.Items.Count; i++)
                    {
                        //if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chk_sel")).Checked)
                        //{
                        // R.COURSE_ID || '^' || R.OPEN_COURSE_ID || '^' || U.USER_ID||'^'|| R.COURSE_RESULT_SEQ
                        DataRow xRow = xDt.NewRow();
                        xRow["keys"] = iCourseID + "^" + iOpenCourseID + "^" + grdList.DataKeys[i].ToString();
                        xRow["confirm"] = ((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chkConfirm")).Checked ? "1" : "0";
                        xRow["non_approval_cd"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("ddlNonApproval")).SelectedValue;
                        xRow["non_approval_remark"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("txtNonApprovalRemark")).Text;

                        xDt.Rows.Add(xRow);

                        xCntSel++;
                        //}
                    }
                }
                else if (gubun == "2")
                {
                    for (int i = 0; i < this.grdUserList.Items.Count; i++)
                    {
                        // R.COURSE_ID || '^' || R.OPEN_COURSE_ID || '^' || U.USER_ID||'^'|| R.COURSE_RESULT_SEQ
                        DataRow xRow = xDt.NewRow();
                        xRow["keys"] = iCourseID + "^" + iOpenCourseID + "^" + grdUserList.DataKeys[i].ToString();
                        xRow["confirm"] = ((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdUserList.Items[i]).FindControl("chkConfirm")).Checked ? "1" : "0";
                        xRow["non_approval_cd"] = "";
                        xRow["non_approval_remark"] = "";
                        //xRow["non_approval_cd"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdUserList.Items[i]).FindControl("ddlNonApproval")).SelectedValue;
                        //xRow["non_approval_remark"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdUserList.Items[i]).FindControl("txtNonApprovalRemark")).Text;

                        xDt.Rows.Add(xRow);
                    }
                }

                if (xCntSel == 0 && gubun == "1")
                {
                    ScriptHelper.Page_Alert(this.Page, CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A047", new string[] { "" }, new string[] { "" }, System.Threading.Thread.CurrentThread.CurrentCulture));
                }
                else
                {
                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                                    "SetEduList",
                                                    LMS_SYSTEM.APPLICATION,
                                                    "CLT.WEB.UI.LMS.APPLICATION",
                                                    xDt, "");

                    if (xRtn.ToUpper() == "TRUE")
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001", new string[] { "교육대상자 선발" }, new string[] { "Educational object selection" }, Thread.CurrentThread.CurrentCulture));
                        //if (iTab == "2")
                        //{   
                        InitControl2();

                        this.BindGrid();
                        //BindGrdUserList("");
                        //}
                        //else
                        //{   
                        //    BindGrdList("");
                        //}
                    }
                    else
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                    }
                }
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
                string[] xParams = new string[1];
                string xSql = string.Empty;

                //DISPLAY: block
                btnSave2.Attributes.Add("style", "display:none;");

                //교육불가사유코드(0011)
                if (Util.IsNullOrEmptyObject(iDtNonApproval))
                {


                    //course type 
                    xParams = new string[2];
                    xParams[0] = "0011";
                    xParams[1] = "Y";
                    iDtNonApproval = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                 "GetCommonCodeInfo",
                                                 LMS_SYSTEM.APPLICATION,
                                                 "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);
                }

                xParams = new string[2];
                xParams[0] = "0064";
                xParams[1] = "Y";
                iDtNationality = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                         "GetCommonCodeInfo",
                                                         LMS_SYSTEM.APPLICATION,
                                                         "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddllNationality, iDtNationality);

                //xParams = new string[2];
                //xParams[0] = "0065";
                //xParams[1] = "Y";
                //iDtYN = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                //                                         "GetCommonCodeInfo",
                //                                         LMS_SYSTEM.APPLICATION,
                //                                         "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);

                //WebControlHelper.SetDropDownList(this.ddlVacationFlg, iDtYN);

                xParams = new string[2];
                xParams[0] = "B20";
                xParams[1] = "Y";

                iDtRank = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                 "GetDutystepCodeInfo",
                                 LMS_SYSTEM.APPLICATION,
                                 "CLT.WEB.UI.LMS.APPLICATION", (object)xParams
                                 , Thread.CurrentThread.CurrentCulture
                                 );
                WebControlHelper.SetDropDownList(this.ddlRank, iDtRank, "step_name", "duty_step", WebControlHelper.ComboType.All);



                // if (Util.IsNullOrEmptyObject(iDtNationality))

                //    xParams = new string[2];
                //    xParams[0] = "0064";
                //    xParams[1] = "Y";
                //    iDtNationality = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                //                                             "GetCommonCodeInfo",
                //                                             LMS_SYSTEM.APPLICATION,
                //                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);
                //}


                //if (this.IsSettingKorean())
                //{
                //    lblTab1.Text = "교육대상자";
                //    lblTab2.Text = "전체해상직원";
                //}
                //else
                //{
                //    lblTab1.Text = "Education object selection";
                //    lblTab2.Text = "ALL";
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        private void InitControl2()
        {
            try
            {
                string[] xParams = new string[1];

                //교육불가사유코드(0011)
                if (Util.IsNullOrEmptyObject(iDtNonApproval))
                {


                    //course type 
                    xParams = new string[2];
                    xParams[0] = "0011";
                    xParams[1] = "Y";
                    iDtNonApproval = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                 "GetCommonCodeInfo",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.EDU", (object)xParams, Thread.CurrentThread.CurrentCulture);
                }


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
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list_wpg", xScriptMsg);

                    return;
                }

                base.pRender(this.Page, new object[,] {
                                                        { this.btnSave1, "E" }
                                                        , { this.btnSave2, "E" }
                                                      });

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (!IsPostBack)
                    {

                        InitControl();

                        //if (iTab == "2")
                        //{
                        //전체해상직원
                        //this.SetGridClear(this.grdUserList, this.PageNavigator2, this.PageInfo2);
                        //ScriptHelper.ScriptStartup(this, "vp_a_appraisal_competency_detail_wpg", "<script>ChangeLayer('2');</script>");
                        //BindGrdUserList("");
                        //}
                        //else
                        //{
                        //교육대상자
                        this.SetGridClear(this.grdList, this.PageNavigator1, this.PageInfo1);
                        ScriptHelper.ScriptStartup(this, "competency_detail", "<script>ChangeLayer('1');</script>");
                        //PageNavigator2.Visible = PageInfo2.Visible = grdUserList.Visible = false;
                        //BindGrdList("");
                        //BindGrid();
                        //}

                        InitialPage();
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
        protected void grdList_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                GrdItemBound(sender, e);
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
                GrdItemBound(sender, e);
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

                InitControl2();
                this.BindGrid();
                //this.BindGrdList("");
                //ScriptHelper.ScriptStartup(this, "select_user_list", "<script>ChangeLayer('1');</script>");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void PageNavigator2_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        {
            try
            {
                this.CurrentPageIndex = e.PageIndex;
                this.PageInfo2.CurrentPageIndex = e.PageIndex;
                this.PageNavigator2.CurrentPageIndex = e.PageIndex;

                InitControl2();
                this.BindGrid();
                //this.BindGrdUserList("");
                //ScriptHelper.ScriptStartup(this, "select_user_list", "<script>ChangeLayer('2');</script>");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void btnSave1_Click(object sender, EventArgs e)
        {
            try
            {
                SetGrdList("1");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void btnSave2_Click(object sender, EventArgs e)
        {
            try
            {
                SetGrdList("2");
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
                //DataTable xDt = GetDsGrdList("all").Tables[1];
                DataTable xDt = null;

                string[] xParams = new string[7];
                xParams[0] = "100000";
                xParams[1] = PageNavigator1.CurrentPageIndex.ToString();
                string[] rSearch = Util.Split(Util.Request("search"), "^", 2);
                xParams[2] = rSearch[0];
                xParams[3] = rSearch[1];
                xParams[4] = ddllNationality.SelectedValue;
                xParams[5] = rdoInsu_Y.Checked ? "Y" : "N";
                xParams[6] = ddlRank.SelectedValue;

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                             "GetEduListNew",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);
                
                int xColumnCnt = 10;
                string[] xExcelHeader = new string[xColumnCnt];
                if (this.IsSettingKorean())
                {
                    xExcelHeader[0] = "현재부서";
                    xExcelHeader[1] = "직급";
                    xExcelHeader[2] = "사번";
                    xExcelHeader[3] = "성명";
                    xExcelHeader[4] = "최종선박하선일";
                    xExcelHeader[5] = "최종하선선박";
                    xExcelHeader[6] = "소속";
                    xExcelHeader[7] = "주민등록번호";
                    xExcelHeader[8] = "주소";
                    xExcelHeader[9] = "연락처";
                }
                else
                {
                    xExcelHeader[0] = "Currently the Department";
                    xExcelHeader[1] = "Grade";
                    xExcelHeader[2] = "ID";
                    xExcelHeader[3] = "Name";
                    xExcelHeader[4] = "Date of Disembarkation";
                    xExcelHeader[5] = "Vessel of Disembarkation";
                    xExcelHeader[6] = "Company Name";
                    xExcelHeader[7] = "Registration No";
                    xExcelHeader[8] = "Address";
                    xExcelHeader[9] = "Staff Mobile";
                    //xExcelHeader[8] = "Tel";
                }

                string[] xDtHeader = new string[xColumnCnt];

                if (this.IsSettingKorean())
                {
                    xDtHeader[0] = "DEPT_NAME";
                    xDtHeader[1] = "STEP_NAME";
                    xDtHeader[2] = "USER_ID";
                    xDtHeader[3] = "USER_NM_KOR";
                    xDtHeader[4] = "ORD_FDATE";
                    xDtHeader[5] = "B_DEPT_NAME";
                    xDtHeader[6] = "COMPANY_NM";
                    xDtHeader[7] = "PERSONAL_NO";
                    xDtHeader[8] = "USER_ADDR";
                    xDtHeader[9] = "MOBILE_PHONE";
                }
                else
                {
                    xDtHeader[0] = "DEPT_NAME";
                    xDtHeader[1] = "STEP_NAME";
                    xDtHeader[2] = "USER_ID";
                    xDtHeader[3] = "USER_NM_KOR";
                    xDtHeader[4] = "ORD_FDATE";
                    xDtHeader[5] = "B_DEPT_NAME";
                    xDtHeader[6] = "COMPANY_NM";
                    xDtHeader[7] = "PERSONAL_NO";
                    xDtHeader[8] = "USER_ADDR";
                    xDtHeader[9] = "MOBILE_PHONE";
                }
                this.GetExcelFile(xDt, xExcelHeader, xDtHeader, "1");
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
                        e.Item.Cells[1].Text = "현재 부서";
                        e.Item.Cells[2].Text = "직급";
                        e.Item.Cells[3].Text = "사번";
                        e.Item.Cells[4].Text = "성명";
                        e.Item.Cells[5].Text = "최종선박하선일";
                        e.Item.Cells[6].Text = "교육신청일";
                        e.Item.Cells[7].Text = "이전이수일";
                        //e.Item.Cells[8].Text = "확정";
                        e.Item.Cells[9].Text = "교육불가사유";
                        e.Item.Cells[10].Text = "불가사유";



                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Currently<br/>the Department";
                        e.Item.Cells[2].Text = "Grade";
                        e.Item.Cells[3].Text = "ID";
                        e.Item.Cells[4].Text = "Name";
                        e.Item.Cells[5].Text = "Date of<br/>Disembarkation";
                        e.Item.Cells[6].Text = "Date of<br/>Application";
                        e.Item.Cells[7].Text = "Date of<br/>Completion";
                        //e.Item.Cells[8].Text = "Confirm";
                        e.Item.Cells[9].Text = "Absent";
                        e.Item.Cells[10].Text = "Remark";


                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        #region protected void grdUserList_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        protected void grdUserList_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "현재 부서";
                        e.Item.Cells[2].Text = "직급";
                        e.Item.Cells[3].Text = "사번";
                        e.Item.Cells[4].Text = "성명";
                        e.Item.Cells[5].Text = "최종선박하선일";
                        e.Item.Cells[6].Text = "교육신청일";
                        e.Item.Cells[7].Text = "이전이수일";
                        //e.Item.Cells[8].Text = "확정";
                        //e.Item.Cells[9].Text = "교육불가사유";
                        //e.Item.Cells[10].Text = "불가사유";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Currently<br/>the Department";
                        e.Item.Cells[2].Text = "Grade";
                        e.Item.Cells[3].Text = "ID";
                        e.Item.Cells[4].Text = "Name";
                        e.Item.Cells[5].Text = "Date of<br/>Disembarkation";
                        e.Item.Cells[6].Text = "Date of<br/>Application";
                        e.Item.Cells[7].Text = "Date of<br/>Completion";
                        //e.Item.Cells[8].Text = "Confirm";
                        //e.Item.Cells[9].Text = "Absent";
                        //e.Item.Cells[10].Text = "Remark";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        #region protected void btnRetrieve_Click(object sender, EventArgs e)
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                InitControl2();

                BindGrid();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

    }
}
