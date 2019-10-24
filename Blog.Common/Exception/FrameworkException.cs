using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common
{
    /// <summary>
    /// 服务异常
    /// </summary>
  public  class ServiceException:Exception
    {
        public ServiceException()
        {

        }
        public ServiceException(string message):base(message)
        {

        }
        public ServiceException(string message, Exception innerException):base(message,innerException)
        {

        }
    }
}
