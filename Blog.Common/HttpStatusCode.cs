using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common
{
    /// <summary>
    /// api接口返回状态
    /// </summary>
    public class HttpStatusCode
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        public const string SUCCESS = "200";
        /// <summary>
        /// 请求失败
        /// </summary>
        public const string BAD_REQUEST = "400";
        /// <summary>
        /// 身份验证错误
        /// </summary>
        public const string FORBIDDEN = "403";
        /// <summary>
        /// 系统异常，非业务逻辑
        /// </summary>
        public const string ERROR = "500";
    }
}
