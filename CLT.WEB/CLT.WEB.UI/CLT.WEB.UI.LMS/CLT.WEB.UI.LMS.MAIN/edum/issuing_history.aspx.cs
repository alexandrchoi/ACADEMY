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
using CLT.WEB.UI.COMMON.BASE;
using CLT.WEB.UI.FX.AGENT;
using System.Threading;
using CLT.WEB.UI.FX.UTIL;
using C1.Web.C1WebGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.EDUM
{
    public partial class issuing_history : BasePage
    {
        #region 인터페이스 그룹

        #endregion

        #region 기타 프로시저 그룹 [Core Logic]
        private DataTable GetDtGrdList(string rGubun)
        {
            DataTable xDt = new DataTable();
            try
            {
                string[] xParams = new string[7];
                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = Util.ForbidText(txtSTART_DATE.Text);
                xParams[3] = Util.ForbidText(txtEND_DATE.Text);
                xParams[4] = ddlCourseType.SelectedValue;
                xParams[5] = Util.ForbidText(txtUserNMKor.Text);
                xParams[6] = Util.ForbidText(txtCourseNM.Text);

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                       "GetEduIssuingHistory",
                                       LMS_SYSTEM.EDUMANAGEMENT,
                                       "CLT.WEB.UI.LMS.EDUM",
                                       (object)xParams,
                                       rGubun, Thread.CurrentThread.CurrentCulture);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }
        private void BindGrdList(int rPageIndex, string rGubun)
        {
            try
            {
                DataTable xDt = GetDtGrdList(rGubun);
                
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
                    PageInfo1.CurrentPageIndex = rPageIndex;
                    PageNavigator1.CurrentPageIndex = rPageIndex;
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
        #endregion

        #region 초기화 그룹
        private void InitControl()
        {
            try
            {
                this.txtSTART_DATE.Attributes.Add("onkeyup", "ChkDate(this);");
                this.txtEND_DATE.Attributes.Add("onkeyup", "ChkDate(this);");

                this.txtSTART_DATE.Text = DateFormat(GetFirstDayofMonth());
                this.txtEND_DATE.Text = DateFormat(GetLastDayofMonth());

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

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    BindGrdList(1, "");
                }
                else
                {
                    //return;
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list_wpg", xScriptMsg);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID;

                base.pRender(this.Page, new object[,] { 
                                                        { this.btnRetrieve, "I" }
                                                      });



                if (!IsPostBack)
                {
                    InitControl();
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        #region 화면 컨트롤 이벤트 핸들러 그룹
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrdList(1, "");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void grdList_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                //DataRowView DRV = (DataRowView)e.Item.DataItem;
                //HyperLink hlkUserId = ((HyperLink)e.Item.FindControl("hlkCourseNM"));
                //hlkUserId.NavigateUrl = "javascript:;";
                //hlkUserId.Attributes.Add("onclick", "javascript:GoAppForm('" + DRV["KEYS"].ToString() + "', '" + Convert.ToString(DRV["PAGE_HV"]) + "'); return false;");
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
                this.BindGrdList(e.PageIndex, "");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        protected void grdList_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "번호";
                        e.Item.Cells[1].Text = "교육유형";
                        e.Item.Cells[2].Text = "회사명";
                        e.Item.Cells[3].Text = "과정명";
                        e.Item.Cells[4].Text = "증서번호";
                        e.Item.Cells[5].Text = "성명";
                        e.Item.Cells[6].Text = "영문명";
                        e.Item.Cells[7].Text = "생년월일";
                        e.Item.Cells[8].Text = "교육기간";
                        e.Item.Cells[9].Text = "발급일자";
                        e.Item.Cells[10].Text = "발급사유";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Course Type";
                        e.Item.Cells[2].Text = "Company";
                        e.Item.Cells[3].Text = "Couse Name";
                        e.Item.Cells[4].Text = "Certificate no.";
                        e.Item.Cells[5].Text = "Name";
                        e.Item.Cells[6].Text = "Eng.Name";
                        e.Item.Cells[7].Text = "Birthday";
                        e.Item.Cells[8].Text = "Edu.Period";
                        e.Item.Cells[9].Text = "Issue date";
                        e.Item.Cells[10].Text = "Reason";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        #region 액셀
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable xDt = GetDtGrdList("all");

                int xColumnCnt = 11;
                string[] xExcelHeader = new string[xColumnCnt];
                if (this.IsSettingKorean())
                {
                    xExcelHeader[0] = "번호";
                    xExcelHeader[1] = "교육유형";
                    xExcelHeader[2] = "회사명";
                    xExcelHeader[3] = "과정명";
                    xExcelHeader[4] = "증서번호";
                    xExcelHeader[5] = "성명";
                    xExcelHeader[6] = "영문명";
                    xExcelHeader[7] = "생년월일";
                    xExcelHeader[8] = "교육기간";
                    xExcelHeader[9] = "발급일자";
                    xExcelHeader[10] = "발급사유";
                }
                else
                {
                    xExcelHeader[0] = "No.";
                    xExcelHeader[1] = "Course Type";
                    xExcelHeader[2] = "Company";
                    xExcelHeader[3] = "Couse Name";
                    xExcelHeader[4] = "Certificate no.";
                    xExcelHeader[5] = "Name";
                    xExcelHeader[6] = "Eng.Name";
                    xExcelHeader[7] = "Birthday";
                    xExcelHeader[8] = "Edu.Period";
                    xExcelHeader[9] = "Issue date";
                    xExcelHeader[10] = "Reason";

                }

                string[] xDtHeader = new string[xColumnCnt];
                xDtHeader[0] = "rnum";
                xDtHeader[1] = "course_type_nm";
                xDtHeader[2] = "company_nm";
                xDtHeader[3] = "course_nm";
                xDtHeader[4] = "certificate_no";
                xDtHeader[5] = "user_nm_kor";
                xDtHeader[6] = "user_nm_eng";
                xDtHeader[7] = "birth_date";
                xDtHeader[8] = "course_date";
                xDtHeader[9] = "issue_date";
                xDtHeader[10] = "reason";
                
                this.GetExcelFile(xDt, xExcelHeader, xDtHeader, "0");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
    }
}
