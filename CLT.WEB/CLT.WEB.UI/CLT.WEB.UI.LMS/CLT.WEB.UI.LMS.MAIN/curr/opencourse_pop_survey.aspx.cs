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
    /// 1. 작업개요 : opencourse_pop_survey Class
    /// 
    /// 2. 주요기능 : 과정개설에서 설문조사 pop 
    ///				  
    /// 3. Class 명 : opencourse_pop_survey
    /// 
    /// 4. 작 업 자 : 최인재 / 2012.03.02
    /// </summary>
    public partial class opencourse_pop_survey : BasePage
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
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list", xScriptMsg);

                    return;
                }

                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping

                if (!IsPostBack)
                {
                    this.txtBeginDt.Attributes.Add("onkeyup", "ChkDate(this);");
                    this.txtEndDt.Attributes.Add("onkeyup", "ChkDate(this);");

                    base.pRender(this.Page, new object[,] { { this.btnRetrieve, "I" } } , Convert.ToString(Request.QueryString["menucode"]));

                    ViewState["RES_NO"] = Request.QueryString["res_no"].ToString();
                    ViewState["RES_SUB"] = Request.QueryString["res_sub"].ToString();

                    if (Session["USER_GROUP"].ToString() != "000009")
                    {
                        this.BindGrid();
                    }
                    else
                    {
                        base.SetGridClear(this.grd, this.PageNavigator1);
                        this.PageInfo1.TotalRecordCount = 0;
                        this.PageInfo1.PageSize = this.PageSize;
                        this.PageNavigator1.TotalRecordCount = 0;
                    }
                }

                base.pRender(this.Page, new object[,] { { this.btnRetrieve, "I" }},Convert.ToString(Request.QueryString["MenuCode"]));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }


        /******************************************************************************************
       * Function name : BindGrid
       * Purpose       : 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        private void BindGrid()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                xParams = new string[5];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                xParams[2] = this.txtBeginDt.Text;
                xParams[3] = this.txtEndDt.Text;
                xParams[4] = this.txtResNM.Text; 


                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_opencourse_md",
                                             "GetSurveyInfo",
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





        /******************************************************************************************
       * Function name : btnRetrieve_Click
       * Purpose       : 설문조사 조회 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
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
        * Function name : grd_ItemCreated
        * Purpose       : Header Setting 
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grd_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "RES_NO";
                        e.Item.Cells[1].Text = "RES_SUB";

                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "설문제목";
                        e.Item.Cells[4].Text = "항목수";
                        e.Item.Cells[5].Text = "설문등록일";
                        e.Item.Cells[6].Text = "응답기간";
                        e.Item.Cells[7].Text = "게시여부";
                        e.Item.Cells[8].Text = "총대상자";
                        e.Item.Cells[9].Text = "응답수";
                        e.Item.Cells[10].Text = "미응답수";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "RES_NO";
                        e.Item.Cells[1].Text = "RES_SUB";

                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "Survey Title";
                        e.Item.Cells[4].Text = "Items";
                        e.Item.Cells[5].Text = "Creation Date";
                        e.Item.Cells[6].Text = "Response Period";
                        e.Item.Cells[7].Text = "Post Status";
                        e.Item.Cells[8].Text = "Participant";
                        e.Item.Cells[9].Text = "Answer";
                        e.Item.Cells[10].Text = "No Answer";

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
