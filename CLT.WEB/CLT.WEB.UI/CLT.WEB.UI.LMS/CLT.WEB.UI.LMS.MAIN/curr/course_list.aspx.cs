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
    /// 1. 작업개요 : course_list Class
    /// 
    /// 2. 주요기능 : 과정을 조회하는 화면 
    ///				  
    /// 3. Class 명 : course_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2012.01.16
    /// </summary>
    public partial class course_list : BasePage
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
                    this.BindDropDownList();
                    if (Session["USER_GROUP"].ToString() != "000009")
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
                    //this.BindGrid();
                }

                base.pRender(this.Page, new object[,] { { this.btnRetrieve, "I" }, { this.btnExcel, "I"} });
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

                //course group 
                xParams[0] = "0003";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlGroup, xDt);

                //course field 
                xParams[0] = "0004";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlField, xDt);

                //course type
                xParams[0] = "0006";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlType, xDt);

                //vessel type
                xParams[0] = "0028";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlVslType, xDt);

                //language
                xParams[0] = "0017";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlLang, xDt);

                //usage
                xDt = new DataTable();	//DataTable 객체 생성 
                xDt.Columns.AddRange(new DataColumn[] { new DataColumn("d_knm"), new DataColumn("d_cd") }); // DataTable Range 설정             
                xDt.Rows.Add(new DataColumn("Yes"), new DataColumn("Y"));
                xDt.Rows.Add(new DataColumn("No"), new DataColumn("N"));

                WebControlHelper.SetDropDownList(this.ddlUse, xDt);
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
                xParams = new string[9];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                xParams[2] = this.ddlGroup.SelectedValue.Replace("*", "");
                xParams[3] = this.ddlField.SelectedValue.Replace("*", "");
                xParams[4] = this.ddlType.SelectedValue.Replace("*", "");
                xParams[5] = this.ddlVslType.SelectedValue.Replace("*", "");
                xParams[6] = this.ddlLang.SelectedValue.Replace("*", "");
                xParams[7] = this.txtCourseNm.Text;
                xParams[8] = this.ddlUse.SelectedValue.Replace("*", "");

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                             "GetCourseList",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams);


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

        /************************************************************
        * Function name : btnExcel_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                //if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")            
                // 조회조건이 있을 경우 처리
                xParams = new string[9];
                xParams[0] = string.Empty;
                xParams[1] = string.Empty; 

                xParams[2] = this.ddlGroup.SelectedValue.Replace("*", "");
                xParams[3] = this.ddlField.SelectedValue.Replace("*", "");
                xParams[4] = this.ddlType.SelectedValue.Replace("*", "");
                xParams[5] = this.ddlVslType.SelectedValue.Replace("*", "");
                xParams[6] = this.ddlLang.SelectedValue.Replace("*", "");
                xParams[7] = this.txtCourseNm.Text;
                xParams[8] = this.ddlUse.SelectedValue.Replace("*", "");

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                             "GetCourseList_Excel",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams);

                string[] xHeader = new string[9];

                if (this.IsSettingKorean())
                {
                    xHeader[0] = "No.";
                    xHeader[1] = "과정유형";
                    xHeader[2] = "과정코드";
                    xHeader[3] = "과정명";
                    xHeader[4] = "유효기간";
                    xHeader[5] = "언어";
                    xHeader[6] = "선종";
                    xHeader[7] = "Usage";
                    xHeader[8] = "등록일자";
                }
                else
                {
                    xHeader[0] = "No.";
                    xHeader[1] = "Course Type";
                    xHeader[2] = "Course Code";
                    xHeader[3] = "Course Name";
                    xHeader[4] = "Expired period";
                    xHeader[5] = "Language";
                    xHeader[6] = "Vessel Type";
                    xHeader[7] = "Usage";
                    xHeader[8] = "Reg. Date";
                }

                this.GetExcelFile(xDt, xHeader);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

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
                        e.Item.Cells[1].Text = "과정유형";
                        e.Item.Cells[2].Text = "과정분야";
                        e.Item.Cells[3].Text = "과정코드";
                        e.Item.Cells[4].Text = "과정명";
                        e.Item.Cells[5].Text = "유효기간";
                        e.Item.Cells[6].Text = "선종";
                        e.Item.Cells[7].Text = "사용";
                        e.Item.Cells[8].Text = "담당자";
                        e.Item.Cells[9].Text = "등록일";
                        e.Item.Cells[10].Text = "temp_save_flg";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Course Type";
                        e.Item.Cells[2].Text = "Course Field";
                        e.Item.Cells[2].Text = "Course ID";
                        e.Item.Cells[4].Text = "Course Name";
                        e.Item.Cells[5].Text = "Expired period";
                        e.Item.Cells[6].Text = "Vessel Type";
                        e.Item.Cells[7].Text = "Usage";
                        e.Item.Cells[8].Text = "Manager";
                        e.Item.Cells[9].Text = "Reg. Date";
                        e.Item.Cells[10].Text = "temp_save_flg";
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
