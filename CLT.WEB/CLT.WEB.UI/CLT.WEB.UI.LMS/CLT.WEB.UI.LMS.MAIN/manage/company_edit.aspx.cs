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
using System.Drawing;

namespace CLT.WEB.UI.LMS.MANAGE
{
    /// <summary>
    /// 1. 작업개요 : 법인사(타사) 입력,수정화면 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 법인사(타사) 입력,수정화면
    ///				  
    /// 3. Class 명 : company_edit
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    /// 
    public partial class company_edit : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : Page Load 이벤트
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
                    ScriptHelper.ScriptBlock(this, "company_edit", xScriptMsg);

                    return;
                }

                if (Request.QueryString["EDITMODE"] == null)  // 받는값이 없으면 창종료...
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A102", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    ScriptHelper.ScriptBlock(this, "company_edit", "<script>self.close()</script>");
                    return;
                }

                if (!IsPostBack)
                {
                    //this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                    this.txtZipCode.Attributes.Add("ReadOnly", "ReadOnly");
                    BindDropDownList();
                    ViewState["EDITMODE"] = Request.QueryString["EDITMODE"];

                    if (Request.QueryString["COMPANY_ID"] != null)
                        ViewState["COMPANY_ID"] = Request.QueryString["COMPANY_ID"].ToString();

                    if (Request.QueryString["EDITMODE"] != null)
                    {
                        if (Request.QueryString["COMPANY_ID"] == null)
                            return;

                        if (ViewState["EDITMODE"].ToString() == "EDIT") // 수정이면
                        {
                            EDIT(ViewState["EDITMODE"].ToString());
                            //수정모드에서는 사업자 등록번호, 회사코드, 수정불가 하도록 변경(회사구분의 경우는 관리자만 수정가능)
                            this.txtTex1.ReadOnly = true;
                            this.txtTex2.ReadOnly = true;
                            this.txtTex3.ReadOnly = true;
                            this.txtCompanyCode.ReadOnly = true;

                            if (Session["USER_GROUP"].ToString() != "000001") // 회사구분은 관리자만 수정가능
                            {
                                this.ddlCompanyKind.Enabled = false;
                                this.ddlCompanyKind.BackColor = Color.FromName("#dcdcdc");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(txtCompanyCode.Text)) // Null 또는 Empty 이면...
                                    this.txtCompanyCode.ReadOnly = false;
                            }

                            this.txtTex1.BackColor = Color.FromName("#dcdcdc");
                            this.txtTex2.BackColor = Color.FromName("#dcdcdc");
                            this.txtTex3.BackColor = Color.FromName("#dcdcdc");

                            if (txtCompanyCode.ReadOnly == true)
                                this.txtCompanyCode.BackColor = Color.FromName("#dcdcdc");
                        }

                    }
                    base.pRender(this.Page,
                                 new object[,] { //{ this.btnCancle, "I" },
                                             { this.btnRewrite, "I" },
                                             { this.btnSave, "E" },
                                           },
                                 Convert.ToString(Request.QueryString["MenuCode"]));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }

        }
        #endregion
        
        /************************************************************
        * Function name : button_OnClick
        * Purpose       : btnSave, btnRewrite 버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region button_OnClick(object seder, EventArgs e)
        protected void button_OnClick(object seder, EventArgs e)
        {
            try
            {
                Button btn = (Button)seder;
                if (btn.ID == "btnSave")
                {
                    Save();
                }
                else if (btn.ID == "btnRewrite")
                {
                    Rewrite();
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /*************************************************************
        * Function name : BindDropDownList
        * Purpose       : DropDownList Bind 메서드
        * Input         : void
        * Output        : void
        *************************************************************/
        #region BindDropDownList()
        public void BindDropDownList()
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
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion
        
        /*************************************************************
        * Function name : Save
        * Purpose       : 회원사 Save  메서드
        * Input         : void
        * Output        : void
        *************************************************************/
        #region Save()
        public void Save()
        {
            try
            {
                string xRtn = Boolean.TrueString;
                string xScriptContent = string.Empty;

                if (string.IsNullOrEmpty(this.txtCompanyCode.Text))
                {
                    // 회사코드를 입력해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "회사코드" }, new string[] { "Company Code" }, Thread.CurrentThread.CurrentCulture));
                    this.txtCompanyName.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtCompanyName.Text))
                {
                    // 회사명을 입력해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "회사명" }, new string[] { "Company Name" }, Thread.CurrentThread.CurrentCulture));
                    this.txtCompanyName.Focus();
                    return;
                }
                else if (this.ddlCompanyScale.SelectedItem.Text == "*")
                {
                    // 회사규모를 선택해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "회사규모" }, new string[] { "Company Size" }, Thread.CurrentThread.CurrentCulture));
                    this.ddlCompanyScale.Focus();
                    return;
                }
                else if (this.ddlCompanyKind.SelectedItem.Text == "*")
                {
                    // 회사구분을 선택해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "회사구분" }, new string[] { "Company Kind" }, Thread.CurrentThread.CurrentCulture));
                    this.ddlCompanyScale.Focus();
                    return;
                }
                else if (String.IsNullOrEmpty(this.txtCEOName.Text))
                {
                    // 회사규모를 선택해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "회사규모" }, new string[] { "Company Size" }, Thread.CurrentThread.CurrentCulture));
                    this.ddlCompanyScale.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtTex1.Text))
                {
                    // 사업자 등록번호를 입력해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "사업자 등록번호" }, new string[] { "Business Registration Number" }, Thread.CurrentThread.CurrentCulture));
                    this.txtTex1.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtTex2.Text))
                {
                    // 사업자 등록번호를 입력해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "사업자 등록번호" }, new string[] { "Business Registration Number" }, Thread.CurrentThread.CurrentCulture));
                    this.txtTex2.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtTex3.Text))
                {
                    // 사업자 등록번호를 입력해 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "사업자 등록번호" }, new string[] { "Business Registration Number" }, Thread.CurrentThread.CurrentCulture));
                    this.txtTex3.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtZipCode.Text))
                {
                    // 우편번호를 검색하여 주세요!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "우편번호" }, new string[] { "Zip Code" }, Thread.CurrentThread.CurrentCulture));
                    this.txtZipCode.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtPhone.Text))
                {
                    //xScriptContent = "<script>alert('전화번호를 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "전화번호" }, new string[] { "Phone Number" }, Thread.CurrentThread.CurrentCulture));
                    this.txtPhone.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtAddr1.Text))
                {
                    //xScriptContent = "<script>alert('주소를 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "주소" }, new string[] { "Address" }, Thread.CurrentThread.CurrentCulture));
                    this.txtAddr1.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtAddr2.Text))
                {
                    //xScriptContent = "<script>alert('주소를 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "주소" }, new string[] { "Address" }, Thread.CurrentThread.CurrentCulture));
                    this.txtAddr2.Focus();
                    return;
                }
                
                string[] xTempParams = new string[2];
                string xCompanyID = string.Empty;

                if (ViewState["EDITMODE"].ToString() == "NEW")  // COMPANY_ID가 Null 이면 Insert
                {
                    // company_id 처리
                    xTempParams[0] = "t_company";
                    xTempParams[1] = "company_id";

                    xCompanyID = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                     "GetMaxIDOfTable",
                                                     LMS_SYSTEM.MANAGE,
                                                     "CLT.WEB.UI.LMS.MANAGE.vp_m_manage_company_edit_wpg", (object)xTempParams);
                }
                else
                {
                    xCompanyID = ViewState["COMPANY_ID"].ToString();  //Request.QueryString["COMPANY_ID"].ToString().ToUpper();
                }

                string[] xParams = new string[18];
                xParams[0] = xCompanyID;  // 업체ID
                xParams[1] = this.txtCompanyCode.Text.Replace("'", "''"); // 업체코드
                xParams[2] = this.txtCompanyName.Text.Replace("'", "''"); // 업체명
                xParams[3] = this.ddlCompanyScale.SelectedValue; // 회사규모
                xParams[4] = this.txtTex1.Text.Replace("'", "''") + this.txtTex2.Text.Replace("'", "''") + this.txtTex3.Text.Replace("'", "''"); // 사업자 등록번호
                xParams[5] = this.txtRegNo.Text.Replace("'", "''");  //  법인 등록번호
                xParams[6] = this.txtEmpoly_Ins.Text.Replace("'", "''"); // 고용 보험번호
                xParams[7] = this.txtCEOName.Text.Replace("'", "''"); // 대표자명
                xParams[8] = this.txtHomePage.Text.Replace("'", "''"); // 홈페이지
                xParams[9] = this.txtBusi.Text.Replace("'", "''"); // 업태
                xParams[10] = this.txtCompanyType.Text.Replace("'", "''"); // 종목
                xParams[11] = this.txtZipCode.Text.Replace("'", "''"); // 우편번호
                xParams[12] = this.txtAddr1.Text.Replace("'", "''") + " | " + this.txtAddr2.Text.Replace("'", "''"); // 주소
                xParams[13] = this.txtPhone.Text.Replace("'", "''"); // 전화번호
                xParams[14] = this.txtFax.Text.Replace("'", "''"); // 팩스번호
                xParams[15] = Session["USER_ID"].ToString();  // 사용자 ID
                xParams[16] = this.ddlCompanyKind.SelectedItem.Value; // 회사구분
                xParams[17] = this.txtCompanyEngName.Text.Replace("'", "''"); // 회사명(영문)
                
                if (ViewState["EDITMODE"].ToString() == "NEW")  // EDIT Mode가 아니면 INSERT
                {
                    //2014.03.19 seojw
                    //회사코드 중복 체크
                    DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_company_md",
                                                  "GetCompanyDup",
                                                  LMS_SYSTEM.MANAGE,
                                                  "CLT.WEB.UI.LMS.MANAGE", (object)xParams);
                    
                    //동일한 회사코드가 있을 경우
                    if (xDt.Rows[0]["CNT"].ToString() != "0")
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A014", new string[] { "회사코드" }, new string[] { "Company Code" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_company_md",
                                 "SetCompanyInfo",
                                 LMS_SYSTEM.MANAGE,
                                 "CLT.WEB.UI.LMS.MANAGE",
                                 (object)xParams);
                }
                else
                {
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_company_md",
                                 "SetCompanyInfoUpdate",
                                 LMS_SYSTEM.MANAGE,
                                 "CLT.WEB.UI.LMS.MANAGE",
                                 (object)xParams);
                }

                if (xRtn.ToUpper() == "TRUE")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    //ScriptHelper.ScriptBlock(this, "vp_m_manage_user_reg_wpg", "<script>self.close()</script>");
                    ViewState["EDITMODE"] = "EDIT";
                    ViewState["COMPANY_ID"] = xCompanyID;
                }
                else
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    //ScriptHelper.ScriptBlock(this, "vp_m_manage_user_reg_wpg", "<script>self.close()</script>");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion
        
        /*************************************************************
        * Function name : Edit
        * Purpose       : 회원사 Edit  메서드
        * Input         : void
        * Output        : void
        *************************************************************/
        #region EDIT(string rSeq)
        public void EDIT(string rSeq)
        {
            try
            {
                if (Request.QueryString["COMPANY_ID"] == null)
                    return;

                string[] xParams = new string[1];
                xParams[0] = Request.QueryString["COMPANY_ID"].ToString();

                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_company_md",
                                                  "GetCompanyDetail",
                                                  LMS_SYSTEM.MANAGE,
                                                  "CLT.WEB.UI.LMS.MANAGE", (object)xParams);

                DataRow xDr = xDt.Rows[0];

                this.txtCompanyCode.Text = xDr["company_code"].ToString().ToUpper();
                this.txtCompanyName.Text = xDr["company_nm"].ToString();
                this.txtCompanyEngName.Text = xDr["company_nm_eng"].ToString();
                WebControlHelper.SetSelectItem_DropDownList(this.ddlCompanyKind, xDr["company_kind"].ToString());
                WebControlHelper.SetSelectItem_DropDownList(this.ddlCompanyScale, xDr["company_scale"].ToString());

                //this.txtTex.Text = xDr["tax_no"].ToString();
                this.txtTex1.Text = xDr["tax_no"].ToString().Substring(0, 3);
                this.txtTex2.Text = xDr["tax_no"].ToString().Substring(3, 2);
                this.txtTex3.Text = xDr["tax_no"].ToString().Substring(5, 5);

                this.txtRegNo.Text = xDr["reg_no"].ToString();
                this.txtEmpoly_Ins.Text = xDr["empoly_ins_no"].ToString();
                this.txtCEOName.Text = xDr["company_ceo"].ToString();
                this.txtHomePage.Text = xDr["home_add"].ToString();
                this.txtBusi.Text = xDr["busi_conditions"].ToString();
                this.txtCompanyType.Text = xDr["company_type"].ToString();
                this.txtPhone.Text = xDr["tel_no"].ToString();
                this.txtFax.Text = xDr["fax_no"].ToString();
                this.txtZipCode.Text = xDr["zip_code"].ToString();

                string xAddr = xDr["company_addr"].ToString();
                string[] xAddrs = xAddr.Split('|');

                int xCnount = 0;
                foreach (string address in xAddrs)
                {
                    if (xCnount == 0)
                        this.txtAddr1.Text = address.Trim();
                    else if (xCnount == 1)
                        this.txtAddr2.Text = address.Trim();
                    xCnount++;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion
        
        /*************************************************************
        * Function name : Rewrite
        * Purpose       : 회원사 Rewrite  메서드
        * Input         : void
        * Output        : void
        *************************************************************/
        #region Rewrite()
        public void Rewrite()
        {
            try
            {
                this.txtCompanyCode.Text = string.Empty;
                this.txtCompanyName.Text = string.Empty;
                this.txtCompanyEngName.Text = string.Empty;
                WebControlHelper.SetSelectItem_DropDownList(this.ddlCompanyKind, "*");
                WebControlHelper.SetSelectItem_DropDownList(this.ddlCompanyScale, "*");
                this.txtTex1.Text = string.Empty;
                this.txtTex2.Text = string.Empty;
                this.txtTex3.Text = string.Empty;
                this.txtRegNo.Text = string.Empty;
                this.txtEmpoly_Ins.Text = string.Empty;
                this.txtCEOName.Text = string.Empty;
                this.txtHomePage.Text = string.Empty;
                this.txtBusi.Text = string.Empty;
                this.txtCompanyType.Text = string.Empty;
                this.txtPhone.Text = string.Empty;
                this.txtFax.Text = string.Empty;
                this.txtZipCode.Text = string.Empty;
                this.txtAddr1.Text = string.Empty;
                this.txtAddr2.Text = string.Empty;

                this.txtTex1.ReadOnly = false;
                this.txtTex2.ReadOnly = false;
                this.txtTex3.ReadOnly = false;
                this.txtCompanyCode.ReadOnly = false;
                this.ddlCompanyKind.Enabled = true;

                ViewState["EDITMODE"] = "NEW";
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
