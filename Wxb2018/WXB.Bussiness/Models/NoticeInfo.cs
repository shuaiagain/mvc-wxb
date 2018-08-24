using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.Models
{

    /// <summary>
    /// 通知信息
    /// </summary>
    public class NoticeInfo
    {
        public int ID { get; set; }

        /// <summary>
        /// 通知标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 通知附件Url
        /// </summary>
        public string AttachmentUrl { get; set; }

        /// <summary>
        /// 反馈人ID
        /// </summary>
        public int? FeedBackerID { get; set; }

        /// <summary>
        /// 返回人
        /// </summary>
        public string FeedBacker { get; set; }

        /// <summary>
        /// 反馈内容
        /// </summary>
        public string FeedBack { get; set; }

        /// <summary>
        /// 反馈时间
        /// </summary>
        public DateTime? FeedBackTime { get; set; }

        /// <summary>
        /// 录入人
        /// </summary>
        public string Inputer { get; set; }

        /// <summary>
        /// 录入人ID
        /// </summary>
        public int? InputerID { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime? InputTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 指定角色(用于'指定方式'为'指定角色')
        /// </summary>
        public string ChooseRoles { get; set; }
    }
}
