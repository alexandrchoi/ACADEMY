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

namespace CLT.WEB.UI.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : 교육대상자선발 조회 Class
    /// 
    /// 2. 주요기능 : LMS 교육대상자선발 조회 화면
    ///				  
    /// 3. Class 명 : select_user_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class select_user_result_list : BasePage
    {
        #region 초기화 그룹
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "select_user_result_list", xScriptMsg);

                    return;
                }
                
                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (!IsPostBack)
                    {
                        InitControl();

                        //교육대상자
                        this.SetGridClear(this.grdList, this.PageNavigator1, this.PageInfo1);
                        
                        InitialPage();

                        this.BindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        private void InitControl()
        {
            try
            {

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        private void InitialPage()
        {
            this.PageInfo1.TotalRecordCount = 0;
            this.PageInfo1.PageSize = 100;
            this.PageNavigator1.TotalRecordCount = 0;
        }

        //교육대상자 New
        private void BindGrid()
        {
            try
            {
                DataTable xDt = null;

                string[] xParams = new string[7];
                xParams[0] = "100";
                xParams[1] = PageNavigator1.CurrentPageIndex.ToString();
                string[] rSearch = Util.Split(Util.Request("search"), "^", 2);
                xParams[2] = rSearch[0];
                xParams[3] = rSearch[1];

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                             "GetEduConfirmList",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);


                this.grdList.DataSource = xDt;
                this.grdList.DataBind();

                if (xDt.Rows.Count < 1)
                {
                    InitialPage();
                }
                else
                {
                    this.PageInfo1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    this.PageInfo1.PageSize = 100;
                    this.PageNavigator1.PageSize = 100;
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

        #region 화면 컨트롤 이벤트 핸들러 그룹
        protected void grdList_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {

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
                //this.BindGrdList("");
                //ScriptHelper.ScriptStartup(this, "select_user_list", "<script>ChangeLayer('1');</script>");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        #region protected void grdList_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        protected void grdList_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "현재 부서";
                        e.Item.Cells[2].Text = "직급";
                        e.Item.Cells[3].Text = "사번";
                        e.Item.Cells[4].Text = "성명";
                        e.Item.Cells[5].Text = "최종선박하선일";
                        e.Item.Cells[6].Text = "교육신청일시";
                        e.Item.Cells[7].Text = "이전이수일";



                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Currently<br/>the Department";
                        e.Item.Cells[2].Text = "Grade";
                        e.Item.Cells[3].Text = "ID";
                        e.Item.Cells[4].Text = "Name";
                        e.Item.Cells[5].Text = "Date of<br/>Disembarkation";
                        e.Item.Cells[6].Text = "Date of<br/>Application";
                        e.Item.Cells[7].Text = "Date of<br/>Completion";


                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

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

        #region protected void btnExcel_Click(object sender, EventArgs e)
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable xDt = GetDsGrdList("all").Tables[1];
                DataTable xDt = null;

                string[] xParams = new string[7];
                xParams[0] = "100000";
                xParams[1] = PageNavigator1.CurrentPageIndex.ToString();
                string[] rSearch = Util.Split(Util.Request("search"), "^", 2);
                xParams[2] = rSearch[0];
                xParams[3] = rSearch[1];

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                             "GetEduConfirmList",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);

                int xColumnCnt = 10;
                string[] xExcelHeader = new string[xColumnCnt];
                if (this.IsSettingKorean())
                {
                    xExcelHeader[0] = "현재부서";
                    xExcelHeader[1] = "직급";
                    xExcelHeader[2] = "사번";
                    xExcelHeader[3] = "성명";
                    xExcelHeader[4] = "최종선박하선일";
                    xExcelHeader[5] = "최종하선선박";
                    xExcelHeader[6] = "소속";
                    xExcelHeader[7] = "주민등록번호";
                    xExcelHeader[8] = "주소";
                    xExcelHeader[9] = "연락처";
                }
                else
                {
                    xExcelHeader[0] = "Currently the Department";
                    xExcelHeader[1] = "Grade";
                    xExcelHeader[2] = "ID";
                    xExcelHeader[3] = "Name";
                    xExcelHeader[4] = "Date of Disembarkation";
                    xExcelHeader[5] = "Vessel of Disembarkation";
                    xExcelHeader[6] = "Company Name";
                    xExcelHeader[7] = "Registration No";
                    xExcelHeader[8] = "Address";
                    xExcelHeader[9] = "Staff Mobile";
                    //xExcelHeader[8] = "Tel";
                }

                string[] xDtHeader = new string[xColumnCnt];

                if (this.IsSettingKorean())
                {
                    xDtHeader[0] = "DEPT_NAME";
                    xDtHeader[1] = "STEP_NAME";
                    xDtHeader[2] = "USER_ID";
                    xDtHeader[3] = "USER_NM_KOR";
                    xDtHeader[4] = "ORD_FDATE";
                    xDtHeader[5] = "B_DEPT_NAME";
                    xDtHeader[6] = "COMPANY_NM";
                    xDtHeader[7] = "PERSONAL_NO";
                    xDtHeader[8] = "USER_ADDR";
                    xDtHeader[9] = "MOBILE_PHONE";
                }
                else
                {
                    xDtHeader[0] = "DEPT_NAME";
                    xDtHeader[1] = "STEP_NAME";
                    xDtHeader[2] = "USER_ID";
                    xDtHeader[3] = "USER_NM_KOR";
                    xDtHeader[4] = "ORD_FDATE";
                    xDtHeader[5] = "B_DEPT_NAME";
                    xDtHeader[6] = "COMPANY_NM";
                    xDtHeader[7] = "PERSONAL_NO";
                    xDtHeader[8] = "USER_ADDR";
                    xDtHeader[9] = "MOBILE_PHONE";
                }
                this.GetExcelFile(xDt, xExcelHeader, xDtHeader, "1");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
    }
}
