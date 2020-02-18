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
//
using System.Globalization;
using System.Threading;

// 필수 using 문

using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.COMMON.BASE;

namespace CLT.WEB.UI.ACADEMY.INTRO
{
    /// <summary>
    /// 1. 작업개요 : Sub 페이지 Class
    /// 
    /// 2. 주요기능 : 
    ///				  
    /// 3. Class 명 : Sub
    /// 
    /// 4. 작 업 자 : 
    /// 
    /// 5. Revision History : 
    /// 
    /// </summary>
    public partial class Sub : BasePage
    {

        #region 초기화 그룹

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //Response.Redirect("asset/FullCalendar", false);
            }
            catch (Exception ex)
            {
                base.NotifyError(ex);
            }
        }

        #endregion

        #region 기타 프로시저 그룹 [Core Logic]
        
        #endregion

        #region 화면 컨트롤 이벤트 핸들러 그룹
        
        #endregion
    }
}

