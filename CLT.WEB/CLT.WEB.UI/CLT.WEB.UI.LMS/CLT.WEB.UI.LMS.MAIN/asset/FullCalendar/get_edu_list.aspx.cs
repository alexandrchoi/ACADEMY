using System;
using System.Data;
using System.Web;
using System.Collections.Generic;

// 필수 using 문


using System.Threading;
using C1.Web.C1WebGrid;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;

namespace CLT.WEB.UI.LMS
{
    /// <summary>
    /// 1. 작업개요 : get_edu_list Class
    /// 
    /// 2. 주요기능 : 교육정보 월별 조회 처리
    ///				  
    /// 3. Class 명 : get_edu_list
    /// 
    /// 4. 작 업 자 : 최인재 / 2019.12.12
    /// </summary>
    public partial class get_edu_list : BasePage
    {
        protected string iGoUrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string[] xParams = new string[3];
                xParams[0] = Request["date"];
                xParams[1] = Session["USER_ID"].ToString();
                xParams[2] = Convert.ToString(Session["USER_GROUP"]);

                DataTable xDt = SBROKER.GetTable("CLT.WEB.BIZ.LMS.MAIN.vp_m_main_md",
                                             "GetEduList",
                                             LMS_SYSTEM.MAIN,
                                             "CLT.WEB.UI.LMS.MAIN", (object)xParams, Thread.CurrentThread.CurrentCulture);

                //DataRow[] xDrs = null;


                //string xMenuCode1 = Request["menucode1"].ToString();

                // 대분류 메뉴를 선택했을 경우, 첫번째 중분류-소분류 메뉴로 가도록 처리
                //string xMenuCode2 = (Request["menucode2"].ToString() == "0") ? "1" : Request["menucode2"].ToString();
                //string xMenuCode3 = (Request["menucode3"].ToString() == "0") ? "1" : Request["menucode3"].ToString();

                //this.iGoUrl = Request["gourl"];

                // OpenCourse 관련 DataBinding 처리
                //string[] xParams = new string[4];
                //xParams[0] = "000001"; // Count
                //xParams[1] = "7"; // new icon 일자
                //xParams[2] = Convert.ToString(Session["company_kind"]); // 

                Response.Clear();
                Response.Write(xDt.ToJson());
                Response.End();
            }
            catch (Exception ex)
            {
                base.NotifyError(ex); 
            }
        }
    }
    public static class CommonExtensions

    {

        public static string ToJson(this DataTable value)

        {

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            serializer.MaxJsonLength = 2147483647;

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            Dictionary<string, object> row;

            foreach (DataRow dr in value.Rows)

            {

                row = new Dictionary<string, object>();

                foreach (DataColumn col in value.Columns)

                {

                    row.Add(col.ColumnName, dr[col]);

                }

                rows.Add(row);

            }

            return serializer.Serialize(rows);

        }



        public static string ToJson(this DataSet value)

        {

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            serializer.MaxJsonLength = 2147483647;

            Dictionary<string, List<Dictionary<string, object>>> dsList = new Dictionary<string, List<Dictionary<string, object>>>();

            List<Dictionary<string, object>> rows;

            Dictionary<string, object> row;



            foreach (DataTable dt in value.Tables)

            {

                rows = new List<Dictionary<string, object>>();

                foreach (DataRow dr in dt.Rows)

                {

                    row = new Dictionary<string, object>();

                    foreach (DataColumn col in dt.Columns)

                    {

                        row.Add(col.ColumnName, dr[col]);

                    }

                    rows.Add(row);

                }

                dsList.Add(dt.TableName, rows);

            }

            return serializer.Serialize(dsList);

        }



    }

}
