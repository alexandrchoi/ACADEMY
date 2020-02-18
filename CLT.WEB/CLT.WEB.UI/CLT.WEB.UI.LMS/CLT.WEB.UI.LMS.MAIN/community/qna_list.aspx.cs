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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.COMMUNITY
{
    /// <summary>
    /// 1. 작업개요 : Q&A 조회 Class
    /// 
    /// 2. 주요기능 : LMS Q&A 조회 관리

    ///				  
    /// 3. Class 명 : qna_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.01
    ///
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class qna_list : BasePage
    {
        public static string iDelYN = "";

        /************************************************************
        * Function name : Page_Load
        * Purpose       : 페이지 Load 이벤트

        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                if (!IsPostBack)
                {
                    getAuth();

                    //this.ddlDelYn.Items.Add(new ListItem("ALL", ""));
                    //this.ddlDelYn.Items.Add(new ListItem("N", "N"));
                    //this.ddlDelYn.Items.Add(new ListItem("Y", "Y"));
                    //this.ddlDelYn.SelectedValue = string.IsNullOrEmpty(Request.QueryString["delYN"]) ? iDelYN : Request.QueryString["delYN"].ToString();

                    //this.ddlType.Items.Add(new ListItem("모두", "000001"));
                    //this.ddlType.Items.Add(new ListItem("제목", "000002"));
                    //this.ddlType.Items.Add(new ListItem("내용", "000003"));
                    BindDropDownList();

                    base.pRender(this.Page, new object[,] { { this.btnNew, "E" }
                                                      });
                    //btnNew.Visible = AuthWrite;
                    //lblDelYn.Visible = ddlDelYn.Visible = AuthDelete | AuthAdmin;

                    //DateTime xdtFrom = DateTime.Today.AddDays(-31);

                    //this.txtqna_From.Text = xdtFrom.ToString("yyyy.MM.dd");
                    //this.txtNotice_To.Text = DateTime.Now.ToString("yyyy.MM.dd");

                    //if (Request.QueryString["BIND"] == null)

                    //this.txtNotice_From.Attributes.Add("onkeyup", "ChkDate(this);");
                    //this.txtNotice_To.Attributes.Add("onkeyup", "ChkDate(this);");

                    SetGridClear(this.C1WebGrid1, PageNavigator1);

                    //if (Session["USER_GROUP"].ToString() != "000009")
                    BindGrid();

                    if (Session["USER_GROUP"].ToString() != "000001")
                    {
                        this.C1WebGrid1.Columns[6].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void DdlDelYn_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGridClear(this.C1WebGrid1, PageNavigator1);
            BindGrid();
        }
        #endregion
        
        /************************************************************
        * Function name : btnRetrieve_Click
        * Purpose       : 조회 Click 이벤트

        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnRetrieve_Click(object sender, EventArgs e)
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                if (!string.IsNullOrEmpty(txtNotice_From.Text) && string.IsNullOrEmpty(txtNotice_To.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A039", new string[] { "작성일자" }, new string[] { "Date" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                else if (string.IsNullOrEmpty(txtNotice_From.Text) && !string.IsNullOrEmpty(txtNotice_To.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A039", new string[] { "작성일자" }, new string[] { "Date" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }
                */

                BindGrid();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : btnClear_OnClick
        * Purpose       : 초기화 Click 이벤트

        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnClear_OnClick(object sender, EventArgs e)
        protected void btnClear_OnClick(object sender, EventArgs e)
        {
            try
            {
                this.txtContent.Text = string.Empty;
                //DateTime xdtFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                //this.txtNotice_From.Text = string.Empty; //xdtFrom.ToString("yyyy.MM.dd");
                //this.txtNotice_To.Text = string.Empty; //DateTime.Now.ToString("yyyy.MM.dd");
                //this.txtCus_ID.Text = string.Empty;
                //this.txtCus_NM.Text = string.Empty;
                //this.ddlCus_Date.Items.Clear();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
        * Function name : btnNew_OnClick
        * Purpose       : 생성 Click 이벤트

        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnNew_OnClick(object sender, EventArgs e)
        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            try
            {
                string xScriptMsg = string.Format("<script>window.location.href='/community/qna_edit.aspx?EDITMODE=NEW&MenuCode={0}';</script>", Session["MENU_CODE"]);
                ScriptHelper.ScriptBlock(this, "qna_list", xScriptMsg);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : C1WebGrid1_ItemDataBound
        * Purpose       : 그리드 Databound 이벤트

        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void C1WebGrid1_ItemDataBound(object sender, C1ItemEventArgs e)
        protected void C1WebGrid1_ItemDataBound(object sender, C1ItemEventArgs e)
        {
            try
            {
                DataRowView xItem = (DataRowView)e.Item.DataItem;

                if (e.Item.ItemType == C1ListItemType.Item || e.Item.ItemType == C1ListItemType.AlternatingItem)
                {
                    Image img = ((Image)e.Item.FindControl("imgItems"));

                    if (xItem["attcount"] == null)
                        img.ImageUrl = "/asset/Images/blank.gif";
                    else if (xItem["attcount"].ToString() == "0")
                        img.ImageUrl = "/asset/Images/blank.gif";
                    else
                        img.ImageUrl = "/asset/Images/clip.png";

                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : grd_ItemCreated
        * Purpose       : 그리드 언어별 항목명 이벤트

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
                        e.Item.Cells[0].Text = "순번";
                        e.Item.Cells[1].Text = "제목";
                        //e.Item.Cells[2].Text = "첨부파일";
                        e.Item.Cells[3].Text = "작성자";
                        e.Item.Cells[4].Text = "작성일자";
                        e.Item.Cells[5].Text = "조회수";
                        e.Item.Cells[6].Text = "삭제유무";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No";
                        e.Item.Cells[1].Text = "Subject";
                        //e.Item.Cells[2].Text = "첨부파일";
                        e.Item.Cells[3].Text = "Author";
                        e.Item.Cells[4].Text = "Date";
                        e.Item.Cells[5].Text = "Hits";
                        e.Item.Cells[6].Text = "Delete";
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
        * Function name : BindGrid
        * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region public void BindGrid()
        public void BindGrid()
        {
            try
            {
                string[] xParams = new string[7];

                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = string.Empty; // this.txtNotice_From.Text;  // 작성일자
                xParams[3] = string.Empty; // this.txtNotice_To.Text;  // 작성일자
                xParams[4] = this.ddlType.SelectedValue;  // 000001 : 모두, 000002 : 제목, 000003 내용
                xParams[5] = this.txtContent.Text.Replace("'", "''");  // 검색내용
                xParams[6] = Session["USER_GROUP"].ToString();

                DataTable xDt = new DataTable();
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMUNITY.vp_y_community_qna_md",
                           "GetQnAMaster",
                           LMS_SYSTEM.COMMUNITY,
                           "CLT.WEB.UI.LMS.COMMUNITY.qna_list", (object)xParams, Thread.CurrentThread.CurrentCulture);

                this.C1WebGrid1.DataSource = xDt;
                this.C1WebGrid1.DataBind();

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
        * Function name : BindDropDownList
        * Purpose       : 공통코드 Data처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region BindDropDownList()
        public void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[2];

                xParams[0] = "0063"; // 게시판 검색구분

                xParams[1] = "Y";
                DataTable xDtUser = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                     "GetCommonCodeInfo",
                                                     LMS_SYSTEM.COMMUNITY,
                                                     "CLT.WEB.UI.LMS.COMMUNITY.qna_list", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlType, xDtUser, 0, false);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion
    }
}