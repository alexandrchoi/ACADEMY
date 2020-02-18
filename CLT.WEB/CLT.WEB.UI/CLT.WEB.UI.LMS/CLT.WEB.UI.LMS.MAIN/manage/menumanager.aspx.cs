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
    /// 1. 작업개요 : 메뉴별 권한 관리 Class
    /// 
    /// 2. 주요기능 : LMS 웹사이트 메뉴별 권한 관리
    ///				  
    /// 3. Class 명 : menumanager
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02
    /// </summary>
    public partial class menumanager : BasePage
    {
        DataTable iGridDt = null;
        int iRowcount = 0;
        ArrayList ialCodelist = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    //this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                    base.pRender(this.Page, new object[,] { { this.btnSave, "E" }
                                                      });

                    //this.PageSize = 50;
                    if (Session["USER_GROUP"].ToString() == "000009")
                    {
                        //return;
                        string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                        ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list_wpg", xScriptMsg);
                    }
                    else
                    {
                        this.BindDropDownList();
                        this.BindGrid();
                    }
                }

            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }

        }

        private void BindDropDownList()
        {
            try
            {
                string[] xParams = new string[1];
                string xSql = string.Empty;
                DataTable xDt = null;

                // UserGroupCode
                xParams[0] = "0041";

                xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md",
                                             "GetCommonCodeInfo",
                                             LMS_SYSTEM.MANAGE,
                                             "CLT.WEB.UI.LMS.MANAGE", (object)xParams, Thread.CurrentThread.CurrentCulture);

                WebControlHelper.SetDropDownList(this.ddlCourseField, xDt, 0, false);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        private void BindGrid()
        {
            try
            {
                string[] xParams = new string[3];
                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // pageno
                xParams[2] = this.ddlCourseField.SelectedItem.Value.ToString();



                iGridDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_manage_md",        // Class Name
                                                 "GetMenuGroup",                           // Mehtod Name
                                                 LMS_SYSTEM.MAIN,                          // Project Type
                                                 "CLT.WEB.UI.LMS.MANAGE",                  // NameSpace Name
                                                 (object)xParams);                         // Page Info


                C1WebGrid1.DataSource = iGridDt;
                C1WebGrid1.DataBind();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            //if (iGridDt.Rows.Count < 1)
            //{
            //    this.PageInfo1.TotalRecordCount = 0;
            //    this.PageInfo1.PageSize = this.PageSize;
            //    this.PageNavigator1.TotalRecordCount = 0;
            //}
            //else
            //{
            //    this.PageInfo1.TotalRecordCount = Convert.ToInt32(iGridDt.Rows[0]["totalrecordcount"]);
            //    this.PageInfo1.PageSize = this.PageSize;
            //    this.PageNavigator1.TotalRecordCount = Convert.ToInt32(iGridDt.Rows[0]["totalrecordcount"]);
            //}

            //this.PageInfo1.RecordCount = iGridDt.Rows.Count; //Convert.ToInt32(iGridDt.Rows[0]["totalrecordcount"]);
            //this.PageNavigator1.RecordCount = 0;//iGridDt.Rows.Count; //Convert.ToInt32(iGridDt.Rows[0]["totalrecordcount"]);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string xRtn = Boolean.TrueString;
                string xScriptContent = string.Empty;

                ArrayList xalList = (ArrayList)ViewState["alCodelist"];

                if (xalList == null)
                {
                    //xScriptContent = "<script>alert('변경한 메뉴 권한이 없습니다!');opener.reload();self.close();</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A083", new string[] { "변경된 메뉴" }, new string[] { "Changed the menu" }, Thread.CurrentThread.CurrentCulture));
                    return;
                }


                string[,] xParam = new string[this.C1WebGrid1.Items.Count, 6];


                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {

                    Label xlblcode = ((Label)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("lblmenu"));

                    string xcode = xlblcode.Text;



                    if (!xalList.Contains(xcode))
                        continue;

                    CheckBox xchkInquiry = ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry"));
                    CheckBox xchkEdit = ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit"));
                    CheckBox xchkDel = ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkDel"));
                    CheckBox xchkAdmin = ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkAdmin"));

                    xParam[i, 0] = xcode; // menucode
                    xParam[i, 1] = this.ddlCourseField.SelectedValue;
                    xParam[i, 2] = ConvertCheckbox(xchkInquiry);
                    xParam[i, 3] = ConvertCheckbox(xchkEdit);
                    xParam[i, 4] = ConvertCheckbox(xchkDel);
                    xParam[i, 5] = ConvertCheckbox(xchkAdmin);
                }

                xRtn = SBROKER.GetString("CLT.WEB.BIZ.LMS.MANAGE.vp_m_manage_md",
                                                             "SetMenuGroup",
                                                             LMS_SYSTEM.MAIN,
                                                             "CLT.WEB.UI.LMS.MANAGE",
                                                             (object)xParam);

                if (xRtn.ToUpper() == "TRUE")
                {
                    //xScriptContent = "<script>alert('정상적으로 처리 완료되었습니다.');opener.reload();self.close();</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A054", new string[] { "처리" }, new string[] { "Processed" }, Thread.CurrentThread.CurrentCulture));
                }
                else
                {

                    //xScriptContent = "<script>alert('정상적으로 처리되지 않았으니, 관리자에게 문의 바랍니다.');self.close();</script>";
                    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A101", new string[] { "관리자" }, new string[] { "Administrator" }, Thread.CurrentThread.CurrentCulture));
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        private void ChkChanged(object sender, EventArgs e, string rFindControl)
        {
            try
            {
                CheckBox chkTemp = (CheckBox)sender;
                if (ViewState["alCodelist"] != null) // User가 체크한 체크박스가 존재 할 경우
                    ialCodelist = (ArrayList)ViewState["alCodelist"];

                string xMenucode = chkTemp.Attributes["menucode"].ToString();

                // 대분류 구분
                if (xMenucode.Substring(1, 2) == "00")
                {
                    for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                    {
                        CheckBox xchk = ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl(rFindControl));
                        if (string.IsNullOrEmpty(xchk.Attributes["menucode"]))
                            continue;


                        if (xchk.Attributes["menucode"].Substring(0, 1) == xMenucode.Substring(0, 1))
                        {
                            if (chkTemp.Checked == true)
                            {
                                xchk.Checked = true;
                            }
                            else
                            {
                                xchk.Checked = false;
                            }


                            if (rFindControl == "chkEdit") // 수정권한시

                            {
                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry")).Checked = chkTemp.Checked;  // 조회권한 변경

                            }
                            else if (rFindControl == "chkDel") // 삭제권한시

                            {
                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry")).Checked = chkTemp.Checked;  // 조회권한 변경

                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked = chkTemp.Checked;  // 조회권한 변경

                            }
                            else if (rFindControl == "chkAdmin") // 관리자 권한시

                            {
                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry")).Checked = chkTemp.Checked;  // 조회권한 변경

                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked = chkTemp.Checked;  // 조회권한 변경

                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkDel")).Checked = chkTemp.Checked;  // 조회권한 변경

                            }

                            if (!ialCodelist.Contains(xchk.Attributes["menucode"].ToString()))
                                ialCodelist.Add(xchk.Attributes["menucode"].ToString());

                            xchk.Focus();
                        }
                    }
                }
                else if (xMenucode.Substring(2, 1) == "0") // 중분류 구분
                {
                    for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                    {
                        CheckBox xchk = ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl(rFindControl));
                        if (string.IsNullOrEmpty(xchk.Attributes["menucode"]))
                            continue;

                        if ((xchk.Attributes["menucode"].Substring(0, 2) == xMenucode.Substring(0, 2)) || (xchk.Attributes["menucode"].ToString() == xMenucode.Substring(0, 1) + "00"))
                        {
                            if (xchk.Attributes["menucode"].Substring(0, 1) != "0") // 대메뉴이면 Contiune
                                continue;

                            if (chkTemp.Checked == true)
                            {
                                xchk.Checked = true;
                            }
                            else
                            {
                                xchk.Checked = false;
                            }


                            if (rFindControl == "chkEdit") // 수정권한시

                            {
                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry")).Checked = chkTemp.Checked;  // 조회권한 변경


                            }
                            else if (rFindControl == "chkDel") // 삭제권한시

                            {
                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry")).Checked = chkTemp.Checked;  // 조회권한 변경


                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked = chkTemp.Checked;  // 조회권한 변경


                            }
                            else if (rFindControl == "chkAdmin") // 관리자 권한시

                            {
                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry")).Checked = chkTemp.Checked;  // 조회권한 변경


                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked = chkTemp.Checked;  // 조회권한 변경


                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkDel")).Checked = chkTemp.Checked;  // 조회권한 변경


                            }

                            if (!ialCodelist.Contains(xchk.Attributes["menucode"].ToString()))
                                ialCodelist.Add(xchk.Attributes["menucode"].ToString());

                            xchk.Focus();
                        }
                    }
                }









                /*
                else if (xMenucode.Substring(2, 1) != "0") // 소분류 구분
                {
                    for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                    {
                        CheckBox xchk = ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl(rFindControl));
                        if (string.IsNullOrEmpty(xchk.Attributes["menucode"]))
                            continue;

                        if ((xchk.Attributes["menucode"].Substring(0, 2) == xMenucode.Substring(0, 2)) || (xchk.Attributes["menucode"].ToString() == xMenucode.Substring(0, 1) + "00")) 
                        {
                            if (xchk.Attributes["menucode"].Substring(2, 1) != "0") // 소메뉴이면 Contiune
                                continue;

                            if (chkTemp.Checked == true)
                            {
                                xchk.Checked = true;
                            }
                            else
                            {
                                xchk.Checked = false;
                            }


                            if (rFindControl == "chkEdit") // 수정권한시
                            {
                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry")).Checked = chkTemp.Checked;  // 조회권한 변경

                            }
                            else if (rFindControl == "chkDel") // 삭제권한시
                            {
                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry")).Checked = chkTemp.Checked;  // 조회권한 변경

                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked = chkTemp.Checked;  // 조회권한 변경

                            }
                            else if (rFindControl == "chkAdmin") // 관리자 권한시
                            {
                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry")).Checked = chkTemp.Checked;  // 조회권한 변경

                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked = chkTemp.Checked;  // 조회권한 변경

                                ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkDel")).Checked = chkTemp.Checked;  // 조회권한 변경

                            }

                            if (!ialCodelist.Contains(xchk.Attributes["menucode"].ToString()))
                                ialCodelist.Add(xchk.Attributes["menucode"].ToString());

                            xchk.Focus();
                        }
                    }
                }
                */


                for (int i = 0; i < this.C1WebGrid1.Items.Count; i++)
                {
                    CheckBox xchk = ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl(rFindControl));
                    if (string.IsNullOrEmpty(xchk.Attributes["menucode"]))
                        continue;

                    if (xMenucode == xchk.Attributes["menucode"])
                    {
                        if (rFindControl == "chkEdit") // 수정권한시

                        {
                            ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry")).Checked = chkTemp.Checked;  // 조회권한 변경


                        }
                        else if (rFindControl == "chkDel") // 삭제권한시

                        {
                            ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry")).Checked = chkTemp.Checked;  // 조회권한 변경


                            ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked = chkTemp.Checked;  // 조회권한 변경


                        }
                        else if (rFindControl == "chkAdmin") // 관리자 권한시

                        {
                            ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkInquiry")).Checked = chkTemp.Checked;  // 조회권한 변경


                            ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkEdit")).Checked = chkTemp.Checked;  // 조회권한 변경


                            ((CheckBox)((C1.Web.C1WebGrid.C1GridItem)this.C1WebGrid1.Items[i]).FindControl("chkDel")).Checked = chkTemp.Checked;  // 조회권한 변경


                        }
                    }
                }

                if (!ialCodelist.Contains(xMenucode))
                    ialCodelist.Add(xMenucode);

                //ViewState.Add("alCodelist", ialCodelist);
                ViewState["alCodelist"] = ialCodelist;

                if (xMenucode.Substring(1, 2) != "00")
                    chkTemp.Focus();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        private string ConvertCheckbox(CheckBox rChk)
        {
            string xResult = "N";
            try
            {
                if (rChk.Checked == true)
                    xResult = "Y";
                else
                    xResult = "N";
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xResult;
        }

        protected void ddlCourseFiled_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.BindGrid();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        protected void chkInquiry_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChkChanged(sender, e, "chkInquiry");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void chkEdit_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChkChanged(sender, e, "chkEdit");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void chkDel_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChkChanged(sender, e, "chkDel");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void chkAdmin_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChkChanged(sender, e, "chkAdmin");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        protected void grd_ItemCreated(object seder, C1ItemEventArgs e)
        {
            if (e.Item.ItemType == C1ListItemType.Header)
            {
                if (this.IsSettingKorean())
                {
                    e.Item.Cells[0].Text = "대메뉴";
                    e.Item.Cells[1].Text = "중메뉴";
                    e.Item.Cells[2].Text = "소메뉴";
                    e.Item.Cells[3].Text = "조회권한";
                    e.Item.Cells[4].Text = "수정권한";
                    e.Item.Cells[5].Text = "삭제권한";
                    e.Item.Cells[6].Text = "관리자권한";
                }
                else
                {
                    e.Item.Cells[0].Text = "Level 1";
                    e.Item.Cells[1].Text = "Level 2";
                    e.Item.Cells[2].Text = "Level 3";
                    e.Item.Cells[3].Text = "Inquiry";
                    e.Item.Cells[4].Text = "Modify";
                    e.Item.Cells[5].Text = "Delete";
                    e.Item.Cells[6].Text = "Admin";
                }
            }
        }

        protected void C1WebGrid1_ItemDataBound(object sender, C1ItemEventArgs e)
        {
            try
            {
                string xInquiry_yn = string.Empty;
                string xEdit_yn = string.Empty;
                string xDel_yn = string.Empty;
                string xAdmin_yn = string.Empty;
                string xMenucode = string.Empty;

                CheckBox chkTempInquery = null;
                CheckBox chkTempEdit = null;
                CheckBox chkTempDel = null;
                CheckBox chkTempAdmin = null;

                Label lblcode = null;

                DataRowView xItem = (DataRowView)e.Item.DataItem;



                if (xItem != null)
                {
                    xInquiry_yn = xItem["inquiry_yn"].ToString();
                    xEdit_yn = xItem["edit_yn"].ToString();
                    xDel_yn = xItem["del_yn"].ToString();
                    xAdmin_yn = xItem["admin_yn"].ToString();
                    xMenucode = xItem["mgroupcode"].ToString();
                }

                if (e.Item.ItemType == C1ListItemType.Item || e.Item.ItemType == C1ListItemType.AlternatingItem)
                {

                    chkTempInquery = ((CheckBox)e.Item.FindControl("chkInquiry"));
                    chkTempEdit = ((CheckBox)e.Item.FindControl("chkEdit"));
                    chkTempDel = ((CheckBox)e.Item.FindControl("chkDel"));
                    chkTempAdmin = ((CheckBox)e.Item.FindControl("chkAdmin"));
                    lblcode = ((Label)e.Item.FindControl("lblmenu"));
                    lblcode.Text = xMenucode;
                    // 메뉴코드
                    chkTempInquery.Attributes.Add("menucode", xMenucode);
                    chkTempEdit.Attributes.Add("menucode", xMenucode);
                    chkTempDel.Attributes.Add("menucode", xMenucode);
                    chkTempAdmin.Attributes.Add("menucode", xMenucode);



                    if (xInquiry_yn == "Y")
                        chkTempInquery.Checked = true;
                    else
                        chkTempInquery.Checked = false;

                    if (xEdit_yn == "Y")
                        chkTempEdit.Checked = true;
                    else
                        chkTempEdit.Checked = false;

                    if (xDel_yn == "Y")
                        chkTempDel.Checked = true;
                    else
                        chkTempDel.Checked = false;

                    if (xAdmin_yn == "Y")
                        chkTempAdmin.Checked = true;
                    else
                        chkTempAdmin.Checked = false;
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

    }
}
