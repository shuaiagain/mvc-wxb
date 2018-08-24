using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.ViewModels
{
    public class OnDutyVM
    {
        /// <summary>
        /// 当前值班人
        /// </summary>
        public string CurrentDutyUser { get; set; }

        /// <summary>
        /// 值班ID
        /// </summary>
        public int? CurrentDutyID { get; set; }

        /// <summary>
        /// 其他没有点击'下班'的值班人员
        /// </summary>
        public List<DutyLogVM> OtherDutyUser { get; set; }
    }
}
