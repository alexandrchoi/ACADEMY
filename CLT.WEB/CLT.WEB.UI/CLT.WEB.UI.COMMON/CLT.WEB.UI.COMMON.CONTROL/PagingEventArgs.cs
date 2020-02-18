using System;

namespace CLT.WEB.UI.COMMON.CONTROL
{
    public class PagingEventArgs : EventArgs
    {
        protected int pageIndex;

        public PagingEventArgs(int page)
        {
            this.pageIndex = page;
        }

        public int PageIndex
        {
            get
            {
                return this.pageIndex;
            }
        }
    }
}
