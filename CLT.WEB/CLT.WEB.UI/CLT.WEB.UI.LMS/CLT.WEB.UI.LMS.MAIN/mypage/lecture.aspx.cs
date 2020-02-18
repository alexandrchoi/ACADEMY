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
using System.IO;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.MYPAGE
{
    /// <summary>
    /// 1. 작업개요 : 나의 강의실 Class
    /// 
    /// 2. 주요기능 : LMS 나의 강의실
    ///				  
    /// 3. Class 명 : lecture
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class lecture : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    //return;
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "lecture", xScriptMsg);
                }
                else
                {
                    if (!IsPostBack)
                    {
                        this.tabctrl_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void grd1_ItemCreated(object sender, C1ItemEventArgs e)
        protected void grd1_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "open_course_id"; //key
                        e.Item.Cells[1].Text = "COURSE_TYPE_KEY";
                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "교육유형";
                        e.Item.Cells[4].Text = "과정명";
                        e.Item.Cells[5].Text = "진도";
                        e.Item.Cells[6].Text = "시험";
                        e.Item.Cells[7].Text = "과제";
                        e.Item.Cells[8].Text = "총점";
                        e.Item.Cells[9].Text = "학습시작";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "open_course_id"; //key
                        e.Item.Cells[1].Text = "COURSE_TYPE_KEY";
                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "Course Type";
                        e.Item.Cells[4].Text = "Course Name";
                        e.Item.Cells[5].Text = "Progress";
                        e.Item.Cells[6].Text = "Exam";
                        e.Item.Cells[7].Text = "Report";
                        e.Item.Cells[8].Text = "Total Score";
                        e.Item.Cells[9].Text = "Learning";
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
        *                    C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grd1_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                DataRowView DRV = (DataRowView)e.Item.DataItem;
                Button brnPrint = ((Button)e.Item.FindControl("btnPrint"));
                //E-LEARNING 과정만 나타나도록 함 
                //if (Convert.ToString(DRV["COURSE_TYPE_KEY"].ToString()) == "000003")
                //    brnPrint.Visible = true;
                //else
                //    brnPrint.Visible = false;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        
        /************************************************************
        * Function name : grd1btn_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grd1btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button xbtn = (Button)sender;

                for (int i = 0; i < this.grd1.Items.Count; i++)
                {
                    Button xtemp = ((Button)((C1.Web.C1WebGrid.C1GridItem)this.grd1.Items[i]).FindControl("btnPrint"));
                    if (xbtn.UniqueID == xtemp.UniqueID)
                    {
                        //<input type = "button" value = "강의실" class = "btn_search" onclick = "javascript:openPopWindow('/mypage/study.aspx?open_course_id=<%# DataBinder.Eval(Container.DataItem, "OPEN_COURSE_ID")%>&MenuCode=<%=Session["MENU_CODE"]%>','study_win', '900', '610');" />  
                        string xScriptContent = string.Format("<script>openPopWindow('/mypage/study.aspx?open_course_id={0}&MenuCode={1}', 'study_win', '1024', '770');</script>", this.grd1.Items[i].Cells[0].Text, Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "study_win", xScriptContent);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
        *                 C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grd2_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                DataRowView DRV = (DataRowView)e.Item.DataItem;
                Button brnPrint = ((Button)e.Item.FindControl("btnPrint2"));
                //E-LEARNING 과정만 나타나도록 함 
                //if (Convert.ToString(DRV["COURSE_TYPE_KEY"].ToString()) == "000003")
                //    brnPrint.Visible = true;
                //else
                //    brnPrint.Visible = false;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : grd2btn_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grd2btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button xbtn = (Button)sender;

                for (int i = 0; i < this.grd2.Items.Count; i++)
                {
                    Button xtemp = ((Button)((C1.Web.C1WebGrid.C1GridItem)this.grd2.Items[i]).FindControl("btnPrint2"));
                    if (xbtn.UniqueID == xtemp.UniqueID)
                    {
                        //<input type = "button" value = "강의실" class = "btn_search" onclick = "javascript:openPopWindow('/mypage/study.aspx?open_course_id=<%# DataBinder.Eval(Container.DataItem, "OPEN_COURSE_ID")%>','study_win', '900', '610');" />              
                        string xScriptContent = string.Format("<script>openPopWindow('/mypage/study.aspx?open_course_id={0}&MenuCode={1}', 'study_win', '1024', '770');</script>", this.grd2.Items[i].Cells[0].Text, Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "study_win", xScriptContent);
                        break;
                    }
                }
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
        #region protected void PageNavigator1_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        protected void PageNavigator1_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        {
            try
            {
                this.CurrentPageIndex = e.PageIndex;
                PageInfo1.CurrentPageIndex = e.PageIndex;
                PageNavigator1.CurrentPageIndex = e.PageIndex;
                this.BindGrid1();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void grd2_ItemCreated(object sender, C1ItemEventArgs e)
        protected void grd2_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "open_course_id"; //key
                        e.Item.Cells[1].Text = "COURSE_TYPE_KEY";
                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "교육유형";
                        e.Item.Cells[4].Text = "과정명";
                        e.Item.Cells[5].Text = "진도";
                        e.Item.Cells[6].Text = "시험";
                        e.Item.Cells[7].Text = "과제";
                        e.Item.Cells[8].Text = "총점";
                        e.Item.Cells[9].Text = "수료";
                        e.Item.Cells[10].Text = "복습";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "open_course_id"; //key
                        e.Item.Cells[1].Text = "COURSE_TYPE_KEY";
                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "Course Type";
                        e.Item.Cells[4].Text = "Course Name";
                        e.Item.Cells[5].Text = "Progress";
                        e.Item.Cells[6].Text = "Exam";
                        e.Item.Cells[7].Text = "Report";
                        e.Item.Cells[8].Text = "Total Score";
                        e.Item.Cells[9].Text = "Completion";
                        e.Item.Cells[10].Text = "Review";
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
        protected void PageNavigator2_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        {
            try
            {
                this.CurrentPageIndex = e.PageIndex;
                PageInfo2.CurrentPageIndex = e.PageIndex;
                PageNavigator2.CurrentPageIndex = e.PageIndex;
                this.BindGrid2();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : BindGrid
        * Purpose       : 수강중인과정 (Taking Course) 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void BindGrid1()
        private void BindGrid1()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                //if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")            
                // 조회조건이 있을 경우 처리
                xParams = new string[3];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                xParams[2] = Session["USER_ID"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_lecture_md",
                                             "GetTakingCourseList",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                
                this.grd1.DataSource = xDt;
                this.grd1.DataBind();

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
        * Function name : BindGrid
        * Purpose       : 수강완료과정 (Completed Course)
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void BindGrid2()
        private void BindGrid2()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                //if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")            
                // 조회조건이 있을 경우 처리
                xParams = new string[3];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                xParams[2] = Session["COM_SNO"].ToString(); //공통사번

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_lecture_md",
                                             "GetCompletedCourseList",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);


                this.grd2.DataSource = xDt;
                this.grd2.DataBind();

                if (xDt.Rows.Count < 1)
                {
                    this.PageInfo2.TotalRecordCount = 0;
                    this.PageInfo2.PageSize = this.PageSize;
                    this.PageNavigator2.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo2.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    this.PageInfo2.PageSize = this.PageSize;
                    this.PageNavigator2.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
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
        * Function name : tabctrl_Click
        * Purpose       : panel tab click 시 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void tabctrl_Click(object sender, EventArgs e)
        protected void tabctrl_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton link = (LinkButton)sender;
                if (link == null)
                {
                    link = this.linkLecturing;
                }

                if (link == linkLecturing)
                {
                    this.BindGrid1();

                    linkLecturing.CssClass = "current";
                    linkLectured.CssClass = "";

                    pnlLecturing.Visible = true;
                    pnlLectured.Visible = false;

                    pnlLecturing.CssClass = "tab-content current";
                    pnlLectured.CssClass = "tab-content";

                    //this.pnlLecturing.Attributes["class"] = "tab-content current";
                    //this.pnlLectured.Attributes["class"] = "tab-content";

                    //ScriptHelper.ScriptBlock(this, "showTab", "<script>$(document).ready(function(){$('#tab_1').trigger('click');});</script>");
                }
                else
                {
                    this.BindGrid2();

                    linkLecturing.CssClass = "";
                    linkLectured.CssClass = "current";

                    this.pnlLecturing.Visible = false;
                    this.pnlLectured.Visible = true;

                    pnlLecturing.CssClass = "tab-content";
                    pnlLectured.CssClass = "tab-content current";

                    //this.pnlLecturing.Attributes["class"] = "tab-content";
                    //this.pnlLectured.Attributes["class"] = "tab-content current";

                    //ScriptHelper.ScriptBlock(this, "showTab", "<script>$(document).ready(function(){$('#tab_2').trigger('click');});</script>");
                }
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