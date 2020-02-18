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

using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Threading; 

namespace CLT.WEB.UI.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : assess_edit Class
    /// 
    /// 2. 주요기능 : 평가문제 수정 및 선박 송부 
    ///				  
    /// 3. Class 명 : assess_edit
    /// 
    /// 4. 작 업 자 : 최인재 / 2011.12.27
    /// </summary>
    public partial class assess_edit : BasePage
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

                    this.rdoExamType.SelectedIndex = 1; //초기화 
                    this.rdoExamType_SelectedIndexChanged(new object(), new EventArgs());
                }

                if (Request.QueryString["QUESTION_ID"] != null && Request.QueryString["QUESTION_ID"].ToString() != "")
                {                    
                    if (!IsPostBack)
                    {
                        ViewState["QUESTION_ID"] = Request.QueryString["QUESTION_ID"].ToString();
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

                base.pRender(this.Page, new object[,] { { this.btnSend, "E" },  { this.btnTemp, "E" }}, Convert.ToString(Request.QueryString["MenuCode"]));

            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
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
                ViewState["QUESTION_ID"] = string.Empty;
                ViewState["QUESTION_KIND"] = string.Empty;
                ViewState["QUESTION_LANG"] = string.Empty;
                ViewState["QUESTION_TYPE"] = string.Empty;
                ViewState["COURSE_GROUP"] = string.Empty;
                ViewState["COURSE_FIELD"] = string.Empty;
                ViewState["QUESTION_SCORE"] = string.Empty;
                ViewState["QUESTION_ANSWER"] = string.Empty;
                ViewState["QUESTION_CONTENT"] = string.Empty;
                ViewState["QUESTION_EXAMPLE"] = string.Empty;
                ViewState["QUESTION_DESC"] = string.Empty;
                ViewState["USE_FLG"] = string.Empty;
                ViewState["TEMP_SAVE_FLG"] = string.Empty;

                ViewState["COURSE_LIST"] = string.Empty;
                ViewState["SUBJECT_LIST"] = string.Empty;
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
        #region private void BindDropDownList()
        private void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[1];
                string xSql = string.Empty;
                DataTable xDt = null;

                //classification 
                xParams[0] = "0042";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlClassification, xDt,  WebControlHelper.ComboType.NullAble);

                // Lang
                xParams[0] = "0017";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlLang, xDt, WebControlHelper.ComboType.NullAble);

                //SCORE
                xDt = new DataTable();	//DataTable 객체 생성 
                xDt.Columns.AddRange(new DataColumn[] { new DataColumn("d_knm"), new DataColumn("d_cd") }); // DataTable Range 설정 
                for (int i = 5; i <= 10; i+=5)
                {
                    xDt.Rows.Add(new DataColumn(i.ToString()), new DataColumn(i.ToString()));
                }
                WebControlHelper.SetDropDownList(this.ddlScore, xDt, WebControlHelper.ComboType.NullAble);

                //group 
                xParams[0] = "0003";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlGroup, xDt, WebControlHelper.ComboType.NullAble);

                //field 
                xParams[0] = "0004";
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.CURRICULUM,
                                             "CLT.WEB.UI.LMS.CURR", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlField, xDt, WebControlHelper.ComboType.NullAble);
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

                xParams[0] = ViewState["QUESTION_ID"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_assess_md",
                                                 "GetAssessInfo",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);

                if (xDt != null && xDt.Rows.Count > 0)
                {
                    DataRow dr = xDt.Rows[0];

                    //VIEWSTATE에 입력되는 정보 저장 
                    ViewState["QUESTION_KIND"] = dr["QUESTION_KIND"].ToString();
                    ViewState["QUESTION_LANG"] = dr["QUESTION_LANG"].ToString();
                    ViewState["QUESTION_TYPE"] = dr["QUESTION_TYPE"].ToString();
                    ViewState["COURSE_GROUP"] = dr["COURSE_GROUP"].ToString();
                    ViewState["COURSE_FIELD"] = dr["COURSE_FIELD"].ToString();
                    ViewState["QUESTION_SCORE"] = dr["QUESTION_SCORE"].ToString();
                    ViewState["QUESTION_ANSWER"] = dr["QUESTION_ANSWER"].ToString();
                    ViewState["QUESTION_CONTENT"] = dr["QUESTION_CONTENT"].ToString();
                    ViewState["QUESTION_EXAMPLE"] = dr["QUESTION_EXAMPLE"].ToString();
                    ViewState["QUESTION_DESC"] = dr["QUESTION_DESC"].ToString();
                    ViewState["USE_FLG"] = dr["USE_FLG"].ToString();
                    ViewState["TEMP_SAVE_FLG"] = dr["TEMP_SAVE_FLG"].ToString();

                    ViewState["COURSE_LIST"] = dr["COURSE_LIST"].ToString();
                    ViewState["SUBJECT_LIST"] = dr["SUBJECT_LIST"].ToString();


                    //data binding 
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlClassification, dr["QUESTION_KIND"].ToString()); //classification
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlLang, dr["QUESTION_LANG"].ToString()); //language
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlGroup, dr["COURSE_GROUP"].ToString()); //group 
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlField, dr["COURSE_FIELD"].ToString()); //field
                    WebControlHelper.SetSelectItem_DropDownList(this.ddlScore, dr["QUESTION_SCORE"].ToString()); //score

                    this.txtQuestion.Text = dr["QUESTION_CONTENT"].ToString();
                    this.txtExplan.Text = dr["QUESTION_DESC"].ToString();

                    int xQuestionType = Convert.ToInt16(dr["QUESTION_TYPE"].ToString() == string.Empty ? "0" : dr["QUESTION_TYPE"].ToString());

                    //exam type에 따라 화면 컨트롤 변경 
                    for (int i = 0; i < this.rdoExamType.Items.Count; i++)
                    {
                        if (this.rdoExamType.Items[i].Value == xQuestionType.ToString())
                        {
                            this.rdoExamType.SelectedIndex = i;
                            this.rdoExamType_SelectedIndexChanged(new object(), new EventArgs());
                        }
                    }

                    //각 answer, example display 
                    string[] xQList = dr["QUESTION_ANSWER"].ToString().Split('凸');
                    string[] xExampleList = dr["QUESTION_EXAMPLE"].ToString().Split('凸');

                    if (this.dtlQ.Items.Count > 1)
                    {
                        for (int i = 0; i < this.dtlQ.Items.Count; i++)
                        {
                            for (int k = 1; k < xQList.GetLength(0); k++)
                            {
                                if (Convert.ToInt32(xQList[k]) == i + 1)
                                {
                                    ((CheckBox)this.dtlQ.Items[i].FindControl("chkQ")).Checked = true;
                                }
                            }
                            ((TextBox)this.dtlExample.Items[i].FindControl("txtExample")).Text = xExampleList[i + 1];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < this.dtlExample.Items.Count; i++)
                        {
                            ((TextBox)this.dtlExample.Items[i].FindControl("txtExample")).Text = xExampleList[i + 1];
                        }
                    }


                    //COURSE LIST
                    if (ViewState["COURSE_LIST"].ToString() != string.Empty)
                    {
                        xParams = new string[1];
                        xParams[0] ="'" + ViewState["COURSE_LIST"].ToString().Replace("凸", "','") + "'";
                        string[] xCList = ViewState["COURSE_LIST"].ToString().Split('凸'); 

                        xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_assess_md",
                                                         "GetAssessCourseInfo",
                                                         LMS_SYSTEM.CURRICULUM,
                                                         "CLT.WEB.UI.LMS.CURR", (object)xParams);

                        for (int i = 0; i < xCList.Length; i++)
                        {
                            foreach (DataRow drC in xDt.Rows)
                            {
                                if (xCList[i] == drC[1].ToString())
                                {
                                    this.lstCourseItemList.Items.Add(drC[0].ToString());
                                    break; 
                                }
                            }
                        }
                    }


                    //SUBJECT LIST                    
                    if (ViewState["SUBJECT_LIST"].ToString() != string.Empty)
                    {
                        xParams = new string[1];
                        xParams[0] = "'" + ViewState["SUBJECT_LIST"].ToString().Replace("凸", "','") + "'";

                        string[] xSList = ViewState["SUBJECT_LIST"].ToString().Split('凸'); 

                        xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_assess_md",
                                                         "GetAssessSubjectInfo",
                                                         LMS_SYSTEM.CURRICULUM,
                                                         "CLT.WEB.UI.LMS.CURR", (object)xParams);
                        for (int i = 0; i < xSList.Length; i++)
                        {
                            foreach (DataRow drS in xDt.Rows)
                            {
                                if (xSList[i] == drS[1].ToString())
                                {
                                    this.lstSubjectItemList.Items.Add(drS[0].ToString());
                                }
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
                //ViewState["QUESTION_ID"] = Request.QueryString["QUESTION_ID"].ToString();
                //ViewState["QUESTION_KIND"] = dr["QUESTION_KIND"].ToString();
                //ViewState["QUESTION_LANG"] = dr["QUESTION_LANG"].ToString();
                //ViewState["QUESTION_TYPE"] = dr["QUESTION_TYPE"].ToString();
                //ViewState["COURSE_GROUP"] = dr["COURSE_GROUP"].ToString();
                //ViewState["COURSE_FIELD"] = dr["COURSE_FIELD"].ToString();
                //ViewState["QUESTION_SCORE"] = dr["QUESTION_SCORE"].ToString();
                //ViewState["QUESTION_ANSWER"] = dr["QUESTION_ANSWER"].ToString();
                //ViewState["QUESTION_CONTENT"] = dr["QUESTION_CONTENT"].ToString();
                //ViewState["QUESTION_EXAMPLE"] = dr["QUESTION_EXAMPLE"].ToString();
                //ViewState["QUESTION_DESC"] = dr["QUESTION_DESC"].ToString();
                //ViewState["USE_FLG"] = dr["USE_FLG"].ToString();
                //ViewState["TEMP_SAVE_FLG"] = dr["TEMP_SAVE_FLG"].ToString(); 

                string[] xQE = CreateString_Q_Example();
                string xQ = xQE[0]; // string.Empty;
                string xExample = xQE[1]; // string.Empty;

                string xCourseList = this.CreateString_CourseList();
                string xSubjectList = this.CreateString_SubjectList();

                if (ViewState["QUESTION_KIND"].ToString() != this.ddlClassification.SelectedItem.Value)
                    return true;
                else if (ViewState["QUESTION_LANG"].ToString() != this.ddlLang.SelectedItem.Value)
                    return true;
                else if (ViewState["QUESTION_TYPE"].ToString() != this.rdoExamType.SelectedValue.ToString().PadLeft(6, '0'))
                    return true;
                else if (ViewState["COURSE_GROUP"].ToString() != this.ddlGroup.SelectedItem.Value)
                    return true;
                else if (ViewState["COURSE_FIELD"].ToString() != this.ddlField.SelectedItem.Value)
                    return true;
                else if (ViewState["QUESTION_SCORE"].ToString() != this.ddlScore.SelectedItem.Value)
                    return true;
                else if (ViewState["QUESTION_ANSWER"].ToString() != xQ)
                    return true;
                else if (ViewState["QUESTION_CONTENT"].ToString() != this.txtQuestion.Text)
                    return true;
                else if (ViewState["QUESTION_EXAMPLE"].ToString() != xExample)
                    return true;
                else if (ViewState["QUESTION_DESC"].ToString() != this.txtExplan.Text)
                    return true;

                else if (ViewState["COURSE_LIST"].ToString() != xCourseList)
                    return true;
                else if (ViewState["SUBJECT_LIST"].ToString() != xSubjectList)
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
                string[] xQE = CreateString_Q_Example();
                string xQ = xQE[0]; // string.Empty;
                string xExample = xQE[1]; // string.Empty;           

                if (this.ddlClassification.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.ddlField.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.ddlGroup.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.ddlLang.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.ddlScore.SelectedItem.Value.Replace("*", string.Empty) == string.Empty)
                    return false;
                if (this.rdoExamType.SelectedIndex < 0)
                    return false;

                if (this.txtQuestion.Text == string.Empty)
                    return false;

                if (xQ == null)
                    return false;
                if (xQ != null && xQ.Replace("凸", string.Empty) == string.Empty)
                    return false;

                if (xExample.Replace("凸", string.Empty) == string.Empty)
                    return false;
                if (this.dtlQ.Items.Count > 1 && xQ.Replace("凸", string.Empty) == string.Empty)
                    return false;

                int xCnt = 0;
                if (this.dtlQ.Items.Count > 1)
                {
                    for (int i = 0; i < this.dtlQ.Items.Count; i++)
                    {
                        if (((CheckBox)this.dtlQ.Items[i].FindControl("chkQ")).Checked == true)
                        {
                            xCnt++;
                        }
                    }

                    if (this.rdoExamType.SelectedValue == "6")
                    {
                        if (xCnt > 1)
                        {
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A007",
                                                             new string[] { "4지선다 정답", "1개" },
                                                             new string[] { "Result", "1" },
                                                             Thread.CurrentThread.CurrentCulture
                                                            ));
                            return false;
                        }
                    }
                }

                if (this.rdoExamType.SelectedValue == "2")
                {
                    if (xExample.ToUpper() == "凸O" || xExample.ToUpper() == "凸X")
                    {
                    }
                    else
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A052",
                                                   new string[] { "O/X", "O/X" },
                                                   new string[] { "O/X Result", "O/X" },
                                                   Thread.CurrentThread.CurrentCulture
                                                  ));
                        return false;
                    }
                }
                ////question_desc 는 null 입력 가능 
                //if (this.txtExplan.Text == string.Empty)
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
       * Function name : CreateString_Q_Example
       * Purpose       : Answer, Example String Creation 하는 부분
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region private string[] CreateString_Q_Example()
        private string[] CreateString_Q_Example()
        {
            string[] xReturn = new string[2]; 
            try
            {               
                if (this.dtlQ.Items.Count > 1)
                {
                    for (int i = 0; i < this.dtlQ.Items.Count; i++)
                    {
                        if (((CheckBox)this.dtlQ.Items[i].FindControl("chkQ")).Checked == true)
                        {
                            xReturn[0] += "凸" + Convert.ToString(i + 1);
                        }
                        xReturn[1] += "凸" + ((TextBox)this.dtlExample.Items[i].FindControl("txtExample")).Text;
                    }
                }
                else
                {
                    for (int i = 0; i < this.dtlExample.Items.Count; i++)
                    {
                        xReturn[0] += ((TextBox)this.dtlExample.Items[i].FindControl("txtExample")).Text; 
                        xReturn[1] += "凸" + ((TextBox)this.dtlExample.Items[i].FindControl("txtExample")).Text;
                    }

                    if (this.rdoExamType.SelectedValue == "2")
                    {
                        xReturn[0] = xReturn[0].ToUpper();
                        xReturn[1] = xReturn[1].ToUpper();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xReturn; 
        }
        #endregion 

        /******************************************************************************************
       * Function name : CreateString_CourseList
       * Purpose       : 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region private string CreateString_CourseList()
        private string CreateString_CourseList()
        {
            string xReturn = string.Empty;
            try
            {
                string xlstCourse = string.Empty;
                string[] xItem = null;                

                for (int i = 0; i < this.lstCourseItemList.Items.Count; i++)
                {
                    xItem = this.lstCourseItemList.Items[i].Text.Split('|');
                    xlstCourse += xItem[0].Trim() + "凸";
                }

                xReturn = xlstCourse;                 
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xReturn;
        }
        #endregion 

        /******************************************************************************************
       * Function name : CreateString_SubjectList
       * Purpose       : 
       * Input         : void
       * Output        : void
       ******************************************************************************************/
        #region private string CreateString_SubjectList()
        private string CreateString_SubjectList()
        {
            string xReturn = string.Empty;
            try
            {
                string xlstSubject = string.Empty;
                string[] xItem = null;

                for (int i = 0; i < this.lstSubjectItemList.Items.Count; i++)
                {
                    xItem = this.lstSubjectItemList.Items[i].Text.Split('|');
                    xlstSubject += xItem[0].Trim() + "凸";
                }

                xReturn = xlstSubject;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xReturn;
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
                if (this.IsDataChanged())
                {
                    if (this.IsDataValidation())
                    {
                        string[] xParams = new string[15];

                        xParams[0] = ViewState["QUESTION_ID"].ToString();
                        xParams[1] = this.ddlClassification.SelectedItem.Value;
                        xParams[2] = this.ddlLang.SelectedItem.Value;
                        xParams[3] = this.rdoExamType.SelectedValue.ToString().PadLeft(6, '0'); //QUESTION_TYPE 
                        xParams[4] = this.ddlGroup.SelectedItem.Value; //COURSE_GROUP
                        xParams[5] = this.ddlField.SelectedItem.Value; //COURSE_FIELD

                        string[] xQE = CreateString_Q_Example();
                        string xQ = xQE[0];
                        string xExample = xQE[1];

                        xParams[6] = xQ;  //QUESTION_ANSWER
                        xParams[7] = this.txtQuestion.Text; //QUESTION_CONTENT
                        xParams[8] = xExample;  //QUESTION_EXAMPLE
                        xParams[9] = this.txtExplan.Text; //QUESTION_DESC
                        xParams[10] = this.ddlScore.SelectedItem.Value; //QUESTION_SCORE
                        xParams[11] = Session["USER_ID"].ToString();
                        xParams[12] = (Request.QueryString["TEMP_FLG"] != null && Request.QueryString["TEMP_FLG"].ToString() == "Y") ? "Y" : "N";

                        xParams[13] = this.CreateString_CourseList();
                        xParams[14] = this.CreateString_SubjectList(); 

                        string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_assess_md",
                                                        "SetAssessInfo",
                                                        LMS_SYSTEM.CURRICULUM,
                                                        "CLT.WEB.UI.LMS.CURR", (object)xParams);

                        if (xRtn != string.Empty)
                        {
                            //{0}이(가) 저장되었습니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                          new string[] { "평가문제" },
                                                          new string[] { "Assess" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));

                            //저장 후 신규 id 값으로 재조회 
                            ViewState["QUESTION_ID"] = xRtn;
                            this.BindData();
                        }
                        else
                        {
                            //{0}이(가) 입력되지 않았습니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004",
                                                          new string[] { "평가문제" },
                                                          new string[] { "Assess" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));
                        }
                    }
                    else
                    {
                        //{0}의 필수 항목 입력이 누락되었습니다.
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A012",
                                                          new string[] { "평가문제" },
                                                          new string[] { "Assess" },
                                                          Thread.CurrentThread.CurrentCulture
                                                         ));
                    }
                }
                else
                {
                    //변경내용을 재 확인 바랍니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A023",
                                                          new string[] { "평가문제" },
                                                          new string[] { "Assess" },
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

        /************************************************************
      * Function name : btnAddCourse_Click
      * Purpose       : Course Add 
      * Input         : void
      * Output        : void
      *************************************************************/
        #region protected void btnAddCourse_Click(object sender, EventArgs e)
        protected void btnAddCourse_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtCourseID.Value != string.Empty)
                {
                    bool xSel = false;
                    string xCourse = this.txtCourseID.Value + " | " + this.txtCourseNM.Value;
                    for (int k = 0; k < this.lstCourseItemList.Items.Count; k++)
                    {
                        if (xCourse == this.lstCourseItemList.Items[k].Text)
                        {
                            xSel = true;
                            break;
                        }
                    }
                    if (xSel == false)
                        this.lstCourseItemList.Items.Add(xCourse);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion 

        /************************************************************
      * Function name : btnRemoveCourse_Click
      * Purpose       : Course Remove 
      * Input         : void
      * Output        : void
      *************************************************************/
        #region protected void btnRemoveCourse_Click(object sender, EventArgs e)
        protected void btnRemoveCourse_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.lstCourseItemList.Items.Count; i++)
                {
                    if (this.lstCourseItemList.Items[i].Selected)
                    {
                        this.lstCourseItemList.Items.Remove(this.lstCourseItemList.Items[i].Text);
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion 

        /************************************************************
      * Function name : btnAddCourse_Click
      * Purpose       : Subject Add 
      * Input         : void
      * Output        : void
      *************************************************************/
        #region protected void btnAddSubject_Click(object sender, EventArgs e)
        protected void btnAddSubject_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtSubjectId.Value != string.Empty)
                {
                    bool xSel = false;
                    string xSubject = this.txtSubjectId.Value + " | " + this.txtSubjectNm.Value;
                    for (int k = 0; k < this.lstSubjectItemList.Items.Count; k++)
                    {
                        if (xSubject == this.lstSubjectItemList.Items[k].Text)
                        {
                            xSel = true;
                            break;
                        }
                    }
                    if (xSel == false)
                        this.lstSubjectItemList.Items.Add(xSubject);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
      * Function name : btnRemoveSubject_Click
      * Purpose       : Subject Remove 
      * Input         : void
      * Output        : void
      *************************************************************/
        #region protected void btnRemoveSubject_Click(object sender, EventArgs e)
        protected void btnRemoveSubject_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.lstSubjectItemList.Items.Count; i++)
                {
                    if (this.lstSubjectItemList.Items[i].Selected)
                    {
                        this.lstSubjectItemList.Items.Remove(this.lstSubjectItemList.Items[i].Text);
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion 

        /************************************************************
      * Function name : rdoExamType_SelectedIndexChanged
      * Purpose       : 질문유형 선택 시, example 라인 변경 하기 
      * Input         : void
      * Output        : void
      *************************************************************/
        #region protected void rdoExamType_SelectedIndexChanged(object sender, EventArgs e)
        protected void rdoExamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string xsel = this.rdoExamType.SelectedValue;
                int cnt = 0;

                DataTable dtQ = new DataTable();
                DataColumn Col = new DataColumn("check", Type.GetType("System.Boolean"));
                DataColumn Col1 = new DataColumn("num", Type.GetType("System.String"));
                dtQ.Columns.Add(Col);
                dtQ.Columns.Add(Col1);


                DataTable dtExample = new DataTable();
                DataColumn Cole = new DataColumn("exam", Type.GetType("System.String"));
                dtExample.Columns.Add(Cole);

                switch (xsel)
                {
                    case "3":
                        cnt = 7;
                        break;
                    case "6":
                        cnt = 4;
                        break;
                    default:
                        cnt = 1;
                        break;
                }

                for (int i = 0; i < cnt; i++)
                {
                    DataRow rows = dtQ.NewRow();
                    rows[0] = false;
                    rows[1] = Convert.ToString(i + 1);
                    dtQ.Rows.Add(rows);

                    DataRow rows1 = dtExample.NewRow();
                    rows1[0] = string.Empty;
                    dtExample.Rows.Add(rows1);
                }

                if (xsel == "3" || xsel == "6")
                {
                    this.dtlQ.DataSource = dtQ;
                    this.dtlQ.DataBind();
                }
                else
                {
                    this.dtlQ.DataSource = null;
                    this.dtlQ.DataBind();
                }
                this.dtlExample.DataSource = dtExample;
                this.dtlExample.DataBind();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion 
       

        }
}
