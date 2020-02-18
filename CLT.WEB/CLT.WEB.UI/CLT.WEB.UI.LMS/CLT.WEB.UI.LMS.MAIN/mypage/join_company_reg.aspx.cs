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
    public partial class join_company_reg : BasePage
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

            //List 페이지로부터 CompanyName, RegNo 를 전달받은 경우만 해당 페이지를 처리하고
            //그렇지 않은 경우, 메세지 출력과 함께 창을 종료한다.
            if ((Request.QueryString["CompanyName"] != null && Request.QueryString["CompanyName"].ToString() != "") && (Request.QueryString["RegNo"] != null && Request.QueryString["CompanyName"].ToString() != ""))
            {
                this.txtCompanyName.Text = Request.QueryString["CompanyName"].ToString();
                this.txtTex.Text = Request.QueryString["RegNo"].ToString();
            }
            else
            {
                ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A102", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                ScriptHelper.ScriptBlock(this, "vp_m_manage_user_reg_wpg", "<script>swindow.location.href='/';</script>");
            }

            if (!IsPostBack)
            {
                if (!UrlReferrerCheck("/mypage/join_company_agree.aspx"))
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

                // 회사규모 (대기업, 중소기업 등..)
                xParams[0] = "0043";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.MANAGE,
                                             "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlCompanyScale, xDt);

                // 회사구분 (그룹사, 사업자 위수탁)
                xParams[0] = "0061";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.MANAGE,
                                             "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlCompanyKind, xDt);

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
                    //ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A107", new string[] { this.txtID.Text }, new string[] { this.txtID.Text }, Thread.CurrentThread.CurrentCulture));
                    string xMsg = MsgInfo.GetMsg("A107", new string[] { this.txtID.Text }, new string[] { this.txtID.Text }, Thread.CurrentThread.CurrentCulture);
                    ScriptHelper.ScriptBlock(this, "showTab", "<script>$(document).ready(function(){alert('"+ xMsg + "');$('#tab_2').trigger('click');});</script>");
                    this.txtID.Enabled = false;
                    return;
                }
                else
                {
                    //Response.Write("<script language='javascript'>alert('동일한 ID가 존재합니다.');</script>");
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A026", new string[] { this.txtID.Text }, new string[] { this.txtID.Text }, Thread.CurrentThread.CurrentCulture));
                    string xMsg = MsgInfo.GetMsg("A026", new string[] { this.txtID.Text }, new string[] { this.txtID.Text }, Thread.CurrentThread.CurrentCulture);
                    ScriptHelper.ScriptBlock(this, "showTab", "<script>$(document).ready(function(){alert('" + xMsg + "');$('#tab_2').trigger('click');});</script>");
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
                else if (string.IsNullOrEmpty(this.txtCompanyName.Text))
                {
                    // 회사명을 입력해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "회사명" }, new string[] { "Company Name" }, Thread.CurrentThread.CurrentCulture));
                    this.txtCompanyName.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtCEOName.Text))
                {
                    // 대표자명을 입력해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "대표자명" }, new string[] { "CEO Name" }, Thread.CurrentThread.CurrentCulture));
                    this.txtCEOName.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtTex.Text))
                {
                    // 사업자 등록번호를 입력해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "사업자 등록번호" }, new string[] { "Business Registration Number" }, Thread.CurrentThread.CurrentCulture));
                    this.txtTex.Focus();
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
                else if (string.IsNullOrEmpty(this.txtPhone.Text))
                {
                    //xScriptContent = "<script>alert('전화번호를 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "전화번호" }, new string[] { "Phone Number" }, Thread.CurrentThread.CurrentCulture));
                    this.txtPhone.Focus();
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
                else if (string.IsNullOrEmpty(this.txtuser_nm_kor.Text))
                {
                    // 담당자명을 입력해 주세요.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "담당자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                    this.txtMobilePhone.Focus();
                    return;
                }
                else if (this.ddlComapnyduty.SelectedItem.Text == "*")
                {
                    // 회사규모를 선택해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "회사규모" }, new string[] { "Company Size" }, Thread.CurrentThread.CurrentCulture));
                    this.ddlComapnyduty.Focus();
                    return;
                }
                
                string[] xTempParams = new string[2];
                string xCompanyID = string.Empty;
                // company_id 처리
                xTempParams[0] = "t_company";
                xTempParams[1] = "company_id";

                xCompanyID = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                 "GetMaxIDOfTable",
                                                 LMS_SYSTEM.MANAGE,
                                                 "CLT.WEB.UI.LMS.MANAGE", (object)xTempParams);
                

                xParams[0] = this.txtID.Text.ToLower().Replace("'", "''");                 // 사용자 ID


                xParams[1] = OpusCryptoLibrary.Cryptography.Encrypt(txtPass.Text.Replace("'", "''"), txtPass.Text.Replace("'", "''"));
                //xParams[1] = (string)xx.Encrypt(txtPass.Text.Replace("'", "''")); //this.txtPass.Text.Replace("'", "''");                         // 비밀번호

                xParams[2] = this.txtCEOName.Text.Replace("'", "''");                      // 대표자명                
                xParams[3] = this.ddlComapnyduty.SelectedItem.Value;    // 사용자 직급
                xParams[4] = this.txtMobilePhone.Text.Replace("'", "''");                  // 사용자 휴대전화번호
                xParams[5] = this.txtPhone_Admin.Text.Replace("'", "''");                  // 사무실 전화번호
                xParams[6] = this.txtEmail.Text.Replace("'", "''");                        // 메일주소
                xParams[7] = this.txtFax.Text.Replace("'", "''");                          // 팩스번호
                xParams[8] = this.txtuser_nm_kor.Text.Replace("'", "''");                   // 담당자명
                xParams[9] = this.txtCompanyName.Text.Replace("'", "''");  // 회사명                
                xParams[10] = this.txtTex.Text.Replace("'", "''");  // 사업자 등록번호          
                xParams[11] = this.txtRegNo.Text.Replace("'", "''");  // 법인 등록번호
                xParams[12] = this.txtEmpoly_Ins.Text.Replace("'", "''");  // 고용 보험번호
                xParams[13] = this.txtBusi.Text.Replace("'", "''");  // 업태
                xParams[14] = this.txtCompanyType.Text.Replace("'", "''");  // 종목
                xParams[15] = this.txtZipCode.Text;  // 우편번호
                xParams[16] = this.txtAddr1.Text.Replace("'", "''") + " | " + this.txtAddr2.Text.Replace("'", "''"); // 주소
                xParams[17] = this.txtHomePage.Text.Replace("'", "''");  // 홈페이지
                xParams[18] = this.ddlCompanyScale.SelectedValue.ToString().Replace("'", "''"); // 
                xParams[19] = this.txtPhone.Text.Replace("'", "''");  // 전화번호
                xParams[20] = xCompanyID; // 업체 ID(MMDD + Seq 4자리)
                //
                xParams[21] = ddlCompanyKind.SelectedValue.ToString().Replace("'", "''"); // 회사구분;
                xParams[22] = txtEmpCountVessel.Text.Replace("'", "''");  // 근로자수(해상직원)
                xParams[23] = txtEmpCountShore.Text.Replace("'", "''");  // 근로자수(육상직원)
                xParams[24] = txtuser_nm_eng_first.Text.Replace("'", "''");  // 영문성명 First
                xParams[25] = txtuser_nm_eng_last.Text.Replace("'", "''");  // 영문성명 Last
                xParams[26] = txtBirth_dt.Text.Replace(".", "").Trim() == string.Empty ? null : txtBirth_dt.Text; //생년월일
                xParams[27] = txtDept.Text.Replace("'", "''");  // 부서
                xParams[28] = txtEmail_Admin.Text.Replace("'", "''");                        // 메일주소
                xParams[29] = txtAcquisition.Text.Replace(".", "").Trim() == string.Empty ? null : txtAcquisition.Text; //고용보험취득일

                if (rbSMS_y.Checked == true)
                    xParams[30] = "Y"; // SMS 수신여부
                else
                    xParams[30] = "N"; // SMS 수신여부

                if (rbMail_y.Checked == true)
                    xParams[31] = "Y"; // MAIL 수신여부
                else
                    xParams[31] = "N"; // MAIL 수신여부
                
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                             "SetUserInfo",
                             LMS_SYSTEM.MANAGE,
                             "CLT.WEB.UI.LMS.MANAGE",
                             (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
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