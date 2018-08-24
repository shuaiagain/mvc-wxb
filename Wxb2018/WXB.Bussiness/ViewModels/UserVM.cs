using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.ViewModels
{
    public class UserVM
    {
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否是'管理员'(0:是)
        /// </summary>
        public int? Flag { get; set; }

        #region 扩展字段

        /// <summary>
        /// 角色类型（一个用户可能有多个角色）
        /// </summary>
        public string RoleTypes { get; set; }
        #endregion
    }
}
