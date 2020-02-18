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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.APPR
{
    /// <summary>
    /// 1. 작업개요 : 역량평가항목 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 역량평가항목
    ///				  
    /// 3. Class 명 : code_edit
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    /// 
    public partial class code_edit : BasePage
    {
        bool IsNew = false;
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
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "code_edit", xScriptMsg);

                    return;
                }

                // List 페이지로부터 Grade를 전달받은 경우 해당 데이터 바인딩

                // 그렇지 않은 경우 새로운 데이터 추가 하도록 클리어
                base.pRender(this.Page, new object[,] { { this.btnSend, "I" } }, Convert.ToString(Request.QueryString["MenuCode"]));

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (Request.QueryString["grade"] != null && Request.QueryString["grade"].ToString() != "")
                    {
                        if (Request.QueryString["grade"].ToString() == "new")
                        {
                            this.IsNew = true;
                        }
                        else
                        {
                            btnSend.Visible = false;
                            ViewState["grade"] = Request.QueryString["grade"].ToString();
                            this.txtGrade.Enabled = false;

                            if (!IsPostBack && Convert.ToString(Session["USER_GROUP"]) != "000009" && !String.IsNullOrEmpty(Convert.ToString(Session["USER_GROUP"])))
                            {
                                this.BindData();
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
        
        /******************************************************************************************
        * Function name : BindData
        * Purpose       : 넘겨받은 grade에 해당하는 데이터를 페이지의 컨트롤에 바인딩 처리
        * Input         : void
        * Output        : void
        ******************************************************************************************/
        private void BindData()
        {
            try
            {
                string[] xParams = new string[1];
                DataTable xDt = null;

                xParams[0] = ViewState["grade"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                                 "GetApprCodeInfo",
                                                 LMS_SYSTEM.CAPABILITY,
                                                 "CLT.WEB.UI.LMS.APPR", (object)xParams);

                if (xDt != null && xDt.Rows.Count > 0)
                {
                    DataRow dr = xDt.Rows[0];

                    this.txtGrade.Text = dr["grade"].ToString();
                    this.txtGradeNM.Text = dr["grade_nm"].ToString();
                    this.txtScore.Text = dr["score"].ToString();
                    this.txtGradeDesc.Text = dr["grade_desc"].ToString();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /******************************************************************************************
        * Function name : btnSend_Click
        * Purpose       : 수정, 추가된 항목 저장 후 선박으로 발송
        * Input         : void
        * Output        : void
        ******************************************************************************************/
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;

                // 필수입력항목 체크
                if (string.IsNullOrEmpty(this.txtGrade.Text))
                {
                    xScriptMsg = "<script>alert('등급을 입력하여 주세요!');</script>";
                    ScriptHelper.ScriptBlock(this, "code_edit", xScriptMsg);
                    this.txtGrade.Focus();
                    return;
                }

                string[] xParams = new string[5];

                xParams[0] = Util.ForbidText(this.txtGrade.Text);
                xParams[1] = Util.ForbidText(this.txtGradeNM.Text);
                xParams[2] = Util.ForbidText(this.txtScore.Text);
                xParams[3] = Util.ForbidText(this.txtGradeDesc.Text);
                xParams[4] = Session["USER_ID"].ToString();

                if (IsNew)   // 추가
                {
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                             "SetApprCodeInsert",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.APPR",
                                             (object)xParams);

                    if (xRtn.ToUpper() == "TRUE")
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001", new string[] { "등급평가코드" }, new string[] { "Competence evaluation code" }, Thread.CurrentThread.CurrentCulture));
                    }
                    else if (xRtn.ToUpper() == "DLC")
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A014", new string[] { "등급평가코드" }, new string[] { "Competence evaluation code" }, Thread.CurrentThread.CurrentCulture));
                    }
                    else
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                    }
                }
                else // 수정
                {
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.APPR.vp_a_appraisal_md",
                                             "SetApprCodeUpdate",
                                             LMS_SYSTEM.CAPABILITY,
                                             "CLT.WEB.UI.LMS.APPR",
                                             (object)xParams);

                    if (xRtn.ToUpper() == "TRUE")
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001", new string[] { "등급평가코드" }, new string[] { "Competence evaluation code" }, Thread.CurrentThread.CurrentCulture));
                    }
                    else
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

    }
}
