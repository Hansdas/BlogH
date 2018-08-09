using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    /// <summary>
    /// 登录异常
    /// </summary>
   public class LoginException:Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginException() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        public LoginException(string message) : base(message) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public LoginException(string message, Exception innerException) : base(message, innerException) { }
    }
}
