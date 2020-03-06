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
    /// 3. Class 명 : received_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class received_edit : BasePage
    {
        string iManTotCnt = string.Empty;

        int iManTot = 0;
        //int iManCnt = 0;
        //int iUseMan = 0;

        #region protected void Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //this.Page.Form.DefaultButton = this.btn.UniqueID; // Page Default Button Mapping
                if (Request.QueryString["OPEN_COURSE_ID"] != null && Request.QueryString["OPEN_COURSE_ID"].ToString() != string.Empty)
                {
                    if (!IsPostBack)
                    {
                        DataTable dt = null;

                        dt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_s_received_md",
                                             "GetReceivedCourseInfo",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", Request.QueryString["OPEN_COURSE_ID"].ToString(), Session["user_id"].ToString());

                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            this.txtCourseNm.Text = dr["COURSE_NM"].ToString();
                            this.txtCourseDt.Text = dr["COURSE_DT"].ToString();
                        }

                        //Retrieve 하는 버튼이 없기 때문에 Clear 하면 안됨!! 
                        this.BindGrid();
                        this.PageInfo1.PageSize = 10;
                        //base.SetGridClear(this.grd, this.PageNavigator1);
                        //this.PageInfo1.TotalRecordCount = 0;
                        //this.PageInfo1.PageSize = this.PageSize;
                        //this.PageNavigator1.TotalRecordCount = 0;

                    }
                    // 개설 과정의 정원 체크
                    if (Request.QueryString["ManTotCnt"] != null && Request.QueryString["ManTotCnt"].ToString() != string.Empty)
                    {
                        iManTotCnt = Request.QueryString["ManTotCnt"].ToString();

                        iManTot = Convert.ToInt16(Request.QueryString["ManTotCnt"].Substring(Request.QueryString["ManTotCnt"].Length - Convert.ToInt16(Request.QueryString["ManTotCnt"].LastIndexOf('/').ToString())).Replace("/", ""));
                        //iManCnt = Convert.ToInt16(Request.QueryString["ManTotCnt"].Substring(0, Convert.ToInt16(Request.QueryString["ManTotCnt"].IndexOf('/').ToString())));

                        //iUseMan = iManTot - iManCnt;
                        ViewState["ManTotCnt"] = iManTotCnt;
                    }

                }

                base.pRender(this.Page, new object[,] { { this.btnDelete, "D" }, { this.btnSave, "E" } }, Convert.ToString(Request.QueryString["MenuCode"]));

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
        #region private void BindGrid()
        private void BindGrid()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                //if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")            
                // 조회조건이 있을 경우 처리
                xParams = new string[5];
                xParams[0] = "10"; // this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                xParams[2] = Request.QueryString["OPEN_COURSE_ID"].ToString();
                xParams[3] = Session["user_id"].ToString();
                xParams[4] = Session["COMPANY_KIND"].ToString();


                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_s_received_md",
                                             "GetReceivedUserList",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams);
                
                this.grd.DataSource = xDt;
                this.grd.DataBind();
                this.txtCount.Text = xDt.Rows.Count.ToString(); //접수인원 

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
        * Function name : btnSave_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnSave_Click()
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.grd.Items.Count <= iManTotCnt)   //개설된 정원보다 신청 인원이 초과할 경우 체크
                //{
                string[] xParams = new string[5];
                xParams[0] = Request.QueryString["OPEN_COURSE_ID"].ToString();
                xParams[1] = Session["USER_ID"].ToString();
                for (int i = 0; i < this.grd.Items.Count; i++)
                {
                    xParams[2] += this.grd.Items[i].Cells[0].Text.ToString() + "|";
                }
                xParams[3] = this.txtCourseNm.Text;
                xParams[4] = this.txtCourseDt.Text;

                string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPLICATION.vp_s_received_md",
                                                    "SetReceivedCourseResult",
                                                    LMS_SYSTEM.APPLICATION,
                                                    "CLT.WEB.UI.LMS.APPLICATION", (object)xParams);

                if (xRtn != string.Empty)
                {
                    //A001: {0}이(가) 저장되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                      new string[] { "교육접수" },
                                                      new string[] { "Training Received" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));

                    //저장 후 신규 id 값으로 재조회 
                    this.BindGrid();

                }
                else
                {
                    //A004: {0}이(가) 입력되지 않았습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004",
                                                      new string[] { "교육접수" },
                                                      new string[] { "Training Received" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));

                }
                //}
                //else
                //{
                //    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A007",
                //                                        new string[] { "교육접수", iManTotCnt +"명" },
                //                                        new string[] { "Training Received", iManTotCnt + "Persons" },
                //                                        Thread.CurrentThread.CurrentCulture
                //                                       ));
                //}
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnDelete_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnDelete_Click(object sender, EventArgs e)
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool check = false;
                string[] xParams = new string[5];
                xParams[0] = Request.QueryString["OPEN_COURSE_ID"].ToString();
                xParams[1] = Session["USER_ID"].ToString();

                for (int i = 0; i < this.grd.Items.Count; i++)
                {
                    check = ((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grd.Items[i]).FindControl("chk")).Checked;
                    //check = ((CheckBox)this.grd.Items[i].FindControl("chk")).Checked;
                    if (check)
                    {
                        xParams[2] += this.grd.Items[i].Cells[0].Text.ToString() + "|";
                    }
                }
                xParams[3] = this.txtCourseNm.Text;
                xParams[4] = this.txtCourseDt.Text;

                if (xParams[2] == null || xParams[2] == string.Empty)
                {
                    //Please do {0} first
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A018",
                                                                                           new string[] { "Select" },
                                                                                           new string[] { "Select" },
                                                                                           Thread.CurrentThread.CurrentCulture
                                                                                          ));
                }
                else
                {
                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPLICATION.vp_s_received_md",
                                                        "SetReceivedCourseResult_Del",
                                                        LMS_SYSTEM.APPLICATION,
                                                        "CLT.WEB.UI.LMS.APPLICATION", (object)xParams);

                    if (xRtn != string.Empty)
                    {
                        //A001: {0}이(가) 저장되었습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A002",
                                                          new string[] { "교육접수" },
                                                          new string[] { "Training Received" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));

                        //저장 후 신규 id 값으로 재조회 
                        this.BindGrid();

                    }
                    else
                    {
                        //A004: {0}이(가) 입력되지 않았습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004",
                                                          new string[] { "교육접수" },
                                                          new string[] { "Training Received" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));

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
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
       *************************************************************/
        #region protected void grd_ItemCreated(object sender, C1ItemEventArgs e)
        protected void grd_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "user_id";
                        e.Item.Cells[1].Text = "MOBILE_PHONE";
                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "주민등록번호";
                        e.Item.Cells[4].Text = "성명";
                        e.Item.Cells[5].Text = "직책";
                        e.Item.Cells[6].Text = "훈련생구분";
                        e.Item.Cells[7].Text = "고용보험 취득일";
                        //e.Item.Cells[8].Text = "Chk";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "user_id";
                        e.Item.Cells[1].Text = "MOBILE_PHONE";
                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "Register No.";
                        e.Item.Cells[4].Text = "Name";
                        e.Item.Cells[5].Text = "Rank";
                        e.Item.Cells[6].Text = "Trainee Classification";
                        e.Item.Cells[7].Text = "Acquisition Date";
                        //e.Item.Cells[8].Text = "Chk";
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
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void PageNavigator1_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
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
        
        /************************************************************
        * Function name : LnkBtnUserAdd_Click
        * Purpose       : 선택된 user 정보를 user grid에 반영 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void LnkBtnUserAdd_Click(object sender, EventArgs e)
        protected void LnkBtnUserAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string[] xUser = this.HidUserAdd.Value.Split('凸');
                string[] xData = null;
                bool xSel = false;

                int xUserCnt = xUser.Length;
                if (grd.Items.Count + xUserCnt <= iManTot)
                {
                    DataSet ds = new DataSet();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("user_id", typeof(String));
                    dt.Columns.Add("MOBILE_PHONE", typeof(String));
                    dt.Columns.Add("no", typeof(String));
                    dt.Columns.Add("personal_no", typeof(String));
                    dt.Columns.Add("user_nm_kor", typeof(String));
                    dt.Columns.Add("duty_work", typeof(String));
                    dt.Columns.Add("status", typeof(String));
                    dt.Columns.Add("enter_dt", typeof(String));
                    dt.Columns.Add("8", typeof(Boolean));

                    //기존에 추가되어 있는 데이터를 datatable에 추가 
                    for (int i = 0; i < this.grd.Items.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int k = 0; k < this.grd.Items[i].Cells.Count - 1; k++)
                        {
                            dr[k] = this.grd.Items[i].Cells[k].Text;
                        }
                        dr[8] = false;
                        dt.Rows.Add(dr);
                    }

                    //넘어온 아이템을 data table에 추가 
                    for (int i = 0; i < xUser.Length; i++)
                    {
                        xData = xUser[i].Split('|');
                        xSel = false;
                        for (int k = 0; k < this.grd.Items.Count; k++)
                        {
                            if (xData[0].Trim() == this.grd.Items[k].Cells[0].Text.Trim())
                            {
                                xSel = true;
                                break;
                            }
                        }
                        if (xSel == false)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = xData[0].Trim();
                            dr[1] = string.Empty;
                            dr[2] = string.Empty;
                            dr[3] = string.Empty;
                            dr[4] = xData[1].Trim();
                            dr[5] = string.Empty;
                            dr[6] = string.Empty;
                            dr[7] = string.Empty;
                            dr[8] = false;
                            dt.Rows.Add(dr);
                            //this.grd2.Items.Count++; 
                            //this.grd2.Items[this.grd2.Items.Count].Cells.Add(); 
                        }
                    }

                    ds.Tables.Add(dt);
                    this.grd.DataSource = ds;
                    this.grd.DataBind();


                }
                else
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A007",
                                                      new string[] { "교육접수", +iManTot + "명" },
                                                      new string[] { "Training Received", iManTot + "Persons" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                    return;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion 

    }
}
