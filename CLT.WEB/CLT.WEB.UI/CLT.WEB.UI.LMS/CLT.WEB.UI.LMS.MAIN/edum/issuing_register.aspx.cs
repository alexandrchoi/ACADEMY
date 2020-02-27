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
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.FX.AGENT;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.EDUM
{
    public partial class issuing_register : BasePage
    {   
        public string iSearch { get { return Util.Request("search"); } }
        public string iUserId { get { return Util.Split(Util.Request("search"),"^", 4)[0]; } }
        public string iRecordId { get { return Util.Split(Util.Request("search"), "^", 4)[1]; } }
        public string iUserNMKor { get { return Util.Split(Util.Request("search"), "^", 4)[2]; } }
        //public string iCourseID { get { return Util.Split(Util.Request("search"), "^", 4)[3]; } }

        private void BindData()
        {
            try
            {
                string[] xParams = Util.Split(iSearch, "^", 4);

                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                                 "GetEduTrainigRecord",
                                                 LMS_SYSTEM.CAPABILITY,
                                                 "CLT.WEB.UI.LMS.APPR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                if (xDt != null && xDt.Rows.Count > 0)
                {
                    DataRow dr = xDt.Rows[0];

                    lblUserId.Text = dr["user_id"].ToString();
                    lblUserNMKor.Text = Convert.ToString(dr["USER_NM_KOR"]);
                    txtCourseID.Value = dr["course_id"].ToString();
                    txtCourseNM.Value = dr["course_nm"].ToString();
                    txtSTART_DATE.Text = dr["course_begin_dt"].ToString();
                    txtEND_DATE.Text = dr["course_end_dt"].ToString();
                    ddlInstitution.SelectedValue = Convert.ToString(dr["learning_institution"]);
                }
                else
                {
                    lblUserId.Text = iUserId;
                    lblUserNMKor.Text = iUserNMKor;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /************************************************************
        * Function name : Page_Load
        * Purpose       : 페이지 로드될 때 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "issuing_register", xScriptMsg);

                    return;
                }

                base.pRender(this.Page, new object[,] { 
                                                            { this.btnSave, "E" }
                                                          }, Convert.ToString(Request.QueryString["MenuCode"]));

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (!IsPostBack)
                    {
                        this.txtSTART_DATE.Attributes.Add("onkeyup", "ChkDate(this);");
                        this.txtEND_DATE.Attributes.Add("onkeyup", "ChkDate(this);");

                        string[] xParams = new string[1];
                        DataTable xDt = null;

                        //Institution 
                        xParams[0] = "0007";
                        xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                     "GetCommonCodeInfo",
                                                     LMS_SYSTEM.EDUMANAGEMENT,
                                                     "CLT.WEB.UI.LMS.EDUM", (object)xParams, Thread.CurrentThread.CurrentCulture);
                        WebControlHelper.SetDropDownList(this.ddlInstitution, xDt, WebControlHelper.ComboType.NullAble);

                        if (!Util.IsNullOrEmptyObject(iSearch))
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(Session["USER_GROUP"])) && Convert.ToString(Session["USER_GROUP"]) != "000009")
                            {
                                BindData();
                                //btnCalendar.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;

                string[] xParams = new string[6];
                xParams[1] = lblUserId.Text;
                xParams[2] = txtCourseID.Value;
                xParams[3] = ddlInstitution.SelectedValue;
                xParams[4] = txtSTART_DATE.Text;
                xParams[5] = txtEND_DATE.Text;

                if (Util.IsNullOrEmptyObject(iSearch))
                {
                    xParams[0] = "";
                }
                else // 수정
                {
                    xParams[0] = iRecordId;
                }

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                             "SetEduTrainingRecord",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.APPR",
                                             (object)xParams);
                
                if (xRtn.ToUpper() == "TRUE")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001", new string[] { "교육결과 입력" }, new string[] { "Register training Record" }, Thread.CurrentThread.CurrentCulture));
                    if (string.IsNullOrEmpty(iRecordId))
                    {
                        txtSTART_DATE.Text = "";
                        txtEND_DATE.Text = "";
                        txtCourseNM.Value = "";
                        txtCourseID.Value = "";
                        ddlInstitution.SelectedValue = "";
                    }
                }
                else
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                }
                
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }

        }
    }
}
