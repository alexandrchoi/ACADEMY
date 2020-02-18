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
using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading;

using C1.Web.C1WebReport;
using C1.Win.C1Report;
using System.IO;
using Microsoft.Win32;

namespace CLT.WEB.UI.LMS.APPLICATION
{
    public partial class report : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string rRptName = Request.QueryString["rpt"];  //report xml파일명을 받아서, 분기 시키면 될듯. 

                if (rRptName == "agreement_report.xml")
                {
                    this.Report_Aggrement(rRptName);
                }
                else if (rRptName == "agreement_contract_report.xml")
                {
                    this.Report_Aggrement_Contract(rRptName);
                }
                else if (rRptName == "agreement_certificate_report.xml")
                {
                    this.Report_Aggrement_Certificate(rRptName);
                }
                else if (rRptName == "agreement_list_report.xml")
                {
                    this.Report_Aggrement_List(rRptName);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }



        private void Report_Aggrement(string rRptName)
        {
            try
            {
                string[] xPara = new string[3] { Session["COMPANY_ID"].ToString(), Request.QueryString["open_course_id"], Session["COMPANY_KIND"].ToString() }; 
                DataSet xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.APPLICATION.vp_s_agreement_md",
                                                 "GetAgreementReport",
                                                 LMS_SYSTEM.APPLICATION,
                                                 "CLT.WEB.UI.LMS.APPLICATION.vp_s_report", (object)xPara);

                DataTable xDtCompany = xDs.Tables["t_company"];
                DataTable xDtCourseResult = xDs.Tables["t_course_result"];
                DataTable xDtCourse = xDs.Tables["t_course"];
                DataTable xDtTextBook = xDs.Tables["t_textbook"];
                DataTable xDtCourseResultCnt = xDs.Tables["t_course_result_cnt"];

                int xFee = 0;
                int xTotalFee = 0;
                if (Convert.ToString(xDtCourse.Rows[0]["training_fee"]) != "")
                {
                    xFee = Convert.ToInt32(xDtCourse.Rows[0]["training_fee"]);
                    xTotalFee = xFee * (Convert.ToString(xDtCourseResultCnt.Rows[0]["user_cnt"]) == "" ? 0 : Convert.ToInt32(xDtCourseResultCnt.Rows[0]["user_cnt"]));
                }

                C1WebReport1.Report.UsePrinterResolution = false;
                //C1WebReport1.NavigationBar.HasGotoPageButton = true;
                //C1WebReport1.EnableCallback = true;
                //C1WebReport1.NavigationBar.HasExportButton = true;
                //C1.Web.C1WebReport.ExportFormatsEnum formats = 0;
                //formats = formats | C1.Web.C1WebReport.ExportFormatsEnum.XLS;
                //C1WebReport1.NavigationBar.ExportFormats = formats;
                //C1WebReport1.ExportRenderMethod = C1.Web.C1WebReport.ExportRenderMethodEnum.File;
                //C1WebReport1.NavigationBar.Visible = true;

                C1Report xRpt = C1WebReport1.Report;
                xRpt.Load(Request.MapPath("/report/" + rRptName), "Agreement_Report");


                xRpt.Fields["agree_title"].Text = string.Format("본 계약의 '{0}'(이하 '갑'이라 한다.)와 '한진해운 운항훈련원((주)유수에스엠)'(이하 '을'이라 한다.)은 '갑'의 {1}에 대한 위탁계약을 다음과 같이 체결한다.",
                                                                Convert.ToString(xDtCompany.Rows[0]["company_nm"]),
                                                                Convert.ToString(xDtCourse.Rows[0]["course_nm"]));
                xRpt.Fields["agree_no1"].Text = string.Format("제1조(계약범위)\r\n  '갑'이 위탁한 {0} 및 수행을 위한 다음 사항을 계약의 범위로 한다.\r\n    가. 훈련과정은 노동부로부터 지정받은 훈련과정으로 다음과 같다.",
                                                              Convert.ToString(xDtCourse.Rows[0]["course_nm"]));
                xRpt.Fields["agree_no2"].Text = "  나. '갑'의 훈련요구에 따른 훈련운영에 관란 사항에 대한 협조\r\n   다. 고용보험 환급에 관한 사항 '갑'이 고용보험 환급을 받기 위해 필요한 제반 사항에 대한 협조";
                xRpt.Fields["agree_no3"].Text = string.Format("제2조(계약기간 및 인원)\r\n  가. 계약기간은 {0}을 수행하는 훈련기간을 의미한다.\r\n  나. 훈련기간 및 훈련인원\r\n    1) 훈련기간 : {1}\r\n    2) 인원 : {2}명",
                                                              Convert.ToString(xDtCourse.Rows[0]["course_nm"]),
                                                              Convert.ToString(xDtCourse.Rows[0]["course_begin_dt"]) + "~" + Convert.ToString(xDtCourse.Rows[0]["course_end_dt"]),
                                                              Convert.ToString(xDtCourseResultCnt.Rows[0]["user_cnt"]));
                // 과정 정보 Binding
                string xTmp = null;
                xRpt.Fields["agree_no1_1"].Text = Convert.ToString(xDtCourse.Rows[0]["course_nm"]);
                xRpt.Fields["agree_no1_2"].Text = Convert.ToString(xDtCourse.Rows[0]["course_day_time"]);
                xRpt.Fields["agree_no1_3"].Text = Convert.ToString(xDtCourse.Rows[0]["class_man_count"]);
                xTmp = Convert.ToString(xDtCourse.Rows[0]["course_begin_dt"]) + "\r\n~\r\n" + Convert.ToString(xDtCourse.Rows[0]["course_end_dt"]);
                xRpt.Fields["agree_no1_4"].Text = xTmp;
                xTmp = null;
                foreach (DataRow dr in xDtTextBook.Rows)
                {
                    xTmp += Convert.ToString(dr["textbook_nm"]) + "/";
                }
                xRpt.Fields["agree_no1_5"].Text = xTmp;
                xRpt.Fields["agree_no1_6"].Text = string.Format("{0:#,##0}", xFee);
                xRpt.Fields["agree_no4"].Text = string.Format("제3조(계약금액 및 지급)\r\n  가. 총 계약금액은 {0}원 으로 한다.\r\n    본 금액은 1인당 {1}원이며, 훈련인원의 변경시 자동 조정된다.",
                                                              string.Format("{0:#,##0}", xTotalFee),
                                                              string.Format("{0:#,##0}", xFee));
                xRpt.Fields["agree_no9"].Text = DateTime.Now.Year.ToString() + "년   " + DateTime.Now.Month.ToString() + "월   " + DateTime.Now.Day.ToString() + "일";
                //xRpt.Fields["agree_no7"].Text = string.Format("갑\r\n\r\n회사명     {0}\r\n\r\n대 표 자     {1}          (인)", Convert.ToString(xDtCompany.Rows[0]["company_nm"]), Convert.ToString(xDtCompany.Rows[0]["company_ceo"]));

                // 과정 교육 결과 정보 Binding
                Field xFSub;
                C1Report xSub = new C1Report();
                xFSub = xRpt.Fields["Field6"];
                xSub = xFSub.Subreport;
                xSub.DataSource.ConnectionString = string.Empty;
                xSub.DataSource.Recordset = xDtCourseResult;

                xRpt.DataSource.ConnectionString = string.Empty;
                string xImgPath = @"http://" + Request.Url.Authority + "/report/sign02.gif";
                xRpt.Fields["logo1"].Picture = xImgPath;

                //RegistryKey xRegkey = Registry.CurrentUser;
                //xRegkey = xRegkey.OpenSubKey(@"Software\Microsoft\Internet Explorer\PageSetup", true);

                //string xFooter = string.Empty;
                //string xHeader = string.Empty;
                //string xMarginBottom = string.Empty;
                //string xMarginLeft = string.Empty;
                //string xMarginRight = string.Empty;
                //string xMarginTop = string.Empty;
                //if (xRegkey != null)
                //{
                //    xFooter = xRegkey.GetValue("footer").ToString();
                //    xMarginBottom = xRegkey.GetValue("margin_bottom").ToString();
                //    xMarginLeft = xRegkey.GetValue("margin_left").ToString();
                //    xMarginRight = xRegkey.GetValue("margin_right").ToString();
                //    xMarginTop = xRegkey.GetValue("margin_top").ToString();

                //    xRegkey.SetValue("footer", "", RegistryValueKind.String);
                //    xRegkey.SetValue("margin_bottom", "0.78740", RegistryValueKind.String);
                //    xRegkey.SetValue("margin_left", "0.59055", RegistryValueKind.String);
                //    xRegkey.SetValue("margin_right", "0.59055", RegistryValueKind.String);
                //    xRegkey.SetValue("margin_top", "0.78740", RegistryValueKind.String);
                //}

                ////Thread.Sleep(3000);

                //xRegkey.SetValue("footer", xFooter, RegistryValueKind.String);
                //xRegkey.SetValue("margin_bottom", xMarginBottom, RegistryValueKind.String);
                //xRegkey.SetValue("margin_left", xMarginLeft, RegistryValueKind.String);
                //xRegkey.SetValue("margin_right", xMarginRight, RegistryValueKind.String);
                //xRegkey.SetValue("margin_top", xMarginTop, RegistryValueKind.String);

                using (MemoryStream oStream = new MemoryStream())
                {
                    xRpt.RenderToStream(oStream, FileFormatEnum.HTMLPaged, "", "Agreement_Report.htm");
                    xRpt.Dispose();

                    oStream.Position = 0;
                    string xScrptStr = string.Empty;
                    xScrptStr += "<script language=\"javascript\">window.onload = function(){window.print();}</script>";

                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

                    string htmlWrite = encoding.GetString(oStream.ToArray());
                    htmlWrite += xScrptStr;

                    Response.Clear();
                    Response.Write(htmlWrite);
                    Response.End();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

           
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        private void Report_Aggrement_Contract(string rRptName)
        {
            try
            {
                string[] xPara = new string[3] { Session["COMPANY_ID"].ToString(), Request.QueryString["open_course_id"], Session["COMPANY_KIND"].ToString() };
                DataSet xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.APPLICATION.vp_s_agreement_md",
                                                 "GetAgreementContractReport",
                                                 LMS_SYSTEM.APPLICATION,
                                                 "CLT.WEB.UI.LMS.APPLICATION.vp_s_report", (object)xPara);

                DataTable xDtCompany = xDs.Tables["t_company"];
                DataTable xDtCourseResult = xDs.Tables["t_course_result"];
                DataTable xDtCourse = xDs.Tables["t_course"];

                C1Report xRpt = C1WebReport1.Report;
                xRpt.Load(Request.MapPath("/report/" + rRptName), "Agreement_Contract_Report");

                if (xDtCourse != null && xDtCourse.Rows.Count > 0)
                {
                    xRpt.Fields["course_nm"].Text = Convert.ToString(xDtCourse.Rows[0]["course_nm"]);
                    xRpt.Fields["course_begin_end_dt"].Text = Convert.ToString(xDtCourse.Rows[0]["course_begin_dt"]) + "~" + Convert.ToString(xDtCourse.Rows[0]["course_end_dt"]);
                }

                if (xDtCompany != null && xDtCompany.Rows.Count > 0)
                {
                    xRpt.Fields["company_ceo"].Text = Convert.ToString(xDtCompany.Rows[0]["company_ceo"]);
                    xRpt.Fields["company_addr"].Text = Convert.ToString(xDtCompany.Rows[0]["company_addr"]);

                    xRpt.Fields["company_nm"].Text = Convert.ToString(xDtCompany.Rows[0]["company_nm"]);
                    xRpt.Fields["company_ceo_bottom"].Text = Convert.ToString(xDtCompany.Rows[0]["company_ceo"]) + "          (인)";
                }

                if (xDtCourse != null && xDtCourse.Rows.Count > 0)
                {
                    xRpt.Fields["agree_contract_text"].Text = string.Format(" 위 훈련생을 {0}({1}) 수료 후 정당한 사유가 없는 한 채용할 예정이며 훈련생에게 채용 후 근로조건(급여 및 복지 등)에 관한 사항을 설명하였음을 확인 합니다.",
                                                                            Convert.ToString(xDtCourse.Rows[0]["course_nm"]),
                                                                            Convert.ToString(xDtCourse.Rows[0]["course_begin_dt"]) + "~" + Convert.ToString(xDtCourse.Rows[0]["course_end_dt"]));
                }
                xRpt.Fields["agree_contract_datetime"].Text = DateTime.Now.Year.ToString() + "년   " + DateTime.Now.Month.ToString() + "월   " + DateTime.Now.Day.ToString() + "일";

                // 과정 교육 결과 정보 Binding
                Field xFSub;
                C1Report xSub = new C1Report();
                xFSub = xRpt.Fields["Field6"];
                xSub = xFSub.Subreport;
                xSub.DataSource.ConnectionString = string.Empty;
                xSub.DataSource.Recordset = xDtCourseResult;
                xRpt.DataSource.ConnectionString = string.Empty;

                using (MemoryStream oStream = new MemoryStream())
                {
                    xRpt.RenderToStream(oStream, FileFormatEnum.HTMLPaged, "", "Agreement_Contract_Report.htm");
                    xRpt.Dispose();

                    oStream.Position = 0;
                    string xScrptStr = string.Empty;
                    xScrptStr += "<script language=\"javascript\">window.onload = function(){window.print();}</script>";

                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

                    string htmlWrite = encoding.GetString(oStream.ToArray());
                    htmlWrite += xScrptStr;

                    Response.Clear();
                    Response.Write(htmlWrite);
                    Response.End();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }


        private void Report_Aggrement_List(string rRptName)
        {
            try
            {
                string[] xPara = Request.QueryString["open_course_id"].Split('|');

                if (xPara.Length > 0)
                {
                    DataSet xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.APPLICATION.vp_s_record_md",
                                                     "GetAgreementListReport",
                                                     LMS_SYSTEM.APPLICATION,
                                                     "CLT.WEB.UI.LMS.APPLICATION.vp_s_report", (object)xPara);

                    DataTable xDtCourseResult = xDs.Tables["t_course_result"];
                    DataTable xDtCourseResultSum = xDs.Tables["t_course_result_sum"];

                    C1Report xRpt = C1WebReport1.Report;
                    xRpt.Load(Request.MapPath("/report/" + rRptName), "Agreement_List_Report");

                    xRpt.Fields["agree_list_datetime"].Text = DateTime.Now.Year.ToString() + "년   " + DateTime.Now.Month.ToString() + "월   " + DateTime.Now.Day.ToString() + "일";

                    if (xDtCourseResultSum != null && xDtCourseResultSum.Rows.Count > 0)
                    {
                        xRpt.Fields["training_fee_sum"].Text = Convert.ToString(xDtCourseResultSum.Rows[0]["training_fee_sum"]);
                        xRpt.Fields["training_fee_tax_sum"].Text = Convert.ToString(xDtCourseResultSum.Rows[0]["training_fee_tax_sum"]);
                        xRpt.Fields["training_fee_total"].Text = Convert.ToString(xDtCourseResultSum.Rows[0]["training_fee_total"]);
                        xRpt.Fields["company_scale_fee_sum"].Text = Convert.ToString(xDtCourseResultSum.Rows[0]["company_scale_fee_sum"]);
                    }

                    // 과정 교육 결과 정보 Binding
                    Field xFSub;
                    C1Report xSub = new C1Report();
                    xFSub = xRpt.Fields["Field6"];
                    xSub = xFSub.Subreport;
                    xSub.DataSource.ConnectionString = string.Empty;
                    xSub.DataSource.Recordset = xDtCourseResult;
                    xRpt.DataSource.ConnectionString = string.Empty;
                    string xImgPath = @"http://" + Request.Url.Authority + "/report/sign01.jpg";
                    xRpt.Fields["logo1"].Picture = xImgPath;

                    using (MemoryStream oStream = new MemoryStream())
                    {
                        xRpt.RenderToStream(oStream, FileFormatEnum.HTMLPaged, "", "Agreement_List_Report.htm");
                        xRpt.Dispose();

                        oStream.Position = 0;
                        string xScrptStr = string.Empty;
                        xScrptStr += "<script language=\"javascript\">window.onload = function(){window.print();}</script>";

                        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

                        string htmlWrite = encoding.GetString(oStream.ToArray());
                        htmlWrite += xScrptStr;

                        Response.Clear();
                        Response.Write(htmlWrite);
                        Response.End();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        private void Report_Aggrement_Certificate(string rRptName)
        {
            try
            {
                string[] xPara = Request.QueryString["open_course_id"].Split('|');

                if (xPara.Length > 0)
                {
                    string xImgPath = @"http://" + Request.Url.Authority + "/report/sign01.jpg";
                    DataTable xDtCourseResult = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_s_record_md",
                                                     "GetAgreementCertificateReport",
                                                     LMS_SYSTEM.APPLICATION,
                                                     "CLT.WEB.UI.LMS.APPLICATION.vp_s_report", (object)xPara, xImgPath);
                    //xDtCourseResult.Columns.Add("logo1");
                    //xDtCourseResult.Columns["logo1"].DefaultValue = xImgPath;
                    //xDtCourseResult.Rows[0]["logo1"] = xImgPath;

                    C1Report xRpt = C1WebReport1.Report;
                    xRpt.Load(Request.MapPath("/report/" + rRptName), "Agreement_Certificate_Report");

                    // 과정 교육 결과 정보 Binding
                    Field xFSub;
                    C1Report xSub = new C1Report();
                    xFSub = xRpt.Fields["Field6"];
                    xSub = xFSub.Subreport;
                    xSub.Sections.Detail.OnFormat = "fImage.Picture = logo1";
                    xSub.DataSource.ConnectionString = string.Empty;
                    xSub.DataSource.Recordset = xDtCourseResult;
                    xRpt.DataSource.ConnectionString = string.Empty;

                    using (MemoryStream oStream = new MemoryStream())
                    {
                        xRpt.RenderToStream(oStream, FileFormatEnum.HTMLPaged, "", "Agreement_Certificate_Report.htm");
                        xRpt.Dispose();

                        oStream.Position = 0;
                        string xScrptStr = string.Empty;
                        xScrptStr += "<script language=\"javascript\">window.onload = function(){window.print();}</script>";

                        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

                        string htmlWrite = encoding.GetString(oStream.ToArray());
                        htmlWrite += xScrptStr;

                        Response.Clear();
                        Response.Write(htmlWrite);
                        Response.End();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
    }
}
