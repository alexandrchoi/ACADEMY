using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CLT.WEB.UI.FX.UTIL
{
    public class StringHelper
    {
        public static string AscToHtml(string rSource)
        {
            StringBuilder xSource = new StringBuilder(rSource);
            rSource.Replace("&", "&amp;");
            rSource.Replace(@"""", "&quot;");
            rSource.Replace("<", "&lt;");
            rSource.Replace(">", "&gt;");
            rSource.Replace(" ", "&nbsp;");
            rSource.Replace("" + (char)13 + (char)10, "<br>");

            return rSource.ToString();
        }

        public static string CurrentURL()
        {
            // return value : /Sample/Default.aspx
            return System.Web.HttpContext.Current.Request.ServerVariables["URL"].ToString();
        }

        public static string CurrentPageName()
        {
            string[] xSplitUrl = null;
            xSplitUrl = CurrentPageName().Split(new char[] { '/' });

            // Default.aspx
            return xSplitUrl[xSplitUrl.Length - 1];

        }

        public static string CutTitle(object rSource, int rCutLength)
        {
            string xReturn = rSource.ToString();
            if (xReturn.Length > rCutLength)
                xReturn = xReturn.Substring(0, rCutLength - 3) + "...";

            return xReturn;
        }

        // 파라미터 값이 정수인지 여부 판단
        public static bool IsNumber(string rValue)
        {
            try
            {
                if (rValue != null)
                {
                    Convert.ToInt32(rValue);
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        // 입력된 문자열에서 특정 패턴과 일치하는 문자를 지정된 문자로 변경하는 처리
        public static string ReplaceIgnoreCase(string rInput, string rPattern, string rReplacement)
        {
            return Regex.Replace(rInput, rPattern, rReplacement, RegexOptions.IgnoreCase);
        }

        // 글자수 잘라서 [....] 표현해주는 처리
        public static string ToTruncate(string rStr, int rLength)
        {
            string xRet = "";
            if (Util.IsNullOrEmptyObject(rStr))
                return "";

            if (rStr.Length > rLength)
                xRet = rStr.Substring(0, rLength) + "...";
            else
                xRet = rStr;

            return xRet;
        }
    }
}
