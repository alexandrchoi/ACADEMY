using System;
using System.Collections.Generic;
using System.Text;

using System.Web.Security;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CLT.WEB.UI.FX.UTIL
{
    /// <summary>
    /// 1. 작업개요 : ScriptHelper Class
    /// 
    /// 2. 주요기능 : 코드 비하인드 구현 부분에서 Client Script를 사용하기 위한 처리
    ///				  
    /// 3. Class 명 : ScriptHelper
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.27
    /// </summary>
    public class ScriptHelper
    {
        /************************************************************
        * Function name : Page_Alert
        * Purpose       : 특정 페이지에 alert 메세지 창을 띄우기 위한 처리
        * Input         : Page rPage, string rContent
        * Output        : void
        *************************************************************/
        public static void Page_Alert(Page rPage, string rContent)
        {
            string xScript = null;
            xScript = "<script type='text/javascript' language='javascript'>";
            xScript += string.Format("alert('{0}');", rContent);
            xScript += "</script>";

            rPage.ClientScript.RegisterClientScriptBlock(rPage.GetType(), "alert", xScript);
        }
        /************************************************************
        * Function name : Page_Alert
        * Purpose       : 특정 페이지에 alert 메세지 창을 띄우기 위한 처리
        * Input         : Page rPage, string rContent
        * Output        : void
        *************************************************************/
        public static void Page_AlertClose(Page rPage, string rContent)
        {
            string xScript = null;
            xScript = "<script type='text/javascript' language='javascript'>";
            xScript += string.Format("alert('{0}');", rContent);
            xScript += "self.close();";
            xScript += "</script>";

            rPage.ClientScript.RegisterClientScriptBlock(rPage.GetType(), "alert", xScript);
        }

        /************************************************************
        * Function name : Location
        * Purpose       : 특정 페이지의 url을 변경하기 위한 처리
        * Input         : Page rPage, string rUrl
        * Output        : void
        *************************************************************/
        public static void Location(Page rPage, string rUrl)
        {
            string xScript = null;
            xScript = "<script type='text/javascript' language='javascript'>";
            xScript += string.Format("location.href='{0}';", rUrl);
            xScript += "</script>";

            rPage.ClientScript.RegisterClientScriptBlock(rPage.GetType(), "location", xScript);
        }

        /************************************************************
        * Function name : OpenPopWindow
        * Purpose       : 특정 페이지에서 Popup 창을 띄우기 위한 처리
        * Input         : Page rPage, string rUrl, string rScriptKey, string rWidth, string rHeight
        * Output        : void
        *************************************************************/
        public static void OpenPopWindow(Page rPage, string rUrl, string rScriptKey, string rWidth, string rHeight)
        {
            string xScript = null;
            xScript = "<script type='text/javascript' language='javascript'>";
            xScript += string.Format("openPopWindow('{0}', '{1}', '{2}', '{3}');", rUrl, rScriptKey, rWidth, rHeight);
            xScript += "</script>";

            rPage.ClientScript.RegisterClientScriptBlock(rPage.GetType(), rScriptKey, xScript);
        }

        /************************************************************
        * Function name : ScriptBlock
        * Purpose       : 특정 페이지에[ <script> ~~ </script> ] 블록 전체를 등록하는 처리
        * Input         : Page rPage, string rUrl, string rScriptKey, string rWidth, string rHeight
        * Output        : void
        *************************************************************/
        public static void ScriptBlock(Page rPage, string rScriptKey, string rScript)
        {
            rPage.ClientScript.RegisterClientScriptBlock(rPage.GetType(), rScriptKey, rScript);
        }
        /************************************************************
        * Function name : ScriptBlock
        * Purpose       : 특정 페이지에[ <script> ~~ </script> ] 블록 전체를 등록하는 처리
        * Input         : Page rPage, string rUrl, string rScriptKey, string rWidth, string rHeight
        * Output        : void
        *************************************************************/
        public static void ScriptStartup(Page rPage, string rScriptKey, string rScript)
        {
            rPage.ClientScript.RegisterStartupScript(rPage.GetType(), rScriptKey, rScript);
        }
    }
}
