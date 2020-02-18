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
using CLT.WEB.UI.COMMON.BASE;
using System.Data.OleDb;
using System.IO;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.FX.AGENT;
using System.Threading;
using C1.Web.C1WebGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.MANAGE
{
    /// <summary>
    /// 1. 작업개요 : 소속회사 선택 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 소속회사 선택
    ///				  
    /// 3. Class 명 : company_select
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    /// 
    public partial class company_select : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 웹페이지 Load 이벤트
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
                    ScriptHelper.ScriptBlock(this, "company_select", xScriptMsg);

                    return;
                }

                // List 페이지로부터 opener_textZipCode_id, opener_Addr1_id를 전달받은 경우만 해당 페이지를 처리하고
                // 그렇지 않은 경우, 메세지 출력과 함께 창을 종료한다.
                if ((Request.QueryString["opener_textCompany_NM"] != null && Request.QueryString["opener_textCompany_NM"].ToString() != "") || (Request.QueryString["opener_Company_id"] != null && Request.QueryString["opener_Company_id"].ToString() != ""))
                {
                    ViewState["opener_textCompany_NM"] = Request.QueryString["opener_textCompany_NM"].ToString();
                    ViewState["opener_Company_id"] = Request.QueryString["opener_Company_id"].ToString();
                    BeginGrid();
                }
                else
                {
                    string xScriptContent = "<script>alert('잘못된 경로를 통해 접근하였습니다.');self.close();</script>";
                    ScriptHelper.ScriptBlock(this, "company_select", xScriptContent);
                }
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : search_OnClick
        * Purpose       : 웹페이지 검색버튼 클릭 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void search_OnClick(object sender, EventArgs e)
        {
            try
            {
                BeginGrid();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        
        protected void txtZipcode_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                BeginGrid();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        public void BeginGrid()
        {
            try
            {
                string[] xParams = new string[1];

                //if (string.IsNullOrEmpty(txtZipcode.Text))
                //{
                //    Response.Write("<script language='javascript'>alert('검색할 회사명을 입력해 주세요!');</script>");
                //    return;
                //}
                if (Request.QueryString["USERGROUP"].ToString() != "000001") // 관리자가 아니면
                {
                    xParams[0] = Request.QueryString["COMPANY_ID"].ToString();
                }
                else
                    xParams[0] = txtZipcode.Text;

                // 소속회사 검색
                DataTable xdt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                                 "GetCompanySearch",
                                                 LMS_SYSTEM.MANAGE,
                                                 "CLT.WEB.UI.LMS.MANAGE",
                                                 (object)xParams);

                C1WebGrid1.DataSource = xdt;
                C1WebGrid1.DataBind();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

    }
}
