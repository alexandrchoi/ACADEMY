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

namespace CLT.WEB.UI.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : 설문 조회 Class
    /// 
    /// 2. 주요기능 : LMS 설문 조회 관리

    ///				  
    /// 3. Class 명 : survey_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.01
    ///
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class survey_list : BasePage
    {
        string[] xHeader = null;
        DataTable xDtCount = null;


        /************************************************************
        * Function name : Page_Load
        * Purpose       : 설문조사 페이지 Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                if (!IsPostBack)
                {

                    //this.txtRes_From.Attributes.Add("onkeyup", "ChkDate(this);");
                    //this.txtRes_To.Attributes.Add("onkeyup", "ChkDate(this);");

                    //this.PageInfo1.TotalRecordCount = 0;
                    //this.PageInfo1.PageSize = this.PageSize;
                    //this.PageNavigator1.TotalRecordCount = 0;

                    base.pRender(this.Page, new object[,] { { this.btnDelete, "D" },
                                                        { this.btnNotice, "A" },
                                                        { this.btnNew, "E" },
                                                        { this.btnExcel, "I" },
                                                        { this.btnRetrieve, "I" },
                                                      });

                    ViewState["UserIDInfo"] = Session["USER_ID"].ToString();

                    SetGridClear(this.C1WebGrid1, this.PageNavigator1, this.PageInfo1);
                    //this.PageInfo1.TotalRecordCount = 0;
                    //this.PageInfo1.PageSize = this.PageSize;
                    //this.PageNavigator1.TotalRecordCount = 0;


                    if (Session["USER_GROUP"].ToString() == "000009")
                    {
                        //return;
                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list_wpg", xScriptMsg);
                    }
                    else
                    {
                        BindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : btnRetrieve_Click
        * Purpose       : 조회 버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnRetrieve_Click(object sender, EventArgs e)
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


        /************************************************************
        * Function name : btnNew_Click
        * Purpose       : 신규추가 버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnNew_Click(object sender, EventArgs e)
        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                string xUrl = string.Format("/curr/survey_ins.aspx?MenuCode={0}", Session["MENU_CODE"]);
                Response.Redirect(xUrl);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : btnDelete_Click
        * Purpose       : 삭제 버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnDelete_Click(object sender, EventArgs e)
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {


                string xRtn = Boolean.FalseString;  // Update 후 결과값 Return
                ArrayList xalChk = new ArrayList(); // 사용자 CheckBox   
                string xScriptMsg = string.Empty;
                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    //if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked == true)  // 체크박스가 True 이면
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                    {
                        if (this.C1WebGrid1.Items[i].Cells[5].Text == "Y") // 게시여부 
                        {
                            //xScriptMsg = string.Format("<script>alert('게시된 설문은 삭제 할 수 없습니다! \\r\\n 설문제목: {0}');</script>", this.C1WebGrid1.Items[i].Cells[9].Text);
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A110", new string[] { this.C1WebGrid1.Items[i].Cells[9].Text }, new string[] { this.C1WebGrid1.Items[i].Cells[9].Text }, Thread.CurrentThread.CurrentCulture));
                            break;
                        }
                        else
                        {
                            string xUserID = this.C1WebGrid1.DataKeys[i].ToString();  // 체크한 그리드의 사용자 Key 값 가져오기

                            if (!xalChk.Contains(xUserID))
                                xalChk.Add(xUserID);
                        }
                    }
                }

                if (xalChk.Count == 0)
                {
                    // 체크박스가 선택되지 않았습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "체크박스" }, new string[] { "Check Box" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                if (xalChk.Count != 0)
                {
                    string[] xParams = new string[xalChk.Count];
                    int xCount = 0;
                    foreach (string xCode in xalChk)
                    {
                        xParams[xCount] = xCode;
                        xCount++;
                    }

                    // 게시되지 않은 설문정보 삭제
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                     "SetResearchDelete",
                     LMS_SYSTEM.CURRICULUM,
                     "CLT.WEB.UI.LMS.CURR",
                     (object)xParams);

                    if (xRtn.ToUpper() == "TRUE")
                    {
                        //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                        BindGrid();

                    }
                    else
                    {
                        //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : btnNotice_Click
        * Purpose       : 게시 버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnNotice_Click(object sender, EventArgs e)
        protected void btnNotice_Click(object sender, EventArgs e)
        {
            try
            {
                string xRtn = Boolean.FalseString;  // Update 후 결과값 Return
                ArrayList xalChk = new ArrayList(); // 사용자 CheckBox   
                string xScriptMsg = string.Empty;
                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    //if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked == true)  // 체크박스가 True 이면
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                    {
                        if (this.C1WebGrid1.Items[i].Cells[5].Text == "Y") // 게시여부 
                        {
                            //xScriptMsg = string.Format("<script>alert('이미 게시된 설문 입니다! \\r\\n 설문제목: {0}');</script>", this.C1WebGrid1.Items[i].Cells[9].Text);
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A111", new string[] { this.C1WebGrid1.Items[i].Cells[9].Text }, new string[] { this.C1WebGrid1.Items[i].Cells[9].Text }, Thread.CurrentThread.CurrentCulture));
                            break;
                        }
                        else
                        {
                            string xUserID = this.C1WebGrid1.DataKeys[i].ToString();  // 체크한 그리드의 사용자 Key 값 가져오기

                            if (!xalChk.Contains(xUserID))
                                xalChk.Add(xUserID);
                        }
                    }
                }

                if (xalChk.Count == 0)
                {
                    // 체크박스가 선택되지 않았습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "체크박스" }, new string[] { "Check Box" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                if (xalChk.Count != 0)
                {
                    string[,] xParams = new string[xalChk.Count, 3];
                    int xCount = 0;
                    foreach (string xCode in xalChk)
                    {
                        xParams[xCount, 0] = "Y";
                        xParams[xCount, 1] = Session["USER_ID"].ToString();
                        xParams[xCount, 2] = xCode;

                        xCount++;
                    }

                    // 게시되지 않은 설문조사 게시
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                     "SetResearchNotice",
                     LMS_SYSTEM.CURRICULUM,
                     "CLT.WEB.UI.LMS.CURR",
                     (object)xParams);

                    if (xRtn.ToUpper() == "TRUE")
                    {
                        // 정상적으로 처리 완료 되었습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                        BindGrid();

                    }
                    else
                    {
                        //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : btnExcel_Click
        * Purpose       : 엑셀출력 버튼 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnExcel_Click(object sender, EventArgs e)
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string[] xParams = new string[7];
                DataTable xDt = null;

                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                if (!string.IsNullOrEmpty(txtRes_From.Text))
                    xParams[2] = txtRes_From.Text.Trim();

                if (!string.IsNullOrEmpty(txtRes_To.Text))
                    xParams[3] = txtRes_To.Text.Trim();

                if (!string.IsNullOrEmpty(txtRes_NM.Text))
                    xParams[4] = txtRes_NM.Text;

                xParams[6] = "EXCEL";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                       "GetSurveyInfo",
                                       LMS_SYSTEM.CURRICULUM,
                                       "CLT.WEB.UI.LMS.CURR",
                                       (object)xParams);

                if (xDt.Rows.Count == 0)
                {
                    // 자료가 없습니다!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A062", new string[] { "자료" }, new string[] { "Data" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                if (xDt.Rows.Count > 0)
                    GetExcelFile(xDt, xHeader);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion




        /************************************************************
        * Function name : BindGrid
        * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region BindGrid()
        public void BindGrid()
        {
            try
            {
                string[] xParams = new string[7];
                DataTable xDt = null;

                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                if (!string.IsNullOrEmpty(txtRes_From.Text))
                    xParams[2] = txtRes_From.Text.Trim().Replace("'", "''"); ;

                if (!string.IsNullOrEmpty(txtRes_To.Text))
                    xParams[3] = txtRes_To.Text.Trim().Replace("'", "''"); ;

                if (!string.IsNullOrEmpty(txtRes_NM.Text))
                    xParams[4] = txtRes_NM.Text.Replace("'", "''"); ;

                xParams[5] = string.Empty;
                xParams[6] = "GRID";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                       "GetSurveyInfo",
                                       LMS_SYSTEM.CURRICULUM,
                                       "CLT.WEB.UI.LMS.CURR",
                                       (object)xParams);

                C1WebGrid1.DataSource = xDt;
                C1WebGrid1.DataBind();

                if (xDt.Rows.Count < 1)
                {
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageInfo1.TotalRecordCount = 0;
                    this.PageNavigator1.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageInfo1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    //PageInfo1.CurrentPageIndex = rPageIndex;
                    //PageNavigator1.CurrentPageIndex = rPageIndex;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion


        /************************************************************
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void grd_ItemCreated(object sender, C1ItemEventArgs e)
        protected void grd_ItemCreated(object seder, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    xHeader = new string[8];
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[1].Text = "설문제목";
                        e.Item.Cells[2].Text = "항목수";
                        e.Item.Cells[3].Text = "설문등록일";
                        e.Item.Cells[4].Text = "응답기간";
                        e.Item.Cells[5].Text = "게시여부";
                        e.Item.Cells[6].Text = "총대상자";
                        e.Item.Cells[7].Text = "응답수";
                        e.Item.Cells[8].Text = "미응답수";
                    }
                    else
                    {
                        e.Item.Cells[1].Text = "Survey Title";
                        e.Item.Cells[2].Text = "Items";
                        e.Item.Cells[3].Text = "Creation Date";
                        e.Item.Cells[4].Text = "Response Period";
                        e.Item.Cells[5].Text = "Post Status";
                        e.Item.Cells[6].Text = "Participant";
                        e.Item.Cells[7].Text = "Answer";
                        e.Item.Cells[8].Text = "No Answer";
                    }

                    xHeader[0] = e.Item.Cells[1].Text;
                    xHeader[1] = e.Item.Cells[2].Text;
                    xHeader[2] = e.Item.Cells[3].Text;
                    xHeader[3] = e.Item.Cells[4].Text;
                    xHeader[4] = e.Item.Cells[5].Text;
                    xHeader[5] = e.Item.Cells[6].Text;
                    xHeader[6] = e.Item.Cells[7].Text;
                    xHeader[7] = e.Item.Cells[8].Text;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : grd_OnItemDataBound
        * Purpose       : C1WebGrid의 Data Bound 이젠트
                          C1WebGrid Unbound 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grd_OnItemDataBound(object sender, C1ItemEventArgs e)
        {
            //DataRowView xItem = (DataRowView)e.Item.DataItem;

            //Label lblSumCnt = null;

            //if (e.Item.ItemType == C1ListItemType.Item || e.Item.ItemType == C1ListItemType.AlternatingItem)
            //{
            //    if (xItem != null)
            //    {
            //        if (string.IsNullOrEmpty(xItem["open_course_id"].ToString())) // OpenCourse ID가 null 이면 일반설문
            //        {



            //            lblSumCnt = ((Label)e.Item.FindControl("lblSumCnt"));

            //        }
            //        else
            //        {

            //        }
            //    }
            //}

            ////lblSumCnt
        }

        /************************************************************
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        #region PageNavigator1_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
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
        #endregion


    }
}