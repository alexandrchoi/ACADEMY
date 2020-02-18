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

namespace CLT.WEB.UI.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : SMS 발송 Class
    /// 
    /// 2. 주요기능 : LMS SMS 발송 화면
    ///				  
    /// 3. Class 명 : sms_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class sms_list : BasePage
    {
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
                    //this.txtSMS_From.Attributes.Add("onkeyup", "ChkDate(this);");
                    //this.txtSMS_To.Attributes.Add("onkeyup", "ChkDate(this);");

                    base.pRender(this.Page, new object[,] { { this.btnRetrieve, "I" } });

                    if (Session["USER_GROUP"].ToString() == "000009")
                    {
                        //return;
                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "sms_list", xScriptMsg);
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
        #region protected void btnRetrieve_Click(object sender, EventArgs e)
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
                        e.Item.Cells[0].Text = "제목";
                        e.Item.Cells[1].Text = "전송일시";
                        e.Item.Cells[2].Text = "수신처";
                        e.Item.Cells[3].Text = "성공";
                        e.Item.Cells[4].Text = "실패";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "Subject";
                        e.Item.Cells[1].Text = "Sent";
                        e.Item.Cells[2].Text = "Recipient";
                        e.Item.Cells[3].Text = "Success";
                        e.Item.Cells[4].Text = "Failure";
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
        * Purpose       : SMS 전송 리스트를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region public void BindGrid()
        public void BindGrid()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSMS_From.Text) && string.IsNullOrEmpty(txtSMS_To.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "조회기간" }, new string[] { "Date To" }, Thread.CurrentThread.CurrentCulture)); return;
                }
                else if (string.IsNullOrEmpty(txtSMS_From.Text) && !string.IsNullOrEmpty(txtSMS_To.Text))
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "조회기간" }, new string[] { "Date From" }, Thread.CurrentThread.CurrentCulture)); return;
                }

                string[] xParams = new string[5];

                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();

                if (rbBooking.Checked == true) // 예약발송일 경우
                    xParams[2] = "Y";

                else
                    xParams[2] = "N";

                xParams[3] = this.txtSMS_From.Text;
                xParams[4] = this.txtSMS_To.Text;

                DataTable xDt = new DataTable();
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_m_sms_md",
                                       "GetSMSListMaster",
                                       LMS_SYSTEM.APPLICATION,
                                       "CLT.WEB.UI.LMS.APPLICATION", (object)xParams);

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
        * Function name : btnNew_OnClick
        * Purpose       : SMS 작성 Click 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnNew_OnClick(object sender, EventArgs e)
        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            try
            {
                string xScriptMsg = string.Format("<script>window.location.href='/application/sms_send.aspx?MenuCode={0}';</script>", Session["MENU_CODE"]);
                ScriptHelper.ScriptBlock(this, "sms_send", xScriptMsg);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

    }
}