using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.Models
{

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
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

        public int TotalAudit { get; set; }

        public int TotalCreate { get; set; }

        public int ValidateFlag { get; set; }

        public int IsCanAdminAds { get; set; }

        public int Flag { get; set; }

        public int Flag2 { get; set; }

        public string Flag3 { get; set; }

        public string Flag4 { get; set; }

        public int IsCanDeleteNews { get; set; }

        public DateTime? InsertTime { get; set; }
    }
}
