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
    /// 1. 작업개요 : assess_list Class
    /// 
    /// 2. 주요기능 : 교육과정에서 사용되는 평가문제를 조회하는 화면 
    ///				  
    /// 3. Class 명 : evaluation_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2011.12.26
    /// </summary>
    public partial class assess_list : BasePage
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

                base.pRender(this.Page, new object[,] { { this.btnRetrieve, "I" } });
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

                xParams[2] = this.ddlClassification.SelectedItem.Value.Replace("*", ""); //분류 kind 
                xParams[3] = this.ddlLang.SelectedItem.Value.Replace("*", ""); //언어 language
                xParams[4] = this.ddlExamType.SelectedItem.Value.Replace("*", ""); //시험유형 type 
                xParams[5] = this.txtQuestion.Text; // 질문 content 
                xParams[6] = this.ddlGroup.SelectedItem.Value.Replace("*", ""); // 과정 그룹  group 
                xParams[7] = this.ddlField.SelectedItem.Value.Replace("*", ""); // 분야 field

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_assess_md",
                                             "GetAssessList",
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

                //kind 
                xParams[0] = "0042";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlClassification, xDt);

                // Lang
                xParams[0] = "0017";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlLang, xDt);

                //type
                xParams = new string[2];
                xParams[0] = "0045";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlExamType, xDt);

                //group 
                xParams = new string[1];
                xParams[0] = "0003";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlGroup, xDt);

                //field 
                xParams[0] = "0004";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlField, xDt);
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
                        e.Item.Cells[1].Text = "분류";
                        e.Item.Cells[2].Text = "과정그룹";
                        e.Item.Cells[3].Text = "과정분야";
                        e.Item.Cells[4].Text = "질문";
                        e.Item.Cells[5].Text = "시험유형";
                        e.Item.Cells[6].Text = "등록일자";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Classification";
                        e.Item.Cells[2].Text = "Course Group";
                        e.Item.Cells[3].Text = "Course Field";
                        e.Item.Cells[4].Text = "Question";
                        e.Item.Cells[5].Text = "Exam Type";
                        e.Item.Cells[6].Text = "Reg. Date";
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
