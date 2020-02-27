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
using System.Globalization;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.COMMON.BASE;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Linq;

namespace CLT.WEB.UI.LMS.MYPAGE
{
    public partial class join_user_reg : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    Response.Redirect("/");
                }
            }
            catch { Response.Redirect("/"); }

            if (!IsPostBack)
            {
                if (!UrlReferrerCheck("/mypage/join_user_agree.aspx"))
                {
                    string xScriptMsg = string.Format("<script>alert('잘못된 접근입니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "join", xScriptMsg);
                    return;
                }

                Session["MENU_CODE1"] = "0"; // 대분류 메뉴
                Session["MENU_CODE2"] = "0"; // 중분류 메뉴
                Session["MENU_CODE3"] = "2"; // 소분류 메뉴
                Session["MENU_CODE"] = Session["MENU_CODE1"] + "" + Session["MENU_CODE2"] + "" + Session["MENU_CODE3"];

                this.txtZipCode.Attributes.Add("ReadOnly", "ReadOnly");
                BindDropDownList();
            }

        }

        public bool UrlReferrerCheck(string pPageName)
        {
            bool bReturn = true;

            try
            {
                if (Request.UrlReferrer == null)
                {
                    bReturn = false;
                }
                else
                {
                    string refer = Request.UrlReferrer.ToString().ToLower();
                    string ServerPath = Request.Url.ToString();
                    ServerPath = ServerPath.Substring(0, ServerPath.LastIndexOf("/"));

                    if (refer.IndexOf(ServerPath) == -1)
                    {
                        bReturn = false;
                    }

                    if (!string.IsNullOrEmpty(pPageName))
                    {
                        if (refer.IndexOf(pPageName) == -1)
                        {
                            bReturn = false;
                        }
                    }
                }
            }
            catch
            {
                bReturn = false;
            }

            return bReturn;
        }

        /************************************************************
        * Function name : BindDropDownList
        * Purpose       : DropDownList 데이터 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region BindDropDownList()
        private void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[2];
                string xSql = string.Empty;
                DataTable xDt = null;
                
                // 직급(직위) 해당 직급은 법인사 관리자에게만 해당되며, 인사시스템에서 사용하는 직급이아님!(사장, 과장, 부장 등...)
                xParams[0] = "0023";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.MANAGE,
                                             "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlComapnyduty, xDt);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : btnIDCheck_OnClick
        * Purpose       : ID 중복체크 버튼클릭 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnIDCheck_OnClick()
        protected void btnIDCheck_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtID.Text))
                {
                    //Response.Write("<script language='javascript'>alert('ID 를 입력해 주세요!');</script>");
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "ID" }, new string[] { "ID" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                string[] xParams = new string[1];
                xParams[0] = txtID.Text.ToLower().Trim();
                // ID 중복체크
                DataTable xdt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                                 "GetUser",
                                                 LMS_SYSTEM.MANAGE,
                                                 "CLT.WEB.UI.LMS.MANAGE",
                                                 (object)xParams);
                if (xdt.Rows.Count == 0)
                {
                    //Response.Write("<script language='javascript'>alert('사용가능한 ID 입니다.');</script>");
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A107", new string[] { this.txtID.Text }, new string[] { this.txtID.Text }, Thread.CurrentThread.CurrentCulture));
                    this.txtID.Enabled = false;
                    return;
                }
                else
                {
                    //Response.Write("<script language='javascript'>alert('동일한 ID가 존재합니다.');</script>");
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A026", new string[] { this.txtID.Text }, new string[] { this.txtID.Text }, Thread.CurrentThread.CurrentCulture));
                    this.txtID.Text = string.Empty;
                    this.txtID.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnSave_OnClick
        * Purpose       : 저장 버튼클릭 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnSave_OnClick()
        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                string xRtn = Boolean.TrueString;
                string xScriptContent = string.Empty;
                string[] xParams = new string[32];

                if (this.txtID.Enabled == true)
                {
                    //ID 중복체크 버튼을 클릭하여 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A104", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    this.txtID.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtPass.Text))
                {
                    // 비밀번호를 입력해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A105", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    this.txtPass.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtPassCheck.Text))
                {
                    //비밀번호 이(가) 입력되지 않았습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "비밀번호" }, new string[] { "Password" }, Thread.CurrentThread.CurrentCulture));
                    this.txtPassCheck.Focus();
                    return;
                }
                else if (this.txtPass.Text != this.txtPassCheck.Text)
                {
                    // {0}이(가) 입력되지 않았거나 잘못된 형식입니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A016", new string[] { "비밀번호" }, new string[] { "Password" }, Thread.CurrentThread.CurrentCulture));
                    this.txtPassCheck.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtZipCode.Text))
                {
                    // 우편번호를 검색하여 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "우편번호" }, new string[] { "Zip Code" }, Thread.CurrentThread.CurrentCulture));
                    this.txtZipCode.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtAddr1.Text))
                {
                    // 주소를 입력해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "주소" }, new string[] { "Address" }, Thread.CurrentThread.CurrentCulture));
                    this.txtAddr1.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtAddr2.Text))
                {
                    //xScriptContent = "<script>alert('상세주소를 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "주소" }, new string[] { "Address" }, Thread.CurrentThread.CurrentCulture));
                    this.txtAddr2.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtDept.Text))
                {
                    //xScriptContent = "<script>alert('부서를 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "부서" }, new string[] { "Dept" }, Thread.CurrentThread.CurrentCulture));
                    this.txtDept.Focus();
                    return;
                }
                else if (this.ddlComapnyduty.SelectedItem.Text == "*")
                {
                    //xScriptContent = "<script>alert('직급을 선택해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "직급" }, new string[] { "Rank" }, Thread.CurrentThread.CurrentCulture));
                    this.ddlComapnyduty.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtEmail.Text))
                {
                    //xScriptContent = "<script>alert('Email을 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "E-Mail" }, new string[] { "E-Mail" }, Thread.CurrentThread.CurrentCulture));
                    this.txtEmail.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtMobilePhone.Text))
                {
                    //xScriptContent = "<script>alert('휴대폰을 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "휴대폰번호" }, new string[] { "Mobile Phone Number" }, Thread.CurrentThread.CurrentCulture));
                    this.txtMobilePhone.Focus();
                    return;
                }
                
                xParams[0] = this.txtID.Text.ToLower().Replace("'", "''");                 // 사용자 ID
                xParams[1] = OpusCryptoLibrary.Cryptography.Encrypt(txtPass.Text.Replace("'", "''"), txtPass.Text.Replace("'", "''"));
                xParams[2] = "";
                xParams[3] = this.txtuser_nm_kor.Text.Replace("'", "''");                   // 사용자명
                xParams[4] = " "; //주민번호
                xParams[5] = txtuser_nm_eng_first.Text.Replace("'", "''");  // 영문성명 First
                xParams[6] = txtuser_nm_eng_last.Text.Replace("'", "''");  // 영문성명 Last
                xParams[7] = this.ddlComapnyduty.SelectedItem.Value;    // 사용자 직급
                xParams[8] = txtDept.Text.Replace("'", "''");  // 부서
                xParams[9] = this.txtMobilePhone.Text.Replace("'", "''");                  // 사용자 휴대전화번호
                xParams[10] = " ";  //고용형태 duty_gu
                xParams[11] = this.txtEmail.Text.Replace("'", "''");                        // 메일주소
                xParams[12] = this.txtPhone.Text.Replace("'", "''");                  // 사무실 전화번호
                xParams[13] = " ";                          // 팩스번호
                xParams[14] = " ";  // 승인 담당자 admin_id
                xParams[15] = " ";  // 업체 연락처
                xParams[16] = " ";  // 업체 담당자
                xParams[17] = "000004";  // 상태 승인대기
                xParams[18] = " ";  // 회사ID
                xParams[19] = txtAcquisition.Text.Replace(".", "").Trim() == string.Empty ? null : txtAcquisition.Text; //고용보험취득일
                xParams[20] = "000010";  // 사용자그룹
                xParams[21] = " "; // 신분 //socialpos
                xParams[22] = txtZipCode.Text; // 사용자 우편번호 user_zip_code
                xParams[23] = txtAddr1.Text + " | " + txtAddr2.Text; // 사용자 주소 user_addr
                if (rbSMS_y.Checked == true)
                    xParams[24] = "Y"; // SMS 수신여부
                else
                    xParams[24] = "N"; // SMS 수신여부

                if (rbMail_y.Checked == true)
                    xParams[25] = "Y"; // MAIL 수신여부
                else
                    xParams[25] = "N"; // MAIL 수신여부

                xParams[26] = xParams[0];
                xParams[27] = xParams[0];
                xParams[28] = "000001";
                xParams[29] = txtBirth_dt.Text.Replace(".", "").Trim() == string.Empty ? null : txtBirth_dt.Text; //생년월일

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                         "SetUser",
                                         LMS_SYSTEM.MANAGE,
                                         "CLT.WEB.UI.LMS.MANAGE",
                                         (object)xParams);

                string[] xResult = xRtn.Split('|');

                //if (xRtn.ToUpper() == "TRUE")
                if (xResult[0].ToUpper() == "TRUE")
                {
                    bSaveFile();

                    //ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A129", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    //ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    ScriptHelper.ScriptBlock(this, "join_company_reg", "<script>location.href='/mypage/join_complete.aspx';</script>");
                }
                else
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    ScriptHelper.ScriptBlock(this, "join_company_reg", "<script>location.href='/';</script>");

                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }

        }

        private bool bSaveFile()
        {
            bool bReturn = false;

            try
            {
                string xFileName = "";
                byte[] xFileByte = null;

                FileUpload file = this.fileUplaod;// ((FileUpload)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("fileUplaod"));
                if (file.FileBytes.Length > 0)
                {
                    xFileName = file.FileName.Replace(" ", "_").Replace("..", "_");
                    xFileByte = file.FileBytes;

                    SBROKER.GetString("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                    "SetFileAtt",
                                    LMS_SYSTEM.MANAGE,
                                    "CLT.WEB.UI.LMS.MANAGE",
                                    xFileByte,
                                    xFileName,
                                    txtID.Text.ToUpper().Replace("'", "''"));
                }
                file.Dispose();

                bReturn = true;
            }
            catch (Exception ex)
            {
                bReturn = false;
            }

            return bReturn;
        }
        #endregion
    }
}