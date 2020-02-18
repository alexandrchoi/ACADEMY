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
    /// 1. 작업개요 : 강사 정보관리 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 강사 정보관리
    ///				  
    /// 3. Class 명 : lecturer_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    /// 
    public partial class lecturer_list : BasePage
    {
        string[] xHeader = null;
        public static string iDelYN = "N";

        /************************************************************
        * Function name : Page_Load
        * Purpose       : Page_Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                if (!IsPostBack)
                {
                    //Enter Key 입력시 Search 되도록 설정
                    //this.txtCreated_From.Attributes.Add("onkeyup", "ChkDate(this);");
                    //this.txtCreated_To.Attributes.Add("onkeyup", "ChkDate(this);");

                    base.pRender(this.Page, new object[,] { { this.btnNew, "A" },
                                                        { this.btnDelete, "D" },
                                                        { this.btnExcel, "I" },
                                                        { this.btnRetrieve, "I" }
                                                      });
                    //BindDropDownList();
                    this.ddlDelYn.Items.Add(new ListItem("ALL", ""));
                    this.ddlDelYn.Items.Add(new ListItem("N", "N"));
                    this.ddlDelYn.Items.Add(new ListItem("Y", "Y"));
                    this.ddlDelYn.SelectedValue = string.IsNullOrEmpty(Request.QueryString["delYN"]) ? iDelYN : Request.QueryString["delYN"].ToString();

                    SetGridClear(this.C1WebGrid1, this.PageNavigator1, this.PageInfo1);
                    this.PageInfo1.TotalRecordCount = 0;
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageNavigator1.TotalRecordCount = 0;
                    
                    if (Session["USER_GROUP"].ToString() == "000009")
                    {
                        //return;
                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "lecturer_list", xScriptMsg);
                    }
                    else
                    {
                        BindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        protected void DdlDelYn_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGridClear(this.C1WebGrid1, this.PageNavigator1, this.PageInfo1);
            BindGrid();
        }

        /************************************************************
        * Function name : button_Click
        * Purpose       : 버튼클릭 이벤트 (btnRetrieve, btnDelete, btnExcel)
        * Input         : void
        * Output        : void
        *************************************************************/
        #region button_Click(object seder, EventArgs e)
        protected void button_Click(object seder, EventArgs e)
        {
            try
            {
                Button btn = (Button)seder;
                if (btn.ID == "btnRetrieve") // 조회
                {
                    if (!string.IsNullOrEmpty(txtCreated_From.Text) && string.IsNullOrEmpty(txtCreated_To.Text))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A039", new string[] { "기간" }, new string[] { "Period" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    else if (string.IsNullOrEmpty(txtCreated_From.Text) && !string.IsNullOrEmpty(txtCreated_To.Text))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A039", new string[] { "기간" }, new string[] { "Period" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    BindGrid();
                }
                else if (btn.ID == "btnDelete") // 삭제
                {
                    Delete();
                    PageInfo1.CurrentPageIndex = 1;
                    PageNavigator1.CurrentPageIndex = 1;

                }
                else if (btn.ID == "btnExcel") // 엑셀출력
                {
                    Excel();
                }
                else if (btn.ID == "btnNew") // 신규등록
                {
                    //string xScriptContent = string.Format("<script>openPopWindow('/curr/lecturer_edit.aspx?EDITMODE=NEW&MenuCode={0}', 'lecturer_list', '1024', '721');</script>", Session["MENU_CODE"]);
                    //ScriptHelper.ScriptBlock(this, "lecturer_list", xScriptContent);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void grd_ItemCreated(object sender, C1ItemEventArgs e)
        protected void grd_ItemCreated(object seder, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    xHeader = new string[10];
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[1].Text = "순번";
                        e.Item.Cells[2].Text = "성명";
                        e.Item.Cells[3].Text = "회사명";
                        e.Item.Cells[4].Text = "직위";
                        e.Item.Cells[5].Text = "휴대폰";
                        e.Item.Cells[6].Text = "이메일";
                        e.Item.Cells[7].Text = "가입일";
                        e.Item.Cells[8].Text = "이력서";
                        e.Item.Cells[9].Text = "증빙서류";
                    }
                    else
                    {
                        e.Item.Cells[1].Text = "No";
                        e.Item.Cells[2].Text = "Lecturer";
                        e.Item.Cells[3].Text = "Company";
                        e.Item.Cells[4].Text = "Grade";
                        e.Item.Cells[5].Text = "Mobile";
                        e.Item.Cells[6].Text = "E-mail";
                        e.Item.Cells[7].Text = "Date of Enrollment";
                        e.Item.Cells[8].Text = "Resume";
                        e.Item.Cells[9].Text = "Document";
                    }
                    xHeader[0] = e.Item.Cells[1].Text;
                    xHeader[1] = e.Item.Cells[2].Text;
                    xHeader[2] = e.Item.Cells[3].Text;
                    xHeader[3] = e.Item.Cells[4].Text;
                    xHeader[4] = e.Item.Cells[5].Text;
                    xHeader[5] = e.Item.Cells[6].Text;
                    xHeader[6] = e.Item.Cells[7].Text;
                    xHeader[7] = e.Item.Cells[8].Text;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : OnCheckedChanged
        * Purpose       : C1WebGrid의 Header 체크박스 체크 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        #region chkHeader_OnCheckedChanged()
        protected void chkHeader_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox xchk = (CheckBox)sender;

                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked = xchk.Checked;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        #region PageNavigator1_OnPageIndexChanged
        protected void PageNavigator1_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        {
            try
            {
                this.CurrentPageIndex = e.PageIndex;
                this.PageInfo1.CurrentPageIndex = e.PageIndex;
                this.PageNavigator1.CurrentPageIndex = e.PageIndex;
                this.BindGrid();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : BindDropDownList
        * Purpose       : 컨텐츠 목록 데이터를 DropDownList에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region DropDownList
        public void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[1];

                xParams[0] = "0044"; // 사용자 그룹

                DataTable xDtUser = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                     "GetCommonCodeInfo",
                                                     LMS_SYSTEM.MANAGE,
                                                     "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);
                //WebControlHelper.SetDropDownList(this.ddlStatus, xDtUser, 0);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : BindGrid
        * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region BindGrid()
        public void BindGrid()
        {
            try
            {
                string[] xParams = new string[9];

                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = this.txtLecturerNm.Text.Replace("'", "''"); // 회사명
                xParams[3] = this.txtCreated_From.Text.Replace("'", "''"); // 가입일자 From
                xParams[4] = this.txtCreated_To.Text.Replace("'", "''"); // 가입일자 To
                xParams[5] = ddlDelYn.SelectedValue;
                xParams[6] = Session["USER_GROUP"].ToString(); // 로그인 사용자 그룹 
                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_lecturer_md",
                               "GetLecturerList",
                               LMS_SYSTEM.CURRICULUM,
                               "CLT.WEB.UI.LMS.CURR",
                               (object)xParams);

                this.C1WebGrid1.DataSource = xDt;
                this.C1WebGrid1.DataBind();

                if (xDt.Rows.Count < 1)
                {
                    this.PageInfo1.TotalRecordCount = 0;
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageNavigator1.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]);
                }

                if (ddlDelYn.SelectedValue == "Y")
                {
                    btnDelete.Visible = false;
                }
                else
                {
                    btnDelete.Visible = true;
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
        * Function name : Delete()
        * Purpose       : 컨텐츠 목록 데이터 삭제를 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region Delete()
        public void Delete()
        {
            try
            {
                string xRtn = Boolean.FalseString;  // Update 후 결과값 Return
                ArrayList xalChk = new ArrayList(); // 사용자 CheckBox
                string xScriptMsg = string.Empty;
                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                    {
                        string xLecturerID = this.C1WebGrid1.DataKeys[i].ToString();  // 체크한 그리드의 사용자 Key 값 가져오기

                        if (!xalChk.Contains(xLecturerID))
                            xalChk.Add(xLecturerID);
                    }
                }

                if (xalChk.Count == 0)
                {
                    // 체크박스가 선택되지 않았습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "체크박스" }, new string[] { "Check Box" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                string[,] xParams = new string[xalChk.Count, 3];

                int xCount = 0;
                foreach (string xList in xalChk)
                {
                    xParams[xCount, 0] = xList;                                // 체크한 Company ID
                    xParams[xCount, 1] = Session["USER_ID"].ToString();        // 로그인한 user ID
                    xParams[xCount, 2] = "9";                                  // User Status 사용안함
                    xCount++;
                }

                // 사용자 정보는 Delete 하지 않고 Status 를 사용안함(000002) 으로 처리
                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.CURR.vp_c_lecturer_md",
                 "SetLecturerDelete",
                 LMS_SYSTEM.CURRICULUM,
                 "CLT.WEB.UI.LMS.CURR",
                 (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    BindGrid();
                }
                else
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                }

                BindGrid(); // 삭제 후 Data 조회
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : Excel()
        * Purpose       : 엑셀출력
        * Input         : void
        * Output        : void
        *************************************************************/
        #region Excel()
        public void Excel()
        {
            try
            {
                string[] xParams = new string[9];
                
                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = this.txtLecturerNm.Text.Replace("'", "''"); // 회사명
                xParams[3] = this.txtCreated_From.Text.Replace("'", "''"); // 가입일자 From
                xParams[4] = this.txtCreated_To.Text.Replace("'", "''"); // 가입일자 To
                xParams[5] = ddlDelYn.SelectedValue;
                xParams[6] = Session["USER_GROUP"].ToString(); // 로그인 사용자 그룹 


                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_lecturer_md",
                               "GetLecturerList",
                               LMS_SYSTEM.CURRICULUM,
                               "CLT.WEB.UI.LMS.CURR",
                               (object)xParams);
                
                if (xDt.Rows.Count == 0)
                {
                    // 자료가 없습니다!
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A062", new string[] { "자료" }, new string[] { "Data" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                if (xDt.Rows.Count > 0)
                    GetExcelFile(xDt, xHeader);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

    }
}
