using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace CLT.WEB.UI.FX.UTIL
{
    public class Util
    {
        #region public static bool IsNullOrEmptyObject(object inObject)
        /************************************************************
        *  Function name : IsNullOrEmptyObject
        *  Purpose       : Null 체크함수
        *  Input         : 공용
        *  Output        : bool
        **************************************************************/
        public static bool IsNullOrEmptyObject(object inObject)
        {
            if (inObject == null || inObject == DBNull.Value || inObject.ToString().Trim() == "" || inObject.ToString().Trim() == "undefined")
                return true;
            return false;
        }
        #endregion

        #region public static string GetFileName(object rFineName)
        /************************************************************
        *  Function name : GetFileName
        *  Purpose       : 파일이름 가져오기
        *  Input         : 공용
        *  Output        : string
        **************************************************************/
        public static string GetFileName(object rFineName)
        {
            if (IsNullOrEmptyObject(rFineName))
                return "";

            string xRtn = "";
            string[] fileNames = Convert.ToString(rFineName).Split('(');
            if (fileNames.Length > 0)
            {
                for (int i = 0; i < fileNames.Length-1; i++)
                {
                    xRtn += fileNames[i];
                }
                return xRtn;
            }
            else
            {
                return Convert.ToString(rFineName).Split('(')[0];
            }
        }
        #endregion

        #region public static string ConvertToString(object rObject)
        /************************************************************
        *  Function name : ConvertToString
        *  Purpose       : object -> string
        *  Input         : 공용
        *  Output        : 
        **************************************************************/
        public static string ConvertToString(object rObject)
        {
            if (IsNullOrEmptyObject(rObject))
                return "";
            return rObject.ToString().Trim();
        }
        #endregion

        #region public static string[] Split(string content, string seperator)
        /************************************************************
        *  Function name : Split
        *  Purpose       : Split 함수
        *  Input         : 공용
        *  Output        : 
        **************************************************************/
        public static string[] Split(string content, string seperator)
        {
            return content.Split(new char[] { Convert.ToChar(seperator) });
        }
        #endregion

        #region public static string[] Split(string content, string seperator, int arrayLenth)
        /************************************************************
        *  Function name : Split
        *  Purpose       : Spilt 함수
        *  Input         : 공용
        *  Output        : 
        **************************************************************/
        public static string[] Split(string content, string seperator, int arrayLenth)
        {
            string[] arraryResult = new string[arrayLenth];

            if (!string.IsNullOrEmpty(content))
            {
                string[] arrarySplit = Split(content, seperator);
                if (arrarySplit.Length > arrayLenth)
                    arraryResult = new string[arrarySplit.Length];

                if (arrarySplit.Length < arrayLenth)
                    Array.Copy(arrarySplit, arraryResult, arrarySplit.Length);
                else
                    Array.Copy(arrarySplit, arraryResult, arrayLenth);
            }

            //null 처리
            for (int i = 0; i < arrayLenth; i++)
            {
                if (string.IsNullOrEmpty(arraryResult[i]))
                    arraryResult[i] = "";
            }
            
            return arraryResult;
        }
        #endregion

        #region public static string ForbidText(string content, string gubun)
        /************************************************************
        *  Function name : ForbidText
        *  Purpose       : Request 공격 방어 함수
        *  Input         : 공용
        *  Output        : 
        **************************************************************/
        public static string ForbidText(string content, string gubun)
        {
            string result = content;
            if (!string.IsNullOrEmpty(result))
            {
                if (gubun.ToLower() == "request")
                {
                    //result = result.Replace("%", "o/o");
                    result = result.Replace(";", "ː");
                    result = Regex.Replace(result, "style", "xtyle", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, "body", "xody", RegexOptions.IgnoreCase);
                }
                else if (gubun.ToLower() == "rss")
                {
                    //result = result.Replace("%", "o/o");
                    result = result.Replace(";", "ː");
                    result = Regex.Replace(result, "style", "xtyle", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, "body", "xody", RegexOptions.IgnoreCase);
                }

                //디비저장시 에러나므로 꼭 사용함
                result = result.Replace("'", " ");
                result = result.Replace("--", "__");
                result = result.Replace("<%", "<ː");
                result = result.Replace("%>", "ː>");
                result = Regex.Replace(result, "declare", "xeclare", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "cast", "xast", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "nvarchar", "xvarchar", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "varchar", "xarchar", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "meta", "xeta", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "htc", "xtc", RegexOptions.IgnoreCase);

                // XSS 공격방지
                result = Regex.Replace(result, "script", "xcript", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "iframe", "xframe", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "alert", "xlert", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "window.open", "xindow.open", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "location", "xocation", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "cookie", "xookie", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "session", "xession", RegexOptions.IgnoreCase);
            }

            return result;
        }
        #endregion

        #region public static string ForbidText(string content, string gubun)
        /************************************************************
        *  Function name : ForbidText
        *  Purpose       : Request 공격 방어 함수
        *  Input         : 공용
        *  Output        : 
        **************************************************************/
        public static string ForbidText(string content)
        {
            return ForbidText(content, "request");
        }
        #endregion

        public enum RequestOption
        {
            HtmlCheckBox,
            RSS,
            Blank
        }

        /// <summary>
        /// Request
        /// </summary>
        /// <param name="RequestKey"></param>
        /// <returns>Request</returns>
        public static string Request(string RequestKey)
        {
            return Util.ForbidText(System.Web.HttpContext.Current.Request[RequestKey], "request");
        }

        /// <summary>
        /// Request
        /// </summary>
        /// <param name="RequestKey"></param>
        /// <returns>Request</returns>
        public static string Request(string RequestKey, Util.RequestOption option)
        {
            if (option == Util.RequestOption.HtmlCheckBox)
            {
                string splitStr = Util.Split(Request(RequestKey), ",", 2)[0];
                if (string.IsNullOrEmpty(splitStr))
                    return "false";
                return "true";
            }
            else if (option == Util.RequestOption.RSS)
                return Util.ForbidText(System.Web.HttpContext.Current.Request[RequestKey], "rss");
            else if (option == Util.RequestOption.Blank)
                return Util.ForbidText(System.Web.HttpContext.Current.Request[RequestKey], "");
            else
                return Util.ForbidText(System.Web.HttpContext.Current.Request[RequestKey], "request");

        }


        /************************************************************
        *  Function name : 요일변환함수
        *  Purpose       : 
        *  Input         : 
        *  Output        : 
        **************************************************************/
        public static string GetDayOfWeek(DateTime dateTime)
        {
            DayOfWeek d = dateTime.DayOfWeek;

            string ret = string.Empty;

            switch (d)
            {
                case DayOfWeek.Friday:
                    ret = "금";
                    break;
                case DayOfWeek.Monday:
                    ret = "월";
                    break;
                case DayOfWeek.Saturday:
                    ret = "토";
                    break;
                case DayOfWeek.Sunday:
                    ret = "일";
                    break;
                case DayOfWeek.Thursday:
                    ret = "목";
                    break;
                case DayOfWeek.Tuesday:
                    ret = "화";
                    break;
                case DayOfWeek.Wednesday:
                    ret = "수";
                    break;
                default:
                    break;
            }

            return ret;

        }

        public static string UploadGetFileName(FileUpload fileControl, string saveFolder)
        {
            saveFolder = HttpContext.Current.Server.MapPath(saveFolder);
            string postFilePath = fileControl.PostedFile.FileName;
            if (string.IsNullOrEmpty(postFilePath))
            {
                return null;
            }
            else
            {
                FileInfo upFile = new FileInfo(postFilePath);
                string physicalPath = saveFolder + "\\" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + upFile.Name;

                // 업로드 보안사항 체크
                if (IsOkCheckSecurityUpload(physicalPath))
                {
                    // 폴더가 존재하지 않으면 생성
                    if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);

                    // 파일저장
                    fileControl.PostedFile.SaveAs(physicalPath);

                    return physicalPath;
                }
                else
                {
                    return null;
                }
            }
        }

        // 엑셀파일 데이터 테이블로 변경
        public static DataTable GetDtExcelFile(string excelFilePath, bool isExcelVersion2007)
        {
            string strProvider = string.Empty;
            //xlsx 2007 Office system 드라이버: 데이터연결 구성요소 서버에 설치해야함
            if (isExcelVersion2007)
                strProvider = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + excelFilePath + "; Extended Properties=Excel 12.0";
            else
                strProvider = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + excelFilePath + "; Extended Properties=Excel 8.0;";

            OleDbConnection excelConnection = null;
            OleDbCommand dbCommand = null;
            OleDbDataAdapter dataAdapter = null;
            DataTable dTable = new DataTable();
            try
            {
                excelConnection = new OleDbConnection(strProvider);
                excelConnection.Open();

                string strSQL = "SELECT * FROM [Sheet1$]";
                dbCommand = new OleDbCommand(strSQL, excelConnection);
                dataAdapter = new OleDbDataAdapter(dbCommand);
                dataAdapter.Fill(dTable);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                dTable.Dispose();
                if (dataAdapter != null)
                    dataAdapter.Dispose();
                if (dbCommand != null)
                    dbCommand.Dispose();
                if (excelConnection != null)
                {
                    excelConnection.Close();
                    excelConnection.Dispose();
                }
            }

            return dTable;
        }

        public static bool IsOkCheckSecurityUpload(string savePhysicalPath)
        {
            string saveFileExtension = Path.GetExtension(savePhysicalPath).ToLower(); // 확장자명
            string[] forbidExtensions = new string[] { ".asp", ".aspx", ".cs", ".vb", ".vbs", ".cgi", "html", ".htm" }; // 업로드 금지 확장자

            // 확장자 체크
            if (Array.IndexOf(forbidExtensions, saveFileExtension) != -1)
            {
                HttpContext.Current.Response.Write("<script>alert('실행 가능한 파일은 업로드 할 수 없습니다.');</script>");
                return false;
            }
            return true;
        }

        public static void FileDel(string fullFilePath, bool isWebPath)
        {
            if (isWebPath)
                fullFilePath = HttpContext.Current.Server.MapPath(fullFilePath);

            try
            {
                FileInfo saveFile = new FileInfo(fullFilePath);
                if (saveFile.Exists)
                {
                    GC.Collect();
                    saveFile.Delete();
                }
            }
            catch
            {

            }
        }
    }
}
