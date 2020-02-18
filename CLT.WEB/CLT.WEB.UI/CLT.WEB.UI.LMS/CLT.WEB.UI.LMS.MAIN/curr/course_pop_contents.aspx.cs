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


using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading; 

namespace CLT.WEB.UI.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : course_pop_contents Class
    /// 
    /// 2. 주요기능 : contents를 선택하는 pop up 
    ///				  
    /// 3. Class 명 : course_pop_contents
    /// 
    /// 4. 작 업 자 : 최인재/ 2012.01.27
    /// </summary>
    public partial class course_pop_contents : BasePage
    {
        protected string listItems; //넘길 contents 문자열 

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list", xScriptMsg);

                    return;
                }

                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping

                if (!IsPostBack)
                {
                    this.BindDropDownList();
                    //this.BindGrid();
                    if (Session["USER_GROUP"].ToString() != "000009")
                    {
                        this.BindGrid();
                    }
                    else
                    {
                        base.SetGridClear(this.C1WebGrid1, this.PageNavigator1);
                        this.PageInfo1.TotalRecordCount = 0;
                        this.PageInfo1.PageSize = this.PageSize;
                        this.PageNavigator1.TotalRecordCount = 0;
                    }
                }
                base.pRender(this.Page, new object[,] { { this.btnRetrieve, "I" }, { this.btnAdd, "E" } }, Convert.ToString(Request.QueryString["MenuCode"]));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

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


                // 조회조건이 있을 경우 처리
                xParams = new string[6];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                xParams[2] = this.txtContentsNM.Text; // contents_nm
                xParams[3] = this.txtRemark.Text; // remark
                xParams[4] = this.ddlContentsLang.SelectedItem.Value.Replace("*", ""); // lang
                xParams[5] = this.ddlContentsType.SelectedItem.Value.Replace("*", ""); // contents_type

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                             "GetPopContentsList",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams);


                C1WebGrid1.DataSource = xDt;
                C1WebGrid1.DataBind();

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
        * Purpose       : DropDownList 데이터 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void BindDropDownList()
        private void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[1];
                string xSql = string.Empty;
                DataTable xDt = null;

                // Content Type
                xParams[0] = "0042";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlContentsType, xDt);

                // Lang
                xParams[0] = "0017";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlContentsLang, xDt);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
       * Function name : btnRetrieve_Click
       * Purpose       : 
       * Input         : void
       * Output        : void
       *************************************************************/
        #region protected void btnRetrieve_Click(object sender, EventArgs e)
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                this.CurrentPageIndex = 1;
                this.PageInfo1.CurrentPageIndex = 1;
                this.PageNavigator1.CurrentPageIndex = 1;
                this.BindGrid();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
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
                string xContentsId = string.Empty;
                string xContentsNm = string.Empty;
                string xStr = string.Empty;

                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    check = ((CheckBox)this.C1WebGrid1.Items[i].FindControl("chk")).Checked;
                    if (check)
                    {
                        xContentsId = this.C1WebGrid1.Items[i].Cells[9].Text.Trim();
                        xContentsNm = this.C1WebGrid1.Items[i].Cells[1].Text.Trim();
                        xStr = "CONTENT" + " | " + xContentsId + " | " + xContentsNm;
                        this.listItems += "凸" + xStr;
                    }
                }

                this.listItems = (String.IsNullOrEmpty(listItems) ? string.Empty : this.listItems.Substring(1));
                ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>OK('C');</script>");
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
        #region protected void C1WebGrid1_ItemCreated(object sender, C1ItemEventArgs e)
        protected void C1WebGrid1_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "컨텐츠명";
                        e.Item.Cells[2].Text = "파일명";
                        e.Item.Cells[3].Text = "분류";
                        e.Item.Cells[4].Text = "언어";
                        e.Item.Cells[5].Text = "Remark";
                        e.Item.Cells[6].Text = "등록일";
                        e.Item.Cells[7].Text = "임시";
                        e.Item.Cells[8].Text = "Check";
                        e.Item.Cells[9].Text = "contents_id";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Contents Name";
                        e.Item.Cells[2].Text = "File Name";
                        e.Item.Cells[3].Text = "Type";
                        e.Item.Cells[4].Text = "Language";
                        e.Item.Cells[5].Text = "Remark";
                        e.Item.Cells[6].Text = "Reg Date";
                        e.Item.Cells[7].Text = "Temp";
                        e.Item.Cells[8].Text = "Check";
                        e.Item.Cells[9].Text = "contents_id";
                    }
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
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion


    }
}
