using System;
using System.Data;
using System.Configuration;
using System.Collections;//
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

// 필수 using 문
using C1.Web.C1WebGrid;
using C1.Web.C1WebChart;
using C1.Win.C1Chart;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;
using System.Drawing;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.UI.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : 설문 결과 Class
    /// 
    /// 2. 주요기능 : LMS 설문 결과
    ///				  
    /// 3. Class 명 : survey_results_detail
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.01
    ///
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class survey_results_detail : BasePage
    {
        // Layout.css 파일에 있는 각 Layout 값을 전역변수로 지정하여 사용...(동적 컨트롤 생성용)
        string tableStyle = "background-color:White; overflow:auto;";
        //string pop_left = "background-color:#E3EBEF;height: 27px;padding-left:5px; border-bottom:#C5D3DE solid 1px;border-top:#C5D3DE solid 1px;";
        //string pop_right = "padding-left:5px;border-bottom:#C5D3DE solid 1px;border-top:#C5D3DE solid 1px; text-align:left;";

        string pop_Top = "padding-left:5px;border-top:#C5D3DE solid 1px; text-align:left;";
        string pop_bottom = "padding-left:5px;border-bottom:#C5D3DE solid 1px; text-align:left;";
        string pop = "padding-left:5px; text-align:left;";

        string iGrid = "";

        string grid_main = "text-decoration :underline;";
        string grid_list_top = "BORDER-RIGHT: #ffffff 1px solid;BORDER-TOP:#acbdd7 2px solid;BORDER-LEFT: #ffffff 1px solid;BORDER-COLLAPSE: collapse;width:100%;font-size:9pt;font-weight:bold;text-align:center;background-color:#f2f2f2;BORDER-bottom:#e3e3e3 2px solid;height:32px;color:#6485ba;";
        string grid_list_oddbody = "BORDER-RIGHT: #ffffff 1px solid;BORDER-TOP:#acbdd7 0px solid;BORDER-LEFT: #ffffff 1px solid;BORDER-bottom:#e3e3e3 1px solid;BORDER-COLLAPSE: collapse;width:100%;font-size:12pt;height:22px;color:#555555;background-color: #ffffff;";
        string grid_list_evenbody = "BORDER-RIGHT: #ffffff 1px solid;BORDER-TOP:#acbdd7 0px solid; BORDER-LEFT: #ffffff 1px solid;BORDER-bottom:#e3e3e3 1px solid;BORDER-COLLAPSE: collapse;width:100%;font-size:12pt;height:22px;color:#555555;background-color: #f6f6f6;";
        
        string iChartStyle = string.Empty;

        /************************************************************
        * Function name : Page_Load
        * Purpose       : 설문조사 페이지 Load 이벤트
        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["USER_GROUP"].ToString() == this.GuestUserID)
                {
                    string xScriptMsg = string.Format("<script>alert('사용권한이 없습니다.');window.location.href='/';</script>", Session["MENU_CODE"]);
                    ScriptHelper.ScriptBlock(this, "survey_result_detail", xScriptMsg);

                    return;
                }

                bool xOpen = true;
                if (Request.QueryString["rMode"] == null && Request.QueryString["rMode"].ToString() == "")
                    xOpen = false;
                else if (Request.QueryString["rResNo"] == null && Request.QueryString["rResNo"].ToString() == "")
                    xOpen = false;
                else if (Request.QueryString["rResSumCnt"] == null && Request.QueryString["rResSumCnt"].ToString() == "")
                    xOpen = false;
                else if (Request.QueryString["rRes_rec_cnt"] == null && Request.QueryString["rRes_rec_cnt"].ToString() == "")
                    xOpen = false;
                else if (Request.QueryString["rResQueCnt"] == null && Request.QueryString["rResQueCnt"].ToString() == "")
                    xOpen = false;

                if (xOpen == false)
                {
                    string xScriptContent = "<script>alert('잘못된 경로를 통해 접근하였습니다.');self.close();</script>";
                    ScriptHelper.ScriptBlock(this, "survey_result_detail", xScriptContent);
                    return;
                }

                if (!IsPostBack)
                {
                    //this.Page.Form.DefaultButton = this.btnRetrieve.UniqueID; // Page Default Button Mapping
                    //BindChart();
                }
                //AddResult(Request.QueryString["rResQueCnt"].ToString());
                AddContent(Request.QueryString["rResQueCnt"].ToString());
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }
        #endregion
        
        /************************************************************
        * Function name : btnList_OnClick
        * Purpose       : 설문 리스트로 이동

        * Input         : void
        * Output        : void
        *************************************************************/
        #region protected void btnList_OnClick(object sender, EventArgs e)
        protected void btnList_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/curr/survey_results.aspx");
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        #endregion

        /************************************************************
        * Function name : AddResult
        * Purpose       : 웹페이지 설문결과 서식 동적컨트롤 생성 메서드
        * Input         : string rResQueCnt(설문 항목수)
        * Output        : void
        *************************************************************/
        #region AddResult
        public void AddResult(string rResQueCnt)
        {
            try
            {
                // 설문 항목수 만큰 그리드와 차트를 생성한다. (서술형은 차트 제외)
                int xQueCnt = Convert.ToInt32(rResQueCnt);  // 설문 항목수
                decimal xSumCnt = Convert.ToInt32(Request.QueryString["rResSumCnt"].ToString());  // 총 대상자
                decimal xRecCnt = Convert.ToInt32(Request.QueryString["rRes_rec_cnt"].ToString());  // 응답자
                decimal xResRate = (xRecCnt / xSumCnt) * 100;  // 설문 참여율

                Table xTB_Header = new Table();
                xTB_Header.ID = "xTB_Header";
                xTB_Header.Style.Value = tableStyle;
                xTB_Header.BorderWidth = 0;
                xTB_Header.CellPadding = 0;
                xTB_Header.CellSpacing = 1;
                xTB_Header.Style.Add(HtmlTextWriterStyle.Width, "750px");

                TableRow xTR_Header = new TableRow();
                xTR_Header.ID = "xTR_Header";

                // Table Cell 생성
                TableCell xTC_Header1 = new TableCell();
                xTC_Header1.ID = "xTC_Header1";
                xTC_Header1.Style.Add(HtmlTextWriterStyle.TextAlign, "left");

                System.Web.UI.WebControls.Label xLB_Header = new System.Web.UI.WebControls.Label();
                xLB_Header.Font.Bold = true;
                //xLB_Header.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
                xLB_Header.Text += "설문 대상인원 : " + xSumCnt.ToString() + "명, ";
                xLB_Header.Text += "설문 참여인원 : " + xRecCnt.ToString() + "명, ";
                xLB_Header.Text += "설문 참여율 : " + Convert.ToString(Math.Floor(xResRate)) + "%";
                //xLB_Header.Text += "설문 참여율 : " + string.Format("{0:P2}",xResRate) + "%";

                xTC_Header1.Controls.Add(xLB_Header);
                xTR_Header.Controls.Add(xTC_Header1);
                xTB_Header.Controls.Add(xTR_Header);
                this.ph01.Controls.Add(xTB_Header);

                for (int i = 0; i < xQueCnt; i++)
                {
                    DataTable xMasterDt = null;  // 설문문제 및 보기
                    DataTable xGridDt = null;  // Grid 
                    DataTable xChartDt = null; // 차트

                    string[] xParams = new string[2];
                    xParams[0] = Request.QueryString["rResNo"].ToString();
                    xParams[1] = Convert.ToInt32(i + 1).ToString("000");

                    xMasterDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                               "GetResearchResultChoice",
                               LMS_SYSTEM.CURRICULUM,
                               "CLT.WEB.UI.LMS.CURR",
                               (object)xParams);

                    // 차트및 그리드가 들어갈 Table 생성
                    Table xTB_Master = new Table();
                    xTB_Master.ID = "xTB_Master" + i.ToString();
                    xTB_Master.Style.Value = tableStyle;
                    xTB_Master.BorderWidth = 0;
                    xTB_Master.CellPadding = 0;
                    xTB_Master.CellSpacing = 1;
                    xTB_Master.Style.Add(HtmlTextWriterStyle.Width, "750px");

                    // Table Row 생성
                    TableRow xTR_Master01 = new TableRow();
                    xTR_Master01.ID = "xTR_Master01" + i.ToString();
                    xTR_Master01.Style.Add(HtmlTextWriterStyle.Height, "20px");


                    TableRow xTR_Master02 = new TableRow();
                    xTR_Master02.ID = "xTR_Master02" + i.ToString();

                    // Table Cell 생성
                    TableCell xTC_Master01 = new TableCell();
                    xTC_Master01.ID = "xTC_Master01" + i.ToString();
                    TableCell xTC_Master02 = new TableCell();
                    xTC_Master02.ID = "xTC_Master02" + i.ToString();
                    TableCell xTC_Master11 = new TableCell();
                    xTC_Master11.ID = "xTC_Master11" + i.ToString();
                    TableCell xTC_Master12 = new TableCell();
                    xTC_Master12.ID = "xTC_Master12" + i.ToString();

                    xTC_Master01.Style.Add(HtmlTextWriterStyle.Width, "20px");
                    xTC_Master01.Style.Value = pop_Top;
                    xTC_Master01.Width = Unit.Pixel(30);

                    xTC_Master02.Style.Add(HtmlTextWriterStyle.Height, "20px");
                    xTC_Master02.Style.Add(HtmlTextWriterStyle.Width, "725px");
                    xTC_Master02.Width = Unit.Pixel(725);
                    xTC_Master02.Style.Value = pop_Top;

                    xTC_Master11.Style.Add(HtmlTextWriterStyle.Height, "20px");
                    xTC_Master11.Style.Value = pop;

                    xTC_Master12.Style.Add(HtmlTextWriterStyle.Height, "20px");
                    xTC_Master12.Style.Value = pop;

                    // 설문문제 번호
                    xTC_Master01.HorizontalAlign = HorizontalAlign.Center;
                    xTC_Master01.Text = Convert.ToInt32(i + 1).ToString() + " 번";

                    // 설문문제 내용
                    xTC_Master02.Text = xMasterDt.Rows[0]["res_content"].ToString();

                    // 설문타입
                    xTC_Master12.Text = " 설문구분 : " + xMasterDt.Rows[0]["res_type"].ToString();


                    xTR_Master01.Controls.Add(xTC_Master01);
                    xTR_Master01.Controls.Add(xTC_Master02);

                    xTR_Master02.Controls.Add(xTC_Master11);
                    xTR_Master02.Controls.Add(xTC_Master12);

                    xTB_Master.Controls.Add(xTR_Master01);
                    xTB_Master.Controls.Add(xTR_Master02);

                    if (xMasterDt.Rows[0]["res_typecode"].ToString() != "000003") // 서술형이 아니면 차트 생성
                    {
                        // 설문 결과내용 조회
                        xGridDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                     "GetResearchResultChart",
                                      LMS_SYSTEM.CURRICULUM,
                                     "CLT.WEB.UI.LMS.CURR",
                                     (object)xParams);


                        DataTable xDtRate = new DataTable();
                        xDtRate = xGridDt;
                        int nCnt = 0;
                        foreach (DataRow xDr in xGridDt.Rows)
                        {
                            int xTotCnt = 0;  // 해당 Rows 의 합계
                            double xRate = 0; // 백분율

                            // 보기항목의 합계를 내기위해 계산...
                            for (int j = 1; j < xGridDt.Columns.Count; j++)
                            {
                                xTotCnt = xTotCnt + Convert.ToInt32(xDr[j].ToString());
                            }

                            // 백분율 계산
                            for (int j = 1; j < xGridDt.Columns.Count; j++)
                            {
                                string xResult = string.Empty;

                                xRate = (Convert.ToDouble(xDr[j].ToString()) / Convert.ToDouble(xTotCnt)) * 100;
                                //xDtRate.Rows[nCnt][j] = Math.Round(xRate, 2); //Math.Floor(xRate);
                            }
                            nCnt++;
                        }


                        AddGrid(xTB_Master, xParams, xMasterDt.Rows[0]["res_typecode"].ToString(), i, xGridDt); // Grid 출력
                        if (xMasterDt.Rows[0]["res_typecode"].ToString() != "000003") // 서술형이 아니면
                            AddChart(xTB_Master, xParams, i, xGridDt, xMasterDt.Rows[0]["res_content"].ToString());


                    }

                    this.ph01.Controls.Add(xTB_Master);
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
        * Function name : AddChart 
        * Purpose       : 웹페이지 설문결과 서식 차트 생성 메서드

        * Input         : Table rTB_Master, string[] rParams, int i, DataTable xDt, string rContent
         *                PlaceHolder Table, 검색조건 Paramater, 결과값 Table, 설문문제 
        * Output        : void
        *************************************************************/
        #region AddChart
        public void AddChart(Table rTB_Master, string[] rParams, int i, DataTable xDt, string rContent)
        {
            try
            {
                TableRow xTR_Chart = new TableRow();
                xTR_Chart.ID = "xTR_Chart" + i.ToString();

                TableCell xTC_Chart00 = new TableCell();
                xTC_Chart00.ID = "xTC_Chart00" + i.ToString();
                TableCell xTC_Chart = new TableCell();
                xTC_Chart.ID = "xTC_Chart" + i.ToString();

                C1WebChart c1Chart = new C1WebChart();

                // 차트 스타일 설정
                //c1Chart1.Style.Border.BorderStyle = BorderStyleEnum.InsetBevel;
                c1Chart.Width = Unit.Percentage(80);
                c1Chart.Height = Unit.Pixel(150);
                c1Chart.BackColor = Color.CadetBlue;


                // Add in the header
                Title hdr = c1Chart.Header;
                hdr.Text = rContent;
                //hdr.Style.Font = new Font("Arial Black", 16);
                hdr.Style.BackColor = Color.Tan;
                hdr.Style.Border.BorderStyle = BorderStyleEnum.RaisedBevel;
                hdr.Style.Border.Thickness = 4;

                //// Add in the footer
                //Title ftr = c1Chart1.Footer;
                //ftr.Text = "Nowhere";
                //ftr.Style.Font = new Font("Arial Narrow", 12, FontStyle.Bold);
                //ftr.LocationDefault = new Point(10, -1);


                //// Setup the legend.
                //Legend lgd = c1Chart1.Legend;
                //lgd.Compass = CompassEnum.East;
                //lgd.Style.Border.BorderStyle = BorderStyleEnum.RaisedBevel;
                //lgd.Style.Border.Color = Color.CadetBlue;
                //lgd.Style.Border.Thickness = 4;
                //lgd.Style.Font = new Font("Arial Narrow", 10);
                //lgd.Style.HorizontalAlignment = AlignHorzEnum.Center;
                //lgd.Text = "Series";
                //lgd.Visible = true;



                xTC_Chart.Controls.Add(c1Chart);
                xTR_Chart.Controls.Add(xTC_Chart00);
                xTR_Chart.Controls.Add(xTC_Chart);
                rTB_Master.Controls.Add(xTR_Chart);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        /************************************************************
        * Function name : AddGrid
        * Purpose       : 웹페이지 설문결과 서식 그리드 생성 메서드

        * Input         : string[] rParams, string rResType
        * Output        : void
        *************************************************************/
        #region AddGrid
        public void AddGrid(Table rTB_Master, string[] rParams, string rResType, int i, DataTable xGridDt)
        {
            try
            {
                if (rResType != "000003") // 서술형이 아니면
                {

                    TableRow xTR_Grid = new TableRow();
                    xTR_Grid.ID = "xTR_Grid" + i.ToString();

                    TableRow xTR_Grid00 = new TableRow();
                    xTR_Grid00.ID = "xTR_Grid00" + i.ToString();
                    xTR_Grid00.Style.Add(HtmlTextWriterStyle.Height, "10px");

                    TableCell xTC_Grid00 = new TableCell();
                    xTC_Grid00.ID = "xTC_Grid00" + i.ToString();

                    TableCell xTC_Grid = new TableCell();
                    xTC_Grid.ID = "xTC_Grid" + i.ToString();

                    xTR_Grid.Style.Add(HtmlTextWriterStyle.Height, "15px");

                    xTC_Grid00.Style.Value = pop;
                    xTC_Grid.Style.Value = pop;

                    C1WebGrid xGrid = new C1WebGrid();
                    xGrid.Width = Unit.Percentage(98);
                    xGrid.AllowColSizing = true;
                    xGrid.CssClass = grid_main;
                    xGrid.AutoGenerateColumns = false;
                    xGrid.HeaderStyle.Font.Bold = true;
                    xGrid.HeaderStyle.CssClass = grid_list_top;
                    xGrid.ItemStyle.CssClass = grid_list_evenbody;
                    xGrid.ItemStyle.Wrap = true;
                    xGrid.AlternatingItemStyle.CssClass = grid_list_evenbody;

                    C1BoundColumn colDutyStep = new C1BoundColumn();
                    colDutyStep.DataField = "직급";
                    colDutyStep.HeaderText = "직급";
                    colDutyStep.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colDutyStep.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

                    //colDutyStep.ItemStyle.Width = Unit.Percentage(20);

                    xGrid.Columns.Add(colDutyStep);
                    for (int j = 1; j < xGridDt.Columns.Count; j++)
                    {
                        if (j == 1)
                        {
                            C1BoundColumn col1 = new C1BoundColumn();
                            col1.DataField = xGridDt.Columns[j].ToString();

                            col1.HeaderText = xGridDt.Columns[j].ToString();
                            col1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col1.ItemStyle.Width = Unit.Percentage(20);

                            xGrid.Columns.Add(col1);
                        }
                        else if (j == 2)
                        {
                            C1BoundColumn col2 = new C1BoundColumn();
                            col2.DataField = xGridDt.Columns[j].ToString();

                            col2.HeaderText = xGridDt.Columns[j].ToString();
                            col2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col2.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col2.ItemStyle.Width = Unit.Percentage(20);

                            xGrid.Columns.Add(col2);
                        }
                        else if (j == 3)
                        {
                            C1BoundColumn col3 = new C1BoundColumn();
                            col3.DataField = xGridDt.Columns[j].ToString();

                            col3.HeaderText = xGridDt.Columns[j].ToString();
                            col3.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col3.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col3.ItemStyle.Width = Unit.Percentage(20);

                            xGrid.Columns.Add(col3);
                        }
                        else if (j == 4)
                        {
                            C1BoundColumn col4 = new C1BoundColumn();
                            col4.DataField = xGridDt.Columns[j].ToString();

                            col4.HeaderText = xGridDt.Columns[j].ToString();
                            col4.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col4.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col4.ItemStyle.Width = Unit.Percentage(20);

                            xGrid.Columns.Add(col4);
                        }
                        else if (j == 5)
                        {
                            C1BoundColumn col5 = new C1BoundColumn();
                            col5.DataField = xGridDt.Columns[j].ToString();

                            col5.HeaderText = xGridDt.Columns[j].ToString();
                            col5.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col5.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col5.ItemStyle.Width = Unit.Percentage(20);

                            xGrid.Columns.Add(col5);
                        }
                        else if (j == 6)
                        {
                            C1BoundColumn col6 = new C1BoundColumn();
                            col6.DataField = xGridDt.Columns[j].ToString();

                            col6.HeaderText = xGridDt.Columns[j].ToString();
                            col6.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col6.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col6.ItemStyle.Width = Unit.Percentage(20);

                            xGrid.Columns.Add(col6);
                        }
                        else if (j == 7)
                        {
                            C1BoundColumn col7 = new C1BoundColumn();
                            col7.DataField = xGridDt.Columns[j].ToString();

                            col7.HeaderText = xGridDt.Columns[j].ToString();
                            col7.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col7.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col7.ItemStyle.Width = Unit.Percentage(20);

                            xGrid.Columns.Add(col7);
                        }

                        xGrid.DataSource = xGridDt;
                        xGrid.DataBind();

                        xTC_Grid.Controls.Add(xGrid);
                        xTR_Grid.Controls.Add(xTC_Grid00);
                        xTR_Grid.Controls.Add(xTC_Grid);
                        rTB_Master.Controls.Add(xTR_Grid);
                        rTB_Master.Controls.Add(xTR_Grid00);

                    }

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
        * Function name : AddContent 
        * Purpose       : 웹페이지 설문결과 서식 동적컨트롤 생성 메서드
        * Input         : string rResQueCnt
        * Output        : void
        *************************************************************/
        #region AddContent
        public void AddContent(string rResQueCnt)
        {
            int xCount = Convert.ToInt32(rResQueCnt); // 설문 항목수(문제갯수)

            // 설문 문제만큼 TR, TD 생성
            for (int i = 0; i < xCount; i++)
            {
                DataTable xMasterDt = null;
                DataTable xDetailDt = null;
                string[] xParams = new string[2];
                xParams[0] = Request.QueryString["rResNo"].ToString();
                xParams[1] = Convert.ToInt32(i + 1).ToString("000");

                xMasterDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                           "GetResearchResultChoice",
                           LMS_SYSTEM.CURRICULUM,
                           "CLT.WEB.UI.LMS.CURR",
                           (object)xParams);

                Table xTB_Master = new Table();
                xTB_Master.ID = "xTB_Master" + i.ToString();
                xTB_Master.Style.Value = tableStyle;
                xTB_Master.BorderWidth = 0;
                xTB_Master.CellPadding = 0;
                xTB_Master.CellSpacing = 1;
                xTB_Master.Style.Add(HtmlTextWriterStyle.Width, "750px");

                TableRow xTR_Master01 = new TableRow();
                xTR_Master01.ID = "xTR_Master01" + i.ToString();

                TableRow xTR_Master02 = new TableRow();
                xTR_Master02.ID = "xTR_Master02" + i.ToString();

                TableCell xTC_Master01 = new TableCell();
                xTC_Master01.ID = "xTC_Master01" + i.ToString();
                TableCell xTC_Master02 = new TableCell();
                xTC_Master02.ID = "xTC_Master02" + i.ToString();
                TableCell xTC_Master11 = new TableCell();
                xTC_Master11.ID = "xTC_Master11" + i.ToString();
                TableCell xTC_Master12 = new TableCell();
                xTC_Master12.ID = "xTC_Master12" + i.ToString();


                xTR_Master01.Style.Add(HtmlTextWriterStyle.Height, "20px");


                xTC_Master01.Style.Add(HtmlTextWriterStyle.Width, "35px");
                xTC_Master01.Style.Value = pop_Top;
                xTC_Master01.Width = Unit.Pixel(35);

                xTC_Master02.Style.Add(HtmlTextWriterStyle.Height, "20px");
                xTC_Master02.Style.Add(HtmlTextWriterStyle.Width, "725px");
                xTC_Master02.Width = Unit.Pixel(725);
                xTC_Master02.Style.Value = pop_Top;


                xTC_Master11.Style.Add(HtmlTextWriterStyle.Height, "20px");
                xTC_Master11.Style.Value = pop;

                xTC_Master12.Style.Add(HtmlTextWriterStyle.Height, "20px");
                xTC_Master12.Style.Value = pop;



                // 설문문제 번호
                xTC_Master01.HorizontalAlign = HorizontalAlign.Center;
                xTC_Master01.Text = "No." + Convert.ToInt32(i + 1).ToString() + ".";


                // 설문문제 내용
                xTC_Master02.Text = xMasterDt.Rows[0]["res_content"].ToString();


                // 설문타입
                if (IsSettingKorean())
                    xTC_Master12.Text = " 설문구분 : " + xMasterDt.Rows[0]["res_type"].ToString();
                else
                    xTC_Master12.Text = " Survey Type : " + xMasterDt.Rows[0]["res_type"].ToString();


                int xSeq = xMasterDt.Rows.Count;

                // 
                xTR_Master01.Controls.Add(xTC_Master01);
                xTR_Master01.Controls.Add(xTC_Master02);

                xTR_Master02.Controls.Add(xTC_Master11);
                xTR_Master02.Controls.Add(xTC_Master12);

                xTB_Master.Controls.Add(xTR_Master01);
                xTB_Master.Controls.Add(xTR_Master02);

                //for (int j = 0; j < xSeq; j++)
                //{
                //    if (xMasterDt.Rows[0]["res_typecode"].ToString() == "000003")
                //        break;

                //    TableRow xTR_Detail = new TableRow();
                //    xTR_Detail.ID = "xTR_Detail" + i.ToString() + j.ToString();
                //    TableCell xTC_Detail01 = new TableCell();
                //    xTC_Detail01.ID = "xTC_Detail01" + i.ToString() + j.ToString();
                //    TableCell xTC_Detail02 = new TableCell();
                //    xTC_Detail02.ID = "xTC_Detail02" + i.ToString() + j.ToString();

                //    xTR_Detail.Style.Add(HtmlTextWriterStyle.Height, "15px");

                //    xTC_Detail01.Style.Add(HtmlTextWriterStyle.Height, "15px");
                //    xTC_Detail01.Style.Value = pop;

                //    xTC_Detail02.Style.Add(HtmlTextWriterStyle.Height, "15px");
                //    xTC_Detail02.Style.Value = pop;


                //    xTC_Detail02.Text = " " + Convert.ToInt32(j + 1).ToString() + ". " + xMasterDt.Rows[j]["example"].ToString();

                //    xTR_Detail.Controls.Add(xTC_Detail01);
                //    xTR_Detail.Controls.Add(xTC_Detail02);
                //    xTB_Master.Controls.Add(xTR_Detail);
                //}



                TableRow xTR_Grid = new TableRow();
                xTR_Grid.ID = "xTR_Grid" + i.ToString();

                TableCell xTC_Grid00 = new TableCell();
                xTC_Grid00.ID = "xTC_Grid00" + i.ToString();

                TableCell xTC_Grid = new TableCell();
                xTC_Grid.ID = "xTC_Grid" + i.ToString();

                xTR_Grid.Style.Add(HtmlTextWriterStyle.Height, "15px");

                xTC_Grid00.Style.Value = pop;
                xTC_Grid.Style.Value = pop;


                xDetailDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                             "GetResearchResultChart",
                              LMS_SYSTEM.CURRICULUM,
                             "CLT.WEB.UI.LMS.CURR",
                             (object)xParams);

                if (xMasterDt.Rows[0]["res_typecode"].ToString() != "000003" && xMasterDt.Rows[0]["res_typecode"].ToString() != "000006")  // 서술형이 아닐 경우만 차트와 그리드 생성
                {

                    TableRow xTR_Chart = new TableRow();
                    xTR_Chart.ID = "xTR_Chart" + i.ToString();

                    TableCell xTC_Chart00 = new TableCell();
                    xTC_Chart00.ID = "xTC_Chart00" + i.ToString();
                    TableCell xTC_Chart = new TableCell();
                    xTC_Chart.ID = "xTC_Chart" + i.ToString();

                    xTR_Chart.Style.Add(HtmlTextWriterStyle.Height, "150px");


                    xTC_Chart00.Style.Add(HtmlTextWriterStyle.Height, "150px");
                    xTC_Chart00.Style.Value = pop;

                    xTC_Chart.Style.Add(HtmlTextWriterStyle.Height, "150px");
                    xTC_Chart.Style.Value = pop;
                    //xTC_Chart.RowSpan = 2;

                    C1WebChart WebChart = new C1WebChart();


                    WebChart.ID = "WebChart" + i.ToString();
                    WebChart.Width = Unit.Percentage(98);
                    WebChart.Height = Unit.Pixel(230);
                    WebChart.BackColor = Color.White;
                    WebChart.ChartGroups[0].ChartType = Chart2DTypeEnum.Bar;

                    DataView xDv = new DataView(xDetailDt);

                    WebChart.DataSource = xDetailDt;
                    WebChart.DataBind();

                    ChartDataSeriesCollection dsc = WebChart.ChartGroups[0].ChartData.SeriesList;
                    dsc.Clear();
                    ChartDataSeries ds = dsc.AddNewSeries();
                    ds.FillStyle.Color1 = Color.LightSteelBlue;
                    ds.FillStyle.Color2 = Color.Aqua;
                    ds.FillStyle.HatchStyle = HatchStyleEnum.Sphere;
                    ds.X.DataField = "seq";
                    ds.Y.DataField = "count";

                    //double xSum = 0;
                    //foreach (DataRow xDr in xDetailDt.Rows)
                    //{
                    //    xSum = xSum + Convert.ToDouble(xDr["count"].ToString());
                    //}

                    Axis ay = WebChart.ChartArea.AxisY;

                    ay.Min = 0;
                    ay.Max = Convert.ToDouble(Request.QueryString["rRes_rec_cnt"].ToString());


                    ds = dsc.AddNewSeries();

                    WebChart.ChartGroups[0].ChartType = Chart2DTypeEnum.Bar;
                    WebChart.ChartGroups[0].Stacked = true;

                    xTC_Chart.Controls.Add(WebChart);
                    xTR_Chart.Controls.Add(xTC_Chart00);
                    xTR_Chart.Controls.Add(xTC_Chart);
                    xTB_Master.Controls.Add(xTR_Chart);
                }


                C1WebGrid WebGrid = new C1WebGrid();
                WebGrid.Width = Unit.Percentage(98);

                //WebGrid.AllowSorting = true;
                //WebGrid.AllowColSizing = true;

                if (xMasterDt.Rows[0]["res_typecode"].ToString() != "000003" && xMasterDt.Rows[0]["res_typecode"].ToString() != "000006")  // 서술형이 아닐 경우만 해당그리드 생성
                {
                    C1BoundColumn colSeq = new C1BoundColumn();

                    if (IsSettingKorean())
                        colSeq.HeaderText = "보기순번";
                    else
                        colSeq.HeaderText = "Answer SEQ";

                    colSeq.DataField = "seq";
                    colSeq.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colSeq.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    colSeq.ItemStyle.Width = Unit.Percentage(20);


                    C1BoundColumn colExample = new C1BoundColumn();

                    if (IsSettingKorean())
                        colExample.HeaderText = "보기문항";
                    else
                        colExample.HeaderText = "View Question";

                    colExample.DataField = "example";
                    colExample.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colExample.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    colExample.ItemStyle.Width = Unit.Percentage(60);


                    C1BoundColumn colCount = new C1BoundColumn();

                    if (IsSettingKorean())
                        colCount.HeaderText = "응답자";
                    else
                        colCount.HeaderText = "Answer";

                    colCount.DataField = "count";
                    colCount.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colCount.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

                    WebGrid.Columns.Add(colSeq);
                    WebGrid.Columns.Add(colExample);
                    WebGrid.Columns.Add(colCount);


                }
                else if (xMasterDt.Rows[0]["res_typecode"].ToString() == "000003")
                {

                    xDetailDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                                                 "GetResearchResultDescription",
                                                  LMS_SYSTEM.CURRICULUM,
                                                 "CLT.WEB.UI.LMS.CURR",
                                                 (object)xParams);

                    C1BoundColumn colUserID = new C1BoundColumn();

                    if (IsSettingKorean())
                        colUserID.HeaderText = "설문자";
                    else
                        colUserID.HeaderText = "User";

                    colUserID.DataField = "user_id";
                    colUserID.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colUserID.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    colUserID.ItemStyle.Width = Unit.Percentage(20);

                    C1BoundColumn colExplain = new C1BoundColumn();

                    if (IsSettingKorean())
                        colExplain.HeaderText = "설문답변";
                    else
                        colExplain.HeaderText = "Answer";

                    colExplain.DataField = "explain";
                    colExplain.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colExplain.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    colExplain.ItemStyle.Width = Unit.Percentage(80);

                    WebGrid.Columns.Add(colUserID);
                    WebGrid.Columns.Add(colExplain);
                }
                else if (xMasterDt.Rows[0]["res_typecode"].ToString() == "000006") // 순서배열이면...
                {
                    xDetailDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.CURR.vp_m_survey_md",
                             "GetResearchResultArray",
                              LMS_SYSTEM.CURRICULUM,
                             "CLT.WEB.UI.LMS.CURR",
                             (object)xParams);

                    //TableRow xTR_Chart = new TableRow();
                    //xTR_Chart.ID = "xTR_Chart" + i.ToString();

                    //TableCell xTC_Chart00 = new TableCell();
                    //xTC_Chart00.ID = "xTC_Chart00" + i.ToString();
                    //TableCell xTC_Chart = new TableCell();
                    //xTC_Chart.ID = "xTC_Chart" + i.ToString();

                    //xTR_Chart.Style.Add(HtmlTextWriterStyle.Height, "150px");


                    //xTC_Chart00.Style.Add(HtmlTextWriterStyle.Height, "150px");
                    //xTC_Chart00.Style.Value = pop;

                    //xTC_Chart.Style.Add(HtmlTextWriterStyle.Height, "150px");
                    //xTC_Chart.Style.Value = pop;

                    //C1WebChart WebChart = new C1WebChart();


                    //WebChart.ID = "WebChart" + i.ToString();
                    //WebChart.Width = Unit.Percentage(98);
                    //WebChart.Height = Unit.Pixel(230);
                    //WebChart.BackColor = Color.White;
                    //WebChart.ChartGroups[0].ChartType = Chart2DTypeEnum.Bar;
                    //WebChart.ChartGroups[0].Stacked = true;

                    //DataView xDv = new DataView(xDetailDt);

                    //WebChart.DataSource = xDetailDt;
                    //WebChart.DataBind();

                    //ChartDataSeriesCollection dsc = WebChart.ChartGroups[0].ChartData.SeriesList;
                    //dsc.Clear();
                    //ChartDataSeries ds = dsc.AddNewSeries();
                    //ds.FillStyle.Color1 = Color.LightSteelBlue;
                    //ds.FillStyle.Color2 = Color.Aqua;
                    //ds.FillStyle.HatchStyle = HatchStyleEnum.Sphere;
                    //ds.X.DataField = "seq";

                    //  //ds.Y.DataField = "count";

                    ////double xSum = 0;
                    ////foreach (DataRow xDr in xDetailDt.Rows)
                    ////{
                    ////    xSum = xSum + Convert.ToDouble(xDr["count"].ToString());
                    ////}

                    ////Axis ay = WebChart.ChartArea.AxisY;
                    //int k = 0;
                    //foreach (DataRow xDrs in xDetailDt.Rows)
                    //{
                    //    //Array[] axList = new Array[xDetailDt.Rows.Count];
                    //    //ArrayList alList = new ArrayList();
                    //    int[] alList = new int[xDetailDt.Rows.Count];
                    //    for (int j = 1; j < xDetailDt.Columns.Count; j++)
                    //    {
                    //        alList[k]  = Convert.ToInt32(xDrs[j].ToString());
                    //        //alList.Add(Convert.ToDouble(xDrs[j].ToString()));
                    //        //ds.Y.Add(Convert.ToDouble(xDrs[j].ToString()));
                    //        //ds.Y.DataField = xDetailDt.Columns[j].Caption;
                    //    }
                    //    ds.Y.Add(alList);
                    //    k++;
                    //    //ds.Y.Add(
                    //}


                    //Axis ay = WebChart.ChartArea.AxisY;
                    //ay.Min = 0;
                    //ay.Max = Convert.ToDouble(Request.QueryString["rRes_rec_cnt"].ToString());




                    //WebChart.ChartGroups[0].ChartType = Chart2DTypeEnum.Bar;
                    //WebChart.ChartGroups[0].Stacked = true;

                    //xTC_Chart.Controls.Add(WebChart);
                    //xTR_Chart.Controls.Add(xTC_Chart00);
                    //xTR_Chart.Controls.Add(xTC_Chart);
                    //xTB_Master.Controls.Add(xTR_Chart);

                    C1BoundColumn colSeq = new C1BoundColumn();
                    colSeq.DataField = xDetailDt.Columns[0].ToString();
                    if (IsSettingKorean())
                        colSeq.HeaderText = "응답수";
                    else
                        colSeq.HeaderText = "Count";

                    colSeq.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colSeq.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    WebGrid.Columns.Add(colSeq);


                    for (int j = 1; j < xDetailDt.Columns.Count; j++)
                    {
                        if (j == 1)
                        {
                            C1BoundColumn col1 = new C1BoundColumn();
                            col1.DataField = xDetailDt.Columns[j].ToString();

                            col1.HeaderText = xDetailDt.Columns[j].ToString();
                            col1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col1.ItemStyle.Width = Unit.Percentage(20);

                            WebGrid.Columns.Add(col1);
                        }
                        else if (j == 2)
                        {
                            C1BoundColumn col2 = new C1BoundColumn();
                            col2.DataField = xDetailDt.Columns[j].ToString();

                            col2.HeaderText = xDetailDt.Columns[j].ToString();
                            col2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col2.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col2.ItemStyle.Width = Unit.Percentage(20);

                            WebGrid.Columns.Add(col2);
                        }
                        else if (j == 3)
                        {
                            C1BoundColumn col3 = new C1BoundColumn();
                            col3.DataField = xDetailDt.Columns[j].ToString();

                            col3.HeaderText = xDetailDt.Columns[j].ToString();
                            col3.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col3.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col3.ItemStyle.Width = Unit.Percentage(20);

                            WebGrid.Columns.Add(col3);
                        }
                        else if (j == 4)
                        {
                            C1BoundColumn col4 = new C1BoundColumn();
                            col4.DataField = xDetailDt.Columns[j].ToString();

                            col4.HeaderText = xDetailDt.Columns[j].ToString();
                            col4.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col4.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col4.ItemStyle.Width = Unit.Percentage(20);

                            WebGrid.Columns.Add(col4);
                        }
                        else if (j == 5)
                        {
                            C1BoundColumn col5 = new C1BoundColumn();
                            col5.DataField = xDetailDt.Columns[j].ToString();

                            col5.HeaderText = xDetailDt.Columns[j].ToString();
                            col5.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col5.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col5.ItemStyle.Width = Unit.Percentage(20);

                            WebGrid.Columns.Add(col5);
                        }
                        else if (j == 6)
                        {
                            C1BoundColumn col6 = new C1BoundColumn();
                            col6.DataField = xDetailDt.Columns[j].ToString();

                            col6.HeaderText = xDetailDt.Columns[j].ToString();
                            col6.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col6.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col6.ItemStyle.Width = Unit.Percentage(20);

                            WebGrid.Columns.Add(col6);
                        }
                        else if (j == 7)
                        {
                            C1BoundColumn col7 = new C1BoundColumn();
                            col7.DataField = xDetailDt.Columns[j].ToString();

                            col7.HeaderText = xDetailDt.Columns[j].ToString();
                            col7.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            col7.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            //col7.ItemStyle.Width = Unit.Percentage(20);

                            WebGrid.Columns.Add(col7);
                        }
                    }

                }


                WebGrid.HeaderStyle.Height = Unit.Pixel(22);
                WebGrid.HeaderStyle.BackColor = ColorTranslator.FromHtml("#d1d5f0");

                WebGrid.Style.Value = iGrid;

                WebGrid.Width = Unit.Pixel(600);
                WebGrid.AlternatingItemStyle.HorizontalAlign = HorizontalAlign.Center;
                WebGrid.AlternatingItemStyle.BorderWidth = 1;



                WebGrid.AutoGenerateColumns = false;
                WebGrid.DataSource = xDetailDt;
                WebGrid.DataBind();

                xTC_Grid.Controls.Add(WebGrid);
                xTR_Grid.Controls.Add(xTC_Grid00);
                xTR_Grid.Controls.Add(xTC_Grid);
                xTB_Master.Controls.Add(xTR_Grid);

                TableRow xTR_Master03 = new TableRow();
                xTR_Master02.ID = "xTR_Master03" + i.ToString();


                TableCell xTC_Master04 = new TableCell();
                xTC_Master04.ID = "xTC_Master04" + i.ToString();
                xTC_Master04.ColumnSpan = 2;
                xTC_Master04.Text = "1";
                xTC_Master04.Font.Size = 0;
                xTC_Master04.Style.Value = "height:7px;border-bottom:#C5D3DE solid 1px;";

                xTR_Master03.Controls.Add(xTC_Master04);
                xTB_Master.Controls.Add(xTR_Master03);

                this.ph01.Controls.Add(xTB_Master);

            }
        }
        #endregion
        
        ///************************************************************
        //* Function name : AddContent 
        //* Purpose       : 웹페이지 설문결과 서식 동적컨트롤 생성 메서드


        //* Input         : string rResQueCnt
        //* Output        : void
        //*************************************************************/
        //#region public void AddContent(string rResQueCnt)
        //public void AddContent(string rResQueCnt)
        //{
        //    int xCount = Convert.ToInt32(rResQueCnt); // 설문 항목수(문제갯수)

        //    // 설문 문제만큼 TR, TD 생성
        //    for (int i = 0; i < xCount; i++)
        //    {
        //        DataTable xMasterDt = null;
        //        DataTable xDetailDt = null;
        //        string[] xParams = new string[2];
        //        xParams[0] = Request.QueryString["rResNo"].ToString();
        //        xParams[1] = Convert.ToInt32(i + 1).ToString("000");

        //        xMasterDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_survey_md",
        //                   "GetResearchResultChoice",
        //                   LMS_SYSTEM.MANAGE,
        //                   "CLT.WEB.UI.LMS.MANAGE.vp_m_manage_survey_list_wpg",
        //                   (object)xParams);

        //        Table xTB_Master = new Table();
        //        xTB_Master.ID = "xTB_Master" + i.ToString();
        //        xTB_Master.Style.Value = tableStyle;
        //        xTB_Master.BorderWidth = 0;
        //        xTB_Master.CellPadding = 0;
        //        xTB_Master.CellSpacing = 1;
        //        xTB_Master.Style.Add(HtmlTextWriterStyle.Width, "750px");

        //        TableRow xTR_Master01 = new TableRow();
        //        xTR_Master01.ID = "xTR_Master01" + i.ToString();

        //        TableRow xTR_Master02 = new TableRow();
        //        xTR_Master02.ID = "xTR_Master02" + i.ToString();

        //        TableCell xTC_Master01 = new TableCell();
        //        xTC_Master01.ID = "xTC_Master01" + i.ToString();
        //        TableCell xTC_Master02 = new TableCell();
        //        xTC_Master02.ID = "xTC_Master02" + i.ToString();
        //        TableCell xTC_Master11 = new TableCell();
        //        xTC_Master11.ID = "xTC_Master11" + i.ToString();
        //        TableCell xTC_Master12 = new TableCell();
        //        xTC_Master12.ID = "xTC_Master12" + i.ToString();


        //        xTR_Master01.Style.Add(HtmlTextWriterStyle.Height, "20px");


        //        xTC_Master01.Style.Add(HtmlTextWriterStyle.Width, "25px");
        //        xTC_Master01.Style.Value = pop_Top;
        //        xTC_Master01.Width = Unit.Pixel(25); 

        //        xTC_Master02.Style.Add(HtmlTextWriterStyle.Height, "20px");
        //        xTC_Master02.Style.Add(HtmlTextWriterStyle.Width, "725px");
        //        xTC_Master02.Width = Unit.Pixel(725);
        //        xTC_Master02.Style.Value = pop_Top;


        //        xTC_Master11.Style.Add(HtmlTextWriterStyle.Height, "20px");
        //        xTC_Master11.Style.Value = pop;

        //        xTC_Master12.Style.Add(HtmlTextWriterStyle.Height, "20px");
        //        xTC_Master12.Style.Value = pop;



        //        // 설문문제 번호
        //        xTC_Master01.HorizontalAlign = HorizontalAlign.Center;
        //        xTC_Master01.Text = Convert.ToInt32(i + 1).ToString() + " 번";


        //        // 설문문제 내용
        //        xTC_Master02.Text = xMasterDt.Rows[0]["res_content"].ToString();


        //        // 설문타입

        //        if (IsSettingKorean())
        //            xTC_Master12.Text = " 설문구분 : " + xMasterDt.Rows[0]["res_type"].ToString();
        //        else
        //            xTC_Master12.Text = " Survey Type : " + xMasterDt.Rows[0]["res_type"].ToString();


        //        int xSeq = xMasterDt.Rows.Count;

        //        // 
        //        xTR_Master01.Controls.Add(xTC_Master01);
        //        xTR_Master01.Controls.Add(xTC_Master02);

        //        xTR_Master02.Controls.Add(xTC_Master11);
        //        xTR_Master02.Controls.Add(xTC_Master12);

        //        xTB_Master.Controls.Add(xTR_Master01);
        //        xTB_Master.Controls.Add(xTR_Master02);

        //        //for (int j = 0; j < xSeq; j++)
        //        //{
        //        //    if (xMasterDt.Rows[0]["res_typecode"].ToString() == "000003")
        //        //        break;

        //        //    TableRow xTR_Detail = new TableRow();
        //        //    xTR_Detail.ID = "xTR_Detail" + i.ToString() + j.ToString();
        //        //    TableCell xTC_Detail01 = new TableCell();
        //        //    xTC_Detail01.ID = "xTC_Detail01" + i.ToString() + j.ToString();
        //        //    TableCell xTC_Detail02 = new TableCell();
        //        //    xTC_Detail02.ID = "xTC_Detail02" + i.ToString() + j.ToString();

        //        //    xTR_Detail.Style.Add(HtmlTextWriterStyle.Height, "15px");

        //        //    xTC_Detail01.Style.Add(HtmlTextWriterStyle.Height, "15px");
        //        //    xTC_Detail01.Style.Value = pop;

        //        //    xTC_Detail02.Style.Add(HtmlTextWriterStyle.Height, "15px");
        //        //    xTC_Detail02.Style.Value = pop;


        //        //    xTC_Detail02.Text = " " + Convert.ToInt32(j + 1).ToString() + ". " + xMasterDt.Rows[j]["example"].ToString();

        //        //    xTR_Detail.Controls.Add(xTC_Detail01);
        //        //    xTR_Detail.Controls.Add(xTC_Detail02);
        //        //    xTB_Master.Controls.Add(xTR_Detail);
        //        //}



        //        TableRow xTR_Grid = new TableRow();
        //        xTR_Grid.ID = "xTR_Grid" + i.ToString();

        //        TableCell xTC_Grid00 = new TableCell();
        //        xTC_Grid00.ID = "xTC_Grid00" + i.ToString();

        //        TableCell xTC_Grid = new TableCell();
        //        xTC_Grid.ID = "xTC_Grid" + i.ToString();

        //        xTR_Grid.Style.Add(HtmlTextWriterStyle.Height, "15px");

        //        xTC_Grid00.Style.Value = pop;
        //        xTC_Grid.Style.Value = pop;


        //        xDetailDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_survey_md",
        //                     "GetResearchResultChart",
        //                      LMS_SYSTEM.MANAGE,
        //                     "CLT.WEB.UI.LMS.MANAGE.vp_m_manage_survey_list_wpg",
        //                     (object)xParams);

        //        if (xMasterDt.Rows[0]["res_typecode"].ToString() != "000003")  // 서술형이 아닐 경우만 차트와 그리드 생성
        //        {
        //            /*
        //            TableRow xTR_Chart = new TableRow();
        //            xTR_Chart.ID = "xTR_Chart" + i.ToString();

        //            TableCell xTC_Chart00 = new TableCell();
        //            xTC_Chart00.ID = "xTC_Chart00" + i.ToString();
        //            TableCell xTC_Chart = new TableCell();
        //            xTC_Chart.ID = "xTC_Chart" + i.ToString();

        //            xTR_Chart.Style.Add(HtmlTextWriterStyle.Height, "180px");


        //            xTC_Chart00.Style.Add(HtmlTextWriterStyle.Height, "180px");
        //            xTC_Chart00.Style.Value = pop;

        //            xTC_Chart.Style.Add(HtmlTextWriterStyle.Height, "180px");
        //            xTC_Chart.Style.Value = pop;
        //            //xTC_Chart.RowSpan = 2;

        //            C1WebChart WebChart = new C1WebChart();


        //            WebChart.ID = "WebChart" + i.ToString();
        //            WebChart.Width = Unit.Percentage(98);
        //            WebChart.BackColor = Color.White;

        //            WebChart.ChartGroups[0].ChartType = Chart2DTypeEnum.Bar;

        //            //ChartDataSeries dscoll = WebChart.ChartGroups[0].ChartData.SeriesList;


        //            // 차트 스타일 설정



        //            DataView xDv = new DataView(xDetailDt);

        //            WebChart.DataSource = xDetailDt;
        //            WebChart.DataBind();
        //            ChartDataSeriesCollection dsc = WebChart.ChartGroups[0].ChartData.SeriesList;
        //            dsc.Clear();
        //            ChartDataSeries ds = dsc.AddNewSeries();
        //            ds.FillStyle.Color1 = Color.LightSteelBlue;
        //            ds.FillStyle.Color2 = Color.Aqua;
        //            ds.FillStyle.HatchStyle = HatchStyleEnum.Sphere;
        //            //ds.X.DataField = "example";
        //            //ds.X.DataField = "seq";
        //            //ds.Y.DataField = "직급";
        //            ds = dsc.AddNewSeries();

        //            WebChart.ChartGroups[0].ChartType = Chart2DTypeEnum.Bar;
        //            WebChart.ChartGroups[0].Stacked = true;


        //            WebChart.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
        //            WebChart.ImageRenderMethod = ImageRenderMethodEnum.BinaryWrite;


        //             //차트 스타일 설정..
        //            //WebChart.ImageRenderMethod = ImageRenderMethodEnum.AspPage;

        //            //WebChart.ImageTransferMethod = ImageTransferMethodEnum.Session;
        //            //WebChart.ChartStyle.Border.BorderStyle = C1.Win.C1Chart.BorderStyleEnum.Dashed;
        //            //WebChart.ChartStyle.Border.Color = System.Drawing.Color.FromName("White");
        //            //WebChart.Header.Visible = false;
        //            //WebChart.Header.Style.Border.BorderStyle = C1.Win.C1Chart.BorderStyleEnum.Dashed;

        //            //WebChart.ChartStyle.Autowrap = false;

        //            WebChart.Height = Unit.Pixel(250);


        //            Axis ax = WebChart.ChartArea.AxisX;
        //            Axis ay = WebChart.ChartArea.AxisY;

        //            int xSumCnt = 0;
        //            foreach (DataRow xDrs in xDetailDt.Rows)
        //            {
        //                xSumCnt = xSumCnt + Convert.ToInt32(xDrs["count"].ToString());
        //            }

        //            ay.Max = Convert.ToDouble(Request.QueryString["rResSumCnt"].ToString());

        //            ay.Min = 0;
        //            ay.Max = Convert.ToDouble(xSumCnt);
        //            ay.AnnoFormat = FormatEnum.NumericGeneral;

        //            ax.Max = xDetailDt.Rows.Count;


        //            if (IsSettingKorean())
        //            {
        //                ax.Text = "설문보기";
        //                ay.Text = "응답자";
        //            }
        //            else
        //            {
        //                ax.Text = "Answer Type";
        //                ay.Text = "Answer";
        //            }

        //            xTC_Chart.Controls.Add(WebChart);
        //            xTR_Chart.Controls.Add(xTC_Chart00);
        //            xTR_Chart.Controls.Add(xTC_Chart);
        //            xTB_Master.Controls.Add(xTR_Chart);
        //            */

        //            TableRow xTR_Chart = new TableRow();
        //            xTR_Chart.ID = "xTR_Chart" + i.ToString();

        //            TableCell xTC_Chart00 = new TableCell();
        //            xTC_Chart00.ID = "xTC_Chart00" + i.ToString();
        //            TableCell xTC_Chart = new TableCell();
        //            xTC_Chart.ID = "xTC_Chart" + i.ToString();

        //            xTR_Chart.Style.Add(HtmlTextWriterStyle.Height, "150px");


        //            xTC_Chart00.Style.Add(HtmlTextWriterStyle.Height, "150px");
        //            xTC_Chart00.Style.Value = pop;

        //            xTC_Chart.Style.Add(HtmlTextWriterStyle.Height, "150px");
        //            xTC_Chart.Style.Value = pop;
        //            //xTC_Chart.RowSpan = 2;

        //            C1WebChart WebChart = new C1WebChart();
        //            WebChart.ID = "WebChart" + i.ToString();
        //            WebChart.Width = Unit.Percentage(80);
        //            WebChart.BackColor = Color.White;


        //            DataView xDv = new DataView(xDetailDt);

        //            WebChart.DataSource = xDetailDt;
        //            ChartDataSeriesCollection dsc = WebChart.ChartGroups[0].ChartData.SeriesList;
        //            dsc.Clear();
        //            ChartDataSeries ds = dsc.AddNewSeries();
        //            ds.FillStyle.Color1 = Color.LightSteelBlue;
        //            //ds.X.DataField = "example";
        //            //ds.Y.DataField = "count";
        //            ds = dsc.AddNewSeries();
        //            WebChart.ChartGroups[0].ChartType = Chart2DTypeEnum.Bar;

        //            Axis ax = WebChart.ChartArea.AxisX;
        //            Axis ay = WebChart.ChartArea.AxisY;

        //            //int xSumCnt = 0;
        //            //foreach (DataRow xDrs in xDetailDt.Rows)
        //            //{
        //            //    xSumCnt = xSumCnt + Convert.ToInt32(xDrs["count"].ToString());
        //            //}


        //            ay.Min = 0;
        //            //ay.Max = Convert.ToDouble(xSumCnt);
        //            ax.Max = xDetailDt.Rows.Count;


        //            // 차트 스타일 설정..
        //            WebChart.ChartStyle.Border.BorderStyle = C1.Win.C1Chart.BorderStyleEnum.Dashed;
        //            WebChart.ChartStyle.Border.Color = System.Drawing.Color.FromName("White");
        //            WebChart.Header.Visible = false;
        //            WebChart.Header.Style.Border.BorderStyle = C1.Win.C1Chart.BorderStyleEnum.Dashed;

        //            xTC_Chart.Controls.Add(WebChart);
        //            xTR_Chart.Controls.Add(xTC_Chart00);
        //            xTR_Chart.Controls.Add(xTC_Chart);
        //            xTB_Master.Controls.Add(xTR_Chart);
        //        }


        //        C1WebGrid WebGrid = new C1WebGrid();
        //        WebGrid.Width = Unit.Percentage(98);

        //        WebGrid.AllowSorting = true;
        //        WebGrid.AllowColSizing = true;

        //        if (xMasterDt.Rows[0]["res_typecode"].ToString() != "000003")  // 서술형이 아닐 경우만 해당그리드 생성
        //        {

        //            C1BoundColumn colDutyStep = new C1BoundColumn();
        //            colDutyStep.DataField = "직급";
        //            colDutyStep.HeaderText = "직급";
        //            colDutyStep.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //            colDutyStep.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //            //colDutyStep.ItemStyle.Width = Unit.Percentage(15);
        //            WebGrid.Columns.Add(colDutyStep);
        //            for (int j = 1; j < xDetailDt.Columns.Count; j++)
        //            {
        //                if (j == 1)
        //                {
        //                    C1BoundColumn col1 = new C1BoundColumn();
        //                    col1.DataField = xDetailDt.Columns[j].ToString();

        //                    col1.HeaderText = xDetailDt.Columns[j].ToString();
        //                    col1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    col1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    //col1.ItemStyle.Width = Unit.Percentage(20);

        //                    WebGrid.Columns.Add(col1);
        //                }
        //                else if (j == 2)
        //                {
        //                    C1BoundColumn col2 = new C1BoundColumn();
        //                    col2.DataField = xDetailDt.Columns[j].ToString();

        //                    col2.HeaderText = xDetailDt.Columns[j].ToString();
        //                    col2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    col2.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    //col2.ItemStyle.Width = Unit.Percentage(20);

        //                    WebGrid.Columns.Add(col2);
        //                }
        //                else if (j == 3)
        //                {
        //                    C1BoundColumn col3 = new C1BoundColumn();
        //                    col3.DataField = xDetailDt.Columns[j].ToString();

        //                    col3.HeaderText = xDetailDt.Columns[j].ToString();
        //                    col3.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    col3.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    //col3.ItemStyle.Width = Unit.Percentage(20);

        //                    WebGrid.Columns.Add(col3);
        //                }
        //                else if (j == 4)
        //                {
        //                    C1BoundColumn col4 = new C1BoundColumn();
        //                    col4.DataField = xDetailDt.Columns[j].ToString();

        //                    col4.HeaderText = xDetailDt.Columns[j].ToString();
        //                    col4.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    col4.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    //col4.ItemStyle.Width = Unit.Percentage(20);

        //                    WebGrid.Columns.Add(col4);
        //                }
        //                else if (j == 5)
        //                {
        //                    C1BoundColumn col5 = new C1BoundColumn();
        //                    col5.DataField = xDetailDt.Columns[j].ToString();

        //                    col5.HeaderText = xDetailDt.Columns[j].ToString();
        //                    col5.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    col5.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    //col5.ItemStyle.Width = Unit.Percentage(20);

        //                    WebGrid.Columns.Add(col5);
        //                }
        //                else if (j == 6)
        //                {
        //                    C1BoundColumn col6 = new C1BoundColumn();
        //                    col6.DataField = xDetailDt.Columns[j].ToString();

        //                    col6.HeaderText = xDetailDt.Columns[j].ToString();
        //                    col6.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    col6.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    //col6.ItemStyle.Width = Unit.Percentage(20);

        //                    WebGrid.Columns.Add(col6);
        //                }
        //                else if (j == 7)
        //                {
        //                    C1BoundColumn col7 = new C1BoundColumn();
        //                    col7.DataField = xDetailDt.Columns[j].ToString();

        //                    col7.HeaderText = xDetailDt.Columns[j].ToString();
        //                    col7.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    col7.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    //col7.ItemStyle.Width = Unit.Percentage(20);

        //                    WebGrid.Columns.Add(col7);
        //                }
        //            }


        //            //C1BoundColumn colSeq = new C1BoundColumn();

        //            //if (IsSettingKorean())
        //            //    colSeq.HeaderText = "보기순번";
        //            //else
        //            //    colSeq.HeaderText = "Answer SEQ";

        //            //colSeq.DataField = "seq";
        //            //colSeq.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //            //colSeq.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //            //colSeq.ItemStyle.Width = Unit.Percentage(20);


        //            //C1BoundColumn colExample = new C1BoundColumn();

        //            //if (IsSettingKorean())
        //            //    colExample.HeaderText = "보기문항";
        //            //else
        //            //    colExample.HeaderText = "View Question";

        //            //colExample.DataField = "example";
        //            //colExample.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //            //colExample.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //            //colExample.ItemStyle.Width = Unit.Percentage(60);


        //            //C1BoundColumn colCount = new C1BoundColumn();

        //            //if (IsSettingKorean())
        //            //    colCount.HeaderText = "응답자";
        //            //else
        //            //    colCount.HeaderText = "Answer";

        //            //colCount.DataField = "count";
        //            //colCount.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //            //colCount.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

        //            //WebGrid.Columns.Add(colSeq);
        //            //WebGrid.Columns.Add(colExample);
        //            //WebGrid.Columns.Add(colCount);


        //        }
        //        else
        //        {

        //            xDetailDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MANAGE.vp_m_survey_md",
        //                                         "GetResearchResultDescription",
        //                                          LMS_SYSTEM.MANAGE,
        //                                         "CLT.WEB.UI.LMS.MANAGE.vp_m_manage_survey_list_wpg",
        //                                         (object)xParams);

        //            C1BoundColumn colUserID = new C1BoundColumn();

        //            if (IsSettingKorean())
        //                colUserID.HeaderText = "설문자 ID";
        //            else
        //                colUserID.HeaderText = "ID";

        //            colUserID.DataField = "user_id";
        //            colUserID.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //            colUserID.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

        //            C1BoundColumn colExplain = new C1BoundColumn();

        //            if (IsSettingKorean())
        //                colExplain.HeaderText = "설문답변";
        //            else
        //                colExplain.HeaderText = "Answer";

        //            colExplain.DataField = "explain";
        //            colExplain.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //            colExplain.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

        //            WebGrid.Columns.Add(colUserID);
        //            WebGrid.Columns.Add(colExplain);
        //        }



        //        WebGrid.HeaderStyle.Height = Unit.Pixel(22);
        //        WebGrid.HeaderStyle.BackColor = ColorTranslator.FromHtml("#d1d5f0");

        //        WebGrid.Style.Value = iGrid;

        //        WebGrid.Width = Unit.Pixel(600);
        //        WebGrid.AlternatingItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //        WebGrid.AlternatingItemStyle.BorderWidth = 1;



        //        WebGrid.AutoGenerateColumns = false;
        //        WebGrid.DataSource = xDetailDt;
        //        WebGrid.DataBind();

        //        xTC_Grid.Controls.Add(WebGrid);
        //        xTR_Grid.Controls.Add(xTC_Grid00);
        //        xTR_Grid.Controls.Add(xTC_Grid);
        //        xTB_Master.Controls.Add(xTR_Grid);

        //        TableRow xTR_Master03 = new TableRow();
        //        xTR_Master02.ID = "xTR_Master03" + i.ToString();


        //        TableCell xTC_Master04 = new TableCell();
        //        xTC_Master04.ID = "xTC_Master04" + i.ToString();
        //        xTC_Master04.ColumnSpan = 2;
        //        xTC_Master04.Text = "1";
        //        xTC_Master04.Font.Size = 0;
        //        xTC_Master04.Style.Value = "height:7px;border-bottom:#C5D3DE solid 1px;";

        //        xTR_Master03.Controls.Add(xTC_Master04);
        //        xTB_Master.Controls.Add(xTR_Master03);

        //        this.ph01.Controls.Add(xTB_Master);

        //    }
        //}
        //#endregion

        #region SequencingChart
        public C1WebChart SequencingChart(DataTable xDetailDt)
        {
            C1WebChart c1Chart = new C1WebChart();
            try
            {
                // 차트 스타일 설정
                //c1Chart.Style.Border.BorderStyle = BorderStyleEnum.InsetBevel;
                c1Chart.Width = Unit.Percentage(98);
                c1Chart.Height = Unit.Pixel(250);
                c1Chart.BackColor = Color.White;


                // Add in the header
                //Title hdr = c1Chart.Header;
                //hdr.Text = rContent;
                //hdr.Style.Font = new Font("Arial Black", 16);
                //hdr.Style.BackColor = Color.Tan;
                //hdr.Style.Border.BorderStyle = BorderStyleEnum.RaisedBevel;
                //hdr.Style.Border.Thickness = 4;

                //// Add in the footer
                //Title ftr = c1Chart1.Footer;
                //ftr.Text = "Nowhere";
                //ftr.Style.Font = new Font("Arial Narrow", 12, FontStyle.Bold);
                //ftr.LocationDefault = new Point(10, -1);


                // Setup the legend.  시리즈 도표
                //Legend lgd = c1Chart.Legend;
                //lgd.Compass = CompassEnum.East;
                //lgd.Style.Border.BorderStyle = BorderStyleEnum.RaisedBevel;
                //lgd.Style.Border.Color = Color.CadetBlue;
                //lgd.Style.Border.Thickness = 4;
                //lgd.Style.Font = new Font("Arial Narrow", 10);
                //lgd.Style.HorizontalAlignment = AlignHorzEnum.Center;
                //lgd.Text = "Series";
                //lgd.Visible = true;

                // Set the Chart Area Style
                Area area = c1Chart.ChartArea;
                area.Style.Border.BorderStyle = BorderStyleEnum.None;
                area.Style.BackColor = Color.White;
                area.Style.Border.Thickness = 0;


                // Set the default label style.  By using the default style,
                // all the labels styles can be handled uniformly
                c1Chart.ChartLabels.DefaultLabelStyle.BackColor = SystemColors.Info;
                c1Chart.ChartLabels.DefaultLabelStyle.Border.BorderStyle = BorderStyleEnum.None;

                // Set up a Pie chart
                ChartGroup grp = c1Chart.ChartGroups[0];
                grp.ChartType = Chart2DTypeEnum.Pie;
                grp.Pie.OtherOffset = 0;
                //grp.Pie.Start = int.Parse(txtStartAngle.Text);

                // Clear existing, and add new data.
                ChartData dat = grp.ChartData;
                dat.SeriesList.Clear();

                // Pick a nice color for each Series.
                Color[] ColorValue = new Color[]
                    {
                        Color.LightSlateGray, Color.LightSteelBlue, Color.LightSalmon, Color.MediumTurquoise,
                        Color.CornflowerBlue, Color.LightCoral, Color.GreenYellow, //Color.MediumBlue
                    };

                int k = 0;
                foreach (DataRow xDr in xDetailDt.Rows)
                {
                    ChartDataSeries series = dat.SeriesList.AddNewSeries();
                    series.PointData.Length = 1;
                    series.PointData[0] = new PointF(1f, (float)Convert.ToDouble(xDr["count"].ToString()));
                    series.LineStyle.Color = ColorValue[k];
                    series.Label = xDr["example"].ToString();


                    // Add a chart label for each slice
                    C1.Win.C1Chart.Label lab = c1Chart.ChartLabels.LabelsCollection.AddNewLabel();
                    lab.AttachMethod = AttachMethodEnum.DataIndex;

                    AttachMethodData amd = lab.AttachMethodData;
                    amd.GroupIndex = 0;
                    amd.PointIndex = 0;
                    amd.SeriesIndex = k;

                    lab.Text = xDr["example"].ToString();
                    lab.Compass = LabelCompassEnum.Radial;
                    lab.Connected = true;
                    lab.Offset = 10;
                    lab.Visible = true; //radioLabelsOn.Checked;

                    k++;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagete Policy");
                if (rethrow) throw;
            }
            return c1Chart;
        }
        #endregion SequencingChart

    }
}