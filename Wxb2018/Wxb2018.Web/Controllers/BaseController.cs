using System;
using System.Web.Mvc;
using System.Web.Security;
using WXB.Bussiness.Service;
using WXB.Bussiness.ViewModels;

namespace Wxb2018.Controllers
{
    public class BaseController : Controller
    {

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                var cookie = filterContext.HttpContext.Request.Cookies["USERSESSIONS"];
                if (cookie != null)
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

                        this.LoginCookie = new WFFormsAuthentication()
                        {
                            Name = user.Name,
                            Flag = user.Flag,
                            UserId = user.ID,
                            RoleTypes = user.RoleTypes
                        };

                        ViewBag.UserData = this.LoginCookie;
                    }

                    return;
                }
                else if (!Request.IsAuthenticated || this.UserData == null)
                {
                    object[] actionFilter = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthorizeAttribute), false);
                    object[] controllerFilter = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AuthorizeAttribute), false);

                    if (controllerFilter.Length > 0 || actionFilter.Length > 0)
                    {
                        if (Devx.WebHelper.IsAjaxRequest())
                        {
                            filterContext.Result = Json(new
                            {
                                error = true,
                                message = "请登录！",
                                login = true
                            }, "text/plain", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            filterContext.Result = RedirectToAction("login", "account");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
            }

            base.OnAuthorization(filterContext);
        }

        protected WFFormsAuthentication UserData
        {
            get
            {
                if (Request.IsAuthenticated && this.User != null)
                {
                    var principal = this.User as WFFormPrincipal;
                    if (principal != null && principal.UserData != null)
                    {
                        return principal.UserData;
                    }
                }
                else if (this.LoginCookie != null)
                {
                    return LoginCookie;
                }

                return null;
            }
        }

        /// <summary>
        /// 通过获取主站点的cookie进行登录的
        /// </summary>
        private WFFormsAuthentication LoginCookie { get; set; }
    }
}