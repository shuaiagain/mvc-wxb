using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using WXB.Bussiness.Service;
using WXB.Bussiness.ViewModels;

namespace Wxb2018.Filters
{
    public class LoggerFilter : BaseFilter
    {

        /// <summary>
        /// 操作类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string Description { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            new LogService().Add(new LogVM()
            {
                Operator = this.UserData.Name,
                OperatorID = this.UserData.UserId,
                RoleTypes=this.UserData.RoleTypes,
                OperateTime = DateTime.Now,
                OperateDescribe = this.Description,
                OperateType = this.Type
            });
        }

    }
}