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
    /// 1. 작업개요 : 사용자 정보관리 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 사용자 정보관리
    ///				  
    /// 3. Class 명 : user_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    /// 
    public partial class user_list : BasePage
    {
        string[] xHeader = null;

        /************************************************************
        * Function name : Page_Load
        * Purpose       : Page_Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                if (!IsPostBack)
                {
                    //Enter Key 입력시 Search 되도록 설정
                    //this.SetControlAttributes(new object[] { txtCompany, ddlDutyStep, ddlUserGroup, txtUserName, txtPersonal_No, ddlStatus }, this.btnRetrieve, this.Page);
                    //this.txtCompany.Attributes.Add("onkeypress", "if(event.keyCode == 13) { " + Page.ClientScript.GetPostBackEventReference(btnRetrieve, "") + "; return false;}");
                    //this.ddlDutyStep.Attributes.Add("onkeypress", "if(event.keyCode == 13) { " + Page.ClientScript.GetPostBackEventReference(btnRetrieve, "") + "; return false;}");
                    //this.ddlUserGroup.Attributes.Add("onkeypress", "if(event.keyCode == 13) { " + Page.ClientScript.GetPostBackEventReference(btnRetrieve, "") + "; return false;}");
                    //this.txtUserName.Attributes.Add("onkeypress", "if(event.keyCode == 13) { " + Page.ClientScript.GetPostBackEventReference(btnRetrieve, "") + "; return false;}");
                    //this.txtPersonal_No.Attributes.Add("onkeypress", "if(event.keyCode == 13) { " + Page.ClientScript.GetPostBackEventReference(btnRetrieve, "") + "; return false;}");
                    //this.ddlStatus.Attributes.Add("onkeypress", "if(event.keyCode == 13) { " + Page.ClientScript.GetPostBackEventReference(btnRetrieve, "") + "; return false;}");

                    ViewState["UserIDInfo"] = Session["USER_ID"].ToString();  // 로그인한 사용자 ID를 가져 온다.
                    BindDropDownList();

                    string[] xParams = new string[1];
                    xParams[0] = "admin";// 임시로 지정...  //Session["USER_ID"].ToString();

                    // 로그인 정보 ID 만 가져 올 수 있는 관계로 임시로 사용...
                    DataTable xDtUser = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                       "GetUser",
                                       LMS_SYSTEM.MANAGE,
                                       "CLT.WEB.UI.LMS.MANAGE",
                                       (object)xParams);

                    if (xDtUser.Rows.Count == 0)
                        return;

                    base.pRender(this.Page, new object[,] { { this.btnDelete, "D" },
                                                    { this.btnApploval, "E" },
                                                    { this.btnUseage, "A" },
                                                    { this.btnExcel, "I" },
                                                    { this.btnRetrieve, "I" }
                                                  });

                    SetGridClear(this.C1WebGrid1, this.PageNavigator1, this.PageInfo1);
                    this.PageInfo1.TotalRecordCount = 0;
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageNavigator1.TotalRecordCount = 0;
                    
                    if (Session["USER_GROUP"].ToString() == "000009")
                    {
                        //return;
                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "user_list", xScriptMsg);
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
        * Function name : OnPreRender
        * Purpose       : OnPreRender 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region OnPreRender()
        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                base.OnPreRender(e);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnRetrieve_Click
        * Purpose       : 사용자 정보 조회 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnRetrieve_Click()
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == "000001")
                {
                    string[] xParams = new string[1];

                    string xRtn = Boolean.TrueString;
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                 "SetUserUpdateSSO",
                                 LMS_SYSTEM.MAIN,
                                 "CLT.WEB.UI.LMS.MAIN",
                                 (object)xParams);
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnNew_OnClick
        * Purpose       : 신규등록 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        //#region protected void btnNew_OnClick(object sender, EventArgs e)
        //protected void btnNew_OnClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string xScriptContent = string.Format("<script>openPopWindow('/manage/user_edit.aspx?EDITMODE=NEW&MenuCode={0}', 'user_edit', '1280', '721');</script>", Session["MENU_CODE"]);
        //        ScriptHelper.ScriptBlock(this, "user_edit", xScriptContent);
        //        //
        //    }
        //    catch (Exception ex)
        //    {
        //        base.NotifyError(ex);
        //    }
        //}
        //#endregion

        /************************************************************
        * Function name : btnDelete_Click
        * Purpose       : 사용자 정보 삭제
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnDelete_Click()
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string xRtn = Boolean.FalseString;  // Update 후 결과값 Return
                string xScriptMsg = string.Empty;   // Message 출력
                ArrayList xalChk = new ArrayList(); // 사용자 CheckBox   

                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    //if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked == true)
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                    {
                        string xUserID = this.C1WebGrid1.DataKeys[i].ToString();  // 체크한 그리드의 사용자 Key 값 가져오기

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
                    xParams[xCount, 2] = "000002";                             // 공통코드 User Status 사용안함
                    xCount++;
                }

                // 사용자 정보는 Delete 하지 않고 Status 를 사용안함(000002) 으로 처리
                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                 "SetUserDelete",
                 LMS_SYSTEM.MANAGE,
                 "CLT.WEB.UI.LMS.MANAGE",
                 (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    BindGrid();
                    PageInfo1.CurrentPageIndex = 1;
                    PageNavigator1.CurrentPageIndex = 1;
                }
                else
                {
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                }

                BindGrid(); // 삭제 후 Data 조회
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnApploval_Click
        * Purpose       : 선택된 회원 승인 요청
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnApploval_Click()
        protected void btnApploval_Click(object sender, EventArgs e)
        {
            try
            {
                string xRtn = Boolean.FalseString;  // Update 후 결과값 Return
                string xScriptMsg = string.Empty;   // Message 출력
                ArrayList xalChk = new ArrayList(); // 사용자 CheckBox   

                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    //if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked == true)
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                    {
                        TableCell xTCC = new TableCell();
                        xTCC = (TableCell)this.C1WebGrid1.Items[i].Cells[11];  // 승인여부 

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
                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                 "SetUserDelete",
                 LMS_SYSTEM.MANAGE,
                 "CLT.WEB.UI.LMS.MANAGE",
                 (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    BindGrid();
                    PageInfo1.CurrentPageIndex = 1;
                    PageNavigator1.CurrentPageIndex = 1;
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
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnUseage_Click
        * Purpose       : 선택된 회원 승인 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnUseage_Click()
        protected void btnUseage_Click(object sender, EventArgs e)
        {
            try
            {
                string xRtn = Boolean.FalseString;  // Update 후 결과값 Return
                string xScriptMsg = string.Empty;   // Message 출력
                ArrayList xalChk = new ArrayList(); // 사용자 CheckBox   

                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    //if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked == true)
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                    {
                        TableCell xTCC = new TableCell();
                        xTCC = (TableCell)this.C1WebGrid1.Items[i].Cells[13];  // 승인여부 

                        string xUserID = this.C1WebGrid1.DataKeys[i].ToString();  // 체크한 그리드의 사용자 Key 값 가져오기                        
                        if (xTCC.Text != "000004") // 승인대기 상태가 아닐경우
                        {
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
                    xParams[xCount, 0] = xList;                                // 체크한 Company ID
                    xParams[xCount, 1] = Session["USER_ID"].ToString();        // 로그인한 user ID
                    xParams[xCount, 2] = "000003";                             // 공통코드 User Status 승인대기                    
                    xCount++;
                }

                // 사용자 정보는 Delete 하지 않고 Status 를 사용안함(000002) 으로 처리
                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                 "SetUserDelete",
                 LMS_SYSTEM.MANAGE,
                 "CLT.WEB.UI.LMS.MANAGE",
                 (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    BindGrid();
                    PageInfo1.CurrentPageIndex = 1;
                    PageNavigator1.CurrentPageIndex = 1;
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                BindGrid(); // 승인 후 Data 조회
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnReject_Click
        * Purpose       : 선택된 회원 미승인 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnReject_Click(object sender, EventArgs e)
        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                string xRtn = Boolean.FalseString;  // Update 후 결과값 Return
                string xScriptMsg = string.Empty;   // Message 출력
                ArrayList xalChk = new ArrayList(); // 사용자 CheckBox   

                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    //if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked == true)
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                    {
                        TableCell xTCC = new TableCell();
                        xTCC = (TableCell)this.C1WebGrid1.Items[i].Cells[11];  // 승인여부 

                        string xUserID = this.C1WebGrid1.DataKeys[i].ToString();  // 체크한 그리드의 사용자 Key 값 가져오기                        
                        if (xTCC.Text == "000004" || xTCC.Text == "000003") // 승인대기 상태가 아닐경우
                        {
                            if (!xalChk.Contains(xUserID))
                                xalChk.Add(xUserID);
                        }
                        else
                        {
                            //xScriptMsg = string.Format("<script>alert('승인 또는 승인 대기 상태 가 아닙니다!);</script>", xUserID); 
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A117", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
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
                    xParams[xCount, 2] = "000001";                             // 공통코드 User Status 승인대기                    
                    xCount++;
                }

                // 사용자 정보는 Delete 하지 않고 Status 를 사용안함(000002) 으로 처리
                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                 "SetUserDelete",
                 LMS_SYSTEM.MANAGE,
                 "CLT.WEB.UI.LMS.MANAGE",
                 (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                    BindGrid();
                    PageInfo1.CurrentPageIndex = 1;
                    PageNavigator1.CurrentPageIndex = 1;
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                BindGrid(); // 승인 후 Data 조회
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnExcel_Click
        * Purpose       : 조회한 Data Ecxel 출력
        * Input         : void
        * Output        : void
        *************************************************************/
        #region btnExcel_Click()
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string[] xParams = new string[14];
                DataTable xDt = null;

                xParams[0] = "0";
                xParams[1] = "0";
                xParams[2] = string.Empty;  // 회사명
                xParams[3] = string.Empty;  // 직급
                xParams[4] = string.Empty;  // 신분
                xParams[5] = string.Empty;  // 입사일자
                xParams[6] = string.Empty;  // 한글성명
                xParams[7] = string.Empty;  // 사용자 그룹
                xParams[8] = string.Empty;  // 주민번호
                xParams[9] = string.Empty;  // 상태
                xParams[10] = string.Empty; // 조회하는 로그인된 사용자그룹 *중요한 정보!!!
                xParams[11] = string.Empty; // 조회하는 로그인된 사용자의 업체ID(Company_ID) *중요한 정보!!!
                xParams[12] = ViewState["UserIDInfo"].ToString(); // 조회하는 로그인된 사용자의 ID
                xParams[13] = "N";

                string[] xParamUser = new string[1];
                xParamUser[0] = ViewState["UserIDInfo"].ToString();

                DataTable xDtUser = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                                   "GetUser",
                                                   LMS_SYSTEM.MANAGE,
                                                   "CLT.WEB.UI.LMS.MANAGE",
                                                   (object)xParamUser);
                if (xDtUser.Rows.Count == 0)
                    return;

                DataRow xDrv = xDtUser.Rows[0];
                xParams[10] = xDrv["user_group"].ToString();
                xParams[11] = xDrv["company_id"].ToString();

                if (!string.IsNullOrEmpty(txtCompany.Text))
                    xParams[2] = txtCompany.Text;

                if (ddlDutyStep.SelectedValue != "*")
                    xParams[3] = ddlDutyStep.SelectedValue;

                if (!string.IsNullOrEmpty(txtUserName.Text))
                    xParams[6] = txtUserName.Text;

                if (ddlUserGroup.SelectedValue != "*")
                    xParams[7] = ddlUserGroup.SelectedValue;

                if (!string.IsNullOrEmpty(txtPersonal_No.Text))
                    xParams[8] = txtPersonal_No.Text;
                
                if (ddlStatus.SelectedItem.Text != "*")
                    xParams[9] = ddlStatus.SelectedItem.Value;

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                       "GetUserInfo",
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
                    xHeader = new string[13];
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[1].Text = "ID";
                        e.Item.Cells[2].Text = "성명";
                        e.Item.Cells[3].Text = "주민번호";
                        e.Item.Cells[4].Text = "사용자그룹";
                        e.Item.Cells[5].Text = "회사명";
                        e.Item.Cells[6].Text = "부서명";
                        e.Item.Cells[7].Text = "직급";
                        e.Item.Cells[8].Text = "휴대폰";
                        e.Item.Cells[9].Text = "이메일";
                        e.Item.Cells[10].Text = "수강횟수";
                        e.Item.Cells[11].Text = "등록일";
                        e.Item.Cells[12].Text = "상태";
                    }
                    else
                    {
                        e.Item.Cells[1].Text = "ID";
                        e.Item.Cells[2].Text = "Name";
                        e.Item.Cells[3].Text = "Registration";
                        e.Item.Cells[4].Text = "User Group";
                        e.Item.Cells[5].Text = "Company";
                        e.Item.Cells[6].Text = "Dept.";
                        e.Item.Cells[7].Text = "Grade";
                        e.Item.Cells[8].Text = "Mobile";
                        e.Item.Cells[9].Text = "E-mail";
                        e.Item.Cells[10].Text = "Course";
                        e.Item.Cells[11].Text = "Regist Date ";
                        e.Item.Cells[12].Text = "Status";
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
                    xHeader[9] = e.Item.Cells[10].Text;
                    xHeader[10] = e.Item.Cells[11].Text;
                    xHeader[11] = e.Item.Cells[12].Text;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : C1WebGrid1_OnItemDataBound
        * Purpose       : C1WebGrid의 Item이 Bound 될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void grd_OnItemDataBound(object seder, EventArgs e)
        protected void grd_OnItemDataBound(object seder, C1ItemEventArgs e)
        {
            try
            {
                string xUserGroup = Session["USER_GROUP"].ToString();
                string xResult = e.Item.Cells[3].Text;

                if (xResult == "&nbsp;")
                    xResult = string.Empty;

                // 관리자, 교관, 해상인사담당, 법인사 관리자는 주민번호 모두 표시 나머지는 부분표시
                if (!string.IsNullOrEmpty(xResult))
                {
                    if (xUserGroup == "000001" || xUserGroup == "000003" || xUserGroup == "000004" || xUserGroup == "000007")
                        e.Item.Cells[3].Text = xResult;
                    else
                    {
                        if (xResult.Length > 12)
                            e.Item.Cells[3].Text = xResult.Substring(0, 7) + "●●●●●●●";
                        else
                            e.Item.Cells[3].Text = string.Empty;
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
                this.C1WebGrid1.DataBind();

                string[] xParams = new string[14];
                DataTable xDt = null;

                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = string.Empty;  // 회사명
                xParams[3] = string.Empty;  // 직급
                xParams[4] = string.Empty;  // 신분
                xParams[5] = string.Empty;  // 입사일자
                xParams[6] = string.Empty;  // 한글성명
                xParams[7] = string.Empty;  // 사용자 그룹
                xParams[8] = string.Empty;  // 주민번호
                xParams[9] = string.Empty;  // 상태
                xParams[10] = Session["USER_GROUP"].ToString(); //string.Empty; // 조회하는 로그인된 사용자그룹 *중요한 정보!!!
                xParams[11] = Session["COMPANY_ID"].ToString(); //string.Empty; // 조회하는 로그인된 사용자의 업체ID(Company_ID) *중요한 정보!!!
                xParams[12] = Session["USER_ID"].ToString(); //ViewState["UserIDInfo"].ToString(); // 조회하는 로그인된 사용자의 ID
                xParams[13] = "Y";

                string[] xParamUser = new string[1];
                xParamUser[0] = ViewState["UserIDInfo"].ToString();

                //DataTable xDtUser = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                //                                   "GetUser",
                //                                   LMS_SYSTEM.MANAGE,
                //                                   "CLT.WEB.UI.LMS.MANAGE.vp_m_manage_user_list_wpg",
                //                                   (object)xParamUser);
                //if (xDtUser.Rows.Count == 0)
                //    return;
                //DataRow xDrv = xDtUser.Rows[0];
                //xParams[10] = xDrv["user_group"].ToString();
                //xParams[11] = xDrv["company_id"].ToString();

                if (!string.IsNullOrEmpty(txtCompany.Text))
                    xParams[2] = txtCompany.Text.Replace("'", "''");

                if (ddlDutyStep.SelectedValue != "*")
                    xParams[3] = ddlDutyStep.SelectedValue;

                if (!string.IsNullOrEmpty(txtUserName.Text))
                    xParams[6] = txtUserName.Text.Replace("'", "''");

                if (ddlUserGroup.SelectedValue != "*")
                    xParams[7] = ddlUserGroup.SelectedValue;

                if (!string.IsNullOrEmpty(txtPersonal_No.Text))
                    xParams[8] = txtPersonal_No.Text.Replace("'", "''");

                if (ddlStatus.SelectedItem.Text != "*")
                    xParams[9] = ddlStatus.SelectedItem.Value;

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_user_md",
                                       "GetUserInfo",
                                       LMS_SYSTEM.MANAGE,
                                       "CLT.WEB.UI.LMS.MANAGE",
                                       (object)xParams);

                C1WebGrid1.DataSource = xDt;
                C1WebGrid1.DataBind();

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
        * Function name : BindDropDownList
        * Purpose       : 공통코드 Data처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region BindDropDownList()
        public void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[2];

                xParams[0] = "0041"; // 사용자 그룹
                xParams[1] = "Y";
                DataTable xDtUser = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                     "GetCommonCodeInfo",
                                                     LMS_SYSTEM.MANAGE,
                                                     "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlUserGroup, xDtUser, 0);
                
                // 직책(직급)코드 Dutystep
                xParams[1] = "Y";

                DataTable xDtDutyStep = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                 "GetDutystepCodeInfo",
                                 LMS_SYSTEM.MANAGE,
                                 "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlDutyStep, xDtDutyStep, "step_name", "duty_step");

                xParams[0] = "0044"; // 상태
                xParams[1] = "Y";
                DataTable xDtStatus = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                     "GetCommonCodeInfo",
                                                     LMS_SYSTEM.MANAGE,
                                                     "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);
                WebControlHelper.SetDropDownList(this.ddlStatus, xDtStatus, 0);
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
