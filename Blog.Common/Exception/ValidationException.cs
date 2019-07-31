using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common
{
    /// <summary>
    /// 验证异常
    /// </summary>
   public class ValidationException:Exception
    {
        public ValidationException()
        {

        }
        public ValidationException(string message):base(message)
        {

        }
        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
