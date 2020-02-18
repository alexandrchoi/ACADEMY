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
    public partial class survey_ins : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 설문조사 페이지 Load 이벤트
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
                    ScriptHelper.ScriptBlock(this, "survey_ins", xScriptMsg);

                    return;
                }

                if (!IsPostBack)
                {
                    //this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                    base.pRender(this.Page, new object[,] { { this.btnNext, "E" }},
                        Convert.ToString(Request.QueryString["MenuCode"]));

                    //this.txtRes_From.Attributes.Add("onkeyup", "ChkDate(this);");
                    //this.txtRes_To.Attributes.Add("onkeyup", "ChkDate(this);");

                    //this.txtPeriodFrom.Attributes.Add("onkeyup", "ChkDate(this);");
                    //this.txtPeriodTo.Attributes.Add("onkeyup", "ChkDate(this);");


                    ViewState["UserIDInfo"] = Session["USER_ID"].ToString();
                    BindList();

                    if (rbRes_Kind.Checked == true)  // 일반설문
                    {
                        TRDATE.Visible = false;
                        TRCUS.Visible = false;
                    }
                    else
                    {
                        TRDATE.Visible = true;
                        TRCUS.Visible = true;
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
        * Function name : btnSelect_OnClick
        * Purpose       : 교육기간 조회 버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnSelect_OnClick(object seder, EventArgs e)
        protected void btnSelect_OnClick(object seder, EventArgs e)
        {
            try
            {
                string[] xParams = new string[3];
                xParams[0] = HiddenCourseID.Value;
                xParams[1] = txtPeriodFrom.Text;
                xParams[2] = txtPeriodTo.Text;


                DataTable xDt = new DataTable();
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                       "GetOpenCourseDate",
                                       LMS_SYSTEM.MANAGE,
                                       "CLT.WEB.UI.LMS.MANAGE", (object)xParams);

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

        /************************************************************
        * Function name : btnNext_OnClick
        * Purpose       : 설문생성 페이지 이동 버튼
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnNext_OnClick(object sender, EventArgs e)
        protected void btnNext_OnClick(object sender, EventArgs e)
        {
            try
            {
                

                string xSend_yn = string.Empty;  // 선박전송여부
                string xCompany = string.Empty;  // 법인사 ID (구분자 : |)
                string xUsergroup = string.Empty; // 사용자 그룹 (구분자 : I)
                string xSocialpos = string.Empty; // 직책
                string xDutystep = string.Empty; // 직급
                string xCourse = string.Empty; // 과정ID
                string xCoursefrom = string.Empty;  // 과정 교육기간 From(YYYYMMDD)
                string xCourseTo = string.Empty;  // 과정 교육기간 To(YYYYMMDD)
                string xResQueCnt = string.Empty; // 설문 문항
                string xResSub = string.Empty;  // 설문 제목
                string xResObj = string.Empty;  // 설문 목적
                string xResFrom = string.Empty;  // 설문 조사기간 From
                string xResTo = string.Empty;  // 설문 조사기간 To
                string xResKind = string.Empty; // 설문유형
                string xScriptMsg = string.Empty; // 필수항목체크용 Message 변수
                string xMenuCode = Session["MENU_CODE"].ToString();

                

                // 선박 전송여부
                if (this.chkSent_YN.Checked == true)
                {
                    xSend_yn = "1";  // (1: 대상, NullOrEmpty : 전송대상이 아님)

                    int nCnt = 0;
                    foreach (ListItem Items in lboxDutystep.Items)
                    {
                        if (Items.Selected == true)
                            nCnt++;
                    }

                    if (nCnt == 0)
                        this.lboxDutystep.SelectedValue = "*";
                }
                else
                    xSend_yn = "3";

                // 설문 유형
                if (this.rbRes_Kind.Checked == true)
                {
                    xResKind = "000001"; // 일반설문
                    WebControlHelper.SetSelectText_DropDownList(this.ddlCus_Date, "*");
                }
                else if (this.rbRes_KindCurr.Checked == true)
                {
                    xResKind = "000002"; // 과정설문
                    this.lboxCompany.SelectedValue = "*";
                    this.lboxDutystep.SelectedValue = "*";
                    this.lboxSocialpos.SelectedValue = "*";
                    this.lboxUserGorup.SelectedValue = "*";
                }

                // 법인사 ID
                xCompany = MixData(this.lboxCompany);

                // 사용자 그룹ID
                xUsergroup = MixData(this.lboxUserGorup);

                // 직책 Code
                xSocialpos = MixData(this.lboxSocialpos);

                // 직급 Code
                xDutystep = MixData(this.lboxDutystep);

                // 과정 ID
                if (this.ddlCus_Date.SelectedItem != null)
                {
                    if (this.ddlCus_Date.SelectedItem.Text != "*")
                    {
                        xCourse = this.ddlCus_Date.SelectedItem.Value;

                        string[] xCusDate = this.ddlCus_Date.SelectedItem.Text.Split('~');
                        int xCount = 0;
                        foreach (string xDate in xCusDate)
                        {
                            if (xCount == 0)
                                xCoursefrom = xDate.Trim();
                            else if (xCount == 1)
                                xCourseTo = xDate.Trim();

                            xCount++;
                        }
                    }
                }               

                // 과정 교육기간 From(YYYY.MM.DD)
                //xCoursefrom = this.txtCus_From.Text;

                //// 과정 교육기간To(YYYY.MM.DD)
                //xCourseTo = this.txtCus_To.Text;

                // 설문 문항
                if (this.ddlResquecnt.SelectedItem.Text != "*")
                    xResQueCnt = this.ddlResquecnt.SelectedValue;

                // 설문 제목
                xResSub = this.txtRes_SUB.Text.Replace("'", "''");

                // 설문 목적
                xResObj = this.txtRes_Obj.Text.Replace("\r", "").Replace("\n", " ").Replace("'", "''");

                // 설문 조사기간 From
                xResFrom = this.txtRes_From.Text;

                // 설문 조사기간 To
                xResTo = this.txtRes_To.Text;


                if (rbRes_Kind.Checked == true) // 설문이 일반 설문일 경우
                {
                    // Data 필수입력값 체크
                    if (string.IsNullOrEmpty(xUsergroup))  // 1. 회원 그룹별
                    {
                        //xScriptMsg = "<script>alert('회원그룹을 선택해 주세요!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "회원그룹" }, new string[] { "User Group" }, Thread.CurrentThread.CurrentCulture));
                        this.lboxUserGorup.Focus();
                        return;
                    }
                }

                if (string.IsNullOrEmpty(xResQueCnt))  // 2. 설문문항
                {
                    //xScriptMsg = "<script>alert('설문문항을 선택해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "설문문항" }, new string[] { "User Group" }, Thread.CurrentThread.CurrentCulture));
                    this.lblResquecnt.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(xResSub))  // 3. 설문제목
                {
                    //xScriptMsg = "<script>alert('설문제목을 입력해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "설문제목" }, new string[] { "Survey Title" }, Thread.CurrentThread.CurrentCulture));
                    this.txtRes_SUB.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(xResObj))  // 4. 설문목적
                {
                    //xScriptMsg = "<script>alert('설문목적을 선택해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "설문목적" }, new string[] { "Purpose of the survey" }, Thread.CurrentThread.CurrentCulture));
                    this.txtRes_Obj.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(xResFrom))  // 5. 투표기한 From
                {
                    //xScriptMsg = "<script>alert('투표기한을 선택해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "투표기한" }, new string[] { "Response Period" }, Thread.CurrentThread.CurrentCulture));
                    this.txtRes_From.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(xResTo))  // 5. 투표기한 To
                {
                    //xScriptMsg = "<script>alert('투표기한을 선택해 주세요!');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "투표기한" }, new string[] { "Response Period" }, Thread.CurrentThread.CurrentCulture));
                    this.txtRes_To.Focus();
                    return;
                }

                if (rbRes_KindCurr.Checked == true) // 설문이 과정 설문일 경우
                {
                    if (string.IsNullOrEmpty(xCourse))
                    {
                        //xScriptMsg = "<script>alert('과정을 선택해 주세요!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "과정" }, new string[] { "Course" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    else if (string.IsNullOrEmpty(xCoursefrom)) // 과정 기간 From
                    {
                        //xScriptMsg = "<script>alert('과정교육기간을 선택해 주세요!');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "과정 교육기간" }, new string[] { "Education Period" }, Thread.CurrentThread.CurrentCulture));
                        this.ddlCus_Date.Focus();
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(xScriptMsg))
                {
                    Response.Write(xScriptMsg);
                    return;
                }

                string xUrl = "/curr/survey_insdetail.aspx?";
                xUrl += string.Format("rSendyn={0}&", xSend_yn);
                xUrl += string.Format("rCompany={0}&", xCompany);
                xUrl += string.Format("rUsergroup={0}&", xUsergroup);
                xUrl += string.Format("rSocialpos={0}&", xSocialpos);
                xUrl += string.Format("rDutystep={0}&", xDutystep);
                xUrl += string.Format("rCourse={0}&", xCourse);
                xUrl += string.Format("rCoursefrom={0}&", xCoursefrom);
                xUrl += string.Format("rCourseTo={0}&", xCourseTo);
                xUrl += string.Format("rResQueCnt={0}&", xResQueCnt);
                //xUrl += string.Format("rResSub={0}&", xResSub);  // 설문제목
                //xUrl += string.Format("rResObj={0}&", xResObj);  // 설문목적
                xUrl += string.Format("rResFrom={0}&", xResFrom);
                xUrl += string.Format("rResTo={0}&", xResTo);
                xUrl += string.Format("rResKind={0}&", xResKind);
                xUrl += string.Format("rSendFLG={0}&", xSend_yn); // 선박전송여부
                xUrl += string.Format("rMode={0}&", "new"); // 신규모드
                xUrl += "rNoticeyn=N&";  // 게시여부
                xUrl += string.Format("MenuCode={0}", xMenuCode);

                // 값이 너무 많아서 제목, 목적은 세션으로 넘긴다.
                Session["RESSUB"] = xResSub; // 설문제목
                Session["RESOBJ"] = xResObj; // 설문목적

                Response.Redirect(xUrl);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : rbRes_Kind_OnCheckedChanged
        * Purpose       : 일반설문 라디오박스 체크 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region rbRes_Kind_OnCheckedChanged(object sender, EventArgs e)
        protected void rbRes_Kind_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton rbuton = (RadioButton)sender;

                LiteralControl spanLiteral = (LiteralControl)this.ph01.FindControl("M");
                if (spanLiteral != null)
                    this.ph01.Controls.Remove(spanLiteral);

                if (rbuton.Checked == true)
                {
                    TRDATE.Visible = false;
                    TRCUS.Visible = false;
                    TRDUTY.Visible = true;
                    TRUSER.Visible = true;
                    TDTARGET.RowSpan = 3;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : rbRes_Kind_OnCheckedChanged
        * Purpose       :과정설문 라디오박스 체크 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region rbRes_KinkCurr_OnCheckedChanged(object sender, EventArgs e)
        protected void rbRes_KinkCurr_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton rbuton = (RadioButton)sender;

                LiteralControl spanLiteral1 = new LiteralControl("<span class=\"required\">필수입력</span>");
                spanLiteral1.ID = "M1";

                LiteralControl spanLiteral2 = new LiteralControl("<span class=\"required\">필수입력</span>");
                spanLiteral2.ID = "M2";

                this.ph01.Controls.Add(spanLiteral1);
                this.ph02.Controls.Add(spanLiteral2);

                if (rbuton.Checked == true)
                {
                    TRDATE.Visible = true;
                    TRCUS.Visible = true;
                    TRDUTY.Visible = false;
                    TRUSER.Visible = false;
                    TDTARGET.RowSpan = 3;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : BindList
        * Purpose       : ListBox 데이터 바인딩을 위한 처리 메서드
        * Input         : void
        * Output        : void
        *************************************************************/
        #region BindList()
        public void BindList()
        {
            try
            {
                // 법인사 리스트

                string[] xParams = new string[2];
                string xSql = string.Empty;
                DataTable xDt = null;

                xParams[0] = "0";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                             "GetCompanyList",
                                             LMS_SYSTEM.MANAGE,
                                             "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetListBox(this.lboxCompany, xDt, "company_nm", "company_id", true);
                


                // 사용자 그룹 리스트

                xParams[0] = "0041";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.MANAGE,
                                             "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                
                foreach (DataRow xDrs in xDt.Rows)
                {
                    if (xDrs["d_cd"].ToString() == "000009")
                    {
                        xDt.Rows.Remove(xDrs);
                        break;
                    }
                }

                WebControlHelper.SetListBox(this.lboxUserGorup, xDt, true);



                // 신분구분(해상직원, 용역, 촉탁...) 리스트(Socialpos)
                xParams[0] = "HC11";  // 신분구분코드

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                             "GetSocialposCodeInfo",
                                             LMS_SYSTEM.MANAGE,
                                             "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetListBox(this.lboxSocialpos, xDt, "dname", "dcode", true);



                // 직책(사장, 과장, 부장...)코드 리스트 Dutystep
                xParams[1] = "Y";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                 "GetDutystepCodeInfo",
                                 LMS_SYSTEM.MANAGE,
                                 "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetListBox(this.lboxDutystep, xDt, "step_name", "duty_step", true);

                // 과정 리스트

                xParams[1] = "Y";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                             "GetCourseList",
                                             LMS_SYSTEM.MANAGE,
                                             "CLT.WEB.UI.LMS.MANAGE", (object)xParams);


                //WebControlHelper.SetDropDownList(this.ddlCourse, xDt, "course_nm", "course_id");

                string[,] xResque = new string[50, 2];
                int xCount = 1;

                for (int i = 0; i < xResque.GetLength(0); i++)
                {
                    xResque[i, 0] = xCount.ToString() + " Item";
                    xResque[i, 1] = xCount.ToString();

                    xCount++;
                }

                WebControlHelper.SetDropDownList(this.ddlResquecnt, xResque);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion


        /************************************************************
        * Function name : MixData
        * Purpose       : ListBox Data 에서 선택된 자료를 String 자료로 Return (구분자: |)
        * Input         : ListBox rListBox
        * Output        : string
        *************************************************************/
        #region MixData(ListBox rListBox)
        public string MixData(ListBox rListBox)
        {
            string xResult = string.Empty;
            ArrayList alReult = new ArrayList();
            try
            {
                if (rListBox.Items[0].Selected == true) // * 를 선택 했으면
                {
                    for (int i = 0; i < rListBox.Items.Count; i++)
                    {
                        if (!alReult.Contains(rListBox.Items[i].Value))
                        {
                            if (rListBox.Items[i].Text != "*")
                            {
                                xResult += rListBox.Items[i].Value + "┼";
                                alReult.Add(rListBox.Items[i].Value);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < rListBox.Items.Count; i++)
                    {
                        if (rListBox.Items[i].Selected == true)
                        {
                            if (!alReult.Contains(rListBox.Items[i].Value))
                            {
                                if (rListBox.Items[i].Text != "*")
                                {
                                    xResult += rListBox.Items[i].Value + "┼";
                                    alReult.Add(rListBox.Items[i].Value);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xResult;
        }
        #endregion

    }
}