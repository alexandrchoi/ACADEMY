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

namespace CLT.WEB.UI.LMS.COMMUNITY
{
    /// <summary>
    /// 1. 작업개요 : 설문조사 Class
    /// 
    /// 2. 주요기능 : LMS 설문조사
    ///				  
    /// 3. Class 명 : survey_answer_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.01
    ///
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class survey_answer_list : BasePage
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
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                if (!IsPostBack)
                {

                    this.PageInfo1.TotalRecordCount = 0;
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageNavigator1.TotalRecordCount = 0;


                    ViewState["UserIDInfo"] = Session["USER_ID"].ToString();

                    SetGridClear(this.C1WebGrid1, this.PageNavigator1, this.PageInfo1);

                    if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                    {
                        //return;
                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "survey_answer_list", xScriptMsg);
                    }
                    else
                    {
                        BindGrid();
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
        * Function name : button_Click
        * Purpose       : 설문조사 조회버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region button_OnClick(object sender, EventArgs e)
        protected void button_OnClick(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
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
        protected void grd_ItemCreated(object seder, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "설문기간";
                        e.Item.Cells[1].Text = "설문제목";
                        e.Item.Cells[2].Text = "작성자";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "Survey Period";
                        e.Item.Cells[1].Text = "Subject";
                        e.Item.Cells[2].Text = "Author";
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
        * Function name : grd_OnItemDataBound
        * Purpose       : C1WebGrid의 Data Bound 이젠트
                          C1WebGrid Unbound 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grd_OnItemDataBound(object sender, C1ItemEventArgs e)
        {
            //DataRowView xItem = (DataRowView)e.Item.DataItem;

            //Label lblSumCnt = null;

            //if (e.Item.ItemType == C1ListItemType.Item || e.Item.ItemType == C1ListItemType.AlternatingItem)
            //{
            //    if (xItem != null)
            //    {
            //        if (string.IsNullOrEmpty(xItem["open_course_id"].ToString())) // OpenCourse ID가 null 이면 일반설문
            //        {



            //            lblSumCnt = ((Label)e.Item.FindControl("lblSumCnt"));

            //        }
            //        else
            //        {

            //        }
            //    }
            //}

            ////lblSumCnt
        }

        /************************************************************
        * Function name : BindGrid
        * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region BindGrid()
        public void BindGrid()
        {
            try
            {
                string[] xParams = new string[7];
                DataTable xDt = null;

                DataTable xDtSocialpos = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                       "GetUSERSOCIALPOS",
                       LMS_SYSTEM.COMMUNITY,
                       "CLT.WEB.UI.LMS.COMMUNITY",
                       (object)new string[1] { Session["USER_ID"].ToString() });

                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = Session["USER_ID"].ToString();
                xParams[3] = Session["USER_GROUP"].ToString();
                xParams[4] = Session["COMPANY_ID"].ToString();
                xParams[5] = Session["DUTY_STEP"].ToString();


                if (xDtSocialpos.Rows.Count > 0)
                    xParams[6] = xDtSocialpos.Rows[0]["SOCIALPOS"].ToString();
                else
                    xParams[6] = string.Empty;


                if (Request.QueryString["OPEN_COURSE_ID"] != null)
                {
                    xParams[3] = Request.QueryString["OPEN_COURSE_ID"].ToString();
                    //xParams[4] = Request.QueryString["USER_ID"].ToString();
                }

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                       "GetAnswerSuveyInfo",
                                       LMS_SYSTEM.COMMUNITY,
                                       "CLT.WEB.UI.LMS.COMMUNITY",
                                       (object)xParams);

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
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        #region PageNavigator1_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
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