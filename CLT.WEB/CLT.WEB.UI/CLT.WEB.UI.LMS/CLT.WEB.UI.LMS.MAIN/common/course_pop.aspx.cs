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

// 필수 using 문

using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading;

namespace CLT.WEB.UI.LMS.COMMON
{
    /// <summary>
    /// 1. 작업개요 : course_pop Class
    /// 
    /// 2. 주요기능 : 등록과정 목록을 조회하는 처리
    ///				  
    /// 3. Class 명 : course_pop
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.08
    /// </summary>
    public partial class course_pop : BasePage
    {
        int iPageSize = 10;

        private string iCourseType { get { return Util.Request("course_type"); } }
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 페이지 로드될 때 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                //{
                //    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                //    ScriptHelper.ScriptBlock(this, "course_pop", xScriptMsg);

                //    return;
                //}

                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID;
                base.pRender(this.Page, new object[,] { { this.btnRetrieve, "I" } }, Convert.ToString(Request.QueryString["MenuCode"]));

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    // List 페이지로부터 opener_textbox_id, opener_textbox_nm를 전달받은 경우만 해당 페이지를 처리하고
                    // 그렇지 않은 경우, 메세지 출력과 함께 창을 종료한다.
                    if ((Request.QueryString["opener_textbox_id"] != null && Request.QueryString["opener_textbox_id"].ToString() != "") || (Request.QueryString["opener_textbox_nm"] != null && Request.QueryString["opener_textbox_nm"].ToString() != ""))
                    {
                        ViewState["opener_textbox_id"] = Request.QueryString["opener_textbox_id"].ToString();
                        ViewState["opener_textbox_nm"] = Request.QueryString["opener_textbox_nm"].ToString();

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
                                this.PageInfo1.PageSize = iPageSize;
                                this.PageNavigator1.TotalRecordCount = 0;
                            }
                        }
                    }
                    else
                    {
                        string xScriptContent = "<script>alert('잘못된 경로를 통해 접근하였습니다.');self.close();</script>";
                        ScriptHelper.ScriptBlock(this, "course_pop", xScriptContent);
                    }
                }
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
        private void BindGrid()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                if (string.IsNullOrEmpty(this.txtCourseNM.Text) && this.ddlCourseLang.SelectedItem.Text == "*" && this.ddlCourseType.SelectedItem.Text == "*")
                {
                    // 조회조건이 없을 경우 처리
                    xParams = new string[2];
                    xParams[0] = iPageSize.ToString(); // pageSize
                    xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                 "GetCourseInfo",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                }
                else
                {
                    // 조회조건이 있을 경우 처리
                    xParams = new string[6];
                    xParams[0] = iPageSize.ToString(); // pagesize
                    xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                    xParams[2] = this.txtCourseNM.Text; // course_nm
                    xParams[3] = this.ddlCourseLang.SelectedItem.Value.Replace("*", ""); // course_lang
                    xParams[4] = this.ddlCourseType.SelectedItem.Value.Replace("*", ""); // course_type
                    xParams[5] = iCourseType;
                    xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                 "GetCourseInfoByCondition",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                }

                C1WebGrid1.DataSource = xDt;
                C1WebGrid1.DataBind();

                if (xDt.Rows.Count < 1)
                {
                    this.PageInfo1.TotalRecordCount = 0;
                    this.PageInfo1.PageSize = iPageSize;
                    this.PageNavigator1.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    this.PageInfo1.PageSize = iPageSize;
                    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /************************************************************
        * Function name : BindDropDownList
        * Purpose       : DropDownList 데이터 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        private void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[1];
                string xSql = string.Empty;
                DataTable xDt = null;

                // CourseType
                xParams[0] = "0006";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.MAIN,
                                             "CLT.WEB.UI.LMS.MAIN", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlCourseType, xDt);

                if (!string.IsNullOrEmpty(iCourseType))
                    ddlCourseType.SelectedValue = iCourseType;

                // Lang
                xParams[0] = "0017";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.MAIN,
                                             "CLT.WEB.UI.LMS.MAIN", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlCourseLang, xDt);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /************************************************************
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void WebGrid1_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        //e.Item.Cells[1].Text = "In/Out";
                        e.Item.Cells[1].Text = "과정유형";
                        e.Item.Cells[2].Text = "과정명";
                        e.Item.Cells[3].Text = "언어";
                        e.Item.Cells[4].Text = "등록일자";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        //e.Item.Cells[1].Text = "In/Out";
                        e.Item.Cells[1].Text = "Type";
                        e.Item.Cells[2].Text = "Course Name";
                        e.Item.Cells[3].Text = "Language";
                        e.Item.Cells[4].Text = "Reg Date";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : btnRetrieve_Click
        * Purpose       : 조회 조건에 대한 결과를 조회하여 리스트로 출력하는 처리
        * Input         : void
        * Output        : void
        *************************************************************/
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
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
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

    }
}