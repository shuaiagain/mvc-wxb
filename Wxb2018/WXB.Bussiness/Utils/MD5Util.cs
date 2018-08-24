using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WXB.Bussiness.Utils
{
	/// <summary>
	/// MD5Util 的摘要说明。
	/// </summary>
	public class MD5Util
	{
		public MD5Util()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/** 获取大写的MD5签名结果 */
        public static string GetMD5(string encypStr, string charset = "GB2312")
		{
			string retStr;
			MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

			//创建md5对象
			byte[] inputBye;
			byte[] outputBye;

			//使用GB2312编码方式把字符串转化为字节数组．
			try
			{
				inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
			}
			catch (Exception ex)
			{
				inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
			}
			outputBye = m5.ComputeHash(inputBye);

			retStr = System.BitConverter.ToString(outputBye);
			retStr = retStr.Replace("-", "").ToUpper();
			return retStr;
		}
        /// <summary>
        /// 根据文件路径得到文件的MD5值
        /// </summary>
        /// <param name="FilePath">文件的路径</param>
        /// <returns>MD5值</returns>
        public static string GetFileMD5(string FilePath)
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return "";
                }
                FileStream get_file = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash_byte = get_md5.ComputeHash(get_file);
                string resule = System.BitConverter.ToString(hash_byte);
                resule = resule.Replace("-", "");
                return resule;
            }
            catch (Exception e)
            {
                return "";
            }
        }

	}
}
