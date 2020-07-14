using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common
{
    /// <summary>
    /// 常用静态字段
    /// </summary>
    public static class ConstantKey
    {
        /// <summary>
        /// 领域校验校验key
        /// </summary>
        public const string CHECK_KEY = "CHECK_KEY";
        /// <summary>
        /// 映射到静态资源的相对请求路径
        /// </summary>
        public const string STATIC_FILE = "/StaticFiles";
        /// <summary>
        /// 网站根目录,程序启动时Startup类的Configure方法里面赋值
        /// </summary>

        public static string WebRoot = "";
        /// <summary>
        /// 广场页面加载微语cache的ky
        /// </summary>
        public const string CACHE_SQUARE_WHISPER = "SQUARE_WHISPER";
        /// <summary>
        /// 旧nginx上传文件虚拟路径
        /// </summary>

        public const string NGINX_FILE_ROUTE_OLD = "picture";
        /// <summary>
        /// nginx上传文件虚拟路径
        /// </summary>

        public const string NGINX_FILE_ROUTE = "bf";
    }
}
