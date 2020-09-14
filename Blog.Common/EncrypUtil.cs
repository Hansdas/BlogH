using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Common
{
   public class EncrypUtil
    {
        public static string MD5Encry(string strs)
        {
            if (string.IsNullOrEmpty(strs))
                return "";
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(strs);//将要加密的字符串转换为字节数组
            byte[] encryptdata = md5.ComputeHash(bytes);//将字符串加密后也转换为字符数组
            return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串
        }
    }
}
