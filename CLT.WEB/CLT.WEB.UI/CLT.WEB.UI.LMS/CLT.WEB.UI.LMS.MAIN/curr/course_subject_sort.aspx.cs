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
    /// 1. 작업개요 : course_subject_sort Class
    /// 
    /// 2. 주요기능 : subject sort 순서 정의 
    ///				  
    /// 3. Class 명 : course_subject_sort
    /// 
    /// 4. 작 업 자 : 김은정 / 2012.04.25
    /// </summary>
    public partial class course_subject_sort : BasePage
    {
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

                if (Request.QueryString["COURSE_ID"] != null && Request.QueryString["COURSE_ID"].ToString() != string.Empty)
                {
                    if (!IsPostBack)
                    {
                        ViewState["COURSE_ID"] = Request.QueryString["COURSE_ID"].ToString();
                        this.BindData();
                    }
                }
                else
                {
                    ViewState["COURSE_ID"] = string.Empty;
                }

                base.pRender(this.Page, new object[,] { { this.btnDown, "I" }, { this.btnTemp, "E" }, { this.btnUp, "I" } }, Convert.ToString(Request.QueryString["MenuCode"]));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }


        /******************************************************************************************
       * Function name : BindData
       * Purpose       : 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region private void BindData()
        private void BindData()
        {
            try
            {
                string[] xParams = new string[1];
                DataTable xDt = null;

                if (ViewState["COURSE_ID"].ToString() == string.Empty)
                    return;

                xParams[0] = ViewState["COURSE_ID"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                 "GetCourseSubjectList",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", ViewState["COURSE_ID"].ToString());

                foreach (DataRow dr in xDt.Rows)
                {
                    this.lstItemList.Items.Add(dr[0].ToString()); 
                }

                this.lblItemsCount.Text = this.lstItemList.Items.Count.ToString();
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion


        /************************************************************
       * Function name : btnUp_Click
       * Purpose       : 
       * Input         : void
       * Output        : void
       *************************************************************/
        #region protected void btnUp_Click(object sender, EventArgs e)
        protected void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedIdx = 0;
                int newIdx = 0;
                string strMove1 = string.Empty;
                string strMove2 = string.Empty;
                string strTmp = string.Empty;

                for (int i = 0; i < this.lstItemList.Items.Count; i++)
                {
                    if (this.lstItemList.Items[i].Selected)
                    {
                        selectedIdx = i;
                    }
                }

                if (selectedIdx > 0)
                {
                    newIdx = selectedIdx - 1;
                    strMove1 = this.lstItemList.Items[selectedIdx].Text;
                    strMove2 = this.lstItemList.Items[newIdx].Text;

                    strTmp = strMove1;
                    strMove1 = strMove2;
                    strMove2 = strTmp;

                    this.lstItemList.Items[selectedIdx].Text = strMove1;
                    this.lstItemList.Items[newIdx].Text = strMove2;

                    this.lstItemList.Items[selectedIdx].Selected = false;
                    this.lstItemList.Items[newIdx].Selected = true;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
       * Function name : btnDown_Click
       * Purpose       : 
       * Input         : void
       * Output        : void
       *************************************************************/
        #region protected void btnDown_Click(object sender, EventArgs e)
        protected void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedIdx = 0;
                int newIdx = 0;
                string strMove1 = string.Empty;
                string strMove2 = string.Empty;
                string strTmp = string.Empty;

                for (int i = 0; i < this.lstItemList.Items.Count; i++)
                {
                    if (this.lstItemList.Items[i].Selected)
                    {
                        selectedIdx = i;
                    }
                }

                if (selectedIdx < (this.lstItemList.Items.Count - 1))
                {
                    newIdx = selectedIdx + 1;
                    strMove1 = this.lstItemList.Items[selectedIdx].Text;
                    strMove2 = this.lstItemList.Items[newIdx].Text;

                    strTmp = strMove1;
                    strMove1 = strMove2;
                    strMove2 = strTmp;

                    this.lstItemList.Items[selectedIdx].Text = strMove1;
                    this.lstItemList.Items[newIdx].Text = strMove2;

                    this.lstItemList.Items[selectedIdx].Selected = false;
                    this.lstItemList.Items[newIdx].Selected = true;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion



        /******************************************************************************************
       * Function name : IsDataValidation
       * Purpose       : Data Validation check 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region private bool IsDataValidation()
        private bool IsDataValidation()
        {
            try
            {
                if (ViewState["COURSE_ID"] == null || ViewState["COURSE_ID"].ToString() == string.Empty)
                    return false; 

                if (this.lstItemList.Items.Count == 0)
                    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return true;
        }
        #endregion

        /************************************************************
       * Function name : btnSend_Click
       * Purpose       : 
       * Input         : void
       * Output        : void
       *************************************************************/
        protected void btnTemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.IsDataValidation())
                {
                    string xlstSubject = string.Empty;

                    string[] xItem = null;
                    for (int i = 0; i < this.lstItemList.Items.Count; i++)
                    {
                        xItem = this.lstItemList.Items[i].Text.Split('|');
                        xlstSubject += xItem[0].Trim() + "|";
                    }

                    string[] xParams = new string[2];
                    xParams[0] = ViewState["COURSE_ID"].ToString();
                    xParams[1] = xlstSubject;

                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                    "SetCourseSubjectSortInfo",
                                                    LMS_SYSTEM.CURRICULUM,
                                                    "CLT.WEB.UI.LMS.CURR", (object)xParams);

                    if (xRtn != string.Empty)
                    {
                        //A001: {0}이(가) 저장되었습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                          new string[] { "과목 정렬" },
                                                          new string[] { "Subject Sort" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));
                        ////저장 후 신규 id 값으로 재조회 
                        //ViewState["SUBJECT_ID"] = xRtn;
                        //this.BindData();
                    }
                    else
                    {
                        //A004: {0}이(가) 입력되지 않았습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004",
                                                          new string[] { "과목 정렬" },
                                                          new string[] { "Subject Sort" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));

                    }
                }
                else
                {
                    //A012: {0}의 필수 항목 입력이 누락되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                      new string[] { "과목 정렬" },
                                                      new string[] { "Subject Sort" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }



    }
}
