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
    /// 1. 작업개요 : 설문 결과 Class
    /// 
    /// 2. 주요기능 : LMS 설문 결과 관리

    ///				  
    /// 3. Class 명 : survey_result
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.01
    ///
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class survey_results : BasePage
    {
        string[] xHeader = null;

        /************************************************************
        * Function name : Page_Load
        * Purpose       : 설문조사 페이지 Load 이벤트
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

                    //this.txtRes_From.Attributes.Add("onkeyup", "ChkDate(this);");
                    //this.txtRes_To.Attributes.Add("onkeyup", "ChkDate(this);");

                    ViewState["UserIDInfo"] = Session["USER_ID"].ToString();

                    if (Session["USER_GROUP"].ToString() == "000009")
                    {
                        //return;
                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list_wpg", xScriptMsg);
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
        * Function name : btnRetrieve_Click
        * Purpose       : 조회 버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnRetrieve_Click(object sender, EventArgs e)
        protected void btnRetrieve_Click(object sender, EventArgs e)
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
        * Function name : btnExcel_Click
        * Purpose       : 엑셀출력 버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnExcel_Click(object sender, EventArgs e)
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string[] xParams = new string[7];
                DataTable xDt = null;

                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                if (!string.IsNullOrEmpty(txtRes_From.Text))
                    xParams[2] = txtRes_From.Text.Trim();

                if (!string.IsNullOrEmpty(txtRes_To.Text))
                    xParams[3] = txtRes_To.Text.Trim();

                if (!string.IsNullOrEmpty(txtRes_NM.Text))
                    xParams[4] = txtRes_NM.Text;

                xParams[6] = "EXCEL";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                       "GetSurveyInfo",
                                       LMS_SYSTEM.CURRICULUM,
                                       "CLT.WEB.UI.LMS.CURR",
                                       (object)xParams);

                if (xDt.Rows.Count == 0)
                {
                    // 자료가 없습니다!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A062", new string[] { "자료" }, new string[] { "Data" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                if (xDt.Rows.Count > 0)
                    GetExcelFile(xDt, xHeader);
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
                    xHeader = new string[9];
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "번호";
                        e.Item.Cells[1].Text = "설문제목";
                        e.Item.Cells[2].Text = "항목수";
                        e.Item.Cells[3].Text = "설문등록일";
                        e.Item.Cells[4].Text = "응답기간";
                        e.Item.Cells[5].Text = "게시여부";
                        e.Item.Cells[6].Text = "총대상자";
                        e.Item.Cells[7].Text = "응답수";
                        e.Item.Cells[8].Text = "미응답수";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Survey Title";
                        e.Item.Cells[2].Text = "Items";
                        e.Item.Cells[3].Text = "Creation Date";
                        e.Item.Cells[4].Text = "Response Period";
                        e.Item.Cells[5].Text = "Post Status";
                        e.Item.Cells[6].Text = "Participant";
                        e.Item.Cells[7].Text = "Answer";
                        e.Item.Cells[8].Text = "No Answer";
                    }

                    xHeader[0] = e.Item.Cells[0].Text;
                    xHeader[1] = e.Item.Cells[1].Text;
                    xHeader[2] = e.Item.Cells[2].Text;
                    xHeader[3] = e.Item.Cells[3].Text;
                    xHeader[4] = e.Item.Cells[4].Text;
                    xHeader[5] = e.Item.Cells[5].Text;
                    xHeader[6] = e.Item.Cells[6].Text;
                    xHeader[7] = e.Item.Cells[7].Text;
                    xHeader[8] = e.Item.Cells[8].Text;
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
                string[] xParams = new string[8];
                DataTable xDt = null;

                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                if (!string.IsNullOrEmpty(txtRes_From.Text))
                    xParams[2] = txtRes_From.Text.Trim().Replace("'", "''");

                if (!string.IsNullOrEmpty(txtRes_To.Text))
                    xParams[3] = txtRes_To.Text.Trim().Replace("'", "''");

                if (!string.IsNullOrEmpty(txtRes_NM.Text))
                    xParams[4] = txtRes_NM.Text.Replace("'", "''");

                xParams[5] = "Y";
                xParams[6] = "GRID";
                xParams[7] = "RESULT";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                       "GetSurveyInfo",
                                       LMS_SYSTEM.CURRICULUM,
                                       "CLT.WEB.UI.LMS.CURR",
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

    }
}