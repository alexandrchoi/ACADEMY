using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace CLT.WEB.UI.COMMON.CONTROL
{
    public class PageNavigatorDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            PageNavigator component = (PageNavigator)base.Component;
            StringWriter writer = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            if (component.TotalRecordCount == 0)
            {
                component.TotalRecordCount = 100;
            }
            component.RenderPageLink();
            component.RenderControl(writer2);
            Literal literal = new Literal();
            //literal.Text = "<p align='center' style='font-family:verdana;font-size:11px;color:#0fa0f0'>▒ PageNavigator ▒</p>";
            literal.Text = "<p align='center' style='font-family:맑은고딕;font-size:15px;color:#0fa0f0'>▒ PageNavigator ▒</p>";
            literal.RenderControl(writer2);
            return writer.ToString();
        }
    }
}
