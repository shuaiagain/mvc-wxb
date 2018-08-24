using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Devx;
using Devx.DbProvider;
using WXB.Bussiness.Utils;
using WXB.Bussiness.ViewModels;
using WXB.Bussiness.Common;
using Newtonsoft.Json;

namespace WXB.Bussiness.Service
{
    public class LogService
    {

        #region 添加日志
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public LogVM Add(LogVM log)
        {
            if (log == null) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {
                log.ID = dbContext.Insert("log").Column("Operator", log.Operator)
                                         .Column("OperatorID", log.OperatorID)
                                         .Column("OperateTime", log.OperateTime)
                                         .Column("OperateType", log.OperateType)
                                         .Column("OperateDescribe", log.OperateDescribe)
                                         .Column("RoleTypes", log.RoleTypes)
                                         .ExecuteReturnLastId<int>();

                return log;
            }
        }
        #endregion

        #region 获取登录记录
        /// <summary>
        /// 获取登录记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public PageVM<LogVM> GetLoginLogPageData(LogQuery query)
        {

            if (!query.PageIndex.HasValue)
                query.PageIndex = 1;
            if (!query.PageSize.HasValue)
                query.PageSize = 10;

            string sql = @" select * from log g where g.operatetype = 1  ";
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                sql += string.Format(" and g.Operator like '%{0}%' ", query.KeyWord);
            }

            if (query.StartTime.HasValue)
            {
                sql += string.Format(" and date(g.OperateTime) >= '{0}' ", query.StartTime);
            }

            if (query.EndTime.HasValue)
            {
                sql += string.Format(" and date(g.OperateTime) <= '{0}' ", query.EndTime);
            }

            sql += " order by g.ID DESC ";
            string pageSql = string.Format(" Limit {0},{1}", (query.PageIndex - 1) * query.PageSize, query.PageSize);
            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {
                //获取指定页数据
                List<LogVM> list = dbContext.Sql(sql + pageSql).Query<LogVM, List<LogVM>>(reader =>
                {
                    var duVM = new LogVM()
                    {
                        ID = reader.AsInt("ID"),
                        Operator = reader.AsString("Operator"),
                        OperatorID = reader.AsInt("OperatorID")
                    };

                    if (!string.IsNullOrEmpty(reader["OperateTime"].ToString()))
                    {
                        duVM.OperateTime = Convert.ToDateTime(reader["OperateTime"]);
                        duVM.OperateTimeStr = duVM.OperateTime.Value.ToString("yyyy-MM-dd hh:MM:ss");
                    }
                    else
                    {
                        duVM.OperateTimeStr = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(reader["RoleTypes"].ToString()))
                    {
                        duVM.RoleTypes = reader.AsString("RoleTypes");
                    }
                    else
                    {
                        duVM.RoleTypeStr = string.Empty;
                    }

                    return duVM;
                });

                string[] roleIds = new string[] { };
                list.ForEach(u =>
                {
                    if (!string.IsNullOrEmpty(u.RoleTypes))
                    {
                        roleIds = u.RoleTypes.Split(',');
                        foreach (var elem in roleIds)
                        {
                            if (string.IsNullOrEmpty(elem)) continue;
                            u.RoleTypeStr += ((EnumRoleType)Convert.ToInt32(elem)).ToString() + " ";
                        }
                    }

                });

                //获取数据总数
                int totalCount = dbContext.Sql(sql).Query().Count;
                //总页数
                double totalPages = ((double)totalCount / query.PageSize.Value);

                PageVM<LogVM> pageVM = new PageVM<LogVM>();
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
