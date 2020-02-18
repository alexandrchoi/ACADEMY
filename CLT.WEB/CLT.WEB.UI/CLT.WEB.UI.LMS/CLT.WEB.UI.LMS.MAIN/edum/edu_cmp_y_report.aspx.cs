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
using CLT.WEB.UI.COMMON.BASE;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.EDUM
{
    public partial class edu_cmp_y_report : BasePage
    {
        private DataTable iDtCourseResult;

        public DataTable IDtCourseResult
        {
            get { return iDtCourseResult; }
            set { iDtCourseResult = value; }
        }
	
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list_wpg", xScriptMsg);

                    return;
                }

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    string rRptName = Request.QueryString["rpt"];  //report xml파일명을 받아서, 분기 시키면 될듯. 

                    if (rRptName == "vp_s_agreement_certificate_report.xml")
                    {
                        //마이페이지 > 수료현황 > 수료증 출력에서 사용함 
                        this.Report_Mypage_Certificate(rRptName);
                    }
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

                if (string.IsNullOrEmpty(xUser))
                {
                    string[] xPara = xOpencourse.Split('|');
                    iDtCourseResult = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMISSION.vp_s_record_md",
                                                     "GetAgreementCertificateReport",
                                                     LMS_SYSTEM.COMMISSION,
                                                     "CLT.WEB.UI.LMS.COMMISSION.vp_s_report_wpg", (object)xPara, "");
                }
                else
                {   
                    iDtCourseResult = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_training_md",
                                                     "GetTrainingReport",
                                                     LMS_SYSTEM.MYPAGE,
                                                     "CLT.WEB.UI.LMS.COMMISSION.vp_s_report_wpg", xOpencourse, xUser, "");
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
