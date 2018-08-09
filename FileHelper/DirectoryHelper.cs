using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileHelper
{
    /// <summary>
    /// 目录帮助类
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// 判断是否存在指定目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsExists(string path)
        {
            return Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories).Any();
        }
        /// <summary>
        /// 创建目录,返回路径
        /// </summary>
        /// <param name="path"></param>
        /// 
        public static string CreateDirectory()
        {
            string time = DateTime.Now.ToString("yyyyMMdd");
            string savePath = @"D:\Blog\admin\" + time + @"\";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            return savePath;
        }
    }
}
