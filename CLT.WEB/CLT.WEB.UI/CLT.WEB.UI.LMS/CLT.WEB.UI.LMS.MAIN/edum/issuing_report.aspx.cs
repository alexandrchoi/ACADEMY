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
    public partial class issuing_report : BasePage
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

                    if (iDtCourseResult.Rows.Count > 0)
                    {
                        if (Convert.ToDouble(iDtCourseResult.Rows[0]["REPORT_TYPE_ID"].ToString()) > 10)
                        {
                            Response.Redirect("issuing_report_old.aspx?keys=" + Request.QueryString["keys"]);
                        }
                    }
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

                        if (string.IsNullOrEmpty(Convert.ToString(iDtCourseResult.Rows[i]["birth_dt"])))
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(iDtCourseResult.Rows[i]["personal_no"])) && Convert.ToString(iDtCourseResult.Rows[i]["personal_no"]).Length > 6)
                            {
                                string xYear = "19";
                                string xCond = Convert.ToString(iDtCourseResult.Rows[i]["personal_no"]).Substring(7, 1);
                                if (xCond == "3" || xCond == "4" || xCond == "7" || xCond == "8") xYear = "20";

                                string xBirthDay = Convert.ToString(iDtCourseResult.Rows[i]["personal_no"]).Substring(0, 6);
                                iDtCourseResult.Rows[i]["birth_dt"] = xYear + "" + xBirthDay.Substring(0, 2) + "." + xBirthDay.Substring(2, 2) + "." + xBirthDay.Substring(4, 2);
                                DateTime xBirthDt = Convert.ToDateTime(iDtCourseResult.Rows[i]["birth_dt"]);
                                iDtCourseResult.Rows[i]["birth_dt_eng"] = xBirthDt.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture) + " " + Convert.ToInt32(xBirthDt.ToString("dd")) + ", " + xBirthDt.ToString("yyyy");
                            }
                        }
                        //if (Convert.ToString(iDtCourseResult.Rows[i]["COUNTRY_KIND"]) == "KR")
                        //{
                        //    iDtCourseResult.Rows[i]["COUNTRY_KIND_NM"] = "R.O.KOREA";
                        //}
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //DateTime xBeginDt = Convert.ToDateTime(iDtCourseResult.Rows[i]["course_begin_dt"]);
                        //DateTime xEndDt = Convert.ToDateTime(iDtCourseResult.Rows[i]["course_end_dt"]);
                        //iDtCourseResult.Rows[i]["course_begin_dt_eng"] = xBeginDt.ToString("dd") + "th " + xBeginDt.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture) + ", " + xBeginDt.ToString("yyyy");
                        //iDtCourseResult.Rows[i]["course_end_dt_eng"] = xEndDt.ToString("dd") + "th " + xEndDt.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture) + ", " + xEndDt.ToString("yyyy");

                        //if (Convert.ToInt16(xBeginDt.ToString("yyyy")) >= 2008 && iDtCourseResult.Rows[i]["COURSE_GUBUN"].ToString() == "Y") // 국토해양부 과정이면
                        //{
                        //pnl0.Visible = true; //국토해양부 로고 표시
                        //pnl1.Visible = false;
                        //}
                        //else
                        //{
                        //pnl0.Visible = false; //국토해양부 로고 미표시
                        //pnl1.Visible = true;
                        //}
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        string xReportDescKor = "";
                        string xReportDescEng = "";

                        switch (iDtCourseResult.Rows[i]["REPORT_TYPE_ID"])
                        {
                            case "000001":
                                //1. 선박조종시뮬레이터 교육
                                xReportDescKor = "위 사람은 STCW 개정협약 및 코드 제 Ⅰ장 표 A-Ⅰ/12, 제Ⅱ장 A-Ⅱ / 1, A - Ⅱ / 2 및 B-Ⅰ / 12, B - V / a의 규정에 의거하여 실시한 선박조종시뮬레이터 교육 과정을 수료하였음을 증명합니다.";
                                xReportDescEng = "This is to certify that the above person has successfully completed a course \"Ship Handling Simulator\" in accordance with the provision of ChapterⅠ Section A-Ⅰ / 12, Chapter Ⅱ Section A Tables A-Ⅱ / 1, A - Ⅱ / 2 and Section B - Ⅰ / 12 and B-V / a of STCW Convention & Code(1978 and 2010 Amendment), applicable to issue of  this certificate.";
                                
                                break;
                            case "000002":
                                //2. 선박모의조종 및 선교 팀워크 교육
                                xReportDescKor = "위 사람은 STCW 개정협약 및 코드 제 Ⅰ장 표 A-Ⅰ/12, 제Ⅱ장 A-Ⅱ / 1, A - Ⅱ / 2 및 IMO Model Course 1.22의 규정에 의거하여 실시한 선박 모의조종 및 선교 팀워크 교육 과정을 수료하였음을 증명합니다.";
                                xReportDescEng = "This is to certify that the above person has successfully completed a course \"Ship Simulator and Bridge Teamwork\" in accordance with the provision of Chapter Ⅰ Section A - Ⅰ / 12, Chapter Ⅱ Section A Tables A-Ⅱ / 1, A - Ⅱ / 2 of STCW Convention & Code(1978 and 2010 Amendment) and IMO Model Course 1.22, applicable to issue of this certificate.";
                                
                                break;
                            case "000003":
                                //3. 엔진룸 시뮬레이터 교육
                                xReportDescKor = "위 사람은 STCW 개정협약 및 코드 제 Ⅲ장 표 A-Ⅲ/1의 규정에 의거하여 실시한 엔진룸 시뮬레이터 교육 과정을 수료하였음을 증명합니다.";
                                xReportDescEng = "This is to certify that the above person has successfully completed a course \"Engine Room Simulator\" in accordance with the provision of Chapter Ⅲ Section A Tables A - Ⅲ / 1 of STCW Convention & Code (1978 and 2010 Amendment), applicable to issue of this certificate.";
                                
                                break;
                            case "000004":
                                //4. 전자해도장치 교육
                                xReportDescKor = "위 사람은 STCW 개정협약 및 코드 제Ⅱ장 A-Ⅱ/1, A-Ⅱ/2, A - Ⅱ / 3 및 IMO Model Course 1.27의 규정에 의거하여 전자해도장치(ECDIS) 교육 과정을 수료하였음을 증명합니다.";
                                xReportDescEng = "This is to certify that the above person has successfully completed a course \"The operational use of Electronic Chart Display and Information System(ECDIS)\" in accordance with the provision of Chapter Ⅱ Section A Tables A-Ⅱ/1, A-Ⅱ/2 and A-Ⅱ/3 of STCW Convention and Code(1978 and 2010 Amendment) and IMO Model Course 1.27, applicable to issue of  this certificate.";
                                
                                break;
                            case "000005":
                                if (iDtCourseResult.Rows[i]["ME_GUBUN"].ToString() == "M")     /* 직급 M 항해 E 기관 */
                                {
                                    //5. 리더십 및 팀워크교육 (항해)
                                    xReportDescKor = "위 사람은 STCW 개정협약 및 코드 제 Ⅱ장 표 A-Ⅱ/1의 규정에 의거하여 실시한 리더십 및 팀워크교육 과정 (선교자원관리 포함)을 수료하였음을 증명합니다.";
                                    xReportDescEng = "This is to certify that the above person has successfully completed a course \"Leadership and Teamwork\"in accordance with the provision of Chapter Ⅱ Section A Tables A-Ⅱ/1 of STCW Convention and Code(1978 and 2010 Amendment), and also met requirements for Bridge Resource Management set out in Tables A-Ⅱ/1 of STCW Convention & Code(1978 and 2010 Amendment), applicable to issue of this certificate.";
                                }
                                else
                                {
                                    //6. 리더십 및 팀워크교육 (기관)
                                    xReportDescKor = "위 사람은 STCW 개정협약 및 코드 제 Ⅲ장 표 A-Ⅲ/1의 규정에 의거하여 실시한 리더십 및 팀워크교육 과정 (기관실자원관리 포함)을 수료하였음을 증명합니다.";
                                    xReportDescEng = "This is to certify that the above person has successfully completed a course \"Leadership and Teamwork\"in accordance with the provision of Chapter Ⅲ Section A Tables A-Ⅲ/1 of STCW Convention and Code(1978 and 2010 Amendment), and also met requirements for Engine-Room Resource  Management set out in Tables A-Ⅲ/1 of STCW Convention & Code(1978 and 2010 Amendment), applicable to issue of this certificate.";
                                }
                                
                                break;
                            case "000006":
                                if (iDtCourseResult.Rows[i]["ME_GUBUN"].ToString() == "M")     /* 직급 M 항해 E 기관 */
                                {
                                    //7. 리더십 및 관리기술 직무교육 (항해)
                                    xReportDescKor = "위 사람은 STCW 개정협약 및 코드 제 Ⅱ장 표 A-Ⅱ/2의 규정에 의거하여 실시한 리더십 및 관리기술 직무교육 과정을 수료하였음을 증명합니다.";
                                    xReportDescEng = "This is to certify that the above person has successfully completed a course \"Leadership and Managerial skill\" in accordance with the provision of Chapter Ⅱ Section A Tables A-Ⅱ/2 of STCW Convention and Code (1978 and 2010 Amendment), applicable to issue of  this certificate.";
                                }
                                else
                                {
                                    //8. 리더십 및 관리기술 직무교육 (기관)
                                    xReportDescKor = "위 사람은 STCW 개정협약 및 코드 제 Ⅲ장 표 A-Ⅲ/2의 규정에 의거하여 실시한 리더십 및 관리기술 직무교육 과정을 수료하였음을 증명합니다.";
                                    xReportDescEng = "This is to certify that the above person has successfully completed a course \"Leadership and Managerial skill\" in accordance with the provision of Chapter Ⅲ Section A Tables A-Ⅲ/2 of STCW Convention and Code(1978 and 2010 Amendment), applicable to issue of  this certificate.";
                                }
                                
                                break;
                            case "000007":
                                //9. JRC ECDIS TST (901)
                                xReportDescKor = "";
                                xReportDescEng = "<table><tr><td>JAN-701/901/901M</td></tr><tr><td>JAN-701B/901B/2000</td></tr></table><!--<td rowspan=\"2\">JRC</td>-->";
                                
                                break;
                            case "000008":
                                //10. JRC ECDIS TST (9201)
                                xReportDescKor = "";
                                xReportDescEng = "JAN-7201/9201";
                                
                                break;
                            case "000009":
                                //11. 선박평형수관리협약교육
                                xReportDescKor = "위 사람은 국제해사기구의 선박평형수관리협약 제 B-6 규칙 및 대한민국 선박평형수관리법 제9조 제3항에 따른 교육 과정을 수료하였음을 증명합니다.";
                                xReportDescEng = "This is to certify that the above person has completed the training course according to IMO's Ballast Water Convention, Section B, Regulation B-6 and the Republic of Korea's Ballast Water Management Act, Article 9.3";
                                
                                break;
                            case "000010":
                                if (iDtCourseResult.Rows[i]["COURSE_INOUT"].ToString() == "000001")     /* 000001 사내 000002 사외 */
                                {
                                    //12. 선박위험물관리교육 - 내부
                                    xReportDescKor = "위 사람은 STCW 개정협약 및 코드 제 Ⅱ장 표 A-Ⅱ/1, A - Ⅱ / 2, 제 Ⅶ장, MARPOL 73 / 78 부속서 Ⅲ 및 49 CFR 172의 규정에 의거하여 실시한 선박위험물관리교육 과정을 수료하였음을 증명합니다.";
                                    xReportDescEng = "This is to certify that the above person had completed the training course \"Transportation of Dangerous goods\" in accordance with STCW A-Ⅱ/1, A-Ⅱ/2, IMDG Code, SOLAS 1974 amended Chapter Ⅶ, MARPOL 73/78 Annex Ⅲ and 49 CFR 172 and been qualified for cargo handling on ships carrying dangerous and hazardous substances in packaged form.";
                                }
                                else
                                {
                                    //13. 선박위험물관리교육 - 외부
                                    xReportDescKor = "위 사람은 STCW 개정협약 및 코드 제 Ⅱ장 표 A-Ⅱ/1, A - Ⅱ / 2, 제 Ⅶ장, MARPOL 73 / 78 부속서 Ⅲ 및 49 CFR 172의 규정에 의거하여 실시한 선박위험물관리교육 과정을 수료하였음을 증명합니다.";
                                    xReportDescEng = "This is to certify that the above person had completed the training course \"Transportation of Dangerous goods\" in accordance with STCW A-Ⅱ/1, A-Ⅱ/2, IMDG Code, SOLAS 1974 amended Chapter Ⅶ, MARPOL 73/78 Annex Ⅲ and 49 CFR 172 and been qualified for cargo handling on ships carrying dangerous and hazardous substances in packaged form.";
                                }

                                break;
                            default:
                                break;
                        }

                        iDtCourseResult.Rows[i]["REPORT_DESC_KOR"] = xReportDescKor;
                        iDtCourseResult.Rows[i]["REPORT_DESC_ENG"] = xReportDescEng;
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
