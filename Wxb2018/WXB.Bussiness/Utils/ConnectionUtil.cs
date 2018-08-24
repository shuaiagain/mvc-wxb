using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace WXB.Bussiness.Utils
{
    public class ConnectionUtil
    {
        public static readonly string connWXB = ConfigurationManager.ConnectionStrings["connWXB"].Name;
        public static readonly string connCMS = ConfigurationManager.ConnectionStrings["connCMS"].Name;
    }
}
