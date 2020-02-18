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
    /// 3. Class 명 : received_pop_user
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class received_pop_user : BasePage
    {
        protected string iarrUser = string.Empty;
        int iManTotCnt = 0;
        int iManCnt = 0;
        int iUseMan = 0;

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID;

                if (!IsPostBack)
                {

                    if (Request.QueryString["OPEN_COURSE_ID"] != null)
                        ViewState["OPEN_COURSE_ID"] = Request.QueryString["OPEN_COURSE_ID"].ToString();
                    else
                        ViewState["OPEN_COURSE_ID"] = string.Empty;

                    //this.BindDropDownList();

                    //this.BindGrid();
                    if (Session["USER_GROUP"].ToString() != "000009")
                    {
                        this.BindGrid();
                    }
                    else
                    {
                        base.SetGridClear(this.grd, this.PageNavigator1);
                        this.PageInfo1.TotalRecordCount = 0;
                        this.PageInfo1.PageSize = this.PageSize;
                        this.PageNavigator1.TotalRecordCount = 0;
                    }

                    this.SetControlAttributes(new object[] { txtName }, this.btnRetrieve, this.Page);

                }
                // 개설 과정의 정원 체크
                if (Request.QueryString["ManTotCnt"] != null && Request.QueryString["ManTotCnt"].ToString() != string.Empty)
                {
                    iManTotCnt = Convert.ToInt16(Request.QueryString["ManTotCnt"].Substring(Request.QueryString["ManTotCnt"].Length - Convert.ToInt16(Request.QueryString["ManTotCnt"].LastIndexOf('/').ToString())));
                    iManCnt = Convert.ToInt16(Request.QueryString["ManTotCnt"].Substring(0, Convert.ToInt16(Request.QueryString["ManTotCnt"].IndexOf('/').ToString())));

                    iUseMan = iManTotCnt - iManCnt;
                }

                base.pRender(this.Page, new object[,] { { this.btnAdd, "E" }, { this.btnRetrieve, "I" } }, Convert.ToString(Request.QueryString["MenuCode"]));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : BindDropDownList
        * Purpose       : DropDownList 데이터 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void BindDropDownList()
        private void BindDropDownList()
        {
            //try
            //{
            //    string[] xParams = new string[1];
            //    string xSql = string.Empty;
            //    DataTable xDt = null;

            //    xParams[0] = "0002";
            //    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.BASE.vp_l_common_md",
            //                                 "GetCommonCodeInfo",
            //                                 LMS_SYSTEM.COMMISSION,
            //                                 "CLT.WEB.UI.LMS.COMMISSION", (object)xParams);
            //    WebControlHelper.SetDropDownList(this.ddlClassification, xDt);
            //}
            //catch (Exception ex)
            //{
            //    bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
            //    if (rethrow) throw;
            //}
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
                xParams[0] = "10";// this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex            
                xParams[2] = Session["user_id"].ToString();
                xParams[3] = this.txtName.Text;
                xParams[4] = Session["COMPANY_KIND"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_s_received_md",
                                             "GetReceivedUserInfoList",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams);


                this.grd.DataSource = xDt;
                this.grd.DataBind();

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
        * Function name : btnRetreive_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnRetreive_Click(object sender, EventArgs e)
        protected void btnRetreive_Click(object sender, EventArgs e)
        {
            try
            {
                this.BindGrid();
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
                        e.Item.Cells[1].Text = "user_nm_kor";
                        e.Item.Cells[2].Text = "No.";


                        e.Item.Cells[3].Text = "성명";
                        e.Item.Cells[4].Text = "영문명";
                        e.Item.Cells[5].Text = "주민등록번호";

                        e.Item.Cells[6].Text = "직책";
                        e.Item.Cells[7].Text = "훈련생구분";
                        e.Item.Cells[8].Text = "고용보험 취득일";

                        e.Item.Cells[9].Text = "휴대전화";
                        e.Item.Cells[10].Text = "Email";
                        //e.Item.Cells[11].Text = "Chk";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "user_id";
                        e.Item.Cells[1].Text = "user_nm_kor";
                        e.Item.Cells[2].Text = "No.";

                        e.Item.Cells[3].Text = "Name";
                        e.Item.Cells[4].Text = "ENG Name";
                        e.Item.Cells[5].Text = "Register No.";

                        e.Item.Cells[6].Text = "Rank";
                        e.Item.Cells[7].Text = "Trainee Classification";
                        e.Item.Cells[8].Text = "Acquisition Date";

                        e.Item.Cells[9].Text = "Mobile";
                        e.Item.Cells[10].Text = "Email";
                        //e.Item.Cells[11].Text = "Chk";
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
        * Function name : LnkBtnUser_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void LnkBtnUser_Click(object sender, EventArgs e)
        protected void LnkBtnUser_Click(object sender, EventArgs e)
        {

            //try
            //{
            //    string[] xParams = null;
            //    DataTable xDt = null;

            //    xParams = new string[2];
            //    xParams[0] = ViewState["OPEN_COURSE_ID"].ToString(); 
            //    xParams[1] = this.HidUser.Value;


            //    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMISSION.vp_s_received_md",
            //                                 "GetReceivedUserInfo",
            //                                 LMS_SYSTEM.COMMISSION,
            //                                 "CLT.WEB.UI.LMS.COMMISSION", (object)xParams);

            //    if(xDt.Rows.Count>0)
            //    {
            //        DataRow dr = xDt.Rows[0];
            //        this.txtUserNm.Text = dr["USER_NM_KOR"].ToString();
            //        this.txtPersonalNo.Text = dr["personal_no"].ToString();
            //        this.chkInsure.Checked = dr["INSURANCE_FLG"].ToString() == "Y" ? true : false;
            //        this.txtInsureDt.Text = dr["enter_dt"].ToString(); 
            //        //WebControlHelper.SetSelectItem_DropDownList(this.ddlClassification, dr["status"].ToString());
            //        WebControlHelper.SetSelectItem_DropDownList(this.ddlClassification, dr["employed_state"].ToString());

            //        this.txtEngNm.Text = dr["user_nm_eng"].ToString();
            //        this.txtMobile.Text = dr["mobile_phone"].ToString();
            //        this.txtOffice.Text = dr["office_phone"].ToString();
            //        this.txtEmail.Text = dr["email_id"].ToString(); 
            //    }                
            //}
            //catch (Exception ex)
            //{
            //    base.NotifyError(ex); 
            //}
        }
        #endregion 
        
        /************************************************************
        * Function name : btnRetrieve_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnAdd_Click(object sender, EventArgs e)
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                bool check;

                //iarrUser
                int xCnt = 0;
                string xUserId = string.Empty;
                string xUserNm = string.Empty;

                for (int i = 0; i < this.grd.Items.Count; i++)
                {
                    check = ((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grd.Items[i]).FindControl("chk")).Checked;
                    //check = ((CheckBox)this.grd.Items[i].FindControl("chk")).Checked;
                    if (check)
                    {
                        xUserId = this.grd.Items[i].Cells[0].Text.Trim(); //user_id
                        xUserNm = this.grd.Items[i].Cells[1].Text.Trim(); //user_nm_kor 
                        this.iarrUser += "凸" + xUserId + "|" + xUserNm;
                        xCnt++;
                    }
                }

                if (xCnt > iUseMan)   //개설된 정원보다 신청 인원이 초과할 경우 체크
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A007",
                                                       new string[] { "교육접수", +iManTotCnt + "명" },
                                                       new string[] { "Training Received", iManTotCnt + "Persons" },
                                                       Thread.CurrentThread.CurrentCulture
                                                      ));
                    return;

                }
                if (this.iarrUser == string.Empty)
                {
                    //A018 Please do {0} first
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A018",
                                                                                            new string[] { "Select" },
                                                                                            new string[] { "Select" },
                                                                                            Thread.CurrentThread.CurrentCulture
                                                                                           ));
                }
                else
                {
                    this.iarrUser = (String.IsNullOrEmpty(iarrUser) ? string.Empty : this.iarrUser.Substring(1));
                    ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>OK();</script>");
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
