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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.COMMUNITY
{
    /// <summary>
    /// 1. 작업개요 : 공지사항 조회 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 공지사항 조회 관리
    ///				  
    /// 3. Class 명 : notice_detail
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02.
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    /// 
    /// </summary>
    public partial class notice_detail : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 공지사항 페이지 Load 이벤트

        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //this.Page.Form.DefaultButton = this.btn.UniqueID; // Page Default Button Mapping
                if (Request.QueryString["rseq"] != null && Request.QueryString["rseq"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        //onclick="opener.__doPostBack('ctl00$ContentPlaceHolderMain$LnkBtnDown1','');" 
                        //lblDown1.Attributes.Add("onclick", "javascript:opener.__doPostBack('ctl00$ContentPlaceHolderMain$LnkBtnDown1','');");

                        ////if (ViewState["HitCnt"] == null)
                        //if (iHitCnt == false)
                        //{
                        //    iHitCnt = true;
                        //    //ViewState["HitCnt"] = Boolean.TrueString;
                        //}

                        //base.pRender(this.Page,
                        //             new object[,] {
                        //                     { this.btnList, "I" },
                        //                     { this.btnModify, "E" },
                        //                     { this.btnDelete, "A" },
                        //                     { this.btnRestore, "A" },
                        //                   },
                        //             Convert.ToString(Request.QueryString["MenuCode"]));

                        getAuth();

                        BindHitCnt(Request.QueryString["rseq"].ToString());
                        BindData(Request.QueryString["rseq"].ToString());
                        
                        btnModify.Visible = AuthWrite;
                    }
                }
                else
                {

                    // DOM Explorer 에서 편집하지 못하도록 버튼 숨김형태로 변경
                    this.btnModify.Visible = false;
                    this.btnDelete.Visible = false;
                    this.btnRestore.Visible = false;

                    //return;
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "notice_list", xScriptMsg);


                }


                /*
                System.Text.RegularExpressions.Regex cRegEx = new System.Text.RegularExpressions.Regex(@"\w+\.aspx");
                System.Text.RegularExpressions.Match cMatch = cRegEx.Match(Request.UrlReferrer.ToString());
                //System.Text.RegularExpressions.Match cMatch = cRegEx.Match(Request.RawUrl);

                if (cMatch.Success)
                {
                    string filename = cMatch.Value;

                    string xScriptContent = "<script>document.write('" + filename + "');</script>";

                    ScriptHelper.ScriptBlock(this, "notice_list", xScriptContent);
                }
                */


            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion



        /************************************************************
        * Function name : button_Click
        * Purpose       : 공지사항 신규, 수정, 삭제, 조회 버튼 Click 이벤트

        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void button_Click(object sender, EventArgs e)
        protected void button_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;

                if (btn.ID == "btnNew")  // 신규
                {
                    string xURL = string.Format("/community/notice_edit.aspx?EDITMODE=NEW&MenuCode={0}&delYN={1}", Session["MENU_CODE"], Request.QueryString["delYN"]);
                    Response.Redirect(xURL);
                }
                else if (btn.ID == "btnModify")  // 수정
                {
                    string xUrl = "/community/notice_edit.aspx?";
                    xUrl += string.Format("EDITMODE={0}", "EDIT");
                    xUrl += string.Format("&SEQ={0}", Request.QueryString["rseq"]);
                    xUrl += string.Format("&MenuCode={0}", Session["MENU_CODE"]);
                    xUrl += string.Format("&delYN={0}", Request.QueryString["delYN"]);
                    Response.Redirect(xUrl);
                }
                else if (btn.ID == "btnDelete")  // 삭제
                {
                    buttonDelete(Request.QueryString["rseq"], "Y");
                }
                else if (btn.ID == "btnRestore")  // 복구
                {
                    buttonDelete(Request.QueryString["rseq"], "N");
                }
                else if (btn.ID == "btnList")  // 조회
                {
                    Response.Redirect("/community/notice_list.aspx?BIND=BIND&delYN=" + Request.QueryString["delYN"]);
                }
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
                //Button btnDown = (Button)sender;
                LinkButton btnDown = (LinkButton)sender;
                if (btnDown.ID == "btnDown1")
                {
                    SaveFile(ViewState["FILE1"].ToString());
                }
                else if (btnDown.ID == "btnDown2")
                {
                    SaveFile(ViewState["FILE2"].ToString());
                }
                else if (btnDown.ID == "btnDown3")
                {
                    SaveFile(ViewState["FILE3"].ToString());
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : BindHitCnt
        * Purpose       : 공지사항 조회시 조회숫자 Count 메서드

        * Input         : string rSeq (공지사항 번호)
        * Output        : void
        *************************************************************/
        #region public void BindHitCnt(string rSeq)
        public void BindHitCnt(string rSeq)
        {
            try
            {
                //SetNoticeHitCnt
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMUNITY.vp_y_community_notice_md",
                            "SetNoticeHitCnt",
                            LMS_SYSTEM.COMMUNITY,
                            "CLT.WEB.UI.LMS.COMMUNITY.notice_detail",
                            (object)rSeq);

                if (xRtn == Boolean.FalseString)
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
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
        * Function name : BindData
        * Purpose       : 공지사항 조회 메서드

        * Input         : string rSeq (공지사항 번호)
        * Output        : void
        *************************************************************/
        #region public void BindData(string rSeq)
        public void BindData(string rSeq)
        {
            try
            {
                string[] xParams = new string[2];
                xParams[0] = rSeq;
                xParams[1] = Session["USER_GROUP"].ToString();

                DataTable xDt = new DataTable();
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMUNITY.vp_y_community_notice_md",
                                       "GetNoticeDetail",
                                       LMS_SYSTEM.COMMUNITY,
                                       "CLT.WEB.UI.LMS.COMMUNITY.notice_detail", (object)xParams, Thread.CurrentThread.CurrentCulture);

                if (xDt == null || xDt.Rows.Count == 0)
                    return;

                this.lblSubject.Text = xDt.Rows[0]["not_sub"].ToString();  // 제목   
                this.lblCreatedBy.Text = xDt.Rows[0]["user_nm"].ToString();  // 작성자


                this.lblHitCnt.Text = xDt.Rows[0]["hit_cnt"].ToString();  // 읽은수


                this.lblCreated.Text = xDt.Rows[0]["ins_dt"].ToString();  // 작성일자
                //this.lblSendFLG.Text = xDt.Rows[0]["send_flg"].ToString();  // 전송일자
                this.lblContent.Text = xDt.Rows[0]["content"].ToString(); // 공지사항 내용

                base.pRender(this.Page,
                             new object[,] {
                                             { (xDt.Rows[0]["del_yn"].ToString() == "Y" ? btnRestore : btnDelete), "A" },
                             },
                             Convert.ToString(Request.QueryString["MenuCode"]));

                int nCount = 1;
                foreach (DataRow xDr in xDt.Rows)
                {
                    if (xDr["file_nm"] == null)
                        continue;

                    if (string.IsNullOrEmpty(xDr["file_nm"].ToString()))
                        continue;

                    if (nCount == 1)
                    {
                        this.lblDown1.Text = "<a href =javascript:__doPostBack('ctl00$ContentPlaceHolderMain$btnDown1','')>" + xDr["file_nm"].ToString() + "</a>";
                        ViewState["FILE1"] = xDr["file_nm"].ToString();
                        this.dtAttFile.Visible = this.ddAttFile.Visible = true;
                    }
                    else if (nCount == 2)
                    {
                        this.lblDown2.Text = "<a href =javascript:__doPostBack('ctl00$ContentPlaceHolderMain$btnDown2','')>" + xDr["file_nm"].ToString() + "</a>";
                        ViewState["FILE2"] = xDr["file_nm"].ToString();
                    }
                    else if (nCount == 3)
                    {
                        this.lblDown3.Text = "<a href =javascript:__doPostBack('ctl00$ContentPlaceHolderMain$btnDown3','')>" + xDr["file_nm"].ToString() + "</a>";
                        ViewState["FILE3"] = xDr["file_nm"].ToString();
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
        * Function name : SaveFile
        * Purpose       : 첨부파일 Save 메서드

        * Input         : void
        * Output        : void
        *************************************************************/
        #region public void SaveFile(string rFilename)
        public void SaveFile(string rFilename)
        {
            try
            {
                string[] xParams = new string[2];
                xParams[0] = Request.QueryString["rseq"].ToString();
                xParams[1] = rFilename;

                object obj = null;
                obj = SBROKER.GetObject("CLT.WEB.BIZ.LMS.COMMUNITY.vp_y_community_notice_md",
                                        "GetNoticeFile",
                                        LMS_SYSTEM.COMMUNITY,
                                        "CLT.WEB.UI.LMS.COMMUNITY.notice_detail", (object)xParams);


                byte[] fileByte = (byte[])obj;


                string xFilePath = Server.MapPath("\\file\\tempfile\\") + rFilename;
                FileStream xNewFile = new FileStream(xFilePath, FileMode.Create);

                xNewFile.Write(fileByte, 0, fileByte.Length); // byte 배열 내용파일 쓰는 처리
                xNewFile.Close(); // 파일 닫는 처리

                Response.Clear();

                FileInfo xfileinfo = new FileInfo(xFilePath);

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
                Response.Flush();


                if (File.Exists(xFilePath))
                    File.Delete(xFilePath);

                Response.End();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }


        public string SaveFile(string rFilename, string aa)
        {
            string xFilePath = string.Empty;
            try
            {
                string[] xParams = new string[2];
                xParams[0] = Request.QueryString["rseq"].ToString();
                xParams[1] = rFilename;

                object obj = null;
                obj = SBROKER.GetObject("CLT.WEB.BIZ.LMS.COMMUNITY.vp_y_community_notice_md",
                                        "GetNoticeFile",
                                        LMS_SYSTEM.COMMUNITY,
                                        "CLT.WEB.UI.LMS.COMMUNITY.notice_detail", (object)xParams);


                byte[] fileByte = (byte[])obj;


                xFilePath = Server.MapPath("\\file\\tempfile\\") + rFilename;
                FileStream xNewFile = new FileStream(xFilePath, FileMode.Create);

                xNewFile.Write(fileByte, 0, fileByte.Length); // byte 배열 내용파일 쓰는 처리
                xNewFile.Close(); // 파일 닫는 처리

                Response.Clear();

                FileInfo xfileinfo = new FileInfo(xFilePath);

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
                Response.Flush();


                if (File.Exists(xFilePath))
                    File.Delete(xFilePath);

                Response.End();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xFilePath;
        }
        #endregion


        /************************************************************
        * Function name : btnDelete
        * Purpose       : 공지사항 Delete 메서드

        * Input         : void
        * Output        : void
        *************************************************************/
        public void buttonDelete(string rSeq, string rDelYn)
        {
            try
            {
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;

                string[] xParams = new string[2];
                xParams[0] = rSeq;
                xParams[1] = rDelYn;

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMUNITY.vp_y_community_notice_md",
                    "SetNoticeDelete",
                    LMS_SYSTEM.COMMUNITY,
                    "CLT.WEB.UI.LMS.COMMUNITY.notice_detail",
                    xParams);

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


                //button_Click(btnList, new EventArgs());

                xScriptMsg = "<script>window.location.href='/community/notice_list.aspx?BIND=BIND';</script>";
                ScriptHelper.ScriptBlock(this, "notice_list", xScriptMsg);

                //Response.Redirect("/community/notice_list.aspx");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }


        protected void LnkBtnDown1_Click(object sender, EventArgs e)
        {
            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
        }


    }
}
