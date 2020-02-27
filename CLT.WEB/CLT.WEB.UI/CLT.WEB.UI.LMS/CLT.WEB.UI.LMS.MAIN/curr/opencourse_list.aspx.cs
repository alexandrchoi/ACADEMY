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

using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading; 

namespace CLT.WEB.UI.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : 교육과정에서 사용되는 과정개설을 조회하는 화면 
    /// 
    /// 2. 주요기능 : 교육과정에서 사용되는 과정개설을 조회하는 화면 
    ///				  
    /// 3. Class 명 : opencourse_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2012.01.03
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        * 서진한 2012.08.01
    ///        * Source
    ///          opencourse_list
    ///        * Comment 
    ///          영문화 작업       
    /// </summary>
    public partial class opencourse_list : BasePage
    {
        /************************************************************
      * Function name : Page_Load
      * Purpose       : 페이지 로드될 때 처리: combo binding, 자동 조회 
      * Input         : void
      * Output        : void
      *************************************************************/
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping

                if (!IsPostBack)
                {
                    this.txtBeginDt.Attributes.Add("onkeyup", "ChkDate(this);");
                    this.txtEndDt.Attributes.Add("onkeyup", "ChkDate(this);");

                    this.BindDropDownList();
                    //this.BindGrid();
                    if (Session["USER_GROUP"].ToString() != this.GuestUserID)
                    {
                        this.BindGrid();
                    }
                    else
                    {
                        //base.SetGridClear(this.grd, this.PageNavigator1);
                        //this.PageInfo1.TotalRecordCount = 0;
                        //this.PageInfo1.PageSize = this.PageSize;
                        //this.PageNavigator1.TotalRecordCount = 0;

                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list", xScriptMsg);
                    }

                    ////최초 grid header 정보 나오게 하려면 아래와 같이~!! 
                    //this.grd.DataBind();
                    //this.PageInfo1.TotalRecordCount = 0;
                    //this.PageInfo1.PageSize = this.PageSize;
                    //this.PageNavigator1.TotalRecordCount = 0;
                }
                base.pRender(this.Page, new object[,] { { this.btnRetrieve, "I" }, { this.btnExcel, "I" } });
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

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
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
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
                xParams = new string[6];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                xParams[2] = this.txtBeginDt.Text;
                xParams[3] = this.txtEndDt.Text;
                xParams[4] = this.ddlCourseType.SelectedItem.Value.Replace("*", ""); //교육유형
                xParams[5] = this.txtCourseNM.Text; //과정명


                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_opencourse_md",
                                             "GetOpencourseList",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);


                this.grd.DataSource = xDt;
                this.grd.DataBind();

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
        * Function name : btnExcel_Click
        * Purpose       : 조회정보 excel 출력 
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //this.GetExcelFile(dt, 

                string[] xParams = null;
                DataTable xDt = null;

                //if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")            
                // 조회조건이 있을 경우 처리
                xParams = new string[6];
                xParams[0] = string.Empty; // pagesize
                xParams[1] = string.Empty; // currentPageIndex
                xParams[2] = this.txtBeginDt.Text;
                xParams[3] = this.txtEndDt.Text;
                xParams[4] = this.ddlCourseType.SelectedItem.Value.Replace("*", ""); //교육유형
                xParams[5] = this.txtCourseNM.Text; //과정명


                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_opencourse_md",
                                             "GetOpencourseList_Excel",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams);
                //col.GroupInfo.HeaderText 
                //this.grd.Columns[0].HeaderText
                string[] xHeader = new string[10];

                if (this.IsSettingKorean())
                {
                    xHeader[0] = "No.";
                    xHeader[1] = "년도";
                    xHeader[2] = "교육구분";
                    xHeader[3] = "과정명";
                    xHeader[4] = "차수";
                    xHeader[5] = "수강신청기간";
                    xHeader[6] = "교육기간";
                    xHeader[7] = "개설일자";
                    xHeader[8] = "사용여부";
                    xHeader[9] = "담당자";
                }
                else
                {
                    xHeader[0] = "No.";
                    xHeader[1] = "Year";
                    xHeader[2] = "Course Type";
                    xHeader[3] = "Course Name";
                    xHeader[4] = "Course Sequence";
                    xHeader[5] = "Apply Period";
                    xHeader[6] = "Learning Period";
                    xHeader[7] = "Date";
                    xHeader[8] = "Usage";
                    xHeader[9] = "Manager";
                }

                this.GetExcelFile(xDt, xHeader);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

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
                }
                else
                {
                    if (rCode == "000001") // 자체교육
                        xResult = "Internal Training";
                    else if (rCode == "000002")  // 사업주위탁

                        xResult = "Commissioned Education";
                    else if (rCode == "000003") // 청년취업아카데미
                        xResult = "Youth Job Academy";
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
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "년도";
                        e.Item.Cells[2].Text = "교육구분";
                        e.Item.Cells[3].Text = "과정명";
                        e.Item.Cells[4].Text = "차수";
                        e.Item.Cells[5].Text = "수강신청기간";
                        e.Item.Cells[6].Text = "교육기간";
                        e.Item.Cells[7].Text = "개설일자";
                        e.Item.Cells[8].Text = "사용여부";
                        e.Item.Cells[9].Text = "담당자";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Year";
                        e.Item.Cells[2].Text = "Course Type";
                        e.Item.Cells[3].Text = "Course Name";
                        e.Item.Cells[4].Text = "Course Sequence";
                        e.Item.Cells[5].Text = "Apply Period";
                        e.Item.Cells[6].Text = "Learning Period";
                        e.Item.Cells[7].Text = "Date";
                        e.Item.Cells[8].Text = "Usage";
                        e.Item.Cells[9].Text = "Manager";
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
