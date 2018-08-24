using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.Models
{

    /// <summary>
    /// 操作日志
    /// </summary>
    public class LogInfo
    {
        public int ID { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int? OperateType { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string OperateDescribe { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public int? OperatorID { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperateTime { get; set; }
    }
}
