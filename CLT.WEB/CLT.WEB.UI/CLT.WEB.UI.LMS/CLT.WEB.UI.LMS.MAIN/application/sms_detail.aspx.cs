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

namespace CLT.WEB.UI.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : SMS 발송 Class
    /// 
    /// 2. 주요기능 : LMS SMS 발송 화면
    ///				  
    /// 3. Class 명 : sms_detail
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class sms_detail : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 페이지 Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "sms_detail", xScriptMsg);

                    return;
                }

                if (Request.QueryString["rSMSGroupNo"] != null && Request.QueryString["rSMSGroupNo"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        //this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                        BIndGrid();
                    }
                }
                else
                {
                    return;
                    //string xScriptContent = "<script>alert('잘못된 경로를 통해 접근하였습니다.');self.close();</script>";
                    //ScriptHelper.ScriptBlock(this, "sms_detail", xScriptContent);
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
        protected void grd_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "수신자명";
                        e.Item.Cells[1].Text = "수신번호";
                        e.Item.Cells[2].Text = "전송일시";
                        e.Item.Cells[3].Text = "결과";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "Recipient";
                        e.Item.Cells[1].Text = "Mobile";
                        e.Item.Cells[2].Text = "Sent";
                        e.Item.Cells[3].Text = "Status";
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
        public void BIndGrid()
        {
            try
            {
                string[] xParams = new string[1];

                xParams[0] = Request.QueryString["rSMSGroupNo"].ToString();

                DataTable xDt = new DataTable();
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_m_sms_md",
                           "GetSMSListDetail",
                           LMS_SYSTEM.APPLICATION,
                           "CLT.WEB.UI.LMS.APPLICATION", (object)xParams);

                this.txtContent.Text = xDt.Rows[0]["sent_sms"].ToString();

                this.C1WebGrid1.DataSource = xDt;
                this.C1WebGrid1.DataBind();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

    }
}
