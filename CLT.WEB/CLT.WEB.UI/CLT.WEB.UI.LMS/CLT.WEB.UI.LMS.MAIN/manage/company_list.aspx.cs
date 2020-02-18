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

namespace CLT.WEB.UI.LMS.MANAGE
{
    /// <summary>
    /// 1. 작업개요 : 회원사 정보관리 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 회원사 정보관리
    ///				  
    /// 3. Class 명 : company_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    /// 
    public partial class company_list : BasePage
    {
        string[] xHeader = null;

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
                    //this.SetControlAttributes(new object[] { this.txtCompany, this.ddlStatus, this.txtCreated_From, this.txtCreated_To }, this.btnRetrieve, this.Page);
                    this.txtCreated_From.Attributes.Add("onkeyup", "ChkDate(this);");
                    this.txtCreated_To.Attributes.Add("onkeyup", "ChkDate(this);");

                    base.pRender(this.Page, new object[,] { { this.btnNew, "A" },
                                                        { this.btnDelete, "D" },
                                                        { this.btnApploval, "E" },
                                                        { this.btnUseage, "A" },
                                                        { this.btnExcel, "I" },
                                                        { this.btnRetrieve, "I" },
                                                        { this.btnReject, "A" }
                                                      });
                    BindDropDownList();
                    SetGridClear(this.C1WebGrid1, this.PageNavigator1, this.PageInfo1);
                    this.PageInfo1.TotalRecordCount = 0;
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageNavigator1.TotalRecordCount = 0;
                    
                    if (Session["USER_GROUP"].ToString() == "000009")
                    {
                        //return;
                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "company_list", xScriptMsg);
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
        
        /************************************************************
        * Function name : button_Click
        * Purpose       : 버튼클릭 이벤트 (btnRetrieve, btnDelete, btnApploval, btnUseage, btnExcel)
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
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A039", new string[] { "가입기간" }, new string[] { "Period" }, Thread.CurrentThread.CurrentCulture));
                        return;
                    }
                    else if (string.IsNullOrEmpty(txtCreated_From.Text) && !string.IsNullOrEmpty(txtCreated_To.Text))
                    {
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A039", new string[] { "가입기간" }, new string[] { "Period" }, Thread.CurrentThread.CurrentCulture));
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
                else if (btn.ID == "btnApploval") // 승인요청
                {
                    Apploval();
                    PageInfo1.CurrentPageIndex = 1;
                    PageNavigator1.CurrentPageIndex = 1;

                }
                else if (btn.ID == "btnUseage") // 승인
                {
                    Useage();
                    PageInfo1.CurrentPageIndex = 1;
                    PageNavigator1.CurrentPageIndex = 1;

                }
                else if (btn.ID == "btnReject") // 승인취소
                {
                    Reject();
                    PageInfo1.CurrentPageIndex = 1;
                    PageNavigator1.CurrentPageIndex = 1;

                }
                else if (btn.ID == "btnNew") // 신규등록
                {
                    //string xScriptContent = string.Format("<script>openPopWindow('/manage/company_edit.aspx?EDITMODE=NEW&MenuCode={0}', 'company_list', '800', '298');</script>", Session["MENU_CODE"]);
                    //ScriptHelper.ScriptBlock(this, "company_list", xScriptContent);
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
                    xHeader = new string[9];
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[1].Text = "번호";
                        e.Item.Cells[2].Text = "회사명";
                        e.Item.Cells[3].Text = "대표자명";
                        e.Item.Cells[4].Text = "회사규모";
                        e.Item.Cells[5].Text = "담당자명";
                        e.Item.Cells[6].Text = "담당자연락처";
                        e.Item.Cells[7].Text = "근로자수";
                        e.Item.Cells[8].Text = "등록일";
                        e.Item.Cells[9].Text = "승인";
                    }
                    else
                    {
                        e.Item.Cells[1].Text = "No";
                        e.Item.Cells[2].Text = "Company";
                        e.Item.Cells[3].Text = "CEO";
                        e.Item.Cells[4].Text = "Company Size";
                        e.Item.Cells[5].Text = "Person in Charge";
                        e.Item.Cells[6].Text = "Tel";
                        e.Item.Cells[7].Text = "Emp.Count";
                        e.Item.Cells[8].Text = "Date of Enrollment";
                        e.Item.Cells[9].Text = "Status";
                    }
                    xHeader[0] = e.Item.Cells[1].Text;
                    xHeader[1] = e.Item.Cells[2].Text;
                    xHeader[2] = e.Item.Cells[3].Text;
                    xHeader[3] = e.Item.Cells[4].Text;
                    xHeader[4] = e.Item.Cells[5].Text;
                    xHeader[5] = e.Item.Cells[6].Text;
                    xHeader[6] = e.Item.Cells[7].Text;
                    xHeader[7] = e.Item.Cells[8].Text;
                    xHeader[8] = e.Item.Cells[9].Text;
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
                WebControlHelper.SetDropDownList(this.ddlStatus, xDtUser, 0);
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
                xParams[2] = this.txtCompany.Text.Replace("'", "''"); // 회사명
                xParams[3] = this.txtCreated_From.Text.Replace("'", "''"); // 가입기간 From
                xParams[4] = this.txtCreated_To.Text.Replace("'", "''"); // 가입기간 To
                if (this.ddlStatus.SelectedItem.Text != "*")
                    xParams[5] = this.ddlStatus.SelectedItem.Value;  // 승인여부
                else
                    xParams[5] = string.Empty;

                xParams[6] = Session["USER_GROUP"].ToString(); // 로그인 사용자 그룹 
                xParams[7] = Session["COMPANY_ID"].ToString(); // 로그인 사용자가 관리자가 아닐 경우 자기가 소속된 법인사만 조회된다.
                xParams[8] = "Y";
                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_company_md",
                               "GetCompany",
                               LMS_SYSTEM.MANAGE,
                               "CLT.WEB.UI.LMS.MANAGE",
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
                
                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    //if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked == true)
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                    {
                        TableCell xTCC = new TableCell();
                        xTCC = (TableCell)this.C1WebGrid1.Items[i].Cells[9];  // 승인여부 
                        
                        string xCompanyID = this.C1WebGrid1.DataKeys[i].ToString();  // 체크한 그리드의 사용자 Key 값 가져오기                        
                        if (!xalChk.Contains(xCompanyID))
                            xalChk.Add(xCompanyID);

                        if (xTCC.Text == "000002") // 미승인상태일 경우
                        {
                            // A099 {1}를 이미 완료하였음으로 {0}할수 없습니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A099", new string[] { "삭제", "삭제" }, new string[] { "Delete", "Delete" }, Thread.CurrentThread.CurrentCulture));
                            return;
                        }
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
                    xParams[xCount, 2] = "000002";                             // 공통코드 User Status 사용안함
                    xCount++;
                }

                // 사용자 정보는 Delete 하지 않고 Status 를 사용안함(000002) 으로 처리
                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_company_md",
                 "SetCompanyDelete",
                 LMS_SYSTEM.MANAGE,
                 "CLT.WEB.UI.LMS.MANAGE",
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
                xParams[2] = this.txtCompany.Text; // 회사명
                xParams[3] = this.txtCreated_From.Text; // 가입기간 From
                xParams[4] = this.txtCreated_To.Text; // 가입기간 To

                if (this.ddlStatus.SelectedItem.Text != "*")
                    xParams[5] = this.ddlStatus.SelectedItem.Value;  // 승인여부
                else
                    xParams[5] = string.Empty;

                xParams[6] = Session["USER_GROUP"].ToString(); // 로그인 사용자 그룹 
                xParams[7] = Session["COMPANY_ID"].ToString(); // 로그인 사용자가 관리자가 아닐 경우 자기가 소속된 법인사만 조회된다.
                xParams[8] = "N";
                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_company_md",
                               "GetCompany",
                               LMS_SYSTEM.MANAGE,
                               "CLT.WEB.UI.LMS.MANAGE",
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
        
        /************************************************************
        * Function name : Apploval()
        * Purpose       : 승인요청
        * Input         : void
        * Output        : void
        *************************************************************/
        #region Apploval()
        public void Apploval()
        {
            try
            {
                string xRtn = Boolean.FalseString;  // Update 후 결과값 Return
                ArrayList xalChk = new ArrayList(); // 사용자 CheckBox   

                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    //if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked == true)
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                    {
                        TableCell xTCC = new TableCell();
                        xTCC = (TableCell)this.C1WebGrid1.Items[i].Cells[9];  // 승인여부 

                        string xUserID = this.C1WebGrid1.DataKeys[i].ToString();  // 체크한 그리드의 사용자 Key 값 가져오기                        
                        if (xTCC.Text == "000003") // 승인상태일 경우
                        {
                            // A099 {1}를 이미 완료하였음으로 {0}할수 없습니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A099", new string[] { "승인처리를", "승인" }, new string[] { "Apploval Complete", "Apploval" }, Thread.CurrentThread.CurrentCulture));
                            return;
                            //xScriptMsg = string.Format("<script>alert('이미 승인처리된 ID 입니다! 사용자ID : {0}');</script>", xUserID);
                        }
                        else if (xTCC.Text == "000004") // 승인대기 상태일 경우
                        {
                            // A088 결재가 진행중입니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A088", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                            return;
                            //xScriptMsg = string.Format("<script>alert('승인대기 상태의 ID 입니다! 사용자ID : {0}');</script>", xUserID);
                        }

                        xTCC = (TableCell)this.C1WebGrid1.Items[i].Cells[10];
                        if (string.IsNullOrEmpty(this.C1WebGrid1.Items[i].Cells[10].Text)) // 회사구분 체크) // 회사구분 체크 // 회사코드 체크
                        {
                            // 회사코드를 입력해 주세요!
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "회사코드" }, new string[] { "Company Code" }, Thread.CurrentThread.CurrentCulture));
                            return;
                        }
                        else if (this.C1WebGrid1.Items[i].Cells[10].Text == "&nbsp;")
                        {
                            // 회사코드를 입력해 주세요!
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "회사코드" }, new string[] { "Company Code" }, Thread.CurrentThread.CurrentCulture));
                            return;
                        }
                        else if (string.IsNullOrEmpty(this.C1WebGrid1.Items[i].Cells[11].Text)) // 회사구분 체크
                        {
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "회사구분" }, new string[] { "Company Kind" }, Thread.CurrentThread.CurrentCulture));
                            return;
                        }
                        else if (string.IsNullOrEmpty(this.C1WebGrid1.Items[i].Cells[12].Text)) // 우편번호 체크
                        {
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "우편번호" }, new string[] { "Zip Code" }, Thread.CurrentThread.CurrentCulture));
                            return;
                        }
                        else if (string.IsNullOrEmpty(this.C1WebGrid1.Items[i].Cells[13].Text)) // 우편번호 체크
                        {
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "주소" }, new string[] { "Address" }, Thread.CurrentThread.CurrentCulture));
                            return;
                        }
                        if (!xalChk.Contains(xUserID))
                            xalChk.Add(xUserID);
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
                    xParams[xCount, 0] = xList;                                // 체크한 user ID
                    xParams[xCount, 1] = Session["USER_ID"].ToString();        // 로그인한 user ID
                    xParams[xCount, 2] = "000004";                             // 공통코드 User Status 승인대기                    
                    xCount++;
                }

                // 사용자 정보는 Delete 하지 않고 Status 를 사용안함(000002) 으로 처리
                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_company_md",
                 "SetCompanyDelete",
                 LMS_SYSTEM.MANAGE,
                 "CLT.WEB.UI.LMS.MANAGE",
                 (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    BindGrid();
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                }
                BindGrid(); // 승인 후 Data 조회
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : Useage()
        * Purpose       : 승인
        * Input         : void
        * Output        : void
        *************************************************************/
        #region Useage()
        public void Useage()
        {
            try
            {
                string xRtn = Boolean.FalseString;  // Update 후 결과값 Return
                ArrayList xalChk = new ArrayList(); // 사용자 CheckBox   

                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    //if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked == true)
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                    {
                        TableCell xTCC = new TableCell();
                        xTCC = (TableCell)this.C1WebGrid1.Items[i].Cells[9];  // 승인여부 

                        string xUserID = this.C1WebGrid1.DataKeys[i].ToString();  // 체크한 그리드의 사용자 Key 값 가져오기                        
                        if (xTCC.Text != "000004") // 승인대기 상태가 아닐경우
                        {
                            // 승인 대기 상태가 아닙니다!
                            //xScriptMsg = string.Format("<script>alert('승인대기 상태의 ID가 아닙니다! 사용자ID : {0}');</script>", xUserID); 
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A109", new string[] { xUserID }, new string[] { xUserID }, Thread.CurrentThread.CurrentCulture));
                            return;
                        }

                        if (!xalChk.Contains(xUserID))
                            xalChk.Add(xUserID);
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
                    xParams[xCount, 0] = xList;                                // 체크한 user ID
                    xParams[xCount, 1] = Session["USER_ID"].ToString();        // 로그인한 user ID
                    xParams[xCount, 2] = "000003";                             // 공통코드 User Status 승인

                    xCount++;
                }

                // 사용자 정보는 Delete 하지 않고 Status 를 사용안함(000002) 으로 처리
                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_company_md",
                 "SetCompanyDelete",
                 LMS_SYSTEM.MANAGE,
                 "CLT.WEB.UI.LMS.MANAGE",
                 (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    BindGrid();
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                }

                BindGrid(); // 승인 후 Data 조회
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion
        
        /************************************************************
        * Function name : Reject()
        * Purpose       : 승인취소
        * Input         : void
        * Output        : void
        *************************************************************/
        #region Reject()
        public void Reject()
        {
            try
            {
                string xRtn = Boolean.FalseString;  // Update 후 결과값 Return
                ArrayList xalChk = new ArrayList(); // 사용자 CheckBox   

                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    //if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked == true)
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                    {
                        TableCell xTCC = new TableCell();
                        xTCC = (TableCell)this.C1WebGrid1.Items[i].Cells[9];  // 승인여부 

                        string xUserID = this.C1WebGrid1.DataKeys[i].ToString();  // 체크한 그리드의 사용자 Key 값 가져오기                        
                        if (xTCC.Text == "000004" || xTCC.Text == "000003") // 승인대기 상태가 아닐경우
                        {
                            if (!xalChk.Contains(xUserID))
                                xalChk.Add(xUserID);
                        }
                        else
                        {
                            //xScriptMsg = string.Format("<script>alert('승인 또는 승인 대기 상태의 ID가 아닙니다! 사용자ID : {0}');</script>", xUserID); 
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A117", new string[] { xUserID }, new string[] { xUserID }, Thread.CurrentThread.CurrentCulture));
                            return;
                        }
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
                    xParams[xCount, 0] = xList;                                // 체크한 user ID
                    xParams[xCount, 1] = Session["USER_ID"].ToString();        // 로그인한 user ID
                    xParams[xCount, 2] = "000001";                             // 공통코드 User Status 미승인                    
                    xCount++;
                }

                // 사용자 정보는 Delete 하지 않고 Status 를 사용안함(000002) 으로 처리
                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_company_md",
                 "SetCompanyDelete",
                 LMS_SYSTEM.MANAGE,
                 "CLT.WEB.UI.LMS.MANAGE",
                 (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    BindGrid();
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                }

                BindGrid(); // 승인 후 Data 조회
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
