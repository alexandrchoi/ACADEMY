using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CLT.WEB.UI.COMMON.CONTROL
{
    public class BasePagingInfo : WebControl, INamingContainer
    {
        private int iCurrentPageIndex = 1;
        private int iTotalRecordCount = 100;
        private int iPageSize = 15;

        [Category("Paging용 필수 데이터"), Description("로드할 페이지 인덱스를 가져오거나 설정한다")]
        public int CurrentPageIndex
        {
            get
            {
                if(this.ViewState["CurrentPageIndex"] != null)
                    return (int)(this.ViewState["CurrentPageIndex"]);
                else
                    return this.iCurrentPageIndex;

            }
            set
            {
                this.iCurrentPageIndex = value;
                this.ViewState["CurrentPageIndex"] = this.iCurrentPageIndex;
            }
        }

        [Category("Paging용 필수 데이터"), Description("데이터 소스의 전체 레코드 수")]
        public int TotalRecordCount
        {
            get
            {
                if (this.ViewState["TotalRecordCount"] != null)
                    return (int)(this.ViewState["TotalRecordCount"]);
                else
                    return this.iTotalRecordCount;
            }
            set
            {
                this.iTotalRecordCount = value;
                this.ViewState["TotalRecordCount"] = this.iTotalRecordCount;
            }
        }

        [Description("한 페이지에서 보여질 항목의 개수"), Category("Paging용 필수 데이터")]
        public int PageSize
        {
            get
            {
                if (this.ViewState["PageSize"] != null)
                    return (int)(this.ViewState["PageSize"]);
                else
                    return this.iPageSize;
            }
            set
            {
                this.iPageSize = value;
                this.ViewState["PageSize"] = this.iPageSize;
            }
        }

        public int PageCount
        {
            get
            {
                if (Math.Round((double)this.TotalRecordCount / this.PageSize, 1) == 1)
                    return (this.TotalRecordCount / this.PageSize);
                else
                    return (this.TotalRecordCount / this.PageSize) + 1;

                

            }
        }
    }
}