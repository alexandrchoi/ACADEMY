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
using System.Drawing;

namespace CLT.WEB.UI.LMS.MANAGE
{
    /// <summary>
    /// 1. 작업개요 : 사용자 정보관리 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 사용자 정보관리
    ///				  
    /// 3. Class 명 : user_edit
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    /// 
    public partial class user_edit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "user_edit", xScriptMsg);

                    return;
                }

                if (Request.QueryString["EDITMODE"] == null)
                    return;
                
                if (!IsPostBack)
                {
                    //this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                    //this.txtAcquisition.Attributes.Add("onkeyup", "ChkDate(this);"); //고용보험취득일 체크

                    // ReadOnly 일때 Postback 시 값이 없어지는것을 방지...
                    this.txtZipCode.Attributes.Add("ReadOnly", "ReadOnly");
                    this.txtCompany.Attributes.Add("ReadOnly", "ReadOnly");
                    this.txtPersonal_no2.Text = txtPersonal_no2.UniqueID;
                    
                    ViewState["EDITMODE"] = Request.QueryString["EDITMODE"].ToString();

                    if (Request.QueryString["USER_ID"] != null)
                        ViewState["USER_ID"] = Request.QueryString["USER_ID"].ToString();
                    
                    this.rbSMS_y.Checked = true;
                    this.rbSMS_n.Checked = false;

                    this.rbMail_y.Checked = true;
                    this.rbMail_n.Checked = false;
                    
                    BindDropDownList();
                    
                    base.pRender(this.Page,
                                 new object[,] { //{ this.btnCancle, "I" }, 
                                             //{ this.btnIDcheck, "I" }, 
                                             { this.btnRewrite, "I" },
                                             { this.btnSave, "E" },
                                           },
                                 Convert.ToString(Request.QueryString["MenuCode"]));

                    // 수정 모드일때에는 USER_ID 도 같이 넘긴다.
                    if (ViewState["EDITMODE"].ToString() == "EDIT")
                    {
                        EditMode(Request.QueryString["USER_ID"].ToString());  // 사용자 정보 수정일 때에는 수정 하고자 하는 ID값을 넘긴다.

                        if (Session["USER_GROUP"].ToString() != "000001") // 관리자가 아니면
                        {
                            //WebControlHelper.SetSelectItem_DropDownList(this.ddlUserGroup, Session["USER_GROUP"].ToString());
                            this.ddlUserGroup.Enabled = false;
                            //this.btnComapny.Visible = false;
                        }
                    }
                    else if (ViewState["EDITMODE"].ToString() == "NEW")
                    {
                        if (Session["USER_GROUP"].ToString() != "000001") // 관리자가 아니면
                        {
                            WebControlHelper.SetSelectItem_DropDownList(this.ddlUserGroup, "000008");
                            this.ddlUserGroup.Enabled = false;

                            if (Session["USER_GROUP"].ToString() == "000007")  // 법인사 관리자는 법인사 수강자만 입력가능...
                            {
                                txtID.ReadOnly = true;
                                txtID.BackColor = Color.FromName("#dcdcdc");
                                btnIDcheck.Enabled = false;

                                txtPass.ReadOnly = true;
                                txtPassCheck.ReadOnly = true;

                                txtPass.BackColor = Color.FromName("#dcdcdc");
                                txtPassCheck.BackColor = Color.FromName("#dcdcdc");
                            }

                            string[] xParams = new string[1];
                            xParams[0] = Session["COMPANY_ID"].ToString();
                            DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                                  "GetCompanyName",
                                                  LMS_SYSTEM.MANAGE,
                                                  "CLT.WEB.UI.LMS.MANAGE", (object)xParams);

                            hidCompany_id.Value = Session["COMPANY_ID"].ToString();
                            this.txtCompany.Text = xDt.Rows[0]["company_nm"].ToString();

                            //this.btnComapny.Visible = false;
                        }
                    }
                }
                ddlUserGroup_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : BindDropDownList
        * Purpose       : DropDownList 데이터 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        private void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[2];
                string xSql = string.Empty;
                DataTable xDt = null;
                
                if (Session["USER_GROUP"].ToString() == "000001" || Session["USER_GROUP"].ToString() == "000007") // 관리자 일경우
                {
                    // 직급(직위) 해당 직급은 법인사 관리자에게만 해당되며, 인사시스템에서 사용하는 직급이아님!(사장, 과장, 부장 등...)
                    xParams[0] = "0023";
                    xParams[1] = "Y";
                    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                 "GetCommonCodeInfo",
                                                 LMS_SYSTEM.MANAGE,
                                                 "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                    xParams[1] = "Y";
                    DataTable xDtDutyStep = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                     "GetDutystepCodeInfo",
                                     LMS_SYSTEM.MANAGE,
                                     "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);
                    DataRow xDr = null;

                    foreach (DataRow xDrs in xDtDutyStep.Rows)
                    {
                        xDr = xDt.NewRow();

                        xDr["D_CD"] = xDrs["duty_step"].ToString();
                        xDr["D_KNM"] = xDrs["step_name"].ToString();
                        xDt.Rows.Add(xDr);
                    }

                    WebControlHelper.SetDropDownList(this.ddlComapnyduty, xDt);
                }
                else
                {
                    // 직책(직급)코드 Dutystep
                    xParams[1] = "Y";
                    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                     "GetDutystepCodeInfo",
                                     LMS_SYSTEM.MANAGE,
                                     "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);
                    WebControlHelper.SetDropDownList(this.ddlComapnyduty, xDt, "step_name", "duty_step");
                }

                // 사용자그룹 (관리자, 강사 등..)
                xParams[0] = "0041";
                xParams[1] = "Y";
                xDt = null;
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.MANAGE,
                                             "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlUserGroup, xDt);

                //훈련생구분 
                xParams[0] = "0062";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.MANAGE,
                                             "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlTrainee, xDt);



                xParams = new string[2];
                xParams[0] = "";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                     "GetVHDeptCode",
                                     LMS_SYSTEM.MANAGE,
                                     "CLT.WEB.UI.LMS.MANAGE",
                                     xParams,
                                     " ORDER BY dept_name ");
                WebControlHelper.SetDropDownList(ddlDept, xDt, "dept_name", "dept_code", WebControlHelper.ComboType.NullAble);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        
        /************************************************************
        * Function name : EditMode
        * Purpose       : DataTable의 Edit 모드 처리...
        * Input         : String rUser_ID
        * Output        : void
        *************************************************************/
        private void EditMode(string rUser_ID)
        {
            try
            {
                string[] xParams = new string[1];
                xParams[0] = rUser_ID;

                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                                  "GetUserEdit",
                                                  LMS_SYSTEM.MANAGE,
                                                  "CLT.WEB.UI.LMS.MANAGE", (object)xParams);
                if (xDt.Rows.Count == 0)
                    return;

                DataRow xDr = xDt.Rows[0];
                
                //this.txtID.Text = xDr["user_id"].ToString().ToLower();
                this.txtID.Text = xDr["user_id"].ToString();
                this.txtID.Enabled = false; // ID는 수정 불가능
                this.btnIDcheck.Enabled = false;
                
                this.txtuser_nm_kor.Text = xDr["user_nm_kor"].ToString();
                this.txtuser_nm_eng_first.Text = xDr["user_nm_eng_first"].ToString();
                this.txtuser_nm_eng_last.Text = xDr["user_nm_eng_last"].ToString();
                WebControlHelper.SetSelectItem_DropDownList(this.ddlComapnyduty, xDr["duty_step"].ToString());

                string xPersonalNo = xDr["personal_no"].ToString();
                int xCount = 0;
                
                foreach (string xPersnals in xPersonalNo.Split('-'))
                {
                    if (xCount == 0)
                        this.txtPersonal_no1.Text = xPersnals.Trim();
                    else if (xCount == 1)
                        this.txtPersonal_no2.Attributes.Add("Value", xPersnals);

                    xCount++;
                }

                txtEmail.Text = xDr["email_id"].ToString();
                txtPhone.Text = xDr["office_phone"].ToString();
                txtMobilePhone.Text = xDr["mobile_phone"].ToString();
                hidCompany_id.Value = xDr["company_id"].ToString();
                txtCompany.Text = xDr["company_nm"].ToString();
                hidCompany_id.Value = xDr["company_id"].ToString();
                txtZipCode.Text = xDr["user_zip_code"].ToString();

                WebControlHelper.SetSelectItem_DropDownList(this.ddlUserGroup, xDr["user_group"].ToString());
                //this.ddlUserGroup.Enabled = false;

                string[] xAddr = xDr["user_addr"].ToString().Split('|');

                xCount = 0;
                foreach (string xAddrs in xAddr)
                {
                    if (xCount == 0)
                        this.txtAddr1.Text = xAddrs.Trim();
                    else
                        this.txtAddr2.Text = xAddrs.Trim();

                    xCount++;
                }

                if (xDr["sms_yn"].ToString() == "Y")
                {
                    this.rbSMS_y.Checked = true;
                    this.rbSMS_n.Checked = false;
                }
                else
                {
                    this.rbSMS_y.Checked = false;
                    this.rbSMS_n.Checked = true;
                }

                if (xDr["mail_yn"].ToString() == "Y")
                {
                    this.rbMail_y.Checked = true;
                    this.rbMail_n.Checked = false;
                }
                else
                {
                    this.rbMail_y.Checked = false;
                    this.rbMail_n.Checked = true;
                }

                WebControlHelper.SetSelectItem_DropDownList(this.ddlTrainee, xDr["trainee_class"].ToString());
                this.txtAcquisition.Text = xDr["enter_dt"].ToString();
                this.txtBirth_dt.Text = xDr["birth_dt"].ToString();


                WebControlHelper.SetSelectItem_DropDownList(ddlDept, xDr["dept_code"].ToString());
                txtDept.Text = xDr["dept_name"].ToString();

                if (xDr["pic_file"] != DBNull.Value && !string.IsNullOrEmpty(xDr["pic_file"].ToString()))
                {
                    try
                    {
                        int nFind = xDr["pic_file_nm"].ToString().IndexOf(".");
                        if (nFind > 0)
                        {
                            string mimeType = "image/" + xDr["pic_file_nm"].ToString().Substring(nFind+1);
                            string base64 = Convert.ToBase64String((byte[])xDr["pic_file"]);

                            img_pic_file.ImageUrl = string.Format("data:{0};base64,{1}", mimeType, base64);

                            Unit xUnit = img_pic_file.Width;

                            if (xUnit.Value > 150) img_pic_file.Width = 150;
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        // ID 중복체크
        protected void btnIDCheck_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (this.txtID.Enabled == false)
                    return;

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

        /*2013.06.28 [CHM-201325393] LMS 기능변경 요청전 ...*/
        /*
        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlUserGroup.SelectedItem.Value != "000008") // 법인사 수강자가 아니면 ID, 비밀번호 체크
                {
                    if (ViewState["EDITMODE"] == "NEW")
                    {
                        if (this.txtID.Enabled == true)
                        {
                            //xScriptMsg = "<script>alert('ID 중복체크 버튼을 클릭하여 주세요!');</script>";
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A104", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                            this.txtID.Focus();
                            return;
                        }
                    }
                    if (string.IsNullOrEmpty(this.txtPass.Text))
                    {
                        //xScriptMsg = "<script>alert('비밀번호를 입력해 주세요!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A105", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                        this.txtPass.Focus();
                        return;
                    }
                    else if (string.IsNullOrEmpty(this.txtPassCheck.Text))
                    {
                        //xScriptMsg = "<script>alert('비밀번호 확인란을 입력해 주세요!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "비밀번호" }, new string[] { "Password" }, Thread.CurrentThread.CurrentCulture));
                        this.txtPassCheck.Focus();
                        return;
                    }
                    else if (this.txtPass.Text != this.txtPassCheck.Text)
                    {
                        //xScriptMsg = "<script>alert('입력하신 비밀번호가 다릅니다!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A016", new string[] { "비밀번호" }, new string[] { "Password" }, Thread.CurrentThread.CurrentCulture));
                        this.txtPassCheck.Focus();
                        return;
                    }
                }
                else
                {
                    if (ViewState["EDITMODE"] == "EDIT")
                    {
                        if (string.IsNullOrEmpty(this.txtPass.Text))
                        {
                            //xScriptMsg = "<script>alert('비밀번호를 입력해 주세요!');</script>";
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A105", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                            this.txtPass.Focus();
                            return;
                        }
                        else if (string.IsNullOrEmpty(this.txtPassCheck.Text))
                        {
                            //xScriptMsg = "<script>alert('비밀번호 확인란을 입력해 주세요!');</script>";
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "비밀번호" }, new string[] { "Password" }, Thread.CurrentThread.CurrentCulture));
                            this.txtPassCheck.Focus();
                            return;
                        }
                        else if (this.txtPass.Text != this.txtPassCheck.Text)
                        {
                            //xScriptMsg = "<script>alert('입력하신 비밀번호가 다릅니다!');</script>";
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A016", new string[] { "비밀번호" }, new string[] { "Password" }, Thread.CurrentThread.CurrentCulture));
                            this.txtPassCheck.Focus();
                            return;
                        }
                    }
                }
                if (string.IsNullOrEmpty(this.txtZipCode.Text))
                {
                    //xScriptMsg = "<script>alert('우편번호를 검색하여 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "우편번호" }, new string[] { "Zip Code" }, Thread.CurrentThread.CurrentCulture));
                    this.txtZipCode.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtAddr1.Text))
                {
                    //xScriptMsg = "<script>alert('주소를 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "주소" }, new string[] { "Address" }, Thread.CurrentThread.CurrentCulture));
                    this.txtAddr1.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtAddr2.Text))
                {
                    //xScriptMsg = "<script>alert('상세주소를 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "주소" }, new string[] { "Address" }, Thread.CurrentThread.CurrentCulture));
                    this.txtAddr2.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtuser_nm_kor.Text))
                {
                    //xScriptMsg = "<script>alert('이름을 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "이름" }, new string[] { "Name" }, Thread.CurrentThread.CurrentCulture));
                    this.txtuser_nm_kor.Focus();
                    return;
                }
                //else if (string.IsNullOrEmpty(this.txtuser_nm_eng_first.Text))
                //{
                //    //xScriptMsg = "<script>alert('이름영문(First)을 입력해 주세요!');</script>";
                //    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "이름영문(First)" }, new string[] { "First Name" }, Thread.CurrentThread.CurrentCulture));
                //    this.txtuser_nm_eng_first.Focus();
                //    return; 
                //}
                //else if (string.IsNullOrEmpty(this.txtuser_nm_eng_last.Text))
                //{
                //    //xScriptMsg = "<script>alert('이름영문(Last)을 입력해 주세요!');</script>";
                //    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "이름영문(Last)" }, new string[] { "Last Name" }, Thread.CurrentThread.CurrentCulture));
                //    this.txtuser_nm_eng_last.Focus();
                //    return;
                //}
                else if (string.IsNullOrEmpty(txtCompany.Text))
                {
                    //xScriptMsg = "<script>alert('회사를 선택해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "회사" }, new string[] { "Company" }, Thread.CurrentThread.CurrentCulture));
                    this.txtCompany.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtPhone.Text))
                {
                    //xScriptMsg = "<script>alert('전화번호를 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "전화번호" }, new string[] { "Phone Number" }, Thread.CurrentThread.CurrentCulture));
                    this.txtPhone.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtMobilePhone.Text))
                {
                    //xScriptMsg = "<script>alert('휴대전화번호를 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "휴대폰번호" }, new string[] { "Mobile Phone Number" }, Thread.CurrentThread.CurrentCulture));
                    this.txtMobilePhone.Focus();
                    return;
                }
                else if (this.ddlComapnyduty.SelectedItem.Text == "*")
                {
                    //xScriptContent = "<script>alert('직급을 선택해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "직급" }, new string[] { "Rank" }, Thread.CurrentThread.CurrentCulture));
                    this.ddlComapnyduty.Focus();
                    return;
                }
                else if (this.ddlUserGroup.SelectedItem.Text == "*")
                {
                    //xScriptContent = "<script>alert('사용자그룹을 선택해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "사용자 그룹" }, new string[] { "User Group" }, Thread.CurrentThread.CurrentCulture));
                    this.ddlUserGroup.Focus();
                    return;
                }
                //자사근로자이면 고용보험취득일 필수 입력!!
                else if (this.ddlTrainee.SelectedItem.Value == "000001" && this.txtAcquisition.Text.Replace(".", "").Trim() == string.Empty)
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "고용보험취득일" }, new string[] { "Acquisition Date" }, Thread.CurrentThread.CurrentCulture));
                    this.txtAcquisition.Focus();
                    return;
                }
                if (this.ddlUserGroup.SelectedValue != "000007")
                {
                    if (string.IsNullOrEmpty(this.txtPersonal_no1.Text))
                    {
                        //xScriptMsg = "<script>alert('주민등록번호를 입력해 주세요!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "주민등록번호" }, new string[] { "Personal Number" }, Thread.CurrentThread.CurrentCulture));
                        this.txtPersonal_no1.Focus();
                        return;
                    }
                    else if (string.IsNullOrEmpty(this.txtPersonal_no2.Text))
                    {
                        //xScriptMsg = "<script>alert('주민등록번호를 입력해 주세요!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "주민등록번호" }, new string[] { "Personal Number" }, Thread.CurrentThread.CurrentCulture));
                        this.txtPersonal_no2.Focus();
                        return;
                    }
                }
                //if (Request.QueryString["EDITMODE"] != null && Request.QueryString["EDITMODE"] == "EDIT")
                if (ViewState["EDITMODE"].ToString() == "EDIT")
                    UpdateSave();
                else
                    InsertSave();

                //string xScriptMsg = "<script>opener.__doPostBack('ctl00_ContentPlaceHolderMainUp_btnRetrieve','');self.close();</script>";
                //ScriptHelper.ScriptBlock(this, "user_edit", xScriptMsg);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        */

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() != "000007")
                {
                    if (ViewState["EDITMODE"] == "NEW")
                    {
                        if (this.txtID.Enabled == true)
                        {
                            //xScriptMsg = "<script>alert('ID 중복체크 버튼을 클릭하여 주세요!');</script>";
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A104", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                            this.txtID.Focus();
                            return;
                        }
                    }
                }
                
                //앞단(aspx)에서 이미 모두 체크
                /*
                // 법인사 수강자일 경우 비밀번호, 주민번호 필수
                if (this.ddlUserGroup.SelectedItem.Value == "000008") 
                {
                    if (string.IsNullOrEmpty(this.txtPass.Text))
                    {
                        //xScriptMsg = "<script>alert('비밀번호를 입력해 주세요!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A105", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                        this.txtPass.Focus();
                        return;
                    }
                    else if (string.IsNullOrEmpty(this.txtPassCheck.Text))
                    {
                        //xScriptMsg = "<script>alert('비밀번호 확인란을 입력해 주세요!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "비밀번호" }, new string[] { "Password" }, Thread.CurrentThread.CurrentCulture));
                        this.txtPassCheck.Focus();
                        return;
                    }
                    else if (this.txtPass.Text != this.txtPassCheck.Text)
                    {
                        //xScriptMsg = "<script>alert('입력하신 비밀번호가 다릅니다!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A016", new string[] { "비밀번호" }, new string[] { "Password" }, Thread.CurrentThread.CurrentCulture));
                        this.txtPassCheck.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtPersonal_no1.Text))
                    {
                        //xScriptMsg = "<script>alert('주민등록번호를 입력해 주세요!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "주민등록번호" }, new string[] { "Personal Number" }, Thread.CurrentThread.CurrentCulture));
                        this.txtPersonal_no1.Focus();
                        return;
                    }
                    else if (string.IsNullOrEmpty(this.txtPersonal_no2.Text))
                    {
                        //xScriptMsg = "<script>alert('주민등록번호를 입력해 주세요!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "주민등록번호" }, new string[] { "Personal Number" }, Thread.CurrentThread.CurrentCulture));
                        this.txtPersonal_no2.Focus();
                        return;
                    }
                }
                //법인사 관리자가 비밀번호를 변경하려고 할때 비밀번호와 비밀번호 확인이 서로 틀리다면
                else if (this.ddlUserGroup.SelectedItem.Value == "000007")
                {
                    if (this.txtPass.Text != this.txtPassCheck.Text)
                    {
                        //xScriptMsg = "<script>alert('입력하신 비밀번호가 다릅니다!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A016", new string[] { "비밀번호" }, new string[] { "Password" }, Thread.CurrentThread.CurrentCulture));
                        this.txtPassCheck.Focus();
                        return;
                    }
                }
                
                if (string.IsNullOrEmpty(this.txtuser_nm_kor.Text))
                {
                    //xScriptMsg = "<script>alert('이름을 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "이름" }, new string[] { "Name" }, Thread.CurrentThread.CurrentCulture));
                    this.txtuser_nm_kor.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtCompany.Text))
                {
                    //xScriptMsg = "<script>alert('회사를 선택해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "회사" }, new string[] { "Company" }, Thread.CurrentThread.CurrentCulture));
                    this.txtCompany.Focus();
                    return;
                }
                else if (this.ddlComapnyduty.SelectedItem.Text == "*")
                {
                    //xScriptContent = "<script>alert('직급을 선택해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "직급" }, new string[] { "Rank" }, Thread.CurrentThread.CurrentCulture));
                    this.ddlComapnyduty.Focus();
                    return;
                }
                else if (this.ddlUserGroup.SelectedItem.Text == "*")
                {
                    //xScriptContent = "<script>alert('사용자그룹을 선택해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "사용자 그룹" }, new string[] { "User Group" }, Thread.CurrentThread.CurrentCulture));
                    this.ddlUserGroup.Focus();
                    return;
                }
                //자사근로자이면 고용보험취득일 필수 입력!!
                else if (this.ddlTrainee.SelectedItem.Value == "000001" && this.txtAcquisition.Text.Replace(".", "").Trim() == string.Empty)
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "고용보험취득일" }, new string[] { "Acquisition Date" }, Thread.CurrentThread.CurrentCulture));
                    this.txtAcquisition.Focus();
                    return;
                }
                */
                //if (Request.QueryString["EDITMODE"] != null && Request.QueryString["EDITMODE"] == "EDIT")
                if (ViewState["EDITMODE"].ToString() == "EDIT")
                {
                    UpdateSave();

                    bSaveFile();
                }
                else
                {
                    if (this.ddlUserGroup.SelectedItem.Value == "000008")
                    {
                        string[] xUserParams = new string[2];
                        xUserParams[0] = txtPersonal_no1.Text.Replace("'", "''") + "-" + txtPersonal_no2.Text.Replace("'", "''");
                        xUserParams[1] = hidCompany_id.Value;

                        //2014.03.20 seojw(문서영 대리 요청)
                        //법인사 수강자인 경우 주민번호로 기등록된 데이터 있는지 검색
                        //만약 데이터 있다면 insert가 아닌 update 처리 
                        DataTable xDtUserDup = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                                       "GetUserDup",
                                                       LMS_SYSTEM.MANAGE,
                                                       "CLT.WEB.UI.LMS.MANAGE",
                                                       (object)xUserParams);

                        if (xDtUserDup.Rows.Count != 0)
                        {
                            txtID.Text = xDtUserDup.Rows[0]["USER_ID"].ToString();

                            UpdateSave();
                            
                            bSaveFile();
                            return;
                        }
                    }
                    InsertSave();

                    bSaveFile();
                }

                //string xScriptMsg = "<script>opener.__doPostBack('ctl00_ContentPlaceHolderMainUp_btnRetrieve','');self.close();</script>";
                //ScriptHelper.ScriptBlock(this, "user_edit", xScriptMsg);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        public void UpdateSave()
        {
            try
            {
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;
                string[] xParams = new string[23];

                //VirtualAgentClass xx = new VirtualAgentClass();

                xParams[0] = txtID.Text.ToLower().Replace("'", "''");

                xParams[1] = OpusCryptoLibrary.Cryptography.Encrypt(this.txtPass.Text, this.txtPass.Text);

                //xParams[1] = (string)xx.Encrypt(txtPass.Text.Replace("'", "''"));
                xParams[2] = txtuser_nm_kor.Text.Replace("'", "''");
                xParams[3] = txtuser_nm_eng_first.Text.Replace("'", "''");
                xParams[4] = txtuser_nm_eng_last.Text.Replace("'", "''");
                xParams[5] = ddlComapnyduty.SelectedValue.ToString();
                xParams[6] = txtPersonal_no1.Text.Replace("'", "''") + "-" + txtPersonal_no2.Text.Replace("'", "''");
                xParams[7] = txtEmail.Text.Replace("'", "''");
                xParams[8] = txtPhone.Text.Replace("'", "''");
                xParams[9] = txtMobilePhone.Text.Replace("'", "''");
                xParams[10] = txtCompany.Text.Replace("'", "''");
                xParams[11] = hidCompany_id.Value.Replace("'", "''");
                xParams[12] = txtZipCode.Text.Replace("'", "''");
                xParams[13] = txtAddr1.Text.Replace("'", "''") + " | " + txtAddr2.Text.Replace("'", "''");

                if (rbSMS_y.Checked == true)
                    xParams[14] = "Y"; // SMS 수신여부
                else
                    xParams[14] = "N"; // SMS 수신여부

                if (rbMail_y.Checked == true)
                    xParams[15] = "Y"; // MAIL 수신여부
                else
                    xParams[15] = "N"; // MAIL 수신여부

                xParams[16] = Session["USER_ID"].ToString();
                xParams[17] = this.ddlUserGroup.SelectedItem.Value.ToString();
                xParams[18] = this.ddlTrainee.SelectedItem.Value.ToString().Replace("*", ""); //훈련생 구분
                xParams[19] = this.txtAcquisition.Text.Replace(".", "").Trim() == string.Empty ? null : this.txtAcquisition.Text; //고용보험취득일 
                xParams[20] = Session["user_group"].ToString();
                xParams[21] = txtBirth_dt.Text.Replace(".", "").Trim() == string.Empty ? null : txtBirth_dt.Text; //생년월일 
                xParams[22] = string.IsNullOrEmpty(ddlDept.SelectedValue.ToString()) ? txtDept.Text.Replace("'", "''") : ddlDept.SelectedValue.ToString(); // 사용자 부서 dept_code

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                             "SetUserEdit",
                             LMS_SYSTEM.MANAGE,
                             "CLT.WEB.UI.LMS.MANAGE",
                             (object)xParams);
                
                if (xRtn.ToUpper() == "TRUE")
                {

                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
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

        public void InsertSave()
        {
            try
            {
                // 담당자 관련 정보를 가져온다.
                string[] xUserParams = new string[1];
                xUserParams[0] = this.hidCompany_id.Value;

                DataTable xDtUser = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                                     "GetCompanyUserInfo",
                                                     LMS_SYSTEM.MANAGE,
                                                     "CLT.WEB.UI.LMS.MANAGE",
                                                     (object)xUserParams);

                DataRow xDr = xDtUser.Rows[0];
                
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;
                string[] xParams = new string[30];

                //VirtualAgentClass xx = new VirtualAgentClass();

                xParams[0] = txtID.Text.ToLower().Replace("'", "''"); // 사용자 ID  user_id

                /*
                 * 2014.03.24 seojw - 문서영 대리 요청
                 * 법인사 관리자는 비밀번호가 선택한 회사의 사업자 번호 뒤 5자리가 들어간다.
                 * 법인사 수강자는 비밀번호가 주민번호 뒤 7자리가 들어간다.
                 */
                if (this.ddlUserGroup.SelectedItem.Value == "000007")
                {
                    /*
                    DataTable xDtTexNo = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                                     "GetCompanyInfo",
                                                     LMS_SYSTEM.MANAGE,
                                                     "CLT.WEB.UI.LMS.MANAGE.vp_m_manage_user_edit_wpg",
                                                     (object)xUserParams);

                    xParams[1] = (string)xx.Encrypt(xDtTexNo.Rows[0]["TAX_NO_PW"].ToString());//txtPass.Text.Replace("'", "''"); // 비밀번호 pwd (최초등록시에는 사업자번호 뒷자리가 비밀번호로 저장된다.)

                    xDtTexNo.Dispose();
                     * */

                    xParams[1] = OpusCryptoLibrary.Cryptography.Encrypt(xDr["TAX_NO_PW"].ToString(), xDr["TAX_NO_PW"].ToString());

                    //xParams[1] = (string)xx.Encrypt(xDr["TAX_NO_PW"].ToString());//txtPass.Text.Replace("'", "''"); // 비밀번호 pwd (최초등록시에는 주민번호 뒷자리가 비밀번호로 저장된다.)
                }
                else
                {
                    xParams[1] = OpusCryptoLibrary.Cryptography.Encrypt(txtPersonal_no2.Text.Replace("'", "''"), txtPersonal_no2.Text.Replace("'", "''"));

                    //xParams[1] = (string)xx.Encrypt(txtPersonal_no2.Text.Replace("'", "''"));//txtPass.Text.Replace("'", "''"); // 비밀번호 pwd (최초등록시에는 주민번호 뒷자리가 비밀번호로 저장된다.)
                }
                
                xParams[2] = " "; // 사용자 사번 user_no
                xParams[3] = txtuser_nm_kor.Text.Replace("'", "''"); // 사용자 이름 user_nm_kor
                xParams[4] = txtPersonal_no1.Text.Replace("'", "''") + "-" + txtPersonal_no2.Text.Replace("'", "''"); // 사용자 주민번호 personal_no
                xParams[5] = txtuser_nm_eng_first.Text.Replace("'", "''"); // 사용자 이름 user_nm_eng_first
                xParams[6] = txtuser_nm_eng_last.Text.Replace("'", "''");  // 사용자 이름 user_nm_eng_last
                xParams[7] = ddlComapnyduty.SelectedValue.ToString();  // 사용자 직급 duty_step
                xParams[8] = string.IsNullOrEmpty(ddlDept.SelectedValue.ToString()) ? txtDept.Text.Replace("'", "''") : ddlDept.SelectedValue.ToString(); // 사용자 부서 dept_code
                xParams[9] = txtMobilePhone.Text.Replace("'", "''"); // 사용자 휴대폰 번호 mobile_phone
                xParams[10] = " ";  //고용형태 duty_gu
                xParams[11] = txtEmail.Text.Replace("'", "''"); // 사용자 email email_id
                xParams[12] = txtPhone.Text.Replace("'", "''"); // 사용자 전화번호 office_phone


                xParams[13] = xDr["fax_no"].ToString(); // " ";  //사용자 팩스번호
                xParams[14] = " ";  // 승인 담당자 admin_id
                xParams[15] = xDr["tel_no"].ToString(); //" ";  // 업체 전화번호
                xParams[16] = xDr["user_id"].ToString(); //" "; // 업체 담당자  

                if (this.ddlUserGroup.SelectedItem.Value == "000008") // 법인사 수강자를 선택 한 경우 자동 승인처리
                    xParams[17] = "000003";  // 사용자 상태 [승인] status
                else
                    xParams[17] = "000004"; // 나머지는 승인대기
                
                xParams[18] = hidCompany_id.Value; // 사용자 소속사 company_id 
                xParams[19] = this.txtAcquisition.Text.Replace(".", "").Trim() == string.Empty ? null : this.txtAcquisition.Text; // 사용자 입사일자 enter_dt
                xParams[20] = this.ddlUserGroup.SelectedItem.Value; // "000008";  //사용자 그룹 [법인사 수강자] user_group
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

                xParams[26] = Session["USER_ID"].ToString(); //ins_id
                xParams[27] = Session["USER_ID"].ToString(); //upt_id
                xParams[28] = ddlTrainee.SelectedItem.Value.ToString().Replace("*", ""); //trainee_class 
                xParams[29] = txtBirth_dt.Text.Replace(".", "").Trim() == string.Empty ? null : txtBirth_dt.Text; // 생년월일

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                         "SetUser",
                                         LMS_SYSTEM.MANAGE,
                                         "CLT.WEB.UI.LMS.MANAGE",
                                         (object)xParams);
                
                string[] xResult = xRtn.Split('|');
                
                //if (xRtn.ToUpper() == "TRUE")
                if (xResult[0].ToUpper() == "TRUE")
                {

                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    ViewState["EDITMODE"] = "EDIT";
                    this.txtID.Text = xResult[1];
                    ViewState["USER_ID"] = this.txtID.Text.ToLower().Replace("'", "''");
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
        
        protected void btnRewrite_OnClick(object sender, EventArgs e)
        {
            try
            {
                this.txtID.Text = string.Empty;
                this.txtID.Enabled = true;
                this.btnIDcheck.Enabled = true;
                ViewState["EDITMODE"] = "NEW";
                this.txtPass.Text = string.Empty;
                this.txtPassCheck.Text = string.Empty;
                this.txtuser_nm_kor.Text = string.Empty;
                this.txtPersonal_no1.Text = string.Empty;
                this.txtPersonal_no2.Attributes.Add("Value", string.Empty);
                this.txtuser_nm_eng_first.Text = string.Empty;
                this.txtuser_nm_eng_last.Text = string.Empty;
                this.txtZipCode.Text = string.Empty;
                this.txtAddr1.Text = string.Empty;
                this.txtAddr2.Text = string.Empty;
                hidCompany_id.Value = string.Empty;
                this.txtCompany.Text = string.Empty;
                WebControlHelper.SetSelectItem_DropDownList(this.ddlComapnyduty, "*");
                this.txtPhone.Text = string.Empty;
                this.txtEmail.Text = string.Empty;
                this.txtMobilePhone.Text = string.Empty;

                this.rbMail_n.Checked = false;
                this.rbMail_y.Checked = true;

                this.rbSMS_n.Checked = false;
                this.rbSMS_y.Checked = true;

                if (Session["USER_GROUP"].ToString() == "000001")
                {
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlUserGroup, "*");
                    this.ddlUserGroup.Enabled = true;
                }
                else if (Session["USER_GROUP"].ToString() == "000007") // 법인사 관리자
                {
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlUserGroup, "000008");

                    //txtID.ReadOnly = true;
                    //txtID.BackColor = Color.FromName("#dcdcdc");
                    //btnIDcheck.Enabled = false;

                    //txtPass.ReadOnly = true;
                    //txtPassCheck.ReadOnly = true;

                    //txtPass.BackColor = Color.FromName("#dcdcdc");
                    //txtPassCheck.BackColor = Color.FromName("#dcdcdc");
                }

                WebControlHelper.SetSelectItem_DropDownList(this.ddlTrainee, "*");
                this.txtAcquisition.Text = string.Empty;
                
                this.txtID.Focus();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        #region 메일링 서비스 수신관련 체크박스
        protected void rbMail_y_OnCheckedChanged(object sender, EventArgs e)
        {
            if (rbMail_y.Checked == true)
                this.rbMail_n.Checked = false;
            else
                this.rbMail_n.Checked = true;
        }
        
        protected void rbMail_n_OnCheckedChanged(object sender, EventArgs e)
        {
            if (rbMail_y.Checked == true)
                this.rbMail_y.Checked = false;
            else
                this.rbMail_n.Checked = true;
        }
        #endregion 메일링 서비스 수신관련 체크박스

        protected void ddlUserGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sUserGroup = string.Empty;

            sUserGroup = ddlUserGroup.SelectedValue.ToString();

            if (Session["USER_GROUP"].ToString() == "000001")
            {
                txtPass.ReadOnly = false;
                txtPass.BackColor = Color.FromName("#ffffff");

                txtPassCheck.ReadOnly = false;
                txtPassCheck.BackColor = Color.FromName("#ffffff");

                txtPersonal_no1.ReadOnly = false;
                txtPersonal_no1.BackColor = Color.FromName("#ffffff");

                txtPersonal_no2.ReadOnly = false;
                txtPersonal_no2.BackColor = Color.FromName("#ffffff");
            }
            else
            {
                //비밀번호와 주민등록번호는 "법인사 관리자", "법인사 수강자"일 경우만 입력가능하게 한다.
                if (sUserGroup == "000008" || sUserGroup == "000007" || sUserGroup == "000010")
                {
                    //법인수강자 신규 등록시는 비번 안넣음. 무조건 주민번호 뒷자리
                    if (ViewState["EDITMODE"].ToString() == "NEW" && sUserGroup == "000008")
                    {
                        txtPass.ReadOnly = true;
                        txtPass.BackColor = Color.FromName("#dcdcdc");

                        txtPassCheck.ReadOnly = true;
                        txtPassCheck.BackColor = Color.FromName("#dcdcdc");

                        txtID.ReadOnly = true;
                        txtID.BackColor = Color.FromName("#dcdcdc");
                        btnIDcheck.Enabled = false;

                        txtID.Text = string.Empty;
                        txtPass.Text = string.Empty;
                        txtPassCheck.Text = string.Empty;
                    }
                    //법인관리자 신규 등록시는 비번 안넣음. 무조건 사업자 뒷자리
                    else if (ViewState["EDITMODE"].ToString() == "NEW" && sUserGroup == "000007")
                    {
                        txtPass.ReadOnly = true;
                        txtPass.BackColor = Color.FromName("#dcdcdc");

                        txtPassCheck.ReadOnly = true;
                        txtPassCheck.BackColor = Color.FromName("#dcdcdc");

                        txtPass.Text = string.Empty;
                        txtPassCheck.Text = string.Empty;
                    }
                    else
                    {
                        txtPass.ReadOnly = false;
                        txtPass.BackColor = Color.FromName("#ffffff");

                        txtPassCheck.ReadOnly = false;
                        txtPassCheck.BackColor = Color.FromName("#ffffff");

                        txtID.ReadOnly = false;
                        txtID.BackColor = Color.FromName("#ffffff");
                        btnIDcheck.Enabled = true;
                    }
                    txtPersonal_no1.ReadOnly = false;
                    txtPersonal_no1.BackColor = Color.FromName("#ffffff");

                    txtPersonal_no2.ReadOnly = false;
                    txtPersonal_no2.BackColor = Color.FromName("#ffffff");
                }
                else
                {
                    txtPass.ReadOnly = true;
                    txtPass.BackColor = Color.FromName("#dcdcdc");

                    txtPassCheck.ReadOnly = true;
                    txtPassCheck.BackColor = Color.FromName("#dcdcdc");

                    txtPersonal_no1.ReadOnly = true;
                    txtPersonal_no1.BackColor = Color.FromName("#dcdcdc");

                    txtPersonal_no2.ReadOnly = true;
                    txtPersonal_no2.BackColor = Color.FromName("#dcdcdc");
                }
            }
        }

    }
}
