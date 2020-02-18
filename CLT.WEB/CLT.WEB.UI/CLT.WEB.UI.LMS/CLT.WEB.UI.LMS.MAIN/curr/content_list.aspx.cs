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

// 필수 using 문using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading; 

namespace CLT.WEB.UI.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : content_list Class
    /// 
    /// 2. 주요기능 : 컨텐츠 목록을 조회하는 처리
    ///				  
    /// 3. Class 명 : content_list
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.08
    /// </summary>
    public partial class content_list : BasePage
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
       * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
       * Input         : void
       * Output        : void
       *************************************************************/
        private void BindGrid()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")
                {
                    // 조회조건이 없을 경우 처리
                    xParams = new string[2];
                    xParams[0] = this.PageSize.ToString(); // pagesize
                    xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_content_md",
                                                 "GetContentsInfo",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);
                }
                else
                {
                    // 조회조건이 있을 경우 처리
                    xParams = new string[6];
                    xParams[0] = this.PageSize.ToString(); // pagesize
                    xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                    xParams[2] = this.txtContentsNM.Text; // contents_nm
                    xParams[3] = this.txtRemark.Text; // remark
                    xParams[4] = this.ddlContentsLang.SelectedItem.Value.Replace("*", ""); // lang
                    xParams[5] = this.ddlContentsType.SelectedItem.Value.Replace("*", ""); // contents_type

                    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_content_md",
                                                 "GetContentsInfoByCondition",
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

                // Content Type
                xParams[0] = "0042";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlContentsType, xDt);

                // Lang
                xParams[0] = "0017";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlContentsLang, xDt);
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
                string[] xParams = new string[6];

                xParams[0] = string.Empty;
                xParams[1] = string.Empty; 
                xParams[2] = this.txtContentsNM.Text; // contents_nm
                xParams[3] = this.txtRemark.Text; // remark
                xParams[4] = this.ddlContentsLang.SelectedItem.Value.Replace("*", ""); // lang
                xParams[5] = this.ddlContentsType.SelectedItem.Value.Replace("*", ""); // contents_type

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_content_md",
                                                 "GetContentsInfoForExcel",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);

                string[] xHeader = new string[7];

                if (this.IsSettingKorean())
                {
                    xHeader[0] = "No.";
                    xHeader[1] = "컨텐츠명";
                    xHeader[2] = "파일명";
                    xHeader[3] = "분류";
                    xHeader[4] = "언어";
                    xHeader[5] = "Remark";
                    xHeader[6] = "등록일";
                }
                else
                {
                    xHeader[0] = "No.";
                    xHeader[1] = "Contents Name";
                    xHeader[2] = "File Name";
                    xHeader[3] = "Type";
                    xHeader[4] = "Language";
                    xHeader[5] = "Remark";
                    xHeader[6] = "Reg Date";
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
        protected void C1WebGrid1_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "컨텐츠명";
                        e.Item.Cells[2].Text = "파일명";
                        e.Item.Cells[3].Text = "분류";
                        e.Item.Cells[4].Text = "언어";
                        e.Item.Cells[5].Text = "Remark";
                        e.Item.Cells[6].Text = "등록일";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Contents Name";
                        e.Item.Cells[2].Text = "File Name";
                        e.Item.Cells[3].Text = "Type";
                        e.Item.Cells[4].Text = "Language";
                        e.Item.Cells[5].Text = "Remark";
                        e.Item.Cells[6].Text = "Reg Date";
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
