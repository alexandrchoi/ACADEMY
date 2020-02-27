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

namespace CLT.WEB.UI.LMS.MYPAGE
{
    /// <summary>
    /// 1. 작업개요 : 사용자 정보관리 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 사용자 정보관리
    ///				  
    /// 3. Class 명 : myinfo
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    /// 
    public partial class myinfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "myinfo", xScriptMsg);

                    return;
                }

                if (!IsPostBack)
                {
                    this.txtZipCode.Attributes.Add("ReadOnly", "ReadOnly");

                    BindDropDownList();

                    //base.pRender(this.Page,
                    //             new object[,] { //{ this.btnCancle, "I" }, 
                    //                         //{ this.btnIDcheck, "I" }, 
                    //                         //{ this.btnRewrite, "I" },
                    //                         { this.btnSave, "E" },
                    //                       },
                    //             Convert.ToString(Request.QueryString["MenuCode"]));

                    //getAuth();

                    //btnSave.Visible = AuthWrite;

                    EditMode(Session["USER_ID"].ToString());
                }
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


                xParams = new string[2];
                xParams[0] = "";
                xParams[1] = "";
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

                this.txtuser_nm_kor.Text = xDr["user_nm_kor"].ToString();
                this.txtuser_nm_eng_first.Text = xDr["user_nm_eng_first"].ToString();
                this.txtuser_nm_eng_last.Text = xDr["user_nm_eng_last"].ToString();
                WebControlHelper.SetSelectItem_DropDownList(this.ddlComapnyduty, xDr["duty_step"].ToString());

                txtEmail.Text = xDr["email_id"].ToString();
                txtPhone.Text = xDr["office_phone"].ToString();
                txtMobilePhone.Text = xDr["mobile_phone"].ToString();
                hidCompany_id.Value = xDr["company_id"].ToString();
                txtCompany.Text = xDr["company_nm"].ToString();
                hidCompany_id.Value = xDr["company_id"].ToString();
                txtZipCode.Text = xDr["user_zip_code"].ToString();

                string[] xAddr = xDr["user_addr"].ToString().Split('|');

                int xCount = 0;
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

                this.txtAcquisition.Text = xDr["enter_dt"].ToString();
                this.txtBirth_dt.Text = xDr["birth_dt"].ToString();

                WebControlHelper.SetSelectItem_DropDownList(ddlDept, xDr["dept_code"].ToString());
                txtDept.Text = xDr["dept_name"].ToString();

                if (xDr["dept_code"].ToString() != xDr["dept_name"].ToString())
                {
                    txtDept.ReadOnly = true;
                }

                if (xDr["pic_file"] != DBNull.Value && !string.IsNullOrEmpty(xDr["pic_file"].ToString()))
                {
                    try
                    {
                        int nFind = xDr["pic_file_nm"].ToString().IndexOf(".");
                        if (nFind > 0)
                        {
                            string mimeType = "image/" + xDr["pic_file_nm"].ToString().Substring(nFind + 1);
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

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                UpdateSave();

                bSaveFile();

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

                xParams[1] = OpusCryptoLibrary.Cryptography.Encrypt(this.txtNewPass.Text, this.txtNewPass.Text);
                
                xParams[2] = txtuser_nm_kor.Text.Replace("'", "''");
                xParams[3] = txtuser_nm_eng_first.Text.Replace("'", "''");
                xParams[4] = txtuser_nm_eng_last.Text.Replace("'", "''");
                xParams[5] = ddlComapnyduty.SelectedValue.ToString();
                //xParams[6] = txtPersonal_no1.Text.Replace("'", "''") + "-" + txtPersonal_no2.Text.Replace("'", "''");
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
                //xParams[17] = this.ddlUserGroup.SelectedItem.Value.ToString();
                //xParams[18] = this.ddlTrainee.SelectedItem.Value.ToString().Replace("*", ""); //훈련생 구분
                xParams[19] = this.txtAcquisition.Text.Replace(".", "").Trim() == string.Empty ? null : this.txtAcquisition.Text; //고용보험취득일 
                xParams[20] = Session["user_group"].ToString();
                xParams[21] = txtBirth_dt.Text.Replace(".", "").Trim() == string.Empty ? null : txtBirth_dt.Text; //생년월일 
                xParams[22] = string.IsNullOrEmpty(ddlDept.SelectedValue.ToString()) ? txtDept.Text.Replace("'", "''") : ddlDept.SelectedValue.ToString(); // 사용자 부서 dept_code

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                             "SetMyInfoEdit",
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
        
    }
}
