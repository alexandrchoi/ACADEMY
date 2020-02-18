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
using System.Threading;
using C1.Web.C1WebGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.EDUM
{
    public partial class pass : BasePage
    {
        private DataTable iDtPassFlg;
        private DataTable iDtNonPassFlg;
        //O.COURSE_ID ||'^'|| O.OPEN_COURSE_ID
        private string iSearch { get { return Util.Request("search"); } }
        
        #region 인터페이스 그룹

        #endregion

        #region 기타 프로시저 그룹 [Core Logic]
        private DataSet GetDsGrdList(string rGubun, int rPageIndex)
        {
            DataSet xDs = null;
            try
            {
                string[] xParams = new string[6];
                xParams[0] = this.PageSize.ToString();
                //xParams[1] = this.CurrentPageIndex.ToString();
                xParams[1] = rPageIndex.ToString();
                xParams[2] = iSearch;
                string[] rSearch = Util.Split(iSearch, "^", 2);
                xParams[4] = rSearch[0];
                xParams[5] = rSearch[1];

                xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                       "GetEduPassUserList",
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
                
                //이수구분
                if (Util.IsNullOrEmptyObject(iDtPassFlg))
                {
                    string[] xParams = new string[1];
                    string xSql = string.Empty;

                    xParams = new string[2];
                    xParams[0] = "0010";
                    xParams[1] = "Y";
                    iDtPassFlg = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                 "GetCommonCodeInfo",
                                                 LMS_SYSTEM.EDUMANAGEMENT,
                                                 "CLT.WEB.UI.LMS.EDUM", (object)xParams, Thread.CurrentThread.CurrentCulture);
                }
                // 미이수사유
                if (Util.IsNullOrEmptyObject(iDtNonPassFlg))
                {
                    string[] xParams = new string[1];
                    string xSql = string.Empty;

                    xParams = new string[2];
                    xParams[0] = "0050";
                    xParams[1] = "Y";
                    iDtNonPassFlg = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                 "GetCommonCodeInfo",
                                                 LMS_SYSTEM.EDUMANAGEMENT,
                                                 "CLT.WEB.UI.LMS.EDUM", (object)xParams, Thread.CurrentThread.CurrentCulture);
                }

                DataSet xDs = GetDsGrdList(rGubun, rPageIndex);
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
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list_wpg", xScriptMsg);

                    return;
                }

                base.pRender(this.Page, new object[,] { 
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
                xDt.Columns.Add("PASS_FLG");
                xDt.Columns.Add("ORDER_FLG");
                xDt.Columns.Add("NON_PASS_CD");
                xDt.Columns.Add("NON_PASS_REMARK");
                xDt.Columns.Add("PRE_ORDER_FLG");

                for (int i = 0; i < this.grdList.Items.Count; i++)
                {
                    //R.USER_ID ||'^'|| R.OPEN_COURSE_ID||'^'|| R.COURSE_RESULT_SEQ
                    DataRow xRow = xDt.NewRow();
                    xRow["KEYS"] = grdList.DataKeys[i].ToString();
                    
                    //수강일 경우 체크 박스 우선
                    //xRow["PASS_FLG"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("ddlPassFlg")).SelectedValue == "000004" ? (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chkOrderFlg")).Checked ? "000001" : "000005") : ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("ddlPassFlg")).SelectedValue;
                    xRow["PASS_FLG"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("ddlPassFlg")).SelectedValue == "000004" ? (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chkPassFlg")).Checked ? "000001" : "000005") : ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("ddlPassFlg")).SelectedValue;

                    //이수된 데이터가 아니면 발령은 무조건 "N"
                    if (xRow["PASS_FLG"].ToString() == "000001")
                        xRow["ORDER_FLG"] = ((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chkOrderFlg")).Checked ? "Y" : "N";
                    else
                        xRow["ORDER_FLG"] = "N";
                    
                    xRow["NON_PASS_CD"] = ((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("ddlNonPassCD")).SelectedValue;
                    xRow["NON_PASS_REMARK"] = ((TextBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("txtRemark")).Text;
                    xRow["PRE_ORDER_FLG"] = ((HiddenField)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("hdnOrderFlg")).Value;

                    //교육이수 시 증서번호 체크 후 없으면 발급
                    /******************************************************************************/
                    //if (((DropDownList)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("ddlPassFlg")).SelectedValue == "000001")
                    if(xRow["PASS_FLG"].ToString() =="000001")
                    {
                        SBROKER.ExecuteOnly("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                                "SetCERTIFICATE_KEY",
                                                 LMS_SYSTEM.EDUMANAGEMENT,
                                                "CLT.WEB.UI.LMS.EDUM",
                                                grdList.DataKeys[i].ToString());
                    }
                     /******************************************************************************/

                    xDt.Rows.Add(xRow);
                }

                string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                                "SetEduPassUserList",
                                                LMS_SYSTEM.EDUMANAGEMENT,
                                                "CLT.WEB.UI.LMS.EDUM",
                                                xDt, "");
                string xScriptMsg = "";
                if (xRtn.ToUpper() == "TRUE")
                {
                    //A001: {0}이(가) 저장되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                      new string[] { "컨텐츠" },
                                                      new string[] { "Contents" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                    //xScriptMsg = "<script>alert('정상적으로 저장되었습니다.');</script>";

                    //저장 후 조회 seojw 2014.09.17
                    if (Session["iPageindex"] == null)
                    {
                        this.BindGrdList(1, "");
                    }
                    else
                    {
                        this.BindGrdList(Convert.ToInt32(Session["iPageindex"].ToString()), "");
                    }
                }
                else
                {
                    //A103 정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103",
                                                  new string[] { null },
                                                  new string[] { null },
                                                  Thread.CurrentThread.CurrentCulture
                                                     ));
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                }

                //ScriptHelper.ScriptBlock(this, "vp_a_appraisal_competency_detail_wpg", xScriptMsg);
                
                
                
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
                HtmlInputCheckBox chkPassFlg = ((HtmlInputCheckBox)e.Item.FindControl("chkPassFlg"));
                HtmlInputCheckBox chkOrderFlg = ((HtmlInputCheckBox)e.Item.FindControl("chkOrderFlg"));
                chkPassFlg.Checked = Convert.ToString(DRV["PASS_FLG"]) == "000001";
                chkOrderFlg.Checked = Convert.ToString(DRV["ORDER_FLG"]) == "Y";

                DropDownList ddlPassFlg = ((DropDownList)e.Item.FindControl("ddlPassFlg"));
                WebControlHelper.SetDropDownList(ddlPassFlg, iDtPassFlg, WebControlHelper.ComboType.NullAble);
                ddlPassFlg.SelectedValue = Convert.ToString(DRV["PASS_FLG"]);
                DropDownList ddlNonPassCD = ((DropDownList)e.Item.FindControl("ddlNonPassCD"));
                WebControlHelper.SetDropDownList(ddlNonPassCD, iDtNonPassFlg, WebControlHelper.ComboType.NullAble);
                ddlNonPassCD.SelectedValue = Convert.ToString(DRV["NON_PASS_CD"]);

                TextBox txtRemark = ((TextBox)e.Item.FindControl("txtRemark"));
                if (Convert.ToString(DRV["ORDER_FLG"]) == "Y")
                {
                    chkOrderFlg.Attributes.Add("disabled", "disabled");
                    ddlPassFlg.Enabled = false;
                    chkPassFlg.Attributes.Add("disabled", "disabled");
                    ddlNonPassCD.Enabled = false;
                    txtRemark.Enabled = false;
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
                Session["iPageindex"] = Convert.ToString(e.PageIndex);

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
                        e.Item.Cells[0].Text = "사번";
                        e.Item.Cells[1].Text = "성명";
                        e.Item.Cells[2].Text = "회사명";
                        e.Item.Cells[3].Text = "부서명";
                        e.Item.Cells[4].Text = "직급";
                        e.Item.Cells[5].Text = "진도";
                        e.Item.Cells[6].Text = "기말";
                        e.Item.Cells[7].Text = "과제";
                        e.Item.Cells[8].Text = "총점";
                        e.Item.Cells[9].Text = "교육<br/>시작일";
                        e.Item.Cells[10].Text = "교육<br/>종료일";
                        e.Item.Cells[11].Text = "이수<br/>구분";
                        //e.Item.Cells[12].Text = "이수";
                        //e.Item.Cells[13].Text = "발령";
                        e.Item.Cells[14].Text = "미이수<br/>사유";
                        e.Item.Cells[15].Text = "Remark";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "ID";
                        e.Item.Cells[1].Text = "Name";
                        e.Item.Cells[2].Text = "Company";
                        e.Item.Cells[3].Text = "Department";
                        e.Item.Cells[4].Text = "Grade";
                        e.Item.Cells[5].Text = "Progress";
                        e.Item.Cells[6].Text = "Final Test";
                        e.Item.Cells[7].Text = "Report";
                        e.Item.Cells[8].Text = "Total<br/>Score";
                        e.Item.Cells[9].Text = "Date of<br/>Start";
                        e.Item.Cells[10].Text = "Date of<br/>End";
                        e.Item.Cells[11].Text = "Classification";
                        //e.Item.Cells[12].Text = "Completion";
                        //e.Item.Cells[13].Text = "Appointment";
                        e.Item.Cells[14].Text = "Comments";
                        e.Item.Cells[15].Text = "Remark";
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
