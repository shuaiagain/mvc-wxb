using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Text;
using System.Configuration;

namespace Wxb2018
{
    public class LogHelper
    {
        public static readonly string _DirectoryPath = ConfigurationManager.AppSettings["LogInfo"];

        public static void WriteLog(string msg, LogLevel level = LogLevel.Error)
        {

            string path = _DirectoryPath;
            if (string.IsNullOrEmpty(path)) return;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = path + "\\" + DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ssfff") + ".txt";

            FileMode mode = File.Exists(path) ? FileMode.Append : FileMode.OpenOrCreate;
            FileStream fs = new FileStream(path, mode);
            fs.Close();

            StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8);
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + level + "：" + msg);
            sw.Close();
        }
    }

    #region /// 日志级别
    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// DEBUG
        /// </summary>
        DEBUG = 1,
        /// <summary>
        /// Info
        /// </summary>
        Info = 2,
        /// <summary>
        /// Error
        /// </summary>
        Error = 3,
        /// <summary>
        /// Fatal
        /// </summary>
        Fatal = 4,
        /// <summary>
        /// Warn
        /// </summary>
        Warn = 5
    }
    #endregion

}