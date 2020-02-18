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

// 필수 using 문using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading; 

namespace CLT.WEB.UI.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : content_edit Class
    /// 
    /// 2. 주요기능 : 컨텐츠 수정에 따른 신규 컨텐츠 생성 후, 선박으로 전송하는 처리
    ///				  
    /// 3. Class 명 : content_edit
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.08
    /// </summary>
    public partial class content_edit : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 페이지 로드될 때 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list", xScriptMsg);

                    return;
                }

                // List 페이지로부터 contents_id를 전달받은 경우만 해당 페이지를 처리하고
                // 그렇지 않은 경우, 메세지 출력과 함께 창을 종료한다.            

                if (!IsPostBack)
                {
                    this.BindDropDownList();
                    this.ViewStateClear();
                }

                if (Request.QueryString["CONTENTS_ID"] != null && Request.QueryString["CONTENTS_ID"].ToString() != "")
                {                    
                    if (!IsPostBack)
                    {
                        ViewState["CONTENTS_ID"] = Request.QueryString["CONTENTS_ID"].ToString();
                        this.BindData();
                    }
                }

                if (Request.QueryString["TEMP_FLG"] != null && Request.QueryString["TEMP_FLG"].ToString() == "Y")
                {                    
                    this.btnSend.Visible = false;
                    this.btnTemp.Visible = true;
                }
                else
                {
                    this.btnSend.Visible = true;
                    this.btnTemp.Visible = false;
                }

                base.pRender(this.Page, new object[,] { { this.btnSend, "E" }, { this.btnTemp, "E" } }, Convert.ToString(Request.QueryString["MenuCode"]));

            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion 

        /************************************************************
       * Function name : BindDropDownList
       * Purpose       : DropDownList 데이터 바인딩을 위한 처리
       * Input         : void
       * Output        : void
       *************************************************************/
        #region private void BindDropDownList()
        private void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[1];
                DataTable xDt = null;

                // Contents Type
                xParams[0] = "0042";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlContentsType, xDt,  WebControlHelper.ComboType.NullAble);

                // Lang
                xParams[0] = "0017";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlLang, xDt, WebControlHelper.ComboType.NullAble);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion 

        /************************************************************
        * Function name : ViewStateClear
        * Purpose       : view state 초기화 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void ViewStateClear()
        private void ViewStateClear()
        {
            try
            {
                ViewState["CONTENTS_ID"] = string.Empty;
                ViewState["CONTENTS_TYPE"] = string.Empty;
                ViewState["CONTENTS_LANG"] = string.Empty;
                ViewState["CONTENTS_NM"] = string.Empty;
                ViewState["CONTENTS_REMARK"] = string.Empty;
                ViewState["CONTENTS_FILE_NM"] = string.Empty;
                ViewState["TEMP_SAVE_FLG"] = string.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion 

        /******************************************************************************************
       * Function name : BindData
       * Purpose       : 넘겨받은 contents_id에 해당하는 데이터를 페이지의 컨트롤에 바인딩 처리
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region private void BindData()
        private void BindData()
        {
            try
            {
                string[] xParams = new string[1];
                DataTable xDt = null;

                xParams[0] = ViewState["CONTENTS_ID"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_content_md",
                                                 "GetContentsInfoByID",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);

                if (xDt != null && xDt.Rows.Count > 0)
                {
                    // 향후 변경여부 검사를 위해 ViewState에 입력되는 정보 저장

                    ViewState["CONTENTS_TYPE"] = xDt.Rows[0]["contents_type"].ToString();
                    ViewState["CONTENTS_LANG"] = xDt.Rows[0]["contents_lang"].ToString();
                    ViewState["CONTENTS_NM"] = xDt.Rows[0]["contents_nm"].ToString();
                    ViewState["CONTENTS_REMARK"] = xDt.Rows[0]["contents_remark"].ToString();
                    ViewState["CONTENTS_FILE_NM"] = xDt.Rows[0]["contents_file_nm"].ToString();
                    ViewState["TEMP_SAVE_FLG"] = xDt.Rows[0]["temp_save_flg"].ToString();

                    // Contents Type 설정
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlContentsType, xDt.Rows[0]["contents_type"].ToString());

                    // Lang 설정
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlLang, xDt.Rows[0]["contents_lang"].ToString());

                    // 기타 TextBox 설정
                    this.txtContentsNM.Text = xDt.Rows[0]["contents_nm"].ToString();
                    this.txtRemark.Text = xDt.Rows[0]["contents_remark"].ToString();
                    this.txtFileNM.Value = xDt.Rows[0]["contents_file_nm"].ToString();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion 

        /************************************************************
       * Function name : btnSend_Click
       * Purpose       : Send버튼 클릭될 때 처리
                         어떤 항목이라도 변경이 발생되면, 신규 생성하여 선박으로 전송 처리
       * Input         : void
       * Output        : void
       *************************************************************/
        #region protected void btnSend_Click(object sender, EventArgs e)
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsDataChanged())
                {
                    string[] xTempParams = new string[2];
                    //string xContentsID = "";
                    string xRtn = "";

                    //// contents_id 처리
                    //xTempParams[0] = "t_contents";
                    //xTempParams[1] = "contents_id";
                    //xContentsID = SBROKER.GetString("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                    //                                 "GetMaxIDOfTable",
                    //                                 LMS_SYSTEM.CURRICULUM,
                    //                                 "CLT.WEB.UI.LMS.CURR", (object)xTempParams);

                    // 입력 처리
                    object[] xParams = new object[12];

                    xParams[0] = ViewState["CONTENTS_ID"].ToString();
                    xParams[1] = this.ddlContentsType.SelectedItem.Value; // contents_type
                    xParams[2] = this.ddlLang.SelectedItem.Value; // lang
                    xParams[3] = this.txtContentsNM.Text; // contents_name
                    xParams[4] = this.txtRemark.Text; // remark

                    // 파일 변경이 있을 경우, file object를 그대로 넘기고
                    if (ViewState["CONTENTS_FILE_NM"].ToString() != this.txtFileNM.Value)
                    {
                        xParams[5] = base.ConvertToFileUpload(this.FileUpload1.PostedFile);
                        xParams[6] = this.txtFileNM.Value;
                    }
                    else
                    {
                        xParams[5] = null;
                        xParams[6] = this.txtFileNM.Value;
                    }

                    //if (this.FileUpload1.PostedFile != null && this.FileUpload1.PostedFile.ContentLength > 0)
                    //{
                    //    xParams[5] = this.FileUpload1.PostedFile; // contents_file
                    //    xParams[6] = null; // contents_filename
                    //}
                    //    //파일 변경이 없을 경우, 기존과 동일하게 file명만 넘긴다.
                    //else
                    //{
                    //    xParams[5] = null; // contents_file
                    //    xParams[6] = this.txtFileNM.Value; // contents_filename
                    //}

                    xParams[7] = Server.MapPath(this.ContentsFilePath); // contents_filepath
                    xParams[8] = Session["USER_ID"].ToString(); // Ins_ID
                    xParams[9] = "1"; // send_flg   
                    xParams[10] = (Request.QueryString["TEMP_FLG"] != null && Request.QueryString["TEMP_FLG"].ToString() == "Y") ? "Y" : "N";
                    xParams[11] = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ContentsFilePath"].ToString());

                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_content_md",
                                                     "SetContentsInsert",
                                                     LMS_SYSTEM.CURRICULUM,
                                                     "CLT.WEB.UI.LMS.CURR", (object)xParams);

                    if (xRtn != string.Empty)
                    {
                        //A001: {0}이(가) 저장되었습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                          new string[] { "컨텐츠" },
                                                          new string[] { "Contents" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));
                        //저장 후 신규 id 값으로 재조회 
                        ViewState["CONTENTS_ID"] = xRtn;
                        this.BindData();
                    }
                    else
                    {
                        //A004: {0}이(가) 입력되지 않았습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004",
                                                          new string[] { "컨텐츠" },
                                                          new string[] { "Contents" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));
                    }
                }
                else
                {
                    //A023: 변경내용을 재 확인 바랍니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A023",
                                                      new string[] { null },
                                                      new string[] { null },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion 

        /******************************************************************************************
       * Function name : IsDataChanged
       * Purpose       : 데이터 변경이 발생했는지 여부를 체크하는 처리
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region private bool IsDataChanged()
        private bool IsDataChanged()
        {
            try
            {
                // 변경발생 여부 확인
                if (ViewState["CONTENTS_TYPE"].ToString() != this.ddlContentsType.SelectedItem.Value)
                    return true;

                else if (ViewState["CONTENTS_LANG"].ToString() != this.ddlLang.SelectedItem.Value)
                    return true;

                else if (ViewState["CONTENTS_NM"].ToString() != this.txtContentsNM.Text)
                    return true;

                else if (ViewState["CONTENTS_REMARK"].ToString() != this.txtRemark.Text)
                    return true;

                // 파일 처리 (null인 경우, 파일관련 작업이 없었다고 보면 됨.)
                if (this.FileUpload1.PostedFile != null)
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return false;
        }
        #endregion


    }
}
