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
    public partial class pass_list : BasePage
    {
        #region 인터페이스 그룹

        #endregion

        #region 기타 프로시저 그룹 [Core Logic]
        private DataSet GetDsGrdList(string rGubun)
        {
            DataSet xDs = null;
            try
            {
                string[] xParams = new string[6];
                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = Util.ForbidText(txtSTART_DATE.Text);
                xParams[3] = Util.ForbidText(txtEND_DATE.Text);
                xParams[4] = ddlCourseType.SelectedValue;
                xParams[5] = Util.ForbidText(txtCourseNM.Text);

                xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                       "GetEduPassList",
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
            return xDs;
        }
        private void GetExcel()
        {
            try
            {
                string[] xParams = new string[6];

                DataTable xDt = new DataTable();
                xDt.Columns.Add("open_course_id");
                int chkCnt = 0;
                for (int i = 0; i < this.grdList.Items.Count; i++)
                {
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chk_sel")).Checked)
                    {
                        chkCnt++;
                        DataRow xRow = xDt.NewRow();
                        //O.COURSE_ID ||'^'|| O.OPEN_COURSE_ID 
                        xRow["open_course_id"] = Util.Split(grdList.DataKeys[i].ToString(), "^", 2)[1];

                        xDt.Rows.Add(xRow);
                    }
                }

                if (chkCnt > 0)
                {
                    DataSet xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                                   "GetEduApprovalExcel",
                                                   LMS_SYSTEM.EDUMANAGEMENT,
                                                   "CLT.WEB.UI.LMS.EDUM",
                                                   (object)xParams,
                                                   xDt);
                    string xHtml = "";
                    int xEduCnt = 0;
                    xHtml = "<table border='1'>";
                    xHtml += "<tr><td colspan='8' style='text-align:center;font-size:20px; font-weight:bold;height:60px;'>" + txtSTART_DATE.Text.Substring(0, 7) + " 교육이수자 명단</td></tr>";
                    for (int i = 0; i < xDs.Tables.Count; i++)
                    {
                        if (xDs.Tables[i].Rows.Count > 0)
                        {
                            xHtml += "<tr><td colspan='8'>&nbsp;교육과정 : " + Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_NM"]) + "</td></tr>";
                            xHtml += "<tr><td colspan='8'>&nbsp;교육기간 : " + Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_BEGIN_DT"]) + "(" + Util.GetDayOfWeek(Convert.ToDateTime(Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_BEGIN_DT"]) + " 00:00:00")) + ")~" + Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_END_DT"]) + "(" + Util.GetDayOfWeek(Convert.ToDateTime(Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_END_DT"]) + " 00:00:00")) + ")/ " + Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_DAY"]) + " 일간/ " + Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_TYPE_NM"]) + "/ " + Convert.ToString(xDs.Tables[i].Rows.Count) + "명</td></tr>";
                            xHtml += "<tr style='text-align:center;font-weight:bold;'><td style='background-color:#ddd;'>번호</td><td style='background-color:#ddd;'>직급</td><td style='background-color:#ddd;'>성명</td><td style='background-color:#ddd;'>소속</td><td style='background-color:#ddd;'>사번</td><td style='background-color:#ddd;'>E-MAIL</td><td style='background-color:#ddd;'>거주지</td><td style='background-color:#ddd;'>연락처</td></tr>";
                            for (int j = 0; j < xDs.Tables[i].Rows.Count; j++)
                            {
                                xEduCnt++;
                                DataRow xRow = xDs.Tables[i].Rows[j];
                                xHtml += "<tr><td style='text-align:center'>" + (j + 1).ToString() + "</td><td style='text-align:center'>" + 
                                    Convert.ToString(xRow["STEP_NAME"]) + "</td><td style='text-align:center'>" + 
                                    Convert.ToString(xRow["USER_NM_KOR"]) + "</td><td style='text-align:center'>" + 
                                    Convert.ToString(xRow["COMPANY_NM"]) + "</td><td style='text-align:center'>&nbsp;" + 
                                    Convert.ToString(xRow["USER_ID"]) + "&nbsp;</td><td style='text-align:center'>&nbsp;" +
                                    //Convert.ToString(xRow["PERSONAL_NO"]) + "&nbsp;</td><td>" +
                                    Convert.ToString(xRow["EMAIL_ID"]) + "&nbsp;</td><td>" +
                                    Convert.ToString(xRow["USER_ADDR"]) + "</td><td style='text-align:center'>" + Convert.ToString(xRow["MOBILE_PHONE"]) + "</td></tr>";
                            }
                            xHtml += "<tr><td colspan='8' style='height:50px;'></td></tr>";
                        }
                    }
                    xHtml += "</table>";


                    if (xEduCnt == 0)
                        ScriptHelper.Page_Alert(this, CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A062", new string[] { "자료" }, new string[] { "Data" }, System.Threading.Thread.CurrentThread.CurrentCulture));
                    else
                        this.GetExcelFileHtml(xHtml);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        private void BindGrdList(int rPageIndex, string rGubun)
        {
            try
            {
                DataSet xDs = GetDsGrdList(rGubun);
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
                base.pRender(this.Page, new object[,] { 
                                                        { this.btnRetrieve, "I" }, 
                                                        { this.btnExcel, "I" }
                                                      });

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
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
                                                 LMS_SYSTEM.EDUMANAGEMENT,
                                                 "CLT.WEB.UI.LMS.EDUM", (object)xParams, Thread.CurrentThread.CurrentCulture);
                    WebControlHelper.SetDropDownList(this.ddlCourseType, xDt);

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
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                GetExcel();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
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
                DataRowView DRV = (DataRowView)e.Item.DataItem;
                HyperLink hlkUserId = ((HyperLink)e.Item.FindControl("hlkCourseNM"));
                hlkUserId.NavigateUrl = "javascript:;";
                hlkUserId.Attributes.Add("onclick", "javascript:GoAppForm('" + DRV["KEYS"].ToString() + "'); return false;");
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
                        e.Item.Cells[1].Text = "No.";
                        e.Item.Cells[2].Text = "교육유형";
                        e.Item.Cells[3].Text = "과정명";
                        e.Item.Cells[4].Text = "교육기간";
                        e.Item.Cells[5].Text = "교육총원";
                        e.Item.Cells[6].Text = "수료총원";
                        e.Item.Cells[7].Text = "미이수";
                    }
                    else
                    {   
                        e.Item.Cells[1].Text = "No.";
                        e.Item.Cells[2].Text = "Course Type";
                        e.Item.Cells[3].Text = "Couse Name";
                        e.Item.Cells[4].Text = "Period";
                        e.Item.Cells[5].Text = "Attendance";
                        e.Item.Cells[6].Text = "Completion";
                        e.Item.Cells[7].Text = "InCompletion";
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
