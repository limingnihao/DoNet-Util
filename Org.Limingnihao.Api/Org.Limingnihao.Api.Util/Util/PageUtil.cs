using Org.Limingnihao.Api.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    /// <summary>
    /// 分页操作功能
    /// </summary>
    public class PageUtil
    {

        /// <summary>
        /// int最大值
        /// </summary>
        public static int MAX_SIZE = Int32.MaxValue;

	    /// <summary>
        /// 获取查询起始数
	    /// </summary>
	    public static int GetFirstResult(int pageNow, int pageSize, int total) 
        {
		    if (pageSize <= 0) 
            {
			    return 0;
		    }
            if (pageNow <= 0)
            {
                pageNow = 1;
            }
            int pageTotal = GetPageTotal(total, pageSize);
            if (pageNow >= pageTotal)
            {
                pageNow = pageTotal;
            }
		    int firstResult = (pageNow - 1) * pageSize;
            if (firstResult < 0)
            {
                firstResult = 0;
            }
		    return firstResult;
	    }

        /// <summary>
        /// 查询结束数
        /// </summary>
        /// <returns></returns>
        public static int GetLastResult(int pageNow, int pageSize, int total)
        {
            if (pageNow <= 0)
            {
                pageNow = 1;
            }
            int pageTotal = GetPageTotal(total, pageSize);
            if (pageNow >= pageTotal)
            {
                return total;
            }
            int lastResult = pageNow * pageSize;
            if (lastResult < 0)
            {
                lastResult = 0;
            }
            if (lastResult > total)
            {
                lastResult = total;
            }
            return lastResult;
        }

	    /// <summary>
        /// 获取查询总数量
	    /// </summary>
	    public static int GetMaxResults(int pageSize) 
        {
		    if (pageSize <= 0) 
            {
                return MAX_SIZE;
		    }
		    return pageSize;
	    }

	    /// <summary>
	    /// 获取总页数
        /// <summary>
	    public static int GetPageTotal(int total, int pageSize) 
        {
		    if (pageSize <= 0) 
            {
			    return 1;
		    }
		    int pageTotal = total / pageSize;
		    if (total % pageSize != 0) 
            {
			    pageTotal++;
		    }
		    if (pageTotal < 1) 
            {
			    pageTotal = 1;
		    }
		    return pageTotal;
	    }

        /// <summary>
        /// 根据动作，计算出应该显示的页数
        /// </summary>
        public static int GetPageNow(PageAction action, int pageNow, int pageTotal) 
        {
		    int pageNum = pageNow;
            if (action.Equals(PageAction.First))
            {
			    pageNum = 0;
            }
            else if (action.Equals(PageAction.Last))
            {
			    pageNum = pageTotal;
            }
            else if (action.Equals(PageAction.Next))
            {
			    pageNum++;
            }
            else if (action.Equals(PageAction.Previous))
            {
			    pageNum--;
		    } 
            else 
            {
			    pageNum = 1;
		    }
		    if (pageNum > pageTotal) 
            {
			    pageNum = pageTotal;
		    }
		    if (pageNum < 1) 
            {
			    pageNum = 1;
		    }
		    return pageNum;
	    }

        /// <summary>
        /// 前台分页.
        /// 例如：IList result = PageUtil.getPageDate(pageNow, this.pageSize, list as IList);
        /// </summary>
        public static IList GetPageDate(int pageNow, int pageSize, IList data)
        {
            if (data == null || data.Count <= 0)
            {
                return new List<Object>();
            }
            IList list = new List<Object>();
            int firstResult = PageUtil.GetFirstResult(pageNow, pageSize, data.Count);
            int lastResult = PageUtil.GetLastResult(pageNow, pageSize, data.Count);
            for (int i = firstResult; i < lastResult; i++)
            {
                list.Add(data[i]);
            }
            return list;
        }

    }
}
