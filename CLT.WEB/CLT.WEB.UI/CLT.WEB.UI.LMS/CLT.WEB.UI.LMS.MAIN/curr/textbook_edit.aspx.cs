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

namespace CLT.WEB.UI.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : textbook_edit Class
    /// 
    /// 2. 주요기능 : 교재 수정에 따른 신규 교재 생성 후, 선박으로 전송하는 처리
    ///				  
    /// 3. Class 명 : textbook_edit
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.19
    /// </summary>
    public partial class textbook_edit : BasePage
    {
        /************************************************************
        * Function name : Page_Load
        * Purpose       : 페이지 로드될 때 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            // List 페이지로부터 contents_id를 전달받은 경우만 해당 페이지를 처리하고
            // 그렇지 않은 경우, 메세지 출력과 함께 창을 종료한다.            
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list", xScriptMsg);

                    return;
                }

                if (!IsPostBack)
                {
                    this.BindDropDownList();
                    this.ViewStateClear();
                }

                if (Request.QueryString["textbook_id"] != null && Request.QueryString["textbook_id"].ToString() != "")
                {                    
                    if (!IsPostBack)
                    {
                        ViewState["TEXTBOOK_ID"] = Request.QueryString["textbook_id"].ToString();
                        this.BindData();
                    }
                }

                if (Request.QueryString["temp_flg"] != null && Request.QueryString["temp_flg"].ToString() == "Y")
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
                ViewState["TEXTBOOK_ID"] = string.Empty;

                ViewState["TEXTBOOK_TYPE"] = string.Empty;
                ViewState["TEXTBOOK_LANG"] = string.Empty;
                ViewState["COURSE_GROUP"] = string.Empty;
                ViewState["COURSE_FIELD"] = string.Empty;

                ViewState["TEXTBOOK_NM"] = string.Empty;
                ViewState["PUBLISHER"] = string.Empty;
                ViewState["AUTHOR"] = string.Empty;
                ViewState["PRICE"] = string.Empty;
                ViewState["TEXTBOOK_INTRO"] = string.Empty;
                ViewState["TEXTBOOK_DESC"] = string.Empty;
                ViewState["TEXTBOOK_FILE_NM"] = string.Empty;

                ViewState["TEMP_SAVE_FLG"] = string.Empty;

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
       * Function name : BindDropDownList
       * Purpose       : DropDownList 데이터 바인딩을 위한 처리
       * Input         : void
       * Output        : void
       *************************************************************/
        private void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[1];
                DataTable xDt = null;

                // Course Group
                xParams[0] = "0003";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlCourseGroup, xDt,  WebControlHelper.ComboType.NullAble);

                // Course Field
                xParams[0] = "0004";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlCourseField, xDt, WebControlHelper.ComboType.NullAble);

                // TextBook Type
                xParams[0] = "0049";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlTextBookType, xDt, WebControlHelper.ComboType.NullAble);

                // TextBook Lang
                xParams[0] = "0017";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlTextBookLang, xDt, WebControlHelper.ComboType.NullAble);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /******************************************************************************************
       * Function name : BindData
       * Purpose       : 넘겨받은 textbook_id에 해당하는 데이터를 페이지의 컨트롤에 바인딩 처리
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        private void BindData()
        {
            try
            {
                string[] xParams = new string[1];
                DataTable xDt = null;

                xParams[0] = ViewState["TEXTBOOK_ID"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_textbook_md",
                                                 "GetTextBookInfoByID",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);

                if (xDt != null && xDt.Rows.Count > 0)
                {
                    // 향후 변경여부 검사를 위해 ViewState에 입력되는 정보 저장


                    ViewState["TEXTBOOK_TYPE"] = xDt.Rows[0]["textbook_type"].ToString();
                    ViewState["TEXTBOOK_LANG"] = xDt.Rows[0]["textbook_lang"].ToString();
                    ViewState["COURSE_GROUP"] = xDt.Rows[0]["course_group"].ToString();
                    ViewState["COURSE_FIELD"] = xDt.Rows[0]["course_field"].ToString();

                    ViewState["TEXTBOOK_NM"] = xDt.Rows[0]["textbook_nm"].ToString();
                    ViewState["PUBLISHER"] = xDt.Rows[0]["publisher"].ToString();
                    ViewState["AUTHOR"] = xDt.Rows[0]["author"].ToString();
                    ViewState["PRICE"] = xDt.Rows[0]["price"].ToString();
                    ViewState["TEXTBOOK_INTRO"] = xDt.Rows[0]["textbook_intro"].ToString();
                    ViewState["TEXTBOOK_DESC"] = xDt.Rows[0]["textbook_desc"].ToString();
                    ViewState["TEXTBOOK_FILE_NM"] = xDt.Rows[0]["textbook_file_nm"].ToString();
                    ViewState["TEMP_SAVE_FLG"] = xDt.Rows[0]["temp_save_flg"].ToString();

                    // TextBook Type 설정
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlTextBookType, xDt.Rows[0]["textbook_type"].ToString());
                    // TextBook Lang 설정
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlTextBookLang, xDt.Rows[0]["textbook_lang"].ToString());
                    // Course Group 설정
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlCourseGroup, xDt.Rows[0]["course_group"].ToString());
                    // Course Field 설정
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlCourseField, xDt.Rows[0]["course_field"].ToString());

                    // 기타 TextBox 설정
                    this.txtTextBookNM.Text = xDt.Rows[0]["textbook_nm"].ToString();
                    this.txtPublisher.Text = xDt.Rows[0]["publisher"].ToString();
                    this.txtAuthor.Text = xDt.Rows[0]["author"].ToString();
                    this.txtPrice.Text = xDt.Rows[0]["price"].ToString();
                    this.txtTextBookIntro.Text = xDt.Rows[0]["textbook_intro"].ToString();
                    this.txtTextBookDesc.Text = xDt.Rows[0]["textbook_desc"].ToString();
                    this.txtFileNM.Value = xDt.Rows[0]["textbook_file_nm"].ToString();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /************************************************************
        * Function name : btnSend_Click
        * Purpose       : Send버튼 클릭될 때 처리
        *                 Rev 대상 컬럼 : 교재명, 저자, 금액, 교재소개, 파일, 교재목차, 출판사
        *                 textbook_nm, author, price, textbook_intro, textbook_file_nm, textbook_desc, publisher
        *                 Rev 대상 컬럼이 아닌컬럼의 변경이 발생되면, 신규 생성하여 선박으로 전송 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {

                // Data가 변경이 되었는지 여부
                if (IsDataChanged())
                {

                    if (this.IsDataValidation())
                    {


                        string[] xTempParams = new string[2];
                        string xTextBookID = "";
                        string xRtn = "";

                        //temp_save 모드가 아니고..  Rev 변경이 되었는지 여부 확인 
                        if ((Request.QueryString["TEMP_FLG"] != null && Request.QueryString["TEMP_FLG"].ToString() != "Y")
                            && IsRevDataChanged())
                        {
                            string[] xParams = new string[3];

                            xParams[0] = ViewState["TEXTBOOK_ID"].ToString(); // textbook_id
                            xParams[1] = (ViewState["REV_COLUMNS"] != null && !string.IsNullOrEmpty(ViewState["REV_COLUMNS"].ToString())) ? ViewState["REV_COLUMNS"].ToString() : ""; // rev_columns
                            xParams[2] = Session["USER_ID"].ToString(); ; // ins_id

                            xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_textbook_md",
                                                             "SetTextBookRevInsert",
                                                             LMS_SYSTEM.CURRICULUM,
                                                             "CLT.WEB.UI.LMS.CURR", (object)xParams);
                        }
                        else
                        {
                            // 입력 처리
                            object[] xParams = new object[16];

                            xParams[0] = this.ddlTextBookType.SelectedItem.Value; // textbook_type
                            xParams[1] = this.ddlTextBookLang.SelectedItem.Value; // textbook_lang
                            xParams[2] = this.ddlCourseGroup.SelectedItem.Value; // course_group
                            xParams[3] = this.ddlCourseField.SelectedItem.Value; // course_field

                            xParams[4] = this.txtAuthor.Text; // author
                            xParams[5] = this.txtPrice.Text; // price
                            xParams[6] = this.txtPublisher.Text; // publisher
                            xParams[7] = this.txtTextBookDesc.Text; // textbook_desc
                            xParams[8] = this.txtTextBookIntro.Text; // textbook_intro
                            xParams[9] = this.txtTextBookNM.Text; // textbook_nm

                            // 파일 변경이 있을 경우, file object를 그대로 넘기고
                            if (ViewState["TEXTBOOK_FILE_NM"].ToString() != this.txtFileNM.Value)
                            {
                                xParams[10] = base.ConvertToFileUpload(this.FileUpload1.PostedFile);
                                xParams[11] = this.txtFileNM.Value;
                            }
                            else
                            {
                                xParams[10] = null;
                                xParams[11] = this.txtFileNM.Value;
                            }

                            //if (this.FileUpload1.PostedFile != null && this.FileUpload1.PostedFile.ContentLength > 0)
                            //{
                            //    xParams[10] = this.FileUpload1.PostedFile; // textbook_file
                            //    xParams[11] = null; // textbook_filename
                            //}
                            ////파일 변경이 없을 경우, 기존과 동일하게 file명만 넘긴다.
                            //else
                            //{
                            //    xParams[10] = null; // textbook_file
                            //    xParams[11] = this.txtFileNM.Value; // textbook_filename
                            //}

                            xParams[12] = Session["USER_ID"].ToString(); // Ins_ID
                            xParams[13] = "1"; // send_flg                
                            xParams[14] = (Request.QueryString["TEMP_FLG"] != null && Request.QueryString["TEMP_FLG"].ToString() == "Y") ? "Y" : "N";
                            xParams[15] = ViewState["TEXTBOOK_ID"].ToString();

                            xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_textbook_md",
                                                             "SetTextBookInsert",
                                                             LMS_SYSTEM.CURRICULUM,
                                                             "CLT.WEB.UI.LMS.CURR", (object)xParams);

                            if (xRtn != string.Empty)
                            {
                                
                                //A001: {0}이(가) 저장되었습니다.
                                ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                                  new string[] { "교재" },
                                                                  new string[] { "Textbook" },
                                                                  Thread.CurrentThread.CurrentCulture
                                                                 ));
                                //저장 후 신규 id 값으로 재조회 
                                ViewState["TEXTBOOK_ID"] = xRtn;
                                this.BindData();

                            }
                            else
                            {
                                //A004: {0}이(가) 입력되지 않았습니다.
                                ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004",
                                                                  new string[] { "교재" },
                                                                  new string[] { "Textbook" },
                                                                  Thread.CurrentThread.CurrentCulture
                                                                 ));
                            }

                        }
                    }
                    else
                    {
                        //A012: {0}의 필수 항목 입력이 누락되었습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                          new string[] { "교재" },
                                                          new string[] { "Textbook" },
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

        /******************************************************************************************
        * Function name : IsRevDataChanged
        * Purpose       : Rev 데이터 변경이 발생했는지 여부를 체크하는 처리
        * Rev 대상 컬럼 : 교재명, 저자, 금액, 교재소개, 파일, 교재목차, 출판사
        *                 textbook_nm, author, price, textbook_intro, textbook_file_nm, textbook_desc, publisher, textbook_rev_dt
        * Input         : void
        * Output        : void
        ******************************************************************************************/
        private bool IsRevDataChanged()
        {
            try
            {
                string xRevColumns = "";// 변경된 rev_column을 담는 처리

                // 변경발생 여부 확인
                if (ViewState["TEXTBOOK_NM"].ToString() != this.txtTextBookNM.Text)
                    xRevColumns += "textbook_nm";

                else if (ViewState["AUTHOR"].ToString() != this.txtAuthor.Text)
                    xRevColumns += "凸author";

                else if (ViewState["PRICE"].ToString() != this.txtPrice.Text)
                    xRevColumns += "凸price";

                else if (ViewState["TEXTBOOK_INTRO"].ToString() != this.txtTextBookIntro.Text)
                    xRevColumns += "凸textbook_intro";

                else if (ViewState["TEXTBOOK_DESC"].ToString() != this.txtTextBookDesc.Text)
                    xRevColumns += "凸textbook_desc";

                else if (ViewState["PUBLISHER"].ToString() != this.txtPublisher.Text)
                    xRevColumns += "凸publisher";

                // 파일 처리 (null인 경우, 파일관련 작업이 없었다고 보면 됨.)
                if (this.FileUpload1.PostedFile != null)
                    xRevColumns += "凸textbook_file_nm";

                if (!string.IsNullOrEmpty(xRevColumns))
                {
                    ViewState["REV_COLUMNS"] = xRevColumns.Trim(new char[] { '凸' });
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return false;
        }

        /******************************************************************************************
       * Function name : IsDataValidation
       * Purpose       : Data Validation check 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region private bool IsDataValidation()
        private bool IsDataValidation()
        {
            try
            {
                if (this.ddlCourseField.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.ddlCourseGroup.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;

                if (this.ddlTextBookType.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.ddlTextBookLang.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;

                if (this.txtTextBookNM.Text == string.Empty)
                    return false;
                if (this.txtPublisher.Text == string.Empty)
                    return false;
                if (this.txtAuthor.Text == string.Empty)
                    return false;
                if (this.txtPrice.Text == string.Empty)
                    return false;
                if (this.txtFileNM.Value == string.Empty)
                    return false;

                //if(this.txtFileNM

                //if (this.ddlClass.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                //    return false;
                //if (this.ddlType.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                //    return false;
                //if (this.ddlLang.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                //    return false;
                //if (this.rdoUsage.SelectedIndex < 0)
                //    return false;

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return true;
        }
        #endregion

        /******************************************************************************************
        * Function name : IsDataChanged
        * Purpose       : 데이터 변경이 발생했는지 여부를 체크하는 처리
        * Input         : void
        * Output        : void
        ******************************************************************************************/
        private bool IsDataChanged()
        {
            try
            {
                // 변경발생 여부 확인
                if (ViewState["TEXTBOOK_TYPE"].ToString() != this.ddlTextBookType.SelectedItem.Value)
                    return true;

                else if (ViewState["TEXTBOOK_LANG"].ToString() != this.ddlTextBookLang.SelectedItem.Value)
                    return true;

                else if (ViewState["COURSE_GROUP"].ToString() != this.ddlCourseGroup.SelectedItem.Value)
                    return true;

                else if (ViewState["COURSE_FIELD"].ToString() != this.ddlCourseField.SelectedItem.Value)
                    return true;

                else if (ViewState["TEXTBOOK_NM"].ToString() != this.txtTextBookNM.Text)
                    return true;

                else if (ViewState["PUBLISHER"].ToString() != this.txtPublisher.Text)
                    return true;

                else if (ViewState["AUTHOR"].ToString() != this.txtAuthor.Text)
                    return true;

                else if (ViewState["PRICE"].ToString() != this.txtPrice.Text)
                    return true;

                else if (ViewState["TEXTBOOK_INTRO"].ToString() != this.txtTextBookIntro.Text)
                    return true;

                else if (ViewState["TEXTBOOK_DESC"].ToString() != this.txtTextBookDesc.Text)
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


    }
}
