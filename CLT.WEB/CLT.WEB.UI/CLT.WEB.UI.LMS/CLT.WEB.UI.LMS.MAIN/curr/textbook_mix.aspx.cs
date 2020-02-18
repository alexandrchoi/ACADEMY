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
    /// 1. 작업개요 : textbook_mix 페이지 Class
    /// 
    /// 2. 주요기능 : 특정 (등록)과정의 과정 & 교재 현황조회
    ///				  
    /// 3. Class 명 : textbook_mix
    /// 
    /// 4. 작 업 자 : 김양도 / 2012.02.12
    /// </summary>
    public partial class textbook_mix : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //this.btnRetrieve.Attributes.Add("onclick", string.Format("return keyConfirm({0}, '조회하실 과정을 선택해 주세요.');", this.txtCourseID.ClientID));
                this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping

                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    //return;
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "vp_y_community_notice_list", xScriptMsg);
                }
                else
                {
                    if (!IsPostBack)
                    {
                        //this.BindGrid();
                        //base.SetGridClear(this.C1WebGrid1, this.PageNavigator1);
                        //this.PageInfo1.TotalRecordCount = 0;
                        //this.PageInfo1.PageSize = this.PageSize;
                        //this.PageNavigator1.TotalRecordCount = 0;

                        this.C1WebGrid1.DataBind();

                    }
                    base.pRender(this.Page, new object[,] { { this.btnRetrieve, "I" } });
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : BindGrid
        * Purpose       : 특정 (등록)과정의 과목 & 컨텐츠 데이터를 DataGrid에 바인딩을 위한 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        private void BindGrid()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtCourseID.Value))
                {
                    string[] xParams = new string[1];
                    xParams[0] = this.txtCourseID.Value; // course_id

                    DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_c_textbook_md",
                                                 "GetTextBookMixInfoOfCourse",
                                                 LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR", (object)xParams);
                    C1WebGrid1.DataSource = xDt;
                    C1WebGrid1.DataBind();

                    if (xDt.Rows.Count > 0)
                    {
                        this.txtCourseNM.Text = xDt.Rows[0]["course_nm"].ToString();
                    }

                }
                else
                {
                    C1WebGrid1.DataSource = null;
                    C1WebGrid1.DataBind();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }

        /************************************************************
        * Function name : C1WebGrid1_ItemCreated
        * Purpose       : C1WebGrid의 Item이 생성될때 호출되는 이벤트 핸들러
                          C1WebGrid 해더의 언어설정 적용을 위한 부분
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void C1WebGrid1_ItemCreated(object sender, C1ItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == C1ListItemType.Header)
                {
                    if (this.IsSettingKorean())
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "교재유형";
                        e.Item.Cells[2].Text = "교재명";
                        e.Item.Cells[3].Text = "출판사";
                        e.Item.Cells[4].Text = "저자";
                    }
                    else
                    {
                        e.Item.Cells[0].Text = "No.";
                        e.Item.Cells[1].Text = "TextBook Type";
                        e.Item.Cells[2].Text = "TextBook Name";
                        e.Item.Cells[3].Text = "Publisher";
                        e.Item.Cells[4].Text = "Author";
                    }
                }
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        /************************************************************
        * Function name : btnRetrieve_Click
        * Purpose       : 조회 조건에 대한 결과를 조회하여 리스트로 출력하는 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.txtCourseID.Value == string.Empty)
                //{
                //    //A003: {0} was not selected.
                //    ScriptHelper.Page_Alert(this.Page, MsgInfo.GetMsg("A003",
                //                                                                            new string[] { "과정" },
                //                                                                            new string[] { "Course" },
                //                                                                            Thread.CurrentThread.CurrentCulture
                //                                                                           ));
                //    return; 
                //}

                this.BindGrid();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
    }
}