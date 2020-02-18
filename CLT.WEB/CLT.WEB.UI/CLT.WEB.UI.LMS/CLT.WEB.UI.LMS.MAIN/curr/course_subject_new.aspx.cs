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
using System.Threading; 

using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading; 

namespace CLT.WEB.UI.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : course_subject_new Class
    /// 
    /// 2. 주요기능 : subject와 contents를 연계하여 데이터 생성하는 화면 
    ///				  
    /// 3. Class 명 : course_subject_new
    /// 
    /// 4. 작 업 자 : 최인재 / 2012.01.26
    /// </summary>
    public partial class course_subject_new : BasePage
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

                ClientScript.RegisterStartupScript(this.GetType(), "setLoad", "<script language='javascript'>setLoad();</script>");
                base.pRender(this.Page, new object[,] { { this.btnDown, "I" },{ this.btnRemove, "D" }, { this.btnTemp, "E" }, { this.btnUp, "I" } }, Convert.ToString(Request.QueryString["MenuCode"]));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
       * Function name : btnRemove_Click
       * Purpose       : 
       * Input         : void
       * Output        : void
       *************************************************************/
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.lstItemList.Items.Count; i++)
                {
                    if (this.lstItemList.Items[i].Selected)
                    {
                        this.lstItemList.Items.Remove(this.lstItemList.Items[i].Text);
                        i--;
                    }
                }

                this.lblItemsCount.Text = this.lstItemList.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

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
                if (this.txtSubjectId.Value == string.Empty)
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
                    string xlstContents = string.Empty;

                    string[] xItem = null;
                    for (int i = 0; i < this.lstItemList.Items.Count; i++)
                    {
                        xItem = this.lstItemList.Items[i].Text.Split('|');
                        xlstContents += xItem[1].Trim() + "|";
                    }

                    string[] xParams = new string[2];
                    xParams[0] = this.txtSubjectId.Value;
                    xParams[1] = xlstContents;

                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                    "SetSubjectContentsInfo",
                                                    LMS_SYSTEM.CURRICULUM,
                                                    "CLT.WEB.UI.LMS.CURR", (object)xParams);

                    if (xRtn != string.Empty)
                    {
                        //A001: {0}이(가) 저장되었습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                          new string[] { "과목&컨텐츠" },
                                                          new string[] { "Subject&Contents" },
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
                                                          new string[] { "과목&컨텐츠" },
                                                          new string[] { "Subject&Contents" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));

                    }
                }
                else
                {
                    //A012: {0}의 필수 항목 입력이 누락되었습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                      new string[] { "과목&컨텐츠" },
                                                      new string[] { "Subject&Contents" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
       * Function name : LnkBtnSetItem_Click
       * Purpose       : 
       * Input         : void
       * Output        : void
       *************************************************************/
        #region protected void LnkBtnSetItem_Click(object sender, EventArgs e)
        protected void LnkBtnSetItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] items = this.HidLstItems.Value.Split('凸');
                bool xSel = false;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != string.Empty)
                    {
                        xSel = false;
                        for (int k = 0; k < this.lstItemList.Items.Count; k++)
                        {
                            if (items[i] == this.lstItemList.Items[k].Value)
                            {
                                xSel = true;
                                break;
                            }
                        }
                        if (xSel == false)
                            this.lstItemList.Items.Add(items[i]);
                    }
                }

                this.lblItemsCount.Text = this.lstItemList.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


        /************************************************************
      * Function name : LnkBtnSubjectidChange_Click
      * Purpose       : 
      * Input         : void
      * Output        : void
      *************************************************************/
        #region protected void LnkBtnSubjectidChange_Click(object sender, EventArgs e)
        protected void LnkBtnSubjectidChange_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable xDt = null;

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_course_md",
                                                 "GetSubjectContentsList",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", this.txtSubjectId.Value);

                foreach (DataRow dr in xDt.Rows)
                {
                    this.lstItemList.Items.Add(dr[0].ToString());
                }

                this.lblItemsCount.Text = this.lstItemList.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion


    }
}
