using System;

using System.Web.Mvc;
using WXB.Bussiness.Service;
using WXB.Bussiness.Common;
using WXB.Bussiness.Models;
using WXB.Bussiness.ViewModels;
using Wxb2018;

namespace Wxb2018.Controllers
{
    public class AccountController : BaseController
    {
        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            //var cookies = new System.Web.HttpCookie("USERSESSIONS");
            //cookies.Value = "9EBB4A6C56A5BE408CAFB68FDB55DEE07292922F0B3A533916595AB9E49851E64CC3A96B855A620C8CD3AED7544075FF6B78A726204A7C4979941E8F3769AC7C0DABEB0246B6944914684AB417F2AD40608B81A06C8C37C89B404FDDE3B32C55AA7265E39B8B0455424D4BE515FD222A91C31C7DC5ADFA2D3C349240EABA014A57AC4E1407620CF87B898501686760DF9CE8B99B4F88E9D283675D68D0F1D060";
            //cookies.Expires = DateTime.Now.AddYears(10);
            //Response.Cookies.Add(cookies);
            if (UserData != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect("/index.html");
            //return View();
        }
        #endregion

        #region 登录

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="isRememberPwd">是否记住密码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(string userName, string password, bool isRememberPwd = false)
        {
            try
            {
                ResultEntity<UserVM> result = new ResultEntity<UserVM>();
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                {
                    result.Msg = "用户名或密码不能为空";
                    result.Code = (int)EnumLoginCode.用户名或密码为空;

                    return Json(result);
                }

                UserVM user = new UserService().Login(userName, password);
                if (user == null)
                {
                    result.Msg = "用户名或密码错误";
                    result.Code = (int)EnumLoginCode.用户不存在;

                    return Json(result);
                }

                this.setAuthCookie(userName, user, isRememberPwd);

                result.Code = (int)EnumLoginCode.登录成功;
                result.Msg = "登录成功";
                result.Data = user;

                #region 日志
                var log = new LogVM()
                {
                    Operator = user.Name,
                    OperatorID = user.ID,
                    RoleTypes = user.RoleTypes,
                    OperateTime = DateTime.Now,
                    OperateType = (int)EnumOperateType.登录
                };

                log.OperateDescribe = ((EnumOperateType)log.OperateType).ToString();
                Logger.AddLog(log);
                #endregion

                return Json(result);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }

        #endregion

        #region LoginHeadPartial
        /// <summary>
        /// LoginHeadPartial
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoginHeadPartial()
        {
            ViewBag.UserData = this.UserData;
            return View();
        }
        #endregion

        #region AccountPartial
        [HttpGet]
        public ActionResult AccountPartial()
        {
            return View();
        }
        #endregion

        #region setAuthCookie
        /// <summary>
        /// setAuthCookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="user"></param>
        /// <param name="isPersistent"></param>
        private void setAuthCookie(string name, UserVM user, bool isPersistent)
        {

            WFFormsAuthentication userData = new WFFormsAuthentication()
            {
                UserId = user.ID,
                Name = string.IsNullOrEmpty(user.Name) ? user.Name : user.Alias,
                TrueName = string.IsNullOrEmpty(user.Name) ? user.Name : user.Alias
            };

            if (!string.IsNullOrEmpty(user.RoleTypes)) userData.RoleTypes = user.RoleTypes;

            if (user.Flag.HasValue) userData.Flag = user.Flag;

            //把数据保存到cookie中
            WFFormsAuthentication.SetAuthCookie(name, userData, isPersistent);
        }
        #endregion

        #region 退出

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        //[Authorize]
        public ActionResult Logout(string redirectUrl)
        {
            try
            {
                #region 日志

                var log = new LogVM()
                {
                    Operator = this.UserData.Name,
                    OperatorID = this.UserData.UserId,
                    RoleTypes = this.UserData.RoleTypes,
                    OperateTime = DateTime.Now,
                    OperateType = (int)EnumOperateType.退出
                };

                log.OperateDescribe = ((EnumOperateType)log.OperateType).ToString();
                Logger.AddLog(log);

                #endregion
                var cookie = Request.Cookies["USERSESSIONS"];
                if (cookie != null)
                {
                    WFFormsAuthentication.CookieSignOut();
                }

                WFFormsAuthentication.SignOut();

                return Redirect(string.IsNullOrEmpty(redirectUrl) ? Url.Action("index", "home") : redirectUrl);
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