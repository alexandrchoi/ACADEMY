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

namespace CLT.WEB.UI.LMS.APPLICATION
{
    public partial class agreement_list : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "agreement_list", xScriptMsg);
                    return;
                }

                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID;

                if (!IsPostBack)
                {
                    this.txtBeginDt.Attributes.Add("onkeyup", "ChkDate(this);");
                    this.txtEndDt.Attributes.Add("onkeyup", "ChkDate(this);");

                    if (Session["USER_GROUP"].ToString() != "000009")
                    {
                        this.BindGrid();
                    }
                    else
                    {
                        //this.BindGrid();
                        base.SetGridClear(this.grd, this.PageNavigator1);
                        this.PageInfo1.TotalRecordCount = 0;
                        this.PageInfo1.PageSize = this.PageSize;
                        this.PageNavigator1.TotalRecordCount = 0;
                    }
                }
                base.pRender(this.Page, new object[,] { { this.btnRetrieve, "I" } });
            }
            catch (Exception ex)
            {
                base.NotifyError(ex); 
            }
        }

        /************************************************************
        * Function name : BindGrid
        * Purpose       : 컨텐츠 목록 데이터를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region private void BindGrid()
        private void BindGrid()
        {
            try
            {
                string[] xParams = null;
                DataTable xDt = null;

                //if (string.IsNullOrEmpty(this.txtContentsNM.Text) && string.IsNullOrEmpty(this.txtRemark.Text) && this.ddlContentsLang.SelectedItem.Text == "*" && this.ddlContentsType.SelectedItem.Text == "*")            
                // 조회조건이 있을 경우 처리
                xParams = new string[6];
                xParams[0] = this.PageSize.ToString(); // pagesize
                xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                xParams[2] = this.txtBeginDt.Text;
                xParams[3] = this.txtEndDt.Text;
                xParams[4] = this.txtCourseNm.Text; //과정명 
                xParams[5] = Session["user_id"].ToString();
                
                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.APPLICATION.vp_s_agreement_md",
                                             "GetAgreementList",
                                             LMS_SYSTEM.APPLICATION,
                                             "CLT.WEB.UI.LMS.APPLICATION", (object)xParams);
                
                this.grd.DataSource = xDt;
                this.grd.DataBind();

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
        * Function name : btnRetrieve_Click
        * Purpose       : 조회 조건에 대한 결과를 조회하여 리스트로 출력하는 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnRetrieve_Click(object sender, EventArgs e)
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                this.CurrentPageIndex = 1;
                this.PageInfo1.CurrentPageIndex = 1;
                this.PageNavigator1.CurrentPageIndex = 1;
                this.BindGrid();
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
        protected void grd_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "open_course_id";
                        e.Item.Cells[1].Text = "";
                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "과정명";
                        e.Item.Cells[4].Text = "차수";
                        e.Item.Cells[5].Text = "교육기간";
                        e.Item.Cells[6].Text = "인원";
                        e.Item.Cells[7].Text = "훈련위탁계약서";
                        e.Item.Cells[8].Text = "채용예정약정서";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "open_course_id";
                        e.Item.Cells[1].Text = "";
                        e.Item.Cells[2].Text = "No.";
                        e.Item.Cells[3].Text = "Course Name";
                        e.Item.Cells[4].Text = "Seq";
                        e.Item.Cells[5].Text = "Course Period";
                        e.Item.Cells[6].Text = "Count";
                        e.Item.Cells[7].Text = "Consignment Agreement";
                        e.Item.Cells[8].Text = "Employment Agreement";
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
        * Function name : PageNavigator1_OnPageIndexChanged
        * Purpose       : C1WebGrid의 페이징 처리를 위한 이벤트 핸들러
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void PageNavigator1_OnPageIndexChanged(object sender, CLT.WEB.UI.COMMON.CONTROL.PagingEventArgs e)
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
        * Function name : grdbtn_Click
        * Purpose       : 훈련위탁계약서 
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grdbtn_Click(object sender, EventArgs e)
        {
            try
            {
                Button xbtn = (Button)sender;

                string xPath = string.Empty; 

                for (int i = 0; i<this.grd.Items.Count; i++)
                {
                    Button xtemp = ((Button)((C1.Web.C1WebGrid.C1GridItem)this.grd.Items[i]).FindControl("btn"));
                    if (xbtn.UniqueID == xtemp.UniqueID)
                    {
                        //print 
                        //btnPrint.OnClientClick = "javascript:OpenReport('vp_a_purchase_order_supplier_standard_detaill_report.aspx?odrid=" + iOdrId + "');return false;";
                        //ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script language='javascript'>flash(\"" + xPath + "\");</script>");
                        xPath = "report.aspx?rpt=agreement_report.xml&open_course_id=" + this.grd.Items[i].Cells[0].Text; 
                        ClientScript.RegisterStartupScript(this.GetType(), "Report", "<script language='javascript'>OpenReport(\"" + xPath + "\");</script>");

                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex); 
            }
        }
        
        /************************************************************
        * Function name : grdbtn_Click
        * Purpose       : 채용예정약정서 
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void grdbtn2_Click(object sender, EventArgs e)
        {
            try
            {
                Button xbtn = (Button)sender;

                string xPath = string.Empty;

                for (int i = 0; i < this.grd.Items.Count; i++)
                {
                    Button xtemp = ((Button)((C1.Web.C1WebGrid.C1GridItem)this.grd.Items[i]).FindControl("btn2"));
                    if (xbtn.UniqueID == xtemp.UniqueID)
                    {                        
                        xPath = "report.aspx?rpt=agreement_contract_report.xml&open_course_id=" + this.grd.Items[i].Cells[0].Text;
                        ClientScript.RegisterStartupScript(this.GetType(), "Report", "<script language='javascript'>OpenReport(\"" + xPath + "\");</script>");

                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        
        protected void btnDnConsignment_Click(object sender, EventArgs e)
        {
            try
            {
                string xPath = "/file/download/consignment_contract.xlsx";
                ClientScript.RegisterStartupScript(this.GetType(), "Report", "<script language='javascript'>document.location.href=\"" + xPath + "\";</script>");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void btnDnConsortium_Click(object sender, EventArgs e)
        {
            try
            {
                string xPath = "/file/download/consortium_apply.xlsx";
                ClientScript.RegisterStartupScript(this.GetType(), "Report", "<script language='javascript'>document.location.href=\"" + xPath + "\";</script>");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
    }
}