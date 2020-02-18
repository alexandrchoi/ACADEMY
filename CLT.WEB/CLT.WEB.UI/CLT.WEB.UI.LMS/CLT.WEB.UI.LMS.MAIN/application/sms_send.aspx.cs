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
using C1.Web.C1WebChart;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;

using System.Data.OleDb;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : SMS 발송 Class
    /// 
    /// 2. 주요기능 : LMS SMS 발송
    ///				  
    /// 3. Class 명 : sms_send
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02.
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    /// 
    /// </summary>
    public partial class sms_send : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : Page Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Form.DefaultButton = this.btnSearch.UniqueID; // Page Default Button Mapping
                if (!IsPostBack)
                {
                    if (Session["USER_GROUP"].ToString() == "000009")
                    {
                        //return;
                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "sms_send", xScriptMsg);
                    }

                    //this.txtCus_From.Attributes.Add("onkeyup", "ChkDate(this);");
                    //this.txtCus_To.Attributes.Add("onkeyup", "ChkDate(this);");
                    
                    this.ddlYear.Enabled = false;
                    this.ddlMonth.Enabled = false;
                    this.ddlDay.Enabled = false;
                    this.ddlHour.Enabled = false;
                    this.ddlDay.Enabled = false;
                    this.ddlMinute.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnSearch_Click
        * Purpose       : 기간별 개설과정 조회버튼
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnSearch_Click(object sender, EventArgs e)
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtCus_From.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "조회기간" }, new string[] { "Date From" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtCus_To.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "조회기간" }, new string[] { "Date To" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                string[] xParams = new string[2];
                xParams[0] = this.txtCus_From.Text;
                xParams[1] = this.txtCus_To.Text;

                this.ddlCus_Date.ClearSelection();

                DataTable xDt = new DataTable();
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_m_sms_md",
                                       "GetCourseDate",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlCus_NM, xDt, "course_nm", "course_id");
                //this.ddlCus_NM.Items.FindByValue("11030001").Selected = true;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : ddlCus_NM_OnSelectedIndexChanged
        * Purpose       : 과정선택 DropDownList 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void ddlCus_NM_OnSelectedIndexChanged(object sender, EventArgs e)
        protected void ddlCus_NM_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlCus_NM.SelectedItem.Value == "*")
                    return;

                string[] xParams = new string[3];
                xParams[0] = ddlCus_NM.SelectedItem.Value;
                xParams[1] = this.txtCus_From.Text;
                xParams[2] = this.txtCus_To.Text;

                DataTable xDt = new DataTable();
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_m_sms_md",
                                       "GetOpenCourseDate",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION", (object)xParams);

                WebControlHelper.SetDropDownList(this.ddlCus_Date, xDt, "course_date", "open_course_id");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnAdd_OnClick
        * Purpose       : 선택된 과목 교육기간 대상자를 SMS 발송 리스트에 추가
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnAdd_OnClick(object sender, EventArgs e)
        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.ddlCus_Date.SelectedValue))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "대상조건" }, new string[] { "Target condition" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (this.ddlCus_Date.SelectedValue == "*")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "대상조건" }, new string[] { "Target condition" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                string[] xParams = new string[1];
                xParams[0] = this.ddlCus_Date.SelectedValue;

                DataTable xDt = new DataTable();
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_m_sms_md",
                                       "GetOpenCourseResultUser",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION", (object)xParams);
                
                for (int i = 0; i < xDt.Rows.Count; i++)
                {
                    // 중복추가 제거..
                    //if (lbSentlist.Items.FindByText(xDt.Rows[i]["mobile_phone"].ToString()) == null)
                    if (!SentSMSDuplicate(this.lbSentlist, xDt.Rows[i]["mobile_phone"].ToString()))
                    {
                        if (IsSettingKorean()) // 선택된 언어가 한국어일 경우
                            this.lbSentlist.Items.Add(new ListItem(xDt.Rows[i]["user_nm_kor"].ToString() + " : " + Regex.Replace(xDt.Rows[i]["mobile_phone"].ToString(), @"\D", ""), xDt.Rows[i]["user_id"].ToString()));
                        else
                            this.lbSentlist.Items.Add(new ListItem(xDt.Rows[i]["user_nm_eng"].ToString() + " : " + Regex.Replace(xDt.Rows[i]["mobile_phone"].ToString(), @"\D", ""), xDt.Rows[i]["user_id"].ToString()));

                    }
                }
                this.lblSendCnt.Text = TotalCount();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        ///************************************************************
        //* Function name : btnExcelUpload_OnClick
        //* Purpose       : 주소록 엑셀 업로드
        //* Input         : void
        //* Output        : void
        //*************************************************************/
        //public DataTable ReadXlsFile(string excelFileName, string sheetName)
        //{
        //    //string connectionString = @"Provider=Micrisoft.Jet.OLEDB.4.0;Data Source="
        //    //                          + excelFileName + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";

        //    //OleDbConnection conn = new OleDbConnection(connectionString);
        //    //OleDbCommand comm = new OleDbCommand();

        //    //OleDbDataAdapter adap = new OleDbDataAdapter();
        //    //comm.Connection = conn;
        //    //comm.CommandType = CommandType.Text;
        //    //comm.CommandText = "SELECT * FROM [" + sheetName + "$]";
        //    //adap.SelectCommand = comm;
        //    //System.Data.DataTable dtXls = new DataTable("");
        //    //adap.Fill(dtXls);
        //    //return dtXls;
        //}

        /************************************************************
        * Function name : btnExcelUpload_OnClick
        * Purpose       : 주소록 엑셀 업로드
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnExcelUpload_OnClick(object sender, EventArgs e)
        protected void btnExcelUpload_OnClick(object sender, EventArgs e)
        {
            try
            {
                string excelFileFullPath = Util.UploadGetFileName(this.fu_excel, "/file/excel");

                DataTable dt = Util.GetDtExcelFile(excelFileFullPath, true);

                //데이터 맵핑및 Validator 체크
                //string xResult = "true";

                //dt.Columns["id"].Unique = true;

                foreach (DataRow xDr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(xDr[1].ToString()))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "휴대폰 번호" }, new string[] { "Mobile Phone" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    if (string.IsNullOrEmpty(xDr[0].ToString()))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "이름" }, new string[] { "Name" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    string xPhone = xDr[1].ToString();
                    if (xPhone.Substring(0, 1) != "0")
                        xPhone = "0" + xPhone;

                    if (!SentSMSDuplicate(this.lbSentlist, this.txtMobilePhone.Text))
                        this.lbSentlist.Items.Add(new ListItem(xDr[0].ToString() + " : " + Regex.Replace(xPhone, @"\D", ""), "0"));
                    else
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A014", new string[] { "휴대폰 번호" }, new string[] { "Mobile Phone" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    this.lblSendCnt.Text = TotalCount();
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : RadioButton_OnCheckedChanged
        * Purpose       : rbSentType, rbBooking Radio 버튼 체크 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void RadioButton_OnCheckedChanged(object sender, EventArgs e)
        protected void RadioButton_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton rbButton = (RadioButton)sender;

                if (rbButton.ID == "rbSentType")
                {
                    this.ddlYear.Items.Clear();
                    this.ddlMonth.Items.Clear();
                    this.ddlDay.Items.Clear();
                    this.ddlHour.Items.Clear();
                    this.ddlDay.Items.Clear();
                    this.ddlMinute.Items.Clear();
                    
                    this.ddlYear.Enabled = false;
                    this.ddlMonth.Enabled = false;
                    this.ddlDay.Enabled = false;
                    this.ddlHour.Enabled = false;
                    this.ddlMinute.Enabled = false;
                }
                else if (rbButton.ID == "rbBooking")
                {
                    this.ddlYear.Enabled = true;
                    this.ddlMonth.Enabled = true;
                    this.ddlDay.Enabled = true;
                    this.ddlHour.Enabled = true;
                    this.ddlMinute.Enabled = true;

                    for (int i = 0; i < 2; i++)
                    {
                        ddlYear.Items.Add(DateTime.Now.AddYears(i).Year.ToString());
                    }

                    for (int i = 0; i < 12; i++)
                    {
                        ddlMonth.Items.Add(Convert.ToInt32(i + 1).ToString("00"));
                    }

                    DateTime xDtLast = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);
                    xDtLast = xDtLast.AddDays(-1);

                    for (int i = 0; i < 31; i++)
                    {

                        if (xDtLast.Day >= i)
                            ddlDay.Items.Add(Convert.ToInt32(i + 1).ToString("00"));
                    }

                    for (int i = 0; i < 23; i++)
                    {
                        ddlHour.Items.Add(Convert.ToInt32(i + 1).ToString("00"));
                    }

                    for (int i = 0; i < 59; i++)
                    {
                        ddlMinute.Items.Add(i.ToString("00"));
                    }

                    this.ddlYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
                    this.ddlMonth.Items.FindByValue(DateTime.Now.Month.ToString("00")).Selected = true;
                    this.ddlDay.Items.FindByValue(DateTime.Now.Day.ToString("00")).Selected = true;
                    this.ddlHour.Items.FindByValue(DateTime.Now.Hour.ToString("00")).Selected = true;
                    this.ddlMinute.Items.FindByValue(DateTime.Now.Minute.ToString("00")).Selected = true;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btn_Plus_OnClick
        * Purpose       : SMS 전송 리스트 추가
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btn_Plus_OnClick(object sender, EventArgs e)
        protected void btn_Plus_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtName.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "이름" }, new string[] { "Name" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtMobilePhone.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "휴대폰 번호" }, new string[] { "Mobile Phone" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                if (!SentSMSDuplicate(this.lbSentlist, this.txtMobilePhone.Text))
                    this.lbSentlist.Items.Add(new ListItem(this.txtName.Text + " : " + Regex.Replace(this.txtMobilePhone.Text, @"\D", ""), "0"));

                this.lblSendCnt.Text = TotalCount();
                this.txtName.Text = "";
                this.txtMobilePhone.Text = "";
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : Delete_OnClick
        * Purpose       : SMS 전송 리스트 삭제, 전체삭제
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void Delete_OnClick(object sender, EventArgs e)
        protected void Delete_OnClick(object sender, EventArgs e)
        {
            try
            {
                Button Btn = (Button)sender;

                if (Btn.ID == "btnAllDelete")
                {
                    this.lbSentlist.Items.Clear();
                }
                else if (Btn.ID == "btnDelete")
                {
                    if (this.lbSentlist.SelectedIndex != -1)
                        this.lbSentlist.Items.RemoveAt(this.lbSentlist.SelectedIndex);
                }

                this.lblSendCnt.Text = TotalCount();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnSend_OnClick
        * Purpose       : SMS 전송 버튼
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnSend_OnClick(object sender, EventArgs e)
        protected void btnSend_OnClick(object sender, EventArgs e)
        {
            try
            {
                // 필수입력값 체크
                if (string.IsNullOrEmpty(this.txtSMS_Title.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "제목" }, new string[] { "Title" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtRec_MobilePhone.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "회신번호" }, new string[] { "Response Mobile Phone Number" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (lbSentlist.Items.Count == 0)
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "발신번호 리스트" }, new string[] { "Send Mobile Phone Number" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtContent.Value))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "SMS 내용" }, new string[] { "SMS Contents" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                if (rbBooking.Checked == true) // 예약 발송일 경우
                {
                    if (string.IsNullOrEmpty(this.ddlYear.SelectedValue))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "예약발송 년도" }, new string[] { "Reserved Year" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    else if (string.IsNullOrEmpty(this.ddlMonth.SelectedValue))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "예약발송 월" }, new string[] { "Reserved Month" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    else if (string.IsNullOrEmpty(this.ddlDay.SelectedValue))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "예약발송 일" }, new string[] { "Reserved Day" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    else if (string.IsNullOrEmpty(this.ddlHour.SelectedValue))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "예약발송 시" }, new string[] { "Reserved Hour" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    else if (string.IsNullOrEmpty(this.ddlMinute.SelectedValue))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "예약발송 분" }, new string[] { "Reserved Minute" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                }

                string[] xMasterParams = new string[9];
                xMasterParams[0] = this.txtSMS_Title.Text; // SMS 제목
                xMasterParams[1] = txtRec_MobilePhone.Text; // SMS 회신번호
                xMasterParams[2] = this.txtContent.Value;  // SMS 발송내용
                xMasterParams[3] = this.lbSentlist.Items.Count.ToString();
                if (this.ddlCus_Date.SelectedItem != null)
                    xMasterParams[4] = this.ddlCus_Date.SelectedItem.Value.ToString();  // 과정코드
                else
                    xMasterParams[4] = "";
                xMasterParams[5] = Session["USER_ID"].ToString();
                if (this.rbSentType.Checked == true) // 지금 보낼 경우
                    xMasterParams[6] = "I";
                else if (this.rbBooking.Checked == true) // 예약발송일 경우
                {
                    xMasterParams[6] = "R";
                    xMasterParams[7] = this.ddlYear.SelectedValue + this.ddlMonth.SelectedValue + this.ddlDay.SelectedValue + this.ddlHour.SelectedValue + this.ddlMinute.SelectedValue + "00";
                }
                
                xMasterParams[8] = "00"; // 일반 SMS 코드
                if (this.HiddenMESS.Value == "MAIL")
                    xMasterParams[8] = "10"; // 이미지 없는 MMS 코드

                string[,] xDetailParams = new string[this.lbSentlist.Items.Count, 3];
                int nCount = 0;
                foreach (ListItem Items in this.lbSentlist.Items)
                {
                    string[] xText = Items.Text.Split(':');

                    xDetailParams[nCount, 0] = Items.Value; // 수신자 ID
                    xDetailParams[nCount, 1] = xText[0];    // 수신자 이름
                    xDetailParams[nCount, 2] = xText[1];    // 수신자 전화번호
                    nCount++;
                }

                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;

                object[] xParams = new object[2];

                xParams[0] = (object)xMasterParams;
                xParams[1] = (object)xDetailParams;

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                         "SetSentSMSBoxInsert",
                                         LMS_SYSTEM.APPLICATION,
                                         "CLT.WEB.UI.LMS.APPLICATION",
                                         (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A017",
                                  new string[] { "SMS" },
                                  new string[] { "SMS" },
                                  Thread.CurrentThread.CurrentCulture
                                 ));

                    xScriptMsg = "<script>window.location.href='/application/sms_list.aspx';</script>";
                    ScriptHelper.ScriptBlock(this, "sms_send", xScriptMsg);
                }
                else
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101",
                                  new string[] { "관리자" },
                                  new string[] { "Adminstrator" },
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


        /************************************************************
        * Function name : btnCancel_Click
        * Purpose       : Cancel 버튼
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnCancel_Click(object sender, EventArgs e)
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                string xScriptMsg = "";
                
                xScriptMsg = string.Format("<script>window.location.href = '/application/sms_list.aspx?MenuCode={0}';</script>", Session["MENU_CODE"]);

                ScriptHelper.ScriptBlock(this, "sms_list", xScriptMsg);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        /************************************************************
        * Function name : TotalCount
        * Purpose       : SMS 전송 대상 페이지 출력
        * Input         : void
        * Output        : String
        *************************************************************/
        #region private string TotalCount()
        private string TotalCount()
        {
            string xReturn = string.Empty;
            try
            {
                if (IsSettingKorean())
                    xReturn = this.lbSentlist.Items.Count.ToString();
                else
                    xReturn = this.lbSentlist.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xReturn;
        }
        #endregion
        
        /************************************************************
        * Function name : SentSMSDuplicate
        * Purpose       : SMS 전송대상 휴대폰번호 중복체크
        * Input         : void
        * Output        : String
        *************************************************************/
        #region private bool SentSMSDuplicate(ListBox lbSMS, string rMobilePhone)
        private bool SentSMSDuplicate(ListBox lbSMS, string rMobilePhone)
        {
            bool xDuplicate = false;
            try
            {
                foreach (ListItem Items in lbSMS.Items)
                {
                    string[] xValues = Items.Text.Split(':');
                    if (xValues[1].Trim() == Regex.Replace(rMobilePhone, @"\D", ""))
                    {
                        xDuplicate = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDuplicate;
        }
        #endregion

    }
}
