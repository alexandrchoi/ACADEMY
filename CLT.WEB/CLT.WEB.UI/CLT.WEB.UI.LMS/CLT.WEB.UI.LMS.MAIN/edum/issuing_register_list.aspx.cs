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
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.FX.AGENT;
using C1.Web.C1WebGrid;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.EDUM
{
    /// <summary>
    /// 1. 작업개요 : 
    /// 
    /// 2. 주요기능 : 
    ///				  
    /// 3. Class 명 : issuing_register_list
    /// 
    /// 4. 작 업 자 : 
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_m_main_md
    ///        * Comment 
    ///          * Comment 
    ///          수료증 발급시 교육기관이 한진해운 운항훈련원 인 경우만 발급
    ///          Excel 출력 시 사번, 주민번호 뒤에 공백 제거
    /// </summary>
    public partial class issuing_register_list : BasePage
    {
        #region 인터페이스 그룹

        #endregion
        string xMenuGubun = string.Empty;    //사용자 그룹에 대한 화면 Menu 권한

        #region 기타 프로시저 그룹 [Core Logic]
        private DataSet GetDsGrdList(string rGubun)
        {
            DataSet xDs = null;
            try
            {
                string[] xParams = new string[7];
                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = Util.ForbidText(txtUserNMKor.Text);
                xParams[3] = Util.ForbidText(txtUserId.Text);
                xParams[4] = ddlDutyStep.SelectedValue;
                xParams[5] = Util.ForbidText(txtPersonalNo.Text);
                xParams[6] = xMenuGubun;

                xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                       "GetEduTrainigRecordList",
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
                string[] xParams = new string[2];
                string xSql = string.Empty;

                // 직급코드 Dutystep
                xParams[1] = "Y";
                DataTable xDtDutyStep = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                 "GetDutystepCodeInfo",
                                 LMS_SYSTEM.MANAGE,
                                 "CLT.WEB.UI.LMS.MANAGE"
                                 , (object)xParams
                                 , Thread.CurrentThread.CurrentCulture
                                 );
                WebControlHelper.SetDropDownList(this.ddlDutyStep, xDtDutyStep, "step_name", "duty_step", WebControlHelper.ComboType.NullAble);

                this.SetGridClear(this.grdList, this.PageNavigator1, this.PageInfo1);

                //btnDel.OnClientClick = "return confirm('" + MsgInfo.GetMsg("A119", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture) + @"');";
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
                                                        { this.btnRetrieve, "I" }, 
                                                        { this.btnNew, "I" }, 
                                                        { this.btnExcel, "I" }, 
                                                        { this.btnDel, "E" }
                                                  });


                DataTable xMenu = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                        "GetMenuAuthority",
                                        LMS_SYSTEM.EDUMANAGEMENT,
                                        "CLT.WEB.UI.LMS.EDUM",
                                        Convert.ToString(Session["MENU_CODE"]),
                                        Convert.ToString(Session["USER_GROUP"]));

                foreach (DataRow xMenuDr in xMenu.Rows)
                {
                    xMenuGubun = xMenu.Rows[0]["ADMIN_YN"].ToString();
                }

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (!IsPostBack)
                    {
                        InitControl();
                    }
                }
                else
                {
                    //return;
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "issuing_register_list", xScriptMsg);
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
                DataTable xDt = GetDsGrdList("all").Tables[1];

                int xColumnCnt = 4;
                string[] xExcelHeader = new string[xColumnCnt];
                if (this.IsSettingKorean())
                {
                    //xExcelHeader[0] = "사번";
                    //xExcelHeader[1] = "이름";
                    //xExcelHeader[2] = "과정명";
                    //xExcelHeader[3] = "교육기관";
                    //xExcelHeader[4] = "교육시작일";
                    //xExcelHeader[5] = "교육종료일";
                    
                    xExcelHeader[0] = "과정명";
                    xExcelHeader[1] = "교육기관";
                    xExcelHeader[2] = "교육시작일";
                    xExcelHeader[3] = "교육종료일";
                }
                else
                {
                    //xExcelHeader[0] = "ID";
                    //xExcelHeader[1] = "Name";
                    //xExcelHeader[2] = "Course Name";
                    //xExcelHeader[3] = "Learning Institution";
                    //xExcelHeader[4] = "Date of Start";
                    //xExcelHeader[5] = "Date of End";
                    
                    xExcelHeader[0] = "Course Name";
                    xExcelHeader[1] = "Learning Institution";
                    xExcelHeader[2] = "Date of Start";
                    xExcelHeader[3] = "Date of End";
                }

                string[] xDtHeader = new string[xColumnCnt];
                //xDtHeader[0] = "USER_ID";
                //xDtHeader[1] = "USER_NM_KOR";
                //xDtHeader[2] = "COURSE_NM";
                //xDtHeader[3] = "LEARNING_INSTITUTION";
                //xDtHeader[4] = "COURSE_BEGIN_DT";
                //xDtHeader[5] = "COURSE_END_DT";
                xDtHeader[0] = "COURSE_NM";
                xDtHeader[1] = "LEARNING_INSTITUTION";
                xDtHeader[2] = "COURSE_BEGIN_DT";
                xDtHeader[3] = "COURSE_END_DT";

                this.GetExcelFile(xDt, xExcelHeader, xDtHeader, "1");
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
                        e.Item.Cells[2].Text = "과정명";
                        e.Item.Cells[3].Text = "교육기관";
                        e.Item.Cells[4].Text = "교육시작일";
                        e.Item.Cells[5].Text = "교육종료일";
                    }
                    else
                    {
                        e.Item.Cells[1].Text = "No.";
                        e.Item.Cells[2].Text = "Course Name";
                        e.Item.Cells[3].Text = "Learning Institution";
                        e.Item.Cells[4].Text = "Date of Start";
                        e.Item.Cells[5].Text = "Date of End";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void grdList_ItemDataBound(object sender, C1ItemEventArgs e)
        {
            try
            {
                DataRowView DRV = (DataRowView)e.Item.DataItem;
                HyperLink hlkCourseNM = ((HyperLink)e.Item.FindControl("hlkCourseNM"));
                hlkCourseNM.Text = DRV["COURSE_NM"].ToString();
                hlkCourseNM.NavigateUrl = "javascript:;";
                hlkCourseNM.Attributes.Add("onclick", "javascript:GoAppForm('" + DRV["KEYS"].ToString() + "^" + Server.UrlEncode(txtUserNMKor.Text) + "'); return false;");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            try {

                int xCntSel = 0;
                DataTable xDt = new DataTable();
                xDt.Columns.Add("user_id");
                xDt.Columns.Add("record_id");
                xDt.Columns.Add("course_id");

                for (int i = 0; i < this.grdList.Items.Count; i++)
                {   
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chk_sel")).Checked)
                    {
                        xCntSel++;
                        DataRow xRow = xDt.NewRow();
                        xRow["user_id"] = Util.Split(grdList.DataKeys[i].ToString(), "^", 4)[0];
                        xRow["record_id"] = Util.Split(grdList.DataKeys[i].ToString(), "^", 4)[1];
                        xRow["course_id"] = Util.Split(grdList.DataKeys[i].ToString(), "^", 4)[2];
                        xDt.Rows.Add(xRow);
                    }
                }

                if (xCntSel > 0)
                {
                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                           "GetEduTrainigRecordDelete",
                                           LMS_SYSTEM.EDUMANAGEMENT,
                                           "CLT.WEB.UI.LMS.EDUM", xDt);

                    if (xRtn.ToUpper() == "TRUE")
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A002", new string[] { "교육이력" }, new string[] { "Register Training Record" }, Thread.CurrentThread.CurrentCulture));
                        this.BindGrdList(1, "");
                    }
                    else
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                    }
                }
                else
                {
                    ScriptHelper.Page_Alert(this.Page, CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A047", new string[] { "" }, new string[] { "" }, System.Threading.Thread.CurrentThread.CurrentCulture));
                }

            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
    }
}
