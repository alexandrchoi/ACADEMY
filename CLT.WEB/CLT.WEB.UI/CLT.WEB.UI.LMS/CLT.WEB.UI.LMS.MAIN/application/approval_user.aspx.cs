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
    /// 1. 작업개요 : 교육접수 조회 Class
    /// 
    /// 2. 주요기능 : LMS 교육접수 조회 화면
    ///				  
    /// 3. Class 명 : approval_user
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class approval_user : BasePage
    {
        private DataTable iDtNonApproval;
        private DataTable iDtEmployedState;
        //O.COURSE_ID ||'^'|| O.OPEN_COURSE_ID
        public string iSearch { get { return Util.Request("search"); } }

        #region 인터페이스 그룹

        #endregion

        #region 기타 프로시저 그룹 [Core Logic]
        private DataSet GetDsGrdList(string rGubun)
        {
            this.PageSize = 10;
            DataSet xDs = null;
            try
            {
                string[] xParams = new string[6];
                xParams[0] = "10";// this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = iSearch;
                xParams[3] = ddlApprovalFlg.SelectedValue;
                string[] rSearch = Util.Split(iSearch, "^", 2);
                xParams[4] = rSearch[0];
                xParams[5] = rSearch[1];

                xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                       "GetEduApprovalUserList",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION",
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
                //교육불가사유코드(0011)
                if (Util.IsNullOrEmptyObject(iDtNonApproval))
                {
                    string[] xParams = new string[1];
                    string xSql = string.Empty;

                    xParams = new string[2];
                    xParams[0] = "0011";
                    xParams[1] = "Y";
                    iDtNonApproval = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                 "GetCommonCodeInfo",
                                                 LMS_SYSTEM.APPLICATION,
                                                 "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);
                }

                // 훈련생구분
                if (Util.IsNullOrEmptyObject(iDtEmployedState))
                {
                    string[] xParams = new string[1];
                    string xSql = string.Empty;

                    xParams = new string[2];
                    xParams[0] = "0062";
                    xParams[1] = "Y";
                    iDtEmployedState = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                 "GetCommonCodeInfo",
                                                 LMS_SYSTEM.APPLICATION,
                                                 "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);
                }

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
                    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDtTotalCnt.Rows[0][0]);
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
                                                        { this.btnAdd, "E" },
                                                        { this.btnSave, "E" }
                                                      });

                string[] xParams = new string[1];
                string xSql = string.Empty;
                DataTable xDt = null;

                //course type 
                xParams = new string[2];
                xParams[0] = "0019";
                xParams[1] = "Y";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlApprovalFlg, xDt);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageSize = 10;
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "approval_user", xScriptMsg);

                    return;
                }

                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID;

                base.pRender(this.Page, new object[,] {
                                                        { this.btnRetrieve, "I" },
                                                        { this.btnAdd, "E" },
                                                        { this.btnSave, "E" }
                                                      });

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (!IsPostBack)
                    {
                        InitControl();
                        BindGrdList(1, "");
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
        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable xDt = new DataTable();
                xDt.Columns.Add("KEYS");
                xDt.Columns.Add("APPROVAL_FLG");
                xDt.Columns.Add("EMPLOYED_STATE");
                xDt.Columns.Add("INSURANCE_FLG");
                xDt.Columns.Add("INSURANCE_DT");
                xDt.Columns.Add("non_approval_cd");
                xDt.Columns.Add("non_approval_remark");
                xDt.Columns.Add("COURSE_START_FLG");

                for (int i = 0; i < this.grdList.Items.Count; i++)
                {
                    //R.USER_ID ||'^'|| R.OPEN_COURSE_ID||'^'|| R.COURSE_RESULT_SEQ
                    DataRow xRow = xDt.NewRow();
                    xRow["KEYS"] = grdList.DataKeys[i].ToString();
                    xRow["APPROVAL_FLG"] = ((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chkApproval")).Checked ? "000001" : "000002";
                    xRow["EMPLOYED_STATE"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("ddlEmployedState")).SelectedValue;
                    xRow["INSURANCE_FLG"] = "";
                    xRow["INSURANCE_DT"] = "";
                    xRow["non_approval_cd"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("ddlNonApprovalCD")).SelectedValue;
                    xRow["non_approval_remark"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("txtNonApprovalRemark")).Text;
                    xRow["COURSE_START_FLG"] = ((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chkStartFlg")).Checked ? "Y" : "N";

                    xDt.Rows.Add(xRow);
                }

                string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                                "SetEduApprovalUserList",
                                                LMS_SYSTEM.APPLICATION,
                                                "CLT.WEB.UI.LMS.APPLICATION",
                                                xDt, "");

                if (xRtn.ToUpper() == "TRUE")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001", new string[] { "수강신청/승인처리" }, new string[] { "evaluator" }, Thread.CurrentThread.CurrentCulture));
                    //this.BindGrdList(1, "");
                }
                else
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void grdUserList_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                DataRowView DRV = (DataRowView)e.Item.DataItem;
                HtmlInputCheckBox chkApproval = ((HtmlInputCheckBox)e.Item.FindControl("chkApproval"));
                HtmlInputCheckBox chkStartFlg = ((HtmlInputCheckBox)e.Item.FindControl("chkStartFlg"));

                TextBox txtEnterDT = ((TextBox)e.Item.FindControl("txtEnterDT"));
                DropDownList ddlNonApprovalCD = ((DropDownList)e.Item.FindControl("ddlNonApprovalCD"));
                WebControlHelper.SetDropDownList(ddlNonApprovalCD, iDtNonApproval, WebControlHelper.ComboType.NullAble);
                ddlNonApprovalCD.SelectedValue = Convert.ToString(DRV["non_approval_cd"]);
                DropDownList ddlEmployedState = ((DropDownList)e.Item.FindControl("ddlEmployedState"));
                WebControlHelper.SetDropDownList(ddlEmployedState, iDtEmployedState, WebControlHelper.ComboType.NullAble);
                ddlEmployedState.SelectedValue = Convert.ToString(DRV["EMPLOYED_STATE"]);

                HyperLink hlkUserNMKor = ((HyperLink)e.Item.FindControl("hlkUserNMKor"));
                hlkUserNMKor.Text = Convert.ToString(DRV["USER_NM_KOR"]);
                hlkUserNMKor.NavigateUrl = "javascript:openPopWindow('/manage/user_edit.aspx?EDITMODE=EDIT&USER_ID=" + Convert.ToString(DRV["USER_ID"]) + "','user_edit', '1280', '821');";

                if (DRV["APPROVAL_FLG"].ToString() == "000001") //승인일경우
                {
                    chkApproval.Checked = true;
                }
                if (DRV["COURSE_START_FLG"].ToString() == "Y")
                {
                    chkStartFlg.Checked = true;
                }

                //본인취소시 승인불가
                //if (Convert.ToString(DRV["APPROVAL_FLG"]) == "000005")
                //{
                //    chkApproval.Attributes.Add("disabled", "disabled");
                //}
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
                        e.Item.Cells[0].Text = "신청일자";
                        e.Item.Cells[1].Text = "사번";
                        e.Item.Cells[2].Text = "성명";
                        e.Item.Cells[3].Text = "회사명";
                        e.Item.Cells[4].Text = "주민등록번호";
                        e.Item.Cells[5].Text = "부서명";
                        e.Item.Cells[6].Text = "직급";
                        e.Item.Cells[7].Text = "상태";
                        //e.Item.Cells[8].Text = "승인<br/>처리";
                        e.Item.Cells[9].Text = "훈련생 구분";
                        e.Item.Cells[10].Text = "고용보험<br/>취득일자";
                        e.Item.Cells[11].Text = "교육<br/>불가사유";
                        e.Item.Cells[12].Text = "불가<br/>사유";
                        e.Item.Cells[13].Text = "승인일자";
                        //e.Item.Cells[14].Text = "교육<br/>입과";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "Date of<br/>Application";
                        e.Item.Cells[1].Text = "ID";
                        e.Item.Cells[2].Text = "Name";
                        e.Item.Cells[3].Text = "Company";
                        e.Item.Cells[4].Text = "Registration";
                        e.Item.Cells[5].Text = "Department";
                        e.Item.Cells[6].Text = "Grade";
                        e.Item.Cells[7].Text = "Status";
                        //e.Item.Cells[8].Text = "Approval";
                        e.Item.Cells[9].Text = "Classification";
                        e.Item.Cells[10].Text = "Hire Date";
                        e.Item.Cells[11].Text = "Absent";
                        e.Item.Cells[12].Text = "Remark";
                        e.Item.Cells[13].Text = "Date of<br/>Approval";
                        //e.Item.Cells[14].Text = "Attendance";
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
