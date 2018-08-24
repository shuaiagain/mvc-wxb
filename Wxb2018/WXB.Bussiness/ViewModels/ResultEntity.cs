using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.ViewModels
{
    /// <summary>
    /// 返回数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultEntity<T> where T : new()
    {

        public int Code { get; set; }

        public string Msg { get; set; }

        public T Data { get; set; }
    }
}
