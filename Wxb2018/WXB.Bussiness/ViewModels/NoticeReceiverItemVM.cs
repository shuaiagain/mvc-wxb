using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WXB.Bussiness.ViewModels
{

    public class NoticeReceiverItemVM
    {

        public int? ID { get; set; }

        /// <summary>
        /// 通知ID
        /// </summary>
        public int? NoticeID { get; set; }


        #region 扩展字段
        
        /// <summary>
        /// 通知标题
        /// </summary>
        public string Title { get; set; }
        
        #endregion
    }
}
