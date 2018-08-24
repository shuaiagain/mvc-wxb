using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Principal;
using System.Web.Security;

namespace Wxb2018
{

    /// <summary>
    /// 登录相关
    /// </summary>
    public class WFFormPrincipal : IPrincipal
    {
        public IIdentity Identity { get; set; }

        public WFFormsAuthentication UserData { get; set; }

        public WFFormPrincipal(FormsAuthenticationTicket ticket, WFFormsAuthentication userData)
        {

            if (ticket == null) throw new ArgumentNullException("ticket");

            if (userData == null) throw new ArgumentNullException("userData");

            Identity = new FormsIdentity(ticket);
            UserData = userData;
        }

        public WFFormPrincipal()
        {
        }

        public bool IsInRole(string role)
        {
            return false;
        }
    }

    public class WFFormsAuthentication
    {
        public string SessionId { get; set; }

        public int? UserId { get; set; }

        public string Name { get; set; }

        public string TrueName { get; set; }

        /// <summary>
        /// 角色类型（一个用户会有多个角色）
        /// </summary>
        public string RoleTypes { get; set; }

        /// <summary>
        /// 是否是管理员（0：是）
        /// </summary>
        public int? Flag { get; set; }

        private const int CookieSaveDays = 20;

        #region SetAuthCookie
        public static string SetAuthCookie(string username, WFFormsAuthentication userData, bool remember)
        {

            if (userData == null) throw new ArgumentNullException("userData");

            var data = Newtonsoft.Json.JsonConvert.SerializeObject(userData);

            var expires = remember ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(1);

            var ticket = new FormsAuthenticationTicket(1, username, DateTime.Now, expires, true, data);

            var cookieValue = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue)
           {
               HttpOnly = false,
               Secure = FormsAuthentication.RequireSSL,
               Domain = FormsAuthentication.CookieDomain,
               Path = FormsAuthentication.FormsCookiePath,
           };

            cookie.Expires = expires;
            HttpContext context = HttpContext.Current;
            if (context == null) throw new InvalidOperationException();

            context.Response.Cookies.Remove(cookie.Name);
            context.Response.Cookies.Add(cookie);

            return cookieValue;
            //context.Response.Cookies.Add(new HttpCookie("test", DateTime.Now.ToString("yyyyMMdd HHmmss")) {  Expires=DateTime.Now.AddDays(1)});
        }
        #endregion

        #region SignOut
        public static void SignOut()
        {
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
          {
              HttpOnly = true,
              Secure = FormsAuthentication.RequireSSL,
              Domain = FormsAuthentication.CookieDomain,
              Path = FormsAuthentication.FormsCookiePath,
              Expires = DateTime.Now.AddDays(-1)
          };

            HttpContext context = HttpContext.Current;
            if (HttpContext.Current == null)
                throw new InvalidOperationException();
            context.Response.Cookies.Add(cookie);

            //#region 设置其他站点cookie
            //if (context.Request.Cookies["USERSESSIONS"] != null)
            //{
            //    var cookieTwo = new HttpCookie("USERSESSIONS", "")
            //      {
            //          HttpOnly = true,
            //          Secure = FormsAuthentication.RequireSSL,
            //          Domain = FormsAuthentication.CookieDomain,
            //          Path = FormsAuthentication.FormsCookiePath,
            //          Expires = DateTime.Now.AddDays(-1)
            //      };

            //    context.Response.Cookies.Add(cookieTwo);
            //}
            //#endregion
        }

        #endregion

        public static void CookieSignOut()
        {

            HttpContext context = HttpContext.Current;
            if (HttpContext.Current == null)
                throw new InvalidOperationException();

            if (context.Request.Cookies["USERSESSIONS"] != null)
            {
                var cookieTwo = new HttpCookie("USERSESSIONS", "")
                {
                    HttpOnly = true,
                    Secure = FormsAuthentication.RequireSSL,
                    Domain = FormsAuthentication.CookieDomain,
                    Path = FormsAuthentication.FormsCookiePath,
                    Expires = DateTime.Now.AddDays(-1)
                };

                context.Response.Cookies.Add(cookieTwo);
            }
        }

        //todo
        #region IsValid
        //public static bool IsValid(string sessionId)
        //{
        //    var a = Devx.Cache.Caches.Get<string>("User::" + sessionId, () =>
        //    {
        //        var userService = new Business.Impl.UserService();
        //        var res = userService.GetUserBaseInfo(sessionId, Devx.WebHelper.GetIP());
        //        return res != null && res.result == 0 ? res.userinfo.userid : "";
        //    }, 3600);

        //    return !string.IsNullOrWhiteSpace(a);
        //} 
        #endregion

        #region TryParsePrincipal
        public static WFFormPrincipal TryParsePrincipal(HttpContext context)
        {
            if (context == null || context.Request == null)
                throw new ArgumentNullException("context");

            var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {

                try
                {
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                    if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                    {
                        var userData = Newtonsoft.Json.JsonConvert.DeserializeObject<WFFormsAuthentication>(ticket.UserData);
                        if (userData != null)
                        {
                            return new WFFormPrincipal(ticket, userData);
                            //return IsValid(userData.SessionId) ? new WFFormPrincipal(ticket, userData) : null;
                        }
                    }
                }
                catch
                {
                }
            }

            return null;
        }
        #endregion

    }

}