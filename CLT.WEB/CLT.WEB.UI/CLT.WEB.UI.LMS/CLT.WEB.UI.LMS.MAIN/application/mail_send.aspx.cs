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
using System.IO;
using System.Threading;

namespace CLT.WEB.UI.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : 메일 발송 Class
    /// 
    /// 2. 주요기능 : LMS 메일 발송
    ///				  
    /// 3. Class 명 : mail_send
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02.
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    /// 
    /// </summary>
    public partial class mail_send : BasePage
    {
        public static ArrayList ialFileList = new ArrayList();

        /************************************************************
        * Function name : Page_Load
        * Purpose       : 페이지 Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            txtCONTENT.Text = txt_content.Text;

            try
            {
                this.Page.Form.DefaultButton = this.btnSearch.UniqueID; // Page Default Button Mapping
                if (!IsPostBack)
                {
                    if (Session["USER_GROUP"].ToString() == "000009")
                    {
                        //return;
                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "mail_send", xScriptMsg);
                    }

                    base.pRender(this.Page,
                                 new object[,] { { this.btnMailSend, "E" } });
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnAdd_OnClick
        * Purpose       : 버튼클릭 이벤트
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
                    if (string.IsNullOrEmpty(this.txtTo.Text))
                    {
                        this.txtTo.Text = this.txtTo.Text + xDt.Rows[i]["email_id"].ToString();
                    }
                    else
                    {
                        this.txtTo.Text = this.txtTo.Text + "; " + xDt.Rows[i]["email_id"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnAdd_OnClick
        * Purpose       : 버튼클릭 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnSelect_OnClick(object seder, EventArgs e)
        protected void btnSelect_OnClick(object seder, EventArgs e)
        {
            try
            {
                string[] xParams = new string[1];
                xParams[0] = HiddenCourseID.Value;

                DataTable xDt = new DataTable();
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                       "GetOpenCourseDate",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION", (object)xParams);

                WebControlHelper.SetDropDownList(this.ddlCus_Date, xDt, "course_date", "open_course_id");
                //this.txtCus_ID.Text = HiddenCourseID.Value;
                //this.txtCus_NM.Text = HiddenCourseNM.Value;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnUpload_OnClick
        * Purpose       : 버튼클릭 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnUpload_OnClick(object sender, EventArgs e)
        protected void btnUpload_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (this.FileUpload1.PostedFile.ContentLength == 0)
                {
                    // 첨부파일이 없습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A062", new string[] { "첨부파일" }, new string[] { "Attachment" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                
                int xfileSize = this.FileUpload1.PostedFile.ContentLength;

                int TotalSize = 0;

                TotalSize = TotalSize + this.FileUpload1.PostedFile.ContentLength;
                
                if (this.lbSentlist.Items.Count >= 3) // 첨부파일은 최대 3개까지 가능
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A007", new string[] { "첨부파일", "3개" }, new string[] { "Attachment", "3 File" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (lbSentlist.Items.Contains(new ListItem(this.FileUpload1.FileName)))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A014", new string[] { this.FileUpload1.FileName }, new string[] { this.FileUpload1.FileName }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (TotalSize > 4194302)
                {
                    // 첨부파일이 4메가 보다 큽니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A030", new string[] { "첨부파일", "4메가" }, new string[] { "Attachment", "4MB" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                /*
                foreach (FileUpload fUpload in lbSentlist.Items)
                {
                    TotalSize = TotalSize + fUpload.PostedFile.ContentLength;
                    if (TotalSize > 4194302)
                    {
                        // 첨부파일이 4메가 보다 큽니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A030", new string[] { "첨부파일", "4메가" }, new string[] { "Attachment", "4MB" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                }
                */
                if (!ialFileList.Contains(this.FileUpload1))
                    ialFileList.Add(this.FileUpload1);

                lbSentlist.Items.Add(new ListItem(this.FileUpload1.FileName));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnRemove_OnClick
        * Purpose       : 버튼클릭 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnRemove_OnClick(object sender, EventArgs e)
        protected void btnRemove_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (lbSentlist.SelectedItem == null)
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A049", new string[] { "삭제할 파일" }, new string[] { "Attachment File" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                foreach (FileUpload Temp in ialFileList)
                {
                    if (Temp.FileName == lbSentlist.SelectedItem.Text)
                    {
                        ialFileList.Remove(Temp);
                        lbSentlist.Items.Remove(lbSentlist.SelectedItem);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnMailSend_OnClick
        * Purpose       : 버튼클릭 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnMailSend_OnClick(object sender, EventArgs e)
        protected void btnMailSend_OnClick(object sender, EventArgs e)
        {
            try
            {
                // 필수입력값 체크
                if (string.IsNullOrEmpty(this.txtTo.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "받는사람" }, new string[] { "To." }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtSubject.Text))
                {
                    if (IsSettingKorean())
                        this.txtSubject.Text = "제목없음";
                    else
                        this.txtSubject.Text = "No Title";
                }
                else if (string.IsNullOrEmpty(this.txtCONTENT.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "내용" }, new string[] { "Content" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (string.IsNullOrEmpty(Session["USER_EMAIL"].ToString()))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "발신자 메일 주소" }, new string[] { "Send Email Address" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;

                object[] xParams = new object[2];

                string[] xToCount = this.txtTo.Text.Split(';');  // 받는사람 숫자

                string[] xMasterParams = new string[10];
                xMasterParams[0] = this.txtSubject.Text; // 메일제목
                xMasterParams[1] = this.txtCONTENT.Text; // 메일내용
                xMasterParams[2] = xToCount.Length.ToString(); // 발신자 총원
                xMasterParams[3] = this.txtTo.Text;  // 수신자 메일주소
                if (this.ddlCus_Date.SelectedItem != null)
                    xMasterParams[4] = this.ddlCus_Date.SelectedItem.Value;
                else
                    xMasterParams[4] = "";
                xMasterParams[5] = Session["USER_ID"].ToString();  // 발신자 ID
                xMasterParams[6] = Session["DUTY_STEP"].ToString();  // 발신자 직급
                xMasterParams[7] = Server.MapPath(this.ContentsFilePath);  // 업로드 경로
                xMasterParams[8] = Session["USER_EMAIL"].ToString(); // 발신자 EMail 주소
                xMasterParams[9] = this.cbBcc.Checked.ToString();
                
                ArrayList iRealialFileList = new ArrayList();
                FileUpload fu;

                for (int i = 0; i < ialFileList.Count; i++)
                {
                    for (int j = 0; j < lbSentlist.Items.Count; j++)
                    {
                        fu = (FileUpload)ialFileList[i];

                        if (fu.FileName == lbSentlist.Items[j].Text)
                        {
                            iRealialFileList.Add(fu);
                        }
                    }
                }

                ialFileList.Clear();

                ialFileList = iRealialFileList;
                
                object[] xDetailParams = new object[ialFileList.Count];
                for (int i = 0; i < ialFileList.Count; i++)
                {
                    FileUpload xUpload = (FileUpload)ialFileList[i];
                    xDetailParams[i] = xUpload.PostedFile;
                }

                xParams[0] = xMasterParams;
                xParams[1] = xDetailParams;

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPLICATION.vp_m_mail_md",
                        "SetMailSend",
                        LMS_SYSTEM.APPLICATION,
                        "CLT.WEB.UI.LMS.APPLICATION",
                        (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));

                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));

                }
                //ialFileList.Clear();
                ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { xRtn + " | " }, new string[] { xRtn + " | " }, Thread.CurrentThread.CurrentCulture));

                //Response.Redirect("/clt.web.ui.lms.manage/vp_m_manage_mail_list_wpg.aspx");
            }
            catch (Exception ex)
            {
                //ialFileList.Clear();

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
                
                xScriptMsg = string.Format("<script>window.location.href = '/application/mail_list.aspx?MenuCode={0}';</script>", Session["MENU_CODE"]);

                ScriptHelper.ScriptBlock(this, "mail_list", xScriptMsg);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

    }
}
