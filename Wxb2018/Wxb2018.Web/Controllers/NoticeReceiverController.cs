using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WXB.Bussiness.ViewModels;
using WXB.Bussiness.Service;
using Newtonsoft.Json;

namespace Wxb2018.Controllers
{
    public class NoticeReceiverController : Controller
    {

        #region 获取还未标记接受通知的用户

        public ActionResult GetUserNotReceiveNotice(int? userId, string jsoncallback = "")
        {
            try
            {
                if (userId == 0 || !userId.HasValue)
                {
                    return Content(JsonConvert.SerializeObject(new
                        {
                            Code = -400,
                            Msg = "参数错误"
                        }));
                }

                List<NoticeReceiverItemVM> list = new NoticeReceiverService().GetUserNotReceiveNotice(userId.Value);
                var json = JsonConvert.SerializeObject(new
                {
                    Code = 200,
                    Msg = "获取成功",
                    Data = list
                });

                if (!string.IsNullOrEmpty(jsoncallback))
                {
                    json = jsoncallback + "(" + json + ")";
                }

                return Content(json, "text/plain", System.Text.Encoding.UTF8);
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
