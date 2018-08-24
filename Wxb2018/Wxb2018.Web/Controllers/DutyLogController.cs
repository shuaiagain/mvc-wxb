using System;

using System.Web.Mvc;
using WXB.Bussiness.Common;
using WXB.Bussiness.Service;
using WXB.Bussiness.ViewModels;
using Wxb2018.Filters;

namespace Wxb2018.Controllers
{
    public class DutyLogController : BaseController
    {

        #region 获取值班记录
        /// <summary>
        /// 获取登录记录
        /// </summary>
        /// <returns></returns>
        [AdminFilter]
        public ActionResult Index()
        {
            try
            {
                PageVM<DutyLogVM> pageDuty = new DutyLogService().GetDutyLogPageData(new DutyLogQuery() { PageSize = 10 });

                if (pageDuty == null) pageDuty = new PageVM<DutyLogVM>();

                return View(pageDuty);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 上班
        /// <summary>
        /// 上班
        /// </summary>
        /// <returns></returns>
        [BaseFilter]
        public ActionResult Add()
        {
            DutyLogVM vm = new DutyLogVM()
            {
                UserID = UserData.UserId,
                UserName = UserData.Name,
                StartTime = DateTime.Now
            };

            try
            {

                vm = new DutyLogService().AddDutyLog(vm);
                if (vm == null)
                {
                    return Json(new
                    {
                        Code = -400,
                        Msg = "参数错误"
                    });
                }

                #region 日志

                var log = new LogVM()
                {
                    Operator = this.UserData.Name,
                    OperatorID = this.UserData.UserId,
                    RoleTypes = this.UserData.RoleTypes,
                    OperateTime = DateTime.Now,
                    OperateType = (int)EnumOperateType.上班
                };

                log.OperateDescribe = ((EnumOperateType)log.OperateType).ToString();
                Logger.AddLog(log);

                #endregion

                return Json(new
                {
                    Code = 200,
                    Msg = "添加成功",
                    Data = vm
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 下班
        /// <summary>
        /// 下班
        /// </summary>
        /// <returns></returns>
        [BaseFilter]
        public ActionResult UpdateDutyStatus(DutyLogVM vm)
        {

            if (vm == null || !vm.ID.HasValue)
            {
                return Json(new
                {
                    Code = -400,
                    Msg = "参数错误"
                });
            }

            try
            {

                vm.EndTime = DateTime.Now;

                vm = new DutyLogService().UpdateDutyStatus(vm);
                if (vm == null)
                {
                    return Json(new
                    {
                        Code = -400,
                        Msg = "参数错误"
                    });
                }

                #region 日志

                var log = new LogVM()
                {
                    Operator = this.UserData.Name,
                    OperatorID = this.UserData.UserId,
                    RoleTypes = this.UserData.RoleTypes,
                    OperateTime = DateTime.Now,
                    OperateType = (int)EnumOperateType.下班
                };

                log.OperateDescribe = ((EnumOperateType)log.OperateType).ToString();
                Logger.AddLog(log);

                #endregion

                return Json(new
                {
                    Code = 200,
                    Msg = "更新成功",
                    Data = vm
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 获取当前登录用户的值班信息
        public ActionResult GetUserDutyLog()
        {
            try
            {
                if (UserData == null || !UserData.UserId.HasValue)
                {
                    return Json(new
                    {
                        Code = -401,
                        Msg = "请先登录"
                    }, JsonRequestBehavior.AllowGet);
                }

                OnDutyVM vm = new DutyLogService().GetUserDutyLog(UserData.UserId.Value);

                if (vm == null)
                {
                    return Json(new
                       {
                           Code = -200,
                           Msg = "暂无值班人员"
                       }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(vm.CurrentDutyUser) && vm.OtherDutyUser.Count > 0)
                {
                    return Json(new
                    {
                        Code = 201,
                        Msg = "当前用户还未值班，存在未标记下班的用户",
                        Data = vm
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    Code = 200,
                    Msg = "当前用户正在值班",
                    Data = vm
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 获取值班记录(分页)
        /// <summary>
        /// 获取值班记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDutyLogPageData(DutyLogQuery query)
        {
            if (query == null) query = new DutyLogQuery();

            if (!query.PageIndex.HasValue)
                query.PageIndex = 1;

            if (!query.PageSize.HasValue)
                query.PageSize = 10;

            try
            {
                PageVM<DutyLogVM> list = new DutyLogService().GetDutyLogPageData(query);

                if (list == null || list.Data == null || list.Data.Count == 0)
                {
                    return Json(new
                    {
                        Code = -200,
                        Msg = "暂无数据"
                    });
                }

                return Json(new
                {
                    Code = 200,
                    Msg = "获取成功",
                    Data = list
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

    }
}