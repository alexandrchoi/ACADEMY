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
    /// 1. 작업개요 : 설문 등록 Class
    /// 
    /// 2. 주요기능 : LMS 설문 등록

    ///				  
    /// 3. Class 명 : survey_ins
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.01
    ///
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class survey_insdetail : BasePage
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
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list_wpg", xScriptMsg);

                    return;
                }

                if (!IsPostBack)
                {
                    //this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                    base.pRender(this.Page,
                                 new object[,] { { this.btnSave, "E" } },
                                 Convert.ToString(Request.QueryString["MenuCode"]));

                    ViewState["UserIDInfo"] = Session["USER_ID"].ToString();
                    if (Request.QueryString["rResSub"] != null && Request.QueryString["rResSub"] != "")
                        this.txtRes_SUB.Text = HttpUtility.UrlDecode(Request.QueryString["rResSub"].ToString());

                    if (Session["RESSUB"] != null && Session["RESSUB"].ToString() != "")
                        this.txtRes_SUB.Text = Session["RESSUB"].ToString();

                    if (Request.QueryString["rResFrom"] != null && Request.QueryString["rResFrom"] != "")
                        this.txtRes_From.Text = Request.QueryString["rResFrom"].ToString();

                    if (Request.QueryString["rResTo"] != null && Request.QueryString["rResTo"] != "")
                        this.txtRes_To.Text = Request.QueryString["rResTo"].ToString();

                    if (Request.QueryString["rResDate"] != null && Request.QueryString["rResDate"] != "")
                    {
                        string[] xResDate = HttpUtility.UrlDecode(Request.QueryString["rResDate"].ToString()).Split('~');
                        int xCnt = 0;
                        foreach (string xDate in xResDate)
                        {
                            if (xCnt == 0)
                                this.txtRes_From.Text = xDate.Trim();
                            else if (xCnt == 1)
                                this.txtRes_To.Text = xDate.Trim();

                            xCnt++;
                        }
                    }

                    ViewState["rMode"] = Request.QueryString["rMode"].ToString();

                    if (Request.QueryString["rResQueCnt"] != null)
                        ViewState["rResQueCnt"] = Request.QueryString["rResQueCnt"];
                    if (Request.QueryString["rResNo"] != null)
                        ViewState["rResNo"] = Request.QueryString["rResNo"].ToString();
                }
                
                if (Request.QueryString["rMode"] != null)
                {
                    if (ViewState["rMode"].ToString() == "new")
                    {
                        if (ViewState["rResQueCnt"].ToString() != "")
                        {
                            AddContent(ViewState["rResQueCnt"].ToString());  // 사용자가 선택한 설문문항 갯수    
                        }
                    }
                    else if (ViewState["rMode"].ToString() == "edit")
                    {
                        if (ViewState["rResQueCnt"] != null && ViewState["rResQueCnt"].ToString() != "" && ViewState["rResNo"] != null && ViewState["rResNo"].ToString() != "")
                        {
                            AddContentEdit(Request.QueryString["rResQueCnt"].ToString(), Request.QueryString["rResNo"].ToString());
                        }
                        else
                        {
                            AddContent(ViewState["rResQueCnt"].ToString());  // 사용자가 선택한 설문문항 갯수    
                        }
                    }
                }
                else
                {
                    return;
                    //string xScriptContent = "<script>alert('잘못된 경로를 통해 접근하였습니다.');self.close();</script>";
                    //ScriptHelper.ScriptBlock(this, "vp_m_manage_survey_insdetail_wpg", xScriptContent);
                }

                //if (Request.QueryString["rMode"] != null)
                //{
                //    if (Request.QueryString["rMode"].ToString() == "new")
                //    {
                //        if (Request.QueryString["rResQueCnt"] != null && Request.QueryString["rResQueCnt"].ToString() != "")
                //        {
                //            AddContent(Request.QueryString["rResQueCnt"].ToString());  // 사용자가 선택한 설문문항 갯수    
                //        }
                //    }
                //    else if (Request.QueryString["rMode"].ToString() == "edit")
                //    {
                //        if (Request.QueryString["rResQueCnt"] != null && Request.QueryString["rResQueCnt"].ToString() != "" && Request.QueryString["rResNo"] != null && Request.QueryString["rResNo"].ToString() != "")
                //        {
                //            AddContentEdit(Request.QueryString["rResQueCnt"].ToString(), Request.QueryString["rResNo"].ToString());
                //        }
                //    }
                //}
                //else
                //{
                //    return;
                //    //string xScriptContent = "<script>alert('잘못된 경로를 통해 접근하였습니다.');self.close();</script>";
                //    //ScriptHelper.ScriptBlock(this, "vp_m_manage_survey_insdetail_wpg", xScriptContent);
                //}
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        #endregion

        /************************************************************
        * Function name : btnSave_OnClick
        * Purpose       : 입력된 설문 내용 저장버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnSave_OnClick(object sender, EventArgs e)
        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["rNoticeyn"].ToString() == "Y")
                {
                    // 게시된 설문은 수정 할 수 없습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A112", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                if (ViewState["rMode"].ToString() == "new")
                    ResearchInsert();
                else if (ViewState["rMode"].ToString() == "edit")
                    ResearchUpdate();
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
                Response.Redirect("/curr/survey_list.aspx");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        #endregion

        /************************************************************
        * Function name : RadioButton_CheckedChanged
        * Purpose       : 각 설문별 라디오 버튼 체크 관련 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region RadioButton_CheckedChanged(object sender, EventArgs e)
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // sender RadioButton을 가져온다.
                RadioButton rbBox = (RadioButton)sender;

                string xRbID = string.Empty;  // Radio Button ID
                string xSeqID = string.Empty;  // Radio Button 의 설문문제 순서 ID (1번 설문 RadioButton, 2번 설문 RadioButton)
                                               // RadioButton ID 생성 규칙
                                               // rbSI : 단일선택
                                               // rbMI : 다중선택
                                               // rbDE : 서술형



                // rbSM : 단일선택(혼합형)
                // rbMM : 다중선택(혼합형)
                // rbSE : 순서배열
                xRbID = rbBox.ID.Substring(0, 4);  // ID는 4개...

                if (rbBox.ID.Length == 5)
                    xSeqID = rbBox.ID.Substring(4, 1);  // 설문문제 Seq
                else if (rbBox.ID.Length == 6)
                    xSeqID = rbBox.ID.Substring(4, 2);  // 설문문제 Seq  

                DropDownList ddList = (DropDownList)this.ph01.FindControl("System.Web.UI.WebControls.DropDownList" + xSeqID);


                if (xRbID == "rbSM" || xRbID == "rbMM")
                {
                    if (ddList.Items.Count == 7)
                        ddList.Items.RemoveAt(6);
                }
                else
                {
                    if (ddList.Items.Count == 6)
                    {
                        ListItem Items = new ListItem();
                        Items.Text = "7 번";
                        Items.Value = "7";
                        ddList.Items.Add(Items);
                    }

                }


                if (xRbID == "rbDE") // 서술형 선택일 경우
                {
                    ddList.SelectedIndex = ddList.Items.IndexOf(ddList.Items.FindByValue("1"));
                    ddList.Enabled = false;
                    ddlContent_SelectedIndexChanged((object)ddList, null);  // Dropdownlist 에 선택값 변경후 Event 호출
                                                                            // 해당 보기 입력 Box ReadOnly 로 변경

                    TextBox txtExam00 = (TextBox)this.ph01.FindControl("txtExam" + xSeqID + "0");
                    if (txtExam00 != null)
                    {
                        txtExam00.Text = string.Empty;
                        txtExam00.ReadOnly = true;
                        //txtExam00.BackColor = Color.FromName("#dcdcdc");
                    }

                    for (int k = 1; k < 7; k++)
                    {
                        TextBox txtExam = (TextBox)this.ph01.FindControl("txtExam" + xSeqID + k.ToString());
                        if (txtExam != null)
                        {
                            txtExam00.Text = string.Empty;
                        }
                    }
                }
                else
                {
                    ddList.Enabled = true;
                    TextBox txtExam00 = (TextBox)this.ph01.FindControl("txtExam" + xSeqID + "0");
                    if (txtExam00 != null && txtExam00.ReadOnly == true)
                    {
                        txtExam00.ReadOnly = false;

                        //txtExam00.BackColor = Color.White;
                        //txtExam00.BorderStyle = BorderStyle.NotSet;
                    }
                }


                //ph01.FindControl("System.Web.UI.WebControls.DropDownList" + xSeqID).Focus();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        #endregion

        /************************************************************
        * Function name : ddlContent_SelectedIndexChanged
        * Purpose       : 설문 문항 선택 이벤트
        * Input         : void
        * Output        : void
        * 컨트롤을 이용하여 검색하는 값 : 첫번째 Cell ID 
        *                                 사용자가 선택한 DropDownList
        *                                 사용자가 선택한 DropDownList 가 속한 Table ID
        *                                 사용자가 선택한 DropDownList 가 속한 보기 TableRow ID
        *************************************************************/
        #region ddlContent_SelectedIndexChanged(object sender, EventArgs e)
        private void ddlContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // 이벤트를 호출한 DropDownList를 가져온다.
                DropDownList ddlContent = (DropDownList)sender;

                int xCount = 0;

                // DropDownList 의 맨 끝자리 번호가 설문 문제 번호 ID를 의미
                // dropdownlist1 은 1번째 설문의 DropdownsList
                if (ddlContent.ID.Length == 39)
                    xCount = Convert.ToInt32(ddlContent.ID.Substring(38, 1));

                // DropDownList 의 맨 끝자리 번호가 설문 문제 번호 ID를 의미
                // dropdownlist10 은 10번째 설문의 DropdownsList
                else if (ddlContent.ID.Length == 40)
                    xCount = Convert.ToInt32(ddlContent.ID.Substring(38, 2));

                ViewState["ddlContent"] = ddlContent.ID;

                // dropdownlist 의 맨 끝자리 숫자를 xCount 에.. (xCount 의 값은 설문문제 순서를 의미)
                // TableCell 의 TC_0x 번째 에 Rowspan 변경을 위한 TableCell 검색

                TableHeaderCell TC01 = (TableHeaderCell)this.ph01.FindControl("TC_0" + xCount.ToString());

                if (TC01 == null)
                    return;

                // 사용자가 선택한 DropDownList의 Value 값을 가져온다.
                int xRowCount = Convert.ToInt32(ddlContent.SelectedValue);

                // 보기 항목을 제외한 Rowspan 지정
                
                // 1번째 Row : 질문, 2번째 Row : 보기형태, 3번째 Row : 보기항목수
                
                int xRowspan = 3;

                // 보기 항목은 1~7 개 까지 이므로 
                // 미리 만들어둔 7개의 TableRow를 반복적으로 돌면서 
                // 화면에 보여 줄것인지 숨길것인지 체크하도록 한다.
                for (int i = 0; i < 7; i++)
                {
                    // TableRow ID 검색 방법
                    // TR_EX00 : 1번쨰 설문 1번째 보기
                    // TR_EX01 : 1번쨰 설문 2번째 보기
                    // TR_EX10 : 2번쨰 설문 1번째 보기
                    // TR_EX11 : 2번쨰 설문 2번째 보기
                    TableRow TR_EX = (TableRow)this.ph01.FindControl("TR_EX" + xCount.ToString() + i.ToString());

                    if (i < xRowCount)
                    {
                        TR_EX.Style.Add(HtmlTextWriterStyle.Display, "");  // 해당 Row는 보여주기로 설정
                    }
                    else
                    {
                        TextBox txtExam00 = (TextBox)this.ph01.FindControl("txtExam" + xCount.ToString() + i.ToString());

                        if (txtExam00 != null)
                            txtExam00.Text = string.Empty;

                        TR_EX.Style.Add(HtmlTextWriterStyle.Display, "none");  // 해당 Row는 숨김으로 설정
                    }
                }

                TC01.RowSpan = xRowspan + xRowCount;  // 기본 Row + 사용자가 선택한 보기 숫자
                ViewState["ddlContentClientID"] = ddlContent.ClientID;

                //Response.Write(string.Format("<script>{0}.focus();</script>", ddlContent.ClientID));
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        #endregion

        /************************************************************
        * Function name : AddContent 
        * Purpose       : 웹페이지 설문조사 서식 동적컨트롤 생성 메서드
        * Input         : string rResQueCnt
        * Output        : void
        *************************************************************/
        #region AddContent(string rResQueCnt)
        public void AddContent(string rResQueCnt)
        {
            try
            {
                int xCount = Convert.ToInt32(rResQueCnt);
                int xResCnt = 1;

                for (int i = 0; i < xCount; i++)
                {
                    // 설문조사 테이블 생성...
                    // 테이블은 설문 문항 숫자만큼 생성한다..
                    Table xTB_RES = new Table();
                    xTB_RES.ID = "TB_RES" + i.ToString();  // 테이블 ID : TB_RES + 설문숫자 
                    //xTB_RES.Style.Value = tanbleStyle;
                    //xTB_RES.BorderWidth = 0;
                    //xTB_RES.CellPadding = 0;
                    //xTB_RES.CellSpacing = 1;
                    //xTB_RES.Style.Add(HtmlTextWriterStyle.Width, "98%");

                    /*
                     *  TableRow 생성규칙
                     *  첫번째 Row 00
                     *  두번째 Row 01
                     */
                    TableRow xTR_01 = new TableRow();  // 설문 할  질문
                    TableRow xTR_02 = new TableRow();  // 보기형태
                    TableRow xTR_03 = new TableRow();  // 보기 항목수

                    //TableRow xTR_04 = new TableRow();  // 보기 1

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
                        xTC_01.Text = xResCnt.ToString() + " 번 설문";
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
                    //xTC_03.Style.Add(HtmlTextWriterStyle.Width, "75%");
                    //xTC_03.ColumnSpan = 3;
                    TextBox txtRes_Content = new TextBox();
                    //txtRes_Content.Style.Add(HtmlTextWriterStyle.Width, "98%");
                    txtRes_Content.ID = "txtRes_Content" + i.ToString();
                    xTC_03.Controls.Add(txtRes_Content);


                    // 두번째 Row
                    if (IsSettingKorean())
                        xTC_11.Text = "보기형태";
                    else
                        xTC_11.Text = "Answer Type";

                    //xTC_11.Style.Value = pop_left;

                    //xTC_12.Style.Value = pop_right;

                    // RadioButton ID 생성 규칙
                    // rbSI : 단일선택
                    // rbMI : 다중선택
                    // rbDE : 서술형
                    // rbSM : 단일선택(혼합형)
                    // rbMM : 다중선택(혼합형)
                    // rbSE : 순서배열
                    RadioButton rbSingle = new RadioButton();
                    rbSingle.GroupName = "Res_Type" + i.ToString();

                    if (IsSettingKorean())
                        rbSingle.Text = "단일선택";
                    else
                        rbSingle.Text = "Single Selection";

                    rbSingle.ID = "rbSI" + i.ToString();
                    rbSingle.AutoPostBack = true;
                    rbSingle.Checked = true;
                    rbSingle.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

                    RadioButton rbMulti = new RadioButton();
                    rbMulti.GroupName = "Res_Type" + i.ToString();

                    if (IsSettingKorean())
                        rbMulti.Text = "다중선택";
                    else
                        rbMulti.Text = "Multi Selection";

                    rbMulti.ID = "rbMI" + i.ToString();
                    rbMulti.AutoPostBack = true;
                    rbMulti.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

                    RadioButton rbDescriptive = new RadioButton();
                    rbDescriptive.GroupName = "Res_Type" + i.ToString();

                    if (IsSettingKorean())
                        rbDescriptive.Text = "서술형";
                    else
                        rbDescriptive.Text = "Description";

                    rbDescriptive.ID = "rbDE" + i.ToString();
                    rbDescriptive.AutoPostBack = true;
                    rbDescriptive.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

                    RadioButton rbSingleMix = new RadioButton();
                    rbSingleMix.GroupName = "Res_Type" + i.ToString();

                    if (IsSettingKorean())
                        rbSingleMix.Text = "단일선택(혼합형)";
                    else
                        rbSingleMix.Text = "Single Selection(Mix)";

                    rbSingleMix.ID = "rbSM" + i.ToString();
                    rbSingleMix.AutoPostBack = true;
                    rbSingleMix.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

                    RadioButton rbMultiMix = new RadioButton();
                    rbMultiMix.GroupName = "Res_Type" + i.ToString();

                    if (IsSettingKorean())
                        rbMultiMix.Text = "다중선택(혼합형)";
                    else
                        rbMultiMix.Text = "Multi Selection(Mix)";

                    rbMultiMix.ID = "rbMM" + i.ToString();
                    rbMultiMix.AutoPostBack = true;
                    rbMultiMix.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

                    RadioButton rbSeq = new RadioButton();
                    rbSeq.GroupName = "Res_Type" + i.ToString();

                    if (IsSettingKorean())
                        rbSeq.Text = "순서배열";
                    else
                        rbSeq.Text = "Sequencing";

                    rbSeq.ID = "rbSE" + i.ToString();
                    rbSeq.AutoPostBack = true;
                    rbSeq.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

                    xTC_12.Controls.Add(rbSingle);
                    xTC_12.Controls.Add(rbMulti);
                    xTC_12.Controls.Add(rbDescriptive);
                    xTC_12.Controls.Add(rbSingleMix);
                    xTC_12.Controls.Add(rbMultiMix);
                    xTC_12.Controls.Add(rbSeq);


                    // 세번째 Row
                    //xTC_21.Style.Value = pop_left;

                    if (IsSettingKorean())
                        xTC_21.Text = "보기 항목수";
                    else
                        xTC_21.Text = "Number of answer";

                    //xTC_22.ColumnSpan = 2;
                    //xTC_22.Style.Add(HtmlTextWriterStyle.Width, "98%");
                    //xTC_22.Style.Value = pop_right;


                    // 설문문항 보기 DropDownList
                    DropDownList ddlContent = new DropDownList();
                    ddlContent.AutoPostBack = true;
                    ddlContent.ID = "ddlContent" + i.ToString();
                    ddlContent.SelectedIndexChanged += new System.EventHandler(this.ddlContent_SelectedIndexChanged);  // 선택값 변경 Event
                    ddlContent.ID = ddlContent + i.ToString();  // ID : System.Web.UI.WebControls.DropDownList + 설문문제 숫자

                    // 보기문항은 1 ~ 7까지 이므로 1~7 DropDownList 추가...
                    for (int j = 0; j < 7; j++)
                    {
                        ListItem Items = new ListItem();

                        if (IsSettingKorean())
                            Items.Text = Convert.ToString(j + 1) + " 번";
                        else
                            Items.Text = "A" + Convert.ToString(j + 1);

                        Items.Value = Convert.ToString(j + 1);
                        ddlContent.Items.Add(Items);
                    }

                    xTC_22.Controls.Add(ddlContent);


                    // Row에 Cell 추가
                    xTR_01.Controls.Add(xTC_01);
                    xTR_01.Controls.Add(xTC_02);
                    xTR_01.Controls.Add(xTC_03);


                    xTR_02.Controls.Add(xTC_11);
                    xTR_02.Controls.Add(xTC_12);


                    xTR_03.Controls.Add(xTC_21);
                    xTR_03.Controls.Add(xTC_22);


                    // Table 에 Row 추가
                    xTB_RES.Controls.Add(xTR_01);
                    xTB_RES.Controls.Add(xTR_02);
                    xTB_RES.Controls.Add(xTR_03);

                    int xRowCount = 7;

                    int xNonDisplayCnt = 0;

                    xTC_01.RowSpan = xTC_01.RowSpan + xRowCount;  // 기본 설문 + 보기문항

                    // 사용자가 선택한 숫자만큼 보기문항 추가
                    for (int k = 0; k < xRowCount; k++)
                    {
                        TableRow TR_EX = new TableRow();

                        TableHeaderCell TC_01 = new TableHeaderCell();
                        TableCell TC_02 = new TableCell();

                        TR_EX.ID = "TR_EX" + i.ToString() + k.ToString();

                        if (k != 0) // 처음에는 1번쨰 문항을 제외한 나머지 리스트를 숨김처리
                        {
                            TR_EX.Style.Add(HtmlTextWriterStyle.Display, "none");
                            xNonDisplayCnt++;
                        }

                        //TC_01.Style.Value = pop_left;

                        if (IsSettingKorean())
                            TC_01.Text = "보기 " + Convert.ToString(k + 1);
                        else
                            TC_01.Text = "A" + Convert.ToString(k + 1);

                        //TC_02.Style.Value = pop_right;
                        //TC_02.ColumnSpan = 3;

                        TextBox txtExam00 = new TextBox();
                        txtExam00.ID = "txtExam" + i.ToString() + k.ToString();
                        //txtExam00.Style.Add(HtmlTextWriterStyle.Width, "98%");

                        //this.txtAcquisition.Attributes.Add("onkeyup", "ChkDate(this);"); //고용보험취득일 체크
                        //document.getElementById('<%=txtID.ClientID %>')
                        //txtExam00.Attributes.Add("onkeyup", "isMaxLenth(document.getElementById('" + txtExam00.ClientID  +"'),150,<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg(\"A007\", new string[] { \"보기항목\",\"10\" }, new string[] { \"Example\",\"10\" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>);");
                        //txtExam00.Attributes.Add("onkeyup", "javascript:isMaxLenth(document.getElementById('" + txtExam00.ClientID + "'),150,<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg(\"A007\", new string[] { \"보기항목\",\"10\" }, new string[] { \"Example\",\"10\" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>);");

                        TC_02.Controls.Add(txtExam00);

                        TR_EX.Controls.Add(TC_01);
                        TR_EX.Controls.Add(TC_02);

                        xTB_RES.Controls.Add(TR_EX);
                    }

                    xResCnt++;

                    xTC_01.RowSpan = xTB_RES.Rows.Count - xNonDisplayCnt;  // 숨김처리된 Row 의 갯수만큼 Rowspan 값에서 뺀다.

                    ph01.Controls.Add(xTB_RES);
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
        * Function name : AddContentEdit 
        * Purpose       : 웹페이지 설문조사 서식 동적컨트롤 생성 메서드
        * Input         : string rResQueCnt : 설문 문제갯수, rResNo : 설문ID
        * Output        : void
        *************************************************************/
        #region AddContentEdit(string rResQueCnt, string rResNo)
        public void AddContentEdit(string rResQueCnt, string rResNo)
        {
            try
            {
                int xCount = Convert.ToInt32(rResQueCnt);
                int xResCnt = 1;

                string[] xParams = new string[2];
                xParams[0] = rResNo; // 설문조사 ID

                // res_que_id, res_type, res_content 
                DataTable xDtDetail = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                                       "GetResearchDetailList",
                                                        LMS_SYSTEM.MANAGE,
                                                       "CLT.WEB.UI.LMS.MANAGE", (object)xParams);
                
                for (int i = 0; i < xCount; i++)
                {

                    // 설문조사 테이블 생성...
                    // 테이블은 설문 문항 숫자만큼 생성한다..
                    Table xTB_RES = new Table();
                    xTB_RES.ID = "TB_RES" + i.ToString();  // 테이블 ID : TB_RES + 설문숫자 
                    //xTB_RES.Style.Value = tanbleStyle;
                    //xTB_RES.BorderWidth = 0;
                    //xTB_RES.CellPadding = 0;
                    //xTB_RES.CellSpacing = 1;
                    //xTB_RES.Style.Add(HtmlTextWriterStyle.Width, "98%");
                    //cl.Style.Add("width", "15%");
                    //cg.Controls.Add(cl);
                    //cl = new WebControl(HtmlTextWriterTag.Col);
                    //cl.Style.Add("width", "15%");
                    //cg.Controls.Add(cl);
                    //cl = new WebControl(HtmlTextWriterTag.Col);
                    //cg.Controls.Add(cl);

                    /*
                     *  TableRow 생성규칙
                     *  첫번째 Row 00
                     *  두번째 Row 01
                     */
                    TableRow xTR_01 = new TableRow();  // 설문 할  질문
                    TableRow xTR_02 = new TableRow();  // 보기형태
                    TableRow xTR_03 = new TableRow();  // 보기 항목수

                    //TableRow xTR_04 = new TableRow();  // 보기 1

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
                    xTC_12.CssClass = "form-margin-right";

                    TableHeaderCell xTC_21 = new TableHeaderCell(); // 보기 항목수 제목
                    TableCell xTC_22 = new TableCell(); // 보기 항목수 컨트롤

                    TableHeaderCell xTC_31 = new TableHeaderCell(); // 보기1 제목
                    TableCell xTC_32 = new TableCell(); // 보기1 TextBox


                    // 첫번째 Row Rospan 을 설정하는 Cell
                    //xTC_01.Style.Value = pop_left;
                    xTC_01.Style.Add(HtmlTextWriterStyle.Width, "15%");

                    if (IsSettingKorean())
                        xTC_01.Text = xResCnt.ToString() + " 번 설문";
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
                    //xTC_03.Style.Add(HtmlTextWriterStyle.Width, "70%");
                    //xTC_03.ColumnSpan = 3;
                    TextBox txtRes_Content = new TextBox();
                    //txtRes_Content.Style.Add(HtmlTextWriterStyle.Width, "98%");
                    txtRes_Content.ID = "txtRes_Content" + i.ToString();
                    xTC_03.Controls.Add(txtRes_Content);

                    // Edit 모드 이므로 Data를 가져온다.
                    txtRes_Content.Text = xDtDetail.Rows[i]["res_content"].ToString();

                    // 두번째 Row
                    if (IsSettingKorean())
                        xTC_11.Text = "보기형태";
                    else
                        xTC_11.Text = "Answer Type";

                    //xTC_11.Style.Value = pop_left;

                    //xTC_12.Style.Value = pop_right;

                    // RadioButton ID 생성 규칙
                    // rbSI : 단일선택
                    // rbMI : 다중선택
                    // rbDE : 서술형
                    // rbSM : 단일선택(혼합형)
                    // rbMM : 다중선택(혼합형)
                    // rbSE : 순서배열
                    RadioButton rbSingle = new RadioButton();
                    rbSingle.GroupName = "Res_Type" + i.ToString();

                    if (IsSettingKorean())
                        rbSingle.Text = "단일선택";
                    else
                        rbSingle.Text = "Single Selection";

                    rbSingle.ID = "rbSI" + i.ToString();
                    rbSingle.AutoPostBack = true;
                    rbSingle.Checked = true;
                    rbSingle.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

                    RadioButton rbMulti = new RadioButton();
                    rbMulti.GroupName = "Res_Type" + i.ToString();

                    if (IsSettingKorean())
                        rbMulti.Text = "다중선택";
                    else
                        rbMulti.Text = "Multi Selection";

                    rbMulti.ID = "rbMI" + i.ToString();
                    rbMulti.AutoPostBack = true;
                    rbMulti.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

                    RadioButton rbDescriptive = new RadioButton();
                    rbDescriptive.GroupName = "Res_Type" + i.ToString();

                    if (IsSettingKorean())
                        rbDescriptive.Text = "서술형";
                    else
                        rbDescriptive.Text = "Description";

                    rbDescriptive.ID = "rbDE" + i.ToString();
                    rbDescriptive.AutoPostBack = true;
                    rbDescriptive.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

                    RadioButton rbSingleMix = new RadioButton();
                    rbSingleMix.GroupName = "Res_Type" + i.ToString();

                    if (IsSettingKorean())
                        rbSingleMix.Text = "단일선택(혼합형)";
                    else
                        rbSingleMix.Text = "Single Selection(Mix)";

                    rbSingleMix.ID = "rbSM" + i.ToString();
                    rbSingleMix.AutoPostBack = true;
                    rbSingleMix.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

                    RadioButton rbMultiMix = new RadioButton();
                    rbMultiMix.GroupName = "Res_Type" + i.ToString();

                    if (IsSettingKorean())
                        rbMultiMix.Text = "다중선택(혼합형)";
                    else
                        rbMultiMix.Text = "Multi Selection(Mix)";

                    rbMultiMix.ID = "rbMM" + i.ToString();
                    rbMultiMix.AutoPostBack = true;
                    rbMultiMix.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

                    RadioButton rbSeq = new RadioButton();
                    rbSeq.GroupName = "Res_Type" + i.ToString();

                    if (IsSettingKorean())
                        rbSeq.Text = "순서배열";
                    else
                        rbSeq.Text = "Sequencing";

                    rbSeq.ID = "rbSE" + i.ToString();
                    rbSeq.AutoPostBack = true;
                    rbSeq.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

                    xTC_12.Controls.Add(rbSingle);
                    xTC_12.Controls.Add(rbMulti);
                    xTC_12.Controls.Add(rbDescriptive);
                    xTC_12.Controls.Add(rbSingleMix);
                    xTC_12.Controls.Add(rbMultiMix);
                    xTC_12.Controls.Add(rbSeq);


                    // Edit 모드 이므로 설문유형 Data를 가져온다.
                    if (xDtDetail.Rows[i]["res_type"].ToString() == "000001") // 단일선택일 경우
                        rbSingle.Checked = true;
                    else if (xDtDetail.Rows[i]["res_type"].ToString() == "000002") // 다중선택일 경우
                        rbMulti.Checked = true;
                    else if (xDtDetail.Rows[i]["res_type"].ToString() == "000003") // 서술형일 경우
                        rbDescriptive.Checked = true;
                    else if (xDtDetail.Rows[i]["res_type"].ToString() == "000004") // 단일선택(혼합형)일 경우
                        rbSingleMix.Checked = true;
                    else if (xDtDetail.Rows[i]["res_type"].ToString() == "000005") // 다중선택(혼합형)일 경우
                        rbMultiMix.Checked = true;
                    else if (xDtDetail.Rows[i]["res_type"].ToString() == "000006") // 순서배열일 경우
                        rbSeq.Checked = true;


                    // 세번째 Row
                    //xTC_21.Style.Value = pop_left;

                    if (IsSettingKorean())
                        xTC_21.Text = "보기 항목수";
                    else
                        xTC_21.Text = "Number of answer";

                    //xTC_22.ColumnSpan = 2;
                    //xTC_22.Style.Add(HtmlTextWriterStyle.Width, "98%");
                    //xTC_22.Style.Value = pop_right;


                    // 설문문항 보기 DropDownList
                    DropDownList ddlContent = new DropDownList();
                    ddlContent.AutoPostBack = true;
                    ddlContent.ID = "ddlContent" + i.ToString();
                    ddlContent.SelectedIndexChanged += new System.EventHandler(this.ddlContent_SelectedIndexChanged);  // 선택값 변경 Event
                    ddlContent.ID = ddlContent + i.ToString();  // ID : System.Web.UI.WebControls.DropDownList + 설문문제 숫자

                    // 보기문항은 1 ~ 7까지 이므로 1~7 DropDownList 추가...
                    for (int j = 0; j < 7; j++)
                    {
                        ListItem Items = new ListItem();

                        if (IsSettingKorean())
                            Items.Text = Convert.ToString(j + 1) + " 번";
                        else
                            Items.Text = "A" + Convert.ToString(j + 1);

                        Items.Value = Convert.ToString(j + 1);
                        ddlContent.Items.Add(Items);
                    }

                    xTC_22.Controls.Add(ddlContent);

                    ddlContent.Items.FindByValue(xDtDetail.Rows[i]["example_cnt"].ToString()).Selected = true;  // 사용자가 입력한 보기 갯수만큼 선택


                    // Row에 Cell 추가
                    xTR_01.Controls.Add(xTC_01);
                    xTR_01.Controls.Add(xTC_02);
                    xTR_01.Controls.Add(xTC_03);


                    xTR_02.Controls.Add(xTC_11);
                    xTR_02.Controls.Add(xTC_12);


                    xTR_03.Controls.Add(xTC_21);
                    xTR_03.Controls.Add(xTC_22);


                    // Table 에 Row 추가
                    xTB_RES.Controls.Add(xTR_01);
                    xTB_RES.Controls.Add(xTR_02);
                    xTB_RES.Controls.Add(xTR_03);

                    int xRowCount = 7;

                    int xNonDisplayCnt = 0;

                    xTC_01.RowSpan = xTC_01.RowSpan + xRowCount;  // 기본 설문 + 보기문항

                    // 사용자가 선택한 숫자만큼 보기문항 추가


                    // 사용자가 입력한 보기문항 갯수 가져오기



                    int xExample_Cnt = Convert.ToInt32(xDtDetail.Rows[i]["example_cnt"].ToString());

                    xParams[1] = xDtDetail.Rows[i]["res_que_id"].ToString();
                    // res_que_id, seq, Example
                    DataTable xDtChoice = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                                           "GetResearchChoiceList",
                                                            LMS_SYSTEM.MANAGE,
                                                           "CLT.WEB.UI.LMS.MANAGE", (object)xParams);

                    for (int k = 0; k < xRowCount; k++)
                    {

                        TableRow TR_EX = new TableRow();

                        TableHeaderCell TC_01 = new TableHeaderCell();
                        TableCell TC_02 = new TableCell();

                        TR_EX.ID = "TR_EX" + i.ToString() + k.ToString();

                        if (k >= xExample_Cnt) // 사용자가 처음에 입력한 보기문항을 제외한 나머지 보기항목을 숨김처리
                        {
                            TR_EX.Style.Add(HtmlTextWriterStyle.Display, "none");
                            xNonDisplayCnt++;
                        }

                        //TC_01.Style.Value = pop_left;

                        if (IsSettingKorean())
                            TC_01.Text = "보기 " + Convert.ToString(k + 1);
                        else
                            TC_01.Text = "A" + Convert.ToString(k + 1);

                        //TC_01.Style.Add(HtmlTextWriterStyle.Width, "15%");

                        //TC_02.Style.Value = pop_right;
                        //TC_02.ColumnSpan = 3;
                        //TC_02.Style.Add(HtmlTextWriterStyle.Width, "70%");

                        TextBox txtExam00 = new TextBox();
                        txtExam00.ID = "txtExam" + i.ToString() + k.ToString();
                        //txtExam00.Style.Add(HtmlTextWriterStyle.Width, "98%");

                        if (k < xExample_Cnt)
                            txtExam00.Text = xDtChoice.Rows[k]["example"].ToString();

                        TC_02.Controls.Add(txtExam00);

                        TR_EX.Controls.Add(TC_01);
                        TR_EX.Controls.Add(TC_02);

                        xTB_RES.Controls.Add(TR_EX);
                    }

                    xResCnt++;

                    xTC_01.RowSpan = xTB_RES.Rows.Count - xNonDisplayCnt;  // 숨김처리된 Row 의 갯수만큼 Rowspan 값에서 뺀다.

                    ph01.Controls.Add(xTB_RES);
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
        * Function name : ResearchInsert
        * Purpose       : 설문내용 신규저장(Insert)
        * Input         : void
        * Output        : void
        *************************************************************/
        #region ResearchInsert()
        private void ResearchInsert()
        {
            try
            {
                int xResCnt = 0;  // 설문 갯수관련 변수   
                string xScriptContent = string.Empty;  // 필수 입력값 체크 관련 Message 변수
                
                if (Request.QueryString["rResQueCnt"] != null && Request.QueryString["rResQueCnt"].ToString() != "")
                    xResCnt = Convert.ToInt32(Request.QueryString["rResQueCnt"].ToString());  // 사용자가 선택한 설문문항 갯수 
                
                //  rSendyn 선박전송여부
                //  rCompany 법인사 ID (구분자 : |)
                //  rUsergroup 사용자 그룹 (구분자 : I)
                //  rSocialpos 직책 (구분자 : I)
                //  rDutystep  직급 (구분자 : I)
                //  rCourse  과정ID
                //  rCoursefrom 과정 교육기간 From(YYYYMMDD)
                //  rCourseTo  과정 교육기간 To(YYYYMMDD)
                //  rResQueCnt  설문 문항
                //  rResSub  설문 제목
                //  rResObj  설문 목적
                //  rResFrom  설문 조사기간 From
                //  rResTo  설문 조사기간 To
                //  rResKind 설문유형  000001 : 일반설문, 000002 과정설문

                string xCompany = Request.QueryString["rCompany"].ToString();  // 법인사 ID (구분자 : ┼)
                string xUserGroup = Request.QueryString["rUsergroup"].ToString();  // 사용자 그룹 (구분자 : ┼)
                string xSocialpos = Request.QueryString["rSocialpos"].ToString();  // 직책 (구분자 : ┼)
                string xDutystep = Request.QueryString["rDutystep"].ToString();  // 직급 (구분자 : ┼)


                string[] xMasterParams = new string[16];
                xMasterParams[0] = Request.QueryString["rSendyn"].ToString();  // 선박 전송여부
                xMasterParams[1] = Request.QueryString["rCourse"].ToString();  // 개설과정ID
                xMasterParams[2] = Request.QueryString["rCoursefrom"].ToString();  // 과정 교육기간 From
                xMasterParams[3] = Request.QueryString["rCourseTo"].ToString();  // 과정 교육기간 To
                xMasterParams[4] = Request.QueryString["rResQueCnt"].ToString(); // 설문 문항
                                                                                 //xMasterParams[5] = Request.QueryString["rResSub"].ToString();  // 설문 제목
                                                                                 //xMasterParams[6] = Request.QueryString["rResObj"].ToString();  // 설문 목적
                xMasterParams[5] = Session["RESSUB"].ToString();
                xMasterParams[6] = Session["RESOBJ"].ToString();
                xMasterParams[7] = Request.QueryString["rResFrom"].ToString();  //설문 조사기간 From
                xMasterParams[8] = Request.QueryString["rResTo"].ToString();  // 설문 조사기간 To
                xMasterParams[9] = Request.QueryString["rResKind"].ToString();  // 설문 유형
                xMasterParams[10] = Session["USER_ID"].ToString(); // 로그인한 ID
                xMasterParams[11] = Request.QueryString["rSendFLG"].ToString(); // 선박 전송여부
                xMasterParams[12] = xCompany;  // 회사 ID (구분자 : ┼)
                xMasterParams[13] = xUserGroup;  // 사용자 그룹 (구분자 : ┼)
                xMasterParams[14] = xSocialpos;  //  (구분자 : ┼)
                xMasterParams[15] = xDutystep;  // 직급 (구분자 : ┼)


                // ,0 : 질문
                // ,1 : 설문형태
                // ,2 : 보기 항목수

                // ,3 : 보기1
                // ,4 : 보기2
                // ,5 : 보기3
                // ,6 : 보기4
                // ,7 : 보기5
                // ,8 : 보기6
                // ,9 : 보기7
                string[,] xDetailParams = new string[xResCnt, 10]; // 설문문제 입력 갯수

                bool xMessageChk = false;
                // 사용자가 선택한 설문문항 갯수 만큼 Data 입력 체크
                for (int i = 0; i < xResCnt; i++)
                {
                    xDetailParams[i, 0] = string.Empty; // 배열 초기화



                    xDetailParams[i, 1] = string.Empty; // 배열 초기화



                    xDetailParams[i, 2] = string.Empty; // 배열 초기화



                    xDetailParams[i, 3] = string.Empty; // 배열 초기화



                    xDetailParams[i, 4] = string.Empty; // 배열 초기화



                    xDetailParams[i, 5] = string.Empty; // 배열 초기화



                    xDetailParams[i, 6] = string.Empty; // 배열 초기화



                    xDetailParams[i, 7] = string.Empty; // 배열 초기화



                    xDetailParams[i, 8] = string.Empty; // 배열 초기화



                    xDetailParams[i, 9] = string.Empty; // 배열 초기화





                    TextBox txtRes_Content = (TextBox)this.ph01.FindControl("txtRes_Content" + i.ToString());  // 질문내용 체크

                    int xLength = HlenUTF8(txtRes_Content.Text);
                    if (xLength > 150)
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A005", new string[] { "질문" + Convert.ToString(i + 1), "50", "150" }, new string[] { "Question" + Convert.ToString(i + 1), "50", "150" }, Thread.CurrentThread.CurrentCulture));
                        txtRes_Content.Focus();
                        xMessageChk = true;
                        break;
                    }

                    if (string.IsNullOrEmpty(txtRes_Content.Text))
                    {
                        //xScriptContent = string.Format("<script>alert('{0} 번 설문의 질문이 입력되지 않았습니다!');</script>", Convert.ToString(i + 1));
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A113", new string[] { Convert.ToString(i + 1) }, new string[] { Convert.ToString(i + 1) }, Thread.CurrentThread.CurrentCulture));
                        xMessageChk = true;
                        break;
                    }

                    xDetailParams[i, 0] = txtRes_Content.Text.Replace("'", "''"); // 질문


                    // RadioButton ID 생성 규칙
                    // rbSI : 단일선택
                    // rbMI : 다중선택
                    // rbDE : 서술형



                    // rbSM : 단일선택(혼합형)
                    // rbMM : 다중선택(혼합형)
                    // rbSE : 순서배열
                    RadioButton rbSI = (RadioButton)this.ph01.FindControl("rbSI" + i.ToString()); // 단일선택
                    RadioButton rbMI = (RadioButton)this.ph01.FindControl("rbMI" + i.ToString()); // 다중선택
                    RadioButton rbDE = (RadioButton)this.ph01.FindControl("rbDE" + i.ToString()); // 서술형



                    RadioButton rbSM = (RadioButton)this.ph01.FindControl("rbSM" + i.ToString()); // 단일선택(혼합형)
                    RadioButton rbMM = (RadioButton)this.ph01.FindControl("rbMM" + i.ToString()); // 다중선택(혼합형)
                    RadioButton rbSE = (RadioButton)this.ph01.FindControl("rbSE" + i.ToString()); // 순서배열

                    if (rbSI.Checked == true)
                        xDetailParams[i, 1] = "000001";  //Single Selection       단일선택	       	000001

                    else if (rbMI.Checked == true)
                        xDetailParams[i, 1] = "000002";  //Mulit Selection        다중선택   		000002

                    else if (rbDE.Checked == true)
                        xDetailParams[i, 1] = "000003";  //Descriptive            서술형     		000003

                    else if (rbSM.Checked == true)
                        xDetailParams[i, 1] = "000004";  //Single Selection(Mix)  단일선택(혼합형)	000004

                    else if (rbMM.Checked == true)
                        xDetailParams[i, 1] = "000005";  //Multi Selection(Mix)   다중선택(혼합형)	000005

                    else if (rbSE.Checked == true)
                        xDetailParams[i, 1] = "000006";  //Sequencing             순서배열		    000006


                    if (string.IsNullOrEmpty(xDetailParams[i, 1]))
                    {
                        //xScriptContent = string.Format("<script>alert('{0} 번 설문의 설문형태가 선택되지 않았습니다!');</script>", Convert.ToString(i + 1));
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A114", new string[] { Convert.ToString(i + 1) }, new string[] { Convert.ToString(i + 1) }, Thread.CurrentThread.CurrentCulture));
                        xMessageChk = true;
                        break;
                    }

                    DropDownList ddlContent = (DropDownList)this.ph01.FindControl("System.Web.UI.WebControls.DropDownList" + i.ToString());




                    bool xBreakChk = false;


                    if (xDetailParams[i, 1].ToString() == "000004" || xDetailParams[i, 1].ToString() == "000005")  // 혼합형이면...
                    {
                        if (ddlContent.SelectedItem.Value == "7")
                        {
                            // 혼합형은 6개 이상 입력 하실 수 없습니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A121", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                            xMessageChk = true;
                            break;
                        }
                    }

                    for (int j = 0; j < Convert.ToInt32(ddlContent.SelectedItem.Value); j++)  // 보기 항목수 만큼 보기 문항 입력 및 체크
                    {
                        TextBox txtExam = (TextBox)this.ph01.FindControl("txtExam" + i.ToString() + j.ToString());

                        int xQLength = HlenUTF8(txtExam.Text);
                        if (xQLength > 500)
                        {
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A005", new string[] { Convert.ToString(i + 1) + "번 설문의 " + Convert.ToString(j + 1) + "번 보기", "166", "500" }, new string[] { Convert.ToString(i + 1) + "No " + Convert.ToString(j + 1) + " Example", "166", "500" }, Thread.CurrentThread.CurrentCulture));
                            txtExam.Focus();
                            xMessageChk = true;
                            xBreakChk = true;
                            break;
                        }

                        if (xDetailParams[i, 1].ToString() != "000003")  // 서술형은 보기 항목을 입력하지 않음..
                        {
                            if (string.IsNullOrEmpty(txtExam.Text))
                            {
                                //xScriptContent = string.Format("<script>alert('{0} 번 설문의 {1} 번 보기가 입력되지 않았습니다!');</script>", Convert.ToString(i + 1), Convert.ToString(j + 1));
                                ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A115", new string[] { Convert.ToString(i + 1), Convert.ToString(j + 1) }, new string[] { Convert.ToString(i + 1), Convert.ToString(j + 1) }, Thread.CurrentThread.CurrentCulture));
                                xMessageChk = true;
                                xBreakChk = true;
                                break;
                            }
                            xDetailParams[i, j + 3] = txtExam.Text.Replace("'", "''"); // 보기항목 1~7
                        }
                        else
                            xDetailParams[i, j + 3] = string.Empty; //" "; // 보기항목 1~7
                    }

                    if (xDetailParams[i, 1].ToString() == "000004" || xDetailParams[i, 1].ToString() == "000005")  // 혼합형이면...
                    {

                        int xCount = Convert.ToInt32(ddlContent.SelectedItem.Value);
                        xDetailParams[i, 2] = Convert.ToString(xCount + 1);  // 보기 항목수



                        xDetailParams[i, xCount + 3] = string.Empty; //"";
                    }
                    else
                        xDetailParams[i, 2] = ddlContent.SelectedItem.Value;  // 보기 항목수



                    if (xBreakChk == true)
                        break;
                }

                if (xMessageChk == true)
                    return;

                // 
                //#region T_RESEARCH_TARGET 에 Data를 Insert 하지 않음
                //// 총대상자를 계산한다.
                //ArrayList xalCompany = new ArrayList();
                //ArrayList xalUsergroup = new ArrayList();
                //ArrayList xalSocialpos = new ArrayList();
                //ArrayList xalDutystep = new ArrayList();

                //if (!string.IsNullOrEmpty(xCompany))
                //    xalCompany = UnMix(xCompany); // 회사코드 리스트 분리

                //if (!string.IsNullOrEmpty(xUserGroup))
                //    xalUsergroup = UnMix(xUserGroup);  // 사용자코드 그룹 리스트 분리

                //if (!string.IsNullOrEmpty(xSocialpos))
                //    xalSocialpos = UnMix(xSocialpos); // 직책코드 리스트 분리

                //if (!string.IsNullOrEmpty(xDutystep))
                //    xalDutystep = UnMix(xDutystep); // 직급코드 리스트 분리


                //object[] xParams = new object[7];
                //xParams[0] = (object)xCompany;
                //xParams[1] = (object)xUserGroup;
                //xParams[2] = (object)xSocialpos;
                //xParams[3] = (object)xDutystep;
                //xParams[4] = (string)Request.QueryString["rCourse"].ToString();  // 과정ID
                //xParams[5] = (string)Request.QueryString["rCoursefrom"].ToString();  // 과정 교육기간 From
                //xParams[6] = (string)Request.QueryString["rCourseTo"].ToString();  // 과정 교육기간 To
                //DataTable xDt = null;

                //// 설문조사 대상 찾기...
                //if (Request.QueryString["rResKind"].ToString() == "000001") // 일반 설문이면
                //{
                //    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                //                           "GetSurveyUserList",
                //                           LMS_SYSTEM.MANAGE,
                //                           "CLT.WEB.UI.LMS.MANAGE.vp_m_manage_survey_insdetail_wpg",
                //                           (object)xParams);
                //}
                //else  // 과정 설문이면
                //{
                //    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                //           "GetCurrSurveyUserList",
                //           LMS_SYSTEM.MANAGE,
                //           "CLT.WEB.UI.LMS.MANAGE.vp_m_manage_survey_insdetail_wpg",
                //           (object)xParams);
                //}
                //#endregion

                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;
                object[] xRes_Params = new object[3];
                xRes_Params[0] = (string[])xMasterParams;   // T_RESEARCH 테이블 Parameter 
                xRes_Params[1] = (string[,])xDetailParams;  // T_RESEARCH_DETAIL 테이블 Parameter
                                                            //xRes_Params[2] = (DataTable)xDt;            // T_RESEARCH_TARGET 테이블 Parameter


                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                        "SetResearchInsert",
                        LMS_SYSTEM.MANAGE,
                        "CLT.WEB.UI.LMS.MANAGE",
                        (object)xRes_Params);

                if (xRtn.ToUpper() != "FALSE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    ViewState["rMode"] = "edit";
                    ViewState["rResNo"] = xRtn;
                    this.btnSave.Enabled = false;
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                }
                //Response.Redirect("/curr/survey_list.aspx");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : ResearchUpdate 
        * Purpose       : 설문조사 내용을 Update
        * Input         : Void
        * Output        : Void
        *************************************************************/
        #region ResearchUpdate()
        public void ResearchUpdate()
        {
            try
            {
                int xResCnt = 0;  // 설문 갯수관련 변수   
                string xScriptContent = string.Empty;  // 필수 입력값 체크 관련 Message 변수


                if (Request.QueryString["rResQueCnt"] != null && Request.QueryString["rResQueCnt"].ToString() != "")
                    xResCnt = Convert.ToInt32(Request.QueryString["rResQueCnt"].ToString());  // 사용자가 선택한 설문문항 갯수 

                // 0 : 설문 ID
                // 1 : 사용자가 선택한 설문문항 갯수
                // 2 : Data를 수정한 사용자 ID
                string[] xMasterParams = new string[3];
                xMasterParams[0] = ViewState["rResNo"].ToString(); // 설문ID
                xMasterParams[1] = xResCnt.ToString(); // 사용자가 선택한 설문문항 갯수 
                xMasterParams[2] = Session["USER_ID"].ToString(); // 사용자 ID

                // ,0 : 질문
                // ,1 : 설문형태
                // ,2 : 보기 항목수



                // ,3 : 보기1
                // ,4 : 보기2
                // ,5 : 보기3
                // ,6 : 보기4
                // ,7 : 보기5
                // ,8 : 보기6
                // ,9 : 보기7
                string[,] xDetailParams = new string[xResCnt, 10]; // 설문문제 입력 갯수

                bool xMessageChk = false;
                // 사용자가 선택한 설문문항 갯수 만큼 Data 입력 체크
                for (int i = 0; i < xResCnt; i++)
                {
                    xDetailParams[i, 0] = string.Empty; // 배열 초기화

                    xDetailParams[i, 1] = string.Empty; // 배열 초기화

                    xDetailParams[i, 2] = string.Empty; // 배열 초기화


                    xDetailParams[i, 3] = string.Empty; // 배열 초기화

                    xDetailParams[i, 4] = string.Empty; // 배열 초기화

                    xDetailParams[i, 5] = string.Empty; // 배열 초기화


                    xDetailParams[i, 6] = string.Empty; // 배열 초기화


                    xDetailParams[i, 7] = string.Empty; // 배열 초기화


                    xDetailParams[i, 8] = string.Empty; // 배열 초기화


                    xDetailParams[i, 9] = string.Empty; // 배열 초기화


                    TextBox txtRes_Content = (TextBox)this.ph01.FindControl("txtRes_Content" + i.ToString());  // 질문내용 체크

                    int xLength = HlenUTF8(txtRes_Content.Text);
                    if (xLength > 150)
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A005", new string[] { "질문" + Convert.ToString(i + 1), "50", "150" }, new string[] { "Question" + Convert.ToString(i + 1), "50", "150" }, Thread.CurrentThread.CurrentCulture));
                        txtRes_Content.Focus();
                        xMessageChk = true;
                        break;
                    }

                    if (string.IsNullOrEmpty(txtRes_Content.Text))
                    {
                        //xScriptContent = string.Format("<script>alert('{0} 번 설문의 질문이 입력되지 않았습니다!');</script>", Convert.ToString(i + 1));
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A113", new string[] { Convert.ToString(i + 1) }, new string[] { Convert.ToString(i + 1) }, Thread.CurrentThread.CurrentCulture));
                        xMessageChk = true;
                        break;
                    }

                    xDetailParams[i, 0] = txtRes_Content.Text.Replace("'", "''"); // 질문


                    // RadioButton ID 생성 규칙
                    // rbSI : 단일선택
                    // rbMI : 다중선택
                    // rbDE : 서술형



                    // rbSM : 단일선택(혼합형)
                    // rbMM : 다중선택(혼합형)
                    // rbSE : 순서배열
                    RadioButton rbSI = (RadioButton)this.ph01.FindControl("rbSI" + i.ToString()); // 단일선택
                    RadioButton rbMI = (RadioButton)this.ph01.FindControl("rbMI" + i.ToString()); // 다중선택
                    RadioButton rbDE = (RadioButton)this.ph01.FindControl("rbDE" + i.ToString()); // 서술형



                    RadioButton rbSM = (RadioButton)this.ph01.FindControl("rbSM" + i.ToString()); // 단일선택(혼합형)
                    RadioButton rbMM = (RadioButton)this.ph01.FindControl("rbMM" + i.ToString()); // 다중선택(혼합형)
                    RadioButton rbSE = (RadioButton)this.ph01.FindControl("rbSE" + i.ToString()); // 순서배열

                    if (rbSI.Checked == true)
                        xDetailParams[i, 1] = "000001";  //Single Selection       단일선택	       	000001

                    else if (rbMI.Checked == true)
                        xDetailParams[i, 1] = "000002";  //Mulit Selection        다중선택   		000002

                    else if (rbDE.Checked == true)
                        xDetailParams[i, 1] = "000003";  //Descriptive            서술형     		000003

                    else if (rbSM.Checked == true)
                        xDetailParams[i, 1] = "000004";  //Single Selection(Mix)  단일선택(혼합형)	000004

                    else if (rbMM.Checked == true)
                        xDetailParams[i, 1] = "000005";  //Multi Selection(Mix)   다중선택(혼합형)	000005

                    else if (rbSE.Checked == true)
                        xDetailParams[i, 1] = "000006";  //Sequencing             순서배열		    000006


                    if (string.IsNullOrEmpty(xDetailParams[i, 1]))
                    {
                        //xScriptContent = string.Format("<script>alert('{0} 번 설문의 설문형태가 선택되지 않았습니다!');</script>", Convert.ToString(i + 1));
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A114", new string[] { Convert.ToString(i + 1) }, new string[] { Convert.ToString(i + 1) }, Thread.CurrentThread.CurrentCulture));
                        xMessageChk = true;
                        break;
                    }

                    DropDownList ddlContent = (DropDownList)this.ph01.FindControl("System.Web.UI.WebControls.DropDownList" + i.ToString());
                    //xDetailParams[i, 2] = ddlContent.SelectedItem.Value;  // 보기 항목수




                    bool xBreakChk = false;

                    for (int j = 0; j < Convert.ToInt32(ddlContent.SelectedItem.Value); j++)  // 보기 항목수 만큼 보기 문항 입력 및 체크
                    {
                        TextBox txtExam = (TextBox)this.ph01.FindControl("txtExam" + i.ToString() + j.ToString());

                        int xQLength = HlenUTF8(txtExam.Text);
                        if (xQLength > 500)
                        {
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A005", new string[] { Convert.ToString(i + 1) + "번 설문의 " + Convert.ToString(j + 1) + "번 보기", "166", "500" }, new string[] { Convert.ToString(i + 1) + "No " + Convert.ToString(j + 1) + " Example", "166", "500" }, Thread.CurrentThread.CurrentCulture));
                            txtExam.Focus();
                            xMessageChk = true;
                            xBreakChk = true;
                            break;
                        }

                        if (xDetailParams[i, 1].ToString() != "000003")  // 서술형은 보기 항목을 입력하지 않음..
                        {
                            if (string.IsNullOrEmpty(txtExam.Text))
                            {
                                //xScriptContent = string.Format("<script>alert('{0} 번 설문의 {1} 번 보기가 입력되지 않았습니다!');</script>", Convert.ToString(i + 1), Convert.ToString(j + 1));
                                ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A115", new string[] { Convert.ToString(i + 1), Convert.ToString(j + 1) }, new string[] { Convert.ToString(i + 1), Convert.ToString(j + 1) }, Thread.CurrentThread.CurrentCulture));
                                xBreakChk = true;
                                xMessageChk = true;
                                break;
                            }
                        }
                        xDetailParams[i, j + 3] = txtExam.Text.Replace("'", "''"); // 보기항목 1~7
                    }

                    if (xDetailParams[i, 1].ToString() == "000004" || xDetailParams[i, 1].ToString() == "000005")  // 혼합형이면...
                    {
                        int xCount = Convert.ToInt32(ddlContent.SelectedItem.Value);
                        xDetailParams[i, 2] = Convert.ToString(xCount + 1);  // 보기 항목수


                        xDetailParams[i, xCount + 3] = string.Empty; //"";
                    }
                    else
                        xDetailParams[i, 2] = ddlContent.SelectedItem.Value;  // 보기 항목수

                    //    xDetailParams[i, 9] = " "; // 혼합형 서술...

                    if (xBreakChk == true)
                        break;
                }

                if (xMessageChk == true)
                    return;



                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;
                object[] xRes_Params = new object[3];
                xRes_Params[0] = (string[])xMasterParams;   // T_RESEARCH 테이블 Parameter 
                xRes_Params[1] = (string[,])xDetailParams;  // T_RESEARCH_DETAIL 테이블 Parameter
                                                            //xRes_Params[2] = (DataTable)xDt;            // T_RESEARCH_TARGET 테이블 Parameter


                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                        "SetResearchUpdate",
                        LMS_SYSTEM.MANAGE,
                        "CLT.WEB.UI.LMS.MANAGE",
                        (object)xRes_Params);

                if (xRtn.ToUpper() == "TRUE")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    //xScriptMsg = "<script>window.location.href='/curr/survey_list.aspx';</script>";
                    //ScriptHelper.ScriptBlock(this, "survey_insdetail", xScriptMsg);
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                }
                
                //Response.Redirect("/curr/survey_list.aspx");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : UnMix 
        * Purpose       : '|' 형태로 구분된 문자열을 분리하여 ArrayList 에 넣어준다.
        * Input         : string[] rParams
        * Output        : ArrayList
        *************************************************************/
        #region UnMix(string rParams)
        public ArrayList UnMix(string rParams)
        {

            string[] xResult = rParams.Split('┼');
            ArrayList alList = new ArrayList();
            try
            {
                foreach (string xCode in xResult)
                {
                    if (!string.IsNullOrEmpty(xCode))
                    {
                        if (!alList.Contains(xCode))
                            alList.Add(xCode);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return alList;
        }

        #endregion

        /********************************************************
       * Function name  : HlenUTF8
       * Purpose		: 문자열 인자의 크기(Byte)를 반환
       *                : 한글- 3byte  / 영문 - 1byte
       * Input		    : 문자열
       * Output		    : 문자열의 크기(Byte) 
       *********************************************************/
        public static int HlenUTF8(string rStr)
        {
            int xReturn = System.Text.UTF8Encoding.UTF8.GetByteCount(rStr);  //한글 3Byte
            return xReturn;
        }
        
    }
}