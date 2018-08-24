using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.ViewModels
{

    public class PageVM<T> where T : new()
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public int? TotalPages { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int? PageIndex { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int? TotalCount { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<T> Data { get; set; }
    }
}
