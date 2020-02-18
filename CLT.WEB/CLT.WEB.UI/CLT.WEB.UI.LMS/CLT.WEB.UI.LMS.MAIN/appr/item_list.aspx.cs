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
    /// 1. 작업개요 : 역량평가항목 Class
    /// 
    /// 2. 주요기능 : LMS 역량평가항목
    ///				  
    /// 3. Class 명 : item_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    ///
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class item_list : BasePage
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
                                                        { this.btnRetrieve, "I" },
                                                        { this.btnExcel, "I" },
                                                        { this.btnNew, "E" },
                                                        { this.btnUpload, "E" },
                                                        { this.btnDel, "E" }
                                                      });

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (!IsPostBack)
                    {
                        BindGrid();
                    }

                    btnNew.OnClientClick = "javascript:openPopWindow('/appr/item_edit.aspx?app_item_no=new&MenuCode=" + Session["MENU_CODE"] + "','item_edit_win', '750', '951'); return false;";
                }
                else
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "item_list", xScriptMsg);
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
                          평가 항목 바인딩 처리
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

                DataSet xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                             "GetApprItem",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.APPR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                DataTable xDtTotalCnt = xDs.Tables[0];
                DataTable xDt = xDs.Tables[1];

                if (Convert.ToInt32(xDtTotalCnt.Rows[0][0]) < 1)
                {
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageInfo1.TotalRecordCount = 0;
                    this.PageNavigator1.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageInfo1.TotalRecordCount = Convert.ToInt32(xDtTotalCnt.Rows[0][0]);
                    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDtTotalCnt.Rows[0][0]) - 1;
                }
                grdList.DataSource = xDt;
                grdList.DataBind();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /************************************************************
        * Function name : btnRetrieve_Click
        * Purpose       : 평가 항목 조회
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
                xDt.Columns.Add("app_item_no");

                for (int i = 0; i < this.grdList.Items.Count; i++)
                {
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chk_sel")).Checked)
                    {
                        DataRow xRow = xDt.NewRow();
                        xRow["app_item_no"] = this.grdList.DataKeys[i].ToString();
                        xDt.Rows.Add(xRow);
                    }
                }

                string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                       "SetApprItemDelete",
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
        * Purpose       : 평가 항목 Excel 출력
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

                DataTable xDt = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                         "GetApprItem",
                                         LMS_SYSTEM.CAPABILITY,
                                         "CLT.WEB.UI.LMS.APPR", (object)xParams).Tables[1];

                int xColumnCnt = 11;
                string[] xExcelHeader = new string[xColumnCnt];
                if (this.IsSettingKorean())
                {
                    xExcelHeader[0] = "구분";
                    xExcelHeader[1] = "평가대상";
                    xExcelHeader[2] = "No";
                    xExcelHeader[3] = "역량명";
                    xExcelHeader[4] = "역량정의";
                    xExcelHeader[5] = "SEQ";
                    xExcelHeader[6] = "행위사례";
                    xExcelHeader[7] = "기준일자";
                    xExcelHeader[8] = "OJT";
                    xExcelHeader[9] = "LMS";
                    xExcelHeader[10] = "Others";
                }
                else
                {
                    xExcelHeader[0] = "Classification";
                    xExcelHeader[1] = "Evaluee";
                    xExcelHeader[2] = "No";
                    xExcelHeader[3] = "Name of Competency";
                    xExcelHeader[4] = "Definition of Competency";
                    xExcelHeader[5] = "SEQ";
                    xExcelHeader[6] = "Description";
                    xExcelHeader[7] = "Date";
                    xExcelHeader[8] = "OJT";
                    xExcelHeader[9] = "LMS";
                    xExcelHeader[10] = "Others";
                }

                string[] xDtHeader = new string[xColumnCnt];
                xDtHeader[0] = "step_gu";
                xDtHeader[1] = "app_duty_step";
                xDtHeader[2] = "app_item_seq";
                xDtHeader[3] = "app_item_nm";
                xDtHeader[4] = "app_item_desc";
                xDtHeader[5] = "app_case_seq";
                xDtHeader[6] = "app_case_desc";
                xDtHeader[7] = "app_base_dt";
                xDtHeader[8] = "course_ojt";
                xDtHeader[9] = "course_lms";
                xDtHeader[10] = "course_etc";

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
            CheckBox xchk = (CheckBox)sender;

            for (int i = 0; i < this.grdList.Items.Count; i++)
            {
                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chkEdit")).Checked = xchk.Checked;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

        }

        /************************************************************
        * Function name : grdList_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grdList_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[1].Text = "기준일자";
                        e.Item.Cells[2].Text = "구분";
                        e.Item.Cells[3].Text = "선종";
                        e.Item.Cells[4].Text = "평가대상";
                        e.Item.Cells[5].Text = "No";
                        e.Item.Cells[6].Text = "역량명";
                        e.Item.Cells[7].Text = "역량정의";
                        e.Item.Cells[8].Text = "SEQ";
                        e.Item.Cells[9].Text = "행위사례";
                        e.Item.Cells[10].Text = "OJT";
                        e.Item.Cells[11].Text = "LMS";
                        e.Item.Cells[12].Text = "Others";
                    }
                    else
                    {
                        e.Item.Cells[1].Text = "Date";
                        e.Item.Cells[2].Text = "Inquiry";
                        e.Item.Cells[3].Text = "Vessle Type";
                        e.Item.Cells[4].Text = "Duty Step";
                        e.Item.Cells[5].Text = "No";
                        e.Item.Cells[6].Text = "Name of Competency";
                        e.Item.Cells[7].Text = "Definition of Competency";
                        e.Item.Cells[8].Text = "SEQ";
                        e.Item.Cells[9].Text = "Description";
                        e.Item.Cells[10].Text = "OJT";
                        e.Item.Cells[11].Text = "LMS";
                        e.Item.Cells[12].Text = "Others";
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

        protected void btnSend_Click(object sender, EventArgs e)
        {
            //인터페이스
            try
            {
                string[] xParams = new string[2];
                xParams[0] = "";
                xParams[1] = "";
                string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                             "SetAppraisalItemInf",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.APPR", (object)xParams);
                if (xRtn.ToUpper() == "TRUE")
                {
                    //A001: {0}이(가) 저장되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A017",
                                                      new string[] { "역량평가 항목" },
                                                      new string[] { "Competence evaluation items" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                }
                else
                {
                    //A004: {0}이(가) 입력되지 않았습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004",
                                                      new string[] { "역량평가 항목" },
                                                      new string[] { "Competence evaluation items" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }


    }
}