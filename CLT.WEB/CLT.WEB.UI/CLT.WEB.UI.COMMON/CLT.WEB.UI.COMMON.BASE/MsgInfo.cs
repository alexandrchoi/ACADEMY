using System;
using System.Collections.Generic;
using System.Text;
//
using System.Xml;
using System.Globalization;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Web;

namespace CLT.WEB.UI.COMMON.BASE
{
    public class MsgInfo
    {
        private static XmlDocument iMsg = null;
        private static string iDirectoryPath = null;

        public static void SetDirectoryPath(string rDirectoryPath)
        {
            iDirectoryPath = rDirectoryPath;
        }

        public static string GetMsg(string rMsgCode, string[] rMsgKor, string[] rMsgEng, CultureInfo rArgCultureInfo)
        {
            string xExpression = null;
            string xRtnStr = null;
            XmlNodeList xNodes = null;

            iMsg = null;

            try
            {
                if (iMsg == null)
                {
                    iMsg = new XmlDocument();
                    System.IO.DirectoryInfo xDic = new System.IO.DirectoryInfo(iDirectoryPath);
                    iMsg.Load(xDic.FullName + @"\Msg_Defs.xml");
                    //iMsg.Load(SystemSettings.GET_CST_MSG_LOADPATH + @"/Msg_Defs.xml");
                }

                xExpression = @"//MSG[@ID='" + rMsgCode + "']/";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xExpression += "KO";
                }
                else
                {
                    xExpression += "EN";
                }
                xNodes = iMsg.SelectNodes(xExpression);

                if (xNodes.Count > 0)
                {
                    xRtnStr = xNodes[0].InnerText.Replace(@"\r\n", Environment.NewLine);
                }
                else
                {
                    xRtnStr = "Application message cannot be delivered. Please report to system administrator";
                }

                xRtnStr = string.Format(xRtnStr, rArgCultureInfo.Name.ToLower() == "ko-kr" ? rMsgKor : rMsgEng);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }

            return xRtnStr;
        }
    }
}
