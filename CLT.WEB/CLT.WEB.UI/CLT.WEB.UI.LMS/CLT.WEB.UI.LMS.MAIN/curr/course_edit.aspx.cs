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
using System.Threading; 

using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : course_edit Class
    /// 
    /// 2. 주요기능 : 과정 등록 화면 
    ///				  
    /// 3. Class 명 : course_edit
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>    
    public partial class course_edit : BasePage
    {
        protected string xtest = string.Empty;
        /************************************************************
       * Function name : Page_Load
       * Purpose       : 페이지 로드될 때 처리
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
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list", xScriptMsg);

                    return;
                }

                if (!IsPostBack)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "setLoad", "<script language='javascript'>setLoad();</script>");
                    ViewState["COURSE_ID"] = string.Empty;

                    this.BindDropDownList();
                    this.tabctrl_Click(null, null);
                }

                if (Request.QueryString["TEMP_FLG"] != null && Request.QueryString["TEMP_FLG"].ToString() == "Y")
                {
                    Button_Visible_Set(true);
                }
                else
                {
                    Button_Visible_Set(false);
                }

                if (Request.QueryString["COURSE_ID"] != null && Request.QueryString["COURSE_ID"].ToString() != string.Empty)
                {
                    if (!IsPostBack)
                    {
                        ViewState["COURSE_ID"] = Request.QueryString["COURSE_ID"].ToString();
                    }
                    //temp_save_flg = 'Y' 이면 각 tab의 visible 속성 true 
                    //TEMP_SAVE_FLG = 'N'일 경우 SUBJECT/ASSESS/TEXTBOOK의 각 버튼을 VISIBLE = FALSE 처리
                    string[] xParams = new string[1];
                    DataTable xDt = null;
                    xParams[0] = ViewState["COURSE_ID"].ToString();
                    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                 "GetCourseInfoEdit",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);
                    if (xDt.Rows.Count > 0)
                    {
                        DataRow dr = xDt.Rows[0];
                        ViewState["TEMP_SAVE_FLG"] = dr["TEMP_SAVE_FLG"].ToString();
                        if (dr["TEMP_SAVE_FLG"].ToString() != "Y")
                        {
                            Button_Visible_Set(false);
                        }
                        else
                        {
                            Button_Visible_Set(true);
                        }
                    }

                    if (!IsPostBack)
                    {
                        this.BindData();
                        this.BindGrid2();
                        this.BindGrid2c();
                        this.BindGrid3();
                        this.BindGrid4();
                    }
                }

                base.pRender(this.Page, new object[,] { { this.btnDelete2, "D" }, { this.btnDelete3, "D" }, { this.btnDelete4, "D" }, { this.btnRewrite, "E" }, { this.btnSend, "E" }, { this.btnTemp, "E" }, { this.btnTemp2, "E" }, { this.btnTemp3, "E" }, { this.btnTemp4, "E" }, { this.btnSort, "E" } }
                    , Convert.ToString(Request.QueryString["MenuCode"]));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : Button_Visible_Set
        * Purpose       : button visible 처리 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void Button_Visible_Set(bool rVisible)
        private void Button_Visible_Set(bool rVisible)
        {
            try
            {
                this.btnTemp2.Visible = rVisible;
                this.btnDelete2.Visible = rVisible;
                this.btnSort.Visible = rVisible; 

                this.btnTemp3.Visible = rVisible;
                this.btnDelete3.Visible = rVisible;

                this.btnTemp4.Visible = rVisible;
                this.btnDelete4.Visible = rVisible;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

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

                //language
                xParams[0] = "0017";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlLang, xDt, WebControlHelper.ComboType.NullAble);
                
                //group 
                xParams[0] = "0003";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlGroup, xDt, WebControlHelper.ComboType.NullAble);

                //type
                xParams[0] = "0006";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlType, xDt, WebControlHelper.ComboType.NullAble);

                //field
                xParams[0] = "0004";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlField, xDt, WebControlHelper.ComboType.NullAble);

                //classsification
                xParams[0] = "0042";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlKind, xDt, WebControlHelper.ComboType.NullAble);

                //vsl type
                xParams[0] = "0028";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                DataTable dtVslType = new DataTable();
                DataColumn Col = new DataColumn("vsltypechk", Type.GetType("System.Boolean"));
                dtVslType.Columns.Add(Col);
                Col = new DataColumn("vsltypenm", Type.GetType("System.String"));
                dtVslType.Columns.Add(Col);
                Col = new DataColumn("vsltypeid", Type.GetType("System.String"));
                dtVslType.Columns.Add(Col);

                foreach (DataRow dr in xDt.Rows)
                {
                    DataRow rows = dtVslType.NewRow();
                    rows[0] = false;
                    rows[1] = dr[1].ToString(); //name
                    rows[2] = dr[0].ToString(); //id
                    dtVslType.Rows.Add(rows);
                }

                this.dtlVslType.DataSource = dtVslType;
                this.dtlVslType.DataBind();
                
                ////GetDutyStep
                //xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                //                                        "GetDutyStep",
                //                                        LMS_SYSTEM.CURRICULUM,
                //                                        "CLT.WEB.UI.LMS.CURR", null);

                //DataTable dtGrade = new DataTable();
                //Col = new DataColumn("chk", Type.GetType("System.Boolean"));
                //dtGrade.Columns.Add(Col);
                //Col = new DataColumn("nm", Type.GetType("System.String"));
                //dtGrade.Columns.Add(Col);
                //Col = new DataColumn("id", Type.GetType("System.String"));
                //dtGrade.Columns.Add(Col);

                //foreach (DataRow dr in xDt.Rows)
                //{
                //    DataRow rows = dtGrade.NewRow();
                //    rows[0] = false;
                //    rows[1] = dr[1].ToString(); //name
                //    rows[2] = dr[0].ToString(); //id
                //    dtGrade.Rows.Add(rows);
                //}

                //this.DataList1.DataSource = dtGrade;
                //this.DataList1.DataBind();

                ////GetDutyStep
                //DataTable dtGrade = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                //                                        "GetDutyStep",
                //                                        LMS_SYSTEM.CURRICULUM,
                //                                        "CLT.WEB.UI.LMS.CURR", null);

                //grdGrade.DataSource = dtGrade;
                //grdGrade.DataBind();

                DataTable dtGrade = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                        "GetDutyStep",
                                                        LMS_SYSTEM.CURRICULUM,
                                                        "CLT.WEB.UI.LMS.CURR", null);

                DataTable xdtGrade = new DataTable();
                Col = new DataColumn("stepchk", Type.GetType("System.Boolean"));
                xdtGrade.Columns.Add(Col);
                Col = new DataColumn("stepnm", Type.GetType("System.String"));
                xdtGrade.Columns.Add(Col);
                Col = new DataColumn("stepid", Type.GetType("System.String"));
                xdtGrade.Columns.Add(Col);

                foreach (DataRow dr in dtGrade.Rows)
                {
                    DataRow rows = xdtGrade.NewRow();
                    rows[0] = false;
                    rows[1] = dr["step_nm"].ToString(); //name
                    rows[2] = dr["DUTY_STEP"].ToString(); //id
                    xdtGrade.Rows.Add(rows);
                }

                this.dtlStep.DataSource = xdtGrade;
                this.dtlStep.DataBind();
                
                ////GetDutyWork
                //DataTable dtRank = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                //                                        "GetDutyWork",
                //                                        LMS_SYSTEM.CURRICULUM,
                //                                        "CLT.WEB.UI.LMS.CURR", null);

                //grdRank.DataSource = dtRank;
                //grdRank.DataBind();
                
                //vsl type
                DataTable dtRank = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                        "GetDutyWork",
                                                        LMS_SYSTEM.CURRICULUM,
                                                        "CLT.WEB.UI.LMS.CURR", null);

                DataTable xdtRank = new DataTable();
                Col = new DataColumn("rankchk", Type.GetType("System.Boolean"));
                xdtRank.Columns.Add(Col);
                Col = new DataColumn("ranknm", Type.GetType("System.String"));
                xdtRank.Columns.Add(Col);
                Col = new DataColumn("rankid", Type.GetType("System.String"));
                xdtRank.Columns.Add(Col);

                foreach (DataRow dr in dtRank.Rows)
                {
                    DataRow rows = xdtRank.NewRow();
                    rows[0] = false;
                    rows[1] = dr["duty_nm"].ToString(); //name
                    rows[2] = dr["DUTY_WORK"].ToString(); //id
                    xdtRank.Rows.Add(rows);
                }

                this.dtlRank.DataSource = xdtRank;
                this.dtlRank.DataBind();
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
        * Purpose       : 넘겨받은 contents_id에 해당하는 데이터를 페이지의 컨트롤에 바인딩 처리
        * Input         : void
        * Output        : void
        ******************************************************************************************/
        #region private void BindData()
        private void BindData()
        {
            try
            {
                string[] xParams = new string[1];
                DataTable xDt = null;

                xParams[0] = ViewState["COURSE_ID"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                 "GetCourseInfoEdit",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);

                if (xDt != null && xDt.Rows.Count > 0)
                {
                    DataRow dr = xDt.Rows[0];

                    //VIEWSTATE에 입력되는 정보 저장 
                    ViewState["COURSE_ID"] = dr["COURSE_ID"].ToString();
                    ViewState["TEMP_SAVE_FLG"] = dr["TEMP_SAVE_FLG"].ToString();

                    WebControlHelper.SetSelectItem_DropDownList(this.ddlLang, dr["COURSE_LANG"].ToString());
                    this.rdoInsu.SelectedValue = dr["INSURANCE_FLG"].ToString();
                    this.txtCourseNm.Text = dr["COURSE_NM"].ToString();
                    this.txtCourseNmE.Text = dr["COURSE_NM_ABBR"].ToString();
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlGroup, dr["COURSE_GROUP"].ToString());
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlType, dr["COURSE_TYPE"].ToString());
                    if (dr["COURSE_TYPE"].ToString() == "000005")
                    {
                        this.tabSubject.Enabled = false;
                        this.tabAssess.Enabled = false;
                        this.tabTextBook.Enabled = false;
                    }
                    else
                    {
                        this.tabSubject.Enabled = true;
                        this.tabAssess.Enabled = true;
                        this.tabTextBook.Enabled = true;
                    }

                    WebControlHelper.SetSelectItem_DropDownList(this.ddlField, dr["COURSE_FIELD"].ToString());
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlKind, dr["COURSE_KIND"].ToString());
                    if (!(dr["EXAM_TYPE"] is DBNull))
                        this.rdoExam.SelectedValue = dr["EXAM_TYPE"].ToString();

                    //직급 GRADE
                    string[] xGrade = dr["ESS_DUTY_STEP"].ToString().Split(',');                    
                    for (int i = 0; i < this.dtlStep.Items.Count; i++)
                    {
                        for (int k = 0; k < xGrade.GetLength(0); k++)
                        {
                            if (((Label)this.dtlStep.Items[i].FindControl("lblStepid")).Text == xGrade[k])
                            {
                                ((CheckBox)this.dtlStep.Items[i].FindControl("chkStep")).Checked = true;
                                break;
                            }
                        }
                    }

                    //선종 
                    string[] xVslType = dr["VSL_TYPE"].ToString().Split(',');
                    for (int i = 0; i < this.dtlVslType.Items.Count; i++)
                    {
                        for (int k = 0; k < xVslType.GetLength(0); k++)
                        {
                            if (((Label)this.dtlVslType.Items[i].FindControl("lblVslTypeId")).Text == xVslType[k])
                            {
                                ((CheckBox)this.dtlVslType.Items[i].FindControl("chkVslType")).Checked = true;
                                break;
                            }
                        }
                    }

                    //직책 RANK 
                    string[] xRank = dr["OPT_DUTY_WORK"].ToString().Split(',');
                    for (int i = 0; i < this.dtlRank.Items.Count; i++)
                    {
                        for (int k = 0; k < xRank.GetLength(0); k++)
                        {
                            if (((Label)this.dtlRank.Items[i].FindControl("lblRankid")).Text == xRank[k])
                            {
                                ((CheckBox)this.dtlRank.Items[i].FindControl("chkRank")).Checked = true;
                                break;
                            }
                        }
                    }

                    //for (int i = 1; i < this.grdRank.Items.Count; i++)
                    //{
                    //    for (int k = 0; k < xRank.GetLength(0); k++)
                    //    {
                    //        if (this.grdRank.Items[i].Cells[0].Text == xRank[k])
                    //        {
                    //            ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdRank.Items[i]).FindControl("chkrank")).Checked = true;
                    //            break; 
                    //        }
                    //    }
                    //}

                    this.txtIntro.Text = dr["COURSE_INTRO"].ToString();
                    this.txtGoal.Text = dr["COURSE_OBJECTIVE"].ToString();
                    this.txtCapa.Text = dr["CLASS_MAN_COUNT"].ToString();
                    this.txtExpired.Text = dr["EXPIRED_PERIOD"].ToString();
                    this.txtDays.Text = dr["COURSE_DAY"].ToString();
                    this.txtHours.Text = dr["COURSE_TIME"].ToString();
                    this.rdoUsage.SelectedValue = dr["USE_FLG"].ToString();
                    this.txtManager.Text = dr["MANAGER"].ToString();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : BindGrid
        * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void BindGrid2()
        private void BindGrid2()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                //if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")            
                // 조회조건이 있을 경우 처리
                xParams = new string[3];
                xParams[0] = "15"; // Grid Hear 고정이 불가한 사유로 기존 15개 List를 10개로 변경 
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                xParams[2] = ViewState["COURSE_ID"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                             "GetSubjectList",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams);
                
                this.grd2.DataSource = xDt;
                this.grd2.DataBind();

                if (xDt.Rows.Count < 1)
                {
                    this.PageInfo2.TotalRecordCount = 0;
                    this.PageInfo2.PageSize = 15;
                    this.PageNavigator2.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo2.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    this.PageInfo2.PageSize = 15;
                    this.PageNavigator2.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : BindGrid
        * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void BindGrid2c()
        private void BindGrid2c()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                //if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")            
                // 조회조건이 있을 경우 처리
                xParams = new string[3];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                xParams[2] = ViewState["COURSE_ID"].ToString();

                //if (this.grd2.SelectedIndex < 0)
                //    xParams[2] = string.Empty;
                //else
                //    xParams[2] = this.grd2.Items[this.grd2.SelectedIndex].Cells[7].Text;

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                             "GetContentsList",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams);


                this.grd2c.DataSource = xDt;
                this.grd2c.DataBind();

                if (xDt.Rows.Count < 1)
                {
                    this.PageInfo2c.TotalRecordCount = 0;
                    this.PageInfo2c.PageSize = this.PageSize;
                    this.PageNavigator2c.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo2c.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    this.PageInfo2c.PageSize = this.PageSize;
                    this.PageNavigator2c.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : BindGrid
        * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void BindGrid3()
        private void BindGrid3()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                //if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")            
                // 조회조건이 있을 경우 처리
                xParams = new string[3];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                xParams[2] = ViewState["COURSE_ID"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                             "GetAssessList",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams);
                
                this.grd3.DataSource = xDt;
                this.grd3.DataBind();

                if (xDt.Rows.Count < 1)
                {
                    this.PageInfo3.TotalRecordCount = 0;
                    this.PageInfo3.PageSize = this.PageSize;
                    this.PageNavigator3.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo3.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    this.PageInfo3.PageSize = this.PageSize;
                    this.PageNavigator3.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : BindGrid
        * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void BindGrid4()
        private void BindGrid4()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                //if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")            
                // 조회조건이 있을 경우 처리
                xParams = new string[3];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                xParams[2] = ViewState["COURSE_ID"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                             "GetTextbookList",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams);
                
                this.grd4.DataSource = xDt;
                this.grd4.DataBind();

                if (xDt.Rows.Count < 1)
                {
                    this.PageInfo4.TotalRecordCount = 0;
                    this.PageInfo4.PageSize = this.PageSize;
                    this.PageNavigator4.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo4.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    this.PageInfo4.PageSize = this.PageSize;
                    this.PageNavigator4.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /******************************************************************************************
        * Function name : IsDataValidation
        * Purpose       : Data Validation check 
        * Input         : void
        * Output        : void
        ******************************************************************************************/
        #region private bool IsDataValidation()
        private bool IsDataValidation()
        {
            try
            {
                if (this.ddlLang.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.txtCourseNm.Text == string.Empty)
                    return false;
                if (this.ddlGroup.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.ddlType.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.ddlField.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.ddlKind.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;

                //bool xVslType = false;
                //for (int i = 0; i < this.dtlVslType.Items.Count; i++)
                //{
                //    if (((CheckBox)this.dtlVslType.Items[i].FindControl("chkVslType")).Checked == true)
                //    {
                //        xVslType = true;
                //        break;
                //    }
                //}
                //if (xVslType == false)
                //    return false;

                //if (this.rdoExam.SelectedIndex < 0)
                //    return false;

                if (this.rdoUsage.SelectedIndex < 0)
                    return false;
                
                //bool xGrade = false;                
                //for (int i = 0; i < this.dtlStep.Items.Count; i++)
                //{
                //    if (((CheckBox)this.dtlStep.Items[i].FindControl("chkStep")).Checked == true)
                //    {
                //        xGrade = true;
                //        break;
                //    }
                //}
                //if (xGrade == false)
                //    return false;


                //bool xRank = false;
                //for (int i = 0; i < this.dtlRank.Items.Count; i++)
                //{
                //    if (((CheckBox)this.dtlRank.Items[i].FindControl("chkRank")).Checked == true)
                //    {
                //        xRank = true;
                //        break;
                //    }
                //}
                //if (xRank == false)
                //    return false;


                if (this.txtIntro.Text == string.Empty)
                    return false;

                if (this.txtGoal.Text == string.Empty)
                    return false;

                //if (this.txtCapa.Text == string.Empty) // 정원 체크
                //    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return true;
        }
        #endregion

        /******************************************************************************************
        * Function name : Save
        * Purpose       : Save 
        * Input         : void
        * Output        : void
        ******************************************************************************************/
        private bool Save(bool rTemp)
        {
            try
            {
                if (this.IsDataValidation())
                {
                    string xGrade = string.Empty;
                    for (int i = 0; i < this.dtlStep.Items.Count; i++)
                    {
                        if (((CheckBox)this.dtlStep.Items[i].FindControl("chkStep")).Checked == true)
                        {
                            if (xGrade != string.Empty)
                                xGrade += ",";
                            xGrade += ((Label)this.dtlStep.Items[i].FindControl("lblStepid")).Text;
                        }
                    }

                    string xVslType = string.Empty;
                    for (int i = 0; i < this.dtlVslType.Items.Count; i++)
                    {
                        if (((CheckBox)this.dtlVslType.Items[i].FindControl("chkVslType")).Checked == true)
                        {
                            if (xVslType != string.Empty)
                                xVslType += ",";
                            xVslType += ((Label)this.dtlVslType.Items[i].FindControl("lblVslTypeId")).Text;
                        }
                    }
                    ////선종의 경우, 과정조회 화면에서 DESCRIPTION을 보여줘야 함.. 
                    ////이때, 마지막 따옴표가 없으면, 마지막 값이 안보임.. 
                    ////하여.. 저장 할때 선종이 있을 경우 마지막 따옴표를 넣어 줌 
                    //if (xVslType != string.Empty)
                    //    xVslType += ","; 

                    string xRank = string.Empty;
                    for (int i = 0; i < this.dtlRank.Items.Count; i++)
                    {
                        if (((CheckBox)this.dtlRank.Items[i].FindControl("chkRank")).Checked == true)
                        {
                            if (xRank != string.Empty)
                                xRank += ",";
                            xRank += ((Label)this.dtlRank.Items[i].FindControl("lblRankid")).Text;
                        }
                    }

                    string[] xParams = new string[22];
                    xParams[0] = ViewState["COURSE_ID"] != null ? ViewState["COURSE_ID"].ToString() : string.Empty;
                    xParams[1] = this.ddlLang.SelectedValue;
                    xParams[2] = this.rdoInsu.SelectedValue;
                    xParams[3] = this.txtCourseNm.Text;
                    xParams[4] = this.txtCourseNmE.Text;
                    xParams[5] = this.ddlGroup.SelectedValue;
                    xParams[6] = this.ddlType.SelectedValue;
                    xParams[7] = this.ddlField.SelectedValue;
                    xParams[8] = this.ddlKind.SelectedValue;
                    xParams[9] = string.Empty;
                    xParams[9]  = this.rdoExam.SelectedValue;
                    xParams[10] = xGrade;
                    xParams[11] = xVslType;
                    xParams[12] = xRank;
                    xParams[13] = this.txtIntro.Text;
                    xParams[14] = this.txtGoal.Text;
                    xParams[15] = this.txtCapa.Text;
                    xParams[16] = this.txtExpired.Text;
                    xParams[17] = this.txtDays.Text;
                    xParams[18] = this.txtHours.Text;
                    xParams[19] = this.rdoUsage.SelectedValue;
                    xParams[20] = Session["USER_ID"].ToString();
                    xParams[21] = this.txtManager.Text;

                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                        "SetCourseInfo",
                                                        LMS_SYSTEM.CURRICULUM,
                                                        "CLT.WEB.UI.LMS.CURR", (object)xParams);

                    if (xRtn != string.Empty)
                    {
                        if (rTemp)
                        {
                            //A001: {0}이(가) 저장되었습니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                              new string[] { "과정" },
                                                              new string[] { "Course" },
                                                              Thread.CurrentThread.CurrentCulture
                                                             ));
                        }
                        //저장 후 신규 id 값으로 재조회 
                        ViewState["COURSE_ID"] = xRtn;
                        //Request.QueryString["TEMP_FLG"] = "Y"; 
                        Button_Visible_Set(true);
                        this.BindData();
                        this.BindGrid2();
                        this.BindGrid2c();
                        this.BindGrid3();
                        this.BindGrid4();
                    }
                    else
                    {
                        //A004: {0}이(가) 입력되지 않았습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004",
                                                          new string[] { "과목" },
                                                          new string[] { "Subject" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));
                        return false; 

                    }

                }
                else
                {
                    //A012: {0}의 필수 항목 입력이 누락되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                      new string[] { "과정" },
                                                      new string[] { "Course" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                    return false; 
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return true; 
        }

        /************************************************************
        * Function name : btnSend_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                //temp_save_flg = 'Y'일 경우에만 send 할수 있음 
                if (ViewState["TEMP_SAVE_FLG"] != null && ViewState["TEMP_SAVE_FLG"].ToString() == "Y")
                {
                    //send 후 temp_save_flg = 'N'으로 변경 
                    //각 tab의 버튼 visible 속성을 false로 변경 
                    if (this.Save(false))
                    {
                        string[] xParams = new string[2];
                        xParams[0] = ViewState["COURSE_ID"] != null ? ViewState["COURSE_ID"].ToString() : string.Empty;
                        xParams[1] = Session["USER_ID"].ToString();

                        string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                        "SetCourseSend",
                                                        LMS_SYSTEM.CURRICULUM,
                                                        "CLT.WEB.UI.LMS.CURR", (object)xParams);

                        if (xRtn == "Y")
                        {
                            //A017: {0}이(가) 발송되었습니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A017",
                                                              new string[] { "과정" },
                                                              new string[] { "Course" },
                                                              Thread.CurrentThread.CurrentCulture
                                                             ));

                            this.Button_Visible_Set(false);
                            this.BindData();
                            this.BindGrid2();
                            this.BindGrid2c();
                            this.BindGrid3();
                            this.BindGrid4();
                        }
                        else
                        {
                            //A004: {0}이(가) 입력되지 않았습니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004",
                                                              new string[] { "과정" },
                                                              new string[] { "Course" },
                                                              Thread.CurrentThread.CurrentCulture
                                                             ));
                        }
                    }
                }
                else
                {
                    //임시저장되지 않았습니다. 
                    //임시저장해 주시기 바랍니다. 

                    //A004: {0}이(가) 입력되지 않았습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004",
                                                      new string[] { "과정" },
                                                      new string[] { "Course" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));

                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : btnRewrite_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnRewrite_Click(object sender, EventArgs e)
        protected void btnRewrite_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["COURSE_ID"] = string.Empty;
                ViewState["TEMP_SAVE_FLG"] = string.Empty;

                this.ddlLang.SelectedIndex = 0;
                this.rdoInsu.SelectedItem.Selected = false;
                this.txtCourseNm.Text = string.Empty;
                this.txtCourseNmE.Text = string.Empty;
                this.ddlGroup.SelectedIndex = 0;
                this.ddlType.SelectedIndex = 0;
                this.ddlField.SelectedIndex = 0;
                this.ddlKind.SelectedIndex = 0;
                this.rdoExam.SelectedItem.Selected = false;

                //for (int i = 1; i < this.grdGrade.Items.Count; i++)
                //{
                //    ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdGrade.Items[i]).FindControl("chkduty")).Checked = false;
                //}
                for (int i = 0; i < this.dtlStep.Items.Count; i++)
                {
                    ((CheckBox)this.dtlStep.Items[i].FindControl("chkStep")).Checked = false;
                }

                for (int i = 0; i < this.dtlVslType.Items.Count; i++)
                {
                    ((CheckBox)this.dtlVslType.Items[i].FindControl("chkVslType")).Checked = false;
                }

                //for (int i = 1; i < this.grdRank.Items.Count; i++)
                //{
                //    ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdRank.Items[i]).FindControl("chkrank")).Checked = false;
                //}
                for (int i = 0; i < this.dtlRank.Items.Count; i++)
                {
                    ((CheckBox)this.dtlRank.Items[i].FindControl("chkRank")).Checked = false;
                }


                this.txtIntro.Text = string.Empty;
                this.txtGoal.Text = string.Empty;
                this.txtCapa.Text = string.Empty;
                this.txtExpired.Text = string.Empty;
                this.txtDays.Text = string.Empty;
                this.txtHours.Text = string.Empty;
                this.rdoUsage.SelectedItem.Selected = false;

                this.BindGrid2();
                this.BindGrid3();
                this.BindGrid4();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnTemp_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnTemp_Click(object sender, EventArgs e)
        protected void btnTemp_Click(object sender, EventArgs e)
        {
            try
            {
                //temp_save_flg = 'Y' 
                //각 tab의 버튼 visible 속성을 true로 변경 

                //ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A0014"
                //    , new string[] { "과목" }, new string[] { "Subject" }, Thread.CurrentThread.CurrentCulture)); 

                this.Save(true); 
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }

        }
        #endregion
        
        /************************************************************
        * Function name : btnTemp2_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnTemp2_Click(object sender, EventArgs e)
        protected void btnTemp2_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["COURSE_ID"] != null && ViewState["COURSE_ID"].ToString() != string.Empty)
                {
                    string xlstSubjectId = string.Empty;
                    for (int i = 0; i < this.grd2.Items.Count; i++)
                    {
                        xlstSubjectId += this.grd2.Items[i].Cells[8].Text + "|";
                    }

                    string[] xParams = new string[2];
                    xParams[0] = ViewState["COURSE_ID"] != null ? ViewState["COURSE_ID"].ToString() : string.Empty;
                    xParams[1] = xlstSubjectId;

                    SBROKER.ExecuteOnly("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                    "SetCourseSubjectInfo",
                                                    LMS_SYSTEM.CURRICULUM,
                                                    "CLT.WEB.UI.LMS.CURR", (object)xParams);
                    //A001: {0}이(가) 저장되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                      new string[] { "과정 & 과목" },
                                                      new string[] { "Course & Subject" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                    this.BindGrid2();
                    this.BindGrid2c();
                }
                else
                {
                    //A012: {0}의 필수 항목 입력이 누락되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                      new string[] { "과정" },
                                                      new string[] { "Course" },
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
        * Function name : btnDelete2_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnDelete2_Click(object sender, EventArgs e)
        protected void btnDelete2_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["COURSE_ID"] != null && ViewState["COURSE_ID"].ToString() != string.Empty)
                {
                    string xlstSubjectId = string.Empty;
                    bool check = false;
                    for (int i = 0; i < this.grd2.Items.Count; i++)
                    {
                        check = ((CheckBox)this.grd2.Items[i].FindControl("chkDel")).Checked;
                        if (check)
                        {
                            xlstSubjectId += this.grd2.Items[i].Cells[8].Text + "|";
                        }
                    }

                    string[] xParams = new string[3];
                    xParams[0] = ViewState["COURSE_ID"] != null ? ViewState["COURSE_ID"].ToString() : string.Empty;
                    xParams[1] = xlstSubjectId;
                    xParams[2] = Session["USER_ID"].ToString();

                    SBROKER.ExecuteOnly("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                    "SetCourseSubjectInfo_Del",
                                                    LMS_SYSTEM.CURRICULUM,
                                                    "CLT.WEB.UI.LMS.CURR", (object)xParams);
                    //A002: {0}이(가) 삭제되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A002",
                                                      new string[] { "과정 & 과목" },
                                                      new string[] { "Course & Subject" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                    this.BindGrid2();
                    this.BindGrid2c();
                }
                else
                {
                    //A012: {0}의 필수 항목 입력이 누락되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                      new string[] { "과정" },
                                                      new string[] { "Course" },
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
        * Function name : btnSort_Click
        * Purpose       : subject sort 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnSort_Click(object sender, EventArgs e)
        protected void btnSort_Click(object sender, EventArgs e)
        {
            try
            {
                string xScriptContent = string.Format("<script>openPopWindow('/curr/course_subject_sort.aspx?course_id={0}&MenuCode={1}', 'course_subject_sort_win', '800', '650');</script>", ViewState["COURSE_ID"], Session["MENU_CODE"]);
                ScriptHelper.ScriptBlock(this, "course_subject_sort_win", xScriptContent);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnTemp2_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnTemp3_Click(object sender, EventArgs e)
        protected void btnTemp3_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["COURSE_ID"] != null && ViewState["COURSE_ID"].ToString() != string.Empty)
                {
                    //시험 유형이 none(000001)일 경우 assess 저장 하지 못하도록 함 
                    //시험유형이 현재 none, random(000002)만 enable 상태임 
                    if (this.rdoExam.SelectedItem.Value == "000002")
                    {

                        string xlstAssessId = string.Empty;
                        for (int i = 0; i < this.grd3.Items.Count; i++)
                        {
                            xlstAssessId += this.grd3.Items[i].Cells[7].Text + "|";
                        }

                        string[] xParams = new string[2];
                        xParams[0] = ViewState["COURSE_ID"] != null ? ViewState["COURSE_ID"].ToString() : string.Empty;
                        xParams[1] = xlstAssessId;

                        SBROKER.ExecuteOnly("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                        "SetCourseAssessInfo",
                                                        LMS_SYSTEM.CURRICULUM,
                                                        "CLT.WEB.UI.LMS.CURR", (object)xParams);
                        //A001: {0}이(가) 저장되었습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                          new string[] { "과정 & 평가문제" },
                                                          new string[] { "Course & Assess Question" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));
                        this.BindGrid3();
                    }
                    else
                    {
                        //A020: {0}을 선택할 수 없습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A020",
                                                          new string[] { "평가문제" },
                                                          new string[] { "Assess Question" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));
                    }
                }
                else
                {
                    //A012: {0}의 필수 항목 입력이 누락되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                      new string[] { "과정" },
                                                      new string[] { "Course" },
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
        * Function name : btnDelete2_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnDelete3_Click(object sender, EventArgs e)
        protected void btnDelete3_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["COURSE_ID"] != null && ViewState["COURSE_ID"].ToString() != string.Empty)
                {
                    string xlstAssesstId = string.Empty;
                    bool check = false;
                    for (int i = 0; i < this.grd3.Items.Count; i++)
                    {
                        check = ((CheckBox)this.grd3.Items[i].FindControl("chkDel")).Checked;
                        if (check)
                        {
                            xlstAssesstId += this.grd3.Items[i].Cells[7].Text + "|";
                        }
                    }

                    string[] xParams = new string[3];
                    xParams[0] = ViewState["COURSE_ID"] != null ? ViewState["COURSE_ID"].ToString() : string.Empty;
                    xParams[1] = xlstAssesstId;
                    xParams[2] = Session["USER_ID"].ToString();

                    SBROKER.ExecuteOnly("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                    "SetCourseAssessInfo_Del",
                                                    LMS_SYSTEM.CURRICULUM,
                                                    "CLT.WEB.UI.LMS.CURR", (object)xParams);
                    //A002: {0}이(가) 삭제되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A002",
                                                      new string[] { "과정 & 평가문제" },
                                                      new string[] { "Course & Assess Question" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                    this.BindGrid3();
                }
                else
                {
                    //A012: {0}의 필수 항목 입력이 누락되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                      new string[] { "과정" },
                                                      new string[] { "Course" },
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
        * Function name : btnTemp4_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnTemp4_Click(object sender, EventArgs e)
        protected void btnTemp4_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["COURSE_ID"] != null && ViewState["COURSE_ID"].ToString() != string.Empty)
                {
                    string xlstTextbookId = string.Empty;
                    for (int i = 0; i < this.grd4.Items.Count; i++)
                    {
                        xlstTextbookId += this.grd4.Items[i].Cells[9].Text + "|";
                    }

                    string[] xParams = new string[2];
                    xParams[0] = ViewState["COURSE_ID"] != null ? ViewState["COURSE_ID"].ToString() : string.Empty;
                    xParams[1] = xlstTextbookId;

                    SBROKER.ExecuteOnly("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                    "SetCourseTextbookInfo",
                                                    LMS_SYSTEM.CURRICULUM,
                                                    "CLT.WEB.UI.LMS.CURR", (object)xParams);
                    //A001: {0}이(가) 저장되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                      new string[] { "과정 & 교재" },
                                                      new string[] { "Course & Textbook" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                    this.BindGrid4();
                }
                else
                {
                    //A012: {0}의 필수 항목 입력이 누락되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                      new string[] { "과정" },
                                                      new string[] { "Course" },
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
        * Function name : btnDelete2_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnDelete4_Click(object sender, EventArgs e)
        protected void btnDelete4_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["COURSE_ID"] != null && ViewState["COURSE_ID"].ToString() != string.Empty)
                {
                    string xlstTextbookId = string.Empty;
                    bool check = false;
                    for (int i = 0; i < this.grd4.Items.Count; i++)
                    {
                        check = ((CheckBox)this.grd4.Items[i].FindControl("chkDel")).Checked;
                        if (check)
                        {
                            xlstTextbookId += this.grd4.Items[i].Cells[9].Text + "|";
                        }
                    }

                    string[] xParams = new string[3];
                    xParams[0] = ViewState["COURSE_ID"] != null ? ViewState["COURSE_ID"].ToString() : string.Empty;
                    xParams[1] = xlstTextbookId;
                    xParams[2] = Session["USER_ID"].ToString();

                    SBROKER.ExecuteOnly("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                    "SetCourseTextbookInfo_Del",
                                                    LMS_SYSTEM.CURRICULUM,
                                                    "CLT.WEB.UI.LMS.CURR", (object)xParams);
                    //A002: {0}이(가) 삭제되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A002",
                                                      new string[] { "과정 & 교재" },
                                                      new string[] { "Course & Textbook" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                    this.BindGrid4();
                }
                else
                {
                    //A012: {0}의 필수 항목 입력이 누락되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                      new string[] { "과정" },
                                                      new string[] { "Course" },
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
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void grd2_ItemCreated(object sender, C1ItemEventArgs e)
        protected void grd2_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "언어";
                        e.Item.Cells[2].Text = "분류";
                        e.Item.Cells[3].Text = "과목명";
                        e.Item.Cells[4].Text = "교육시간";
                        e.Item.Cells[5].Text = "강사명";
                        e.Item.Cells[6].Text = "Usage";
                        e.Item.Cells[7].Text = "등록일자";
                        e.Item.Cells[8].Text = "subject_id";
                        e.Item.Cells[9].Text = "subject_nm";
                        e.Item.Cells[10].Text = "chk";
                        e.Item.Cells[11].Text = "temp";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Language";
                        e.Item.Cells[2].Text = "Classification";
                        e.Item.Cells[3].Text = "Subject";
                        e.Item.Cells[4].Text = "Learinig Time";
                        e.Item.Cells[5].Text = "Instructor";
                        e.Item.Cells[6].Text = "Usage";
                        e.Item.Cells[7].Text = "Reg Date";
                        e.Item.Cells[8].Text = "subject_id";
                        e.Item.Cells[9].Text = "subject_nm";
                        e.Item.Cells[10].Text = "chk";
                        e.Item.Cells[11].Text = "temp";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void PageNavigator2_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        protected void PageNavigator2_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        {
            try
            {
                this.CurrentPageIndex = e.PageIndex;
                PageInfo2.CurrentPageIndex = e.PageIndex;
                PageNavigator2.CurrentPageIndex = e.PageIndex;
                this.BindGrid2();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void grd2c_ItemCreated(object sender, C1ItemEventArgs e)
        protected void grd2c_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "분류";
                        e.Item.Cells[2].Text = "과목명";
                        e.Item.Cells[3].Text = "컨텐츠명";
                        e.Item.Cells[4].Text = "파일명";
                        e.Item.Cells[5].Text = "분류";
                        e.Item.Cells[6].Text = "언어";
                        e.Item.Cells[7].Text = "Remark";
                        e.Item.Cells[8].Text = "등록일";
                        e.Item.Cells[9].Text = "contents_id";
                        e.Item.Cells[10].Text = "temp_save_flg";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Classification";
                        e.Item.Cells[2].Text = "Subject";
                        e.Item.Cells[3].Text = "Contents Name";
                        e.Item.Cells[4].Text = "File Name";
                        e.Item.Cells[5].Text = "Type";
                        e.Item.Cells[6].Text = "Language";
                        e.Item.Cells[7].Text = "Remark";
                        e.Item.Cells[8].Text = "Reg Date";
                        e.Item.Cells[9].Text = "contents_id";
                        e.Item.Cells[10].Text = "temp_save_flg";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void PageNavigator2_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        protected void PageNavigator2c_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        {
            try
            {
                this.CurrentPageIndex = e.PageIndex;
                PageInfo2c.CurrentPageIndex = e.PageIndex;
                PageNavigator2c.CurrentPageIndex = e.PageIndex;
                this.BindGrid2c();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void grd3_ItemCreated(object sender, C1ItemEventArgs e)
        protected void grd3_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "분류";
                        e.Item.Cells[2].Text = "과정그룹";
                        e.Item.Cells[3].Text = "과정분야";
                        e.Item.Cells[4].Text = "질문";
                        e.Item.Cells[5].Text = "시험유형";
                        e.Item.Cells[6].Text = "등록일자";
                        e.Item.Cells[7].Text = "question_id";
                        e.Item.Cells[8].Text = "question_content";
                        e.Item.Cells[9].Text = "Chk";
                        e.Item.Cells[10].Text = "temp_save_flg";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Classification";
                        e.Item.Cells[2].Text = "Course Group";
                        e.Item.Cells[3].Text = "Course Field";
                        e.Item.Cells[4].Text = "Question";
                        e.Item.Cells[5].Text = "Exam Type";
                        e.Item.Cells[7].Text = "question_id";
                        e.Item.Cells[8].Text = "question_content";
                        e.Item.Cells[9].Text = "Chk";
                        e.Item.Cells[10].Text = "temp_save_flg";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void PageNavigator3_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        protected void PageNavigator3_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        {
            try
            {
                this.CurrentPageIndex = e.PageIndex;
                PageInfo3.CurrentPageIndex = e.PageIndex;
                PageNavigator3.CurrentPageIndex = e.PageIndex;
                this.BindGrid3();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : grd4_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void grd4_ItemCreated(object sender, C1ItemEventArgs e)
        protected void grd4_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "분류";
                        e.Item.Cells[2].Text = "교재명";
                        e.Item.Cells[3].Text = "저자";
                        e.Item.Cells[4].Text = "출판사";
                        e.Item.Cells[5].Text = "언어";
                        e.Item.Cells[6].Text = "등록자";
                        e.Item.Cells[7].Text = "등록일자";
                        e.Item.Cells[8].Text = "사용여부";
                        e.Item.Cells[9].Text = "textbook_id";
                        e.Item.Cells[10].Text = "textbook_nm";
                        e.Item.Cells[11].Text = "Chk";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Type";
                        e.Item.Cells[2].Text = "TextBook Name";
                        e.Item.Cells[3].Text = "Author";
                        e.Item.Cells[4].Text = "Publisher";
                        e.Item.Cells[5].Text = "Language";
                        e.Item.Cells[6].Text = "Ins User";
                        e.Item.Cells[7].Text = "Ins Date";
                        e.Item.Cells[8].Text = "Usage";
                        e.Item.Cells[9].Text = "textbook_id";
                        e.Item.Cells[10].Text = "textbook_nm";
                        e.Item.Cells[11].Text = "Chk";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void PageNavigator4_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        protected void PageNavigator4_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        {
            try
            {
                this.CurrentPageIndex = e.PageIndex;
                PageInfo4.CurrentPageIndex = e.PageIndex;
                PageNavigator4.CurrentPageIndex = e.PageIndex;
                this.BindGrid4();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : ddlType_SelectedIndexChanged
        * Purpose       : type 선택 시, ojt 항목을 선택 하면 subject, assess, textbook은 입력 불가토록 함 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlType.SelectedItem.Value == "000005")
                {
                    this.tabSubject.Enabled = false;
                    this.tabAssess.Enabled = false;
                    this.tabTextBook.Enabled = false;
                }
                else
                {
                    this.tabSubject.Enabled = true;
                    this.tabAssess.Enabled = true;
                    this.tabTextBook.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : tabctrl_Click
        * Purpose       : panel tab click 시 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void tabctrl_Click(object sender, EventArgs e)
        protected void tabctrl_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton link = (LinkButton)sender;
                if (link == null)
                {
                    link = this.tabCourse;
                }

                //new/edit 모드일 경우, course 정보가 저장되어 course_id가 부여되었을 때만 화면이동할수 있도록 한다. 
                //temp_save_flg = 'Y' 
                bool xMove = false;
                if (Request.QueryString["TEMP_FLG"] != null && Request.QueryString["TEMP_FLG"].ToString() == "Y")
                {
                    if (ViewState["COURSE_ID"] != null && ViewState["COURSE_ID"].ToString() != string.Empty)
                    {
                        string[] xParams = new string[1];
                        DataTable xDt = null;
                        xParams[0] = ViewState["COURSE_ID"].ToString();
                        xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                     "GetCourseInfoEdit",
                                                     LMS_SYSTEM.CURRICULUM,
                                                     "CLT.WEB.UI.LMS.CURR", (object)xParams);
                        if (xDt.Rows.Count > 0)
                        {
                            DataRow dr = xDt.Rows[0];
                            ViewState["TEMP_SAVE_FLG"] = dr["TEMP_SAVE_FLG"].ToString();
                            if (dr["TEMP_SAVE_FLG"].ToString() != "Y")
                            {
                                xMove = false;
                            }
                            else
                            {
                                xMove = true;
                            }
                        }
                        else
                        {
                            xMove = false;
                        }
                    }
                    else
                    {
                        xMove = false;
                    }
                }
                else
                {
                    xMove = true;
                }

                if (xMove == false)
                {
                    //course를 저장 하신 다음에 이동 하시기 바랍니다. 
                    link = this.tabCourse;
                }

                //course
                if (link.Text == this.tabCourse.Text)
                {
                    this.pnlCourse.Visible = true;
                    this.pnlSubject.Visible = false;
                    this.pnlAssess.Visible = false;
                    this.pnlTextBook.Visible = false;
                    pnlCourse.CssClass = "tab-content current";
                    pnlSubject.CssClass = "tab-content";
                    pnlAssess.CssClass = "tab-content";
                    pnlTextBook.CssClass = "tab-content";

                    tabCourse.Attributes["class"] = "current";
                    tabSubject.Attributes["class"] = "";
                    tabAssess.Attributes["class"] = "";
                    tabTextBook.Attributes["class"] = "";
                }

                else if (link.Text == this.tabSubject.Text)
                {
                    this.pnlCourse.Visible = false;
                    this.pnlSubject.Visible = true;
                    this.pnlAssess.Visible = false;
                    this.pnlTextBook.Visible = false;
                    pnlCourse.CssClass = "tab-content";
                    pnlSubject.CssClass = "tab-content current";
                    pnlAssess.CssClass = "tab-content";
                    pnlTextBook.CssClass = "tab-content";

                    tabCourse.Attributes["class"] = "";
                    tabSubject.Attributes["class"] = "current";
                    tabAssess.Attributes["class"] = "";
                    tabTextBook.Attributes["class"] = "";

                    this.BindGrid2();
                    this.BindGrid2c();
                }

                else if (link.Text == this.tabAssess.Text)
                {
                    this.pnlCourse.Visible = false;
                    this.pnlSubject.Visible = false;
                    this.pnlAssess.Visible = true;
                    this.pnlTextBook.Visible = false;
                    pnlCourse.CssClass = "tab-content";
                    pnlSubject.CssClass = "tab-content";
                    pnlAssess.CssClass = "tab-content current";
                    pnlTextBook.CssClass = "tab-content";

                    tabCourse.Attributes["class"] = "";
                    tabSubject.Attributes["class"] = "";
                    tabAssess.Attributes["class"] = "current";
                    tabTextBook.Attributes["class"] = "";

                    this.BindGrid3(); 
                }

                else if (link.Text == this.tabTextBook.Text)
                {
                    this.pnlCourse.Visible = false;
                    this.pnlSubject.Visible = false;
                    this.pnlAssess.Visible = false;
                    this.pnlTextBook.Visible = true;
                    pnlCourse.CssClass = "tab-content";
                    pnlSubject.CssClass = "tab-content";
                    pnlAssess.CssClass = "tab-content";
                    pnlTextBook.CssClass = "tab-content current";

                    tabCourse.Attributes["class"] = "";
                    tabSubject.Attributes["class"] = "";
                    tabAssess.Attributes["class"] = "";
                    tabTextBook.Attributes["class"] = "current";

                    this.BindGrid4(); 
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : LnkBtnSubjectAdd_Click
        * Purpose       : 선택된 subject 정보를 subject grid에 반영 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void LnkBtnSubjectAdd_Click(object sender, EventArgs e)
        protected void LnkBtnSubjectAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string[] items = this.HidSubjectAdd.Value.Split('凸');
                string[] xData = null;
                bool xSel = false;
                DataSet ds = new DataSet();

                DataTable dt = new DataTable();
                dt.Columns.Add("0", typeof(String));
                dt.Columns.Add("subject_lang", typeof(String));
                dt.Columns.Add("subject_kind", typeof(String));
                dt.Columns.Add("subject_nm_l", typeof(String));
                dt.Columns.Add("learning_time", typeof(String));
                dt.Columns.Add("instructor", typeof(String));
                dt.Columns.Add("use_flg", typeof(String));
                dt.Columns.Add("ins_dt", typeof(String));
                dt.Columns.Add("subject_id", typeof(String));
                dt.Columns.Add("subject_nm", typeof(String));
                dt.Columns.Add("10", typeof(Boolean));
                dt.Columns.Add("temp_save_flg", typeof(String));


                //기존에 추가되어 있는 데이터를 datatable에 추가 
                for (int i = 0; i < this.grd2.Items.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    //for (int k = 0; k < this.grd2.Items[i].Cells.Count-1; k++)
                    //{
                    //    dr[k] = this.grd2.Items[i].Cells[k].Text;
                    //}
                    dr[0] = this.grd2.Items[i].Cells[0].Text;
                    dr[1] = this.grd2.Items[i].Cells[1].Text;
                    dr[2] = this.grd2.Items[i].Cells[2].Text;
                    dr[3] = this.grd2.Items[i].Cells[9].Text;
                    dr[4] = this.grd2.Items[i].Cells[4].Text;
                    dr[5] = this.grd2.Items[i].Cells[5].Text;
                    dr[6] = this.grd2.Items[i].Cells[6].Text;
                    dr[7] = this.grd2.Items[i].Cells[7].Text;
                    dr[8] = this.grd2.Items[i].Cells[8].Text;
                    dr[9] = this.grd2.Items[i].Cells[9].Text;
                    //dr[3] = this.grd2.Items[i].Cells[9].Text;
                    dr[10] = false;
                    dr[11] = this.grd2.Items[i].Cells[11].Text;

                    dt.Rows.Add(dr);
                }

                //넘어온 아이템을 data table에 추가 
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null && items[i] != string.Empty)
                    {
                        xData = items[i].Split('|');
                        xSel = false;
                        for (int k = 0; k < this.grd2.Items.Count; k++)
                        {
                            if (xData[1].Trim() == this.grd2.Items[k].Cells[8].Text.Trim())
                            {
                                xSel = true;
                                break;
                            }
                        }
                        if (xSel == false)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = string.Empty;
                            dr[1] = string.Empty;
                            dr[2] = string.Empty;
                            dr[3] = xData[2].Trim();
                            dr[4] = string.Empty;
                            dr[5] = string.Empty;
                            dr[6] = string.Empty;
                            dr[7] = string.Empty;
                            dr[8] = xData[1].Trim();
                            dr[9] = xData[2].Trim();
                            dr[10] = false;
                            dr[11] = xData[3].Trim(); // string.Empty;
                            dt.Rows.Add(dr);
                            //this.grd2.Items.Count++; 
                            //this.grd2.Items[this.grd2.Items.Count].Cells.Add(); 
                        }
                    }
                }

                ds.Tables.Add(dt);
                this.grd2.DataSource = ds;
                this.grd2.DataBind();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }

        }
        #endregion

        /************************************************************
        * Function name : LnkBtnSubjectAdd_Click
        * Purpose       : 선택된 subject 정보를 subject grid에 반영 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void LnkBtnAssessAdd_Click(object sender, EventArgs e)
        protected void LnkBtnAssessAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string[] items = this.HidAssessAdd.Value.Split('凸');
                string[] xData = null;
                bool xSel = false;
                DataSet ds = new DataSet();

                DataTable dt = new DataTable();
                dt.Columns.Add("0", typeof(String));
                dt.Columns.Add("question_kind", typeof(String));
                dt.Columns.Add("course_group", typeof(String));
                dt.Columns.Add("course_field", typeof(String));
                dt.Columns.Add("question_content_l", typeof(String));
                dt.Columns.Add("question_type", typeof(String));
                dt.Columns.Add("ins_dt", typeof(String));
                dt.Columns.Add("question_id", typeof(String));
                dt.Columns.Add("question_content", typeof(String));
                dt.Columns.Add("9", typeof(Boolean));
                dt.Columns.Add("temp_save_flg", typeof(String));


                //기존에 추가되어 있는 데이터를 datatable에 추가 
                for (int i = 0; i < this.grd3.Items.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    //for (int k = 0; k < this.grd3.Items[i].Cells.Count - 1; k++)
                    //{
                    //    dr[k] = this.grd3.Items[i].Cells[k].Text;
                    //}

                    dr[0] = this.grd3.Items[i].Cells[0].Text;
                    dr[1] = this.grd3.Items[i].Cells[1].Text;
                    dr[2] = this.grd3.Items[i].Cells[2].Text;
                    dr[3] = this.grd3.Items[i].Cells[3].Text;
                    dr[4] = this.grd3.Items[i].Cells[8].Text;
                    dr[5] = this.grd3.Items[i].Cells[5].Text;
                    dr[6] = this.grd3.Items[i].Cells[6].Text;
                    dr[7] = this.grd3.Items[i].Cells[7].Text;
                    dr[8] = this.grd3.Items[i].Cells[8].Text;
                    dr[9] = false;
                    dr[10] = this.grd3.Items[i].Cells[10].Text;
                    dt.Rows.Add(dr);
                }

                //넘어온 아이템을 data table에 추가 
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null && items[i] != string.Empty)
                    {
                        xData = items[i].Split('|');
                        xSel = false;
                        for (int k = 0; k < this.grd3.Items.Count; k++)
                        {
                            if (xData[1].Trim() == this.grd3.Items[k].Cells[7].Text.Trim())
                            {
                                xSel = true;
                                break;
                            }
                        }
                        if (xSel == false)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = string.Empty;
                            dr[1] = string.Empty;
                            dr[2] = string.Empty;
                            dr[3] = string.Empty;
                            dr[4] = xData[2].Trim();
                            dr[5] = string.Empty;
                            dr[6] = string.Empty;
                            dr[7] = xData[1].Trim();
                            dr[8] = xData[2].Trim();
                            dr[9] = false;
                            dr[10] = xData[3].Trim(); // string.Empty;
                            dt.Rows.Add(dr);
                        }
                    }
                }

                ds.Tables.Add(dt);
                this.grd3.DataSource = ds;
                this.grd3.DataBind();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : LnkBtnSubjectAdd_Click
        * Purpose       : 선택된 subject 정보를 subject grid에 반영 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void LnkBtnTextbookAdd_Click(object sender, EventArgs e)
        protected void LnkBtnTextbookAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string[] items = this.HidTextbookAdd.Value.Split('凸');
                string[] xData = null;
                bool xSel = false;
                DataSet ds = new DataSet();

                DataTable dt = new DataTable();
                dt.Columns.Add("0", typeof(String));
                dt.Columns.Add("textbook_type", typeof(String));
                dt.Columns.Add("textbook_nm_l", typeof(String));
                dt.Columns.Add("author", typeof(String));
                dt.Columns.Add("publisher", typeof(String));
                dt.Columns.Add("textbook_lang", typeof(String));
                dt.Columns.Add("user_nm_kor", typeof(String));
                dt.Columns.Add("ins_dt", typeof(String));
                dt.Columns.Add("use_flg", typeof(String));
                dt.Columns.Add("textbook_id", typeof(String));
                dt.Columns.Add("textbook_nm", typeof(String));
                dt.Columns.Add("11", typeof(Boolean));
                dt.Columns.Add("temp_save_flg", typeof(String));


                //기존에 추가되어 있는 데이터를 datatable에 추가 
                for (int i = 0; i < this.grd4.Items.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    //for (int k = 0; k < this.grd4.Items[i].Cells.Count - 1; k++)
                    //{
                    //    dr[k] = this.grd4.Items[i].Cells[k].Text;
                    //}

                    dr[0] = this.grd4.Items[i].Cells[0].Text;
                    dr[1] = this.grd4.Items[i].Cells[1].Text;

                    dr[2] = this.grd4.Items[i].Cells[10].Text;

                    dr[3] = this.grd4.Items[i].Cells[3].Text;
                    dr[4] = this.grd4.Items[i].Cells[4].Text;
                    dr[5] = this.grd4.Items[i].Cells[5].Text;
                    dr[6] = this.grd4.Items[i].Cells[6].Text;
                    dr[7] = this.grd4.Items[i].Cells[7].Text;
                    dr[8] = this.grd4.Items[i].Cells[8].Text;
                    dr[9] = this.grd4.Items[i].Cells[9].Text;
                    dr[10] = this.grd4.Items[i].Cells[10].Text;
                    dr[11] = false;
                    dr[12] = this.grd4.Items[i].Cells[12].Text;

                    dt.Rows.Add(dr);
                }

                //넘어온 아이템을 data table에 추가 
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null && items[i] != string.Empty)
                    {
                        xData = items[i].Split('|');
                        xSel = false;
                        for (int k = 0; k < this.grd4.Items.Count; k++)
                        {
                            if (xData[1].Trim() == this.grd4.Items[k].Cells[9].Text.Trim())
                            {
                                xSel = true;
                                break;
                            }
                        }
                        if (xSel == false)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = string.Empty;
                            dr[1] = string.Empty;
                            dr[2] = xData[2].Trim();
                            dr[3] = string.Empty;
                            dr[4] = string.Empty;
                            dr[5] = string.Empty;
                            dr[6] = string.Empty;
                            dr[7] = string.Empty;
                            dr[8] = string.Empty;
                            dr[9] = xData[1].Trim();
                            dr[10] = xData[2].Trim();
                            dr[11] = false;
                            dr[12] = xData[3].Trim(); // string.Empty;
                            dt.Rows.Add(dr);

                        }
                    }
                }

                ds.Tables.Add(dt);
                this.grd4.DataSource = ds;
                this.grd4.DataBind();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

    }
}
