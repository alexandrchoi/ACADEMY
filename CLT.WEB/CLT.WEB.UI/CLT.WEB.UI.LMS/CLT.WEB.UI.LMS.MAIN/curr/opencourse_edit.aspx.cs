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

using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading; 

namespace CLT.WEB.UI.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : 과정개설 수정 및 선박송부 
    /// 
    /// 2. 주요기능 : 과정개설 수정 및 선박송부 
    ///				  
    /// 3. Class 명 : opencourse_edit
    /// 
    /// 4. 작 업 자 : 최인재 / 2012.01.04
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        * 서진한 2012.08.01
    ///        * Source
    ///          opencourse_edit
    ///        * Comment 
    ///          영문화 작업        
    ///    
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *  김은정 2012.08.08
    ///        * Source
    ///          opencourse_edit
    ///        * Comment 
    ///          과정 개설 시 입력 창 종료 후 과정 개설 리스트 조회       
    /// 
    ///    [CHM-201218484] 국토해양부 로고 추가 및 데이터 정렬 기준 추가
    ///    * 김은정 2012.09.14
    ///    * Source
    ///      opencourse_edit.aspx (BindData(), btnSend_Click())
    ///    * Comment 
    ///      국토해양부 과정여부 추가
    /// </summary>
    public partial class opencourse_edit : BasePage
    {
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
                    this.txtApplyBeginDt.Attributes.Add("onkeyup", "ChkDate(this);");
                    this.txtApplyEndDt.Attributes.Add("onkeyup", "ChkDate(this);");
                    this.txtBeginDt.Attributes.Add("onkeyup", "ChkDate(this);");
                    this.txtEndDt.Attributes.Add("onkeyup", "ChkDate(this);");
                    this.txtResBeginDt.Attributes.Add("onkeyup", "ChkDate(this);");
                    this.txtResEndDt.Attributes.Add("onkeyup", "ChkDate(this);");

                    this.BindDropDownList();
                }

                if (Request.QueryString["OPEN_COURSE_ID"] != null && Request.QueryString["OPEN_COURSE_ID"].ToString() != string.Empty)
                {
                    if (Request.QueryString["OPEN_COURSE_ID"].ToString() != "NEW")
                    {                        
                        if (!IsPostBack)
                        {
                            ViewState["OPEN_COURSE_ID"] = Request.QueryString["OPEN_COURSE_ID"].ToString();
                            this.BindData();
                        }
                    }
                    else
                    {
                        ViewState["OPEN_COURSE_ID"] = string.Empty;
                    }
                }

                base.pRender(this.Page, new object[,] { { this.btnSend, "E" }, { this.btnRewrite, "E" } }, Convert.ToString(Request.QueryString["MenuCode"]));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
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

                //year  
                WebControlHelper.SetYearDropDownList(this.ddlYear, WebControlHelper.ComboType.NullAble);

                //Institution 
                xParams[0] = "0007";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlInstitution, xDt,  WebControlHelper.ComboType.NullAble);

                //inout
                xParams[0] = "0005";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlInOut, xDt, WebControlHelper.ComboType.NullAble);

                //place
                xParams[0] = string.Empty;
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                 "GetCommonCodeInfo",
                                 LMS_SYSTEM.CURRICULUM,
                                 "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlPlace, xDt, WebControlHelper.ComboType.NullAble);


                //classification 
                xParams[0] = "0047";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                //WebControlHelper.SetDropDownList(this.ddlClassification, xDt);
                DataTable dtClass = new DataTable();
                DataColumn Colc1 = new DataColumn("check", Type.GetType("System.Boolean"));
                DataColumn Colc2 = new DataColumn("nm", Type.GetType("System.String"));
                DataColumn Colc3 = new DataColumn("id", Type.GetType("System.String"));

                dtClass.Columns.Add(Colc1);
                dtClass.Columns.Add(Colc2);
                dtClass.Columns.Add(Colc3);

                DataRow rows = null;
                foreach (DataRow dr in xDt.Rows)
                {
                    rows = dtClass.NewRow();
                    rows[0] = false;
                    rows[1] = dr["d_knm"].ToString();
                    rows[2] = dr["d_cd"].ToString();
                    dtClass.Rows.Add(rows);
                }

                this.dtlClassification.DataSource = dtClass;
                this.dtlClassification.DataBind();

                //Company accept(적용회사)
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_opencourse_md",
                                             "GetCompany",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", Thread.CurrentThread.CurrentCulture);

                DataTable dtComp = new DataTable();
                DataColumn Col1 = new DataColumn("check", Type.GetType("System.Boolean"));
                DataColumn Col2 = new DataColumn("nm", Type.GetType("System.String"));
                DataColumn Col3 = new DataColumn("id", Type.GetType("System.String"));

                dtComp.Columns.Add(Col1);
                dtComp.Columns.Add(Col2);
                dtComp.Columns.Add(Col3);

                rows = dtComp.NewRow();
                rows[0] = false;
                rows[1] = "ALL";
                rows[2] = "ALL";
                dtComp.Rows.Add(rows);

                foreach (DataRow dr in xDt.Rows)
                {
                    rows = dtComp.NewRow();
                    rows[0] = false;
                    rows[1] = dr["COMPANY_NM"].ToString();
                    rows[2] = dr["COMPANY_ID"].ToString();
                    dtComp.Rows.Add(rows);
                }

                this.dtlCompany.DataSource = dtComp;
                this.dtlCompany.DataBind();
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

                if (ViewState["OPEN_COURSE_ID"].ToString() == string.Empty)
                    return;

                xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_opencourse_md",
                                                 "GetOpencourseInfo",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);

                if (xDt != null && xDt.Rows.Count > 0)
                {
                    DataRow dr = xDt.Rows[0];

                    //VIEWSTATE에 입력되는 정보 저장 
                    ViewState["COURSE_ID"] = dr["COURSE_ID"].ToString();
                    ViewState["COURSE_YEAR"] = dr["COURSE_YEAR"].ToString();
                    ViewState["COURSE_SEQ"] = dr["COURSE_SEQ"].ToString();
                    ViewState["EDUCATIONAL_ORG"] = dr["EDUCATIONAL_ORG"].ToString();
                    ViewState["TRAINING_FEE"] = dr["TRAINING_FEE"].ToString();
                    ViewState["COURSE_TYPE"] = dr["COURSE_TYPE"].ToString();
                    ViewState["PASS_SCORE"] = dr["PASS_SCORE"].ToString();
                    ViewState["TRAINING_SUPPORT_FEE"] = dr["TRAINING_SUPPORT_FEE"].ToString();
                    ViewState["TRAINING_SUPPORT_COMP_FEE"] = dr["TRAINING_SUPPORT_COMP_FEE"].ToString();
                    ViewState["COURSE_BEGIN_APPLY_DT"] = dr["COURSE_BEGIN_APPLY_DT"].ToString();
                    ViewState["COURSE_END_APPLY_DT"] = dr["COURSE_END_APPLY_DT"].ToString();
                    ViewState["COURSE_BEGIN_DT"] = dr["COURSE_BEGIN_DT"].ToString();
                    ViewState["COURSE_END_DT"] = dr["COURSE_END_DT"].ToString();
                    ViewState["RES_BEGIN_DT"] = dr["RES_BEGIN_DT"].ToString();
                    ViewState["RES_END_DT"] = dr["RES_END_DT"].ToString();
                    ViewState["MIN_MAN_COUNT"] = dr["MIN_MAN_COUNT"].ToString();
                    ViewState["MAX_MAN_COUNT"] = dr["MAX_MAN_COUNT"].ToString();
                    ViewState["COMPANY_ACCEPT"] = dr["COMPANY_ACCEPT"].ToString();
                    ViewState["STD_PROGRESS_RATE"] = dr["STD_PROGRESS_RATE"].ToString();
                    ViewState["STD_FINAL_EXAM"] = dr["STD_FINAL_EXAM"].ToString();
                    //ViewState["STD_REPORT"] = dr["STD_REPORT"].ToString();  //현재는 사용하지 않는 필드 임 
                    ViewState["USE_FLG"] = dr["USE_FLG"].ToString();
                    ViewState["COURSE_GUBUN"] = dr["COURSE_GUBUN"].ToString();  // 국토해양부 과정 여부
                    ViewState["RES_NO"] = dr["RES_NO"].ToString();

                    ViewState["COURSE_INOUT"] = dr["COURSE_INOUT"].ToString();
                    ViewState["COURSE_PLACE"] = dr["COURSE_PLACE"].ToString();

                    this.txtCourseID.Value = dr["COURSE_ID"].ToString();
                    this.txtCourseNM.Value = dr["COURSE_NM"].ToString();

                    WebControlHelper.SetSelectItem_DropDownList(this.ddlYear, dr["COURSE_YEAR"].ToString());
                    this.txtSeq.Text = dr["COURSE_SEQ"].ToString();

                    WebControlHelper.SetSelectItem_DropDownList(this.ddlInstitution, dr["EDUCATIONAL_ORG"].ToString());

                    WebControlHelper.SetSelectItem_DropDownList(this.ddlInOut, dr["COURSE_INOUT"].ToString());

                    string[] xinout = new string[1];
                    DataTable dtInOut = null;
                    xinout[0] = string.Empty;
                    if (dr["COURSE_INOUT"].ToString() == "000001")
                    {
                        //사내 
                        xinout[0] = "0008";
                    }
                    else if (dr["COURSE_INOUT"].ToString() == "000002")
                    {
                        //사외
                        xinout[0] = "0009";
                    }

                    dtInOut = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                     "GetCommonCodeInfo",
                                     LMS_SYSTEM.CURRICULUM,
                                     "CLT.WEB.UI.LMS.CURR", (object)xinout, Thread.CurrentThread.CurrentCulture);
                    WebControlHelper.SetDropDownList(this.ddlPlace, dtInOut,  WebControlHelper.ComboType.NullAble);

                    WebControlHelper.SetSelectItem_DropDownList(this.ddlPlace, dr["COURSE_PLACE"].ToString());
                    //this.txtPlace.Text = dr["COURSE_PLACE"].ToString();

                    this.txtEduFee.Text = dr["TRAINING_FEE"].ToString();
                    this.txtVat.Text = string.Format("{0:##0.0}", Convert.ToInt32(dr["TRAINING_FEE"].ToString() == string.Empty ? "0" : dr["TRAINING_FEE"].ToString()) * 0.1);

                    //WebControlHelper.SetSelectItem_DropDownList(this.ddlClassification, dr["COURSE_TYPE"].ToString());
                    string[] xClass = dr["COURSE_TYPE"].ToString().Split('|');
                    for (int k = 0; k < this.dtlClassification.Items.Count; k++)
                    {
                        ((CheckBox)this.dtlClassification.Items[k].FindControl("chkClass")).Checked = false;
                    }
                    for (int i = 0; i < xClass.GetLength(0); i++)
                    {
                        for (int k = 0; k < this.dtlClassification.Items.Count; k++)
                        {
                            if (xClass[i] == ((Label)this.dtlClassification.Items[k].FindControl("lblClassId")).Text)
                            {
                                ((CheckBox)this.dtlClassification.Items[k].FindControl("chkClass")).Checked = true;
                            }
                        }
                    }


                    this.txtScore.Text = dr["PASS_SCORE"].ToString();

                    this.txtSupportFee.Text = dr["TRAINING_SUPPORT_FEE"].ToString();
                    this.txtSupportCompFee.Text = dr["TRAINING_SUPPORT_COMP_FEE"].ToString();

                    this.txtApplyBeginDt.Text = dr["COURSE_BEGIN_APPLY_DT"].ToString();
                    this.txtApplyEndDt.Text = dr["COURSE_END_APPLY_DT"].ToString();

                    this.txtBeginDt.Text = dr["COURSE_BEGIN_DT"].ToString();
                    this.txtEndDt.Text = dr["COURSE_END_DT"].ToString();

                    this.txtResBeginDt.Text = dr["RES_BEGIN_DT"].ToString();
                    this.txtResEndDt.Text = dr["RES_END_DT"].ToString();

                    this.txtMinCount.Text = dr["MIN_MAN_COUNT"].ToString();
                    this.txtMaxCount.Text = dr["MAX_MAN_COUNT"].ToString();

                    string[] xComp = dr["COMPANY_ACCEPT"].ToString().Split('|');
                    //초기화 후 
                    for (int k = 0; k < this.dtlCompany.Items.Count; k++)
                    {
                        ((CheckBox)this.dtlCompany.Items[k].FindControl("chkCompany")).Checked = false;
                    }

                    for (int i = 0; i < xComp.GetLength(0); i++)
                    {
                        for (int k = 0; k < this.dtlCompany.Items.Count; k++)
                        {
                            if (xComp[i] == ((Label)this.dtlCompany.Items[k].FindControl("lblCompanyId")).Text)
                            {
                                ((CheckBox)this.dtlCompany.Items[k].FindControl("chkCompany")).Checked = true;
                            }
                        }
                    }

                    this.txtProgressRate.Text = dr["STD_PROGRESS_RATE"].ToString();
                    this.txtFinalTest.Text = dr["STD_FINAL_EXAM"].ToString();
                    this.txtTotalScore.Text = Convert.ToString(Convert.ToInt32(dr["STD_PROGRESS_RATE"].ToString() == string.Empty ? "0" : dr["STD_PROGRESS_RATE"].ToString())
                                                + Convert.ToInt32(dr["STD_FINAL_EXAM"].ToString() == string.Empty ? "0" : dr["STD_FINAL_EXAM"].ToString()));


                    this.rdoUsage.SelectedValue = dr["USE_FLG"].ToString();
                    this.rdoCourseGubun.SelectedValue = dr["COURSE_GUBUN"].ToString();   // 국토해양부 과정여부

                    //if (dr["USE_FLG"].ToString() == "Y")
                    //    this.rdoUsage.Items[0].Selected = true;
                    //else
                    //    this.rdoUsage.Items[1].Selected = true;


                    this.txtResNm.Value = dr["RES_SUB"].ToString();
                    this.txtResId.Value = dr["RES_NO"].ToString();

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
       * Function name : CreateString_Company_Accept
       * Purpose       : company accept string creation 하는 부분 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region private string CreateString_Company_Accept()
        private string CreateString_Company_Accept()
        {

            string xReturn = string.Empty;
            try
            {
                for (int i = 0; i < this.dtlCompany.Items.Count; i++)
                {
                    if (((CheckBox)this.dtlCompany.Items[i].FindControl("chkCompany")).Checked == true)
                    {
                        if (((Label)this.dtlCompany.Items[i].FindControl("lblCompanyId")).Text == "ALL")
                        {
                            xReturn = "ALL";
                            break;
                        }
                        xReturn += ((Label)this.dtlCompany.Items[i].FindControl("lblCompanyId")).Text + "|";
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xReturn;
        }
        #endregion

        /******************************************************************************************
       * Function name : CreateString_Classification
       * Purpose       : 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region private string CreateString_Classification()
        private string CreateString_Classification()
        {
            string xReturn = string.Empty;
            try
            {
                for (int i = 0; i < this.dtlClassification.Items.Count; i++)
                {
                    if (((CheckBox)this.dtlClassification.Items[i].FindControl("chkClass")).Checked == true)
                    {
                        xReturn += ((Label)this.dtlClassification.Items[i].FindControl("lblClassId")).Text + "|";
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xReturn;
        }
        #endregion

        /******************************************************************************************
       * Function name : IsDataChanged
       * Purpose       : 데이터 변경이 발생했는지 여부를 체크하는 처리
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region private bool IsDataChanged()
        private bool IsDataChanged()
        {
            try
            {
                //ViewState["OPEN_COURSE_ID"].ToString()
                //ViewState["COURSE_ID"] = dr["COURSE_ID"].ToString();
                //ViewState["COURSE_YEAR"] = dr["COURSE_YEAR"].ToString();
                //ViewState["COURSE_SEQ"] = dr["COURSE_SEQ"].ToString();
                //ViewState["EDUCATIONAL_ORG"] = dr["EDUCATIONAL_ORG"].ToString();
                //ViewState["TRAINING_FEE"] = dr["TRAINING_FEE"].ToString();
                //ViewState["COURSE_TYPE"] = dr["COURSE_TYPE"].ToString();
                //ViewState["PASS_SCORE"] = dr["PASS_SCORE"].ToString();
                //ViewState["TRAINING_SUPPORT_FEE"] = dr["TRAINING_SUPPORT_FEE"].ToString();
                //ViewState["TRAINING_SUPPORT_COMP_FEE"] = dr["TRAINING_SUPPORT_COMP_FEE"].ToString();
                //ViewState["COURSE_BEGIN_APPLY_DT"] = dr["COURSE_BEGIN_APPLY_DT"].ToString();
                //ViewState["COURSE_END_APPLY_DT"] = dr["COURSE_END_APPLY_DT"].ToString();
                //ViewState["COURSE_BEGIN_DT"] = dr["COURSE_BEGIN_DT"].ToString();
                //ViewState["COURSE_END_DT"] = dr["COURSE_END_DT"].ToString();
                //ViewState["RES_BEGIN_DT"] = dr["RES_BEGIN_DT"].ToString();
                //ViewState["RES_END_DT"] = dr["RES_END_DT"].ToString();
                //ViewState["MIN_MAN_COUNT"] = dr["MIN_MAN_COUNT"].ToString();
                //ViewState["MAX_MAN_COUNT"] = dr["MAX_MAN_COUNT"].ToString();
                //ViewState["COMPANY_ACCEPT"] = dr["COMPANY_ACCEPT"].ToString();
                //ViewState["STD_PROGRESS_RATE"] = dr["STD_PROGRESS_RATE"].ToString();
                //ViewState["STD_FINAL_EXAM"] = dr["STD_FINAL_EXAM"].ToString();
                ////ViewState["STD_REPORT"] = dr["STD_REPORT"].ToString();  //현재는 사용하지 않는 필드 임 
                //ViewState["USE_FLG"] = dr["USE_FLG"].ToString();

                if (ViewState["OPEN_COURSE_ID"].ToString() == "NEW")
                    return true;

                else if (ViewState["OPEN_COURSE_ID"].ToString() == string.Empty)
                    return true;

                else if (ViewState["COURSE_ID"].ToString() != this.txtCourseID.Value)
                    return true;
                else if (ViewState["COURSE_YEAR"].ToString() != this.ddlYear.Text.Replace("*", string.Empty))
                    return true;
                //else if (ViewState["COURSE_SEQ"].ToString() != this.txtSeq.Text)
                //    return true;
                else if (ViewState["EDUCATIONAL_ORG"].ToString() != this.ddlInstitution.SelectedItem.Value.Replace("*", string.Empty))
                    return true;
                else if (ViewState["COURSE_INOUT"].ToString() != this.ddlInOut.SelectedItem.Value.Replace("*", string.Empty))
                    return true;
                else if (this.ddlPlace.SelectedItem != null && ViewState["COURSE_PLACE"].ToString() != this.ddlPlace.SelectedItem.Value.Replace("*", string.Empty))
                    return true;
                else if (ViewState["TRAINING_FEE"].ToString() != this.txtEduFee.Text)
                    return true;
                //else if (ViewState["COURSE_TYPE"].ToString() != this.ddlClassification.SelectedItem.Value.Replace("*", string.Empty))
                else if (ViewState["COURSE_TYPE"].ToString() != this.CreateString_Classification())
                    return true;
                else if (ViewState["PASS_SCORE"].ToString() != this.txtScore.Text)
                    return true;
                else if (ViewState["TRAINING_SUPPORT_FEE"].ToString() != this.txtSupportFee.Text)
                    return true;
                else if (ViewState["TRAINING_SUPPORT_COMP_FEE"].ToString() != this.txtSupportCompFee.Text)
                    return true;
                else if (ViewState["COURSE_BEGIN_APPLY_DT"].ToString() != this.txtApplyBeginDt.Text)
                    return true;
                else if (ViewState["COURSE_END_APPLY_DT"].ToString() != this.txtApplyEndDt.Text)
                    return true;
                else if (ViewState["COURSE_BEGIN_DT"].ToString() != this.txtBeginDt.Text)
                    return true;
                else if (ViewState["COURSE_END_DT"].ToString() != this.txtEndDt.Text)
                    return true;
                else if (ViewState["RES_BEGIN_DT"].ToString() != this.txtResBeginDt.Text)
                    return true;
                else if (ViewState["RES_END_DT"].ToString() != this.txtResEndDt.Text)
                    return true;

                else if (ViewState["MIN_MAN_COUNT"].ToString() != this.txtMinCount.Text)
                    return true;
                else if (ViewState["MAX_MAN_COUNT"].ToString() != this.txtMaxCount.Text)
                    return true;

                else if (ViewState["COMPANY_ACCEPT"].ToString() != this.CreateString_Company_Accept())
                    return true;

                else if (ViewState["STD_PROGRESS_RATE"].ToString() != this.txtProgressRate.Text)
                    return true;
                else if (ViewState["STD_FINAL_EXAM"].ToString() != this.txtFinalTest.Text)
                    return true;

                else if (ViewState["USE_FLG"].ToString() != this.rdoUsage.SelectedValue)
                    return true;

                else if (ViewState["COURSE_GUBUN"].ToString() != this.rdoCourseGubun.SelectedValue)
                    return true;

                else if (ViewState["RES_NO"].ToString() != this.txtResId.Value)
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return false;
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
                if (this.txtCourseID.Value == string.Empty)
                    return false;
                //if (this.ddlYear.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                //    return false;
                //if (this.txtSeq.Text == string.Empty)
                //    return false;
                if (this.ddlInstitution.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.txtScore.Text == string.Empty)
                    return false;
                if (this.txtApplyBeginDt.Text == string.Empty)
                    return false;
                if (this.txtApplyEndDt.Text == string.Empty)
                    return false;
                if (this.txtBeginDt.Text == string.Empty)
                    return false;
                if (this.txtEndDt.Text == string.Empty)
                    return false;
                if (this.txtProgressRate.Text == string.Empty)
                    return false;
                if (this.txtFinalTest.Text == string.Empty)
                    return false;
                if (this.rdoUsage.SelectedIndex < 0)
                    return false;

                if (this.rdoCourseGubun.SelectedIndex < 0) //  국토해양부 과정여부 체크
                    return false;

                //100보다 크면 저장 안되게 
                int xpro = Convert.ToInt32(this.txtProgressRate.Text == string.Empty ? "0" : this.txtProgressRate.Text);
                int xfinal = Convert.ToInt32(this.txtFinalTest.Text == string.Empty ? "0" : this.txtFinalTest.Text);
                if (xpro + xfinal > 100)
                {
                    return false; 
                }

                int xCnt = 0;
                for (int i = 0; i < this.dtlClassification.Items.Count; i++)
                {
                    if (((CheckBox)this.dtlClassification.Items[i].FindControl("chkClass")).Checked == true)
                    {
                        xCnt++;
                    }
                }

                if (xCnt == 0)
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003",
                                                       new string[] { "교육구분" },
                                                       new string[] { "Course Classification" },
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
        #endregion

        /******************************************************************************************
       * Function name : btnSend_Click
       * Purpose       : 과정개설 신규저장 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region protected void btnSend_Click(object sender, EventArgs e)
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.IsDataChanged())
                {
                    if (this.IsDataValidation())
                    {
                        string[] xParams = new string[27];

                        xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                        xParams[1] = this.txtCourseID.Value;
                        xParams[2] = string.Empty; // this.ddlYear.SelectedItem.Value;
                        if (this.txtBeginDt.Text != string.Empty)
                            xParams[2] = this.txtBeginDt.Text.Substring(0, 4);
                        else
                            xParams[2] = this.ddlYear.SelectedItem.Value;

                        xParams[3] = string.Empty; // this.txtSeq.Text;
                        xParams[4] = this.ddlInstitution.SelectedItem.Value;
                        xParams[5] = this.txtEduFee.Text;
                        xParams[6] = this.CreateString_Classification(); // this.ddlClassification.SelectedItem.Value.Replace("*", string.Empty);
                        xParams[7] = this.txtScore.Text;
                        xParams[8] = this.txtSupportFee.Text;
                        xParams[9] = this.txtSupportCompFee.Text;
                        xParams[10] = this.txtApplyBeginDt.Text;
                        xParams[11] = this.txtApplyEndDt.Text;
                        xParams[12] = this.txtBeginDt.Text;
                        xParams[13] = this.txtEndDt.Text;
                        xParams[14] = this.txtResBeginDt.Text;
                        xParams[15] = this.txtResEndDt.Text;
                        xParams[16] = this.txtMinCount.Text;
                        xParams[17] = this.txtMaxCount.Text;
                        xParams[18] = this.CreateString_Company_Accept();
                        xParams[19] = this.txtProgressRate.Text;
                        xParams[20] = this.txtFinalTest.Text;
                        xParams[21] = this.rdoUsage.SelectedValue;
                        xParams[22] = Session["USER_ID"].ToString();
                        xParams[23] = this.txtResId.Value;
                        xParams[24] = this.ddlInOut.SelectedItem.Value.Replace("*", string.Empty);
                        xParams[25] = this.ddlPlace.SelectedItem.Value.Replace("*", string.Empty);
                        xParams[26] = this.rdoCourseGubun.SelectedValue;   //국토해양부 과정 여부

                        string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_opencourse_md",
                                                        "SetOpencourseInfo",
                                                        LMS_SYSTEM.CURRICULUM,
                                                        "CLT.WEB.UI.LMS.CURR", (object)xParams);

                        if (xRtn != string.Empty)
                        {
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                          new string[] { "개설과정" },
                                                          new string[] { "Open Course" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));

                            //저장 후 신규 id 값으로 재조회 
                            ViewState["OPEN_COURSE_ID"] = xRtn;

                            // 저장 후 화면 종료 -> 과정 개설 리스트 재조회
                            ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>OK();</script>");
                            //this.BindData();
                        }
                        else
                        {
                            //{0}이(가) 입력되지 않았습니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004",
                                                          new string[] { "개설과정" },
                                                          new string[] { "Open Course" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));
                        }
                    }
                    else
                    {
                        //{0}의 필수 항목 입력이 누락되었습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                          new string[] { "개설과정" },
                                                          new string[] { "Open Course" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));
                    }
                }
                else
                {
                    //변경내용을 재 확인 바랍니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A023",
                                                          new string[] { "개설과정" },
                                                          new string[] { "Open Course" },
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

        /******************************************************************************************
       * Function name : btnRewrite_Click
       * Purpose       : 신규 작성 하기 위해 control 및 viewstate 모두 clear 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region protected void btnRewrite_Click(object sender, EventArgs e)
        protected void btnRewrite_Click(object sender, EventArgs e)
        {
            try
            {
                //view clear
                ViewState["OPEN_COURSE_ID"] = string.Empty;
                ViewState["COURSE_ID"] = string.Empty;
                ViewState["COURSE_YEAR"] = string.Empty;
                ViewState["COURSE_SEQ"] = string.Empty;
                ViewState["EDUCATIONAL_ORG"] = string.Empty;
                ViewState["TRAINING_FEE"] = string.Empty;
                ViewState["COURSE_TYPE"] = string.Empty;
                ViewState["PASS_SCORE"] = string.Empty;
                ViewState["TRAINING_SUPPORT_FEE"] = string.Empty;
                ViewState["TRAINING_SUPPORT_COMP_FEE"] = string.Empty;
                ViewState["COURSE_BEGIN_APPLY_DT"] = string.Empty;
                ViewState["COURSE_END_APPLY_DT"] = string.Empty;
                ViewState["COURSE_BEGIN_DT"] = string.Empty;
                ViewState["COURSE_END_DT"] = string.Empty;
                ViewState["RES_BEGIN_DT"] = string.Empty;
                ViewState["RES_END_DT"] = string.Empty;
                ViewState["MIN_MAN_COUNT"] = string.Empty;
                ViewState["MAX_MAN_COUNT"] = string.Empty;
                ViewState["COMPANY_ACCEPT"] = string.Empty;
                ViewState["STD_PROGRESS_RATE"] = string.Empty;
                ViewState["STD_FINAL_EXAM"] = string.Empty;
                ViewState["USE_FLG"] = string.Empty;
                ViewState["RES_NO"] = string.Empty;
                ViewState["COURSE_INOUT"] = string.Empty;
                ViewState["COURSE_PLACE"] = string.Empty;

                //control clear 
                this.txtCourseID.Value = string.Empty;
                this.txtCourseNM.Value = string.Empty;

                this.ddlYear.SelectedIndex = 0;
                this.txtSeq.Text = string.Empty;
                this.ddlInstitution.SelectedIndex = 0;
                this.ddlInOut.SelectedIndex = 0;
                this.ddlPlace.SelectedIndex = 0;
                //this.txtPlace.Text = string.Empty;


                this.txtEduFee.Text = string.Empty;
                this.txtVat.Text = string.Empty;
                //this.ddlClassification.SelectedIndex = 0;
                for (int k = 0; k < this.dtlClassification.Items.Count; k++)
                {
                    ((CheckBox)this.dtlClassification.Items[k].FindControl("chkClass")).Checked = false;
                }


                this.txtScore.Text = string.Empty;
                this.txtSupportFee.Text = string.Empty;
                this.txtSupportCompFee.Text = string.Empty;

                this.txtApplyBeginDt.Text = string.Empty;
                this.txtApplyEndDt.Text = string.Empty;
                this.txtBeginDt.Text = string.Empty;
                this.txtEndDt.Text = string.Empty;
                this.txtResBeginDt.Text = string.Empty;
                this.txtResEndDt.Text = string.Empty;

                this.txtMinCount.Text = string.Empty;
                this.txtMaxCount.Text = string.Empty;

                //초기화 후 
                for (int k = 0; k < this.dtlCompany.Items.Count; k++)
                {
                    ((CheckBox)this.dtlCompany.Items[k].FindControl("chkCompany")).Checked = false;
                }

                this.txtProgressRate.Text = string.Empty;
                this.txtFinalTest.Text = string.Empty;
                this.txtTotalScore.Text = string.Empty;

                if(this.rdoUsage.SelectedItem != null )
                    this.rdoUsage.SelectedItem.Selected = false;

                this.txtResId.Value = string.Empty;
                this.txtResNm.Value = string.Empty;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion



        /******************************************************************************************
       * Function name : txtProgressRate_TextChanged
       * Purpose       : total 하기 (progressRate + final test = total score)
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region protected void txtProgressRate_TextChanged(object sender, EventArgs e)
        protected void txtProgressRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int xpro = Convert.ToInt32(this.txtProgressRate.Text == string.Empty ? "0" : this.txtProgressRate.Text);
                int xfinal = Convert.ToInt32(this.txtFinalTest.Text == string.Empty ? "0" : this.txtFinalTest.Text);
                if (xpro + xfinal > 100)
                {
                    //A030  : <KO>{0}이(가) {1}보다 큽니다.<
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A030",
                                                      new string[] { this.lblTotalScore.Text, "100"},
                                                      new string[] { this.lblTotalScore.Text, "100"},
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                }
                else
                {
                    this.txtTotalScore.Text = Convert.ToString(xpro + xfinal);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /******************************************************************************************
       * Function name : txtEduFee_TextChanged
       * Purpose       : education fee 입력 시, vat 자동 계산 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region protected void txtEduFee_TextChanged(object sender, EventArgs e)
        protected void txtEduFee_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtVat.Text = string.Format("{0:##0.0}", Convert.ToInt32(this.txtEduFee.Text == string.Empty ? "0" : this.txtEduFee.Text) * 0.1);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /******************************************************************************************
       * Function name : ddlInOut_SelectIndexChange
       * Purpose       : 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        protected void ddlInOut_SelectIndexChange(object sender, EventArgs e)
        {
            try
            {
                string xInOut = this.ddlInOut.SelectedItem.Value;

                string[] xParams = new string[1];
                DataTable xDt = null;

                xParams[0] = string.Empty;

                if (xInOut == "000001")
                {
                    //사내 
                    xParams[0] = "0008";
                }
                else if (xInOut == "000002")
                {
                    //사외
                    xParams[0] = "0009";
                }

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                 "GetCommonCodeInfo",
                                 LMS_SYSTEM.CURRICULUM,
                                 "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlPlace, xDt,  WebControlHelper.ComboType.NullAble);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }

        }








    }
}
