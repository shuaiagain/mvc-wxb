using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using WXB.Bussiness.Common;

namespace Wxb2018.Filters
{

    #region 判断是否是'管理员'或'责编'
    /// <summary>
    /// 判断是否是'管理员'或'责编'
    /// </summary>
    public class RoleRightsFilter : BaseFilter
    {

        #region 角色权限判断
        /// <summary>
        /// 角色权限判断
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //只有'管理员'或'责编'可以操作'通知'的相关操作
            var masterEditor = ((int)EnumRoleType.总编).ToString();
            var editor = ((int)EnumRoleType.责编).ToString();
            bool isNotInRole = string.IsNullOrEmpty(this.UserData.RoleTypes) ? true : !this.UserData.RoleTypes.Contains(editor) && !this.UserData.RoleTypes.Contains(editor);

            if (this.UserData.Flag != (int)EnumIsAdmin.是 && isNotInRole)
            {
                filterContext.Result = new RedirectResult("~/home/index");
            }
        }
        #endregion
    }
    #endregion

    #region 判断是否是'管理员'
    /// <summary>
    /// 判断是否是'管理员'
    /// </summary>
    public class AdminFilter : BaseFilter
    {

        #region 角色权限判断
        /// <summary>
        /// 角色权限判断
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (this.UserData.Flag != (int)EnumIsAdmin.是)
            {
                filterContext.Result = new RedirectResult("~/home/index");
            }
        }
        #endregion
    }
    #endregion

}