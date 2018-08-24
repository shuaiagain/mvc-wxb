using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.ViewModels
{
    /// <summary>
    /// 通知信息视图
    /// </summary>
    public class NoticeVM
    {
        public int? ID { get; set; }

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

        #region 扩展字段
        /// <summary>
        /// 
        /// </summary>
        public string InputTimeStr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FeedBackTimeStr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UpdateTimeStr { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 附件url
        /// </summary>
        public string FilePath { get; set; }

        public ChooseUserVM ChooseUser { get; set; }

        /// <summary>
        /// 选中用户的ids
        /// </summary>
        public string ChoosedUserIds { get; set; }

        /// <summary>
        /// 通知接收者
        /// </summary>
        public List<ReceiverVM> NoticeReceiver { get; set; }

        /// <summary>
        /// 接收通知的角色
        /// </summary>
        public List<UserRoleVM> NoticeReceiveRoles { get; set; }
        #endregion

    }
}
