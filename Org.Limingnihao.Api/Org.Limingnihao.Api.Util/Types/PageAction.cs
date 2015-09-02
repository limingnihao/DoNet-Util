using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Types
{
    public enum PageAction : uint
    {
        /// <summary>
        /// 首页
        /// </summary>
        First = 0,

        /// <summary>
        /// 末页
        /// </summary>
        Last = 1,

        /// <summary>
        /// 下一页
        /// </summary>
        Next = 2,

        /// <summary>
        /// 上一页
        /// </summary>
        Previous = 3

    }

}
