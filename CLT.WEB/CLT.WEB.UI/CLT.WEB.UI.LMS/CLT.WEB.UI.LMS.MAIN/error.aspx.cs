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
    public partial class error : BasePage
    {
        // Inner Exception 보기 설정
        protected string DEBUG_MODE
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DebugMode"].ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Convert.ToString(Session["ERR"]) != "")
            {
                //this.txtError.Text = Convert.ToString(Session["ERR"]);

                string xMsg = Convert.ToString(Session["ERR"]);
                int xIndex = xMsg.IndexOf("--INNER EXCEPTION--");



                this.txtError.Text = xMsg.Substring(0, xIndex).Trim();

                this.txtError2.Visible = false;
                this.btnDetail.Visible = false;
                td1.Height = "70px";

                if (DEBUG_MODE == Boolean.TrueString.ToUpper())
                {
                    this.txtError2.Text = Convert.ToString(Session["ERR"]);
                    this.btnDetail.Visible = true;
                }
            }
        }

        protected void Button_OnClick(object seder, EventArgs e)
        {
            this.txtError2.Visible = true;
        }
    }
}
