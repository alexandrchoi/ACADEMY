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
using CLT.WEB.UI.COMMON.BASE;
using System.Data.OleDb;
using System.IO;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.FX.AGENT;
using System.Threading;
using C1.Web.C1WebGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.MANAGE
{
    /// <summary>
    /// 1. 작업개요 : 사용자 엑셀업로드 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 사용자 엑셀업로드
    ///				  
    /// 3. Class 명 : user_excel
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    /// 
    public partial class user_excel : BasePage
    {
        private DataTable iDtDutystep { get { return (DataTable)ViewState["DUTYSTEP"]; } }
        private DataTable iDtTraineeclass { get { return (DataTable)ViewState["TRAINEECLASS"]; } }
        
        /************************************************************
        * Function name : Page_Load
        * Purpose       : Page_Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                    base.pRender(this.Page, new object[,] {
                                                        {
                                                            this.btnSend, "E" ,
                                                            this.btnExcel, "E"
                                                        }
                                                      }, Convert.ToString(Request.QueryString["MenuCode"]));

                    string[] xParams = new string[2];
                    
                    xParams[1] = "Y";
                    ViewState["DUTYSTEP"] = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                             "GetDutystepCodeInfo",
                                                             LMS_SYSTEM.MANAGE,
                                                             "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                    //훈련생구분 
                    xParams[0] = "0062";
                    xParams[1] = "Y";
                    ViewState["TRAINEECLASS"] = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                                 "GetCommonCodeInfo",
                                                                 LMS_SYSTEM.MANAGE,
                                                                 "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                    //btnExcelDown.NavigateUrl = "/file/download/appr_item_list.xls";
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnExcel_Click
        * Purpose       : 엑셀파일업로드 클릭 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string excelFileFullPath = Util.UploadGetFileName(this.fu_excel, "/file/excel");

                if (!string.IsNullOrEmpty(excelFileFullPath))
                {
                    DataTable dt = Util.GetDtExcelFile(excelFileFullPath, true);

                    //데이터 맵핑및 Validator 체크
                    string xResult = "true";
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //}

                    if (xResult == "true")
                    {
                        this.trMESSAGE.Visible = false;

                        grdItem.DataSource = dt;
                        grdItem.DataBind();
                    }

                    Util.FileDel(excelFileFullPath, false);
                    this.fu_excel.Visible = false;
                    btnExcel.Visible = false;
                    this.btnSend.Visible = true;
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
        * Purpose       : 엑셀업로드버튼 클릭 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnSend_Click(object sender, EventArgs e)
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                int xCntSel = 0;
                DataTable xDt = new DataTable();
                xDt.Columns.Add("ID");  //  사용자ID
                xDt.Columns.Add("PWD"); // 비밀번호
                xDt.Columns.Add("USER_NM_KOR");
                xDt.Columns.Add("PERSONAL_NO");
                xDt.Columns.Add("USER_NM_ENG_FIRST");
                xDt.Columns.Add("USER_NM_ENG_LAST");
                xDt.Columns.Add("USER_ZIP_CODE");
                xDt.Columns.Add("USER_ADDR1");
                //xDt.Columns.Add("USER_ADDR2");
                xDt.Columns.Add("DUTY_STEP");
                xDt.Columns.Add("OFFICE_PHONE");
                xDt.Columns.Add("EMAIL_ID");
                xDt.Columns.Add("MOBILE_PHONE");
                //xDt.Columns.Add("SMS_YN");
                //xDt.Columns.Add("MAIL_YN");
                xDt.Columns.Add("TRAINEE_CLASS");
                xDt.Columns.Add("ENTER_DT");

                xDt.Columns.Add("USER_ID");
                xDt.Columns.Add("USER_GROUP");
                xDt.Columns.Add("COMPANY_ID");
                xDt.Columns.Add("STATUS");
                xDt.Columns.Add("ADMIN_PHONE");
                
                for (int i = 0; i < this.grdItem.Items.Count; i++)
                {
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("chk_sel")).Checked)
                    {
                        //DataRow[] xDrs = xDt.Select(string.Format("ID ='{0}'", ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtID")).Text.Replace("'", "''")));
                        //if (xDrs.Length > 0)
                        //{
                        //    string xID = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtID")).Text.ToLower().Trim().Replace("'", "''");
                        //    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A121", new string[] { "ID", xID }, new string[] { "ID", xID }, Thread.CurrentThread.CurrentCulture));
                        //    return;
                        //}
                        
                        //string[] xParams = new string[1];
                        //xParams[0] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtID")).Text.ToLower().Trim().Trim().Replace("'", "''");
                        //// ID 중복체크
                        //DataTable xdt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                        //                                 "GetUser",
                        //                                 LMS_SYSTEM.MANAGE,
                        //                                 "CLT.WEB.UI.LMS.MANAGE",
                        //                                 (object)xParams);

                        //if (xdt.Rows.Count > 0)
                        //{
                        //    string xID = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtID")).Text.ToLower().Trim().Replace("'", "''");
                        //    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A121", new string[] { "ID", xID }, new string[] { "ID", xID }, Thread.CurrentThread.CurrentCulture));
                        //    return;
                        //}
                        

                        DataRow xRow = xDt.NewRow();
                        xRow["ID"] = string.Empty; //((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtID")).Text.ToLower().Trim().Replace("'", "''");

                        //xRow["PWD"] = (string)xx.Encrypt(((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtPesonalNo")).Text.Replace("'", "''").Substring(7, 7));  // "lC0o7ueTCeuroZc8ZrLMNg==";

                        xRow["PWD"] = OpusCryptoLibrary.Cryptography.Encrypt("", ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtPesonalNo")).Text.Replace("'", "''").Substring(7, 7));

                        xRow["USER_NM_KOR"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtUserNMKor")).Text.Replace("'", "''").Trim();
                        xRow["PERSONAL_NO"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtPesonalNo")).Text.Replace("'", "''").Trim();
                        xRow["USER_NM_ENG_FIRST"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtUserNMEngFirst")).Text.Replace("'", "''").Trim();
                        xRow["USER_NM_ENG_LAST"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtUserNMEngLast")).Text.Replace("'", "''").Trim();
                        xRow["USER_ZIP_CODE"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtZipCode")).Text.Replace("'", "''").Trim();
                        xRow["USER_ADDR1"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtAddress1")).Text.Replace("'", "''");
                        //xRow["USER_ADDR2"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtAddress2")).Text.Replace("'", "''");
                        xRow["DUTY_STEP"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("ddlDutyStep")).SelectedValue;
                        xRow["OFFICE_PHONE"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtPhone")).Text.Replace("'", "''").Trim();
                        xRow["EMAIL_ID"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtEMail")).Text.Replace("'", "''").Trim();
                        xRow["MOBILE_PHONE"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtMobilePhone")).Text.Replace("'", "''").Trim();
                        //xRow["SMS_YN"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtSMS_YN")).Text.Replace("'", "''");
                        //xRow["MAIL_YN"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtMAIL_YN")).Text.Replace("'", "''");
                        xRow["TRAINEE_CLASS"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("ddlTraineeClass")).SelectedValue;
                        xRow["ENTER_DT"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdItem.Items[i]).FindControl("txtAcquisition")).Text.Replace("'", "''").Trim();
                        xRow["USER_ID"] = Session["USER_ID"].ToString();
                        xRow["USER_GROUP"] = "000008"; // 법인사 수강자
                        xRow["COMPANY_ID"] = Session["COMPANY_ID"].ToString();
                        xRow["STATUS"] = "000003";  // 사용자 상태 [승인] status
                        xRow["ADMIN_PHONE"] = Session["OFFICE_PHONE"].ToString();
                        xDt.Rows.Add(xRow);
                    }
                }

                if (xDt.Rows.Count > 0)
                {
                    object[] obj = new object[1];
                    obj[0] = (object)xDt;

                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                   "SetUserUpload",
                                   LMS_SYSTEM.MANAGE,
                                   "vp_m_manage_user_excel_wpg", (object)obj);

                    if (xRtn.ToUpper() == "TRUE")
                    {
                        // 정상적으로 처리 되었습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    }
                    else
                    {
                        // 정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    }
                }
                else
                {
                    ScriptHelper.Page_Alert(this, CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A047", new string[] { "" }, new string[] { "" }, System.Threading.Thread.CurrentThread.CurrentCulture));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        protected void grdItem_ItemCreated(object sender, C1ItemEventArgs e)
        {

        }

        protected void grdItem_ItemDataBound(object seder, C1ItemEventArgs e)
        {
            try
            {
                DataRowView DRV = (DataRowView)e.Item.DataItem;

                // 직급
                DropDownList ddlDutyStep = ((DropDownList)e.Item.FindControl("ddlDutyStep"));
                DropDownList ddlTraineeClass = ((DropDownList)e.Item.FindControl("ddlTraineeClass"));

                TextBox txtDate = (TextBox)e.Item.FindControl("txtAcquisition");

                if (txtDate != null)
                {
                    txtDate.Attributes.Add("onkeyup", "ChkDate(this);");
                }
                
                if (ddlDutyStep != null)
                {
                    WebControlHelper.SetDropDownList(ddlDutyStep, iDtDutystep, "step_name", "duty_step");
                    WebControlHelper.SetSelectText_DropDownList(ddlDutyStep, DRV["직급"].ToString());
                }

                if (ddlTraineeClass != null)
                {
                    WebControlHelper.SetDropDownList(ddlTraineeClass, iDtTraineeclass);
                    WebControlHelper.SetSelectText_DropDownList(ddlTraineeClass, DRV["훈련생 구분"].ToString());
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

    }
}
