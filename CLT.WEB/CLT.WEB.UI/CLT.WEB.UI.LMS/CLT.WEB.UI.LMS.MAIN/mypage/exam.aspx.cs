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

using System.Threading;
using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;

//using CLT.COMMON.DPACK;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Drawing;

namespace CLT.WEB.UI.LMS.MYPAGE
{
    public partial class exam : BasePage
    {
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "exam", xScriptMsg);

                    return;
                }

                if (Request.QueryString["OPEN_COURSE_ID"] != null && Request.QueryString["OPEN_COURSE_ID"].ToString() != "")
                {
                    ViewState["OPEN_COURSE_ID"] = Request.QueryString["OPEN_COURSE_ID"].ToString();
                    if (!IsPostBack)
                    {
                        //진도율 100% 인지 확인 
                        string[] xParams = new string[2];
                        xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                        xParams[1] = Session["user_id"].ToString();

                        DataTable dt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                         "GetExamProgress",
                                                         LMS_SYSTEM.MYPAGE,
                                                         "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];

                            if (dr["PROGRESS_RATE"].ToString() == string.Empty || Convert.ToInt32(dr["PROGRESS_RATE"].ToString()) != 100)
                            {
                                ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A018",
                                                                      new string[] { "학습" },
                                                                      new string[] { "Study" },
                                                                      Thread.CurrentThread.CurrentCulture
                                                                     ));

                                ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>self.close();</script>");
                            }
                            else
                            {
                                ////ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>flash(\"" + xPath + "\");</script>");
                                ////for (int i = 0; i < 5; i++)
                                ////{
                                ////    ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>Create(" + i + ");</script>"); 
                                ////}

                                //ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>Create('4');</script>");

                                //string test = string.Empty; 
                                //test = " <asp:TextBox ID=\"TextBox123\"  Text=\"test ttest\" Width = \"100%\" Height = \"49px\" runat=\"server\"></asp:TextBox> ";
                                //ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>Create2(\""+test+"\");</script>");

                                //test = " <asp:TextBox ID=\"TextBox124\"  Width = \"100%\" Height = \"49px\" runat=\"server\"></asp:TextBox> ";
                                //ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>Create2(" + test + ");</script>");
                                
                                ViewState["PASS_FLG"] = dr["PASS_FLG"].ToString();

                                if (dr["PASS_FLG"].ToString() != "000001" && dr["PASS_FLG"].ToString() != "000005")
                                {
                                    //문제정보 바인딩 하면서 사용자가 저장한 정보 같이 binding 
                                    this.BindData(false);
                                }
                                else
                                {
                                    //문제정보 바인딩 하면서 사용자가 저장한 정보 같이 binding 
                                    //및 결과정보 BINDING 
                                    //틀렸을 경우, 빨갛게.. 표기 
                                    this.BindData(true);
                                    //this.lblExamExplain.Text = "맞는개수: " + xR[0].ToString() + "  틀린개수: " + xR[1].ToString() + "<br>"; 
                                }
                            }
                        }
                        else
                        {
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A018",
                                                                      new string[] { "학습" },
                                                                      new string[] { "Study" },
                                                                      Thread.CurrentThread.CurrentCulture
                                                                     ));
                            //ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>self.close();</script>");
                        }
                    }
                }

                if (this.ph.Controls.Count == 0)
                {
                    if (ViewState["PASS_FLG"] != null && ViewState["PASS_FLG"].ToString() != "000001" && ViewState["PASS_FLG"].ToString() != "000005")
                    {
                        this.BindData(false);
                    }
                    else
                    {
                        this.BindData(true);
                        //this.lblExamExplain.Text = "맞는개수: " + xR[0].ToString() + "  틀린개수: " + xR[1].ToString() + "<br>"; 
                    }
                }

                base.pRender(this.Page, new object[,] { { this.btnSave, "E" }, { this.btnSubmit, "E" } }, Convert.ToString(Request.QueryString["MenuCode"]));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : ExamBinding
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private int[] BindData(bool rRight)
        private void BindData(bool rRight)
        {
            try
            {
                int[] xReturn = new int[2] { 0, 0 };

                string[] xParams = new string[2];
                xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                xParams[1] = Session["user_id"].ToString();
                DataTable dt = new DataTable();

                /*
                 * 1. T_ASSESS_RESULT에 정보 저장되어 있는지 여부 확인 
                 * 
                 * -> 저장 되어 있을 경우 
                 * T_ASSESS_RESULT JOIN 하여 정보 조회 
                 * 
                 * ->저장되어 있지 않을 경우 
                 * => EXAM VIEW STATE NULL일 경우 RANDOM하게 정보 가져와서 BINDING 
                 * => EXAM VIEW STATE가 있을 경우 EXAM VIEW STATE 정보로 정보 가져와서 BINDING 
                 * 
                 */

                dt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                     "GetExamQuestion",
                                                     LMS_SYSTEM.MYPAGE,
                                                     "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);

                int xRight = 0; //맞는 개수 
                double xTotalScore = 0;  //총점 
                double xUserScore = 0; //사용자 정보 

                if (dt.Rows.Count > 0)
                {

                    #region 저장된 사용자 정보로 display 
                    int xQNo = 0;
                    TableRow tr = new TableRow();
                    TableCell tc = new TableCell();

                    foreach (DataRow dr in dt.Rows)
                    {
                        //맞는개수 체크 하기 위함
                        if (dr["ISRIGHT_FLG"].ToString() == "Y")
                        {
                            xRight++;
                        }

                        xTotalScore += Convert.ToDouble(dr["QUESTION_SCORE"].ToString() == string.Empty ? "0" : dr["QUESTION_SCORE"].ToString());

                        //------------------------------------------
                        //문제 표기 
                        //------------------------------------------
                        xQNo++;
                        Label xlblQ = new Label();
                        xlblQ.ID = "lbl" + dr["QUESTION_ID"].ToString();
                        xlblQ.Text = "Question " + xQNo.ToString() + "<br>" + dr["QUESTION_CONTENT"].ToString();
                        xlblQ.Font.Bold = true;
                        //xlblQ01.ForeColor = Color.Blue;

                        tc = new TableCell();
                        tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                        tc.Style.Add(HtmlTextWriterStyle.Height, "auto");
                        tc.Controls.Add(xlblQ);

                        tr = new TableRow();
                        tr.Controls.Add(tc);
                        ph.Controls.Add(tr);


                        //    xtxtUA.TextMode = TextBoxMode.MultiLine; 
                        //    xtxtUA.Style.Add(HtmlTextWriterStyle.Width, "100%");
                        //    xtxtUA.Style.Add(HtmlTextWriterStyle.Height, "49px");

                        if (dr["QUESTION_TYPE"].ToString() == "000003" || dr["QUESTION_TYPE"].ToString() == "000006")
                        {
                            //------------------------------------------
                            //문제 보기 표기 - 4지선다, 다중선택 
                            //------------------------------------------
                            CheckBoxList xchl = new CheckBoxList();
                            xchl.ID = "chl" + dr["QUESTION_ID"].ToString();
                            //xchl.AutoPostBack = true; 
                            string[] xarrEx = dr["QUESTION_EXAMPLE"].ToString().Split('凸');
                            for (int i = 1; i < xarrEx.Length; i++)
                            {
                                ListItem xlst = new ListItem();
                                xlst.Value = i.ToString();
                                xlst.Text = i.ToString() + ". " + xarrEx[i];
                                xlst.Selected = false;
                                xchl.Items.Add(xlst);
                            }

                            //사용자 입력정답 표기 
                            string[] xarrAn = dr["USER_ANSWER"].ToString().Split('凸');
                            for (int i = 1; i < xarrAn.Length; i++)
                            {
                                for (int k = 0; k < xchl.Items.Count; k++)
                                {
                                    if (xarrAn[i] == xchl.Items[k].Value)
                                    {
                                        xchl.Items[k].Selected = true;
                                    }
                                }
                            }

                            //ListItem 
                            tc = new TableCell();
                            tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                            tc.Style.Add(HtmlTextWriterStyle.Height, "auto");
                            tc.Controls.Add(xchl);

                            tr = new TableRow();
                            tr.Controls.Add(tc);
                            ph.Controls.Add(tr);
                        }
                        else
                        {
                            //------------------------------------------
                            //문제 보기 표기 - 4지선다, 다중선택 외 
                            //------------------------------------------
                            TextBox xtxtUA = new TextBox();
                            xtxtUA.ID = "txt" + dr["QUESTION_ID"].ToString();
                            xtxtUA.Text = dr["USER_ANSWER"].ToString(); //사용자 정답 표기 
                            xtxtUA.Style.Add(HtmlTextWriterStyle.Width, "100%");
                            //xtxtUA.AutoPostBack = true; 

                            tc = new TableCell();
                            tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                            tc.Style.Add(HtmlTextWriterStyle.Height, "auto");
                            tc.Controls.Add(xtxtUA);

                            tr = new TableRow();
                            tr.Controls.Add(tc);
                            ph.Controls.Add(tr);

                        }


                        //------------------------------------------
                        //이수, 미이수 되었을 경우 => 정답 표기 
                        //------------------------------------------
                        if (rRight)
                        {
                            if (dr["ISRIGHT_FLG"].ToString() == "N")
                            {
                                //------------------------------------------
                                //틀렸을 경우 
                                //------------------------------------------

                                xlblQ.ForeColor = Color.Red;
                                //Label xlbl = (Label)this.ph.FindControl("lbl" + dr["QUESTION_ID"].ToString());
                                //xlbl.ForeColor = Color.Red;

                                Label xlblA = new Label();
                                xlblA.ID = "lblA" + dr["QUESTION_ID"].ToString();
                                xlblA.Text = " =>  " + dr["QUESTION_ANSWER"].ToString().Replace("凸", " ");
                                xlblA.Font.Bold = true;
                                xlblA.ForeColor = Color.Red;

                                tc = new TableCell();
                                tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                                tc.Style.Add(HtmlTextWriterStyle.Height, "auto");
                                tc.Controls.Add(xlblA);

                                tr = new TableRow();
                                tr.Controls.Add(tc);
                                ph.Controls.Add(tr);

                            }
                            else
                            {
                                //------------------------------------------
                                //맞을 경우 
                                //------------------------------------------

                                //Label xlbl = (Label)this.ph.FindControl("lbl" + dr["QUESTION_ID"].ToString());
                                //xlbl.ForeColor = Color.Blue;
                                xlblQ.ForeColor = Color.Blue;
                                xUserScore += Convert.ToDouble(dr["QUESTION_SCORE"].ToString() == string.Empty ? "0" : dr["QUESTION_SCORE"].ToString());
                            }
                        }

                        //LABEL 넣어서 문제와 문제 사이에 한행 빈칸 만들기 
                        Label xlblBR = new Label();
                        xlblBR.ID = "lblbr" + dr["QUESTION_ID"].ToString();
                        xlblBR.Text = "<br>";
                        tc = new TableCell();
                        tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                        tc.Controls.Add(xlblBR);
                        tr = new TableRow();
                        tr.Controls.Add(tc);
                        ph.Controls.Add(tr);
                    }
                    #endregion
                }
                else
                {
                    //randome하게 정보 가져와야 함 
                    if (ViewState["EXAM"] == null)
                    {
                        dt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                     "GetExamQuestion_Random",
                                                     LMS_SYSTEM.MYPAGE,
                                                     "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);

                        #region Random 하게 문제 뿌리기, 뿌리면서 viewstate[exam]에 정보 저장 

                        ArrayList xarr = new ArrayList(); //random 정보 담기

                        int xQNo = 0;
                        TableRow tr = new TableRow();
                        TableCell tc = new TableCell();

                        foreach (DataRow dr in dt.Rows)
                        {
                            xTotalScore += Convert.ToDouble(dr["QUESTION_SCORE"].ToString() == string.Empty ? "0" : dr["QUESTION_SCORE"].ToString());

                            //100점 over 될 경우 skip 
                            if (xTotalScore > 100)
                                break;

                            //------------------------------------------
                            //문제 표기 
                            //------------------------------------------
                            xQNo++;
                            Label xlblQ = new Label();
                            xlblQ.ID = "lbl" + dr["QUESTION_ID"].ToString();
                            xlblQ.Text = "Question " + xQNo.ToString() + "<br>" + dr["QUESTION_CONTENT"].ToString();
                            xlblQ.Font.Bold = true;
                            //xlblQ01.ForeColor = Color.Blue;

                            tc = new TableCell();
                            tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                            tc.Style.Add(HtmlTextWriterStyle.Height, "auto");
                            tc.Controls.Add(xlblQ);

                            tr = new TableRow();
                            tr.Controls.Add(tc);
                            ph.Controls.Add(tr);

                            if (dr["QUESTION_TYPE"].ToString() == "000003" || dr["QUESTION_TYPE"].ToString() == "000006")
                            {
                                //------------------------------------------
                                //문제 보기 표기 - 4지선다, 다중선택 
                                //------------------------------------------
                                CheckBoxList xchl = new CheckBoxList();
                                xchl.ID = "chl" + dr["QUESTION_ID"].ToString();
                                //xchl.AutoPostBack = true; 
                                string[] xarrEx = dr["QUESTION_EXAMPLE"].ToString().Split('凸');
                                for (int i = 1; i < xarrEx.Length; i++)
                                {
                                    ListItem xlst = new ListItem();
                                    xlst.Value = i.ToString();
                                    xlst.Text = i.ToString() + ". " + xarrEx[i];
                                    xlst.Selected = false;
                                    xchl.Items.Add(xlst);
                                }

                                //ListItem 
                                tc = new TableCell();
                                tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                                tc.Style.Add(HtmlTextWriterStyle.Height, "auto");
                                tc.Controls.Add(xchl);

                                tr = new TableRow();
                                tr.Controls.Add(tc);
                                ph.Controls.Add(tr);
                            }
                            else
                            {
                                //------------------------------------------
                                //문제 보기 표기 - 4지선다, 다중선택 외 
                                //------------------------------------------
                                TextBox xtxtUA = new TextBox();
                                xtxtUA.ID = "txt" + dr["QUESTION_ID"].ToString();
                                xtxtUA.Text = string.Empty;
                                xtxtUA.Style.Add(HtmlTextWriterStyle.Width, "100%");
                                //xtxtUA.AutoPostBack = true; 

                                tc = new TableCell();
                                tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                                tc.Style.Add(HtmlTextWriterStyle.Height, "auto");
                                tc.Controls.Add(xtxtUA);

                                tr = new TableRow();
                                tr.Controls.Add(tc);
                                ph.Controls.Add(tr);
                            }

                            //----------------------------------------
                            // random 정보 담기 
                            //----------------------------------------
                            xarr.Add(new string[] { dr["COURSE_RESULT_SEQ"].ToString(), dr["COURSE_ID"].ToString(), dr["QUESTION_ID"].ToString() });

                            //LABEL 넣어서 문제와 문제 사이에 한행 빈칸 만들기 
                            Label xlblBR = new Label();
                            xlblBR.ID = "lblbr" + dr["QUESTION_ID"].ToString();
                            xlblBR.Text = "<br>";
                            tc = new TableCell();
                            tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                            tc.Controls.Add(xlblBR);
                            tr = new TableRow();
                            tr.Controls.Add(tc);
                            ph.Controls.Add(tr);
                        }

                        ViewState["EXAM"] = xarr;

                        #endregion

                    }
                    else
                    {
                        int xQNo = 0;
                        TableRow tr = new TableRow();
                        TableCell tc = new TableCell();

                        ArrayList xarr = (ArrayList)ViewState["EXAM"];
                        string xp = "('','')"; //course_id, question_id 조회 쌍 데이터 만들기 
                        string[] xtemp = null;
                        for (int i = 0; i < xarr.Count; i++)
                        {
                            //xarr.Add(new string[] { dr["COURSE_RESULT_SEQ"].ToString(), dr["COURSE_ID"].ToString(), dr["QUESTION_ID"].ToString() }); 
                            xtemp = (string[])xarr[i];
                            xp += ", ('" + xtemp[1] + "', '" + xtemp[2] + "' ) ";
                        }

                        xParams = new string[3];
                        xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                        xParams[1] = Session["user_id"].ToString();
                        xParams[2] = xp;
                        dt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                 "GetExamQuestion_RandomUse",
                                                 LMS_SYSTEM.MYPAGE,
                                                 "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);

                        DataRow[] xdrarr = null;
                        for (int k = 0; k < xarr.Count; k++)
                        {
                            xtemp = (string[])xarr[k];
                            xdrarr = dt.Select("COURSE_ID = '" + xtemp[1] + "' AND QUESTION_ID = '" + xtemp[2] + "' ");

                            //BINDING 
                            if (xdrarr.Length > 0)
                            {
                                DataRow dr = xdrarr[0];
                                //------------------------------------------
                                //문제 표기 
                                //------------------------------------------
                                xQNo++;
                                Label xlblQ = new Label();
                                xlblQ.ID = "lbl" + dr["QUESTION_ID"].ToString();
                                xlblQ.Text = "Question " + xQNo.ToString() + "<br>" + dr["QUESTION_CONTENT"].ToString();
                                xlblQ.Font.Bold = true;
                                //xlblQ01.ForeColor = Color.Blue;

                                tc = new TableCell();
                                tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                                tc.Style.Add(HtmlTextWriterStyle.Height, "auto");
                                tc.Controls.Add(xlblQ);

                                tr = new TableRow();
                                tr.Controls.Add(tc);
                                ph.Controls.Add(tr);

                                if (dr["QUESTION_TYPE"].ToString() == "000003" || dr["QUESTION_TYPE"].ToString() == "000006")
                                {
                                    //------------------------------------------
                                    //문제 보기 표기 - 4지선다, 다중선택 
                                    //------------------------------------------
                                    CheckBoxList xchl = new CheckBoxList();
                                    xchl.ID = "chl" + dr["QUESTION_ID"].ToString();
                                    //xchl.AutoPostBack = true; 
                                    string[] xarrEx = dr["QUESTION_EXAMPLE"].ToString().Split('凸');
                                    for (int i = 1; i < xarrEx.Length; i++)
                                    {
                                        ListItem xlst = new ListItem();
                                        xlst.Value = i.ToString();
                                        xlst.Text = i.ToString() + ". " + xarrEx[i];
                                        xlst.Selected = false;
                                        xchl.Items.Add(xlst);
                                    }

                                    //ListItem 
                                    tc = new TableCell();
                                    tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                                    tc.Style.Add(HtmlTextWriterStyle.Height, "auto");
                                    tc.Controls.Add(xchl);

                                    tr = new TableRow();
                                    tr.Controls.Add(tc);
                                    ph.Controls.Add(tr);
                                }
                                else
                                {
                                    //------------------------------------------
                                    //문제 보기 표기 - 4지선다, 다중선택 외 
                                    //------------------------------------------
                                    TextBox xtxtUA = new TextBox();
                                    xtxtUA.ID = "txt" + dr["QUESTION_ID"].ToString();
                                    //xtxtUA.Text = string.Empty;
                                    xtxtUA.Style.Add(HtmlTextWriterStyle.Width, "100%");
                                    //xtxtUA.AutoPostBack = true; 

                                    tc = new TableCell();
                                    tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                                    tc.Style.Add(HtmlTextWriterStyle.Height, "auto");
                                    tc.Controls.Add(xtxtUA);

                                    tr = new TableRow();
                                    tr.Controls.Add(tc);
                                    ph.Controls.Add(tr);
                                }

                                //LABEL 넣어서 문제와 문제 사이에 한행 빈칸 만들기 
                                Label xlblBR = new Label();
                                xlblBR.ID = "lblbr" + dr["QUESTION_ID"].ToString();
                                xlblBR.Text = "<br>";
                                tc = new TableCell();
                                tc.Style.Add(HtmlTextWriterStyle.Width, "100%");
                                tc.Controls.Add(xlblBR);
                                tr = new TableRow();
                                tr.Controls.Add(tc);
                                ph.Controls.Add(tr);
                            }
                            //BINDING END 
                        }
                    }
                }

                //------------------------------------------
                //이수, 미이수 되었을 경우 => 정답 표기 
                // 그외는 시험응시 메세지 표기 
                //------------------------------------------
                if (rRight)
                {
                    xReturn[0] = xRight;
                    xReturn[1] = dt.Rows.Count - xRight;

                    this.lblExamExplain.Text = "총 문제수: " + dt.Rows.Count + " (정답: " + xReturn[0].ToString() + ",  오답: " + xReturn[1].ToString() + ")<br>";
                    this.lblExamExplain.Text += "100점기준점수: " + Convert.ToString(Math.Round(xTotalScore, 0)) + ", 이수기준산정점수: " + Convert.ToString(Math.Round(xUserScore, 0)) + "<br><br>";
                }
                else
                {
                    this.lblExamExplain.Text = "시험응시 후에는 반드시 \"Send\"버튼을 클릭하셔야 시험 답안이 제출됩니다. <br>";
                    this.lblExamExplain.Text += "The answer sheet could be submitted, when you should click \"SUBMIT\" button after taking a exam.<br><br>";
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
        * Function name : Save
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        private void Save(string rSave)
        {
            try
            {
                ArrayList xarr = new ArrayList();
                int xCnt = 0;
                //USER_ID
                //OPEN_COURSE_ID
                //COURSE_RESULT_SEQ
                //COURSE_ID
                //QUESTION_ID

                string[] xParams = new string[2];
                xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                xParams[1] = Session["user_id"].ToString();
                DataTable dt = null;

                dt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                 "GetExamQuestion",
                                                 LMS_SYSTEM.MYPAGE,
                                                 "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string xAnswer = string.Empty;
                        if (dr["QUESTION_TYPE"].ToString() == "000003" || dr["QUESTION_TYPE"].ToString() == "000006")
                        {
                            xCnt = 0;
                            //xchl.ID = "chl" + dr["QUESTION_ID"].ToString(); 
                            CheckBoxList chl = (CheckBoxList)this.ph.FindControl("chl" + dr["QUESTION_ID"].ToString());
                            for (int i = 0; i < chl.Items.Count; i++)
                            {
                                if (chl.Items[i].Selected == true)
                                {
                                    xAnswer += "凸" + chl.Items[i].Value.ToString();
                                    xCnt++;
                                }
                            }

                            if (dr["QUESTION_TYPE"].ToString() == "000006")
                            {
                                if (xCnt > 1)
                                {
                                    rSave = "Error1";
                                    break;
                                }
                            }

                            xarr.Add(new string[] { dr["USER_ID"].ToString()
                                    , dr["OPEN_COURSE_ID"].ToString()
                                    , dr["COURSE_RESULT_SEQ"].ToString()
                                    , dr["COURSE_ID"].ToString()
                                    , dr["QUESTION_ID"].ToString()
                                    , xAnswer
                                    , (xAnswer.Trim() ==dr["QUESTION_ANSWER"].ToString() ? "Y" : "N")
                                });
                        }
                        else
                        {
                            //xtxtUA.ID = "txt" + dr["QUESTION_ID"].ToString();
                            TextBox txt = (TextBox)this.ph.FindControl("txt" + dr["QUESTION_ID"].ToString());
                            xAnswer = txt.Text;

                            if (dr["QUESTION_TYPE"].ToString() == "000002")
                            {
                                if (xAnswer.ToString() != string.Empty && (xAnswer.ToUpper() == "O" || xAnswer.ToUpper() == "X"))
                                {
                                    xAnswer = xAnswer.ToUpper();
                                }
                                else
                                {
                                    rSave = "Error2";
                                    break;
                                }
                            }

                            xarr.Add(new string[] { dr["USER_ID"].ToString()
                                    , dr["OPEN_COURSE_ID"].ToString()
                                    , dr["COURSE_RESULT_SEQ"].ToString()
                                    , dr["COURSE_ID"].ToString()
                                    , dr["QUESTION_ID"].ToString()
                                    , xAnswer
                                    , (xAnswer.Replace(" ", string.Empty).Trim() ==dr["QUESTION_ANSWER"].ToString().Replace(" ", string.Empty).Trim() ? "Y" : "N")
                                });
                        }
                    }
                }
                else
                {
                    //viewstate [exam] 에 따라서 값 넘기기 
                    ArrayList xarrExam = (ArrayList)ViewState["EXAM"];
                    string xp = "('','')"; //course_id, question_id 조회 쌍 데이터 만들기 
                    string[] xtemp = null;
                    for (int i = 0; i < xarrExam.Count; i++)
                    {
                        //xarr.Add(new string[] { dr["COURSE_RESULT_SEQ"].ToString(), dr["COURSE_ID"].ToString(), dr["QUESTION_ID"].ToString() }); 
                        xtemp = (string[])xarrExam[i];
                        xp += ", ('" + xtemp[1] + "', '" + xtemp[2] + "' ) ";
                    }

                    xParams = new string[3];
                    xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                    xParams[1] = Session["user_id"].ToString();
                    xParams[2] = xp;
                    dt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                             "GetExamQuestion_RandomUse",
                                             LMS_SYSTEM.MYPAGE,
                                             "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);

                    DataRow[] xdrarr = null;
                    for (int k = 0; k < xarrExam.Count; k++)
                    {
                        xtemp = (string[])xarrExam[k];
                        xdrarr = dt.Select("COURSE_ID = '" + xtemp[1] + "' AND QUESTION_ID = '" + xtemp[2] + "' ");

                        //BINDING 
                        if (xdrarr.Length > 0)
                        {
                            DataRow dr = xdrarr[0];
                            string xAnswer = string.Empty;
                            if (dr["QUESTION_TYPE"].ToString() == "000003" || dr["QUESTION_TYPE"].ToString() == "000006")
                            {
                                xCnt = 0;
                                //xchl.ID = "chl" + dr["QUESTION_ID"].ToString(); 
                                CheckBoxList chl = (CheckBoxList)this.ph.FindControl("chl" + dr["QUESTION_ID"].ToString());
                                for (int i = 0; i < chl.Items.Count; i++)
                                {
                                    if (chl.Items[i].Selected == true)
                                    {
                                        xAnswer += "凸" + chl.Items[i].Value.ToString();
                                        xCnt++;
                                    }
                                }

                                if (dr["QUESTION_TYPE"].ToString() == "000006")
                                {
                                    if (xCnt > 1)
                                    {
                                        rSave = "Error1";
                                        break;
                                    }
                                }

                                xarr.Add(new string[] { dr["USER_ID"].ToString()
                                    , dr["OPEN_COURSE_ID"].ToString()
                                    , dr["COURSE_RESULT_SEQ"].ToString()
                                    , dr["COURSE_ID"].ToString()
                                    , dr["QUESTION_ID"].ToString()
                                    , xAnswer
                                    , (xAnswer.Trim() ==dr["QUESTION_ANSWER"].ToString() ? "Y" : "N")
                                });
                            }
                            else
                            {
                                //xtxtUA.ID = "txt" + dr["QUESTION_ID"].ToString();
                                TextBox txt = (TextBox)this.ph.FindControl("txt" + dr["QUESTION_ID"].ToString());
                                xAnswer = txt.Text;

                                if (dr["QUESTION_TYPE"].ToString() == "000002")
                                {
                                    if (xAnswer.ToString() != string.Empty && (xAnswer.ToUpper() == "O" || xAnswer.ToUpper() == "X"))
                                    {
                                        xAnswer = xAnswer.ToUpper();
                                    }
                                    else
                                    {
                                        rSave = "Error2";
                                        break;
                                    }
                                }

                                xarr.Add(new string[] { dr["USER_ID"].ToString()
                                    , dr["OPEN_COURSE_ID"].ToString()
                                    , dr["COURSE_RESULT_SEQ"].ToString()
                                    , dr["COURSE_ID"].ToString()
                                    , dr["QUESTION_ID"].ToString()
                                    , xAnswer
                                    , (xAnswer.Replace(" ", string.Empty).Trim() ==dr["QUESTION_ANSWER"].ToString().Replace(" ", string.Empty).Trim() ? "Y" : "N")
                                });
                            }
                        }
                    }
                }

                //저장버튼 클릭 했을때만 타도록!! 
                if (rSave == "Save")
                {
                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                    "SetExam",
                                                    LMS_SYSTEM.MYPAGE,
                                                    "CLT.WEB.UI.LMS.MYPAGE", StrTable.MD2ArrayList2STR(xarr));

                    //A001: {0}이(가) 저장 되었습니다. 
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                      new string[] { "시험" },
                                                      new string[] { "Exam" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                }
                else if (rSave == "Submit")
                {
                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                                       "SetExamSubmit",
                                                                       LMS_SYSTEM.MYPAGE,
                                                                       "CLT.WEB.UI.LMS.MYPAGE", StrTable.MD2ArrayList2STR(xarr));

                    //A001: {0}이(가) 저장 되었습니다. 
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                      new string[] { "시험" },
                                                      new string[] { "Exam" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));

                    Response.Redirect("/mypage/exam.aspx?open_course_id=" + ViewState["OPEN_COURSE_ID"].ToString());
                }
                else if (rSave == "Error1")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A007",
                                                                    new string[] { "4지선다 정답", "1개" },
                                                                    new string[] { "Result", "1" },
                                                                    Thread.CurrentThread.CurrentCulture
                                                                   ));
                }
                else if (rSave == "Error2")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A052",
                                                           new string[] { "O/X", "O/X" },
                                                           new string[] { "O/X Result", "O/X" },
                                                           Thread.CurrentThread.CurrentCulture
                                                          ));
                }
                
                //===========================================================
                //===========================================================
                #region 수정전 
                /*
                string[] xParams = new string[2];
                xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                xParams[1] = Session["user_id"].ToString();
                DataTable dt = null; 

                dt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                 "GetExamQuestion",
                                                 LMS_SYSTEM.MYPAGE,
                                                 "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);

                foreach (DataRow dr in dt.Rows)
                {
                    string xAnswer = string.Empty;
                    if (dr["QUESTION_TYPE"].ToString() == "000003" || dr["QUESTION_TYPE"].ToString() == "000006")
                    {
                        //xchl.ID = "chl" + dr["QUESTION_ID"].ToString(); 
                        CheckBoxList chl = (CheckBoxList)this.ph.FindControl("chl" + dr["QUESTION_ID"].ToString());
                        for (int i = 0; i < chl.Items.Count; i++)
                        {
                            if (chl.Items[i].Selected == true)
                            {
                                xAnswer += "凸" + chl.Items[i].Value.ToString();
                            }
                        }
                        xarr.Add(new string[] { dr["USER_ID"].ToString()
                                    , dr["OPEN_COURSE_ID"].ToString()
                                    , dr["COURSE_RESULT_SEQ"].ToString()
                                    , dr["COURSE_ID"].ToString()
                                    , dr["QUESTION_ID"].ToString() 
                                    , xAnswer
                                    , (xAnswer.Trim() ==dr["QUESTION_ANSWER"].ToString() ? "Y" : "N")
                                });
                    }
                    else
                    {
                        //xtxtUA.ID = "txt" + dr["QUESTION_ID"].ToString();
                        TextBox txt = (TextBox)this.ph.FindControl("txt" + dr["QUESTION_ID"].ToString());
                        xAnswer = txt.Text;
                        xarr.Add(new string[] { dr["USER_ID"].ToString()
                                    , dr["OPEN_COURSE_ID"].ToString()
                                    , dr["COURSE_RESULT_SEQ"].ToString()
                                    , dr["COURSE_ID"].ToString()
                                    , dr["QUESTION_ID"].ToString() 
                                    , xAnswer
                                    , (xAnswer.Replace(" ", string.Empty).Trim() ==dr["QUESTION_ANSWER"].ToString().Replace(" ", string.Empty).Trim() ? "Y" : "N")
                                });
                    }
                }                

                //저장버튼 클릭 했을때만 타도록!! 
                if (rSave)
                {
                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                    "SetExam",
                                                    LMS_SYSTEM.MYPAGE,
                                                    "CLT.WEB.UI.LMS.MYPAGE", StrTable.MD2ArrayList2STR(xarr));

                    //A001: {0}이(가) 저장 되었습니다. 
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                      new string[] { "시험" },
                                                      new string[] { "Exam" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                }
                else
                {
                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                                       "SetExamSubmit",
                                                                       LMS_SYSTEM.MYPAGE,
                                                                       "CLT.WEB.UI.LMS.MYPAGE", StrTable.MD2ArrayList2STR(xarr));

                    //A001: {0}이(가) 저장 되었습니다. 
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001",
                                                      new string[] { "시험" },
                                                      new string[] { "Exam" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                    //this.Page_Load(null, null); 

                    Response.Redirect("/mypage/exam.aspx?open_course_id=" + ViewState["OPEN_COURSE_ID"].ToString()); 

                    //this.BindData(true);
                    //this.lblExamExplain.Text = "맞는개수: " + xR[0].ToString() + "  틀린개수: " + xR[1].ToString() + "<br>"; 

                    //string xScriptMsg = string.Empty; 
                    //xScriptMsg = "<script>";
                    //xScriptMsg += "  opener.__doPostBack('ctl00$ContentPlaceHolderMainUp$Page_Load','');";
                    //xScriptMsg += "</script>";

                    //ScriptHelper.ScriptBlock(this, "vp_p_exam_wpg", xScriptMsg);
                }
                 * */
                #endregion 
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        
        /************************************************************
        * Function name : btnSave_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnSave_Click(object sender, EventArgs e)
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //int xCnt = 0;
                //CheckBoxList chl = (CheckBoxList)this.ph.FindControl("chl" + dr["QUESTION_ID"].ToString());
                //for (int i = 0; i < chl.Items.Count; i++)
                //{
                //    if (chl.Items[i].Selected == true)
                //    {
                //        xAnswer += "凸" + chl.Items[i].Value.ToString();
                //        xCnt++;
                //    }
                //}
                if (ViewState["PASS_FLG"].ToString() == "000001" || ViewState["PASS_FLG"].ToString() == "000005")
                {
                    //Cannot select {0}.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A020",
                                                      new string[] { "Save" },
                                                      new string[] { "Save" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                    return;
                }
                this.Save("Save");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion 

        /************************************************************
        * Function name : btnSubmit_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnSubmit_Click(object sender, EventArgs e)
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["PASS_FLG"].ToString() == "000001" || ViewState["PASS_FLG"].ToString() == "000005")
                {
                    //Cannot select {0}.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A020",
                                                      new string[] { "Submit" },
                                                      new string[] { "Submit" },
                                                      Thread.CurrentThread.CurrentCulture
                                                     ));
                    return;
                }
                this.Save("Submit");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion 

    }
}
