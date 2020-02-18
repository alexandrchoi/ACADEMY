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

namespace CLT.WEB.UI.LMS.MYPAGE
{
    /// <summary>
    /// 1. 작업개요 : 수료확인 OJT Class
    /// 
    /// 2. 주요기능 : LMS 수료확인 OJT
    ///				  
    /// 3. Class 명 : training_ojt
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class training_ojt : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 페이지 로드될 때 처리: combo binding, 자동 조회 
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
                    this.txtOjtBeginDt.Attributes.Add("onkeyup", "ChkDate(this);");
                    this.txtOjtEndDt.Attributes.Add("onkeyup", "ChkDate(this);");

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
                        ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list_wpg", xScriptMsg);
                    }
                }

                base.pRender(this.Page, new object[,] { { this.btnRetrieve, "I" } });
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

                xParams[2] = this.txtOjtNm.Text;
                xParams[3] = this.txtOjtBeginDt.Text;
                xParams[4] = this.txtOjtEndDt.Text;
                xParams[5] = Session["COM_SNO"].ToString(); // 공통사번

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_training_md",
                                             "GetTrainingOjtList",
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
                        e.Item.Cells[3].Text = "OJT 명";
                        e.Item.Cells[4].Text = "OJT기간";
                        e.Item.Cells[5].Text = "OJT시간";
                        e.Item.Cells[6].Text = "선명";
                        e.Item.Cells[7].Text = "이수상태";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "open_course_id";
                        e.Item.Cells[1].Text = "";
                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "OJT Name";
                        e.Item.Cells[4].Text = "OJT Period";
                        e.Item.Cells[5].Text = "OJT Time";
                        e.Item.Cells[6].Text = "Vessel";
                        e.Item.Cells[7].Text = "Status";
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