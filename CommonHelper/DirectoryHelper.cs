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
        /// 返回指定目录集合
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<string> IsExists(string path)
        {
            return Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
        }
        /// <summary>
        /// 创建目录,返回路径
        /// </summary>
        /// <param name="path"></param>
        public static string CreateDirectory(string path)
        { 
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
