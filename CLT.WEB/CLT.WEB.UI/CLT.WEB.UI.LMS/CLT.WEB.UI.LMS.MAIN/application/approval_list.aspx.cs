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
    /// 1. 작업개요 : 수강신청/승인 관리 Class
    /// 
    /// 2. 주요기능 : LMS 수강신청/승인 관리 화면
    ///				  
    /// 3. Class 명 : approval_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.01
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class approval_list : BasePage
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
                                       "GetEduApprovalList",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION",
                                       xParams,
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
                    int xEduCnt = 0;
                    string xHtml = "";
                    DataSet xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                                   "GetEduApprovalExcel",
                                                   LMS_SYSTEM.APPLICATION,
                                                   "CLT.WEB.UI.LMS.APPLICATION",
                                                   (object)xParams,
                                                   xDt);

                    //for (int k = 0; k < xDs.Tables.Count; k++)
                    //{
                    //if (xDs.Tables[k].Rows.Count > 0)
                    //{
                    xHtml = "<table border='1'>";
                    xHtml += "<tr><td colspan='8' style='text-align:center;font-size:20px; font-weight:bold;height:60px;'>" + txtSTART_DATE.Text.Substring(0, 7) + " 교육대상자 명단</td></tr>";
                    for (int i = 0; i < xDs.Tables.Count; i++)
                    {
                        if (xDs.Tables[i].Rows.Count > 0)
                        {
                            xHtml += "<tr><td colspan='8'>&nbsp;교육과정 : " + Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_NM"]) + "</td></tr>";
                            xHtml += "<tr><td colspan='8'>&nbsp;교육기간 : " + Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_BEGIN_DT"]) + "(" + Util.GetDayOfWeek(Convert.ToDateTime(Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_BEGIN_DT"]) + " 00:00:00")) + ")~" + Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_END_DT"]) + "(" + Util.GetDayOfWeek(Convert.ToDateTime(Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_END_DT"]) + " 00:00:00")) + ")/ " + Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_DAY"]) + " 일간/ " + Convert.ToString(xDs.Tables[i].Rows[0]["COURSE_TYPE_NM"]) + "/ " + Convert.ToString(xDs.Tables[i].Rows.Count) + "명</td></tr>";
                            xHtml += "<tr style='text-align:center;font-weight:bold;'><td style='background-color:#ddd;'>번호</td><td style='background-color:#ddd;'>직급</td><td style='background-color:#ddd;'>성명</td><td style='background-color:#ddd;'>소속</td><td style='background-color:#ddd;'>사번</td><td style='background-color:#ddd;'>주민등록번호</td><td style='background-color:#ddd;'>거주지</td><td style='background-color:#ddd;'>연락처</td></tr>";
                            for (int j = 0; j < xDs.Tables[i].Rows.Count; j++)
                            {
                                xEduCnt++;
                                DataRow xRow = xDs.Tables[i].Rows[j];
                                xHtml += "<tr><td style='text-align:center'>" + (j + 1).ToString() + "</td><td style='text-align:center'>" + Convert.ToString(xRow["STEP_NAME"]) + "</td><td style='text-align:center'>" + Convert.ToString(xRow["USER_NM_KOR"]) + "</td><td style='text-align:center'>" + Convert.ToString(xRow["COMPANY_NM"]) + "</td><td style='text-align:center'>&nbsp;" + Convert.ToString(xRow["USER_ID"]) + "&nbsp;</td><td style='text-align:center'>&nbsp;" + Convert.ToString(xRow["PERSONAL_NO"]) + "&nbsp;</td><td>" + Convert.ToString(xRow["USER_ADDR"]) + "</td><td style='text-align:center'>" + Convert.ToString(xRow["MOBILE_PHONE"]) + "</td></tr>";
                            }
                            xHtml += "<tr><td colspan='8' style='height:50px;'></td></tr>";
                        }
                    }
                    xHtml += "</table>";
                    //}    
                    //}

                    if (xEduCnt == 0)
                        ScriptHelper.Page_Alert(this, CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A062", new string[] { "자료" }, new string[] { "Data" }, System.Threading.Thread.CurrentThread.CurrentCulture));
                    else
                        this.GetExcelFileHtml(xHtml);
                }
                else
                {
                    ScriptHelper.Page_Alert(this, CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A047", new string[] { "" }, new string[] { "" }, System.Threading.Thread.CurrentThread.CurrentCulture));
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
                                                 LMS_SYSTEM.APPLICATION,
                                                 "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);
                    WebControlHelper.SetDropDownList(this.ddlCourseType, xDt);

                    BindGrdList(1, "");
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
                    if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                    {
                        //return;
                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list_wpg", xScriptMsg);
                    }
                    else
                    {
                        InitControl();
                    }
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
                        e.Item.Cells[5].Text = "차수";
                        e.Item.Cells[6].Text = "신청총원";
                        e.Item.Cells[7].Text = "승인";
                        e.Item.Cells[8].Text = "미승인";
                        e.Item.Cells[9].Text = "교육입과";
                        e.Item.Cells[10].Text = "실시신고";
                    }
                    else
                    {
                        e.Item.Cells[1].Text = "No.";
                        e.Item.Cells[2].Text = "Course Type";
                        e.Item.Cells[3].Text = "Course Name";
                        e.Item.Cells[4].Text = "Period";
                        e.Item.Cells[5].Text = "SEQ";
                        e.Item.Cells[6].Text = "Total";
                        e.Item.Cells[7].Text = "Approval";
                        e.Item.Cells[8].Text = "Reject";
                        e.Item.Cells[9].Text = "Attendance";
                        e.Item.Cells[10].Text = "Report";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void grdList_ItemCommand(object sender, C1CommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Excel")
                {
                    //string[] xParams = new string[6];

                    //DataTable xDt = new DataTable();
                    //xDt.Columns.Add("open_course_id");

                    ////O.COURSE_ID ||'^'|| O.OPEN_COURSE_ID 
                    //DataRow xRow = xDt.NewRow();
                    //xRow["open_course_id"] = Util.Split(grdList.DataKeys[e.Item.ItemIndex].ToString(), "^", 2)[1];
                    //xDt.Rows.Add(xRow);

                    //int xEduCnt = 0;
                    //string xHtml = "";
                    //DataSet xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                    //                               "GetEduApprovalExcel",
                    //                               LMS_SYSTEM.APPLICATION,
                    //                               "CLT.WEB.UI.APPLICATION",
                    //                               (object)xParams,
                    //                               xDt);

                    //xHtml = "<table border='1'>";
                    //for (int i = 0; i < xDs.Tables.Count; i++)
                    //{
                    //    if (xDs.Tables[i].Rows.Count > 0)
                    //    {
                    //        xHtml += "<tr style='text-align:center;font-weight:bold;'><td style='background-color:#ddd;'>주민등록번호</td><td style='background-color:#ddd;'>훈련생구분</td><td style='background-color:#ddd;'>성명</td><td style='background-color:#ddd;'>비용수급사업장번호</td><td style='background-color:#ddd;'>최종학력</td><td style='background-color:#ddd;'>비정규직구분</td><td style='background-color:#ddd;'>기숙사사용여부</td><td style='background-color:#ddd;'>식비사용여부</td><td style='background-color:#ddd;'>훈련구분</td><td style='background-color:#ddd;'>대체인력</td></tr>";
                    //        for (int j = 0; j < xDs.Tables[i].Rows.Count; j++)
                    //        {
                    //            xEduCnt++;
                    //            DataRow xRowTable = xDs.Tables[i].Rows[j];
                    //            xHtml += "<tr><td style='text-align:center'>" + Convert.ToString(xRowTable["PERSONAL_NO"]) + "</td><td style='text-align:center'>" + Convert.ToString(xRowTable["T_TRAINEE_CLASS"]) + "</td><td style='text-align:center'>" + Convert.ToString(xRowTable["USER_NM_KOR"]) + "</td><td style='text-align:center'>" + Convert.ToString(xRowTable["TAX_NO"]) + "</td><td style='text-align:center'>&nbsp;1&nbsp;</td><td style='text-align:center'>&nbsp;" + Convert.ToString(xRowTable["C_TRAINEE_CLASS"]) + "&nbsp;</td><td style='text-align:center'>N</td><td style='text-align:center'>Y</td><td>" + Convert.ToString(xRowTable["COURSE_TYPE"]) + "</td><td></td></tr>";
                    //        }
                    //    }
                    //}
                    //xHtml += "</table>";

                    //if (xEduCnt == 0)
                    //    ScriptHelper.Page_Alert(this, CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A062", new string[] { "자료" }, new string[] { "Data" }, System.Threading.Thread.CurrentThread.CurrentCulture));
                    //else
                    //    this.GetExcelFileHtml(xHtml);

                    string[] xParams = new string[6];
                    DataTable xDt = new DataTable();
                    xDt.Columns.Add("open_course_id");

                    //O.COURSE_ID ||'^'|| O.OPEN_COURSE_ID 
                    DataRow xRow = xDt.NewRow();
                    xRow["open_course_id"] = Util.Split(grdList.DataKeys[e.Item.ItemIndex].ToString(), "^", 2)[1];
                    xDt.Rows.Add(xRow);

                    DataSet xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                                   "GetEduApprovalExcel",
                                                   LMS_SYSTEM.APPLICATION,
                                                   "CLT.WEB.UI.LMS.APPLICATION",
                                                   (object)xParams,
                                                   xDt);
                    DataTable xDtReturn = xDs.Tables[0];

                    int xColumnCnt = 10;
                    string[] xExcelHeader = new string[xColumnCnt];
                    if (this.IsSettingKorean())
                    {
                        xExcelHeader[0] = "주민등록번호";
                        xExcelHeader[1] = "훈련생구분";
                        xExcelHeader[2] = "성명";
                        xExcelHeader[3] = "비용수급사업장번호";
                        xExcelHeader[4] = "최종학력";
                        xExcelHeader[5] = "비정규직구분";
                        xExcelHeader[6] = "기숙사사용여부";
                        xExcelHeader[7] = "식비사용여부";
                        xExcelHeader[8] = "훈련구분";
                        xExcelHeader[9] = "대체인력";
                    }

                    string[] xDtHeader = new string[xColumnCnt];
                    xDtHeader[0] = "PERSONAL_NO";
                    xDtHeader[1] = "T_TRAINEE_CLASS";
                    xDtHeader[2] = "USER_NM_KOR";
                    xDtHeader[3] = "EMPOLY_INS_NO";
                    xDtHeader[4] = "LAST_SCHOOL";
                    xDtHeader[5] = "C_TRAINEE_CLASS";
                    xDtHeader[6] = "IS_DORMITORY";
                    xDtHeader[7] = "IS_MEALS";
                    xDtHeader[8] = "COURSE_TYPE";
                    xDtHeader[9] = "IS_SUBSTITUTE";

                    this.GetExcelFile(xDtReturn, xExcelHeader, xDtHeader, "0");
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

    }
}