
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
        //protected ILog _log;
        //public GlobaExceptionFilter(ILog log)
        //{
        //    _log = log;
        //}
        private readonly NLog.ILogger _logger;
        public GlobaExceptionFilterAttribute()
        {
            _logger = LogManager.GetCurrentClassLogger(); 
        }
        public override void OnException(ExceptionContext context)
        {
            LogEventInfo lei = new LogEventInfo();
            //lei.Properties["Account"] = "1";
            //lei.Properties["Date"] = DateTime.Now;
            //lei.Properties["Logger"] = "1";
            //lei.Properties["Level"] = "1";
            //lei.Properties["Message"] = "1";
            //lei.Properties["Exception"] = "1";
            //lei.Level = NLog.LogLevel.Info; 
            //_logger.Log(lei);
        }
    }
}
