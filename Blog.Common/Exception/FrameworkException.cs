using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common
{
    /// <summary>
    /// 框架异常
    /// </summary>
  public  class FrameworkException:Exception
    {
        public FrameworkException()
        {

        }
        public FrameworkException(string message):base(message)
        {

        }
        public FrameworkException(string message, Exception innerException):base(message,innerException)
        {

        }
    }
}
