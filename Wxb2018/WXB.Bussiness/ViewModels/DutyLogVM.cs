using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.ViewModels
{
    /// <summary>
    /// 值班日志视图
    /// </summary>
    public class DutyLogVM
    {
        public int? ID { get; set; }

        /// <summary>
        /// 值班人名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 值班人员ID
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// 值班开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 值班结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        
        #region 扩展字段

        /// <summary>
        /// 值班开始时间
        /// </summary>
        public string StartTimeStr { get; set; }

        /// <summary>
        /// 值班结束时间
        /// </summary>
        public string EndTimeStr { get; set; } 

        #endregion

    }
}
