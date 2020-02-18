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

using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading;

namespace CLT.WEB.UI.LMS.COMMON
{
    public partial class file_download_blob : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "file_download_blob", xScriptMsg);

                    return;
                }

                Response.Buffer = true;

                string xKind = Request["kind"];
                string xSeq = Request["seq"];
                string xFseq = Request["fseq"];
                string xFileName = Server.UrlDecode(Request["att_file_nm"]);

                byte[] xFile = (byte[])SBROKER.GetObject("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                 "GetAttFile",
                                                 LMS_SYSTEM.OTHERS,
                                                 "CLT.WEB.UI.LMS.COMMON"
                                                 , xKind
                                                 , xSeq
                                                 , xFseq
                                               );

                if (!Util.IsNullOrEmptyObject(xFile))
                {
                    Session.CodePage = 949;
                    Response.Charset = "UTF-8";

                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    Response.ContentType = "Application/UnKnown";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(xFileName.Replace(" ", "_").Replace("..", "_")));
                    Response.BinaryWrite(xFile);
                }

                Response.End();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
    }
}
