using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.Models
{

    /// <summary>
    /// 通知接收人信息
    /// </summary>
    public class NoticeReceiverInfo
    {
        public int ID { get; set; }

        /// <summary>
        /// 接收人ID
        /// </summary>
        public int? ReceiverID { get; set; }

        /// <summary>
        /// 接受人
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 通知ID
        /// </summary>
        public int? NoticeID { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PublishTime { get; set; }

    }
}
