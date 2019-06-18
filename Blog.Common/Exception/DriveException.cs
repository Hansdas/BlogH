using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;

namespace Blog.Common
{
    /// <summary>
    /// 驱动异常
    /// </summary>
  public  class DriveException:Exception 
    {
        public DriveException()
        {

        }
        public DriveException(string message)
        {

        }
        public DriveException(string message, Exception innerException)
        {

        }
    }
}
