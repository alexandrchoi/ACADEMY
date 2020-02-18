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
using System.Text;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading;

namespace CLT.WEB.UI.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : 메일 발송 Class
    /// 
    /// 2. 주요기능 : LMS 메일 발송 화면
    ///				  
    /// 3. Class 명 : mail_detail
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    /// 
    /// </summary>
    public partial class mail_detail : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 설문조사 페이지 Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "mail_detail", xScriptMsg);

                    return;
                }

                if (Request.QueryString["rseq"] != null && Request.QueryString["rseq"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        //this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                        base.pRender(this.Page,
                                     new object[,] { { this.btnDown1, "I" },
                                                 { this.btnDown2, "I" },
                                                 { this.btnDown3, "I" },
                                           },
                                     Convert.ToString(Request.QueryString["MenuCode"]));

                        BindData(Request.QueryString["rseq"].ToString());
                    }
                }
                else
                {
                    //string xScriptContent = "<script>alert('잘못된 경로를 통해 접근하였습니다.');self.close();</script>";
                    //ScriptHelper.ScriptBlock(this, "mail_detail", xScriptContent);
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
        * Function name : btnList_Click
        * Purpose       : 메일 리스트 조회 버튼 Click 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnList_Click(object sender, EventArgs e)
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/application/mail_list.aspx");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnDown_Click
        * Purpose       : File Download 버튼 Click 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnDown_Click(object sender, EventArgs e)
        protected void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnDown = (Button)sender;
                if (btnDown.ID == "btnDown1")
                {
                    SaveFile(this.lblDown1.Text);
                }
                else if (btnDown.ID == "btnDown2")
                {
                    SaveFile(this.lblDown2.Text);
                }
                else if (btnDown.ID == "btnDown3")
                {
                    SaveFile(this.lblDown3.Text);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : SaveFile
        * Purpose       : File Download 메서드
        * Input         : void
        * Output        : void
        *************************************************************/
        #region public void SaveFile(string rFilename)
        public void SaveFile(string rFilename)
        {
            try
            {
                FileInfo xfileinfo = new FileInfo(Server.MapPath(ContentsFilePath) + rFilename);

                Response.Clear();
                Response.ContentType = "application/octet-stream";

                if (Request.UserAgent.IndexOf("MSIE") >= 0)  // InternetExplorer 일 경우
                {
                    /*
                    * 2013.08.20 Seojw
                    * 첨부파일 다운로드 시 스페이스에 "+"기호 붙는걸 방지하기 위해
                    * HttpUtility.UrlEncode → HttpUtility.HttpUtility.UrlPathEncode 로 변경
                    */
                    //Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", HttpUtility.UrlEncode(rFilename))); 
                    Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", HttpUtility.UrlPathEncode(rFilename)));
                }
                else
                    Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", HttpUtility.UrlDecode(rFilename)));
                
                Response.TransmitFile(xfileinfo.FullName); //, 0, -1);
                Response.End();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion
        
        /************************************************************
        * Function name : BindData
        * Purpose       : Mail 상세보기 메서드
        * Input         : void
        * Output        : void
        *************************************************************/
        #region public void BindData(string rSeq)
        public void BindData(string rSeq)
        {
            try
            {
                DataTable xDt = new DataTable();
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_m_mail_md",
                                       "GetMailListDetail",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION", (object)rSeq, Thread.CurrentThread.CurrentCulture);

                if (xDt == null)
                    return;

                this.lblSubject.Text = xDt.Rows[0]["mail_sub"].ToString();
                this.lblFromName.Text = xDt.Rows[0]["user_nm_kor"].ToString();
                this.lblFromEMail.Text = xDt.Rows[0]["email_id"].ToString();
                this.lblSendDate.Text = xDt.Rows[0]["sent_dt"].ToString();
                this.lblToEmail.Text = xDt.Rows[0]["rec_email"].ToString();

                if (IsSettingKorean())
                    this.lblGubun.Text = "발송메일";
                else
                    this.lblGubun.Text = "Send Mail";

                this.txtContent.Text = xDt.Rows[0]["sent_mail"].ToString();

                int nCount = 1;
                bool bBtnVisible = true;
                foreach (DataRow xDr in xDt.Rows)
                {
                    if (xDr["att_file"] == null)
                        continue;

                    if (string.IsNullOrEmpty(xDr["att_file"].ToString()))
                        continue;

                    if (nCount == 1)
                    {
                        this.lblDown1.Text = xDr["att_file"].ToString();
                        this.TRDown1.Visible = true;
                        bBtnVisible = true;
                    }
                    else if (nCount == 2)
                    {
                        this.lblDown2.Text = xDr["att_file"].ToString();
                        this.TRDown2.Visible = true;
                        bBtnVisible = true;
                    }
                    else if (nCount == 3)
                    {
                        this.lblDown3.Text = xDr["att_file"].ToString();
                        this.TRDown3.Visible = true;
                        bBtnVisible = true;
                    }

                    nCount++;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : btnNew_Click
        * Purpose       : 메일 작성 Click 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnNew_Click(object sender, EventArgs e)
        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                string xScriptMsg = string.Format("<script>window.location.href='/application/mail_send.aspx?MenuCode={0}';</script>", Session["MENU_CODE"]);
                ScriptHelper.ScriptBlock(this, "mail_send", xScriptMsg);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

    }
}
