
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NLog;
using System;

namespace Blog
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobaExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly NLog.ILogger _logger;
        public GlobaExceptionFilterAttribute()
        {
            _logger = LogManager.GetCurrentClassLogger(); 
        }
        public override void OnException(ExceptionContext context)
        {
            LogEventInfo lei = new LogEventInfo();
        }
    }
}
