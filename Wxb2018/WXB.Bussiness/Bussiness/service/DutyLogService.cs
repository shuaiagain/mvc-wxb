using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WXB.Bussiness.ViewModels;
using Devx;
using Devx.DbProvider;
using WXB.Bussiness.Utils;

namespace WXB.Bussiness.Service
{
    public class DutyLogService
    {

        #region 新增值班日志
        /// <summary>
        /// 新增值班日志
        /// </summary>
        /// <param name="dutyVm"></param>
        /// <returns></returns>
        public DutyLogVM AddDutyLog(DutyLogVM dutyVm)
        {
            if (dutyVm == null || !dutyVm.UserID.HasValue || !dutyVm.StartTime.HasValue) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {
                dutyVm.ID = dbContext.Insert("dutylog").Column("UserID", dutyVm.UserID)
                                                       .Column("UserName", dutyVm.UserName)
                                                       .Column("StartTime", dutyVm.StartTime)
                                                       .ExecuteReturnLastId<int>();

                return dutyVm;
            }
        }
        #endregion

        #region 下班
        /// <summary>
        /// 下班
        /// </summary>
        /// <param name="dutyVm"></param>
        /// <returns></returns>
        public DutyLogVM UpdateDutyStatus(DutyLogVM dutyVm)
        {
            if (dutyVm == null || !dutyVm.ID.HasValue || !dutyVm.EndTime.HasValue) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {
                dbContext.Update("dutylog").Column("EndTime", dutyVm.EndTime)
                                           .Where("ID", dutyVm.ID)
                                           .Execute();

                return dutyVm;
            }
        }
        #endregion

        #region 获取值班人员信息
        public OnDutyVM GetUserDutyLog(int userId)
        {

            string sql = @" select ID,UserName,UserID from dutylog du where !ISNULL(du.startTime) and ISNULL(du.EndTime)";
            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {

                List<DutyLogVM> dutyList = dbContext.Sql(sql).Query<DutyLogVM, List<DutyLogVM>>(reader =>
                {
                    DutyLogVM vm = new DutyLogVM()
                    {
                        ID = reader.AsInt("ID"),
                        UserID = reader.AsInt("UserID"),
                    };

                    vm.UserName = string.IsNullOrEmpty(reader["UserName"].ToString()) ? "" : reader.AsString("UserName");
                    return vm;
                });

                if (dutyList == null || dutyList.Count == 0) return null;

                List<DutyLogVM> filterList = new List<DutyLogVM>();
                dutyList.ForEach(a =>
                {
                    if (filterList.Find(w => w.UserName == a.UserName) == null)
                        filterList.Add(a);
                });

                OnDutyVM dutyVm = new OnDutyVM() { OtherDutyUser = filterList };

                DutyLogVM temp = filterList.Where(d => d.UserID == userId).FirstOrDefault();
                if (temp != null)
                {
                    dutyVm.CurrentDutyUser = temp.UserName;
                    dutyVm.CurrentDutyID = temp.ID;
                    filterList.Remove(temp);
                }

                return dutyVm;
            }
        }
        #endregion

        #region 获取值班记录
        public PageVM<DutyLogVM> GetDutyLogPageData(DutyLogQuery query)
        {

            if (!query.PageIndex.HasValue)
                query.PageIndex = 1;
            if (!query.PageSize.HasValue)
                query.PageSize = 10;

            string sql = "select * from dutylog du where 1=1 ";
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                sql += string.Format(" and du.UserName like '%{0}%' ", query.KeyWord);
            }

            if (query.StartTime.HasValue)
            {
                sql += string.Format(" and date(du.StartTime) >= '{0}' ", query.StartTime);
            }

            if (query.EndTime.HasValue)
            {
                sql += string.Format(" and date(du.EndTime) <= '{0}' ", query.EndTime);
            }

            sql += " order by du.ID DESC ";
            string pageSql = string.Format(" Limit {0},{1}", (query.PageIndex - 1) * query.PageSize, query.PageSize);
            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {
                //获取指定页数据
                List<DutyLogVM> list = dbContext.Sql(sql + pageSql).Query<DutyLogVM, List<DutyLogVM>>(reader =>
                {

                    var duVM = new DutyLogVM()
                    {
                        ID = reader.AsInt("ID"),
                        UserName = reader.AsString("UserName"),
                        UserID = reader.AsInt("UserID")
                    };

                    if (!string.IsNullOrEmpty(reader["StartTime"].ToString()))
                    {
                        duVM.StartTime = Convert.ToDateTime(reader["StartTime"]);
                        duVM.StartTimeStr = duVM.StartTime.Value.ToString("yyyy-MM-dd hh:MM:ss");
                    }
                    else
                    {
                        duVM.StartTimeStr = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(reader["EndTime"].ToString()))
                    {
                        duVM.EndTime = Convert.ToDateTime(reader["EndTime"]);
                        duVM.EndTimeStr = duVM.EndTime.Value.ToString("yyyy-MM-dd hh:MM:ss");
                    }
                    else
                    {
                        duVM.EndTimeStr = string.Empty;
                    }

                    return duVM;
                });

                //获取数据总数
                int totalCount = dbContext.Sql(sql).Query().Count;
                //总页数
                double totalPages = ((double)totalCount / query.PageSize.Value);

                PageVM<DutyLogVM> pageVM = new PageVM<DutyLogVM>();
                pageVM.Data = list;
                pageVM.TotalCount = totalCount;
                pageVM.TotalPages = (int)Math.Ceiling(totalPages);
                pageVM.PageIndex = query.PageIndex;

                return pageVM;
            }
        }
        #endregion

    }
}
