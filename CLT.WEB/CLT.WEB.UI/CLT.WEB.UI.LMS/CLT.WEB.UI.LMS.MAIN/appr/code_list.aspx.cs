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

namespace CLT.WEB.UI.LMS.APPR
{
    /// <summary>
    /// 1. 작업개요 : 역량평가코드 Class
    /// 
    /// 2. 주요기능 : LMS 역량평가코드
    ///				  
    /// 3. Class 명 : code_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    ///
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class code_list : BasePage
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
                base.pRender(this.Page, new object[,] {
                                                        { this.btnNew, "E" },
                                                        { this.btnDel, "D" },
                                                        { this.btnExcel, "I" },
                                                        { this.btnRetrieve, "I" }
                                                      });

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (!IsPostBack)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(Session["USER_GROUP"])) && Convert.ToString(Session["USER_GROUP"]) != "000009")
                            BindGrid();
                    }

                    btnNew.OnClientClick = "javascript:openPopWindow('/appr/code_edit.aspx?GRADE=new&" + Session["MENU_CODE"] + "','code_edit_win', '580', '480'); return false;";
                }
                else
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list_wpg", xScriptMsg);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : BindGrid
        * Purpose       : C1WebGrid 데이터 바인딩을 위한 처리
                          평가 코드 바인딩 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        private void BindGrid()
        {
            try
            {
                string[] xParams = new string[3];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // pageno
                xParams[2] = "";
                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                             "GetApprCode",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.APPR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                grdList.DataSource = xDt;
                grdList.DataBind();

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
                    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]) - 1;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /************************************************************
        * Function name : btnRetrieve_Click
        * Purpose       : 평가 코드 조회
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
        * Function name : btnDel_Click
        * Purpose       : 평가 코드 삭제
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable xDt = new DataTable();
                xDt.Columns.Add("grade");

                for (int i = 0; i < this.grdList.Items.Count; i++)
                {
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chk_sel")).Checked)
                    {
                        DataRow xRow = xDt.NewRow();
                        xRow["GRADE"] = this.grdList.DataKeys[i].ToString();
                        xDt.Rows.Add(xRow);
                    }
                }

                string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                       "SetApprCodeDelete",
                                       LMS_SYSTEM.CAPABILITY,
                                       "CLT.WEB.UI.LMS.CURR", xDt);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : btnExcel_Click
        * Purpose       : 평가 코드 Excel 출력
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string[] xParams = new string[3];
                xParams[0] = null;
                xParams[1] = "";
                xParams[2] = "all";

                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                             "GetApprCode",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.APPR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                int xColumnCnt = 4;
                string[] xExcelHeader = new string[xColumnCnt];
                if (this.IsSettingKorean())
                {
                    xExcelHeader[0] = "등급";
                    xExcelHeader[1] = "등급내용";
                    xExcelHeader[2] = "점수";
                    xExcelHeader[3] = "설명";
                }
                else
                {
                    xExcelHeader[0] = "Grade";
                    xExcelHeader[1] = "Grade Name";
                    xExcelHeader[2] = "Score";
                    xExcelHeader[3] = "Description";
                }

                string[] xDtHeader = new string[xColumnCnt];
                xDtHeader[0] = "grade";
                xDtHeader[1] = "grade_nm";
                xDtHeader[2] = "score";
                xDtHeader[3] = "grade_desc";

                this.GetExcelFile(xDt, xExcelHeader, xDtHeader, "1");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : grdList_ItemDataBound
        * Purpose       : C1WebGrid의 DataBound 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grdList_ItemDataBound(object sender, C1ItemEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : OnCheckedChanged
        * Purpose       : C1WebGrid의 Header 체크박스 체크 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void chkHeader_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox xchk = (CheckBox)sender;

                for (int i = 0; i < this.grdList.Items.Count; i++)
                {
                    ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chkEdit")).Checked = xchk.Checked;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : grdList_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grd_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                //e.Item.Cells[0].Visible = false;

                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[1].Text = "No.";
                        e.Item.Cells[2].Text = "등급";
                        e.Item.Cells[3].Text = "평가내용";
                        e.Item.Cells[4].Text = "점수";
                        e.Item.Cells[5].Text = "설명";
                    }
                    else
                    {
                        e.Item.Cells[1].Text = "No.";
                        e.Item.Cells[2].Text = "Grade";
                        e.Item.Cells[3].Text = "Grade Name";
                        e.Item.Cells[4].Text = "Score";
                        e.Item.Cells[5].Text = "Description";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        
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