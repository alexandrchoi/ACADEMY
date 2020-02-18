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
    /// 1. 작업개요 : textbook_list Class
    /// 
    /// 2. 주요기능 : 교재 목록을 조회하는 처리
    ///				  
    /// 3. Class 명 : textbook_list
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.19
    /// </summary>
    public partial class textbook_list : BasePage
    {
        /************************************************************
       * Function name : Page_Load
       * Purpose       : 페이지 로드될 때 처리
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
                        //base.SetGridClear(this.C1WebGrid1, this.PageNavigator1);
                        //this.PageInfo1.TotalRecordCount = 0;
                        //this.PageInfo1.PageSize = this.PageSize;
                        //this.PageNavigator1.TotalRecordCount = 0;

                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list", xScriptMsg);
                    }
                }
                base.pRender(this.Page, new object[,] { { this.btnRetrieve, "I" }, { this.btnExcel, "I" } });
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
       * Function name : BindGrid
       * Purpose       : 교재 목록 데이터를 DataGrid에 바인딩을 위한 처리
       * Input         : void
       * Output        : void
       *************************************************************/
        private void BindGrid()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                if (string.IsNullOrEmpty(this.txtTextBookNM.Text) && string.IsNullOrEmpty(this.txtBeginDt.Text) && string.IsNullOrEmpty(this.txtEndDt.Text) && this.ddlTextBookType.SelectedItem.Text == "*" && this.ddlCourseGroup.SelectedItem.Text == "*" && this.ddlCourseField.SelectedItem.Text == "*" && this.ddlTextBookLang.SelectedItem.Text == "*")
                {
                    // 조회조건이 없을 경우 처리
                    xParams = new string[2];
                    xParams[0] = this.PageSize.ToString(); // pagesize
                    xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_textbook_md",
                                                 "GetTextBookInfo",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);
                }
                else
                {
                    // 조회조건이 있을 경우 처리
                    xParams = new string[9];
                    xParams[0] = this.PageSize.ToString(); // pagesize
                    xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                    xParams[2] = this.txtTextBookNM.Text; // textbook_nm
                    xParams[3] = this.ddlTextBookType.SelectedItem.Value.Replace("*", ""); // textbook_type
                    xParams[4] = this.ddlCourseGroup.SelectedItem.Value.Replace("*", ""); // course_group
                    xParams[5] = this.ddlCourseField.SelectedItem.Value.Replace("*", ""); // course_field
                    xParams[6] = this.ddlTextBookLang.SelectedItem.Value.Replace("*", ""); // textbook_lang
                    xParams[7] = this.txtBeginDt.Text; // ins_begin_dt
                    xParams[8] = this.txtEndDt.Text; // ins_end_dt

                    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_textbook_md",
                                                 "GetTextBookInfoByCondition",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);
                }

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

        /************************************************************
        * Function name : BindDropDownList
        * Purpose       : DropDownList 데이터 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        private void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[1];
                string xSql = string.Empty;
                DataTable xDt = null;

                // TextBook Type
                xParams[0] = "0049";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlTextBookType, xDt);

                // Course Group
                xParams[0] = "0003";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlCourseGroup, xDt);

                // Course Field
                xParams[0] = "0004";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlCourseField, xDt);


                // Lang
                xParams[0] = "0017";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlTextBookLang, xDt);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

        }

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
        * Purpose       : 조회 조건에 대한 결과를 Excel로 출력하는 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable xDt = null;
                string[] xParams = new string[4];

                // 조회조건이 있을 경우 처리
                xParams = new string[9];

                xParams[0] = string.Empty;
                xParams[1] = string.Empty; 

                xParams[2] = this.txtTextBookNM.Text; // textbook_nm
                xParams[3] = this.ddlTextBookType.SelectedItem.Value.Replace("*", ""); // textbook_type
                xParams[4] = this.ddlCourseGroup.SelectedItem.Value.Replace("*", ""); // course_group
                xParams[5] = this.ddlCourseField.SelectedItem.Value.Replace("*", ""); // course_field
                xParams[6] = this.ddlTextBookLang.SelectedItem.Value.Replace("*", ""); // textbook_lang
                xParams[7] = this.txtBeginDt.Text; // ins_begin_dt
                xParams[8] = this.txtEndDt.Text; // ins_end_dt

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_textbook_md",
                                                 "GetTextBookInfoForExcel",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);

                string[] xHeader = new string[9];

                if (this.IsSettingKorean())
                {
                    xHeader[0] = "No.";
                    xHeader[1] = "분류";
                    xHeader[2] = "교재명";
                    xHeader[3] = "저자";
                    xHeader[4] = "출판사";
                    xHeader[5] = "언어";
                    xHeader[6] = "등록자";
                    xHeader[7] = "등록일자";
                    xHeader[8] = "사용여부";
                }
                else
                {
                    xHeader[0] = "No.";
                    xHeader[1] = "Type";
                    xHeader[2] = "TextBook Name";
                    xHeader[3] = "Author";
                    xHeader[4] = "Publisher";
                    xHeader[5] = "Language";
                    xHeader[6] = "Ins User";
                    xHeader[7] = "Ins Date";
                    xHeader[8] = "Usage";
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
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러                          C1WebGrid 헤더의 언어설정 적용을 위한 부분        * Input         : void
        * Output        : void
        *************************************************************/
        protected void C1WebGrid1_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "분류";
                        e.Item.Cells[2].Text = "교재명";
                        e.Item.Cells[3].Text = "저자";
                        e.Item.Cells[4].Text = "출판사";
                        e.Item.Cells[5].Text = "언어";
                        e.Item.Cells[6].Text = "등록자";
                        e.Item.Cells[7].Text = "등록일자";
                        e.Item.Cells[8].Text = "사용여부";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Type";
                        e.Item.Cells[2].Text = "TextBook Name";
                        e.Item.Cells[3].Text = "Author";
                        e.Item.Cells[4].Text = "Publisher";
                        e.Item.Cells[5].Text = "Language";
                        e.Item.Cells[6].Text = "Ins User";
                        e.Item.Cells[7].Text = "Ins Date";
                        e.Item.Cells[8].Text = "Usage";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
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


    }
}