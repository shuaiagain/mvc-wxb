using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.Web.Security;
using WXB.Bussiness.Common;
using WXB.Bussiness.Service;
using WXB.Bussiness.ViewModels;
using Newtonsoft.Json;

namespace Wxb2018.Controllers
{
    public class JumpController : Controller
    {

        #region cookie登录跳转页
        public ActionResult Index()
        {
            try
            {
                var cookie = Request.Cookies["USERSESSIONS"];
                if (cookie == null) return RedirectToAction("Login", "Account");

                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket == null || string.IsNullOrEmpty(ticket.UserData)) return RedirectToAction("Login", "Account");

                var userId = Convert.ToInt32(ticket.UserData.Split('$')[0]);
                UserVM user = new UserService().GetUserById(userId);
                if (user == null) return RedirectToAction("Login", "Account");

                #region 如果已有用户登录，则先将之前登录的用户退出
                //1.登录状态获取用户信息（自定义保存的用户）
                var cookieTwo = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookieTwo != null)
                {
                    //2.使用 FormsAuthentication 解密用户凭据
                    var ticketTwo = FormsAuthentication.Decrypt(cookieTwo.Value);

                    //3. 直接解析到用户模型里去
                    WFFormsAuthentication userData = JsonConvert.DeserializeObject<WFFormsAuthentication>(ticketTwo.UserData);
                    var logOut = new LogVM()
                    {
                        Operator = userData.Name,
                        OperatorID = userData.UserId,
                        RoleTypes = userData.RoleTypes,
                        OperateTime = DateTime.Now,
                        OperateType = (int)EnumOperateType.退出
                    };

                    logOut.OperateDescribe = ((EnumOperateType)logOut.OperateType).ToString();
                    Logger.AddLog(logOut);

                    WFFormsAuthentication.SignOut();
                }
                #endregion

                UserVM sessionLogin = null;
                if (!(Session["login"] != null && (sessionLogin = Session["login"] as UserVM) != null && sessionLogin.ID == user.ID))
                {
                    #region 登录日志
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

                    Session["login"] = user;
                }

                var noticeId = Request["noticeId"];
                var noticeReceiverId = Request["noticeReceiverId"];
                if (noticeId == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("ViewNotice", "Home", new { noticeId = noticeId, noticeReceiverId = noticeReceiverId });
                }
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
