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
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Drawing;
using System.IO;

namespace CLT.WEB.UI.LMS.MYPAGE
{
    public partial class study : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "study", xScriptMsg);

                    return;
                }

                if (Request.QueryString["OPEN_COURSE_ID"] != null && Request.QueryString["OPEN_COURSE_ID"].ToString() != "")
                {
                    ViewState["OPEN_COURSE_ID"] = Request.QueryString["OPEN_COURSE_ID"].ToString();
                    if (!IsPostBack)
                    {
                        ViewState["COURSE_ID"] = string.Empty; //binddata에서 course_id 넣어줌 
                        ViewState["COURSE_NM"] = string.Empty; //binddata에서 COURSE_NM 넣어줌 
                        this.BindData();
                        
                        //HtmlGenericControl param = new HtmlGenericControl("PARAM");
                        //FlashObj.Controls.Add(param);

                        //param.Attributes["name"] = "dynamic_prop";
                        //param.Attributes["value"] = "the value!";
                    }
                }
                base.pRender(this.Page, new object[,] { { this.btnNotice, "I" }, { this.btnData, "I" }, { this.btnNext, "E" }, { this.btnSurvey, "E" }, { this.btnExam, "E" }, { this.btnPrevious, "E" } }, Convert.ToString(Request.QueryString["MenuCode"]));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : BindData
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        private void BindData()
        {
            try
            {
                string[] xParams = new string[2];
                DataTable xDt = null;
                DataTable xDtText = null;

                xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                xParams[1] = Session["user_id"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                 "GetStudyTreeList",
                                                 LMS_SYSTEM.MYPAGE,
                                                 "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);

                xDtText = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                 "GetStudyTreeTextList",
                                                 LMS_SYSTEM.MYPAGE,
                                                 "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);


                //this.TreeView.Nodes.Add(new TreeNode()); 
                //TreeNode x = this.TreeView.Nodes[0]; 
                string xSubjectiD = string.Empty;

                int xHindex = 0;  //Header Node Index
                int xDindex = 0;  //Detail Node Index

                foreach (DataRow dr in xDt.Rows)
                {
                    ViewState["COURSE_ID"] = dr["COURSE_ID"].ToString();  //민규수석이 필요 하다함 
                    ViewState["COURSE_NM"] = dr["COURSE_NM"].ToString(); //민규수석이 필요 하다함 

                    if (xSubjectiD != dr["SUBJECT_SEQ"].ToString())
                    {
                        if (xSubjectiD == string.Empty)
                            xHindex = 0;
                        else
                            xHindex++;
                        xDindex = 0; //subject가 바뀌면 항상 초기화 

                        TreeNode nodeS = new TreeNode();
                        nodeS.Text = dr["SUBJECT_NM"].ToString();
                        nodeS.Value = dr["SUBJECT_ID"].ToString();

                        //nodeS.PopulateOnDemand = true; //자식노드 있을때는 설정할 수 없음 
                        nodeS.SelectAction = TreeNodeSelectAction.SelectExpand;
                        this.TreeView.Nodes.Add(nodeS);

                        xSubjectiD = dr["SUBJECT_SEQ"].ToString();
                    }
                    
                    if (xSubjectiD != string.Empty && this.TreeView.Nodes.Count > 0)
                    {
                        TreeNode nodeC = new TreeNode();
                        nodeC.Text = dr["CONTENTS_NM"].ToString();
                        //nodeC.Value = dr["CONTENTS_ID"].ToString();

                        nodeC.Value = dr["VINFO"].ToString() + "|" + xHindex.ToString() + "|" + xDindex.ToString();
                        //SUBJECT_ID | CONTENTS_ID | CONTENTS_FILE_NM | ROW_NUMBER | H INDEX | D INDEX 

                        nodeC.PopulateOnDemand = true;
                        nodeC.SelectAction = TreeNodeSelectAction.SelectExpand;
                        nodeC.ShowCheckBox = true;
                        nodeC.SelectAction = TreeNodeSelectAction.Select;
                        //
                        //nodeC.NavigateUrl = Server.MapPath(ContentsFilePath) + dr["CONTENTS_FILE_NM"].ToString(); 

                        //SUBJECT가 동일 하고, SEQ가 MAX SEQ보다 작을 경우 
                        if (xSubjectiD == dr["LAST_SUBJECT_SEQ"].ToString()
                            && dr["LAST_CONTENTS_SEQ"].ToString() != string.Empty
                            && Convert.ToInt16(dr["CONTENTS_SEQ"].ToString()) <= Convert.ToInt16(dr["LAST_CONTENTS_SEQ"].ToString()))
                        {
                            nodeC.Checked = true;
                        }
                        if (dr["LAST_SUBJECT_SEQ"].ToString() != string.Empty
                            && Convert.ToInt32(dr["SUBJECT_SEQ"].ToString()) < Convert.ToInt32(dr["LAST_SUBJECT_SEQ"].ToString()))
                        {
                            nodeC.Checked = true;
                        }

                        this.TreeView.Nodes[this.TreeView.Nodes.Count - 1].ChildNodes.Add(nodeC);
                        //TreeView1.Nodes[0].ChildNodes[0].ShowCheckBox = true;

                        xDindex++; //index 증분 
                    }

                    this.TreeView.SelectedNodeChanged -= new EventHandler(TreeView_SelectedNodeChanged);
                    TreeView.Nodes[0].ChildNodes[0].Selected = true;
                    this.TreeView_SelectedNodeChanged(null, null);
                    this.TreeView.SelectedNodeChanged += new EventHandler(TreeView_SelectedNodeChanged);
                    //TreeNode node = new TreeNode();
                    //node.Text = dr["SUBJECT_NM"].ToString();
                    //node.Value = dr["SUBJECT_ID"].ToString();
                    //node.PopulateOnDemand = true;
                    //node.SelectAction = TreeNodeSelectAction.SelectExpand;
                    ////x.ChildNodes.Add(node); 
                    //this.TreeView.Nodes.Add(node); 
                }
                //ftp://203.246.154.123/newlms/file/contents/
                //FileInfo xfileinfo = new FileInfo(Server.MapPath(ContentsFilePath) + rFilename);

                DataTable dtText = new DataTable();
                DataColumn col = new DataColumn("btnText", Type.GetType("System.String"));
                dtText.Columns.Add(col);
                col = new DataColumn("textbooknm", Type.GetType("System.String"));
                dtText.Columns.Add(col);
                col = new DataColumn("textbookid", Type.GetType("System.String"));
                dtText.Columns.Add(col);
                col = new DataColumn("textbookfilenm", Type.GetType("System.String"));
                dtText.Columns.Add(col);

                foreach (DataRow dr in xDtText.Rows)
                {
                    DataRow rows = dtText.NewRow();
                    rows[0] = string.Empty;
                    rows[1] = dr["TEXTBOOK_NM"].ToString(); //name                    
                    rows[2] = dr["TEXTBOOK_ID"].ToString(); //id
                    rows[3] = dr["TEXTBOOK_FILE_NM"].ToString(); //id
                    dtText.Rows.Add(rows);
                }

                this.dtlText.DataSource = dtText;
                this.dtlText.DataBind();

                //this.TreeView.SelectedNode.Select();
                //this.TreeView_SelectedNodeChanged(null, null);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /************************************************************
        * Function name : btnDown_Click
        * Purpose       : File Download 버튼 Click 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnDown = (Button)sender;
                string xTextBookId = string.Empty;
                string xTextBookFileNm = string.Empty;
                object obj = null;

                for (int i = 0; i < this.dtlText.Items.Count; i++)
                {
                    Button xtemp = ((Button)this.dtlText.Items[i].FindControl("btnText"));
                    if (btnDown.UniqueID == xtemp.UniqueID)
                    {
                        xTextBookId = ((Label)this.dtlText.Items[i].FindControl("lblTextbookid")).Text;
                        xTextBookFileNm = ((Label)this.dtlText.Items[i].FindControl("lblTextbookFileNm")).Text; //

                        obj = SBROKER.GetObject("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                 "GetStudyTreeTextFile",
                                                 LMS_SYSTEM.MYPAGE,
                                                 "CLT.WEB.UI.LMS.MYPAGE", xTextBookId);
                        byte[] fileByte = (byte[])obj;

                        string xFilePath = Server.MapPath("\\file\\tempfile\\") + xTextBookFileNm;
                        FileStream xNewFile = new FileStream(xFilePath, FileMode.Create);

                        xNewFile.Write(fileByte, 0, fileByte.Length); // byte 배열 내용파일 쓰는 처리
                        xNewFile.Close(); // 파일 닫는 처리

                        Response.Clear();

                        FileInfo xfileinfo = new FileInfo(xFilePath);

                        Response.Clear();
                        Response.ContentType = "application/octet-stream";

                        if (Request.UserAgent.IndexOf("MSIE") >= 0)  // InternetExplorer 일 경우
                        {
                            /*
                            * 2013.08.20 Seojw
                            * 첨부파일 다운로드 시 스페이스에 "+"기호 붙는걸 방지하기 위해
                            * HttpUtility.UrlEncode → HttpUtility.HttpUtility.UrlPathEncode 로 변경
                            */
                            //Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", HttpUtility.UrlEncode(xTextBookFileNm)));
                            Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", HttpUtility.UrlPathEncode(xTextBookFileNm)));
                        }
                        else
                            Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", HttpUtility.UrlDecode(xTextBookFileNm)));
                        
                        Response.TransmitFile(xfileinfo.FullName); //, 0, -1);
                        Response.Flush();
                        
                        if (File.Exists(xFilePath))
                            File.Delete(xFilePath);

                        //Response.End();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /************************************************************
        * Function name : btnPrev_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnPrev_Click(object sender, EventArgs e)
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode xnode = this.TreeView.SelectedNode;
                string xSelValue = this.TreeView.SelectedValue;
                //xSelValue = SUBJECT_ID | CONTENTS_ID | CONTENTS_FILE_NM | ROW_NUMBER | H INDEX | D INDEX 

                string[] xParams = new string[2];
                DataTable xDt = null;

                xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                xParams[1] = Session["user_id"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                 "GetStudyTreeList",
                                                 LMS_SYSTEM.MYPAGE,
                                                 "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);
                
                //현재 선택된 NODE의 ROW_SEQ 이전 ROW_SEQ로 SUBJECT_ID, CONTENTS_ID를 찾아서 
                //TREENODE LOOP 돌아 가며 INDEX 찾아서 SELECT 해줌 
                if (xSelValue.IndexOf("|") > 0)
                {
                    string[] xKey = xSelValue.Split('|');
                    int xPreSeq = Convert.ToInt32(xKey[3]) - 1;

                    DataRow[] xdrarr = xDt.Select("ROW_SEQ = " + xPreSeq);
                    DataRow xdr = null;
                    if (xdrarr.Length > 0)
                    {
                        xdr = xdrarr[0];
                        string xSubjectId = xdr["SUBJECT_ID"].ToString();
                        string xContentsId = xdr["CONTENTS_ID"].ToString();
                        for (int i = 0; i < this.TreeView.Nodes.Count; i++)
                        {
                            for (int k = 0; k < this.TreeView.Nodes[i].ChildNodes.Count; k++)
                            {
                                string[] xPreKey = this.TreeView.Nodes[i].ChildNodes[k].Value.Split('|');
                                if (xSubjectId == xPreKey[0] && xContentsId == xPreKey[1])
                                {
                                    this.TreeView.Nodes[i].ChildNodes[k].Select();
                                    this.TreeView_SelectedNodeChanged(sender, e);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //A003
                    //{0} was not selected.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003",
                                                                                            new string[] { "컨텐츠" },
                                                                                            new string[] { "Contents" },
                                                                                            Thread.CurrentThread.CurrentCulture
                                                                                           ));
                }
                /*
                //======================================
                string[] xParams = new string[2];
                DataTable xDt = null;
                xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                xParams[1] = Session["user_id"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                 "GetStudyTreeList",
                                                 LMS_SYSTEM.MYPAGE,
                                                 "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);
                if (xSelValue.IndexOf("|") > 0)
                {
                    //contents 선택 
                    string[] xKey = xSelValue.Split('|');
                    DataRow[] xdrarr = xDt.Select("SUBJECT_ID = '" + xKey[0] + "' AND CONTENTS_ID = '" + xKey[1] + "' ");
                    DataRow xdr = null;
                    if (xdrarr.Length > 0)
                    {
                        xdr = xdrarr[0];
                    }
                }
                //======================================
                string xSelPreKey = string.Empty; 
                if (xSelValue.IndexOf("|") > 0)
                {
                    string[] xKey = xSelValue.Split('|');
                    //for(int i = 0; i< this.
                    for(int i = 0; i<xDt.Rows.Count; i++)
                    {
                        DataRow dr = xDt.Rows[i];    
                        if (dr["SUBJECT_ID"].ToString() == xKey[0] && dr["CONTENTS_ID"].ToString() == xKey[1])
                        {
                            if (i != 0)
                            {
                                xSelPreKey = xDt.Rows[i - 1]["SUBJECT_ID"].ToString() + "|" + xDt.Rows[i - 1]["CONTENTS_ID"].ToString() + xDt.Rows[i - 1]["CONTENTS_FILE_NM"].ToString();
                                ////this.TreeView.SelectedValue = xSelPreKey;
                                ////this.TreeView.Nodes[
                                //this.TreeView_SelectedNodeChanged(sender, e); 
                            }
                        }
                    }
                }
                */
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion 
        
        /************************************************************
        * Function name : btnNext_Click
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnNext_Click(object sender, EventArgs e)
        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode xnode = this.TreeView.SelectedNode;
                string xSelValue = this.TreeView.SelectedValue;
                //xSelValue = SUBJECT_ID | CONTENTS_ID | CONTENTS_FILE_NM | ROW_NUMBER | H INDEX | D INDEX 

                string[] xParams = new string[2];
                DataTable xDt = null;

                xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                xParams[1] = Session["user_id"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                 "GetStudyTreeList",
                                                 LMS_SYSTEM.MYPAGE,
                                                 "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);
                
                //현재 선택된 NODE의 ROW_SEQ 이전 ROW_SEQ로 SUBJECT_ID, CONTENTS_ID를 찾아서 
                //TREENODE LOOP 돌아 가며 INDEX 찾아서 SELECT 해줌 
                if (xSelValue.IndexOf("|") > 0)
                {
                    string[] xKey = xSelValue.Split('|');
                    int xNextSeq = Convert.ToInt32(xKey[3]) + 1;

                    DataRow[] xdrarr = xDt.Select("ROW_SEQ = " + xNextSeq);
                    DataRow xdr = null;
                    if (xdrarr.Length > 0)
                    {
                        xdr = xdrarr[0];
                        string xSubjectId = xdr["SUBJECT_ID"].ToString();
                        string xContentsId = xdr["CONTENTS_ID"].ToString();
                        for (int i = 0; i < this.TreeView.Nodes.Count; i++)
                        {
                            for (int k = 0; k < this.TreeView.Nodes[i].ChildNodes.Count; k++)
                            {
                                string[] xPreKey = this.TreeView.Nodes[i].ChildNodes[k].Value.Split('|');
                                if (xSubjectId == xPreKey[0] && xContentsId == xPreKey[1])
                                {
                                    this.TreeView.Nodes[i].ChildNodes[k].Select();
                                    this.TreeView_SelectedNodeChanged(sender, e);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //A003
                    //{0} was not selected.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003",
                        new string[] { "컨텐츠" },
                        new string[] { "Contents" },
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
        * Function name : TreeView_SelectedNodeChanged
        * Purpose       : 
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void TreeView_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                TreeNode xnode = this.TreeView.SelectedNode;
                string xSelValue = this.TreeView.SelectedValue;

                //======================================
                string[] xParams = new string[2];
                DataTable xDt = null;

                xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                xParams[1] = Session["user_id"].ToString();

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                 "GetStudyTreeList",
                                                 LMS_SYSTEM.MYPAGE,
                                                 "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);
                //======================================

                //xSelValue = SUBJECT_ID | CONTENTS_ID | CONTENTS_FILE_NM | ROW_NUMBER | H INDEX | D INDEX 

                if (xSelValue.IndexOf("|") > 0)
                {
                    //contents 선택 
                    string[] xKey = xSelValue.Split('|');


                    DataRow[] xdrarr = xDt.Select("SUBJECT_ID = '" + xKey[0] + "' AND CONTENTS_ID = '" + xKey[1] + "' ");
                    DataRow xdr = null;

                    if (xdrarr.Length > 0)
                    {
                        xdr = xdrarr[0];
                        string xLastSubject = xdr["LAST_SUBJECT_ID"].ToString();
                        string xLastContents = xdr["LAST_CONTENTS_ID"].ToString();

                        string xSeq = xdr["CONTENTS_SEQ"].ToString();
                        string xPath = System.Configuration.ConfigurationManager.AppSettings["ContentsFilePath"].ToString() + xdr["CONTENTS_FILE_NM"].ToString();

                        string xRowSeq = xdr["ROW_SEQ"].ToString();

                        if (xLastSubject == string.Empty)
                        {
                            // LAST_SUBJECT_ID, LAST_CONTENTS_ID 가 NULL일 경우 
                            // 첫번째 컨텐츠를 선택 했는지 여부 확인 하여
                            // 첫번째 컨텐츠를 선택 하도록 함 

                            if (xKey[3] != "1")
                            {
                                ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A020",
                                                                                             new string[] { "컨텐츠" },
                                                                                             new string[] { "Contents" },
                                                                                             Thread.CurrentThread.CurrentCulture
                                                                                            ));
                            }
                            else
                            {
                                xParams = new string[5];
                                xParams[0] = Session["user_id"].ToString();
                                xParams[1] = ViewState["OPEN_COURSE_ID"].ToString();
                                xParams[2] = xKey[0]; //subject_id
                                xParams[3] = xKey[1]; //contents_id 

                                xParams[4] = xRowSeq; //현재 SEQ 

                                //xParams[4] = xSeq; //CONTENTS_SEQ 
                                //xParams[5] = xLastSubject;
                                //xParams[6] = xLastContents;


                                string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                        "SetStudyUpdate",
                                                        LMS_SYSTEM.MYPAGE,
                                                        "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);

                                ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>flash(\"" + xPath + "\");</script>");
                                this.txtPage.Text = xSeq + " / " + xdr["TOTAL"].ToString();
                                xnode.Checked = true;
                            }

                        }
                        else
                        {
                            //선택한 컨텐츠가 마지막 들은 컨텐츠의 다음 컨텐츠 인지 여부 확인 
                            // LAST_SUBJECT_ID, LAST_CONTENTS_ID 의 ROW_SEQ 확인 하여 
                            // 선택한 컨텐츠의 ROW_NUMBER 확인 하여 바로 다음차순일 경우에만 실행~!! 

                            DataRow[] xdrarrLast = xDt.Select("SUBJECT_ID = '" + xLastSubject + "' AND CONTENTS_ID = '" + xLastContents + "' ");
                            DataRow xdrLast = null;
                            if (xdrarrLast.Length > 0)
                            {
                                xdrLast = xdrarrLast[0];
                                int xLastRowSeq = Convert.ToInt32(xdrLast["ROW_SEQ"].ToString());
                                int xSelRowSeq = Convert.ToInt32(xKey[3]);
                                if (xSelRowSeq == xLastRowSeq + 1)
                                {
                                    xParams = new string[5];
                                    xParams[0] = Session["user_id"].ToString();
                                    xParams[1] = ViewState["OPEN_COURSE_ID"].ToString();
                                    xParams[2] = xKey[0]; //subject_id
                                    xParams[3] = xKey[1]; //contents_id 

                                    xParams[4] = xSelRowSeq.ToString();

                                    //xParams[4] = xSeq; //CONTENTS_SEQ 
                                    //xParams[5] = xLastSubject;
                                    //xParams[6] = xLastContents;


                                    string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                            "SetStudyUpdate",
                                                            LMS_SYSTEM.MYPAGE,
                                                            "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);

                                    ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>flash(\"" + xPath + "\");</script>");
                                    this.txtPage.Text = xSeq + " / " + xdr["TOTAL"].ToString();
                                    xnode.Checked = true;
                                }
                                else if (xSelRowSeq <= xLastRowSeq)
                                {
                                    ////이전 컨텐츠 선택 했을 경우는 보여줌 
                                    //xParams = new string[7];
                                    //xParams[0] = Session["user_id"].ToString();
                                    //xParams[1] = ViewState["OPEN_COURSE_ID"].ToString();
                                    //xParams[2] = xKey[0]; //subject_id
                                    //xParams[3] = xKey[1]; //contents_id 
                                    //xParams[4] = xSeq; //CONTENTS_SEQ 
                                    //xParams[5] = xLastSubject;
                                    //xParams[6] = xLastContents;


                                    //string xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                    //                        "SetStudyUpdate",
                                    //                        LMS_SYSTEM.MYPAGE,
                                    //                        "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);

                                    ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>flash(\"" + xPath + "\");</script>");
                                    this.txtPage.Text = xSeq + " / " + xdr["TOTAL"].ToString();
                                    xnode.Checked = true;
                                }
                                else
                                {
                                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A020",
                                                            new string[] { "컨텐츠" },
                                                            new string[] { "Contents" },
                                                            Thread.CurrentThread.CurrentCulture
                                                            ));
                                }
                            }
                        }
                    }
                    //FileInfo xfileinfo = new FileInfo(Server.MapPath(ContentsFilePath) + rFilename);
                }
                else
                {
                    //SUBJECT 선택 
                }
                //DataRow[] dr =  idtContents.Select("SUBJECT_ID = '" + xnode
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        
        /************************************************************
        * Function name : btnNotice_OnClick
        * Purpose       : 공지사항 페이지 이동
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnNotice_OnClick(object sender, EventArgs e)
        protected void btnNotice_OnClick(object sender, EventArgs e)
        {
            try
            {
                string xURL = string.Format("<script>opener.parent.window.location.href='/community/edu_notice_list.aspx?COURSE_ID={0}&COURSE_NM={1}&OPEN_COURSE_ID={2}';opener.focus();</script>", ViewState["COURSE_ID"], ViewState["COURSE_NM"], ViewState["OPEN_COURSE_ID"]);
                Response.Write(xURL);
                //Response.Redirect(xURL,false);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnData_OnClick
        * Purpose       : 자료실 페이지 이동
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnData_OnClick(object sender, EventArgs e)
        protected void btnData_OnClick(object sender, EventArgs e)
        {
            try
            {
                string xURL = "<script>opener.parent.window.location.href='/community/data_list.aspx';opener.focus();</script>";
                Response.Write(xURL);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnSurvey_OnClick
        * Purpose       : 설문조사 페이지 이동
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnSurvey_OnClick(object sender, EventArgs e)
        protected void btnSurvey_OnClick(object sender, EventArgs e)
        {
            try
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

                    if (Convert.ToInt32(dr["PROGRESS_RATE"].ToString()) == 100) // 진도율이 100% 일때 
                    {
                        //<a id="res_sub1" href="/community/survey_answer_detail.aspx?rResNo=<%# DataBinder.Eval(Container.DataItem, "res_no")%>&rRes_sub=<%# DataBinder.Eval(Container.DataItem, "res_sub")%>&rRes_object=<%# DataBinder.Eval(Container.DataItem, "res_object")%>&rRes_date=<%# DataBinder.Eval(Container.DataItem, "res_date")%>&rAnswer_yn=N"><%# DataBinder.Eval(Container.DataItem, "res_sub")%></a>
                        //string xURL = string.Format("<script>opener.parent.window.location.href='/community/survey_answer_list_wpg.aspx?OPEN_COURSE_ID={0}&USER_ID={1}';opener.focus();</script>", ViewState["OPEN_COURSE_ID"], Session["USER_ID"].ToString());

                        DataTable dtSurvey = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_survey_md",
                                                         "GetSurveyAnswerInfo",
                                                         LMS_SYSTEM.MYPAGE,
                                                         "CLT.WEB.BIZ.LMS.MANAGE", (object)ViewState["OPEN_COURSE_ID"]);
                        if (dtSurvey.Rows.Count > 0)
                        {
                            string ResNo = dtSurvey.Rows[0]["res_no"].ToString();
                            string Res_sub = dtSurvey.Rows[0]["res_sub"].ToString();
                            string Res_object = dtSurvey.Rows[0]["res_object"].ToString();
                            string Res_date = dtSurvey.Rows[0]["Res_date"].ToString();
                            
                            string xURL = string.Format("<script>opener.parent.window.location.href='/community/survey_answer_detail.aspx?rResNo={0}&rRes_sub={1}&rRes_object={2}&rRes_date={3}&rAnswer_yn=N';opener.focus();</script>", ResNo, Res_sub, Res_object, Res_date);
                            Response.Write(xURL);
                        }
                        return;
                    }
                }

                ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A018",
                                      new string[] { "학습" },
                                      new string[] { "Study" },
                                      Thread.CurrentThread.CurrentCulture
                                     ));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnExam_OnClick
        * Purpose       : 시험 페이지 이동
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnExam_OnClick(object sender, EventArgs e)
        protected void btnExam_OnClick(object sender, EventArgs e)
        {
            try
            {
                //진도율 100% 인지 확인 
                string[] xParams = new string[2];
                xParams[0] = ViewState["OPEN_COURSE_ID"].ToString();
                xParams[1] = Session["user_id"].ToString();

                DataTable dt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MYPAGE.vp_p_study_md",
                                                 "GetExamCheck",
                                                 LMS_SYSTEM.MYPAGE,
                                                 "CLT.WEB.UI.LMS.MYPAGE", (object)xParams);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    if (dr["answer_yn"].ToString() == "Y") // 설문조사를 완료 했을때
                    {
                        string xURL = string.Format("<script>openPopWindow('/mypage/exam.aspx?open_course_id={0}&MenuCode={1}','exam_win', '1000', '671');</script>", ViewState["OPEN_COURSE_ID"], Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "study", xURL);
                        return;
                    }
                }
                else
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A125",
                      new string[] { "등록된 과정 설문조사" },
                      new string[] { "Registered Course Survey" },
                      Thread.CurrentThread.CurrentCulture
                     ));

                    return;
                }

                ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A126",
                                      new string[] { "설문조사" },
                                      new string[] { "Survey" },
                                      Thread.CurrentThread.CurrentCulture
                                     ));
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        protected void TreeView_OnTreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            try
            {
                //if (e.Node.Selected)
                e.Node.Checked = true;   // 체크 해제 불가
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

    }
}