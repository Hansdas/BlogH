using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common
{
    /// <summary>
    /// 身份认证异常
    /// </summary>
   public class AuthException:Exception
    {
        public AuthException()
        {

        }
        public AuthException(string message) : base(message)
        {

        }
        public AuthException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
