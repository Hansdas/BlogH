using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core
{
    /// <summary>
    /// 常用静态字段
    /// </summary>
    public static class ConstantKey
    {
        /// <summary>
        /// 校验重复性key
        /// </summary>
        public const string CHECK_REPEAT_KEY = "CHECK_REPEAT";
        /// <summary>
        /// //自定义使用虚拟资源目录
        /// </summary>
        public const string STATIC_FILE = "/StaticFiles";
        /// <summary>
        /// 网站根目录,程序启动时Startup类的Configure方法里面赋值
        /// </summary>

        public static string WebRoot = "";
    }
}
