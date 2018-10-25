using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using WXB.Bussiness.Models;
using WXB.Bussiness.ViewModels;
using Devx.DbProvider;
using WXB.Bussiness.Utils;
using Devx;
using Newtonsoft.Json;
using WXB.Bussiness.Common;

namespace WXB.Bussiness.Service
{
    public class NoticeService
    {

        #region 保存通知
        /// <summary>
        /// 保存通知
        /// </summary>
        /// <param name="noticeVM"></param>
        /// <returns></returns>
        public NoticeVM Save(NoticeVM noticeVM)
        {

            if (noticeVM == null || string.IsNullOrEmpty(noticeVM.Title) || string.IsNullOrEmpty(noticeVM.Content)) return null;

            try
            {
                List<UserItemVM> userList = new List<UserItemVM>();
                #region 获取用户信息
                if (!string.IsNullOrEmpty(noticeVM.ChooseRoles) || !string.IsNullOrEmpty(noticeVM.ChoosedUserIds))
                {
                    using (var dbContextOne = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
                    {
                        #region 获取指定角色的用户信息

                        if (!string.IsNullOrEmpty(noticeVM.ChooseRoles))
                        {
                            string sqlRole = string.Format(@"select 
		                                                        ud.ID,ud.Name,
		                                                        ur.RoleID as RoleType
                                                        from userdata ud left join userrole ur on ud.ID = ur.UserID
                                                        where 1=1 and ur.RoleID in ({0})", noticeVM.ChooseRoles);
                            //获取指定角色的用户
                            List<UserItemVM> tempList = dbContextOne.Sql(sqlRole).Query<UserItemVM, List<UserItemVM>>(reader =>
                               {
                                   UserItemVM vm = new UserItemVM()
                                   {
                                       ID = reader.AsInt("ID"),
                                       Name = reader.AsString("Name")
                                   };

                                   if (!string.IsNullOrEmpty(reader["RoleType"].ToString()))
                                       vm.RoleTypes = reader.AsString("RoleType");

                                   return vm;
                               });

                            //用户数据去重
                            UserItemVM tempUserItem = new UserItemVM();
                            tempList.ForEach(t =>
                            {
                                tempUserItem = userList.Find(u => u.ID == t.ID);
                                if (tempUserItem == null)
                                {
                                    userList.Add(t);
                                }
                                else if (tempUserItem != null && t.RoleTypes != tempUserItem.RoleTypes)
                                {
                                    userList.Remove(tempUserItem);
                                    tempUserItem.RoleTypes += "," + t.RoleTypes;
                                    userList.Add(tempUserItem);
                                }
                            });
                        }
                        #endregion

                        #region 获取指定用户信息
                        if (!string.IsNullOrEmpty(noticeVM.ChoosedUserIds))
                        {
                            string sqlUser = string.Format(@"select ID,Name from userdata ud where ud.ID in ({0})", noticeVM.ChoosedUserIds);
                            //获取指定ID的用户信息
                            var tempUesrList = dbContextOne.Sql(sqlUser).Query<UserItemVM, List<UserItemVM>>(reader =>
                              {
                                  return new UserItemVM()
                                  {
                                      ID = reader.AsInt("ID"),
                                      Name = reader.AsString("Name")
                                  };
                              });

                            if (tempUesrList != null && tempUesrList.Count > 0)
                            {
                                if (userList == null) userList = new List<UserItemVM>();

                                //用户去重
                                userList = userList.Where(u => tempUesrList.Find(a => a.ID == u.ID) == null).ToList();
                                userList.AddRange(tempUesrList);
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql).UseTransaction(true))
                {
                    #region 新增和编辑处理
                    if (!noticeVM.ID.HasValue)
                    {
                        noticeVM.ID = dbContext.Insert("notice").Column("Title", noticeVM.Title)
                                                    .Column("Content", noticeVM.Content)
                                                    .Column("AttachmentUrl", noticeVM.AttachmentUrl)
                                                    .Column("ChooseRoles", noticeVM.ChooseRoles)
                                                    .Column("UpdateTime", noticeVM.UpdateTime)
                                                    .Column("Inputer", noticeVM.Inputer)
                                                    .Column("InputerID", noticeVM.InputerID)
                                                    .Column("InputTime", noticeVM.InputTime)
                                                    .ExecuteReturnLastId<int>();
                    }
                    else
                    {
                        dbContext.Update("notice").Column("Title", noticeVM.Title)
                                                     .Column("Content", noticeVM.Content)
                                                     .Column("AttachmentUrl", noticeVM.AttachmentUrl)
                                                     .Column("ChooseRoles", noticeVM.ChooseRoles)
                                                     .Column("UpdateTime", noticeVM.UpdateTime)
                                                     .Where("ID", noticeVM.ID)
                                                     .Execute();
                    }
                    #endregion

                    #region 保存通知发送人

                    //先删除之前的数据
                    dbContext.Delete("noticereceiver").Where("NoticeID", noticeVM.ID).Execute();
                    foreach (var item in userList)
                    {
                        dbContext.Insert("noticereceiver").Column("ReceiverID", item.ID)
                                                          .Column("Receiver", item.Name)
                                                          .Column("RoleTypes", item.RoleTypes)
                                                          .Column("NoticeID", noticeVM.ID)
                                                          .Column("PublishTime", noticeVM.UpdateTime)
                                                          .ExecuteReturnLastId<int>();
                    }

                    #endregion

                    dbContext.Commit();
                }

                return noticeVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取通知列表(分页)
        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <returns></returns>
        public PageVM<NoticeVM> GetNoticeListPage(NoticeQuery query)
        {

            if (!query.PageIndex.HasValue)
                query.PageIndex = 1;
            if (!query.PageSize.HasValue)
                query.PageSize = 10;

            string sql = "select * from notice n where 1=1 ";
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                sql += string.Format(" and n.title like '%{0}%' ", query.KeyWord);
            }

            if (query.StartTime.HasValue)
            {
                sql += string.Format(" and date(n.UpdateTime) >= '{0}' ", query.StartTime);
            }

            if (query.EndTime.HasValue)
            {
                sql += string.Format(" and date(n.UpdateTime) <= '{0}' ", query.EndTime);
            }

            sql += " order by n.UpdateTime DESC,n.ID DESC ";
            string pageSql = string.Format(" Limit {0},{1}", (query.PageIndex - 1) * query.PageSize, query.PageSize);
            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {

                //获取指定页数据
                List<NoticeVM> list = dbContext.Sql(sql + pageSql).Query<NoticeVM, List<NoticeVM>>(reader =>
                {

                    var noVM = new NoticeVM()
                    {
                        ID = reader.AsInt("ID"),
                        Title = reader.AsString("Title"),
                        Content = reader.AsString("Content"),
                        Inputer = reader.AsString("Inputer"),
                        InputerID = reader.AsInt("InputerID")
                    };

                    noVM.FeedBack = string.IsNullOrEmpty(reader["FeedBack"].ToString()) ? "" : reader.AsString("FeedBack");
                    noVM.FeedBacker = string.IsNullOrEmpty(reader["FeedBacker"].ToString()) ? "" : reader.AsString("FeedBacker");

                    if (!string.IsNullOrEmpty(reader["AttachmentUrl"].ToString()))
                        noVM.AttachmentUrl = reader.AsString("AttachmentUrl");

                    if (!string.IsNullOrEmpty(reader["FeedBackerID"].ToString()))
                        noVM.FeedBackerID = reader.AsInt("FeedBackerID");

                    if (!string.IsNullOrEmpty(reader["FeedBackTime"].ToString()))
                    {
                        noVM.FeedBackTime = Convert.ToDateTime(reader["FeedBackTime"]);
                        noVM.FeedBackTimeStr = noVM.FeedBackTime.Value.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        noVM.FeedBackTimeStr = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(reader["UpdateTime"].ToString()))
                    {
                        noVM.UpdateTime = Convert.ToDateTime(reader["UpdateTime"]);
                        noVM.UpdateTimeStr = noVM.UpdateTime.Value.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        noVM.UpdateTimeStr = string.Empty;
                    }

                    return noVM;
                });

                //list.ForEach(item =>
                //{
                //    if (!string.IsNullOrEmpty(item.AttachmentUrl))
                //    {
                //        var arr = HttpUtility.UrlDecode(item.AttachmentUrl).Split('>');
                //        item.FilePath = arr.Length > 0 ? arr[0] : string.Empty;
                //        item.FileName = arr.Length > 1 ? arr[1] : string.Empty;
                //    }
                //    else
                //    {
                //        item.FilePath = string.Empty;
                //        item.FileName = string.Empty;
                //    }
                //});

                //获取数据总数
                int totalCount = dbContext.Sql(sql).Query().Count;
                //总页数
                double totalPages = ((double)totalCount / query.PageSize.Value);

                PageVM<NoticeVM> pageVM = new PageVM<NoticeVM>();
                pageVM.Data = list;
                pageVM.TotalCount = totalCount;
                pageVM.TotalPages = (int)Math.Ceiling(totalPages);
                pageVM.PageIndex = query.PageIndex;

                return pageVM;
            }
        }
        #endregion

        #region 删除通知
        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="noticeId"></param>
        public bool Delete(int noticeId)
        {
            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {
                string sql = string.Format(" delete from notice where id = {0} ", noticeId);
                int result = dbContext.Sql(sql).Execute();

                return result > 0 ? true : false;
            }
        }
        #endregion

        #region 获取通知详细
        /// <summary>
        /// 获取通知详细
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public NoticeVM GetNoticeById(int noticeId)
        {
            try
            {
                using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
                {
                    string sql = string.Format("select * from notice n where n.ID = {0} ", noticeId);
                    NoticeVM notice = dbContext.Sql(sql).QuerySingle<NoticeVM>(reader =>
                    {
                        NoticeVM noVM = new NoticeVM()
                        {
                            ID = reader.AsInt("ID"),
                            Title = reader.AsString("Title"),
                            Content = reader.AsString("Content")
                        };

                        noVM.FeedBack = string.IsNullOrEmpty(reader["FeedBack"].ToString()) ? "" : reader.AsString("FeedBack");
                        noVM.FeedBacker = string.IsNullOrEmpty(reader["FeedBacker"].ToString()) ? "" : reader.AsString("FeedBacker");

                        if (!string.IsNullOrEmpty(reader["FeedBackTime"].ToString()))
                        {
                            noVM.FeedBackTime = Convert.ToDateTime(reader["FeedBackTime"]);
                            noVM.FeedBackTimeStr = noVM.FeedBackTime.Value.ToString("yyyy-MM-dd");
                        }

                        if (!string.IsNullOrEmpty(reader["ChooseRoles"].ToString()))
                            noVM.ChooseRoles = reader.AsString("ChooseRoles");

                        if (!string.IsNullOrEmpty(reader["AttachmentUrl"].ToString()))
                            noVM.AttachmentUrl = reader.AsString("AttachmentUrl");

                        if (!string.IsNullOrEmpty(reader["UpdateTime"].ToString()))
                            noVM.UpdateTime = Convert.ToDateTime(reader["UpdateTime"]);

                        return noVM;
                    });

                    string sqlStr = string.Format(@"select 
	                                                    ReceiverID as ID,
	                                                    Receiver as Name
                                                    from noticereceiver nr  where nr.RoleTypes IS NULL and nr.NoticeID = {0} ", noticeId);

                    List<ReceiverVM> receiverList = dbContext.Sql(sqlStr).Query<ReceiverVM, List<ReceiverVM>>(reader =>
                    {
                        var receVM = new ReceiverVM() { ID = reader.AsInt("ID") };

                        if (!string.IsNullOrEmpty(reader["Name"].ToString()))
                            receVM.Name = reader.AsString("Name");

                        return receVM;
                    });

                    notice.NoticeReceiver = receiverList;

                    if (!string.IsNullOrEmpty(notice.ChooseRoles))
                    {
                        List<UserRoleVM> roleList = new List<UserRoleVM>();
                        string[] roleArr = notice.ChooseRoles.Split(',');
                        foreach (string item in roleArr)
                        {
                            if (string.IsNullOrEmpty(item)) continue;
                            roleList.Add(new UserRoleVM()
                            {
                                RoleID = Convert.ToInt32(item),
                                RoleName = ((EnumRoleType)Convert.ToInt32(item)).ToString()
                            });
                        }

                        notice.NoticeReceiveRoles = roleList;
                    }

                    return notice;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 预览
        public NoticeVM GetNotice(int noticeId, UserItemVM user, int noticeReceiverId = 0)
        {
            try
            {
                using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
                {
                    string sql = string.Format("select * from notice n where n.ID = {0} ", noticeId);
                    NoticeVM notice = dbContext.Sql(sql).QuerySingle<NoticeVM>(reader =>
                    {
                        NoticeVM noVM = new NoticeVM()
                        {
                            ID = reader.AsInt("ID"),
                            Title = reader.AsString("Title"),
                            Content = reader.AsString("Content"),
                            Inputer = reader.AsString("Inputer"),
                            InputerID = reader.AsInt("InputerID")
                        };

                        if (!string.IsNullOrEmpty(reader["AttachmentUrl"].ToString()))
                            noVM.AttachmentUrl = reader.AsString("AttachmentUrl");

                        if (!string.IsNullOrEmpty(reader["UpdateTime"].ToString()))
                            noVM.UpdateTimeStr = Convert.ToDateTime(reader["UpdateTime"]).ToString("yyyy-MM-dd");

                        return noVM;
                    });

                    if (noticeReceiverId > 0)
                    {
                        dbContext.Insert("receiverstatus").Column("NoticeReceiverID", noticeReceiverId)
                                                          .Column("ReceiveTime", DateTime.Now)
                                                          .Column("Receiver", user.Name)
                                                          .Column("ReceiverID", user.ID)
                                                          .ExecuteReturnLastId<int>();
                    }

                    return notice;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 保存反馈
        /// <summary>
        /// 保存反馈
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public NoticeVM SaveFeedback(NoticeVM vm)
        {

            if (vm == null || !vm.ID.HasValue) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {

                dbContext.Update("notice").Column("FeedBack", vm.FeedBack)
                                          .Column("FeedBackTime", vm.FeedBackTime)
                                          .Column("FeedBacker", vm.FeedBacker)
                                          .Column("FeedBackerID", vm.FeedBackerID)
                                          .Where("ID", vm.ID)
                                          .Execute();

                return vm;
            }
        }

        #endregion

    }
}
