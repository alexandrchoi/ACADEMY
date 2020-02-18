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
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading;

namespace CLT.WEB.UI.LMS.COMMON
{
    /// <summary>
    /// 1. 작업개요 : file_download Class
    /// 
    /// 2. 주요기능 : 파일 다운로드 처리
    ///				  
    /// 3. Class 명 : file_download
    /// 
    /// 4. 작 업 자 : 
    /// </summary>
    public partial class file_download : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string xScript = "";
                string xFilePath = this.DownloadFilePath + Request["filepath"].ToString();
                bool xRtn = false;
                bool xChk = false;

                switch (xFilePath)
                {
                    case "/file/download/xlviewer-e.exe":
                    case "/file/download/ppviewer.exe":
                    case "/file/download/wdviewer.exe":
                    case "/file/download/HwpViewer.exe":
                    case "/file/download/AdbeRdr80_ko_KR.exe":
                        xChk = true;
                        break;
                }

                if (xChk)
                {
                    //result : true -> success, false -> failure
                    xRtn = FileHelper.FileDownload(this, xFilePath);

                    if (xRtn == false)
                    {
                        xScript = "<script type='text/javascript' language='javascript'>alert('파일이 존재하지 않습니다.');";
                        xScript += "history.back();";
                        xScript += "</script>";
                        //Response.Write("<script type='text/javascript' language='javascript'>alert('" + base.GetMsg("21", "Viewer File") + "');");

                        ScriptHelper.ScriptBlock(this, "file_download", xScript);
                    }
                }
                else
                {
                    xScript = "<script type='text/javascript' language='javascript'>";
                    xScript += "history.back();";
                    xScript += "</script>";

                    ScriptHelper.ScriptBlock(this, "file_download", xScript);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
    }
}
