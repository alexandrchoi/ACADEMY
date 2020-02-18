using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace CLT.WEB.UI.COMMON.CONTROL
{
    public class PageInfoDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            PageInfo component = (PageInfo)base.Component;
            StringWriter writer = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            //if (component.VirtualItemCount == 0)
            //{
            //    component.VirtualItemCount = 100;
            //}
            component.RenderControl(writer2);
            Literal literal = new Literal();
            literal.Text = "<p align='center' style='font-family:verdana;font-size:11px;color:#0fa0f0'>▒ PageInfo ▒</p>";
            literal.RenderControl(writer2);
            return writer.ToString();
        }
    }
}
