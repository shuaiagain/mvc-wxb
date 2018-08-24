using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Devx;
using Devx.DbProvider;
using WXB.Bussiness.Utils;
using WXB.Bussiness.ViewModels;
using WXB.Bussiness.Common;
using Newtonsoft.Json;

namespace WXB.Bussiness.Service
{
    public class NoticeReceiverService
    {

        #region 获取还未标记接受通知的用户
        /// <summary>
        /// 获取还未标记接受通知的用户
        /// </summary>
        /// <param name="userId"></param>
        public List<NoticeReceiverItemVM> GetUserNotReceiveNotice(int userId)
        {

            string sql = string.Format(@" select 
	                                          ne.ID,
	                                          n.Title,
                                              n.ID as NoticeID
                                          from noticereceiver ne join notice n on ne.NoticeID = n.ID where ne.ReceiverID = {0}
                                          and ne.ID not in
                                          (
	                                          select NoticeReceiverID from receiverstatus re where re.ReceiverID = {0}
                                          ) ", userId);

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {

                List<NoticeReceiverItemVM> userList = dbContext.Sql(sql).Query<NoticeReceiverItemVM, List<NoticeReceiverItemVM>>(reader =>
                {
                    NoticeReceiverItemVM temp = new NoticeReceiverItemVM();
                    if (!string.IsNullOrEmpty(reader["ID"].ToString()))
                        temp.ID = reader.AsInt("ID");

                    if (!string.IsNullOrEmpty(reader["NoticeID"].ToString()))
                        temp.NoticeID = reader.AsInt("NoticeID");

                    if (!string.IsNullOrEmpty(reader["Title"].ToString()))
                        temp.Title = reader.AsString("Title");

                    return temp;
                });

                return userList;
            }
        }
        #endregion
    }
}
