using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.ViewModels
{
    /// <summary>
    /// 日志视图
    /// </summary>
    public class LogVM
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

        #region 扩展字段

        /// <summary>
        /// 操作时间str
        /// </summary>
        public string OperateTimeStr { get; set; }

        /// <summary>
        /// 角色类型(可能会有多个角色)
        /// </summary>
        public string RoleTypes { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public string RoleTypeStr { get; set; }

        #endregion
    }
}
