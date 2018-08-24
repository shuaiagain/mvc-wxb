using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wxb2018.Utils
{
    public class UnitConversion
    {
        /// <summary>
        /// 将文件大小Byte转换为对应的B、KB、M
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        public static string FileSizeExchange(int? size)
        {

            string result = string.Empty;

            if (size == 0 || !size.HasValue)
            {
                result = "0B";
            }
            else if (size < 1024)
            {
                result = size + "B";
            }
            else if (size >= 1024 && size.Value < 1024 * 1024)
            {
                result = Math.Round(size.Value * 1.0 / 1024, 2) + "KB";
            }
            else if (size >= 1024 * 1024)
            {
                result = Math.Round(size.Value * 1.0 / (1024 * 1024), 2) + "M";
            }

            return result;
        }
    }
}