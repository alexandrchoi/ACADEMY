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
using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.FX.AGENT;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.EDUM
{
    public partial class issuing : BasePage
    {   
        //O.COURSE_ID ||'^'|| O.OPEN_COURSE_ID
        private string iSearch { get { return Util.Request("search"); } }
        private string iPageHV { get { return Util.Request("page_hv"); } }

        #region 인터페이스 그룹

        #endregion

        #region 기타 프로시저 그룹 [Core Logic]
        private DataSet GetDsGrdList(string rGubun)
        {
            DataSet xDs = null;
            try
            {
                string[] xParams = new string[6];
                xParams[0] = this.PageSize.ToString();
                xParams[1] = this.CurrentPageIndex.ToString();
                xParams[2] = iSearch;
                string[] rSearch = Util.Split(iSearch, "^", 2);
                xParams[4] = rSearch[0];
                xParams[5] = rSearch[1];

                xDs = SBROKER.GetDataSet("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                       "GetEduIssuingUserList",
                                       LMS_SYSTEM.EDUMANAGEMENT,
                                       "CLT.WEB.UI.LMS.EDUM",
                                       (object)xParams,
                                       rGubun, Thread.CurrentThread.CurrentCulture);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDs;
        }
        private void BindGrdList(int rPageIndex, string rGubun)
        {
            try
            {
                DataSet xDs = GetDsGrdList(rGubun);
                DataTable xDtTotalCnt = xDs.Tables[0];
                DataTable xDt = xDs.Tables[1];

                if (Convert.ToInt32(xDtTotalCnt.Rows[0][0]) < 1)
                {
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageInfo1.TotalRecordCount = 0;
                    this.PageNavigator1.TotalRecordCount = 0;
                }
                else
                {
                    this.PageInfo1.PageSize = this.PageSize;
                    this.PageInfo1.TotalRecordCount = Convert.ToInt32(xDtTotalCnt.Rows[0][0]);
                    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(xDtTotalCnt.Rows[0][0]) - 1;
                    PageInfo1.CurrentPageIndex = rPageIndex;
                    PageNavigator1.CurrentPageIndex = rPageIndex;
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

        #region 초기화 그룹
        private void InitControl()
        {
            try
            {
                base.pRender(this.Page, new object[,] { 
                                                        { this.btn_save, "E" }
                                                      });
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.close();</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "issuing_win", xScriptMsg);

                    return;
                }

                base.pRender(this.Page, new object[,] { 
                                                        { this.btn_save, "E" }, 
                                                        { this.btnPrint, "E" }
                                                      });

                if (Convert.ToString(Session["USER_ID"]) != "" && Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {
                    if (!IsPostBack)
                    {
                        InitControl();
                        
                        BindGrdList(1, "");
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion

        #region 화면 컨트롤 이벤트 핸들러 그룹

        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrdList(1, "");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                string xFileName = "";
                byte[] xFileByte = null;
                int xChkSel = 0;
                int xCntSel = 0;
                
                for (int i = 0; i < this.grdList.Items.Count; i++)
                {
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chk_sel")).Checked)
                    {
                        xChkSel++;
                        FileUpload file = ((FileUpload)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("fileUplaod"));
                        if (file.FileBytes.Length > 0)
                        {
                            xCntSel++;
                            xFileName = file.FileName.Replace(" ", "_").Replace("..", "_");
                            xFileByte = file.FileBytes;

                            SBROKER.GetString("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                                            "SetFileAtt",
                                                            LMS_SYSTEM.MANAGE,
                                                            "CLT.WEB.UI.LMS.EDUM.vp_a_eduming_issuing_wpg",
                                                            xFileByte,
                                                            xFileName,
                                                            grdList.DataKeys[i].ToString());
                        }
                        file.Dispose();
                    }
                }

                if (xCntSel > 0)
                {   
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A001", new string[] { "사진" }, new string[] { "Photo" }, Thread.CurrentThread.CurrentCulture));
                    GC.Collect();
                    //BindGrdList(1, "");
                }
                else if (xChkSel == 0)
                {
                    ScriptHelper.Page_Alert(this, CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A047", new string[] { "" }, new string[] { "" }, System.Threading.Thread.CurrentThread.CurrentCulture));
                }
                else
                {
                    ScriptHelper.Page_Alert(this, CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { "사진" }, new string[] { "Photo" }, System.Threading.Thread.CurrentThread.CurrentCulture));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        protected void grdUserList_ItemDataBound(object sender, C1.Web.C1WebGrid.C1ItemEventArgs e)
        {
            try
            {
                DataRowView DRV = (DataRowView)e.Item.DataItem;
                Image img_pic_file = ((Image)e.Item.FindControl("img_pic_file"));
                FileUpload fileUplaod = ((FileUpload)e.Item.FindControl("fileUplaod"));

                img_pic_file.Visible = false;
                //if (!Util.IsNullOrEmptyObject(DRV["PIC_FILE"]))
                //    img_pic_file.Visible = true;
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
                this.BindGrdList(e.PageIndex, "");
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
                        e.Item.Cells[1].Text = "회사명";
                        e.Item.Cells[2].Text = "과정명";
                        e.Item.Cells[3].Text = "증서번호";
                        e.Item.Cells[4].Text = "성명";
                        e.Item.Cells[5].Text = "주민등록번호";
                        e.Item.Cells[6].Text = "사진";
                        e.Item.Cells[7].Text = "정보수정";
                        e.Item.Cells[8].Text = e.Item.Cells[8].Text.Replace("1st issue", "신규발급");
                        e.Item.Cells[9].Text = "발급사유";
                    }
                    else
                    {
                        e.Item.Cells[1].Text = "Company";
                        e.Item.Cells[2].Text = "Course";
                        e.Item.Cells[3].Text = "Cert.No";
                        e.Item.Cells[4].Text = "Name";
                        e.Item.Cells[5].Text = "Registration";
                        e.Item.Cells[6].Text = "Photo";
                        e.Item.Cells[7].Text = "Modification";
                        e.Item.Cells[8].Text = e.Item.Cells[8].Text.Replace("신규발급", "1st issue");
                        e.Item.Cells[9].Text = "Reason";
                    }
                }
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
                string xKeys = "";
                int xCntSel = 0;
                for (int i = 0; i < this.grdList.Items.Count; i++)
                {
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chk_sel")).Checked)
                    {
                        if (string.IsNullOrEmpty(((HtmlInputText)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("txtReason")).Value))
                        {
                            //발급사유 이(가) 입력되지 않았습니다.
                            ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A004", new string[] { "발급사유" }, new string[] { "Reason" }, Thread.CurrentThread.CurrentCulture));
                            ((HtmlInputText)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("txtReason")).Focus();
                            return;
                        }

                        if (!Util.IsNullOrEmptyObject(xKeys))
                            xKeys += "|";
                        xKeys += this.grdList.DataKeys[i].ToString();
                        xCntSel++;
                    }
                }

                /* 로그 INSERT */

                string[,] xParam = new string[xCntSel, 7];
                int j = 0;
                for (int i = 0; i < this.grdList.Items.Count; i++)
                {
                    if (((HtmlInputCheckBox)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("chk_sel")).Checked)
                    {
                        string xKey = this.grdList.DataKeys[i].ToString();
                        string[] arrKeys = xKey.Split('^');
                        string xCertKey = ((HtmlInputHidden)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("txtCertKey")).Value;
                        string xCertName = ((HtmlInputHidden)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("txtCertName")).Value;
                        string xReason = ((HtmlInputText)((C1.Web.C1WebGrid.C1GridItem)this.grdList.Items[i]).FindControl("txtReason")).Value;


                        xParam[j, 0] = arrKeys[0];
                        xParam[j, 1] = arrKeys[1];
                        xParam[j, 2] = arrKeys[2];
                        xParam[j, 3] = xCertKey;
                        xParam[j, 4] = xCertName;
                        xParam[j, 5] = xReason;
                        xParam[j, 6] = Convert.ToString(Session["USER_ID"]);

                        j++;
                    }
                }


                if (xCntSel > 0)
                {
                    string xRtn = Boolean.TrueString;
                    xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.EDUM.vp_a_edumng_md",
                                                                 "SetReportHistory",
                                                                 LMS_SYSTEM.EDUMANAGEMENT,
                                                                 "CLT.WEB.UI.LMS.EDUM",
                                                                 (object)xParam);

                    if (xRtn.ToUpper() == "TRUE")
                    {
                        //c.course_id ||'^'|| o.open_course_id||'^'|| r.course_result_seq'^'|| u.user_id
                        //string xPath = "vp_a_report_wpg.aspx?rpt=vp_a_edumng_cert_1_report.xml&keys=" + xKeys;
                        //string xPath = "issuing_report.aspx?rpt=cert_1_report.xml&keys=" + xKeys;
                        string xPath = "issuing_report.aspx?keys=" + xKeys;
                        if (iPageHV == "H")
                            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language='javascript'>OpenReport('" + xPath + "', '1280');</script>");
                        else if (iPageHV == "V")
                            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language='javascript'>OpenReport('" + xPath + "', '900');</script>");
                    }
                    else
                    {
                        //xScriptContent = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');self.close();</script>";
                        ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                    }
                }
                else
                {
                    ScriptHelper.Page_Alert(this, MsgInfo.GetMsg("A047", new string[] { "" }, new string[] { "" }, System.Threading.Thread.CurrentThread.CurrentCulture));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
    }
}
