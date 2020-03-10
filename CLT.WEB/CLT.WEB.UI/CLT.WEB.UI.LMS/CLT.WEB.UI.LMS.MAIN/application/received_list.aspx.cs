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

namespace CLT.WEB.UI.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : 교육접수 조회 Class
    /// 
    /// 2. 주요기능 : LMS 교육접수 조회 화면
    ///				  
    /// 3. Class 명 : received_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.01
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class received_list : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "received_list", xScriptMsg);

                    return;
                }

                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID;

                if (!IsPostBack)
                {

                    //this.txtBeginDt.Attributes.Add("onkeyup", "ChkDate(this);");
                    //this.txtEndDt.Attributes.Add("onkeyup", "ChkDate(this);");

                    this.BindDropDownList();
                    //this.BindGrid();
                    if (Session["USER_GROUP"].ToString() != "000009")
                    {
                        this.BindGrid();
                    }
                    else
                    {
                        base.SetGridClear(this.C1WebGrid1, this.PageNavigator1);
                        this.PageInfo1.TotalRecordCount = 0;
                        this.PageInfo1.PageSize = this.PageSize;
                        this.PageNavigator1.TotalRecordCount = 0;
                    }
                    base.pRender(this.Page, new object[,] { { this.btnExcel, "I" }, { this.btnRetrieve, "I" } });
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
       * Function name : BindDropDownList
       * Purpose       : DropDownList 데이터 바인딩을 위한 처리
       * Input         : void
       * Output        : void
       *************************************************************/
        #region private void BindDropDownList()
        private void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[1];
                string xSql = string.Empty;
                DataTable xDt = null;

                //course type 
                xParams = new string[2];
                xParams[0] = "0006";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlCourseType, xDt);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
      * Function name : BindGrid
      * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
      * Input         : void
      * Output        : void
      *************************************************************/
        #region private void BindGrid()
        private void BindGrid()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                //if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")            
                // 조회조건이 있을 경우 처리
                xParams = new string[8];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                xParams[2] = this.txtBeginDt.Text;
                xParams[3] = this.txtEndDt.Text;
                xParams[4] = this.txtCourseNm.Text; //과정명 
                xParams[5] = this.ddlCourseType.SelectedItem.Value.Replace("*", ""); //교육유형
                xParams[6] = Session["user_id"].ToString();
                xParams[7] = Session["COMPANY_KIND"].ToString();


                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_s_received_md",
                                             "GetReceivedList",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);


                this.C1WebGrid1.DataSource = xDt;
                this.C1WebGrid1.DataBind();

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
        * Function name : btnRetrieve_Click
        * Purpose       : 조회 조건에 대한 결과를 조회하여 리스트로 출력하는 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnRetrieve_Click(object sender, EventArgs e)
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                this.CurrentPageIndex = 1;
                this.PageInfo1.CurrentPageIndex = 1;
                this.PageNavigator1.CurrentPageIndex = 1;
                this.BindGrid();
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
        * Function name : btnExcel_Click
        * Purpose       : 조회 조건에 대한 결과를 조회하여 리스트로 출력하는 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnExcel_Click(object sender, EventArgs e)
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                //if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")            
                // 조회조건이 있을 경우 처리
                xParams = new string[8];
                xParams[0] = string.Empty; // this.PageSize.ToString(); // pagesize
                xParams[1] = string.Empty; // this.CurrentPageIndex.ToString(); // currentPageIndex

                xParams[2] = this.txtBeginDt.Text;
                xParams[3] = this.txtEndDt.Text;
                xParams[4] = this.txtCourseNm.Text; //과정명 
                xParams[5] = this.ddlCourseType.SelectedItem.Value.Replace("*", ""); //교육유형
                xParams[6] = Session["user_id"].ToString();
                xParams[7] = Session["COMPANY_KIND"].ToString();


                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_s_received_md",
                                             "GetReceivedListExcel",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);

                string[] xHeader = new string[8];

                if (this.IsSettingKorean())
                {
                    xHeader[0] = "No.";
                    xHeader[1] = "교육유형";
                    xHeader[2] = "과정명";
                    xHeader[3] = "차수";
                    xHeader[4] = "수강신청기간";
                    xHeader[5] = "교육기간";
                    xHeader[6] = "교육일수";
                    xHeader[7] = "접수/정원";
                }
                else
                {
                    xHeader[0] = "No.";
                    xHeader[1] = "Course Type";
                    xHeader[2] = "Course Name";
                    xHeader[3] = "Seq.";
                    xHeader[4] = "Apply Period";
                    xHeader[5] = "Course Period";
                    xHeader[6] = "Days";
                    xHeader[7] = "Count";
                }

                this.GetExcelFile(xDt, xHeader);

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
                        e.Item.Cells[0].Text = "open_course_id";
                        e.Item.Cells[1].Text = "";
                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "교육유형";
                        e.Item.Cells[4].Text = "과정명";
                        e.Item.Cells[5].Text = "차수";
                        e.Item.Cells[6].Text = "수강신청기간";
                        e.Item.Cells[7].Text = "교육기간";
                        e.Item.Cells[8].Text = "교육일수";
                        e.Item.Cells[9].Text = "접수/정원";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "open_course_id";
                        e.Item.Cells[1].Text = "";
                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "Course Type";
                        e.Item.Cells[4].Text = "Course Name";
                        e.Item.Cells[5].Text = "Seq.";
                        e.Item.Cells[6].Text = "Apply Period";
                        e.Item.Cells[7].Text = "Course Period";
                        e.Item.Cells[8].Text = "Days";
                        e.Item.Cells[9].Text = "Count";
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
        
    }
}