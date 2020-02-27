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
    /// 1. 작업개요 : subject_edit Class
    /// 
    /// 2. 주요기능 : 과목 수정 및 선박 송부 
    ///				  
    /// 3. Class 명 : subject_edit
    /// 
    /// 4. 작 업 자 : 최인재 / 2012.01.16
    /// </summary>
    public partial class subject_edit : BasePage
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
                    this.BindDropDownList();
                    this.ViewStateClear();
                }

                if (Request.QueryString["SUBJECT_ID"] != null && Request.QueryString["SUBJECT_ID"].ToString() != "")
                {                    
                    if (!IsPostBack)
                    {
                        ViewState["SUBJECT_ID"] = Request.QueryString["SUBJECT_ID"].ToString();
                        this.BindData();
                    }
                }

                if (Request.QueryString["TEMP_FLG"] != null && Request.QueryString["TEMP_FLG"].ToString() == "Y")
                {
                    this.btnSend.Visible = false;
                    this.btnTemp.Visible = true;
                }
                else
                {
                    this.btnSend.Visible = true;
                    this.btnTemp.Visible = false;
                }

                base.pRender(this.Page, new object[,] { { this.btnSend, "E" }, { this.btnTemp, "E" } }, Convert.ToString(Request.QueryString["MenuCode"]));

            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : ViewStateClear
        * Purpose       : view state 초기화 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void ViewStateClear()
        private void ViewStateClear()
        {
            try
            {
                ViewState["SUBJECT_ID"] = string.Empty;
                ViewState["SUBJECT_NM"] = string.Empty;
                ViewState["SUBJECT_KIND"] = string.Empty;
                ViewState["SUBJECT_TYPE"] = string.Empty;
                ViewState["SUBJECT_LANG"] = string.Empty;
                ViewState["LEARNING_TIME"] = string.Empty;
                ViewState["LECTURER_NM"] = string.Empty;
                ViewState["LEARNING_DESC"] = string.Empty;
                ViewState["LEARNING_OBJECTIVE"] = string.Empty;
                ViewState["USE_FLG"] = string.Empty;
                ViewState["TEMP_SAVE_FLG"] = string.Empty;
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

                //classification 
                xParams[0] = "0042";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlClass, xDt,  WebControlHelper.ComboType.NullAble);

                // type
                xParams[0] = "0006";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlType, xDt, WebControlHelper.ComboType.NullAble);

                // Lang
                xParams[0] = "0017";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlLang, xDt, WebControlHelper.ComboType.NullAble);
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

                xParams[0] = ViewState["SUBJECT_ID"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_subject_md",
                                                 "GetSubjectInfo",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);

                if (xDt != null && xDt.Rows.Count > 0)
                {
                    DataRow dr = xDt.Rows[0];

                    //VIEWSTATE에 입력되는 정보 저장 
                    ViewState["SUBJECT_NM"] = dr["SUBJECT_NM"].ToString();
                    ViewState["SUBJECT_KIND"] = dr["SUBJECT_KIND"].ToString();
                    ViewState["SUBJECT_TYPE"] = dr["SUBJECT_TYPE"].ToString();
                    ViewState["SUBJECT_LANG"] = dr["SUBJECT_LANG"].ToString();
                    ViewState["LEARNING_TIME"] = dr["LEARNING_TIME"].ToString();
                    ViewState["LECTURER_NM"] = dr["LECTURER_NM"].ToString();
                    ViewState["LEARNING_DESC"] = dr["LEARNING_DESC"].ToString();
                    ViewState["LEARNING_OBJECTIVE"] = dr["LEARNING_OBJECTIVE"].ToString();
                    ViewState["USE_FLG"] = dr["USE_FLG"].ToString();
                    ViewState["TEMP_SAVE_FLG"] = dr["TEMP_SAVE_FLG"].ToString();

                    this.txtSubject.Text = dr["SUBJECT_NM"].ToString();
                    //data binding 
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlClass, dr["SUBJECT_KIND"].ToString()); //classification
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlType, dr["SUBJECT_TYPE"].ToString()); //type
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlLang, dr["SUBJECT_LANG"].ToString()); //language                 

                    this.txtTime.Text = dr["LEARNING_TIME"].ToString();
                    this.txtInstructor.Text = dr["LECTURER_NM"].ToString();
                    this.txtInstructor1.Text = dr["LECTURER1_NM"].ToString();
                    this.txtContents.Text = dr["LEARNING_DESC"].ToString();
                    this.txtPoint.Text = dr["LEARNING_OBJECTIVE"].ToString();

                    if (dr["USE_FLG"].ToString() == "Y")
                        this.rdoUsage.Items[0].Selected = true;
                    else
                        this.rdoUsage.Items[1].Selected = true;
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
                //ViewState["SUBJECT_ID"] = Request.QueryString["SUBJECT_ID"].ToString();
                //ViewState["SUBJECT_NM"] = dr["SUBJECT_NM"].ToString();
                //ViewState["SUBJECT_KIND"] = dr["SUBJECT_KIND"].ToString();
                //ViewState["SUBJECT_TYPE"] = dr["SUBJECT_TYPE"].ToString();
                //ViewState["SUBJECT_LANG"] = dr["SUBJECT_LANG"].ToString();
                //ViewState["LEARNING_TIME"] = dr["LEARNING_TIME"].ToString();
                //ViewState["LECTURER_NM"] = dr["LECTURER_NM"].ToString();
                //ViewState["LEARNING_DESC"] = dr["LEARNING_DESC"].ToString();
                //ViewState["LEARNING_OBJECTIVE"] = dr["LEARNING_OBJECTIVE"].ToString();
                //ViewState["USE_FLG"] = dr["USE_FLG"].ToString();
                //ViewState["TEMP_SAVE_FLG"] = dr["TEMP_SAVE_FLG"].ToString();

                if (ViewState["SUBJECT_NM"].ToString() != this.txtSubject.Text)
                    return true;
                else if (ViewState["SUBJECT_KIND"].ToString() != this.ddlClass.SelectedItem.Value)
                    return true;
                else if (ViewState["SUBJECT_TYPE"].ToString() != this.ddlType.SelectedItem.Value)
                    return true;
                else if (ViewState["SUBJECT_LANG"].ToString() != this.ddlLang.SelectedItem.Value)
                    return true;
                else if (ViewState["LEARNING_TIME"].ToString() != this.txtTime.Text)
                    return true;
                else if (ViewState["LECTURER_NM"].ToString() != this.txtInstructor.Text)
                    return true;
                else if (ViewState["LECTURER1_NM"].ToString() != this.txtInstructor1.Text)
                    return true;
                else if (ViewState["LEARNING_DESC"].ToString() != this.txtContents.Text)
                    return true;
                else if (ViewState["LEARNING_OBJECTIVE"].ToString() != this.txtPoint.Text)
                    return true;
                else if (ViewState["USE_FLG"].ToString() != this.rdoUsage.SelectedValue)
                    return true;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
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
                if (this.ddlClass.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.ddlType.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.ddlLang.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.rdoUsage.SelectedIndex < 0)
                    return false;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }

            return true;
        }
        #endregion

        /************************************************************
      * Function name : btnSend_Click
      * Purpose       : Send버튼 클릭될 때 처리
                        어떤 항목이라도 변경이 발생되면, 신규 생성하여 선박으로 전송 처리
      * Input         : void
      * Output        : void
      *************************************************************/
        #region protected void btnSend_Click(object sender, EventArgs e)
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.IsDataChanged())
                {
                    if (this.IsDataValidation())
                    {
                        string[] xParams = new string[13];

                        xParams[0] = ViewState["SUBJECT_ID"].ToString();
                        xParams[1] = this.txtSubject.Text;
                        xParams[2] = this.ddlClass.SelectedItem.Value;
                        xParams[3] = this.ddlType.SelectedItem.Value;
                        xParams[4] = this.ddlLang.SelectedItem.Value;
                        xParams[5] = this.txtTime.Text;
                        xParams[6] = this.txtInstructor.Text;
                        xParams[7] = this.txtContents.Text;
                        xParams[8] = this.txtPoint.Text;
                        xParams[9] = this.rdoUsage.SelectedValue;
                        xParams[10] = (Request.QueryString["TEMP_FLG"] != null && Request.QueryString["TEMP_FLG"].ToString() == "Y") ? "Y" : "N";
                        xParams[11] = Session["USER_ID"].ToString();
                        xParams[12] = this.txtInstructor1.Text;

                        string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_subject_md",
                                                        "SetSubjectInfo",
                                                        LMS_SYSTEM.CURRICULUM,
                                                        "CLT.WEB.UI.LMS.CURR", (object)xParams);

                        if (xRtn != string.Empty)
                        {
                            //A001: {0}이(가) 저장되었습니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                              new string[] { "과목" },
                                                              new string[] { "Subject" },
                                                              Thread.CurrentThread.CurrentCulture
                                                             ));
                            //저장 후 신규 id 값으로 재조회 
                            ViewState["SUBJECT_ID"] = xRtn;
                            this.BindData();
                        }
                        else
                        {
                            //A004: {0}이(가) 입력되지 않았습니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004",
                                                              new string[] { "과목" },
                                                              new string[] { "Subject" },
                                                              Thread.CurrentThread.CurrentCulture
                                                             ));

                        }
                    }
                    else
                    {
                        //A012: {0}의 필수 항목 입력이 누락되었습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                          new string[] { "과목" },
                                                          new string[] { "Subject" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));
                    }
                }
                else
                {
                    //A023: 변경내용을 재 확인 바랍니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A023",
                                                      new string[] { null },
                                                      new string[] { null },
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


    }
}
