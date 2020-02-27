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
using CLT.WEB.UI.COMMON.BASE;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.FX.AGENT;
using C1.Web.C1WebGrid;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.EDUM
{
    public partial class edu_cmp_y_list : BasePage
    {
        #region  사용자함수

        private DataTable GetDtResultList(string rMode)
        {
            DataTable xDt = new DataTable();
            try
            {   
                xDt.Columns.Add("start_rnum");
                xDt.Columns.Add("end_rnum");
                xDt.Columns.Add("start_dt");
                xDt.Columns.Add("end_dt");
                xDt.Columns.Add("course_type");
                xDt.Columns.Add("course_nm");

                DataRow xRow = xDt.NewRow();
                if (rMode == "all")
                {
                    xRow["start_rnum"] = "";
                    xRow["end_rnum"] = "";
                }
                else
                {
                    xRow["start_rnum"] = this.PageSize.ToString(); ;
                    xRow["end_rnum"] = this.CurrentPageIndex.ToString();
                }

                xRow["start_dt"] = Util.ForbidText(txtSTART_DATE.Text);
                xRow["end_dt"] = Util.ForbidText(txtEND_DATE.Text);
                xRow["course_type"] = ddlCourseType.SelectedValue;
                xRow["course_nm"] = Util.ForbidText(txtCourseNM.Text);
                xDt.Rows.Add(xRow);

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                       "GetEduResultList",
                                       LMS_SYSTEM.EDUMANAGEMENT,
                                       "CLT.WEB.UI.LMS.EDUM",
                                       xDt, Thread.CurrentThread.CurrentCulture);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }
        private void BindGrid()
        {
            try
            {
                DataTable xDt = GetDtResultList("default");

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
                    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDt.Rows[0]["totalrecordcount"]) - 1;
                    ListNo = (PageInfo1.TotalRecordCount - ((CurrentPageIndex - 1) * this.PageSize));
                }

                grdList.DataSource = xDt;
                grdList.DataBind();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        #region 초기화

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID;

                base.pRender(this.Page, new object[,] { 
                                                        { this.btnRetrieve, "I" }, 
                                                        { this.btnExcel, "I" }
                                                      });

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (!IsPostBack)
                    {
                        this.txtSTART_DATE.Attributes.Add("onkeyup", "ChkDate(this);");
                        this.txtEND_DATE.Attributes.Add("onkeyup", "ChkDate(this);");

                        this.txtSTART_DATE.Text = DateFormat(GetFirstDayofMonth());
                        this.txtEND_DATE.Text = DateFormat(GetLastDayofMonth());

                        string[] xParams = new string[1];
                        string xSql = string.Empty;
                        DataTable xDt = null;

                        //분류
                        xParams[0] = "0006";
                        xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                                     "GetCommonCodeInfo",
                                                     LMS_SYSTEM.EDUMANAGEMENT,
                                                     "CLT.WEB.UI.LMS.EDUM", (object)xParams, Thread.CurrentThread.CurrentCulture);
                        WebControlHelper.SetDropDownList(this.ddlCourseType, xDt);
                        
                        this.BindGrid();
                    }
                }
                else
                {
                    //return;
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "edu_cmp_y_list", xScriptMsg);
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        #region 화면이벤트

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.BindGrid();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
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
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            this.CurrentPageIndex = 1;
            this.PageInfo1.CurrentPageIndex = 1;
            this.PageNavigator1.CurrentPageIndex = 1;
            this.BindGrid();
        }
        protected void grdList_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                //Label lbl_no = ((Label)e.Item.FindControl("lbl_no"));
                //if (!Util.IsNullOrEmptyObject(lbl_no))
                //{
                //    lbl_no.Text = ListNo.ToString();
                //    ListNo = ListNo - 1;
                //}

                DataRowView DRV = (DataRowView)e.Item.DataItem;
                Button brnPrint = ((Button)e.Item.FindControl("btnPrint"));
                //이수여부
                if (Convert.ToString(DRV["pass_flg"].ToString()) == "000001")
                    //한진해운 운항훈련원인 경우만 수료증 발급가능, 한글/영문체크 포함, 교육단체명 변경 시 수정필요
                    //2020.03 지마린서비스일 경우, 출력
                    if (DRV["EDUCATIONAL_ORG"].ToString() == "000007" || DRV["EDUCATIONAL_ORG"].ToString() == "000035")
                        brnPrint.Visible = true;
                    else
                        brnPrint.Visible = false;
                else
                    brnPrint.Visible = false;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void grdList_ItemCommand(object sender, C1.Web.C1WebGrid.C1CommandEventArgs e)
        {
            
        }
        #endregion

        #region 액셀
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable xDt = GetDtResultList("all");

                int xColumnCnt = 7;
                string[] xExcelHeader = new string[xColumnCnt];
                if (this.IsSettingKorean())
                {
                    xExcelHeader[0] = "사번";
                    xExcelHeader[1] = "직급";
                    xExcelHeader[2] = "성명";
                    xExcelHeader[3] = "과정명";
                    xExcelHeader[4] = "교육기간";
                    xExcelHeader[5] = "유효기간";
                    xExcelHeader[6] = "주민번호";
                }
                else
                {
                    xExcelHeader[0] = "ID";
                    xExcelHeader[1] = "Grade";
                    xExcelHeader[2] = "Name";
                    xExcelHeader[3] = "Corse Name";
                    xExcelHeader[4] = "Study Period";
                    xExcelHeader[5] = "Expired Date";
                    xExcelHeader[6] = "Personal No";
                }

                string[] xDtHeader = new string[xColumnCnt];
                xDtHeader[0] = "user_no";
                xDtHeader[1] = "step_name";
                xDtHeader[2] = "user_nm_kor";
                xDtHeader[3] = "course_nm";
                xDtHeader[4] = "course_dt";
                xDtHeader[5] = "expired_period";
                xDtHeader[6] = "PERSONAL_NO";

                this.GetExcelFile(xDt, xExcelHeader, xDtHeader, "1");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        protected void grdList_ItemCreated(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "사번";
                        e.Item.Cells[2].Text = "직급";
                        e.Item.Cells[3].Text = "성명";
                        e.Item.Cells[4].Text = "과정명";
                        e.Item.Cells[5].Text = "교육기간";
                        e.Item.Cells[6].Text = "유효기간";
                        e.Item.Cells[7].Text = "수료증";
                        e.Item.Cells[8].Text = "교육기관";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "ID";
                        e.Item.Cells[2].Text = "Grade";
                        e.Item.Cells[3].Text = "Name";
                        e.Item.Cells[4].Text = "Course Name";
                        e.Item.Cells[5].Text = "Study Period";
                        e.Item.Cells[6].Text = "Expired Date";
                        e.Item.Cells[7].Text = "Certification";
                        e.Item.Cells[8].Text = "Learning Institutions";
                    }
                }
                //교육기관 코드 컬럼은 Print버튼 활성화를 위한 표기로 Visible False 처리
                e.Item.Cells[8].Visible = false;
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                Button xbtn = (Button)sender;
                string xPath = string.Empty;
                for (int i = 0; i < this.grdList.Items.Count; i++)
                {
                    Button xtemp = ((Button)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("btnPrint"));
                    if (!Util.IsNullOrEmptyObject(xtemp) && xbtn.UniqueID == xtemp.UniqueID)
                    {
                        string xEduOrg = ((HtmlInputHidden)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("txtEducationalOrg")).Value;
                        string xReportOld = xEduOrg == "000007" ? "_old" : "";
                        string xWidth = xEduOrg == "000007" ? "880" : "1280";

                        //c.course_id || '^' || o.open_course_id || '^' || r.course_result_seq || '^' || u.user_id
                        string[] keys = Util.Split(Convert.ToString(grdList.DataKeys[i]), "^", 4);
                        xPath = "/edum/edu_cmp_y_report" + xReportOld + ".aspx?rpt=agreement_certificate_report.xml&open_course_id=" + keys[1] + "&user_id=" + keys[3];
                        ClientScript.RegisterStartupScript(this.GetType(), "Report", "<script language='javascript'>OpenReport(\"" + xPath + "\", \"" + xWidth + "\");</script>");
                        
                    }
                }

            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
    }
}
