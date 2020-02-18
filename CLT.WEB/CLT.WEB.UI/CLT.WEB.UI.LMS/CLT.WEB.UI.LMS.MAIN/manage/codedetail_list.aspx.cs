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
    /// 1. 작업개요 : (Master)공통코드 조회 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 Master 공통코드 관리
    ///				  
    /// 3. Class 명 : codedetail_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    public partial class codedetail_list : BasePage
    {
        string[] xHeader = null;

        /************************************************************
        * Function name : Page_Load 
        * Purpose       : 웹페이지 Load 이벤트
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
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list", xScriptMsg);

                    return;
                }

                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                // 버튼 권한 설정
                base.pRender(this.Page,
                             new object[,] { {this.btnExcel, "I"},
                                             {this.btnRetrieve, "I"},
                                             {this.btnSave, "E"},
                                             {this.btnNew, "E"}
                                           },
                             Convert.ToString(Request.QueryString["MenuCode"]));

                if (Request.QueryString["openerm_cd"] != null)
                {
                    if (!IsPostBack)  // 페이지 최초 로드여부
                    {
                        
                        //Enter Key 입력시 Search 되도록 설정
                        //this.txtM_CD.Attributes.Add("onkeypress", "if(event.keyCode == 13) { " + Page.ClientScript.GetPostBackEventReference(btnRetrieve, "") + "; return false;}");
                        //this.txtM_DESC.Attributes.Add("onkeypress", "if(event.keyCode == 13) { " + Page.ClientScript.GetPostBackEventReference(btnRetrieve, "") + "; return false;}");
                        //this.txtM_ENM.Attributes.Add("onkeypress", "if(event.keyCode == 13) { " + Page.ClientScript.GetPostBackEventReference(btnRetrieve, "") + "; return false;}");
                        //this.txtM_KNM.Attributes.Add("onkeypress", "if(event.keyCode == 13) { " + Page.ClientScript.GetPostBackEventReference(btnRetrieve, "") + "; return false;}");


                        SetGridClear(this.C1WebGrid1, this.PageNavigator1, this.PageInfo1);
                        
                        if (Session["USER_GROUP"].ToString() != "000009")
                            BindGrid(Request.QueryString["openerm_cd"]);
                    }
                }
                else
                {
                    //string xScriptContent = "<script>alert('잘못된 경로를 통해 접근하였습니다.');self.close();</script>";
                    //ScriptHelper.ScriptBlock(this, "codedetail_list", xScriptContent);
                    return;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        #region 버튼클릭 이벤트
        /************************************************************
        * Function name : btnRetrieve_Click
        * Purpose       : 조회버튼 클릭 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid(Request.QueryString["openerm_cd"]); // Data 조회 메서드 검색조건은 메서드 안에 포함
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : btnClear_OnClick
        * Purpose       : 공자시항 초기화 Click 이벤트

        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnClear_OnClick(object sender, EventArgs e)
        protected void btnClear_OnClick(object sender, EventArgs e)
        {
            try
            {
                txtM_CD.Text = txtM_DESC.Text = txtM_ENM.Text = txtM_KNM.Text = string.Empty;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        /************************************************************
        * Function name : btnNew_OnClick
        * Purpose       : 신규버튼 클릭 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        //protected void btnNew_OnClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string xScriptContent = "<script>openPopWindow('/manage/codedetail_edit.aspx?openerEditMode=d_cd_new&openerm_cd=" + Request.QueryString["openerm_cd"].ToString() + "','codemaster_list', '580', '440');</script>";
        //        ScriptHelper.ScriptBlock(this, "codedetail_list", xScriptContent);
        //    }
        //    catch (Exception ex)
        //    {
        //        base.NotifyError(ex);
        //    }
        //}


        /************************************************************
        * Function name : btnSave_Click
        * Purpose       : 저장버튼 클릭 이벤트 핸들러 (사용여부 Update용)
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string xRtn = Boolean.FalseString;
                string xScriptMsg = string.Empty;
                ArrayList xalChk = new ArrayList();



                //xalChk = (ArrayList)ViewState["chkUse"];

                int j = 0;
                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    //if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked == true)
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                        j++;
                }

                string[,] xParams = new string[j, 4];

                int xCount = 0;


                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    //if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked == true)
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked)
                    {
                        xParams[xCount, 0] = this.C1WebGrid1.DataKeys[i].ToString(); // MasterCode Key 값(m_cd)

                        if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkuse_yn")).Checked == true)
                            xParams[xCount, 1] = "Y";  // use_yn 값

                        else
                            xParams[xCount, 1] = "N";  // use_yn 값


                        xParams[xCount, 2] = Session["USER_ID"].ToString();  // Update 한 사용자 ID
                        xParams[xCount, 3] = Request.QueryString["openerm_cd"].ToString(); // Update 할 Master Code

                        if (!xalChk.Contains(this.C1WebGrid1.DataKeys[i].ToString()))
                            xalChk.Add(this.C1WebGrid1.DataKeys[i].ToString());
                        xCount++;
                    }
                    

                    //if (xalChk.Contains(this.C1WebGrid1.DataKeys[i].ToString()))
                    //{
                    //    xParams[xCount, 0] = this.C1WebGrid1.DataKeys[i].ToString(); // MasterCode Key 값(m_cd)

                    //    if (((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkuse_yn")).Checked == true)
                    //        xParams[xCount, 1] = "Y";  // use_yn 값

                    //    else
                    //        xParams[xCount, 1] = "N";  // use_yn 값


                    //    xParams[xCount, 2] = Session["USER_ID"].ToString();  // Update 한 사용자 ID
                    //    xParams[xCount, 3] = Request.QueryString["openerm_cd"].ToString(); // Update 할 Master Code
                    //}
                }

                if (xalChk.Count == 0)
                {
                    // 체크박스가 선택되지 않았습니다.
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003", new string[] { "체크박스" }, new string[] { "Check Box" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_detailcode_md",
                                         "SetCodeDetailEditUse_YN",
                                         LMS_SYSTEM.MANAGE,
                                         "CLT.WEB.UI.LMS.MANAGE",
                                         (object)xParams);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리 완료되었습니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));

                    int xTotalCount = this.PageInfo1.TotalRecordCount;
                    int xPageCount = this.PageInfo1.PageSize;
                    int xNavigatorCount = this.PageNavigator1.TotalRecordCount;


                    BindGrid(Request.QueryString["openerm_cd"]);
                    //this.PageInfo1.TotalRecordCount = xTotalCount;
                    //this.PageInfo1.PageSize = xPageCount;
                    //this.PageNavigator1.TotalRecordCount = xNavigatorCount;
                }
                else
                {
                    //xScriptMsg = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A103", new string[] { "" }, new string[] { "" }, Thread.CurrentThread.CurrentCulture));
                }

                // Save 하고나서 ViewStatus 초기화!!!
                ViewState["chkEdit"] = null;

                BindGrid(Request.QueryString["openerm_cd"].ToString()); // Update 후 Data 조회
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : btnExcel_Click
        * Purpose       : 엑셀버튼 클릭 이벤트 핸들러 
        *                 (조회한 자료 엑셀 출력용)
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string[] xParams = new string[8];
                DataTable xDt = null;

                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = string.Empty;
                xParams[3] = string.Empty;
                xParams[4] = string.Empty;
                xParams[5] = string.Empty;
                xParams[6] = Request.QueryString["openerm_cd"];
                xParams[7] = "EXCEL";


                if (!string.IsNullOrEmpty(this.txtM_CD.Text))
                    xParams[2] = this.txtM_CD.Text.Replace("'", "''");  // Master Code

                if (!string.IsNullOrEmpty(this.txtM_DESC.Text))
                    xParams[3] = this.txtM_DESC.Text.Replace("'", "''"); // Master Code 명


                if (!string.IsNullOrEmpty(this.txtM_ENM.Text))
                    xParams[4] = this.txtM_ENM.Text.Replace("'", "''"); // Master Code 영문명


                if (!string.IsNullOrEmpty(this.txtM_KNM.Text))
                    xParams[5] = this.txtM_KNM.Text.Replace("'", "''");  // Master Code 한글명

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_detailcode_md",
                                       "GetDetailCode",
                                       LMS_SYSTEM.MANAGE,
                                       "CLT.WEB.UI.LMS.MANAGE", (object)xParams);

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
        #endregion 버튼클릭 이벤트

        #region 그리드 이벤트

        /************************************************************
        * Function name : C1WebGrid1_ItemDataBound
        * Purpose       : Grid Data Bound 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/  
        protected void C1WebGrid1_ItemDataBound(object sender, C1ItemEventArgs e)
        {
            try
            {
                // 체크박스 변수 초기화
                bool xchkUse_yn = false;
                string xSent_flg = "No";


                DataRowView xItem = (DataRowView)e.Item.DataItem;

                if (xItem != null)
                {
                    if (xItem["use_yn"].ToString().ToUpper() == "Y") // 사용여부 Data가 Y 이면 체크
                        xchkUse_yn = true;
                    else
                        xchkUse_yn = false;

                    if (!string.IsNullOrEmpty(xItem["send_flg"].ToString())) // // 사용여부 Data가 N 이면 체크해제
                    {
                        if (xItem["send_flg"].ToString() == "2")
                            xSent_flg = "Yes";
                        else
                            xSent_flg = "No";
                    }
                }

                if (e.Item.ItemType == C1ListItemType.Item || e.Item.ItemType == C1ListItemType.AlternatingItem)
                {
                    //((CheckBox)e.Item.FindControl("chkuse_yn")).Checked = xchkUse_yn;  // 사용여부 CheckBox
                    ((HtmlInputCheckBox)e.Item.FindControl("chkuse_yn")).Checked = xchkUse_yn;  // 사용여부 CheckBox
                    ((Label)e.Item.FindControl("lblsent_yn")).Text = xSent_flg;  // 전송여부 Label
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }


        /************************************************************
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grd_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    xHeader = new string[7];

                    if (this.IsSettingKorean())
                    {
                        //e.Item.Cells[0].Text = "순번";
                        e.Item.Cells[1].Text = "코드";
                        e.Item.Cells[2].Text = "코드명";
                        e.Item.Cells[3].Text = "영문명";
                        e.Item.Cells[4].Text = "한글명";
                        e.Item.Cells[5].Text = "사용유무";
                        e.Item.Cells[6].Text = "전송유무";
                        e.Item.Cells[7].Text = "선박전송일시";
                    }
                    else
                    {
                        //e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "Code";
                        e.Item.Cells[2].Text = "Code Name";
                        e.Item.Cells[3].Text = "Eng Name";
                        e.Item.Cells[4].Text = "Kor Name";
                        e.Item.Cells[5].Text = "Status";
                        e.Item.Cells[6].Text = "Send";
                        e.Item.Cells[7].Text = "Date of transmission";
                    }

                    xHeader[0] = e.Item.Cells[1].Text;
                    xHeader[1] = e.Item.Cells[2].Text;
                    xHeader[2] = e.Item.Cells[3].Text;
                    xHeader[3] = e.Item.Cells[4].Text;
                    xHeader[4] = e.Item.Cells[5].Text;
                    xHeader[5] = e.Item.Cells[6].Text;
                    xHeader[6] = e.Item.Cells[7].Text;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }


        /************************************************************
        * Function name : chkHeader_OnCheckedChanged
        * Purpose       : Grid 내 Header 체크박스 체크 이벤트

        * Input         : void
        * Output        : void
        *************************************************************/
        protected void chkHeader_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox xchk = (CheckBox)sender;

                // 사용자가 Header 체크박스 선택시 
                // 해당 체크박스 Row에 있는 체크박스 일괄 적용
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


        /************************************************************
        * Function name : chkuse_yn_OnCheckedChanged
        * Purpose       : Grid 내 사용유무 체크박스 체크 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void chkuse_yn_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkTemp = (CheckBox)sender;

                ArrayList xalchk = new ArrayList();

                if (ViewState["chkUse"] != null)
                    xalchk = (ArrayList)ViewState["chkUse"];  // User가 체크한 체크박스가 존재 할 경우

                if (chkTemp.Checked == true)
                {
                    if (!xalchk.Contains(chkTemp.Text + "|" + chkTemp.Checked.ToString()))
                        xalchk.Add(chkTemp.Text + "|" + chkTemp.Checked.ToString());
                }
                else
                {
                    if (xalchk.Contains(chkTemp.Text + "|" + "True"))
                        xalchk.Remove(chkTemp.Text + "|" + chkTemp.Checked.ToString());
                }

                ViewState["chkUse"] = xalchk;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void PageNavigator1_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
        {
            try
            {
                this.CurrentPageIndex = e.PageIndex;
                this.PageInfo1.CurrentPageIndex = e.PageIndex;
                this.PageNavigator1.CurrentPageIndex = e.PageIndex;
                BindGrid(Request.QueryString["openerm_cd"]);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        #endregion

        #region 메서드

        /************************************************************
        * Function name : BindGrid
        * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        public void BindGrid(string rMasterCode)
        {
            try
            {
                string[] xParams = new string[8];
                DataTable xDt = null;

                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = string.Empty;
                xParams[3] = string.Empty;
                xParams[4] = string.Empty;
                xParams[5] = string.Empty;
                xParams[6] = rMasterCode;
                xParams[7] = "GRID";

                if (!string.IsNullOrEmpty(this.txtM_CD.Text))
                    xParams[2] = this.txtM_CD.Text.Replace("'", "''");  // Master Code

                if (!string.IsNullOrEmpty(this.txtM_DESC.Text))
                    xParams[3] = this.txtM_DESC.Text.Replace("'", "''"); // Master Code 명


                if (!string.IsNullOrEmpty(this.txtM_ENM.Text))
                    xParams[4] = this.txtM_ENM.Text.Replace("'", "''"); // Master Code 영문명


                if (!string.IsNullOrEmpty(this.txtM_KNM.Text))
                    xParams[5] = this.txtM_KNM.Text.Replace("'", "''");  // Master Code 한글명


                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_detailcode_md",
                                       "GetDetailCode",
                                       LMS_SYSTEM.MANAGE,
                                       "CLT.WEB.UI.LMS.MANAGE", (object)xParams);

                C1WebGrid1.DataSource = xDt;
                C1WebGrid1.DataBind();

                // 페이지 처리
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

        #endregion 메서드
    }
}
