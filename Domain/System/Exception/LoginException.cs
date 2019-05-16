using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    /// <summary>
    /// 登录异常
    /// </summary>
   public class ValidationException:Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ValidationException() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        public ValidationException(string message) : base(message) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
