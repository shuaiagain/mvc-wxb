using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using WXB.Bussiness.Service;
using WXB.Bussiness.ViewModels;

namespace Wxb2018.Filters
{
    public class BaseFilter : ActionFilterAttribute
    {
        protected WFFormsAuthentication UserData { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var cookie = filterContext.HttpContext.Request.Cookies["USERSESSIONS"];
            if (cookie != null)
            {
                try
                {
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                    if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                    {
                        var userId = Convert.ToInt32(ticket.UserData.Split('$')[0]);
                        UserVM user = new UserService().GetUserById(userId);
                        if (user == null)
                        {
                            filterContext.Result = new RedirectResult("~/account/login");
                            return;
                        }

                        this.UserData = new WFFormsAuthentication()
                        {
                            Name = user.Name,
                            Flag = user.Flag,
                            UserId = user.ID,
                            RoleTypes = user.RoleTypes
                        };
                    }
                }
                catch (Exception ex)
                {
                }

                return;
            }
            else if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/account/login");
                return;
            }

            //1.登录状态获取用户信息（自定义保存的用户）
            var cookieTwo = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            //2.使用 FormsAuthentication 解密用户凭据
            var ticketTwo = FormsAuthentication.Decrypt(cookieTwo.Value);

            //3. 直接解析到用户模型里去
            this.UserData = new JavaScriptSerializer().Deserialize<WFFormsAuthentication>(ticketTwo.UserData);
        }

    }
}