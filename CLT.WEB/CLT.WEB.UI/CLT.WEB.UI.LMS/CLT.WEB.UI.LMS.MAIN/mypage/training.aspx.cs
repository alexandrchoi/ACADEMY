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
    /// 1. 작업개요 : 수료확인 Class
    /// 
    /// 2. 주요기능 : LMS 수료확인
    ///				  
    /// 3. Class 명 : training
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class training : BasePage
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
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID;

                if (!IsPostBack)
                {
                    this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping

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
                        ScriptHelper.ScriptBlock(this, "trainging", xScriptMsg);
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

                //pass flag 
                xParams = new string[1];
                xParams[0] = "0010";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlPass, xDt);
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
                xParams = new string[5];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                xParams[2] = this.ddlCourseType.SelectedItem.Value.Replace("*", ""); //교육유형
                xParams[3] = this.ddlPass.SelectedItem.Value.Replace("*", ""); //이수상태
                xParams[4] = Session["COM_SNO"].ToString(); //공통사번

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_training_md",
                                             "GetTrainingList",
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
                        e.Item.Cells[3].Text = "과정명";
                        e.Item.Cells[4].Text = "교육이수기간";
                        e.Item.Cells[5].Text = "유효기간\r\nYear(s)";
                        e.Item.Cells[6].Text = "교육기관";
                        e.Item.Cells[7].Text = "이수상태";
                        e.Item.Cells[8].Text = "수료증";

                    }
                    else
                    {
                        e.Item.Cells[0].Text = "open_course_id";
                        e.Item.Cells[1].Text = "";
                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "Course Name";
                        e.Item.Cells[4].Text = "Learning Period";
                        e.Item.Cells[5].Text = "Expired Period\r\nYear(s)";
                        e.Item.Cells[6].Text = "Learning Institution";
                        e.Item.Cells[7].Text = "Status";
                        e.Item.Cells[8].Text = "Certificate";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        protected void grd_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                DataRowView DRV = (DataRowView)e.Item.DataItem;
                Button brnPrint = ((Button)e.Item.FindControl("btnPrint"));
                //이수여부
                /*
                if (Convert.ToString(DRV["pass_flg_cd"].ToString()) == "000001")
                {
                    //한진해운 운항훈련원인 경우만 수료증 발급가능, 한글/영문체크 포함, 교육단체명 변경 시 수정필요
                    if (DRV["EDUCATIONAL_ORG"].ToString() == "한진해운 운항훈련원"
                        || DRV["EDUCATIONAL_ORG"].ToString() == "Hanjin Crew Training Center")
                        brnPrint.Visible = true;
                    else
                        brnPrint.Visible = false;
                }
                else
                {
                    brnPrint.Visible = false;
                }
                */
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
        * Function name : grdbtn_Click
        * Purpose       : 수료증  
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grdbtn_Click(object sender, EventArgs e)
        {
            try
            {
                Button xbtn = (Button)sender;

                string xPath = string.Empty;

                for (int i = 0; i < this.grd.Items.Count; i++)
                {
                    Button xtemp = ((Button)((C1.Web.C1WebGrid.C1GridItem)this.grd.Items[i]).FindControl("btnPrint"));
                    if (xbtn.UniqueID == xtemp.UniqueID)
                    {
                        //print
                        xPath = "/edum/edu_cmp_y_report.aspx?rpt=vp_s_agreement_certificate_report.xml&open_course_id=" + this.grd.Items[i].Cells[0].Text + "&user_id=" + Session["user_id"].ToString();
                        ClientScript.RegisterStartupScript(this.GetType(), "Report", "<script language='javascript'>OpenReport(\"" + xPath + "\");</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

    }
}