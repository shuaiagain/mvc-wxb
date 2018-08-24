using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Devx;
using Devx.DbProvider;
using WXB.Bussiness.Utils;
using WXB.Bussiness.Common;
using WXB.Bussiness.ViewModels;

namespace WXB.Bussiness.Service
{
    public class ResponseStatusService
    {

        #region 获取最新响应状态
        /// <summary>
        /// 获取最新响应状态
        /// </summary>
        /// <returns></returns>
        public ResponseStatusView GetResponseStatus()
        {
            string sql = @" select * from responsestatus ORDER BY ID DESC  LIMIT 0,1 ";

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {
                ResponseStatusView resVM = dbContext.Sql(sql).QuerySingle<ResponseStatusView>(reader =>
                {
                    ResponseStatusView temp = new ResponseStatusView();

                    if (!string.IsNullOrEmpty(reader["ID"].ToString()))
                        temp.ID = reader.AsInt("ID");

                    if (!string.IsNullOrEmpty(reader["Status"].ToString()))
                        temp.Status = reader.AsInt("Status");

                    if (!string.IsNullOrEmpty(reader["StartTime"].ToString()))
                        temp.StartTime = Convert.ToDateTime(reader["StartTime"]);

                    if (!string.IsNullOrEmpty(reader["EndTime"].ToString()))
                        temp.EndTime = Convert.ToDateTime(reader["EndTime"]);

                    if (!string.IsNullOrEmpty(reader["InputerID"].ToString()))
                        temp.InputerID = reader.AsInt("InputerID");

                    if (!string.IsNullOrEmpty(reader["Inputer"].ToString()))
                        temp.Inputer = reader.AsString("InputerID");

                    return temp;
                });

                return resVM;
            }
        }
        #endregion

        #region 通过Id获取响应状态详情
        /// <summary>
        /// 通过Id获取响应状态详情
        /// </summary>
        /// <param name="responseStatusId"></param>
        /// <returns></returns>
        public ResponseStatusView GetResponseStatusByID(int ID)
        {
            string sql = string.Format(@" select * from responsestatus where ID = {0} ", ID);

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {
                ResponseStatusView resVM = dbContext.Sql(sql).QuerySingle<ResponseStatusView>(reader =>
                {
                    ResponseStatusView temp = new ResponseStatusView();

                    if (!string.IsNullOrEmpty(reader["ID"].ToString()))
                        temp.ID = reader.AsInt("ID");

                    if (!string.IsNullOrEmpty(reader["Status"].ToString()))
                        temp.Status = reader.AsInt("Status");

                    if (!string.IsNullOrEmpty(reader["StartTime"].ToString()))
                        temp.StartTime = Convert.ToDateTime(reader["StartTime"]);

                    if (!string.IsNullOrEmpty(reader["EndTime"].ToString()))
                        temp.EndTime = Convert.ToDateTime(reader["EndTime"]);

                    if (!string.IsNullOrEmpty(reader["InputerID"].ToString()))
                        temp.InputerID = reader.AsInt("InputerID");

                    if (!string.IsNullOrEmpty(reader["Inputer"].ToString()))
                        temp.Inputer = reader.AsString("InputerID");

                    return temp;
                });

                return resVM;
            }
        }

        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public ResponseStatusView SaveResponse(ResponseStatusView vm)
        {

            if (vm == null) return null;
            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connWXB, DbProviderTypes.MySql))
            {

                if (vm.ID.HasValue)
                {
                    dbContext.Update("responsestatus").Column("Status", vm.Status)
                                                      .Column("StartTime", vm.StartTime)
                                                      .Column("EndTime", vm.EndTime)
                                                      .Column("Inputer", vm.Inputer)
                                                      .Column("InputerID", vm.InputerID)
                                                      .Where("ID", vm.ID.Value)
                                                      .Execute();

                }
                else
                {
                    vm.ID = dbContext.Insert("responsestatus").Column("Status", vm.Status)
                                                              .Column("StartTime", vm.StartTime)
                                                              .Column("EndTime", vm.EndTime)
                                                              .Column("Inputer", vm.Inputer)
                                                              .Column("InputerID", vm.InputerID)
                                                              .ExecuteReturnLastId<int>();
                }

                return vm;
            }
        }
        #endregion
    }
}
