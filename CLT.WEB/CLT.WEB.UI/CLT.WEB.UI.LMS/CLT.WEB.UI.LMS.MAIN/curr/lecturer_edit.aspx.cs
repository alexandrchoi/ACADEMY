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

namespace CLT.WEB.UI.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : 강사 입력,수정화면 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 강사 입력,수정화면
    ///				  
    /// 3. Class 명 : lecturer_edit
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    /// 
    public partial class lecturer_edit : BasePage
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
                    ScriptHelper.ScriptBlock(this, "lecturer_edit", xScriptMsg);

                    return;
                }

                if (Request.QueryString["EDITMODE"] == null)  // 받는값이 없으면 창종료...
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A102", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    ScriptHelper.ScriptBlock(this, "lecturer_edit", "<script>self.close()</script>");
                    return;
                }

                if (!IsPostBack)
                {
                    //this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                    this.txtZipCode.Attributes.Add("ReadOnly", "ReadOnly");
                    BindDropDownList();
                    ViewState["EDITMODE"] = Request.QueryString["EDITMODE"];

                    if (Request.QueryString["LECTURER_ID"] != null)
                        ViewState["LECTURER_ID"] = Request.QueryString["LECTURER_ID"].ToString();

                    if (Request.QueryString["EDITMODE"] != null)
                    {
                        if (Request.QueryString["LECTURER_ID"] == null)
                            return;

                        if (ViewState["EDITMODE"].ToString() == "EDIT") // 수정이면
                        {
                            EDIT(ViewState["EDITMODE"].ToString());

                            this.hidLecturerID.Value = ViewState["LECTURER_ID"].ToString();
                        }

                    }
                    base.pRender(this.Page,
                                 new object[,] { //{ this.btnCancle, "I" },
                                             //{ this.btnRewrite, "I" },
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

                this.ddlGrade.Items.Add(new ListItem("", ""));
                this.ddlGrade.Items.Add(new ListItem("A", "A"));
                this.ddlGrade.Items.Add(new ListItem("B", "B"));
                this.ddlGrade.Items.Add(new ListItem("C", "C"));
                this.ddlGrade.Items.Add(new ListItem("D", "D"));

                // 직책(직급)코드 Dutystep
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                 "GetDutystepCodeInfo",
                                 LMS_SYSTEM.MANAGE,
                                 "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlDutyStep, xDt, "step_name", "duty_step");
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
                /*
                if (string.IsNullOrEmpty(this.txtLecturerNm.Text))
                {
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
                */
                string[] xTempParams = new string[2];
                string xLecturerID = string.Empty;

                if (ViewState["EDITMODE"].ToString() == "NEW")  // LECTURER_ID가 Null 이면 Insert
                {
                    // lecturer_id 처리
                    xTempParams[0] = "t_lecturer";
                    xTempParams[1] = "lecturer_id";

                    xLecturerID = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                    "GetMaxIDOfTable",
                                                     LMS_SYSTEM.CURRICULUM,
                                                    "CLT.WEB.UI.LMS.CURR", (object)xTempParams);
                }
                else
                {
                    xLecturerID = ViewState["LECTURER_ID"].ToString();  //Request.QueryString["LECTURER_ID"].ToString().ToUpper();
                }

                string[] xParams = new string[21];
                xParams[0] = xLecturerID;  // 
                xParams[1] = txtLecturerNm.Text.Replace("'", "''"); //
                xParams[2] = txtLecturerEngNm.Text.Replace("'", "''"); //
                xParams[3] = txtBirthDt.Text.Replace(".", "").Trim() == string.Empty ? null : txtBirthDt.Text; // 사용자 입사일자 enter_dt
                xParams[4] = ddlGrade.SelectedItem.Value; // 
                xParams[5] = ""; // user_id
                xParams[6] = txtJob.Text.Replace("'", "''"); // 
                xParams[7] = txtEducation.Text.Replace("'", "''"); // 
                xParams[8] = txtMajor.Text.Replace("'", "''"); // 
                xParams[9] = txtOrgNm.Text.Replace("'", "''"); // 
                xParams[10] = txtCompanyNm.Text.Replace("'", "''"); // 
                xParams[11] = ddlDutyStep.SelectedItem.Value; // 
                xParams[12] = txtZipCode.Text.Replace("'", "''"); // 우편번호
                xParams[13] = txtAddr1.Text.Replace("'", "''") + " | " + txtAddr2.Text.Replace("'", "''"); // 주소
                xParams[14] = txtPhone.Text.Replace("'", "''"); // 전화번호
                xParams[15] = txtMobile.Text.Replace("'", "''"); // 
                xParams[16] = txtEmail.Text.Replace("'", "''"); // 
                xParams[17] = txtBank.Text.Replace("'", "''"); // 
                xParams[18] = txtAccount.Text.Replace("'", "''"); // 
                xParams[19] = "1"; // 상태
                xParams[20] = Session["USER_ID"].ToString();  // 사용자 ID

                string xFileNameRes = "";
                byte[] xFileByteRes = null;
                string xFileNameDoc = "";
                byte[] xFileByteDoc = null;

                FileUpload file = this.FileUpload1;// ((FileUpload)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("fileUplaod"));
                if (file.FileBytes.Length > 0)
                {
                    xFileNameRes = file.FileName.Replace(" ", "_").Replace("..", "_");
                    xFileByteRes = file.FileBytes;
                }

                file = this.FileUpload2;
                if (file.FileBytes.Length > 0)
                {
                    xFileNameDoc = file.FileName.Replace(" ", "_").Replace("..", "_");
                    xFileByteDoc = file.FileBytes;
                }

                if (ViewState["EDITMODE"].ToString() == "NEW")  // EDIT Mode가 아니면 INSERT
                {
                    //강사코드 중복 체크
                    DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_lecturer_md",
                                                     "GetLecturerDup",
                                                      LMS_SYSTEM.CURRICULUM,
                                                     "CLT.WEB.UI.LMS.CURR", (object)xParams);
                    
                    //동일한 코드가 있을 경우
                    if (xDt.Rows[0]["CNT"].ToString() != "0")
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A014", new string[] { "강사ID" }, new string[] { "Lecturer ID" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_lecturer_md",
                                 "SetLecturerInfo",
                                 LMS_SYSTEM.CURRICULUM,
                                 "CLT.WEB.UI.LMS.CURR",
                                 (object)xParams, xFileByteRes, xFileNameRes, xFileByteDoc, xFileNameDoc);

                }
                else
                {
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_lecturer_md",
                                 "SetLecturerInfoUpdate",
                                 LMS_SYSTEM.CURRICULUM,
                                 "CLT.WEB.UI.LMS.CURR",
                                 (object)xParams, xFileByteRes, xFileNameRes, xFileByteDoc, xFileNameDoc);
                }

                if (xRtn.ToUpper() == "TRUE")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    //ScriptHelper.ScriptBlock(this, "vp_m_manage_user_reg_wpg", "<script>self.close()</script>");
                    ViewState["EDITMODE"] = "EDIT";
                    ViewState["LECTURER_ID"] = xLecturerID;
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
                if (Request.QueryString["LECTURER_ID"] == null)
                    return;

                string[] xParams = new string[1];
                xParams[0] = Request.QueryString["LECTURER_ID"].ToString();

                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_lecturer_md",
                                                  "GetLecturerInfo",
                                                  LMS_SYSTEM.CURRICULUM,
                                                  "CLT.WEB.UI.LMS.CURR", (object)xParams);
                DataRow xDr = xDt.Rows[0];

                this.txtLecturerNm.Text = xDr["lecturer_nm"].ToString().ToUpper();
                this.txtLecturerEngNm.Text = xDr["lecturer_nm_eng"].ToString();
                this.txtBirthDt.Text = xDr["birth_date"].ToString();

                WebControlHelper.SetSelectItem_DropDownList(this.ddlGrade, xDr["grade"].ToString());
                WebControlHelper.SetSelectItem_DropDownList(this.ddlDutyStep, xDr["duty_step"].ToString());
                
                this.txtJob.Text = xDr["job"].ToString();
                this.txtOrgNm.Text = xDr["org_nm"].ToString();
                this.txtEducation.Text = xDr["education"].ToString();
                this.txtMajor.Text = xDr["major"].ToString();
                this.txtCompanyNm.Text = xDr["company_nm"].ToString();
                this.txtPhone.Text = xDr["tel_no"].ToString();
                this.txtMobile.Text = xDr["mobile_phone"].ToString();
                this.txtEmail.Text = xDr["email"].ToString();
                this.txtBank.Text = xDr["acc_bank"].ToString();
                this.txtAccount.Text = xDr["account"].ToString();
                
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

    }
}
