using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WXB.Bussiness.Service;
using WXB.Bussiness.ViewModels;
using WXB.Bussiness.Common;
using WXB.Bussiness.Models;

namespace Wxb2018.Controllers
{
    public class ResponseController : BaseController
    {

        #region 获取响应状态
        /// <summary>
        /// 获取响应状态
        /// </summary>
        /// <returns></returns>
        public ActionResult GetResponseStatus()
        {
            try
            {
                ResponseStatusView resVM = new ResponseStatusService().GetResponseStatus();
                if (resVM == null)
                {
                    return Json(new { Code = -200, Msg = "暂无数据" }, JsonRequestBehavior.AllowGet);
                }

                if (resVM.EndTime.HasValue && DateTime.Now >= resVM.EndTime.Value.AddDays(1))
                {
                    resVM.Status = null;
                }
                else if (!resVM.EndTime.HasValue && resVM.StartTime.HasValue && DateTime.Now < resVM.StartTime)
                {
                    resVM.Status = null;
                }

                return Json(new
                {
                    Code = 200,
                    Msg = "获取成功",
                    Data = new
                    {
                        ID = resVM.ID,
                        Status = resVM.Status,
                        StatusText = resVM.Status.HasValue ? ((EnumResponseStatus)resVM.Status).ToString() : "--"
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }

        #endregion

        #region 编辑
        public ActionResult Edit(int responseId = 0)
        {
            try
            {
                ResponseStatusView vm = new ResponseStatusService().GetResponseStatusByID(responseId);
                if (vm == null) vm = new ResponseStatusView();
                return View("Edit", vm);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public ActionResult SaveResponse(ResponseStatusView vm)
        {

            if (vm == null)
            {
                return Json(new
                {
                    Code = -400,
                    Msg = "参数不能为空",
                });
            }

            try
            {
                ResponseStatusService rpSV = new ResponseStatusService();

                vm.Inputer = UserData.Name;
                vm.InputerID = UserData.UserId;

                if (vm.Status.HasValue && !vm.StartTime.HasValue && !vm.EndTime.HasValue)
                {
                    vm.StartTime = DateTime.Now;
                }

                vm = rpSV.SaveResponse(vm);

                #region 日志

                var log = new LogVM()
                {
                    Operator = this.UserData.Name,
                    OperatorID = this.UserData.UserId,
                    RoleTypes = this.UserData.RoleTypes,
                    OperateTime = DateTime.Now,
                    OperateType = (int)EnumOperateType.编辑
                };

                log.OperateDescribe = ((EnumOperateType)log.OperateType).ToString() + "响应状态";
                Logger.AddLog(log);

                #endregion

                if (vm.EndTime.HasValue && DateTime.Now >= vm.EndTime.Value.AddDays(1))
                {
                    vm.Status = null;
                }
                else if (!vm.EndTime.HasValue && vm.StartTime.HasValue && DateTime.Now < vm.StartTime)
                {
                    vm.Status = null;
                }

                return Json(new
                {
                    Code = 200,
                    Msg = "保存成功",
                    Data = new
                    {
                        ID = vm.ID,
                        Status = vm.Status,
                        StatusText = vm.Status.HasValue ? ((EnumResponseStatus)vm.Status).ToString() : "--"
                    }
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