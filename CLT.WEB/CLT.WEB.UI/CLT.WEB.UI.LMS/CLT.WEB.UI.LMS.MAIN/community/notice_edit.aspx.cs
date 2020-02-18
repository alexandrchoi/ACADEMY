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
using System.IO;
using System.Web.UI.MobileControls;
using System.Collections.Generic;

namespace CLT.WEB.UI.LMS.COMMUNITY
{
    /// <summary>
    /// 1. 작업개요 : 교육안내 조회 Class
    /// 
    /// 2. 주요기능 : LMS 교육안내 조회 관리
    ///				  
    /// 3. Class 명 : notice_edit
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02.
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    /// 
    /// </summary>
    public partial class notice_edit : BasePage
    {
        public static ArrayList ialFileList = new ArrayList();
        public static ArrayList ialDeleteList = new ArrayList(); // 삭제할 기존 첨부파일 리스트 변수        
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
                txtCONTENT.Text = txt_content.Text;

                //this.Page.Form.DefaultButton = this.btn.UniqueID; // Page Default Button Mapping
                if (Request.QueryString["EDITMODE"] != null && Request.QueryString["EDITMODE"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        BindDropDownList();
                        this.txtNotice_From.Attributes.Add("onkeyup", "ChkDate(this);");
                        this.txtNotice_To.Attributes.Add("onkeyup", "ChkDate(this);");

                        base.pRender(this.Page,
                                     new object[,] {
                                             { this.btnSave, "E" },
                                             { this.btnList, "I" },
                                             { this.btnUpload, "I" },
                                             { this.btnRemove, "E" },
                                               },
                                     Convert.ToString(Request.QueryString["MenuCode"]));

                        getAuth();

                        //btnSave.Visible = AuthWrite;

                        if (Request.QueryString["EDITMODE"] != null)
                        {
                            if (Request.QueryString["SEQ"] != null)
                            {
                                ViewState["SEQ"] = Request.QueryString["SEQ"].ToString();
                                EditMode(ViewState["SEQ"].ToString());
                            }

                            ViewState["EDITMODE"] = Request.QueryString["EDITMODE"].ToString();


                        }
                        else
                            return;

                        //if (Request.QueryString["EDIT_MODE"] != null && Request.QueryString["SEQ"] != null)
                        //{
                        //    EditMode(Request.QueryString["SEQ"]);
                        //}
                    }

                    if (this.lbSentlist.Items.Count == 0)
                        ialFileList.Clear();

                    if (this.lbDeleteList.Items.Count == 0)
                        ialDeleteList.Clear();
                }
                else
                {
                    this.btnSave.Visible = false;
                    this.btnUpload.Visible = false;
                    this.btnRemove.Visible = false;

                    //return;
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "notice_list", xScriptMsg);
                }
                this.btnRewrite.Visible = false;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnSelect_OnClick
        * Purpose       : 과정선택 버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        /*
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
                                       LMS_SYSTEM.COMMUNITY,
                                       "CLT.WEB.UI.LMS.MANAGE.notice_edit", (object)xParams);

                WebControlHelper.SetDropDownList(this.ddlCus_Date, xDt, "course_date", "open_course_id");
                this.txtCus_ID.Text = HiddenCourseID.Value;
                this.txtCus_NM.Text = HiddenCourseNM.Value;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        */

        /************************************************************
        * Function name : Button_Click
        * Purpose       : 저장(Save), 조회(List), 초기화(Rewrite)  버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void Button_Click(object sender, EventArgs e)
        protected void Button_Click(object sender, EventArgs e)
        {
            try
            {
                Button xbtn = (Button)sender;

                if (xbtn.ID == "btnSave")
                {
                    // 필수입력값 체크
                    if (string.IsNullOrEmpty(txtNotice_From.Text))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "게시 일자" }, new string[] { "Notice Date" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    else if (string.IsNullOrEmpty(txtNotice_To.Text))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "게시 일자" }, new string[] { "Notice Date" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    else if (string.IsNullOrEmpty(txtSubject.Text))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "제목" }, new string[] { "Title" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    else if (string.IsNullOrEmpty(txtCONTENT.Text))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "내용" }, new string[] { "Content" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }

                    int ChkByte = 0;
                    foreach (FileUpload fUpload in ialFileList)
                    {
                        ChkByte = ChkByte + fUpload.PostedFile.ContentLength;
                    }
                    /*
                    if (ChkByte > 2097151)  // 모든 첨부파일의 합이 2메가보다 크면
                    {
                        // 첨부파일이 2메가 보다 큽니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A030", new string[] { "첨부파일", "2메가" }, new string[] { "Attachment", "2MB" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    */
                    if (ViewState["EDITMODE"].ToString() == "EDIT" && ViewState["SEQ"].ToString() != null)
                        buttonUpdate(xbtn, Request.QueryString["SEQ"]);
                    else
                        buttonSave(xbtn);

                }
                else if (xbtn.ID == "btnList")
                    buttonList(xbtn);
                else if (xbtn.ID == "btnRewrite")
                    buttonRewrite(xbtn);
                else if (xbtn.ID == "btnCancel")
                {
                    string xScriptMsg = "";

                    if (ViewState["EDITMODE"].ToString() == "EDIT" && ViewState["SEQ"].ToString() != null)
                        xScriptMsg = string.Format("<script>window.location.href = '/community/notice_detail.aspx?rseq={0}&MenuCode={1}&delYN={2}';</script>", ViewState["SEQ"].ToString(), Session["MENU_CODE"], Request.QueryString["delYN"]);
                    else
                        xScriptMsg = string.Format("<script>window.location.href = '/community/notice_list.aspx?MenuCode={0}&delYN={1}';</script>", Session["MENU_CODE"], Request.QueryString["delYN"]);

                    ScriptHelper.ScriptBlock(this, "notice_list", xScriptMsg);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : btnUpload_OnClick
        * Purpose       : 첨부파일 업로드 버튼 이벤트
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
                /*
                if (this.FileUpload1.PostedFile.ContentLength > 2097151)
                {
                    // 첨부파일이 2메가 보다 큽니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A030", new string[] { "첨부파일", "2메가" }, new string[] { "Attachment", "2MB" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                */
                int xfileSize = this.FileUpload1.PostedFile.ContentLength;

                if (this.lbSentlist.Items.Count >= 3) // 첨부파일은 최대 3개까지 가능
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A007", new string[] { "첨부파일", "3개" }, new string[] { "Attachment", "3 File" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                //else if (lbSentlist.Items.Contains(new ListItem(this.FileUpload1.FileName)))
                //{
                //    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A007", new string[] { "첨부파일", "1개" }, new string[] { "Attachment", "1 File" }, Thread.CurrentThread.CurrentCulture));
                //    return;
                //}

                if (!ialFileList.Contains(this.FileUpload1))
                    ialFileList.Add(this.FileUpload1);


                ListItem Items = new ListItem();


                lbSentlist.Items.Add(new ListItem(this.FileUpload1.FileName, "NEW"));

                //if (!ialFileList.Contains(this.FileUpload1))
                //    ialFileList.Add(this.FileUpload1);

                //lbSentlist.Items.Add(new ListItem(this.FileUpload1.FileName, "NEW"));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnUpload_OnClick
        * Purpose       : 첨부파일 삭제 버튼 이벤트
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
                        //lbSentlist.Items.Remove(lbSentlist.SelectedItem);
                        break;
                    }
                }

                if (lbSentlist.SelectedItem.Value == "EDIT")  // 기존 첨부파일이 존재 할 경우
                {
                    //if (!ialDeleteList.Contains(lbSentlist.SelectedItem.Text))  // 동일한 Data가 없으면...
                    //    ialDeleteList.Add(lbSentlist.SelectedItem.Text);

                    if (!this.lbDeleteList.Items.Contains(new ListItem(lbSentlist.SelectedItem.Text)))
                        this.lbDeleteList.Items.Add(new ListItem(lbSentlist.SelectedItem.Text));
                }

                this.lbSentlist.Items.Remove(lbSentlist.SelectedItem);

                //if (lbSentlist.SelectedItem.Value == "EDIT")  // 기존 첨부파일이 존재 할 경우
                //{
                //    if (!ialDeleteList.Contains(lbSentlist.SelectedItem.Text))  // 동일한 Data가 없으면...
                //        ialDeleteList.Add(lbSentlist.SelectedItem.Text);
                //}

                //this.lbSentlist.Items.Remove(lbSentlist.SelectedItem);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion



        /************************************************************
        * Function name : buttonSave
        * Purpose       : 공지사항 내용 저장 처리(신규 공지사항 저장)
        * Input         : void
        * Output        : void
        *************************************************************/
        #region public void buttonSave(Button rbtn)
        public void buttonSave(Button rbtn)
        {
            try
            {
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;


                string xSeq = string.Empty;
                string[] xSeqParams = new string[2];
                xSeqParams[0] = "not_no";
                xSeqParams[1] = "t_notice";



                xSeq = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMUNITY.vp_y_community_notice_md",
                    "GetMaxIDOfCode",
                    LMS_SYSTEM.COMMUNITY,
                    "CLT.WEB.UI.LMS.COMMUNITY.notice_edit",
                    (object)xSeqParams);


                object[] xParams = new object[2];

                string[] xMasterParams = new string[10];

                // 일반 공지사항인지 과정 공지사항인지 확인
                //if (ddlCus_Date.SelectedItem == null)
                //{
                //    xMasterParams[0] = "000001"; // m_cd : 0055, 000001 : 일반공지, 000002 : 과정공지
                //    xMasterParams[1] = string.Empty;
                //}
                //else
                //{
                xMasterParams[0] = "000001";  // 공지사항
                /*
                if (ddlCus_Date.SelectedItem == null)
                {
                    xMasterParams[1] = string.Empty;
                }
                else
                {
                    xMasterParams[1] = this.ddlCus_Date.SelectedItem.Value;  // 과정 ID
                }
                */
                xMasterParams[1] = string.Empty;

                xMasterParams[2] = txtNotice_From.Text; // 게시 시작일자
                xMasterParams[3] = txtNotice_To.Text; // 게시 종료일자
                xMasterParams[4] = txtSubject.Text.Replace("'", "''"); // 공지사항 제목
                xMasterParams[5] = txtCONTENT.Text.Replace("'", "''"); // 공지사항 내용
                xMasterParams[6] = Session["USER_ID"].ToString(); // 사용자 ID
                xMasterParams[7] = xSeq; // 공지사항 ID
                if (this.chkSent.Checked == true)
                    xMasterParams[8] = "1"; // 선박발송여부  1 : 전송대상
                else
                    xMasterParams[8] = "3"; // 선박발송여부  3 : 전송안함

                if (this.ddlNoticeType.SelectedItem.Text == "*")
                    xMasterParams[9] = "000000";  // 전체 공지사항
                else
                    xMasterParams[9] = this.ddlNoticeType.SelectedValue;

                object[,] xDetailParams = new object[ialFileList.Count, 3];

                int nCount = 0;

                foreach (FileUpload attfile in ialFileList)
                {
                    //HttpPostedFile xFile = (HttpPostedFile)attfile.PostedFile;
                    //Stream stream = xFile.InputStream;

                    //int xGetbyte = 0;
                    //List<byte> bytes = new List<byte>();
                    //while ((xGetbyte = stream.ReadByte()) > -1)
                    //{
                    //    bytes.Add((byte)xGetbyte);
                    //}

                    //stream.Close();
                    //List<byte> bytes = new List<byte>();


                    string[] ffiles = attfile.FileName.Split(new string[] { "\\" }, StringSplitOptions.None);

                    for (int i = 0; i < ffiles.Length; i++)
                    {
                        ffiles[i] = ffiles[i].Replace(" ", "_");
                    }

                    byte[] bytes = ConvertToFileUpload(attfile);

                    xDetailParams[nCount, 0] = bytes; //bytes.ToArray();
                    xDetailParams[nCount, 1] = ffiles;//attfile.FileName;
                    nCount++;

                }

                xParams[0] = xMasterParams;
                xParams[1] = xDetailParams;

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMUNITY.vp_y_community_notice_md",
                    "SetNotice",
                    LMS_SYSTEM.COMMUNITY,
                    "CLT.WEB.UI.LMS.COMMUNITY",
                    (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    ViewState["EDITMODE"] = "EDIT";
                    ViewState["SEQ"] = xSeq;

                    ialFileList.Clear();
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    ialFileList.Clear();
                    this.lbSentlist.Items.Clear();
                }
                //Response.Redirect("/CLT.WEB.UI.LMS.COMMUNITY/notice_list.aspx?BIND=BIND");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            //finally
            //{
            //    ialFileList.Clear();
            //    this.lbSentlist.Items.Clear();
            //}
        }
        #endregion


        /************************************************************
        * Function name : buttonUpdate
        * Purpose       : 공지사항 내용 저장 처리(기존 공지사항 저장)
        * Input         : void
        * Output        : void
        *************************************************************/
        #region public void buttonUpdate(Button rbtn, string rSeq)
        public void buttonUpdate(Button rbtn, string rSeq)
        {
            try
            {
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;


                object[] xParams = new object[3];

                string[] xMasterParams = new string[10];

                xMasterParams[0] = "000001";  // 공지사항
                /*
                // 일반 공지사항인지 과정 공지사항인지 확인
                if (ddlCus_Date.SelectedItem == null)
                {
                    //xMasterParams[0] = "000001"; // m_cd : 0055, 000001 : 일반공지, 000002 : 과정공지
                    xMasterParams[1] = string.Empty;
                }
                else
                {
                    xMasterParams[1] = txtCus_ID.Text;  // 과정 ID
                }
                */
                xMasterParams[1] = string.Empty;

                xMasterParams[2] = txtNotice_From.Text; // 게시 시작일자
                xMasterParams[3] = txtNotice_To.Text; // 게시 종료일자
                xMasterParams[4] = txtSubject.Text.Replace("'", "''"); // 공지사항 제목
                xMasterParams[5] = txtCONTENT.Text.Replace("'", "''"); // 공지사항 내용
                xMasterParams[6] = Session["USER_ID"].ToString(); // 사용자 ID
                xMasterParams[7] = rSeq; // 기존 공지사항 번호

                if (this.chkSent.Checked == true)
                    xMasterParams[8] = "1"; // 선박발송여부  1 : 전송대상
                else
                    xMasterParams[8] = "3"; // 선박발송여부  3 : 전송안함

                if (this.ddlNoticeType.SelectedItem.Text == "*")
                    xMasterParams[9] = "000000";  // 전체 공지사항
                else
                    xMasterParams[9] = this.ddlNoticeType.SelectedValue;

                string[] xDeleteFile = new string[lbDeleteList.Items.Count];
                // 기존첨부파일에서 삭제할 Data
                //lbDeleteList

                int nCount = 0;
                if (lbDeleteList.Items.Count > 0)
                {
                    foreach (ListItem xFileName in lbDeleteList.Items)
                    {
                        xDeleteFile[nCount] = xFileName.Text;
                        nCount++;
                    }
                }

                //if (ialDeleteList.Count != 0)
                //{
                //    foreach (string xFileName in ialDeleteList)
                //    {
                //        xDeleteFile[nCount] = xFileName;
                //        nCount++;
                //    }
                //}

                //object[] xDetailParams = new object[ialFileList.Count];
                //for (int i = 0; i < ialFileList.Count; i++)
                //{
                //    FileUpload xUpload = (FileUpload)ialFileList[i];
                //    xDetailParams[i] = xUpload.PostedFile;
                //}


                object[,] xDetailParams = new object[ialFileList.Count, 3];

                nCount = 0;
                foreach (FileUpload attfile in ialFileList)
                {
                    HttpPostedFile xFile = (HttpPostedFile)attfile.PostedFile;
                    Stream stream = xFile.InputStream;

                    string[] ffiles = xFile.FileName.Split(new string[] { "\\" }, StringSplitOptions.None);


                    //xFileName = "F:\\" + xFile.FileName;
                    //if (xFile.FileName.Contains("\\"))
                    //{

                    //}
                    int xGetbyte = 0;
                    List<byte> bytes = new List<byte>();


                    while ((xGetbyte = stream.ReadByte()) > -1)
                    {
                        bytes.Add((byte)xGetbyte);
                    }

                    stream.Close();

                    xDetailParams[nCount, 0] = bytes.ToArray();
                    xDetailParams[nCount, 1] = ffiles[ffiles.Length - 1].Replace(" ", "_"); //xFile.FileName;
                    xDetailParams[nCount, 2] = rSeq;
                    nCount++;

                }

                xParams[0] = xMasterParams;
                xParams[1] = xDetailParams;
                xParams[2] = xDeleteFile;


                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMUNITY.vp_y_community_notice_md",
                    "SetNoticeUpdate",
                    LMS_SYSTEM.COMMUNITY,
                    "CLT.WEB.UI.LMS.COMMUNITY",
                    (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    ViewState["EDITMODE"] = "EDIT";
                    ViewState["SEQ"] = rSeq;

                    // 처리가 완료되면 초기화
                    ialFileList.Clear();
                    this.lbDeleteList.Items.Clear();
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    ialFileList.Clear();
                    this.lbSentlist.Items.Clear();
                }
                //Response.Redirect("/CLT.WEB.UI.LMS.COMMUNITY/notice_list.aspx?BIND=BIND");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            //finally
            //{
            //    ialFileList.Clear();
            //    ialDeleteList.Clear();
            //    this.lbSentlist.Items.Clear();
            //}
        }
        #endregion


        /************************************************************
        * Function name : buttonList
        * Purpose       : 공지사항 리스트 조회 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region public void buttonList(Button rbtn)
        public void buttonList(Button rbtn)
        {
            try
            {
                Response.Redirect("/community/notice_list.aspx?delYN=" + Request.QueryString["delYN"]);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : buttonRewrite
        * Purpose       : 입력된 공지사항 내용 초기화
        * Input         : void
        * Output        : void
        *************************************************************/
        #region public void buttonRewrite(Button rbtn)
        public void buttonRewrite(Button rbtn)
        {
            try
            {
                //this.txtCus_ID.Text = string.Empty;
                //this.txtCus_NM.Text = string.Empty;
                //this.ddlCus_Date.Text = string.Empty;
                this.txtNotice_From.Text = string.Empty;
                this.txtNotice_To.Text = string.Empty;
                this.txtSubject.Text = string.Empty;
                this.txtCONTENT.Text = string.Empty;
                this.lbSentlist.Items.Clear();
                ialFileList.Clear();
                ialDeleteList.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion


        /************************************************************
        * Function name : EditMode
        * Purpose       : 공지사항 리스트 조회 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region public void EditMode(string rSeq)
        public void EditMode(string rSeq)
        {
            try
            {
                ialDeleteList.Clear(); // 기존 첨부파일 List 초기화

                //this.ddlCus_Date.Items.Clear();
                this.lbSentlist.Items.Clear();

                string[] xParams = new string[2];
                xParams[0] = rSeq;
                xParams[1] = Session["USER_GROUP"].ToString();

                DataTable xDt = new DataTable();
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMUNITY.vp_y_community_notice_md",
                                       "GetNoticeDetail",
                                       LMS_SYSTEM.COMMUNITY,
                                       "CLT.WEB.UI.LMS.COMMUNITY.notice_edit", (object)xParams, Thread.CurrentThread.CurrentCulture);

                if (xDt == null || xDt.Rows.Count == 0)
                    return;

                //this.txtCus_ID.Text = "";
                //this.txtCus_NM.Text = "";
                /*
                if (!string.IsNullOrEmpty(xDt.Rows[0]["course_id"].ToString()))
                {
                    //this.txtCus_ID.Text = xDt.Rows[0]["course_id"].ToString();
                    //this.txtCus_NM.Text = xDt.Rows[0]["course_nm"].ToString();
                    HiddenCourseID.Value = xDt.Rows[0]["course_id"].ToString();
                    HiddenCourseNM.Value = xDt.Rows[0]["course_nm"].ToString();
                    btnSelect_OnClick(null, null);

                    WebControlHelper.SetSelectItem_DropDownList(this.ddlCus_Date, xDt.Rows[0]["open_course_id"].ToString());
                }
                */
                this.txtSubject.Text = xDt.Rows[0]["not_sub"].ToString();  // 제목   
                this.txtCONTENT.Text = xDt.Rows[0]["content"].ToString(); // 공지사항 내용
                this.txtNotice_From.Text = xDt.Rows[0]["notice_begin_dt"].ToString(); // 공지사항 게시 시작일자
                this.txtNotice_To.Text = xDt.Rows[0]["notice_end_dt"].ToString(); // 공지사항 게시 종료일자


                DataTable xDtAtt = new DataTable();
                xDtAtt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMUNITY.vp_y_community_notice_md",
                                          "GetNoticeFileName",
                                          LMS_SYSTEM.COMMUNITY,
                                          "CLT.WEB.UI.LMS.COMMUNITY.notice_edit", (object)rSeq);

                foreach (DataRow xDr in xDtAtt.Rows)
                {
                    lbSentlist.Items.Add(new ListItem(xDr["file_nm"].ToString(), "EDIT"));
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


                // 회사구분 (그룹사, 사업자 위수탁)
                xParams[0] = "0061";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.COMMUNITY,
                                             "CLT.WEB.UI.LMS.COMMUNITY", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlNoticeType, xDt);

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
