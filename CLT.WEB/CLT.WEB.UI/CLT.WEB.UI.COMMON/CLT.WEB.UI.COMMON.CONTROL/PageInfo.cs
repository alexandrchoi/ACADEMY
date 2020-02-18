using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CLT.WEB.UI.COMMON.CONTROL
{
    [Designer(typeof(PageInfoDesigner)), ToolboxData("<{0}:PageInfo runat=server></{0}:PageInfo>")]
    public class PageInfo : BasePagingInfo
    {
        protected override void Render(HtmlTextWriter output)
        {
            output.Write("<div class='PageInfo'><b>Total</b> : " + this.TotalRecordCount + " <b>Rows</b>, " + this.CurrentPageIndex + " / " + this.PageCount + " <b>Pages</b></div>");
        }
    }
}
