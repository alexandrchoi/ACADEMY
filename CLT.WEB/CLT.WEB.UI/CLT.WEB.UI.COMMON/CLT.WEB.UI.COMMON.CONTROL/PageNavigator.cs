using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CLT.WEB.UI.COMMON.CONTROL
{
    [Designer(typeof(PageNavigatorDesigner)), ToolboxData("<{0}:PageNavigator runat=server></{0}:PageNavigator>")]
    public class PageNavigator : BasePagingInfo
    {
        private Color currentNumberColor = Color.Silver;
        private LinkButton lb;
        private string next = ">";
        private string next10 = ">>";
        private string nextBlockImg_off = "/asset/images/next10_b.gif";
        private string nextBlockImg_on = "/asset/images/next10_a.gif";
        private string nextImg_off = "/asset/images/next_b.gif";
        private string nextImg_on = "/asset/images/next_a.gif";
        private string prev = "<";
        private string prev10 = "<<";
        private string prevBlockImg_off = "/asset/images/prev10_b.gif";
        private string prevBlockImg_on = "/asset/images/prev10_a.gif";
        private string prevImg_off = "/asset/images/prev_b.gif";
        private string prevImg_on = "/asset/images/prev_a.gif";
        
        public event PagingEventHandler OnPageIndexChanged;

        protected void btnPage_Command(object sender, CommandEventArgs e)
        {
            int xPageCount = int.Parse(((LinkButton)sender).CommandArgument);
            if (xPageCount < 1)
            {
                xPageCount = 1;
            }
            if (xPageCount > this.PageCount)
            {
                xPageCount = this.PageCount;
            }
            ((LinkButton)sender).CommandArgument = xPageCount.ToString();
            this.CurrentPageIndex = xPageCount;
            this.ViewState["CurrentPageIndex"] = this.CurrentPageIndex;
            this.RenderPageLink();
            this.PageIndexChanged(sender);
        }

        protected override void CreateChildControls()
        {
            if (this.ViewState["CurrentPageIndex"] != null)
            {
                this.CurrentPageIndex = (int)this.ViewState["CurrentPageIndex"];
            }
            else
            {
                this.ViewState["CurrentPageIndex"] = this.CurrentPageIndex;
            }

            if (this.ViewState["PageSize"] != null)
            {
                this.PageSize = (int)this.ViewState["PageSize"];
            }
            else
            {
                this.ViewState["PageSize"] = this.PageSize;
            }

            if (this.ViewState["TotalRecordCount"] != null)
            {
                this.TotalRecordCount = (int)this.ViewState["TotalRecordCount"];
            }
            else
            {
                this.ViewState["TotalRecordCount"] = this.TotalRecordCount;
            }

            this.RenderPageLink();
        }

        [Category("Paging 관련 이벤트"), Description("페이지가 변경될 때마다 발생하는 이벤트")]
        public void PageIndexChanged(object sender)
        {
            if (this.OnPageIndexChanged != null)
            {
                this.OnPageIndexChanged(sender, new PagingEventArgs(this.CurrentPageIndex));
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            this.EnsureChildControls();
            base.Render(output);
        }

        public void RenderPageLink()
        {
            int num2;
            int pageCount;
            string str;
            string str2;
            this.Controls.Clear();
            int num = this.CurrentPageIndex;
            string str3 = num.ToString();
            int num3 = int.Parse(str3.Substring(str3.Length - 1));
            if ((num % 10) == 0)
            {
                num2 = num;
            }
            else
            {
                num2 = (num + 10) - num3;
            }
            if (this.PageCount > num2)
            {
                pageCount = num2;
            }
            else
            {
                pageCount = this.PageCount;
            }
            this.lb = new LinkButton();
            this.lb.CausesValidation = false;
            this.lb.Command += new CommandEventHandler(this.btnPage_Command);
            this.lb.CssClass = "first";
            if (num2 > 10)
            {
                this.lb.CommandArgument = (((num - 10) - num3) + 1).ToString();
                //this.lb.Text = (this.prevBlockImg_on == null) ? this.prev10 : ("<i class='fas fa-angle-double-left'></i>");
                this.Controls.Add(this.lb);
            }
            //else
            //{
            //    str = "<font color=silver>" + this.prev10 + "</font>";
            //    str2 = "<img src=\"" + this.prevBlockImg_off + "\" border=\"0\">";
            //    this.Controls.Add(new LiteralControl((this.prevBlockImg_off == null) ? str : str2));
            //}
            this.Controls.Add(new LiteralControl(" "));
            this.lb = new LinkButton();
            this.lb.CausesValidation = false;
            this.lb.Command += new CommandEventHandler(this.btnPage_Command);
            this.lb.CssClass = "prev";
            if (num > 1)
            {
                this.lb.CommandArgument = (num - 1).ToString();
                //this.lb.Text = (this.prevImg_on == null) ? this.prev : ("<i class='fas fa-angle-left'></i>");
                this.Controls.Add(this.lb);
            }
            //else
            //{
            //    str = "<font color=silver>" + this.prev + "</font>";
            //    str2 = "<img src=\"" + this.prevImg_off + "\" border=\"0\">";
            //    this.Controls.Add(new LiteralControl((this.prevImg_off == null) ? str : str2));
            //}
            //
            for (int i = num2 - 9; i <= pageCount; i++)
            {
                if (i == num)
                {
                    this.Controls.Add(new LiteralControl(string.Concat(new object[] { "<span class='current'>" + i + "</span> " })));
                }
                else
                {
                    

                    this.lb = new LinkButton();
                    this.lb.CausesValidation = false;
                    this.lb.Command += new CommandEventHandler(this.btnPage_Command);
                    this.lb.CommandArgument = i.ToString();
                    this.lb.Text = i.ToString();
                    this.Controls.Add(this.lb);
                }
            }
            //
            this.lb = new LinkButton();
            this.lb.CausesValidation = false;
            this.lb.Command += new CommandEventHandler(this.btnPage_Command);
            this.lb.CssClass = "next";
            if (num < this.PageCount)
            {
                this.lb.CommandArgument = (num + 1).ToString();
                //this.lb.Text = (this.nextImg_on == null) ? this.next : ("<i class='fas fa-angle-right'></i>");
                //this.lb.Text = "<i class='fas fa-angle-right'></i>";
                this.Controls.Add(this.lb);
            }
            //else
            //{
            //    str = "<font color=silver>" + this.next + "</font>";
            //    str2 = "<img src=\"" + this.nextImg_off + "\" border=\"0\">";
            //    this.Controls.Add(new LiteralControl((this.nextImg_off == null) ? str : str2));
            //}
            this.Controls.Add(new LiteralControl(" "));
            this.lb = new LinkButton();
            this.lb.CausesValidation = false;
            this.lb.Command += new CommandEventHandler(this.btnPage_Command);
            this.lb.CssClass = "last";
            if (this.PageCount > num2)
            {
                this.lb.CommandArgument = (num2 + 1).ToString();
                //this.lb.Text = (this.nextBlockImg_on == null) ? this.next10 : ("<i class='fas fa-angle-double-right'>");
                //this.lb.Text = "<i class='fas fa-angle-double-right'></i>";
                this.Controls.Add(this.lb);
            }
            //else
            //{
            //    str = "<font color=silver>" + this.next10 + "</font>";
            //    str2 = "<img src=\"" + this.nextBlockImg_off + "\" border=\"0\">";
            //    this.Controls.Add(new LiteralControl((this.nextBlockImg_off == null) ? str : str2));
            //}

            // Page Navigation Count 15개를 기준으로 15보다 작다면 하단 Copyright까지의 공백을 추가 한다.
            //if (TotalRecordCount < 15)
            //{
            //    this.Controls.Add(new LiteralControl(string.Concat(new object[] { "<table><tr><td style='height:100px;'</td></tr></table>" })));
            //}
        }

        [Category("Appearance"), Description("현재 선택된 페이지의 색상 지정")]
        public Color CurrnetNumberColor
        {
            get
            {
                return this.currentNumberColor;
            }
            set
            {
                this.currentNumberColor = value;
            }
        }

        [Category("Paging용 이미지"), Description("다음 블럭 페이지를 위한 활성 이미지의 상대 경로입니다"), Editor("System.Web.UI.Design.ImageUrlEditor", typeof(UITypeEditor))]
        public string Next10ImageUrlA
        {
            get
            {
                return this.nextBlockImg_on;
            }
            set
            {
                this.nextBlockImg_on = value;
            }
        }

        [Category("Paging용 이미지"), Editor("System.Web.UI.Design.ImageUrlEditor", typeof(UITypeEditor)), Description("다음 블럭 페이지를 위한 비활성 이미지의 상대 경로입니다")]
        public string Next10ImageUrlB
        {
            get
            {
                return this.nextBlockImg_off;
            }
            set
            {
                this.nextBlockImg_off = value;
            }
        }

        [Description("다음 블럭 페이지를 위한 텍스트"), Category("Paging용 텍스트")]
        public string NextBlockText
        {
            get
            {
                return this.next10;
            }
            set
            {
                this.next10 = value;
            }
        }

        [Description("다음 페이지를 위한 활성 이미지의 상대 경로입니다"), Editor("System.Web.UI.Design.ImageUrlEditor", typeof(UITypeEditor)), Category("Paging용 이미지")]
        public string NextImageUrlA
        {
            get
            {
                return this.nextImg_on;
            }
            set
            {
                this.nextImg_on = value;
            }
        }

        [Description("다음 페이지를 위한 비활성 이미지의 상대 경로입니다"), Category("Paging용 이미지"), Editor("System.Web.UI.Design.ImageUrlEditor", typeof(UITypeEditor))]
        public string NextImageUrlB
        {
            get
            {
                return this.nextImg_off;
            }
            set
            {
                this.nextImg_off = value;
            }
        }

        [Description("다음 페이지를 위한 텍스트"), Category("Paging용 텍스트")]
        public string NextText
        {
            get
            {
                return this.next;
            }
            set
            {
                this.next = value;
            }
        }

        [Editor("System.Web.UI.Design.ImageUrlEditor", typeof(UITypeEditor)), Category("Paging용 이미지"), Description("이전 블럭 페이지를 위한 활성 이미지의 상대 경로입니다")]
        public string Prev10ImageUrlA
        {
            get
            {
                return this.prevBlockImg_on;
            }
            set
            {
                this.prevBlockImg_on = value;
            }
        }

        [Description("이전 블럭 페이지를 위한 비활성 이미지의 상대 경로입니다"), Editor("System.Web.UI.Design.ImageUrlEditor", typeof(UITypeEditor)), Category("Paging용 이미지")]
        public string Prev10ImageUrlB
        {
            get
            {
                return this.prevBlockImg_off;
            }
            set
            {
                this.prevBlockImg_off = value;
            }
        }

        [Category("Paging용 텍스트"), Description("이전 블럭 페이지를 위한 텍스트")]
        public string PrevBlockText
        {
            get
            {
                return this.prev10;
            }
            set
            {
                this.prev10 = value;
            }
        }

        [Editor("System.Web.UI.Design.ImageUrlEditor", typeof(UITypeEditor)), Category("Paging용 이미지"), Description("이전 페이지를 위한 활성 이미지의 상대 경로입니다")]
        public string PrevImageUrlA
        {
            get
            {
                return this.prevImg_on;
            }
            set
            {
                this.prevImg_on = value;
            }
        }

        [Editor("System.Web.UI.Design.ImageUrlEditor", typeof(UITypeEditor)), Description("이전 페이지를 위한 비활성 이미지의 상대 경로입니다"), Category("Paging용 이미지")]
        public string PrevImageUrlB
        {
            get
            {
                return this.prevImg_off;
            }
            set
            {
                this.prevImg_off = value;
            }
        }

        [Description("이전 페이지를 위한 텍스트"), Category("Paging용 텍스트")]
        public string PrevText
        {
            get
            {
                return this.prev;
            }
            set
            {
                this.prev = value;
            }
        }

        public delegate void PagingEventHandler(object sender, PagingEventArgs e);
    }
}
