using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using System.Web.Security;
using System.Globalization;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
//using CLT.WEB.UI.COMMON.WEB;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;
using System.Threading;
using C1.Web.C1WebGrid;
using CLT.WEB.UI.COMMON.CONTROL;
using System.Web.UI;
using IAMVIRTUALAGENTCOMLib;
using System.Collections;

namespace CLT.WEB.UI.COMMON.BASE
{
    /// <summary>
    /// 1. 작업개요 : BasePage Class
    /// 
    /// 2. 주요기능 : Page의 공통 기능 정의
    ///				  
    /// 3. Class 명 : BasePage
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.08
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    //     *김은정 2012.08.08
    //     * Source
    //       BasePage.cs
    //     * Comment 
    //        Session 정보 추가 (공통사번)
    /// 
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        private int _listNo;
        public int ListNo
        {
            get { return _listNo; }
            set { _listNo = value; }
        }

        private string _imgPath = "/asset/images/";  // Images Path
        public string GetImgPath(string AddPathName) { return _imgPath + AddPathName + "/"; }
        public string GetImgPage() { return _imgPath; }

        private string _imgPathApplication = "/asset/images/application/";  // Images Path
        public string GetImgPathApplication() { return _imgPathApplication; }

        private string _imgPathCommission = "/asset/images/commission/";  // Images Path
        public string GetImgPathCommission() { return _imgPathCommission; }

        private string _imgPathIntro = "/asset/images/intro/";  // Images Path
        public string GetImgPathIntro() { return _imgPathIntro; }

        private bool _authRead;
        public bool AuthRead
        {
            get { return _authRead; }
            set { _authRead = value; }
        }
        private bool _authWrite;
        public bool AuthWrite
        {
            get { return _authWrite; }
            set { _authWrite = value; }
        }
        private bool _authDelete;
        public bool AuthDelete
        {
            get { return _authDelete; }
            set { _authDelete = value; }
        }
        private bool _authAdmin;
        public bool AuthAdmin
        {
            get { return _authAdmin; }
            set { _authAdmin = value; }
        }

        //private static string iBASE_PATH = @"Publish";
        //private static string iServerMachine = @"C:\LMS\CLT.WEB\Publish";
        //private static string CST_LOCAL_BIZASSEM_LOADPATH = System.Configuration.ConfigurationManager.AppSettings["CST_LOCAL_BIZASSEM_LOADPATH"].ToString();
        private static string CST_MSG_LOADPATH = System.Configuration.ConfigurationManager.AppSettings["CST_MSG_LOADPATH"].ToString();

        /************************************************************
        * Function name : OnLoad
        * Purpose       : Page가 로드될때 호출되는 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        protected override void OnLoad(EventArgs e)
        {
            // 공통 메세지 정의된 xml 파일 경로를 설정하기 위한 처리
            // Page가 호출되기 전에, 경로가 설정되어야 함


            //if (string.IsNullOrEmpty(SystemSettings.GET_CST_MSG_LOADPATH))
            MsgInfo.SetDirectoryPath(Server.MapPath(CST_MSG_LOADPATH));

            base.OnLoad(e);

            // 개발시, 계속해서 인증처리를 해야되는 번거로움을 피하기 위해,
            // 항상 인증 처리가 되도록 구현,
            // Go-Live 시에는 제거 필요.
            //if (!this.IsAuthenticated())
            //    this.SignIn("google001", "12345");

            // Go-Live 시에는 주석 제거 필요
            if (!this.IsAuthenticated())
            {
                this.SignIn(GuestUserID, "");
            }
        }

        #region 자주사용하는 함수
        virtual protected DateTime GetFirstDayofMonth(DateTime pDT)
        {
            return new DateTime(pDT.Year, pDT.Month, (int)1);
        }
        /// <summary>
        /// 해당월의 마지막일자 리턴한다.
        /// </summary>
        virtual protected DateTime GetLastDayofMonth(DateTime pDT)
        {
            return GetFirstDayofMonth(pDT).AddMonths(1).AddDays(-1);
        }
        /// <summary>
        /// 이번달의 마지막일자 리턴한다.
        /// </summary>
        virtual protected DateTime GetLastDayofMonth()
        {
            return GetLastDayofMonth(DateTime.Today);
        }
        /// <summary>
        /// 이번달의 첫번째일자를 리턴한다.
        /// </summary>
        virtual protected DateTime GetFirstDayofMonth()
        {
            return GetFirstDayofMonth(DateTime.Today);
        }

        /// <summary>
        /// 객체가 Null인지 여부를 나타냅니다.
        /// </summary>
        virtual protected bool IsNull(object psVal)
        {
            if (psVal != null && !Convert.IsDBNull(psVal))
                return false;
            else
                return true;
        }
        virtual protected string DateFormat(object psVal)
        {
            return DateFormat(psVal, ".");
        }
        /// <summary>
        /// 날짜 형식이 지정된 문자열을 반환합니다.
        /// </summary>
        virtual protected string DateFormat(object psVal, string psStyle)
        {
            int iVal = 0;

            try
            {
                if (!IsNull(psVal))
                {
                    if (psVal is DateTime) psVal = ((DateTime)psVal).ToString("yyyy-MM-dd");

                    iVal = Convert.ToInt32(psVal.ToString().Trim().Replace("-", "").Replace(".", "").Replace("/", ""));
                }
            }
            catch { }

            if (iVal == 0)
                return string.Empty;
            else if (iVal.ToString().Length == 5) return iVal.ToString("####-#").Replace("-", psStyle);
            else if (iVal.ToString().Length == 6) return iVal.ToString("####-##").Replace("-", psStyle);
            else if (iVal.ToString().Length == 7) return iVal.ToString("####-##-#").Replace("-", psStyle);
            else if (iVal.ToString().Length == 8) return iVal.ToString("####-##-##").Replace("-", psStyle);
            else return iVal.ToString();
        }
        #endregion

        #region Error Msg 처리
        protected void NotifyError(Exception rEx)
        {
            Session["ERR"] = InitMessage(rEx);
            //string xUrl = "/error.aspx?";
            //string xUrl = "/error.aspx";
            //xUrl += string.Format("rEx={0}", @"TT");
            ClientScript.RegisterStartupScript(this.GetType(), "error",
                                   "<script language='javascript'>OpenPopFixedWindow('/error.aspx','error', '320', '285');</script>");
            //string.Format("rEx={0}", InitMessage(rEx)) + 
            //string.Format("{0}", InitMessage(rEx)) + 
            //"','error', '303', '279');</script>"); 
        }
        private string InitMessage(Exception rEx)
        {
            string sDisplayErrorMessage = string.Empty;
            try
            {
                //   Error Message Case : 메소드를 찾을수 없을경우.
                //   System.Web.Services.Protocols.SoapException: 서버에서 요청을 처리할 수 없습니다. 
                //   ---> System.MissingMethodException: 'CLT.BIZ.SAMPLES.DBCaseStudy.Sample.LOB_INSERTk' 메서드를 찾을 수 없습니다.
                //   위치: System.RuntimeType.InvokeMember(String name, BindingFlags bindingFlags, Binder binder, Object target, Object[] providedArgs, ParameterModifier[] modifiers, CultureInfo culture, String[] namedParams)
                //   위치: System.Type.InvokeMember(String name, BindingFlags invokeAttr, Binder binder, Object target, Object[] args)
                //   위치: BRIDGE1.ExecuteOnly(PARAMBUILDER oPARAMBUILDER) 파일 c:\CLT\CLT.WEBSERVICE\CLT.WEBSERVICE.BROKER\App_Code\BRIDGE1.cs:줄 331
                //   --- 내부 예외 스택 추적의 끝 ---

                string sError = rEx.GetType().FullName;
                sDisplayErrorMessage = rEx.Message + Environment.NewLine + Environment.NewLine
                + "--INNER EXCEPTION--"
                + Environment.NewLine;

                if (rEx.InnerException != null)
                {
                    sDisplayErrorMessage += "Message: " + rEx.InnerException.Message;
                    sDisplayErrorMessage += Environment.NewLine;
                    sDisplayErrorMessage += rEx.InnerException.StackTrace;
                }
                else
                {
                    sDisplayErrorMessage += "InnerException.Stack Trace not detected";
                }

                sDisplayErrorMessage += Environment.NewLine + Environment.NewLine
                                    + "--STACK TRACE--"
                                    + Environment.NewLine
                                    + rEx.Source
                                    + Environment.NewLine
                                    + rEx.StackTrace;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sDisplayErrorMessage;
        }
        #endregion

        #region Paging 관련 처리

        private int iPageSize = 15;
        private int iCurrentPageIndex = 1;

        // 한 페이지에 표시해야 되는 Grid 항목 갯수
        protected int PageSize
        {
            get
            {
                return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PageSize"]);
            }
            set
            {
                iPageSize = value;
            }
        }

        // Grid의 현재 페이지 Index
        protected int CurrentPageIndex
        {
            get
            {
                return this.iCurrentPageIndex;
            }
            set
            {
                this.iCurrentPageIndex = value;
            }
        }

        #endregion

        #region Excel 관련 처리

        /************************************************************
        * Function name : GetExcelFile
        * Purpose       : 컬럼명 없이 Excel 파일 생성하는 처리
        * Input         : DataTable rDt
        * Output        : void
        *************************************************************/
        protected void GetExcelFile(DataTable rDt)
        {
            C1.C1Excel.C1XLBook c1Excel = new C1.C1Excel.C1XLBook();

            c1Excel.Clear();
            C1.C1Excel.XLSheet sheet = c1Excel.Sheets[0];
            sheet.Name = "Sheet1";

            c1Excel.DefaultFont = new System.Drawing.Font("돋움", 11);

            for (int j = 0; j < rDt.Columns.Count; j++)
            {
                sheet[0, j].Value = rDt.Columns[j].ColumnName;
            }

            for (int i = 0; i < rDt.Rows.Count; i++)
            {
                for (int j = 0; j < rDt.Columns.Count; j++)
                {
                    sheet[i + 1, j].Value = rDt.Rows[i][j].ToString();
                }
            }
            string filename = ((Random)new Random()).Next(10000000, 99999999).ToString() + ".xls";
            string savename = Server.MapPath(this.TempFilePath) + filename;
            c1Excel.Save(savename);

            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-disposition", "attachment;");
            Response.ContentType = "application/vnd.ms-excel";
            Response.WriteFile(savename);
            Response.Flush();
            Response.Close();

            System.IO.File.Delete(savename);
        }

        /************************************************************
        * Function name : GetExcelFile
        * Purpose       : 컬럼명 포함한 형태의 Excel 파일 생성하는 처리
        * Input         : DataTable rDt, string[] rHeader
        * Output        : void
        *************************************************************/
        protected void GetExcelFile(DataTable rDt, string[] rHeader)
        {
            try
            {
                C1.C1Excel.C1XLBook c1Excel = new C1.C1Excel.C1XLBook();

                c1Excel.Clear();
                C1.C1Excel.XLSheet sheet = c1Excel.Sheets[0];
                sheet.Name = "Sheet1";

                c1Excel.DefaultFont = new System.Drawing.Font("돋움", 11);

                for (int j = 0; j < rHeader.Length; j++)
                {
                    sheet[0, j].Value = rHeader[j];
                }

                for (int i = 0; i < rDt.Rows.Count; i++)
                {
                    for (int j = 0; j < rDt.Columns.Count; j++)
                    {
                        sheet[i + 1, j].Value = rDt.Rows[i][j].ToString();
                    }
                }
                string filename = ((Random)new Random()).Next(10000000, 99999999).ToString() + ".xls";
                string savename = Server.MapPath(this.TempFilePath) + filename;
                c1Excel.Save(savename);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("content-disposition", "attachment;");
                Response.ContentType = "application/vnd.ms-excel";
                Response.WriteFile(savename);
                Response.Flush();
                Response.Close();

                System.IO.File.Delete(savename);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /************************************************************
        * Function name : GetExcelFile
        * Purpose       : 컬럼명 포함한 형태의 Excel 파일 생성하는 처리
        * Input         : DataTable rDt, string[] rHeader
        * Output        : void
        *************************************************************/
        protected void GetExcelFile(DataTable rDt, string[] rExcelHeader, string[] rDtHeader, string rIsNum)
        {
            try
            {
                C1.C1Excel.C1XLBook c1Excel = new C1.C1Excel.C1XLBook();

                c1Excel.Clear();
                C1.C1Excel.XLSheet sheet = c1Excel.Sheets[0];
                sheet.Name = "Sheet1";

                c1Excel.DefaultFont = new System.Drawing.Font("돋움", 11);

                if (rIsNum == "1")
                {
                    for (int i = 0; i < rExcelHeader.Length + 1; i++)
                    {
                        if (i == 0)
                            sheet[0, i].Value = "No.";
                        else
                            sheet[0, i].Value = rExcelHeader[i - 1];
                    }

                    for (int i = 0; i < rDt.Rows.Count; i++)
                    {
                        for (int j = 0; j < rExcelHeader.Length + 1; j++)
                        {
                            if (j == 0)
                                sheet[i + 1, j].Value = i + 1;
                            else
                                sheet[i + 1, j].Value = Convert.ToString(rDt.Rows[i][rDtHeader[j - 1]]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < rExcelHeader.Length; i++)
                    {
                        sheet[0, i].Value = rExcelHeader[i];
                    }

                    for (int i = 0; i < rDt.Rows.Count; i++)
                    {
                        for (int j = 0; j < rExcelHeader.Length; j++)
                        {
                            sheet[i + 1, j].Value = Convert.ToString(rDt.Rows[i][rDtHeader[j]]);
                        }
                    }
                }
                string filename = ((Random)new Random()).Next(10000000, 99999999).ToString() + ".xls";
                string savename = Server.MapPath(this.TempFilePath) + filename;
                c1Excel.Save(savename);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("content-disposition", "attachment;");
                Response.ContentType = "application/vnd.ms-excel";
                Response.WriteFile(savename);
                Response.Flush();
                Response.Close();

                System.IO.File.Delete(savename);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /************************************************************
        * Function name : GetExcelFile
        * Purpose       : 컬럼명 포함한 형태의 Excel 파일 생성하는 처리
        * Input         : DataTable rDt, string[] rHeader
        * Output        : void
         *        -------- 템플릿 컬럼 때문에 안됨
        
        protected void GetExcelFile(DataTable rDt, C1WebGrid grdList)
        {
            C1.C1Excel.C1XLBook c1Excel = new C1.C1Excel.C1XLBook();

            c1Excel.Clear();
            C1.C1Excel.XLSheet sheet = c1Excel.Sheets[0];
            sheet.Name = "Sheet1";

            c1Excel.DefaultFont = new System.Drawing.Font("돋움", 11);

            for (int i = 0; i < grdList.Columns.Count; i++)
            {
                if (grdList.Columns[i].Visible)
                    sheet[0, i].Value = grdList.Columns[i].HeaderText;
            }

            //for (int j = 0; j < rExcelHeader.Length; j++)
            //{
            //    sheet[0, j].Value = rExcelHeader[j];
            //}

            for (int i = 0; i < rDt.Rows.Count; i++)
            {
                for (int j = 0; j < grdList.Columns.Count; j++)
                {
                    if (grdList.Columns[j].Visible)
                    {
                        //sheet[i + 1, j].Value = Convert.ToString(rDt.Rows[i][(grdList.Columns[j] as C1BoundColumn).DataField]);
                        sheet[i + 1, j].Value = Convert.ToString(grdList.Columns[j]);
                    }
                }

                //for (int j = 0; j < rDt.Columns.Count; j++)
                //{
                //    for (int k = 0; k < rDtHeader.Length; k++)
                //    {
                //        sheet[i + 1, j].Value = Convert.ToString(rDt.Columns[rDtHeader[k]]);
                //    }
                //}
            }
            string filename = ((Random)new Random()).Next(10000000, 99999999).ToString() + ".xls";
            string savename = Server.MapPath(this.TempFilePath) + filename;
            c1Excel.Save(savename);

            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-disposition", "attachment;");
            Response.ContentType = "application/vnd.ms-excel";
            Response.WriteFile(savename);
            Response.Flush();
            Response.Close();

            System.IO.File.Delete(savename);
        }
         * *************************************************************/

        /************************************************************
        * Function name : GetExcelFileHtml
        * Purpose       : Html -> 엑셀파일로 변환
        * Input         : string rHtml
        * Output        : void
        *************************************************************/
        public void GetExcelFileHtml(string rHtml)
        {
            try
            {
                string filename = ((Random)new Random()).Next(10000000, 99999999).ToString() + ".xls";
                string savename = Server.MapPath(this.TempFilePath) + filename;

                System.Text.Encoding encode = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
                StreamWriter sw = new StreamWriter(savename, false, encode);
                try
                {
                    sw.Write(rHtml);
                    sw.Write("\n");
                    sw.Flush();
                }
                finally
                {
                    sw.Close();
                    sw.Dispose();
                }

                Response.ClearContent();
                Response.ClearHeaders();
                //Response.AddHeader("content-disposition", "attachment;filename=" + orgFileName.Trim());
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                Response.WriteFile(savename);
                Response.Flush();
                Response.Close();

                System.IO.File.Delete(savename);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        #endregion

        #region 경로 정의

        // excel 파일 다운로드 등과 같이 
        // 임시로 생성되어 삭제되는 파일들을 관리할 때 사용하는 경로
        protected string TempFilePath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["TempFilePath"].ToString();
            }
        }

        // LMS 컨텐츠 보관 파일 경로
        protected string ContentsFilePath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ContentsFilePath"].ToString();
            }
        }

        // Viewer 등 시스템에서 필요한 파일을 Download 하기 위한 경로
        protected string DownloadFilePath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DownloadFilePath"].ToString();
            }
        }

        #endregion

        #region 언어 설정

        /************************************************************
        * Function name : InitializeCulture
        * Purpose       : 페이지의 언어설정 처리
        *                 Session 정보가 없으면, 인터넷 브라우저 설정 사용하고
        *                 Session 정보가 있으면, Session 정보를 통해 언어 설정 
        * Input         : void
        * Output        : void
        *************************************************************/
        protected override void InitializeCulture()
        {
            if (Session["Language"] != null && Session["Language"].ToString().Trim() != string.Empty)
            {
                switch (Session["Language"].ToString().ToUpper())
                {
                    case "KO":
                        this.Culture = "ko-kr";
                        this.UICulture = "ko-kr";
                        break;
                    case "EN":
                        this.Culture = "en-us";
                        this.UICulture = "en-us";
                        break;
                }
            }
            else
            {
                CultureInfo xCi = System.Threading.Thread.CurrentThread.CurrentCulture;
            }

            base.InitializeCulture();
        }

        /************************************************************
        * Function name : IsSettingKorean
        * Purpose       : 현재 설정이 한국어인지 여부 확인 처리
        * Input         : void
        * Output        : bool
        *************************************************************/
        protected bool IsSettingKorean()
        {
            CultureInfo xCi = System.Threading.Thread.CurrentThread.CurrentCulture;

            if (xCi.Name.ToLower() == "ko-kr")
                return true;
            else
                return false;
        }

        #endregion

        #region 권한 관리
        
        private object[,] iBtn;
        private string iParentMenuCode = null;

        public void pRender(System.Web.UI.Page rPage, object[,] rButtonParams)
        {
            this.iParentMenuCode = null;
            rPage.PreRender += new System.EventHandler(PreRenderEvent);
            this.iBtn = rButtonParams;
        }
        public void pRender(System.Web.UI.Page rPage, object[,] rButtonParams, string rParentMenuCode)
        {
            rPage.PreRender += new System.EventHandler(PreRenderEvent);
            this.iBtn = rButtonParams;
            this.iParentMenuCode = rParentMenuCode;
        }

        public void PreRenderEvent(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["MenuCode"]))
                {
                    Session["MENU_CODE"] = Request.QueryString["MenuCode"];
                }

                if (!IsPostBack || string.IsNullOrEmpty(Convert.ToString(Session["USER_ID"])) || Convert.ToString(Session["USER_GROUP"]) == this.GuestUserID)
                {
                    // 로그인 정보 ID 만 가져 올 수 있는 관계로 임시로 사용...
                    DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                                   "GetMenuAuthority",
                                                   LMS_SYSTEM.MANAGE,
                                                   "CLT.WEB.UI.COMMON.BASE.BasePage",
                                                   this.iParentMenuCode == null ? Convert.ToString(Session["MENU_CODE"]) : this.iParentMenuCode,
                                                   Convert.ToString(Session["USER_GROUP"]));

                    DataRow xDr = null;
                    if (xDt != null && xDt.Rows.Count > 0)
                    {
                        xDr = xDt.Rows[0];
                    }
                    else
                    {
                        //주소를 치고 들어 갔을 때.. guest 권한으로 버튼 enable 처리 하도록!! 
                        for (int i = 0; i < this.iBtn.GetLength(0); i++)
                        {
                            // this.iBtn
                            // [0,0] : Button Object
                            // [0,1] : Authority (I, E, D, A)
                            Button xBtn = null;
                            if (this.iBtn[i, 0] != null)
                            {
                                xBtn = (Button)this.iBtn[i, 0];
                                xBtn.Enabled = false;
                                // DOM Explorer 에서 편집하면 버튼이 활성화 되는 문제 해결을 위해 버튼을 숨김처리함
                                xBtn.Visible = false;
                            }
                        }
                    }

                    if (xDr != null)
                    {
                        for (int i = 0; i < this.iBtn.GetLength(0); i++)
                        {
                            // this.iBtn
                            // [0,0] : Button Object
                            // [0,1] : Authority (I, E, D, A)
                            Button xBtn = null;
                            if (this.iBtn[i, 0] != null)
                            {
                                xBtn = (Button)this.iBtn[i, 0];
                                xBtn.Enabled = false;
                                // DOM Explorer 에서 편집하면 버튼이 활성화 되는 문제 해결을 위해 버튼을 숨김처리함
                                xBtn.Visible = false;
                            }

                            bool xIsAuth = false;
                            switch (Convert.ToString(this.iBtn[i, 1]))
                            {
                                // 조회 권한(inquiry_yn)
                                // 수정 권한(edit_yn)
                                // 삭제 권한(del_yn)
                                // 관리자 권한(admin_yn)
                                case "I":
                                    if (Convert.ToString(xDr["inquiry_yn"]) == "Y")
                                    {
                                        xIsAuth = true;
                                    }
                                    break;
                                case "E":
                                    if (Convert.ToString(xDr["edit_yn"]) == "Y")
                                    {
                                        xIsAuth = true;
                                    }
                                    break;
                                case "D":
                                    if (Convert.ToString(xDr["del_yn"]) == "Y")
                                    {
                                        xIsAuth = true;
                                    }
                                    break;
                                case "A":
                                    if (Convert.ToString(xDr["admin_yn"]) == "Y")
                                    {
                                        xIsAuth = true;
                                    }
                                    break;
                            }

                            if (xIsAuth)
                            {
                                xBtn.Enabled = true;
                                xBtn.Visible = true;
                            }
                            else
                            {
                                xBtn.Enabled = false;
                                xBtn.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }


        public void getAuth()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["MenuCode"]))
                {
                    Session["MENU_CODE"] = Request.QueryString["MenuCode"];
                }

                AuthRead = AuthWrite = AuthDelete = AuthAdmin = false;

                if (!IsPostBack || string.IsNullOrEmpty(Convert.ToString(Session["USER_ID"])) || Convert.ToString(Session["USER_GROUP"]) == this.GuestUserID)
                {
                    // 로그인 정보 ID 만 가져 올 수 있는 관계로 임시로 사용...
                    DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                                   "GetMenuAuthority",
                                                   LMS_SYSTEM.MANAGE,
                                                   "CLT.WEB.UI.COMMON.BASE.BasePage",
                                                   this.iParentMenuCode == null ? Convert.ToString(Session["MENU_CODE"]) : this.iParentMenuCode,
                                                   Convert.ToString(Session["USER_GROUP"]));

                    DataRow xDr = null;
                    if (xDt != null && xDt.Rows.Count > 0)
                    {
                        xDr = xDt.Rows[0];
                    }

                    if (xDr != null)
                    {
                        AuthRead = Convert.ToString(xDr["inquiry_yn"]) == "Y";
                        AuthWrite = Convert.ToString(xDr["edit_yn"]) == "Y";
                        AuthDelete = Convert.ToString(xDr["del_yn"]) == "Y";
                        AuthAdmin = Convert.ToString(xDr["admin_yn"]) == "Y";
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        #endregion

        #region 인증 관리




        // 인증되지 않은 사용자의 권한 처리를 위한 사용자 ID
        protected string DebugMode
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DebugMode"].ToString();
            }
        }

        // 인증되지 않은 사용자의 권한 처리를 위한 사용자 ID
        protected string GuestUserID
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["GuestUserID"].ToString();
            }
        }

        /************************************************************
        * Function name : SignIn
        * Purpose       : ID, Pwd를 이용하여 로그인 처리
        * Input         : string rUserID, string rUserPwd
        * Output        : bool
        *************************************************************/
        public bool SignIn(string rUserID, string rUserPwd)
        {
            // DB에서 user_id, user_pwd 정보를 읽어와서 있는지 여부를 판단 후, 
            // 있을 경우, true
            // 없을 경우, false 로 처리한다.
            bool xRtn = false;
            DataTable xDt = null;
            string[] xParams = new string[2];

            if (DebugMode == Boolean.FalseString)
            {
                string EnPWD = OpusCryptoLibrary.Cryptography.Encrypt(rUserPwd, rUserPwd);

                xParams[0] = rUserID;
                xParams[1] = EnPWD;
            }
            else
            {
                xParams[0] = rUserID;
                xParams[1] = string.Empty;
            }

            // Guest User로 접근한 경우
            if (rUserID == this.GuestUserID)
            {
                Session["USER_ID"] = this.GuestUserID;
                Session["USER_GROUP"] = this.GuestUserID;

                xRtn = true;
            }
            // Login 창을 통해, 사용자가 ID, Password를 입력하여 접근한 경우
            else
            {
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                  "GetUserSessionInfo",
                                                 LMS_SYSTEM.MAIN,
                                                 "CLT.WEB.UI.COMMON.BASE", (object)xParams);

                if (xDt != null && xDt.Rows.Count > 0)
                {
                    FormsAuthentication.SetAuthCookie(rUserID, false); // rUserID는 아이디, false 는 웹브라우저 종료시 로그아웃
                    SetSessionInfo(xDt);
                    xRtn = true;
                }
                else
                {
                    xRtn = false;
                }
            }


            return xRtn;
        }


        /************************************************************
        * Function name : SignInSea
        * Purpose       : 해상직원 ID, Pwd를 이용하여 로그인 처리
        * Input         : string rUserID, string rUserPwd
        * Output        : bool
        *************************************************************/
        public bool SignInSea(string rUserID, string rUserPwd)
        {
            // DB에서 user_id, user_pwd 정보를 읽어와서 있는지 여부를 판단 후, 
            // 있을 경우, true
            // 없을 경우, false 로 처리한다.
            bool xRtn = false;
            DataTable xDt = null;
            string[] xParams = new string[2];

            if (DebugMode == Boolean.FalseString.ToUpper())
            {
                xParams[0] = rUserID;
                xParams[1] = (string)Password_Encode(rUserPwd);  // 해상인사 암호화 모듈
            }
            else
            {
                xParams[0] = rUserID;
                xParams[1] = string.Empty;
            }

            // Guest User로 접근한 경우
            if (rUserID == this.GuestUserID)
            {
                Session["USER_ID"] = this.GuestUserID;
                Session["USER_GROUP"] = this.GuestUserID;

                xRtn = true;
            }
            // Login 창을 통해, 사용자가 ID, Password를 입력하여 접근한 경우
            else
            {
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                  "GetUserSessionInfo",
                                                 LMS_SYSTEM.MAIN,
                                                 "CLT.WEB.UI.COMMON.BASE", (object)xParams);

                if (xDt != null && xDt.Rows.Count > 0)
                {
                    FormsAuthentication.SetAuthCookie(rUserID, false); // rUserID는 아이디, false 는 웹브라우저 종료시 로그아웃
                    SetSessionInfo(xDt);
                    xRtn = true;
                }
                else
                {
                    xRtn = false;
                }
            }


            return xRtn;
        }

        /************************************************************
        * Function name : SignInOK
        * Purpose       : ID, Pwd를 이용하여 로그인 처리
        * Input         : string rUserID, string rUserPwd
        * Output        : bool
        *************************************************************/
        public bool SignInOK(string rUserID, string rUserPwd)
        {
            // DB에서 user_id, user_pwd 정보를 읽어와서 있는지 여부를 판단 후, 
            // 있을 경우, true
            // 없을 경우, false 로 처리한다.
            bool xRtn = false;
            DataTable xDt = null;
            string[] xParams = new string[2];

            xParams[0] = rUserID;
            xParams[1] = string.Empty;


            xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                              "GetUserSessionInfo",
                                             LMS_SYSTEM.MAIN,
                                             "CLT.WEB.UI.COMMON.BASE", (object)xParams);

            if (xDt != null && xDt.Rows.Count > 0)
            {
                FormsAuthentication.SetAuthCookie(rUserID, false); // rUserID는 아이디, false 는 웹브라우저 종료시 로그아웃
                SetSessionInfo(xDt);
                xRtn = true;
            }
            else
            {
                xRtn = false;
            }


            return xRtn;
        }


        /************************************************************
        * Function name : SignOut
        * Purpose       : 로그아웃 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void SignOut()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
        }

        /************************************************************
        * Function name : IsAuthenticated
        * Purpose       : 현재 로그인된 사용자가 로그인이 되어있는지 여부 확인
        * Input         : void
        * Output        : void
        *************************************************************/
        protected bool IsAuthenticated()
        {
            if (Page.User.Identity.IsAuthenticated)
                return true;
            else
                return false;
        }

        /************************************************************
        * Function name : SetSessionInfo
        * Purpose       : 전역적으로 사용될 사용자 관련 정보를 Session에 보관하는 처리
        * Input         : DataTable rUser
        * Output        : void
        *************************************************************/
        private void SetSessionInfo(DataTable rUser)
        {
            Session["USER_ID"] = rUser.Rows[0]["user_id"].ToString(); // 사용자 ID
            Session["USER_NO"] = rUser.Rows[0]["user_no"].ToString(); // 사용자 사번
            Session["USER_KNM"] = rUser.Rows[0]["user_nm_kor"].ToString(); // 한글 성명
            Session["USER_ENM"] = rUser.Rows[0]["user_nm_eng"].ToString(); // 영문명



            Session["USER_EMAIL"] = rUser.Rows[0]["email_id"].ToString(); // 이메일            
            Session["USER_GU"] = rUser.Rows[0]["duty_gu"].ToString(); // 고용형태
            Session["USER_STATUS"] = rUser.Rows[0]["status"].ToString(); // 사용자 계정 승인상태 (000001:미승인, 000002:사용안함, 000003:승인)

            Session["USER_GROUP"] = rUser.Rows[0]["user_group"].ToString(); // 사용자 권한그룹 코드
            Session["USER_GROUP_KNM"] = rUser.Rows[0]["user_group_knm"].ToString(); // 사용자 권한그룹 한글명



            Session["USER_GROUP_ENM"] = rUser.Rows[0]["user_group_enm"].ToString(); // 사용자 권한그룹 영문명




            Session["DUTY_STEP"] = rUser.Rows[0]["duty_step"].ToString(); // 직급코드
            Session["DUTY_STEP_KNM"] = rUser.Rows[0]["step_name"].ToString(); // 직급 한글명



            Session["DUTY_STEP_ENM"] = rUser.Rows[0]["step_ename"].ToString(); // 직급 영문명


            Session["ENTER_DT"] = rUser.Rows[0]["enter_dt"].ToString(); // 입사일자


            Session["DEPT_CODE"] = rUser.Rows[0]["dept_code"].ToString(); // 부서코드



            Session["DEPT_KNM"] = rUser.Rows[0]["dept_name"].ToString(); // 부서 한글명



            Session["DEPT_ENM"] = rUser.Rows[0]["dept_ename"].ToString(); // 부서 영문명




            Session["COMPANY_ID"] = rUser.Rows[0]["company_id"].ToString(); // 소속회사 ID
            Session["COMPANY_KNM"] = rUser.Rows[0]["company_nm"].ToString(); // 소속회사 한글명



            Session["COMPANY_ENM"] = rUser.Rows[0]["company_nm_eng"].ToString(); // 소속회사 영문명


            Session["DUTY_WORK"] = rUser.Rows[0]["duty_work"].ToString();
            Session["OFFICE_PHONE"] = rUser.Rows[0]["office_phone"].ToString();

            Session["COMPANY_KIND"] = rUser.Rows[0]["COMPANY_KIND"].ToString();

            Session["COM_SNO"] = rUser.Rows[0]["COM_SNO"].ToString();  //공통사번 추가

            // 기타 등등 전역적으로 필요한 사용자의 정보가 있으면 추가
        }
        #endregion


        /************************************************************
        * Function name : SetGridClear(C1WebGrid grid, PageNavigator pageInfo)
        * Purpose       : 그리드 초기화 메서드
        * Input         : void
        * Output        : void
        *************************************************************/
        #region SetGridClear(C1WebGrid grid, PageNavigator pageInfo)
        public void SetGridClear(C1WebGrid grid, PageNavigator pageInfo)
        {
            try
            {
                //if (rUserGroup == "000009" || string.IsNullOrEmpty(rUserGroup))  // 손님이면 초기화


                //if (Session["USER_GROUP"].ToString() == "000009" || string.IsNullOrEmpty(Session["USER_GROUP"].ToString()))  // 손님이면 초기화


                //{
                grid.DataBind();
                if (pageInfo != null)
                {
                    pageInfo.TotalRecordCount = 0;
                    pageInfo.PageSize = PageSize;
                    pageInfo.TotalRecordCount = 0;
                }
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion 그리드 초기화



        /************************************************************
        * Function name : SetGridClear(C1WebGrid grid, PageNavigator pageInfo)
        * Purpose       : 그리드 초기화 메서드
        * Input         : void
        * Output        : void
        *************************************************************/
        #region SetGridClear(C1WebGrid grid, PageNavigator pageInfo)
        public void SetGridClear(C1WebGrid grid, PageNavigator pageNavigator, PageInfo pageInfo)
        {
            try
            {
                //if (rUserGroup == "000009" || string.IsNullOrEmpty(rUserGroup))  // 손님이면 초기화



                //if (Session["USER_GROUP"].ToString() == "000009" || string.IsNullOrEmpty(Session["USER_GROUP"].ToString()))  // 손님이면 초기화



                //{
                grid.DataBind();
                if (pageNavigator != null)
                {
                    pageNavigator.TotalRecordCount = 0;
                    pageNavigator.CurrentPageIndex = 1;
                }

                if (pageInfo != null)
                {
                    pageInfo.TotalRecordCount = 0;
                    pageInfo.PageSize = PageSize;
                    pageInfo.TotalRecordCount = 0;

                }
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion 그리드 초기화



        /************************************************************
        * Function name : SetGridClear(C1WebGrid grid)
        * Purpose       : 그리드 초기화 메서드
        * Input         : void
        * Output        : void
        *************************************************************/
        #region SetGridClear(C1WebGrid grid)
        public void SetGridClear(C1WebGrid grid)
        {
            try
            {
                SetGridClear(grid, null);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion 그리드 초기화



        /************************************************************
        * Function name : ConvertToFileUpload
        * Purpose       : 첨부파일 업로드
        * Input         : void
        * Output        : void
        *************************************************************/
        #region ConvertToFileUpload
        public byte[] ConvertToFileUpload(FileUpload fUpload)
        {
            HttpPostedFile xFile = (HttpPostedFile)fUpload.PostedFile;
            Stream stream = xFile.InputStream;

            int xGetbyte = 0;
            List<byte> bytes = new List<byte>();
            while ((xGetbyte = stream.ReadByte()) > -1)
            {
                bytes.Add((byte)xGetbyte);
            }

            stream.Close();

            return bytes.ToArray();
        }

        public byte[] ConvertToFileUpload(HttpPostedFile fUpload)
        {
            HttpPostedFile xFile = fUpload;
            Stream stream = xFile.InputStream;

            int xGetbyte = 0;
            List<byte> bytes = new List<byte>();
            while ((xGetbyte = stream.ReadByte()) > -1)
            {
                bytes.Add((byte)xGetbyte);
            }

            stream.Close();

            return bytes.ToArray();
        }
        #endregion ConvertToFileUpload

        public void SetControlAttributes(object[] rObj, Button rBtn, Page rPage)
        {
            foreach (object xObj in rObj)
            {
                if (xObj.GetType() == typeof(TextBox))
                {
                    TextBox txtBox = (TextBox)xObj;
                    txtBox.Attributes.Add("onkeypress", "if(event.keyCode == 13) { " + rPage.ClientScript.GetPostBackEventReference(rBtn, "") + "; return false;}");
                }
                else if (xObj.GetType() == typeof(DropDownList))
                {
                    DropDownList dropdownlist = (DropDownList)xObj;
                    dropdownlist.Attributes.Add("onkeypress", "if(event.keyCode == 13) { " + rPage.ClientScript.GetPostBackEventReference(rBtn, "") + "; return false;}");
                }
            }
        }

        // 문자 Encoding(Password) 해상직원
        public static string Password_Encode(string rPwd)
        {
            string xPwd = "";

            if (rPwd.Trim() != "")
            {
                // String 값을 Char 배열로 전환
                char[] xChr = rPwd.ToCharArray();
                // 전체 Char 개수
                int xLen = xChr.Length;
                //Encoding을 위한 배열 선언
                int[] xAsc = new int[xLen];
                int[] xTmp = new int[xLen];

                for (int i = 0; i < xLen; i++)
                {
                    // Char를 Ascii값으로 변환후 영문 대소문자의 코드값 차이 계산
                    xAsc[i] = (int)xChr[i] - 33;

                    //숫자/영문여부 Check
                    if (xAsc[i] > 93)
                    {

                    }
                }

                //Encoding을 위한 계산 시작
                xTmp[0] = xAsc[0];
                for (int i = 1; i < xLen; i++)
                {
                    xTmp[i] = xAsc[i - 1] + xAsc[i];
                }

                for (int i = 0; i < xLen; i++)
                {
                    xTmp[i] = xTmp[i] + xLen + i + 1;
                    xTmp[i] = xTmp[i] % 93;
                    xTmp[i] = xTmp[i] + 33;
                    //Char별 Encoding값을 취합
                    xPwd += (char)xTmp[i];
                }
            }
            //Encoding된 Password 값 반환
            return xPwd;
        }

        // IP 대역폭 확인
        public bool IsInternalIP(string rIpAddr)
        {
            bool xInternalIP = false;
            string[] xIPSpliter = null;
            Hashtable xHtIP_A_Cls = null;
            Hashtable xHtIP_B_Cls = null;
            Hashtable xHtIP_C_Cls = null;
            xHtIP_A_Cls = new Hashtable();
            xHtIP_B_Cls = new Hashtable();
            xHtIP_C_Cls = new Hashtable();
            //Add A CLASS
            xHtIP_A_Cls.Add("10", null);
            //Add B CLASS
            xHtIP_B_Cls.Add("172.16", null);
            xHtIP_B_Cls.Add("172.17", null);
            xHtIP_B_Cls.Add("172.18", null);
            xHtIP_B_Cls.Add("172.19", null);
            xHtIP_B_Cls.Add("172.20", null);
            xHtIP_B_Cls.Add("172.21", null);
            xHtIP_B_Cls.Add("172.22", null);
            xHtIP_B_Cls.Add("172.23", null);
            xHtIP_B_Cls.Add("172.24", null);
            xHtIP_B_Cls.Add("172.25", null);
            xHtIP_B_Cls.Add("172.26", null);
            xHtIP_B_Cls.Add("172.27", null);
            xHtIP_B_Cls.Add("172.28", null);
            xHtIP_B_Cls.Add("172.29", null);
            xHtIP_B_Cls.Add("172.30", null);
            xHtIP_B_Cls.Add("172.31", null);
            xHtIP_B_Cls.Add("192.168", null);
            //Add C CLASS
            xHtIP_C_Cls.Add("203.246.128", null);
            xHtIP_C_Cls.Add("203.246.129", null);
            xHtIP_C_Cls.Add("203.246.130", null);
            xHtIP_C_Cls.Add("203.246.131", null);
            xHtIP_C_Cls.Add("203.246.132", null);
            xHtIP_C_Cls.Add("203.246.133", null);
            xHtIP_C_Cls.Add("203.246.134", null);
            xHtIP_C_Cls.Add("203.246.135", null);
            xHtIP_C_Cls.Add("203.246.136", null);
            xHtIP_C_Cls.Add("203.246.137", null);
            xHtIP_C_Cls.Add("203.246.138", null);
            xHtIP_C_Cls.Add("203.246.139", null);
            xHtIP_C_Cls.Add("203.246.140", null);
            xHtIP_C_Cls.Add("203.246.141", null);
            xHtIP_C_Cls.Add("203.246.142", null);
            xHtIP_C_Cls.Add("203.246.143", null);
            xHtIP_C_Cls.Add("203.246.144", null);
            xHtIP_C_Cls.Add("203.246.145", null);
            xHtIP_C_Cls.Add("203.246.146", null);
            xHtIP_C_Cls.Add("203.246.147", null);
            xHtIP_C_Cls.Add("203.246.148", null);
            xHtIP_C_Cls.Add("203.246.149", null);
            xHtIP_C_Cls.Add("203.246.150", null);
            xHtIP_C_Cls.Add("203.246.151", null);
            xHtIP_C_Cls.Add("203.246.152", null);
            xHtIP_C_Cls.Add("203.246.153", null);
            xHtIP_C_Cls.Add("203.246.154", null);
            xHtIP_C_Cls.Add("203.246.155", null);
            xHtIP_C_Cls.Add("203.246.156", null);
            xHtIP_C_Cls.Add("203.246.157", null);
            xHtIP_C_Cls.Add("203.246.158", null);
            xHtIP_C_Cls.Add("203.246.159", null);
            xHtIP_C_Cls.Add("210.176.100", null);
            xHtIP_C_Cls.Add("210.176.101", null);
            xHtIP_C_Cls.Add("203.127.145", null);
            xIPSpliter = rIpAddr.Split('.');
            switch (xIPSpliter[0])
            {
                case "10":
                    xInternalIP = true;
                    break;
                case "172":
                case "192":
                    if (xHtIP_B_Cls.ContainsKey(xIPSpliter[0] + "." + xIPSpliter[1]))
                    {
                        xInternalIP = true;
                    }
                    break;
                case "203":
                case "210":
                    if (xHtIP_C_Cls.ContainsKey(xIPSpliter[0] + "." + xIPSpliter[1] + "." + xIPSpliter[2]))
                    {
                        xInternalIP = true;
                    }
                    break;
            }
            return xInternalIP;
        }

    }
}