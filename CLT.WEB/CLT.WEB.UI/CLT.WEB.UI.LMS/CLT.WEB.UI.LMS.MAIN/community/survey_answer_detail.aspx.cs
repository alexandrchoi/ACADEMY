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

namespace CLT.WEB.UI.LMS.COMMUNITY
{
    /// <summary>
    /// 1. 작업개요 : 설문 등록 Class
    /// 
    /// 2. 주요기능 : LMS 설문 등록
    ///				  
    /// 3. Class 명 : survey_answer_detail
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.01
    ///
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class survey_answer_detail : BasePage
    {
        // Layout.css 파일에 있는 각 Layout 값을 전역변수로 지정하여 사용...(동적 컨트롤 생성용)
        //string tanbleStyle = "background-color:White; overflow:auto;";
        //string pop_left = "background-color:#E3EBEF;height: 27px;padding-left:5px; border-bottom:#C5D3DE solid 1px;border-top:#C5D3DE solid 1px;";
        //string pop_right = " padding-left:5px;border-bottom:#C5D3DE solid 1px;border-top:#C5D3DE solid 1px; text-align:left;";
        
        /************************************************************
        * Function name : Page_Load 
        * Purpose       : 웹페이지 Load 이벤트
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
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "survey_answer_detail", xScriptMsg);

                    return;
                }

                if (Request.QueryString["rResNo"] != null && Request.QueryString["rResNo"] != "")
                {
                    if (!IsPostBack)
                    {
                        //this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                        base.pRender(this.Page,
                                     new object[,] { { this.btnSave, "E" },
                                                     { this.btnSend, "E" }
                                     },
                                     Convert.ToString(Request.QueryString["MenuCode"]));

                        this.txtRes_SUB.Text = Request.QueryString["rRes_sub"].ToString();
                        this.txtRes_Obj.Text = Request.QueryString["rRes_object"].ToString();

                        string[] xResDate = Request.QueryString["rRes_date"].ToString().Split('~');
                        int xCount = 0;
                        foreach (string xDate in xResDate)
                        {
                            if (xCount == 0)
                                this.txtRes_From.Text = xDate.Trim();
                            else if (xCount == 1)
                                this.txtRes_To.Text = xDate.Trim();

                            xCount++;
                        }
                        ViewState["ANSWER_YN"] = Request.QueryString["rAnswer_yn"].ToString();
                    }
                    AddContent(Request.QueryString["rResNo"]);
                }
                else
                {
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
        * Function name : btnList_OnClick
        * Purpose       : 설문 리스트로 이동
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnList_OnClick(object sender, EventArgs e)
        protected void btnList_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/community/survey_answer_list.aspx");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : RadioButton_CheckedChanged
        * Purpose       : 입력된 설문 내용 저장버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnSave_OnClick(object sender, EventArgs e)
        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["ANSWER_YN"].ToString() == "Y")
                {
                    // 게시된 설문은 수정 할 수 없습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A116", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                AnswerInsert(Convert.ToInt32(ViewState["COUNT"].ToString()));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnSend_OnClick
        * Purpose       : 입력된 설문 내용 전송버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnSend_OnClick(object sender, EventArgs e)
        protected void btnSend_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["ANSWER_YN"].ToString() == "Y")
                {
                    // 게시된 설문은 수정 할 수 없습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A118", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                string xMessage = Validateion(Convert.ToInt32(ViewState["COUNT"].ToString()));
                if (!string.IsNullOrEmpty(xMessage))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A016", new string[] { xMessage + "번 설문" }, new string[] { "Q" + xMessage }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                AnswerInsert(Convert.ToInt32(ViewState["COUNT"].ToString()));
                string xRtn = Boolean.FalseString;  // Update 후 결과값 Return

                string[] xParams = new string[2];
                xParams[0] = Request.QueryString["rResNo"];
                xParams[1] = Session["USER_ID"].ToString();
                // 사용자 정보는 Delete 하지 않고 Status 를 사용안함(000002) 으로 처리
                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                 "SetSurveyAnswer",
                 LMS_SYSTEM.COMMUNITY,
                 "CLT.WEB.UI.LMS.COMMUNITY",
                 (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    ViewState["ANSWER_YN"] = "Y";
                    //BindGrid();
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        #region private void chkBox_CheckedChanged(object sender, EventArgs e)
        private void chkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkBox = (CheckBox)sender;

                string xlblID = string.Empty;
                string xSeq = string.Empty;
                
                if (chkBox.ID.Length == 8)
                {
                    xlblID = chkBox.ID.Substring(6, 1);
                    xSeq = chkBox.ID.Substring(7, 1);
                }
                else if (chkBox.ID.Length == 9)
                {
                    xlblID = chkBox.ID.Substring(6, 2);
                    xSeq = chkBox.ID.Substring(8, 1);
                }

                Label lbSeq = (Label)this.ph01.FindControl("lblSeq" + xlblID);

                if (chkBox.Checked == true)
                {
                    if (string.IsNullOrEmpty(lbSeq.Text))
                        lbSeq.Text = xSeq;
                    else
                        lbSeq.Text = lbSeq.Text + ", " + xSeq;
                }
                else
                {
                    string[] xSeqs = lbSeq.Text.Split(',');
                    lbSeq.Text = string.Empty;
                    foreach (string seq in xSeqs)
                    {
                        if (xSeq != seq.Trim())
                        {
                            if (string.IsNullOrEmpty(lbSeq.Text))
                                lbSeq.Text = seq.Trim();
                            else
                                lbSeq.Text += ", " + seq.Trim();
                        }
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
        * Function name : AddContent 
        * Purpose       : 웹페이지 설문조사 서식 동적컨트롤 생성 메서드
        * Input         : string rResNo : 설문ID
        * Output        : void
        *************************************************************/
        #region AddContent
        public void AddContent(string rResNo)
        {
            try
            {
                int xResCnt = 1;
                string[] xParams = new string[2];
                xParams[0] = rResNo;

                DataTable xDtDetail = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                                       "GetResearchDetailList",
                                                        LMS_SYSTEM.COMMUNITY,
                                                       "CLT.WEB.UI.LMS.COMMUNITY", (object)xParams);

                int xCount = xDtDetail.Rows.Count; // 설문문항 갯수
                ViewState["COUNT"] = xCount;
                
                string[] xRParams = new string[2];
                xRParams[0] = Session["USER_ID"].ToString();
                xRParams[1] = Request.QueryString["rResNo"].ToString();

                DataTable xDtResult = null;
                xDtResult = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                                   "Get_Survey_DB_Detail",
                                                    LMS_SYSTEM.COMMUNITY,
                                                   "CLT.WEB.UI.LMS.COMMUNITY", (object)xRParams);
                
                for (int i = 0; i < xCount; i++)
                {
                    // 설문조사 테이블 생성...
                    // 테이블은 설문 문항 숫자만큼 생성한다..
                    Table xTB_RES = new Table();
                    xTB_RES.ID = "TB_RES" + i.ToString();  // 테이블 ID : TB_RES + 설문숫자 
                    //xTB_RES.Style.Value = tanbleStyle;
                    //xTB_RES.BorderWidth = 0;
                    //xTB_RES.CellPadding = 0;
                    //xTB_RES.CellSpacing = 0;
                    //xTB_RES.Style.Add(HtmlTextWriterStyle.Width, "98%");
                    //xTB_RES.Style.Value = "border-bottom:solid 10px #FFFFFF;Width:98%";
                    /*
                     *  TableRow 생성규칙
                     *  첫번째 Row 00
                     *  두번째 Row 01
                     */
                    TableRow xTR_01 = new TableRow();  // 설문 할  질문
                    //TableRow xTR_02 = new TableRow();  // 보기형태
                    //TableRow xTR_03 = new TableRow();  // 보기 항목수                    
                    TableRow xTR_04 = new TableRow();  // 보기 1
                    /*
                     * TableCell 생성규칙
                     * 첫번째 Row 첫번째 Cell 01
                     * 첫번째 Row 두번쨰 Cell 02
                     * 두번째 Row 첫번째 Cell 11
                     * 두번쨰 Row 두번째 Cell 12
                     */
                    TableHeaderCell xTC_01 = new TableHeaderCell();  // 설문 번호
                    TableHeaderCell xTC_02 = new TableHeaderCell();  // 설문 항목
                    TableCell xTC_03 = new TableCell();  // Textbox 컨트롤 

                    TableHeaderCell xTC_11 = new TableHeaderCell();  // 보기형태
                    TableCell xTC_12 = new TableCell();  // 보기 RadioBox

                    TableHeaderCell xTC_21 = new TableHeaderCell(); // 보기 항목수 제목
                    TableCell xTC_22 = new TableCell(); // 보기 항목수 컨트롤

                    TableHeaderCell xTC_31 = new TableHeaderCell(); // 보기1 제목
                    TableCell xTC_32 = new TableCell(); // 보기1 TextBox
                    
                    // 첫번째 Row Rospan 을 설정하는 Cell
                    //xTC_01.Style.Value = pop_left;
                    xTC_01.Style.Add(HtmlTextWriterStyle.Width, "15%");

                    if (IsSettingKorean())
                        xTC_01.Text = Convert.ToString(i + 1) + " 번 설문";
                    else
                        xTC_01.Text = "Q" + xResCnt.ToString();

                    xTC_01.ID = "TC_0" + i.ToString();

                    //xTC_02.Style.Value = pop_left;
                    xTC_02.Style.Add(HtmlTextWriterStyle.Width, "15%");

                    if (IsSettingKorean())
                        xTC_02.Text = "질문";
                    else
                        xTC_02.Text = "Question";

                    //xTC_03.Style.Value = pop_right;
                    //xTC_03.Style.Add(HtmlTextWriterStyle.Width, "87%");
                    //xTC_03.ColumnSpan = 3;
                    //TextBox txtRes_Content = new TextBox();
                    //txtRes_Content.Style.Add(HtmlTextWriterStyle.Width, "98%");
                    //txtRes_Content.ID = "txtRes_Content" + i.ToString();
                    //xTC_03.Controls.Add(txtRes_Content);

                    Label lblRes_Content = new Label();
                    //lblRes_Content.Style.Add(HtmlTextWriterStyle.Width, "75%");
                    lblRes_Content.ID = "txtRes_Content" + i.ToString();
                    xTC_03.Controls.Add(lblRes_Content);
                    
                    //Label lblSeq = new Label();
                    //lblSeq.Font.Bold = true;
                    //lblSeq.Style.Value = "text-align:right;";
                    //lblSeq.Width = 90;
                    
                    //lblSeq.ID = "lblSeq" + Convert.ToInt32(i + 1).ToString();
                    //xTC_03.Controls.Add(lblSeq);
                    
                    // Edit 모드 이므로 Data를 가져온다.
                    lblRes_Content.Text = xDtDetail.Rows[i]["res_content"].ToString();

                    int xRowCount = Convert.ToInt32(xDtDetail.Rows[i]["example_cnt"].ToString()); // 보기 항목 갯수

                    // Edit 모드 이므로 설문유형 Data를 가져온다.
                    if (xDtDetail.Rows[i]["res_type"].ToString() == "000001") // 단일선택일 경우
                    {
                        if (IsSettingKorean())
                            lblRes_Content.Text += " (단일선택)";
                        else
                            lblRes_Content.Text += " (Single Selection)";
                    }
                    else if (xDtDetail.Rows[i]["res_type"].ToString() == "000002") // 다중선택일 경우
                    {
                        if (IsSettingKorean())
                            lblRes_Content.Text += " (다중선택)";
                        else
                            lblRes_Content.Text += " (Multi Selection)";
                    }
                    else if (xDtDetail.Rows[i]["res_type"].ToString() == "000003") // 서술형일 경우
                    {
                        if (IsSettingKorean())
                            lblRes_Content.Text += " (서술형)";
                        else
                            lblRes_Content.Text += " (Description)";
                    }
                    else if (xDtDetail.Rows[i]["res_type"].ToString() == "000004") // 단일선택(혼합형)일 경우
                    {
                        if (IsSettingKorean())
                            lblRes_Content.Text += " (단일선택[혼합형])";
                        else
                            lblRes_Content.Text += " (Single Selection[Mix])";
                    }
                    else if (xDtDetail.Rows[i]["res_type"].ToString() == "000005") // 다중선택(혼합형)일 경우
                    {
                        if (IsSettingKorean())
                            lblRes_Content.Text += " (다중선택[혼합형])";
                        else
                            lblRes_Content.Text += " (Multi Selection[Mix])";
                    }
                    else if (xDtDetail.Rows[i]["res_type"].ToString() == "000006") // 순서배열 경우
                    {
                        if (IsSettingKorean())
                            lblRes_Content.Text += " (보기 순번을 순서대로 입력해 주세요.)";
                        else
                            lblRes_Content.Text += " (Please enter a sequence number)";

                        TextBox txtSeq = new TextBox();
                        txtSeq.ID = "txtSeq" + Convert.ToString(i + 1);
                        txtSeq.MaxLength = xRowCount;

                        DataRow[] xDr = xDtResult.Select(string.Format("res_que_id = '{0}' AND seq = '{1}'", Convert.ToInt32(i + 1).ToString("000"), Convert.ToString(1)));
                        if (xDr.Length > 0)
                            txtSeq.Text = xDr[0]["explain"].ToString().Replace(",", "");

                        string chkNum = "/[^1-" + xRowCount.ToString() + "]/";

                        txtSeq.Attributes.Add("onkeyup", "ChkNum(this," + chkNum + ");");
                        //txtCreated_From.Attributes.Add("onkeyup", "ChkDate(this);");
                        xTC_03.Controls.Add(txtSeq);
                    }
                    
                    // Row에 Cell 추가
                    xTR_01.Controls.Add(xTC_01);
                    xTR_01.Controls.Add(xTC_02);
                    xTR_01.Controls.Add(xTC_03);
                    
                    //xTR_02.Controls.Add(xTC_11);
                    //xTR_02.Controls.Add(xTC_12);
                    
                    //xTR_03.Controls.Add(xTC_21);
                    //xTR_03.Controls.Add(xTC_22);
                    
                    // Table 에 Row 추가
                    xTB_RES.Controls.Add(xTR_01);
                    //xTB_RES.Controls.Add(xTR_02);
                    //xTB_RES.Controls.Add(xTR_03);
                    
                    int xNonDisplayCnt = 0;

                    xTC_01.RowSpan = xTC_01.RowSpan + xRowCount;  // 기본 설문 + 보기문항
                    //xTC_01.RowSpan = 1 +  xRowCount;  // 기본 설문 + 보기문항

                    // 사용자가 선택한 숫자만큼 보기문항 추가
                    
                    // 사용자가 입력한 보기문항 갯수 가져오기                    
                    int xExample_Cnt = Convert.ToInt32(xDtDetail.Rows[i]["example_cnt"].ToString());

                    xParams[1] = xDtDetail.Rows[i]["res_que_id"].ToString();
                    // res_que_id, seq, Example
                    DataTable xDtChoice = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                                           "GetResearchChoiceList",
                                                            LMS_SYSTEM.COMMUNITY,
                                                           "CLT.WEB.UI.LMS.COMMUNITY", (object)xParams);

                    for (int k = 0; k < xRowCount; k++)
                    {
                        // 저장된 Data가 있는지 검색한다.
                        DataRow[] xDrs = null;
                        if (xDtResult != null)
                        {
                            if (xDtResult.Rows.Count != 0)
                                xDrs = xDtResult.Select(string.Format("res_que_id = '{0}' AND seq = '{1}'", Convert.ToInt32(i + 1).ToString("000"), Convert.ToString(k + 1)));

                        }
                        
                        TableRow TR_EX = new TableRow();

                        TableHeaderCell TC_01 = new TableHeaderCell();
                        TableCell TC_02 = new TableCell();

                        TR_EX.ID = "TR_EX" + i.ToString() + k.ToString();

                        //if (k >= xExample_Cnt) // 사용자가 처음에 입력한 보기문항을 제외한 나머지 보기항목을 숨김처리
                        //{
                        //    TR_EX.Style.Add(HtmlTextWriterStyle.Display, "none");
                        //    xNonDisplayCnt++;
                        //}

                        //TC_01.Style.Value = pop_left;

                        if (xDtDetail.Rows[i]["res_type"].ToString() != "000003")
                        {
                            if (IsSettingKorean())
                                TC_01.Text = "보기 " + Convert.ToString(k + 1);
                            else
                                TC_01.Text = "A" + Convert.ToString(k + 1);
                        }
                        else
                        {
                            if (IsSettingKorean())
                                TC_01.Text = "답변 ";
                            else
                                TC_01.Text = "A";
                        }
                        
                        //TC_02.Style.Value = pop_right;
                        //TC_02.ColumnSpan = 3;

                        TextBox txtExam00 = new TextBox();
                        Label lblExam00 = new Label();
                        if (xDtDetail.Rows[i]["res_type"].ToString() == "000003")
                        {
                            txtExam00.ID = "txtExam" + Convert.ToInt32(i + 1).ToString() + Convert.ToInt32(k + 1).ToString();
                            //txtExam00.Style.Add(HtmlTextWriterStyle.Width, "90%");
                            
                            if (xDrs != null)
                            {
                                if (xDrs.Length != 0)
                                {
                                    foreach (DataRow xDr in xDrs)
                                    {
                                        if (xDr["explain"].ToString() != " ")
                                            txtExam00.Text = xDr["explain"].ToString();
                                        else
                                            txtExam00.Text = string.Empty;
                                    }
                                }
                            }

                        }
                        else
                        {
                            lblExam00.ID = "lblExam" + Convert.ToInt32(i + 1).ToString() + Convert.ToInt32(k + 1).ToString();
                            //lblExam00.Style.Add(HtmlTextWriterStyle.Width, "90%");
                        }
                        
                        if (k < xExample_Cnt)
                        {
                            if (xDtDetail.Rows[i]["res_type"].ToString() == "000003")
                            {
                                //if (xDtChoice.Rows[k]["example"].ToString() != " ")
                                //    txtExam00.Text = xDtChoice.Rows[k]["example"].ToString();
                            }
                            else
                            {
                                if (xDtChoice.Rows[k]["example"].ToString() != " ")
                                    lblExam00.Text = xDtChoice.Rows[k]["example"].ToString();
                            }
                        }

                        if (xDtDetail.Rows[i]["res_type"].ToString() != "000002" && xDtDetail.Rows[i]["res_type"].ToString() != "000005" && xDtDetail.Rows[i]["res_type"].ToString() != "000006")  // 다중선택이 아닐 경우...
                        {
                            if (xDtDetail.Rows[i]["res_type"].ToString() != "000003")  // 서술형은 필요없음..
                            {
                                if (xDtChoice.Rows[k]["example"].ToString() == " " || xDtChoice.Rows[k]["example"].ToString() == string.Empty)
                                {
                                    txtExam00.ID = "txtExam" + Convert.ToInt32(i + 1).ToString() + Convert.ToInt32(k + 1).ToString();

                                    if (xDrs != null)
                                    {
                                        if (xDrs.Length > 0)
                                            txtExam00.Text = xDrs[0]["explain"].ToString();
                                    }
                                    //txtExam00.Style.Add(HtmlTextWriterStyle.Width, "90%");
                                    TC_02.Controls.Add(txtExam00);
                                }
                                else
                                {
                                    RadioButton rbtn = new RadioButton();
                                    rbtn.ID = "rbtn" + Convert.ToInt32(i + 1).ToString() + Convert.ToInt32(k + 1).ToString();
                                    rbtn.GroupName = "rbtn" + i.ToString();
                                    rbtn.Text = " ";
                                    TC_02.Controls.Add(rbtn);
                                    if (xDrs != null)
                                    {
                                        if (xDrs.Length != 0)
                                            rbtn.Checked = true;
                                    }
                                }
                            }

                            if (xDtDetail.Rows[i]["res_type"].ToString() == "000003")
                                TC_02.Controls.Add(txtExam00);
                            else
                                TC_02.Controls.Add(lblExam00);

                            TR_EX.Controls.Add(TC_01);
                            TR_EX.Controls.Add(TC_02);
                            
                            xTB_RES.Controls.Add(TR_EX);
                        }
                        else if (xDtDetail.Rows[i]["res_type"].ToString() == "000002" || xDtDetail.Rows[i]["res_type"].ToString() == "000005" || xDtDetail.Rows[i]["res_type"].ToString() == "000006")  // 다중선택이 아닐 경우...
                        {
                            // 혼합형일 경우 Text 박스를 추가한다.
                            if (xDtChoice.Rows[k]["example"].ToString() == " " || xDtChoice.Rows[k]["example"].ToString() == string.Empty)
                            {
                                txtExam00.ID = "txtExam" + Convert.ToInt32(i + 1).ToString() + Convert.ToInt32(k + 1).ToString();
                                //txtExam00.Style.Add(HtmlTextWriterStyle.Width, "90%");
                                TC_02.Controls.Add(txtExam00);
                            }
                            else
                            {
                                CheckBox chkBox = new CheckBox();
                                chkBox.ID = "chkbox" + Convert.ToInt32(i + 1).ToString() + Convert.ToInt32(k + 1).ToString();
                                chkBox.Text = " ";
                                
                                if (xDtDetail.Rows[i]["res_type"].ToString() == "000006") // 순서배열이면 이벤트 추가...
                                {
                                    //TextBox txtSeq = new TextBox();
                                    //txtSeq.ID = "txtSeq" + Convert.ToString(i + 1);
                                    
                                    //xTC_03.Controls.Add(txtSeq);

                                    //chkBox.AutoPostBack = true;
                                    //chkBox.CheckedChanged -= new EventHandler(chkBox_CheckedChanged);
                                    
                                    //chkBox.CheckedChanged += new EventHandler(chkBox_CheckedChanged);

                                    //if (string.IsNullOrEmpty(lblSeq.Text))
                                    //{
                                    //    if (xDrs != null)
                                    //    {
                                    //        if (xDrs.Length > 0)
                                    //        {
                                    //            foreach (DataRow xDr in xDrs)
                                    //            {
                                    //                lblSeq.Text = xDr["explain"].ToString();
                                    //                chkBox.Checked = true;
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //    chkBox.Checked = true;
                                }
                                else
                                {
                                    if (xDrs != null)
                                    {
                                        if (xDrs.Length != 0)
                                            chkBox.Checked = true;
                                    }
                                    TC_02.Controls.Add(chkBox);
                                }
                                TC_02.Controls.Add(lblExam00);
                            }

                            if (TC_01 != null)
                                TR_EX.Controls.Add(TC_01);

                            if (TC_02 != null)
                                TR_EX.Controls.Add(TC_02);
                            
                            xTB_RES.Controls.Add(TR_EX);
                        }
                        
                        if (xDtDetail.Rows[i]["res_type"].ToString() == "000004" || xDtDetail.Rows[i]["res_type"].ToString() == "000005")
                        {

                        }
                        xResCnt++;

                        xTC_01.RowSpan = xTB_RES.Rows.Count - xNonDisplayCnt;  // 숨김처리된 Row 의 갯수만큼 Rowspan 값에서 뺀다.

                        ph01.Controls.Add(xTB_RES);
                    }

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
        * Function name : void AnswerInsert(int rCount)
        * Purpose       : 설문내용 저장(Insert)
        * Input         : int : rCount (문항갯수)
        * Output        : void
        *************************************************************/
        #region AnswerInsert(int rCount)
        public void AnswerInsert(int rCount)
        {
            try
            {
                DataTable xDt = new DataTable();
                DataRow xDr = null;

                xDt.Columns.Add(new DataColumn("colUser_ID", typeof(string)));
                xDt.Columns.Add(new DataColumn("colRes_No", typeof(string)));
                xDt.Columns.Add(new DataColumn("colRes_Que_ID", typeof(string)));
                xDt.Columns.Add(new DataColumn("colSeq", typeof(int)));
                xDt.Columns.Add(new DataColumn("colDuty_Step", typeof(string)));
                xDt.Columns.Add(new DataColumn("colExplain", typeof(string)));

                bool xChk = false;
                for (int i = 0; i < rCount; i++)
                {
                    string xMessage = string.Empty;
                    int xChkCount = 0;
                    bool xSeqChk = false;
                    
                    // 순서정렬 여부 검색
                    TextBox txtSeq = (TextBox)this.ph01.FindControl("txtSeq" + Convert.ToString(i + 1));
                    if (txtSeq != null)
                    {
                        if (!string.IsNullOrEmpty(txtSeq.Text))
                            xSeqChk = true;
                    }

                    //Label lblseqChk = (Label)this.ph01.FindControl("lblSeq" + Convert.ToString(i + 1));
                    //if (lblseqChk != null)
                    //{
                    //    if (!string.IsNullOrEmpty(lblseqChk.Text))
                    //        xSeqChk = true;
                    //}
                    
                    for (int k = 0; k < 8; k++)  // 보기는 최대 7개 까지 이므로 7번 돈다...
                    {
                        RadioButton rbtn = (RadioButton)this.ph01.FindControl("rbtn" + Convert.ToString(i + 1) + Convert.ToString(k + 1));  // 컨트롤 검색
                        CheckBox chkBox = (CheckBox)this.ph01.FindControl("chkBox" + Convert.ToString(i + 1) + Convert.ToString(k + 1));
                        TextBox txtBox = (TextBox)this.ph01.FindControl("txtExam" + Convert.ToString(i + 1) + Convert.ToString(k + 1));
                        //Label lblseq = (Label)this.ph01.FindControl("lblSeq" + Convert.ToString(i + 1));

                        if (rbtn != null)
                        {
                            if (rbtn.Checked == true)
                            {
                                xChk = true;
                                
                                xDr = xDt.NewRow();
                                xDr["colUser_ID"] = Session["USER_ID"].ToString();  // 사용자 ID
                                xDr["colRes_No"] = Request.QueryString["rResNo"];  // 설문번호
                                xDr["colRes_Que_ID"] = Convert.ToInt32(i + 1).ToString("000");  // 설문문항 ID
                                xDr["colSeq"] = Convert.ToString(k + 1);  // 보기 순번(답변)
                                xDr["colDuty_Step"] = Session["DUTY_STEP"].ToString();  // 사용자 직급
                                xDr["colExplain"] = string.Empty;  // 서술형 답안

                                xDt.Rows.Add(xDr);
                            }
                        }
                        else if (chkBox != null)
                        {
                            if (chkBox.Checked == true)
                            {
                                xChk = true;

                                if (xSeqChk == false)
                                {
                                    xDr = xDt.NewRow();
                                    xDr["colUser_ID"] = Session["USER_ID"].ToString();  // 사용자 ID
                                    xDr["colRes_No"] = Request.QueryString["rResNo"];  // 설문번호
                                    xDr["colRes_Que_ID"] = Convert.ToInt32(i + 1).ToString("000");  // 설문문항 ID
                                    xDr["colSeq"] = Convert.ToString(k + 1);  // 보기 순번(답변)
                                    xDr["colDuty_Step"] = Session["DUTY_STEP"].ToString();  // 사용자 직급
                                    xDr["colExplain"] = string.Empty;  // 서술형 답안

                                    xDt.Rows.Add(xDr);
                                }
                            }

                            xChkCount++;  // 순서배열일 경우 체크박스 숫자와 라벨에 있는 숫자를 비교한다.
                        }
                        else if (txtBox != null)
                        {
                            if (!string.IsNullOrEmpty(txtBox.Text))
                            {
                                xChk = true;

                                xDr = xDt.NewRow();
                                xDr["colUser_ID"] = Session["USER_ID"].ToString();  // 사용자 ID
                                xDr["colRes_No"] = Request.QueryString["rResNo"];  // 설문번호
                                xDr["colRes_Que_ID"] = Convert.ToInt32(i + 1).ToString("000");  // 설문문항 ID
                                xDr["colSeq"] = Convert.ToString(k + 1); //"1";  // 보기 순번(답변)
                                xDr["colDuty_Step"] = Session["DUTY_STEP"].ToString();  // 사용자 직급
                                xDr["colExplain"] = txtBox.Text.Replace("'", "''"); ;  // 서술형 답안

                                xDt.Rows.Add(xDr);
                            }
                        }
                    }

                    // 순서배열 Insert
                    //Label lblseqInsert = (Label)this.ph01.FindControl("lblSeq" + Convert.ToString(i + 1));
                    //TextBox txtSeq = (TextBox)this.ph01.FindControl("txtSeq" + +Convert.ToString(i + 1));
                    if (txtSeq != null)
                    {
                        if (txtSeq.Text.Length != txtSeq.MaxLength)
                        {
                            if (!string.IsNullOrEmpty(txtSeq.Text))
                                xChk = false;
                        }

                        if (!string.IsNullOrEmpty(txtSeq.Text))
                        {
                            string xSeqList = string.Empty;
                            string xKeyCheck = txtSeq.Text.Substring(0, 1);
                            ArrayList alSeqChk = new ArrayList();
                            for (int seq = 0; seq < txtSeq.Text.Length; seq++)
                            {
                                string xSeq = txtSeq.Text.Substring(seq, 1);

                                if (alSeqChk.Contains(xSeq))
                                {
                                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A014", new string[] { Convert.ToString(i + 1) + "번 설문 순서" }, new string[] { "Q Sequence" + Convert.ToString(i + 1) }, Thread.CurrentThread.CurrentCulture));
                                    xChk = false;
                                    return;
                                }
                                else
                                {
                                    alSeqChk.Add(xSeq);
                                }
                                //if (seq > 0)
                                //{
                                //    if (xSeq == xKeyCheck)
                                //    {
                                //        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A014", new string[] { Convert.ToString(i + 1) + "번 설문 순서" }, new string[] { "Q Sequence" + Convert.ToString(i + 1) }, Thread.CurrentThread.CurrentCulture));
                                //        xChk = false;
                                //    }
                                //}

                                if (seq == txtSeq.Text.Length)
                                    xSeqList += xSeq;
                                else
                                    xSeqList += xSeq + ",";
                            }

                            if (!string.IsNullOrEmpty(xSeqList))
                            {
                                xDr = xDt.NewRow();
                                xDr["colUser_ID"] = Session["USER_ID"].ToString();  // 사용자 ID
                                xDr["colRes_No"] = Request.QueryString["rResNo"];  // 설문번호
                                xDr["colRes_Que_ID"] = Convert.ToInt32(i + 1).ToString("000");  // 설문문항 ID
                                xDr["colSeq"] = 1;  // 보기 순번(답변)
                                xDr["colDuty_Step"] = Session["DUTY_STEP"].ToString();  // 사용자 직급
                                xDr["colExplain"] = xSeqList.Replace("'", "''");  // 서술형 답안
                                xDt.Rows.Add(xDr);
                            }
                            else
                                xChk = false;
                        }
                    }

                    if (xChk == false) // 필수입력값 누락
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A016", new string[] { Convert.ToString(i + 1) + "번 설문" }, new string[] { "Q" + Convert.ToString(i + 1) }, Thread.CurrentThread.CurrentCulture));
                        xChk = false;
                        break;
                    }
                }

                if (xChk == false)
                    return;

                string xRtn = Boolean.FalseString;

                object[] xParams = new object[1];
                xParams[0] = (object)xDt;
                
                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                        "Set_Survey_DB",
                        LMS_SYSTEM.COMMUNITY,
                        "CLT.WEB.UI.LMS.COMMUNITY",
                        (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));

                    //xScriptMsg = "<script>window.location.href='/clt.web.ui.lms.manage/vp_m_manage_survey_list_wpg.aspx';</script>";
                    //ScriptHelper.ScriptBlock(this, "vp_m_manage_survey_insdetail_wpg", xScriptMsg);
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Porpagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        #region public string Validateion(int rCount)
        public string Validateion(int rCount)
        {
            string xMessage = string.Empty;
            try
            {
                #region 설문타입 정보를 가져온다.
                string[] xParams = new string[1];
                xParams[0] = Request.QueryString["rResNo"].ToString();

                DataTable xDtDetail = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                                       "GetResearchDetailList",
                                                        LMS_SYSTEM.COMMUNITY,
                                                       "CLT.WEB.UI.LMS.COMMUNITY", (object)xParams);

                string xResType = string.Empty;
                #endregion

                for (int i = 0; i < rCount; i++)
                {
                    xResType = xDtDetail.Rows[i]["RES_TYPE"].ToString();
                    bool xChk = false;
                    int xChkCount = 0;
                    for (int k = 0; k < 7; k++)  // 보기는 최대 7개 까지 이므로 7번 돈다...
                    {

                        RadioButton rbtn = (RadioButton)this.ph01.FindControl("rbtn" + Convert.ToString(i + 1) + Convert.ToString(k + 1));  // 컨트롤 검색
                        CheckBox chkBox = (CheckBox)this.ph01.FindControl("chkBox" + Convert.ToString(i + 1) + Convert.ToString(k + 1));
                        TextBox txtBox = (TextBox)this.ph01.FindControl("txtExam" + Convert.ToString(i + 1) + Convert.ToString(k + 1));
                        Label lblseq = (Label)this.ph01.FindControl("lblSeq" + Convert.ToString(i + 1));

                        if (rbtn != null)
                        {
                            if (rbtn.Checked == true)
                                xChk = true;
                        }
                        else if (chkBox != null)
                        {
                            if (chkBox.Checked == true)
                                xChk = true;

                            //if (lblseq != null)
                            //{
                            //    if (chkBox.Checked == false)
                            //        xChk = false;
                            //}
                            xChkCount++;
                        }
                        else if (txtBox != null)
                        {
                            if (!string.IsNullOrEmpty(txtBox.Text))
                            {
                                // 단일 선택 혼합형은 RadioBox 가 Check 되어 있을경우 모두 False 시킨다.
                                if (xResType == "000004") // 단일선택(혼합형) 이면
                                {
                                    int nCount = Convert.ToInt32(xDtDetail.Rows[i]["EXAMPLE_CNT"].ToString());
                                    for (k = 0; k < nCount; k++)
                                    {
                                        RadioButton rbtnChk = (RadioButton)this.ph01.FindControl("rbtn" + Convert.ToString(i + 1) + Convert.ToString(k + 1));  // 컨트롤 검색
                                        if (rbtnChk != null)
                                        {

                                            if (rbtnChk.Checked == true)
                                                rbtnChk.Checked = false;
                                        }
                                    }
                                }

                                xChk = true;
                            }
                        }
                    }
                    
                    // 순서배열 체크
                    TextBox txtSeq = (TextBox)this.ph01.FindControl("txtSeq" + Convert.ToString(i + 1));
                    if (txtSeq != null)
                    {
                        if (txtSeq.Text.Length != txtSeq.MaxLength)
                        {
                            //if (!string.IsNullOrEmpty(txtSeq.Text))
                            xChk = false;
                        }
                        else
                            xChk = true;

                        if (!string.IsNullOrEmpty(txtSeq.Text))
                        {
                            string xSeqList = string.Empty;
                            string xKeyCheck = txtSeq.Text.Substring(0, 1);
                            ArrayList alSeqChk = new ArrayList();
                            for (int seq = 0; seq < txtSeq.Text.Length; seq++)
                            {
                                string xSeq = txtSeq.Text.Substring(seq, 1);

                                if (alSeqChk.Contains(xSeq))
                                {
                                    //ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A014", new string[] { Convert.ToString(i + 1) + "번 설문 순서" }, new string[] { "Q Sequence" + Convert.ToString(i + 1) }, Thread.CurrentThread.CurrentCulture));
                                    xChk = false;
                                    //return;
                                }
                                else
                                {
                                    alSeqChk.Add(xSeq);
                                    xChk = true;
                                }
                            }
                        }
                    }

                    if (xChk == false) // 필수입력값 누락
                    {
                        xMessage = Convert.ToString(i + 1);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xMessage;
        }
        #endregion

    }
}