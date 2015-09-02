using Org.Limingnihao.Api.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util.Model
{
    public class PageVO
    {
        public PageAction PageAction { get; set; }

        public int PageNow { get; set; }

        public int PageSize { get; set; }

        public int PageTotal { get; set; }

        public int NumberTotal { get; set; }

        public int FirstResult{ get; set; }

        public int MaxResults { get; set; }

        public override string ToString()
        {
            return "[PageVO - PageAction=" + PageAction + ", PageNow=" + PageNow + ", PageSize=" + PageSize + ", PageTotal=" + PageTotal + ", NumberTotal=" + NumberTotal + ", FirstResult=" + FirstResult + ", MaxResults=" + MaxResults + "]";
        }

        public PageVO()
        {
            this.PageNow = 1;
            this.PageAction = PageAction.First;
        }

        public void Init(int total)
        {
            this.NumberTotal = total;
            this.PageTotal = PageUtil.GetPageTotal(this.NumberTotal, this.PageSize);
            this.PageNow = PageUtil.GetPageNow(this.PageAction, this.PageNow, this.PageTotal);
            this.FirstResult = PageUtil.GetFirstResult(this.PageNow, this.PageSize, this.NumberTotal);
            this.MaxResults = PageUtil.GetMaxResults(this.PageSize);
        }

    }
}
