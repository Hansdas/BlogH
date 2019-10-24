using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common
{
    /// <summary>
    /// 目录帮助类
    /// </summary>
    public static class DirectoryHelper
    {
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
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        public  static void Delete(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
