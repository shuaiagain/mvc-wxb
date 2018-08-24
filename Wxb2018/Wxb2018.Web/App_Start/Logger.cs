using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WXB.Bussiness.ViewModels;
using WXB.Bussiness.Service;

namespace Wxb2018
{
    public class Logger
    {
        public static LogVM AddLog(LogVM log)
        {
            try
            {
                return new LogService().Add(log);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}