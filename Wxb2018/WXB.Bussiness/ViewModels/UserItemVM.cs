using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.ViewModels
{
    /// <summary>
    /// 用户视图
    /// </summary>
    public class UserItemVM
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
        /// 角色类型
        /// </summary>
        public int? RoleType { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public string RoleTypes { get; set; }

    }
}
