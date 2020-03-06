﻿using System;
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
    public partial class issuing_list : BasePage
    {
        #region 인터페이스 그룹

        #endregion

        #region  사용자함수
        private DataTable GetDtResultList(string rMode)
        {
            DataTable xDt = new DataTable();
            try
            {
                xDt.Columns.Add("start_rnum");
                xDt.Columns.Add("end_rnum");
                xDt.Columns.Add("start_dt");
                xDt.Columns.Add("end_dt");
                xDt.Columns.Add("course_type");
                xDt.Columns.Add("course_nm");
                xDt.Columns.Add("course_seq");
                xDt.Columns.Add("non_approval_cd");
                xDt.Columns.Add("non_pass_cd");

                DataRow xRow = xDt.NewRow();
                if (rMode == "all")
                {
                    xRow["start_rnum"] = "";
                    xRow["end_rnum"] = "";
                }
                else
                {
                    xRow["start_rnum"] = this.PageSize.ToString(); ;
                    xRow["end_rnum"] = this.CurrentPageIndex.ToString();
                }

                xRow["start_dt"] = Util.ForbidText(txtSTART_DATE.Text);
                xRow["end_dt"] = Util.ForbidText(txtEND_DATE.Text);
                xRow["course_type"] = ddlCourseType.SelectedValue;
                xRow["course_nm"] = Util.ForbidText(txtCourseNM.Text);
                //xRow["non_approval_cd"] = ddlNonApp.SelectedValue;
                //xRow["non_pass_cd"] = ddlNonPass.SelectedValue;
                xDt.Rows.Add(xRow);

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                       "GetEduResultNonPassList",
                                       LMS_SYSTEM.MANAGE,
                                       "CLT.WEB.UI.LMS.EDUM.vp_a_edumng_edu_cmp_y_list_wpg",
                                       xDt, Thread.CurrentThread.CurrentCulture);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }
        #endregion

        #region 기타 프로시저 그룹 [Core Logic]
        private DataSet GetDsGrdList(string rGubun)
        {
            DataSet xDs = null;
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

                xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                       "GetEduIssuingList",
                                       LMS_SYSTEM.MANAGE,
                                       "CLT.WEB.UI.LMS.EDUM.vp_a_edumng_approval_list_wpg",
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
                DataRowView DRV = (DataRowView)e.Item.DataItem;
                HyperLink hlkUserId = ((HyperLink)e.Item.FindControl("hlkCourseNM"));
                hlkUserId.NavigateUrl = "javascript:;";
                hlkUserId.Attributes.Add("onclick", "javascript:GoAppForm('" + DRV["KEYS"].ToString() + "', '" + Convert.ToString(DRV["PAGE_HV"]) + "'); return false;");

                DataRowView xItem = (DataRowView)e.Item.DataItem;

                if (e.Item.ItemType == C1ListItemType.Item || e.Item.ItemType == C1ListItemType.AlternatingItem)
                {
                    Label lblType = ((Label)e.Item.FindControl("lblCourseType"));

                    if (xItem["course_type"] != null)
                    {
                        string[] xType = xItem["course_type"].ToString().Split('|');
                        foreach (string xCourseType in xType)
                        {
                            if (string.IsNullOrEmpty(lblType.Text))
                                lblType.Text += GetCourseType(xCourseType);
                            else
                                lblType.Text = lblType.Text + "<BR>" + GetCourseType(xCourseType);
                        }
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
                this.BindGrdList(e.PageIndex, "");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : GetCourseType
        * Purpose       : 개설과정에 대한 교육유형명칭을 가져온다.
        * Input         : void
        * Output        : void
        *************************************************************/
        #region
        public string GetCourseType(string rCode)
        {
            string xResult = string.Empty;
            try
            {
                if (Thread.CurrentThread.CurrentCulture.Name.ToLower() == "ko-kr")
                {
                    if (rCode == "000001") // 자체교육
                        xResult = "자체교육";
                    else if (rCode == "000002")  // 사업주위탁
                        xResult = "사업주위탁";
                    else if (rCode == "000003") // 청년취업아카데미
                        xResult = "청년취업아카데미";
                    else if (rCode == "000004") // 컨소시엄훈련 
                        xResult = "컨소시엄훈련 ";
                }
                else
                {
                    if (rCode == "000001") // 자체교육
                        xResult = "Internal Training";
                    else if (rCode == "000002")  // 사업주위탁
                        xResult = "Commissioned Education";
                    else if (rCode == "000003") // 청년취업아카데미
                        xResult = "Youth Job Academy";
                    else if (rCode == "000004") // 컨소시엄훈련
                        xResult = "Consortium";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xResult;
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
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "과정유형";
                        e.Item.Cells[2].Text = "교육유형";
                        e.Item.Cells[3].Text = "과정명";
                        e.Item.Cells[4].Text = "차수";
                        e.Item.Cells[5].Text = "교육기간";
                        e.Item.Cells[6].Text = "수료인원";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Course Type";
                        e.Item.Cells[2].Text = "Open Course Type";
                        e.Item.Cells[3].Text = "Couse Name";
                        e.Item.Cells[4].Text = "Seq";
                        e.Item.Cells[5].Text = "Period";
                        e.Item.Cells[6].Text = "Completion";
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
                DataTable xDt = GetDtResultList("all");

                int xColumnCnt = 10;
                string[] xExcelHeader = new string[xColumnCnt];
                if (this.IsSettingKorean())
                {
                    xExcelHeader[0] = "사번";
                    xExcelHeader[1] = "직급";
                    xExcelHeader[2] = "성명";
                    xExcelHeader[3] = "과정명";
                    xExcelHeader[4] = "차수";
                    xExcelHeader[5] = "교육기간";
                    xExcelHeader[6] = "교육불가사유";
                    xExcelHeader[7] = "불가사유";
                    xExcelHeader[8] = "미이수사유";
                    xExcelHeader[9] = "비고";
                }
                else
                {
                    xExcelHeader[0] = "ID";
                    xExcelHeader[1] = "Grade";
                    xExcelHeader[2] = "Name";
                    xExcelHeader[3] = "Corse Name";
                    xExcelHeader[4] = "Seq";
                    xExcelHeader[5] = "Period";
                    xExcelHeader[6] = "Absent";
                    xExcelHeader[7] = "Remark";
                    xExcelHeader[8] = "Comments";
                    xExcelHeader[9] = "Remark";
                }

                string[] xDtHeader = new string[xColumnCnt];
                xDtHeader[0] = "user_no";
                xDtHeader[1] = "step_name";
                xDtHeader[2] = "user_nm_kor";
                xDtHeader[3] = "course_nm";
                xDtHeader[4] = "course_seq";
                xDtHeader[5] = "course_dt";
                xDtHeader[6] = "non_approval_nm";
                xDtHeader[7] = "non_approval_remark";
                xDtHeader[8] = "non_pass_nm";
                xDtHeader[9] = "non_pass_remark";

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
