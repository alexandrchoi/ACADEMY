using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

namespace CLT.WEB.UI.FX.UTIL
{
    /// <summary>
    /// 1. 작업개요 : FileHelper Class
    /// 
    /// 2. 주요기능 : 파일관련 처리를 위한 클래스
    ///				  
    /// 3. Class 명 : FileHelper
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.29
    /// </summary>
    public class FileHelper
    {
        /************************************************************
        * Function name : FileDownload
        * Purpose       : 브라우저를 통해 파일을 다운로드하는 처리
        * Input         : Page rPage, string rFilePath
        * Output        : bool
        *************************************************************/
        public static bool FileDownload(Page rPage, string rFilePath)
        {
            rFilePath = rPage.Server.MapPath(rFilePath);
            FileInfo xFileInfo = new FileInfo(rFilePath);

            if (!xFileInfo.Exists)
            {
                return false;
            }
            else
            {
                string xFileNM = Path.GetFileNameWithoutExtension(rFilePath) + new FileInfo(rFilePath).Extension;

                // 파일 다운로드
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpContext.Current.Server.UrlEncode(xFileNM.Replace(" ", "_")));
                HttpContext.Current.Response.AppendHeader("Content-Length", xFileInfo.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/unknown";
                HttpContext.Current.Response.CacheControl = "public";
                HttpContext.Current.Response.WriteFile(rFilePath);

                HttpContext.Current.Response.End();

                return true;
            }
        }
    }
}
