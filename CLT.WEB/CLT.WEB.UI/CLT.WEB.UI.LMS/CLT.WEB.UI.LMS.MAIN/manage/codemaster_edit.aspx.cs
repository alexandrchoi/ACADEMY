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

namespace CLT.WEB.UI.LMS.MANAGE
{
    /// <summary>
    /// 1. 작업개요 : (Master)공통코드 생성, 수정 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 Master 공통코드 생성, 수정 관리

    ///				  
    /// 3. Class 명 : codemaster_new
    /// 
    /// 4. 작 업 자 : 김민규 / 2011.12.26
    /// </summary>
    public partial class codemaster_edit : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 웹페이지 Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "codemaster_edit", xScriptMsg);

                    return;
                }

                string xMasterCode = string.Empty;  // 다른 페이지로 부터 넘겨받은 MasterCode 용 변수

                if (Request.QueryString["openerEditMode"] != null)
                {
                    if (Request.QueryString["openerEditMode"].ToString() == "m_cd_new")   // MasterCode 신규등록일 경우...
                    {
                        string[] xTempParams = new string[5];
                        string xCompanyID = string.Empty;

                        // Master Code 처리
                        xTempParams[0] = "t_code_master";
                        xTempParams[1] = "m_cd";
                        xTempParams[2] = "0";
                        xTempParams[3] = "4";
                        xTempParams[4] = "";


                        xCompanyID = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                         "GetMaxIDOfCode",
                                                         LMS_SYSTEM.MANAGE,
                                                         "CLT.WEB.UI.LMS.MANAGE", (object)xTempParams);
                        if (xCompanyID.Length == 1)
                            this.txtMasterCode.Text = "000" + xCompanyID;

                        else if (xCompanyID.Length == 2)
                            this.txtMasterCode.Text = "00" + xCompanyID;

                        else if (xCompanyID.Length == 3)
                            this.txtMasterCode.Text = "0" + xCompanyID;

                        else
                            this.txtMasterCode.Text = xCompanyID;

                        this.chkUse_yn.Checked = true;
                    }
                    else if (Request.QueryString["openerEditMode"].ToString() == "m_cd_edit")   // MasterCode 수정일 경우...
                    {
                        if (Request.QueryString["openerm_cd"] != null && Request.QueryString["openerm_cd"].ToString() != "")
                            xMasterCode = Request.QueryString["openerm_cd"].ToString();
                    }
                }
                else
                {
                    return;
                    //string xScriptContent = "<script>alert('잘못된 경로를 통해 접근하였습니다.');self.close();</script>";
                    //ScriptHelper.ScriptBlock(this, "codemaster_edit", xScriptContent);
                }

                if (!IsPostBack)
                {
                    //this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                    if (!string.IsNullOrEmpty(xMasterCode) && Request.QueryString["openerEditMode"].ToString() == "m_cd_edit")
                        EditMode(xMasterCode);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnSend_Click
        * Purpose       : 저장, 수정버튼 클릭 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnSend_Click(object sender, EventArgs e)
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string xScriptMsg = string.Empty;

                // 필수입력항목 체크

                if (string.IsNullOrEmpty(this.txtCodeMaster_KNM.Text))
                {
                    //xScriptMsg = "<script>alert('한글명을 입력하여 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "한글명" }, new string[] { "Kor Name" }, Thread.CurrentThread.CurrentCulture));
                    this.txtCodeMaster_KNM.Focus();
                }

                else if (string.IsNullOrEmpty(this.txtCodeMaster_ENM.Text))
                {
                    //xScriptMsg = "<script>alert('영문명을 입력하여 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "영문명" }, new string[] { "Eng Name" }, Thread.CurrentThread.CurrentCulture));
                    this.txtCodeMaster_ENM.Focus();
                }

                else if (string.IsNullOrEmpty(this.txtMasterCode_NM.Text))
                {
                    //xScriptMsg = "<script>alert('코드명을 입력하여 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "코드명" }, new string[] { "Code Name" }, Thread.CurrentThread.CurrentCulture));
                    this.txtMasterCode_NM.Focus();
                }


                if (Request.QueryString["openerEditMode"].ToString() == "m_cd_new")   // MasterCode 신규등록일 경우...
                    MasterInsertCode();
                else if (Request.QueryString["openerEditMode"].ToString() == "m_cd_edit")   // MasterCode 수정일 경우...
                    MasterUpdateCode();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : btnRewrite_Click
        * Purpose       : 코드정보 초기화 버튼            
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnRewrite_Click(object sender, EventArgs e)
        protected void btnRewrite_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtMasterCode_NM.Text = string.Empty;
                this.txtCodeMaster_ENM.Text = string.Empty;
                this.txtCodeMaster_KNM.Text = string.Empty;
                this.chkUse_yn.Checked = true;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion




        /************************************************************
        * Function name : MasterUpdateCode
        * Purpose       : Master 공통코드 Update
        * Input         : void
        * Output        : void
        *************************************************************/
        #region MasterUpdateCode()
        public void MasterUpdateCode()
        {
            try
            {
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;
                string[] xParams = new string[9];

                xParams[0] = txtMasterCode.Text;
                xParams[1] = txtMasterCode_NM.Text.Replace("'", "''");
                xParams[2] = txtCodeMaster_KNM.Text.Replace("'", "''");
                xParams[3] = txtCodeMaster_ENM.Text.Replace("'", "''");
                xParams[8] = "1";
                if (chkUse_yn.Checked == true)
                    xParams[4] = "Y";
                else
                    xParams[4] = "N";

                xParams[5] = Session["USER_ID"].ToString();

                // 사용자 전용 코드 12.02.21 추가 By KMK

                if (chkUser_Code_YN.Checked == true)
                {
                    xParams[6] = "Y";
                    xParams[8] = "3";
                }
                else
                    xParams[6] = "N";


                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_mastercode_md",
                                         "SetCodeMasterEdit",
                                         LMS_SYSTEM.MANAGE,
                                         "CLT.WEB.UI.LMS.MANAGE",
                                         (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');self.close();</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');self.close();</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
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
        * Function name : MasterInsertCode
        * Purpose       : Master 공통코드 Insert
        * Input         : void
        * Output        : void
        *************************************************************/
        #region MasterInsertCode()
        public void MasterInsertCode()
        {
            try
            {
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;
                string[] xParams = new string[8];

                xParams[0] = txtMasterCode.Text.Replace("'", "''");
                xParams[1] = txtMasterCode_NM.Text.Replace("'", "''");
                xParams[2] = txtCodeMaster_KNM.Text.Replace("'", "''");
                xParams[3] = txtCodeMaster_ENM.Text.Replace("'", "''");
                xParams[4] = "1";

                if (chkUse_yn.Checked == true)
                    xParams[5] = "Y";
                else
                    xParams[5] = "N";

                xParams[6] = Session["USER_ID"].ToString();

                if (chkUser_Code_YN.Checked == true)
                {
                    xParams[7] = "Y";
                    xParams[4] = "3";
                }
                else
                    xParams[7] = "N";

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_mastercode_md",
                                         "SetCodeMaster",
                                         LMS_SYSTEM.MANAGE,
                                         "CLT.WEB.UI.LMS.MANAGE",
                                         (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');self.close();</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));

                    string[] xTempParams = new string[5];
                    string xCompanyID = string.Empty;

                    // Master Code 처리
                    xTempParams[0] = "t_code_master";
                    xTempParams[1] = "m_cd";
                    xTempParams[2] = "0";
                    xTempParams[3] = "4";
                    xTempParams[4] = "";


                    xCompanyID = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                     "GetMaxIDOfCode",
                                                     LMS_SYSTEM.MANAGE,
                                                     "CLT.WEB.UI.LMS.MANAGE", (object)xTempParams);
                    if (xCompanyID.Length == 1)
                        this.txtMasterCode.Text = "000" + xCompanyID;

                    else if (xCompanyID.Length == 2)
                        this.txtMasterCode.Text = "00" + xCompanyID;

                    else if (xCompanyID.Length == 3)
                        this.txtMasterCode.Text = "0" + xCompanyID;

                    else
                        this.txtMasterCode.Text = xCompanyID;

                    this.txtCodeMaster_ENM.Text = string.Empty;
                    this.txtCodeMaster_KNM.Text = string.Empty;
                    this.txtMasterCode_NM.Text = string.Empty;

                    this.chkUse_yn.Checked = true;
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');self.close();</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
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
        * Function name : EditMode
        * Purpose       : MasterCode Data Insert
        * Input         : void
        * Output        : void
        *************************************************************/
        #region EditMode(string rMasterCode)
        public void EditMode(string rMasterCode)
        {
            try
            {
                string[] xParams = new string[1];
                xParams[0] = rMasterCode;

                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_mastercode_md",
                                      "GetMasterCodeEdit",
                                      LMS_SYSTEM.MANAGE,
                                      "CLT.WEB.UI.LMS.MANAGE", (object)xParams);
                if (xDt.Rows.Count == 0)
                    return;

                DataRow xDr = xDt.Rows[0];

                this.txtMasterCode.Text = rMasterCode;
                this.txtCodeMaster_KNM.Text = xDr["m_knm"].ToString();
                this.txtCodeMaster_ENM.Text = xDr["m_enm"].ToString();
                this.txtMasterCode_NM.Text = xDr["m_desc"].ToString();
                if (xDr["use_yn"].ToString() == "Y")
                    chkUse_yn.Checked = true;
                else
                    chkUse_yn.Checked = false;

                if (xDr["user_code_yn"].ToString() == "Y")
                    chkUser_Code_YN.Checked = true;
                else
                    chkUser_Code_YN.Checked = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

    }
}
