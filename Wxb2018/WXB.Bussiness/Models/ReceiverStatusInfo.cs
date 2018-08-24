using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.Models
{
    /// <summary>
    /// 接收状态信息
    /// </summary>
    public class ReceiverStatusInfo
    {
        public int ID { get; set; }

        /// <summary>
        /// 通知接收ID
        /// </summary>
        public int? NoticeReceiverID { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiveTime { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 接受人ID
        /// </summary>
        public int? ReceiverID { get; set; }
    }
}
