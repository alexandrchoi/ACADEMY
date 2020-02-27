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
using System.IO;
using CLT.WEB.UI.FX.UTIL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using CLT.WEB.UI.COMMON.BASE;

namespace CLT.WEB.UI.LMS.EDUM
{
    /// <summary>
    /// 1. 작업개요 : 교육이수증 Report
    /// 
    /// 2. 주요기능 : 교육이수증 Report
    ///				  
    /// 3. Class 명 : issuing_report
    /// 
    /// 4. 작 업 자 : 김은정

    /// 
    /// 5. Revision History : 
    ///    [CHM-201218484] 국토해양부 로고 추가 및 데이터 정렬 기준 추가
    ///        * 김은정 2012.09.14
    ///        * Source
    ///          issuing_report
    ///        * Comment 
    ///          교육 이수증발급 시 국토해양부 과정여부 체크하여 국토해양부 로그 표시하여 출력되도록 변경, 영문(성명) 표시 시 "성, 이름" 순으로 표시
    /// 
    /// </summary>
    /// 
    public partial class issuing_report_old : BasePage
    {
        private DataTable iDtCourseResult;

        public DataTable IDtCourseResult
        {
            get { return iDtCourseResult; }
            set { iDtCourseResult = value; }
        }

        private void GetReport()
        {
            FileStream xNewFile = null;
            try
            {
                string[] xPara = Request.QueryString["keys"].Split('|');

                if (xPara.Length > 0)
                {
                    DataSet xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                                     "GetEduIssuingUserReport",
                                                     LMS_SYSTEM.EDUMANAGEMENT,
                                                     "CLT.WEB.UI.LMS.EDUM"
                                                     , (object)xPara);

                    iDtCourseResult = xDs.Tables["table0"];
                    //DataTable xDtLecturer = xDs.Tables["table1"];

                    //이미지저장

                    string xUrl = "http://" + Request.Url.Authority;
                    string xFilePath = "/file/tempfile/report";
                    string xFileFullPath = "";

                    for (int i = 0; i < iDtCourseResult.Rows.Count; i++)
                    {
                        xFileFullPath = "";
                        if (iDtCourseResult.Rows[i]["pic_file"] != DBNull.Value)
                        {
                            byte[] xImgByte = (byte[])iDtCourseResult.Rows[i]["pic_file"];
                            string fileName = Convert.ToString(iDtCourseResult.Rows[i]["pic_file_nm"]);
                            string xFilePathCurr = Server.MapPath(xFilePath + "/" + fileName);

                            xNewFile = new FileStream(xFilePathCurr, FileMode.Create);
                            xNewFile.Write(xImgByte, 0, xImgByte.Length);
                            xNewFile.Close();

                            xFileFullPath = xUrl + xFilePath + "/" + fileName;
                        }
                        iDtCourseResult.Rows[i]["pic_file_nm"] = xFileFullPath;

                        /* 강사명 고정
                        for (int j = 0; j < xDtLecturer.Rows.Count; j++)
                        {
                            string xLecturerNM = Convert.ToString(iDtCourseResult.Rows[i]["LECTURER_NM"]);
                            if (!string.IsNullOrEmpty(xLecturerNM))
                                xLecturerNM = xLecturerNM + "|";
                            iDtCourseResult.Rows[i]["LECTURER_NM"] = xLecturerNM + Convert.ToString(xDtLecturer.Rows[j]["LECTURER_NM"]);    
                        }
                         * */

                        iDtCourseResult.Rows[i]["REPORT_CNT"] = string.Format("{0:000#}", Convert.ToInt32(iDtCourseResult.Rows[i]["REPORT_CNT"]) + i + 1);

                        if (!string.IsNullOrEmpty(Convert.ToString(iDtCourseResult.Rows[i]["personal_no"])) && Convert.ToString(iDtCourseResult.Rows[i]["personal_no"]).Length > 6)
                        {
                            string xBirthDay = Convert.ToString(iDtCourseResult.Rows[i]["personal_no"]).Substring(0, 6);
                            iDtCourseResult.Rows[i]["birth"] = "19" + xBirthDay.Substring(0, 2) + "-" + xBirthDay.Substring(2, 2) + "-" + xBirthDay.Substring(4, 2);
                        }

                        if (Convert.ToString(iDtCourseResult.Rows[i]["COUNTRY_KIND"]) == "KR")
                        {
                            iDtCourseResult.Rows[i]["COUNTRY_KIND_NM"] = "R.O.KOREA";
                        }

                        DateTime xBeginDt = Convert.ToDateTime(iDtCourseResult.Rows[i]["course_begin_dt"]);
                        DateTime xEndDt = Convert.ToDateTime(iDtCourseResult.Rows[i]["course_end_dt"]);
                        iDtCourseResult.Rows[i]["course_begin_dt_eng"] = xBeginDt.ToString("dd") + "th " + xBeginDt.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture) + ", " + xBeginDt.ToString("yyyy");
                        iDtCourseResult.Rows[i]["course_end_dt_eng"] = xEndDt.ToString("dd") + "th " + xEndDt.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture) + ", " + xEndDt.ToString("yyyy");

                        if (Convert.ToInt16(xBeginDt.ToString("yyyy")) >= 2008 && iDtCourseResult.Rows[i]["COURSE_GUBUN"].ToString() == "Y") // 국토해양부 과정이면
                        {
                            pnl0.Visible = true; //국토해양부 로고 표시
                            pnl1.Visible = false;
                        }
                        else
                        {
                            pnl0.Visible = false; //국토해양부 로고 미표시

                            pnl1.Visible = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            finally
            {
                if (xNewFile != null)
                    xNewFile.Dispose();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USER_GROUP"].ToString() == this.GuestUserID)
            {
                string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                ScriptHelper.ScriptBlock(this, "issuing_report", xScriptMsg);

                return;
            }

            if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
            {
                if (!IsPostBack)
                {
                    GetReport();
                }
            }
        }
    }
}
