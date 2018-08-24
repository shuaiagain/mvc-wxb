using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.Models
{

    /// <summary>
    /// 响应状态信息
    /// </summary>
    public class ResponseStatusInfo
    {
        public int ID { get; set; }

        /// <summary>
        /// 响应状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 响应开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 响应结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 录入人
        /// </summary>
        public string Inputer { get; set; }

        /// <summary>
        /// 录入人ID
        /// </summary>
        public int? InputerID { get; set; }
    }
}
