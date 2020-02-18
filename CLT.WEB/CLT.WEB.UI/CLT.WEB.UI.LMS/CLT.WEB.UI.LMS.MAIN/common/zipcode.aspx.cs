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

namespace CLT.WEB.UI.LMS.COMMON
{
    /// <summary>
    /// 1. 작업개요 : 주소검색 Class
    /// 
    /// 2. 주요기능 : 주소검색 처리
    ///				  
    /// 3. Class 명 : zipcode
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02.
    /// </summary>
    public partial class zipcode : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void search_OnClick(object sender, EventArgs e)
        {
        }


        protected void txtZipcode_OnTextChanged(object sender, EventArgs e)
        {
        }

        protected void C1WebGrid1_ItemDataBound(object sender, C1ItemEventArgs e)
        {

        }
    }
}
