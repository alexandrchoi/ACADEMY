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

namespace CLT.WEB.UI.LMS.MAIN
{
    /// <summary>
    /// 1. 작업개요 : SetMenu Class
    /// 
    /// 2. 주요기능 : 메뉴코드 Session 담는 처리
    ///				  
    /// 3. Class 명 : SetMenu
    /// 
    /// 4. 작 업 자 : 
    /// </summary>
    public partial class SetMenu : BasePage
    {
        protected string iGoUrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string xMenuCode1 = Request["menucode1"].ToString();

                // 대분류 메뉴를 선택했을 경우, 첫번째 중분류-소분류 메뉴로 가도록 처리
                string xMenuCode2 = (Request["menucode2"].ToString() == "0") ? "1" : Request["menucode2"].ToString();
                string xMenuCode3 = (Request["menucode3"].ToString() == "0") ? "1" : Request["menucode3"].ToString();
                //string xMenuCode2 = Request["menucode2"].ToString();
                //string xMenuCode3 = Request["menucode3"].ToString();

                this.iGoUrl = Request["gourl"];

                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                               "GetMenuAuthority",
                                               LMS_SYSTEM.MANAGE,
                                               "CLT.WEB.UI.COMMON.BASE.BasePage", xMenuCode1 + xMenuCode2 + xMenuCode3);

                

                Session["MENU_CODE1"] = xMenuCode1; // 대분류 메뉴
                Session["MENU_CODE2"] = xMenuCode2; // 중분류 메뉴
                if (xDt.Rows.Count == 0)
                    Session["MENU_CODE3"] = Request["menucode3"].ToString();
                else
                    Session["MENU_CODE3"] = xMenuCode3; // 소분류 메뉴

                Session["MENU_CODE"] = Session["MENU_CODE1"].ToString() +""+ Session["MENU_CODE2"].ToString() +""+ Session["MENU_CODE3"].ToString();
                if (Session["MENU_CODE"].ToString().Length != 3) Session["MENU_CODE"] = "000";

                if (!string.IsNullOrEmpty(this.iGoUrl))
                    Response.Write(this.iGoUrl);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex); 
            }
        }
    }
}
