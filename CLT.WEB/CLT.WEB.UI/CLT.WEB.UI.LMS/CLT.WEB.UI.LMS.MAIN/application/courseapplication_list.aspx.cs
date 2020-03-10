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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : 수강신청 조회 Class
    /// 
    /// 2. 주요기능 : LMS 수강신청 조회 화면
    ///				  
    /// 3. Class 명 : courseapplication
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.01
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class courseapplication_list : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 수강신청 페이지 Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                if (!IsPostBack)
                {
                    //DateTime xdtFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    //this.txtCurr_From.Text = xdtFrom.ToString("yyyy.MM.dd");
                    //this.txtCurr_To.Text = DateTime.Now.ToString("yyyy.MM.dd");



                    //this.ddlCourseType.Items.Add(new ListItem("*", "*"));
                    //this.ddlCourseType.Items.Add(new ListItem("집체훈련", "000001"));
                    //this.ddlCourseType.Items.Add(new ListItem("우편원격훈련", "000002"));
                    //this.ddlCourseType.Items.Add(new ListItem("혼합훈련", "000004"));
                    //this.ddlCourseType.Items.Add(new ListItem("E-Learning", "000003"));

                    BindDropDownList();

                    this.txtCurr_From.Attributes.Add("onkeyup", "ChkDate(this);");
                    this.txtCurr_To.Attributes.Add("onkeyup", "ChkDate(this);");

                    if (IsSettingKorean())
                    {
                        this.ddlDate.Items.Add(new ListItem("신청일자", "000001"));
                        this.ddlDate.Items.Add(new ListItem("학습일자", "000002"));
                    }
                    else
                    {
                        this.ddlDate.Items.Add(new ListItem("Application Period", "000001"));
                        this.ddlDate.Items.Add(new ListItem("Education Period", "000002"));
                    }
                    SetGridClear(this.C1WebGrid1, this.PageNavigator1, this.PageInfo1);


                    if (Session["USER_GROUP"].ToString() != this.GuestUserID)
                    {
                        BindGrid();
                    }
                    else
                    {
                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "courseapplication", xScriptMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }


        /************************************************************
        * Function name : btnRetrieve_Click
        * Purpose       : 버튼클릭 이벤트
            
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnRetrieve_Click(object sender, EventArgs e)
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCurr_From.Text) && string.IsNullOrEmpty(txtCurr_To.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A039", new string[] { "기간" }, new string[] { "Period" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (string.IsNullOrEmpty(txtCurr_From.Text) && !string.IsNullOrEmpty(txtCurr_To.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A039", new string[] { "기간" }, new string[] { "Period" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                BindGrid();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : C1WebGrid1_ItemDataBound
        * Purpose       : 그리드 Databound 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region C1WebGrid1_ItemDataBound(object sender, C1ItemEventArgs e)
        protected void C1WebGrid1_ItemDataBound(object sender, C1ItemEventArgs e)
        {
            try
            {
                DataRowView xItem = (DataRowView)e.Item.DataItem;

                if (e.Item.ItemType == C1ListItemType.Item || e.Item.ItemType == C1ListItemType.AlternatingItem)
                {
                    Label lblType = ((Label)e.Item.FindControl("lblCourseType"));

                    if (xItem["course_type"] != null)
                    {
                        string[] xType = xItem["course_type"].ToString().Split('|');
                        foreach (string xCourseType in xType)
                        {
                            if (string.IsNullOrEmpty(lblType.Text))
                                lblType.Text += GetCourseType(xCourseType);
                            else
                                lblType.Text = lblType.Text + "<BR>" + GetCourseType(xCourseType);
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
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void grd_ItemCreated(object sender, C1ItemEventArgs e)
        protected void grd_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "순번";
                        e.Item.Cells[1].Text = "교육유형";
                        e.Item.Cells[2].Text = "과정명";
                        e.Item.Cells[3].Text = "차수";
                        e.Item.Cells[4].Text = "수강신청기간";
                        e.Item.Cells[5].Text = "교육기간";
                        e.Item.Cells[6].Text = "교육일수";
                        e.Item.Cells[7].Text = "신청/정원";
                        e.Item.Cells[8].Text = "상태";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No";
                        e.Item.Cells[1].Text = "Course Type";
                        e.Item.Cells[2].Text = "Course Name";
                        e.Item.Cells[3].Text = "SEQ";
                        e.Item.Cells[4].Text = "Application Period";
                        e.Item.Cells[5].Text = "Education Period";
                        e.Item.Cells[6].Text = "Days";
                        e.Item.Cells[7].Text = "Count";
                        e.Item.Cells[8].Text = "Status";
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
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void PageNavigator1_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        protected void PageNavigator1_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        {
            try
            {
                this.CurrentPageIndex = e.PageIndex;
                this.PageInfo1.CurrentPageIndex = e.PageIndex;
                this.PageNavigator1.CurrentPageIndex = e.PageIndex;
                this.BindGrid();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : BindGrid
        * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region public void BindGrid()
        public void BindGrid()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCurr_From.Text) && string.IsNullOrEmpty(txtCurr_To.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A039", new string[] { "조회기간" }, new string[] { "Period" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (string.IsNullOrEmpty(txtCurr_From.Text) && !string.IsNullOrEmpty(txtCurr_To.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A039", new string[] { "조회기간" }, new string[] { "Period" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                string[] xParams = new string[9];

                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();


                xParams[2] = Session["USER_ID"].ToString();

                xParams[3] = this.ddlDate.SelectedValue;
                xParams[4] = this.txtCurr_From.Text;
                xParams[5] = this.txtCurr_To.Text;
                xParams[6] = this.txtCus_ID.Text; // 과정 ID
                if (this.ddlCourseType.SelectedValue != "*")
                    xParams[7] = this.ddlCourseType.SelectedValue;
                else
                    xParams[7] = string.Empty;

                xParams[8] = Session["USER_GROUP"].ToString();

                DataTable xDt = new DataTable();
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_g_courseapplication_md",
                                       "GetCourseApplication",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION.courseapplication", (object)xParams, Thread.CurrentThread.CurrentCulture);


                C1WebGrid1.DataSource = xDt;
                C1WebGrid1.DataBind();

                if (xDt.Rows.Count < 1)
                {
                    this.PageInfo1.TotalRecordCount = 0;
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageNavigator1.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
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
        * Function name : GetCourseType
        * Purpose       : 개설과정에 대한 교육유형명칭을 가져온다.
        * Input         : void
        * Output        : void
        *************************************************************/
        #region
        public string GetCourseType(string rCode)
        {
            string xResult = string.Empty;
            try
            {
                if (Thread.CurrentThread.CurrentCulture.Name.ToLower() == "ko-kr")
                {
                    if (rCode == "000001") // 자체교육
                        xResult = "자체교육";
                    else if (rCode == "000002")  // 사업주위탁
                        xResult = "사업주위탁";
                    else if (rCode == "000003") // 청년취업아카데미
                        xResult = "청년취업아카데미";
                    else if (rCode == "000004") // 컨소시엄훈련 
                        xResult = "컨소시엄훈련 ";
                }
                else
                {
                    if (rCode == "000001") // 자체교육
                        xResult = "Internal Training";
                    else if (rCode == "000002")  // 사업주위탁
                        xResult = "Commissioned Education";
                    else if (rCode == "000003") // 청년취업아카데미
                        xResult = "Youth Job Academy";
                    else if (rCode == "000004") // 컨소시엄훈련
                        xResult = "Consortium";
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

        /************************************************************
        * Function name : BindDropDownList
        * Purpose       : 공통코드 Data처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region BindDropDownList()
        public void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[2];

                xParams[0] = "0006"; // 사용자 그룹
                xParams[1] = "Y";
                DataTable xDtUser = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                     "GetCommonCodeInfo",
                                                     LMS_SYSTEM.APPLICATION,
                                                     "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlCourseType, xDtUser, 0);

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