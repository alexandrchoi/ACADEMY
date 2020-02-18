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


namespace CLT.WEB.UI.LMS.COMMON
{
    public partial class report : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "report", xScriptMsg);

                    return;
                }

                string rRptName = Request.QueryString["rpt"];  //report xml파일명을 받아서, 분기 시키면 될듯. 

                if (rRptName == "agreement_certificate_report.xml")
                {
                    //마이페이지 > 수료현황 > 수료증 출력에서 사용함 
                    this.Report_Mypage_Certificate(rRptName);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        private void Report_Mypage_Certificate(string rRptName)
        {
            try
            {
                string xOpencourse = Request.QueryString["open_course_id"];
                string xUser = Request.QueryString["user_id"];

                if (xOpencourse.Length > 0)
                {
                    string xImgPath = @"http://" + Request.Url.Authority + "/report/sign01.jpg";
                    DataTable xDtCourseResult = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_training_md",
                                                     "GetTrainingReport",
                                                     LMS_SYSTEM.MYPAGE,
                                                     "CLT.WEB.UI.LMS.COMMISSION", xOpencourse, xUser, xImgPath);
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
