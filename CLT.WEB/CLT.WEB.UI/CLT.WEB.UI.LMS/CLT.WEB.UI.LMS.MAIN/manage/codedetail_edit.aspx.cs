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

namespace CLT.WEB.UI.LMS.MANAGE
{
    /// <summary>
    /// 1. 작업개요 : (Detail)공통코드 생성, 수정 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 Detail 공통코드 생성, 수정 관리

    ///				  
    /// 3. Class 명 : codedetail_edit
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    public partial class codedetail_edit : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 웹페이지 Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');self.close();", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "codedetail_edit", xScriptMsg);

                    return;
                }

                string xMasterCode = string.Empty;  // 다른 페이지로 부터 넘겨받은 MasterCode 용 변수

                string xDetailCode = string.Empty;  // 다른 페이지로 부터 넘겨받은 DetailCode 용 변수

                if (Request.QueryString["openerEditMode"] != null)
                {
                    if (Request.QueryString["openerEditMode"].ToString() == "d_cd_new")   // DetailCode 신규등록일 경우...
                    {
                        string[] xTempParams = new string[5];
                        string xCompanyID = string.Empty;

                        // 사용자 전용 코드일 경우 ID 생성을 하지 않는다.
                        if (Request.QueryString["openeruser_code_nm"] != "Y")
                        {
                            // Detailcode_id 처리
                            xTempParams[0] = "t_code_detail";
                            xTempParams[1] = "d_cd";
                            xTempParams[2] = "1";
                            xTempParams[3] = "6";
                            xTempParams[4] = Request.QueryString["openerm_cd"];


                            xCompanyID = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                             "GetMaxIDOfCode",
                                                             LMS_SYSTEM.MANAGE,
                                                             "CLT.WEB.UI.LMS.MANAGE", (object)xTempParams);
                            if (xCompanyID.Length == 1)
                                this.txtMasterCode.Text = "00000" + xCompanyID;

                            else if (xCompanyID.Length == 2)
                                this.txtMasterCode.Text = "0000" + xCompanyID;

                            else if (xCompanyID.Length == 3)
                                this.txtMasterCode.Text = "000" + xCompanyID;

                            else if (xCompanyID.Length == 4)
                                this.txtMasterCode.Text = "00" + xCompanyID;

                            else if (xCompanyID.Length == 5)
                                this.txtMasterCode.Text = "0" + xCompanyID;

                            else
                                this.txtMasterCode.Text = xCompanyID;


                        }
                        else
                        {
                            this.txtMasterCode.Enabled = true;
                            this.txtMasterCode.ReadOnly = false;
                        }
                        //this.chkUse_yn.Checked = true;
                    }
                    else if (Request.QueryString["openerEditMode"].ToString() == "d_cd_edit")   // DetailCode 수정일 경우...
                    {
                        xMasterCode = Request.QueryString["openerm_cd"].ToString();
                        xDetailCode = Request.QueryString["openerd_cd"].ToString();
                    }
                }
                else
                {
                    //string xScriptContent = "<script>alert('잘못된 경로를 통해 접근하였습니다.');self.close();</script>";
                    //ScriptHelper.ScriptBlock(this, "codedetail_edit", xScriptContent);
                    return;
                }

                if (!IsPostBack)
                {
                    //this.Page.Form.DefaultButton = this.btn.UniqueID; // Page Default Button Mapping
                    if (!string.IsNullOrEmpty(xMasterCode) && Request.QueryString["openerEditMode"].ToString() == "d_cd_edit")
                        EditMode(xMasterCode, xDetailCode);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        #region 버튼클릭 이벤트

        /************************************************************
        * Function name : btnSend_Click
        * Purpose       : 저장, 수정버튼 클릭 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string xScriptMsg = string.Empty;

                // 필수입력항목 체크

                if (string.IsNullOrEmpty(this.txtCodeMaster_KNM.Text))
                {
                    //xScriptMsg = "<script>alert('한글명을 클릭하여 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "한글명" }, new string[] { "Kor Name" }, Thread.CurrentThread.CurrentCulture));
                    this.txtCodeMaster_KNM.Focus();
                }

                else if (string.IsNullOrEmpty(this.txtCodeMaster_ENM.Text))
                {
                    //xScriptMsg = "<script>alert('영문명을 클릭하여 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "영문명" }, new string[] { "Eng Name" }, Thread.CurrentThread.CurrentCulture));
                    this.txtCodeMaster_ENM.Focus();
                }

                else if (string.IsNullOrEmpty(this.txtMasterCode_NM.Text))
                {
                    //xScriptMsg = "<script>alert('코드명을 클릭하여 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "코드명" }, new string[] { "Code Name" }, Thread.CurrentThread.CurrentCulture));
                    this.txtMasterCode_NM.Focus();
                }


                // 메세지가 존재할 경우 메세지를 출력하고 Return
                if (!string.IsNullOrEmpty(xScriptMsg))
                {
                    Response.Write(xScriptMsg);
                    return;
                }

                if (Request.QueryString["openerEditMode"].ToString() == "d_cd_new")   // DetailCode 신규등록일 경우...
                    DetailInsertCode();
                else if (Request.QueryString["openerEditMode"].ToString() == "d_cd_edit")  // DetailCode 수정일 경우...
                    DetailUpdateCode();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : btnRewrite_Click
        * Purpose       : 코드정보 초기화 버튼            
        * Input         : void
        * Output        : void
        *************************************************************/
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
        #endregion 버튼클릭 이벤트



        #region 메서드

        /************************************************************
        * Function name : DetailUpdateCode
        * Purpose       : Detail 공통코드 Update
        * Input         : void
        * Output        : void
        *************************************************************/
        public void DetailUpdateCode()
        {
            try
            {
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;
                string[] xParams = new string[7];

                xParams[0] = txtMasterCode.Text;
                xParams[1] = txtMasterCode_NM.Text.Replace("'", "''");
                xParams[2] = txtCodeMaster_KNM.Text.Replace("'", "''");
                xParams[3] = txtCodeMaster_ENM.Text.Replace("'", "''");

                if (chkUse_yn.Checked == true)
                    xParams[4] = "Y";
                else
                    xParams[4] = "N";

                xParams[5] = Session["USER_ID"].ToString();
                xParams[6] = Request.QueryString["openerm_cd"];

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_detailcode_md",
                                         "SetCodeDetailEdit",
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

        /************************************************************
        * Function name : DetailInsertCode
        * Purpose       : Detail 공통코드 Insert
        * Input         : void
        * Output        : void
        *************************************************************/
        public void DetailInsertCode()
        {
            try
            {
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;
                string[] xParams = new string[8];

                xParams[0] = txtMasterCode.Text;
                xParams[1] = txtMasterCode_NM.Text.Replace("'", "''");
                xParams[2] = txtCodeMaster_KNM.Text.Replace("'", "''");
                xParams[3] = txtCodeMaster_ENM.Text.Replace("'", "''");
                xParams[4] = "1";

                if (chkUse_yn.Checked == true)
                    xParams[5] = "Y";
                else
                    xParams[5] = "N";

                xParams[6] = Session["USER_ID"].ToString();
                xParams[7] = Request.QueryString["openerm_cd"];

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_detailcode_md",
                                         "SetCodeDetail",
                                         LMS_SYSTEM.MANAGE,
                                         "CLT.WEB.UI.LMS.MANAGE",
                                         (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');self.close();</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));

                    string[] xTempParams = new string[5];
                    string xCompanyID = string.Empty;

                    // Detailcode_id 처리
                    xTempParams[0] = "t_code_detail";
                    xTempParams[1] = "d_cd";
                    xTempParams[2] = "1";
                    xTempParams[3] = "6";
                    xTempParams[4] = Request.QueryString["openerm_cd"];


                    xCompanyID = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                     "GetMaxIDOfCode",
                                                     LMS_SYSTEM.MANAGE,
                                                     "CLT.WEB.UI.LMS.MANAGE", (object)xTempParams);
                    if (xCompanyID.Length == 1)
                        this.txtMasterCode.Text = "00000" + xCompanyID;

                    else if (xCompanyID.Length == 2)
                        this.txtMasterCode.Text = "0000" + xCompanyID;

                    else if (xCompanyID.Length == 3)
                        this.txtMasterCode.Text = "000" + xCompanyID;

                    else if (xCompanyID.Length == 4)
                        this.txtMasterCode.Text = "00" + xCompanyID;

                    else if (xCompanyID.Length == 5)
                        this.txtMasterCode.Text = "0" + xCompanyID;

                    else
                        this.txtMasterCode.Text = xCompanyID;

                    this.txtCodeMaster_ENM.Text = string.Empty;
                    this.txtCodeMaster_KNM.Text = string.Empty;
                    this.txtMasterCode_NM.Text = string.Empty;
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

        /************************************************************
        * Function name : EditMode
        * Purpose       : DetailCode Data Insert
        * Input         : void
        * Output        : void
        *************************************************************/
        public void EditMode(string rMasterCode, string rDetailCode)
        {
            try
            {
                string[] xParams = new string[2];
                xParams[0] = rMasterCode;
                xParams[1] = rDetailCode;
                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_detailcode_md",
                                      "GetDetailCodeEdit",
                                      LMS_SYSTEM.MANAGE,
                                      "CLT.WEB.UI.LMS.MANAGE", (object)xParams);
                if (xDt.Rows.Count == 0)
                    return;

                DataRow xDr = xDt.Rows[0];

                this.txtMasterCode.Text = rDetailCode;
                this.txtCodeMaster_KNM.Text = xDr["d_knm"].ToString();
                this.txtCodeMaster_ENM.Text = xDr["d_enm"].ToString();
                this.txtMasterCode_NM.Text = xDr["d_desc"].ToString();
                if (xDr["use_yn"].ToString() == "Y")
                    chkUse_yn.Checked = true;
                else
                    chkUse_yn.Checked = false;
            }
            catch (Exception ex)
            {
                bool rethow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethow) throw;
            }
        }
        #endregion 메서드

    }
}
