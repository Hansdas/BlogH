
using Blog.Application.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Threading.Tasks;
using LogLevel = NLog.LogLevel;

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
            string message = context.Exception.Message;
            //Task.Factory.StartNew(() =>
            //{
            //    //_logger.Log(LogLevel.Error, context.Exception, message);
            //    _logger.Error(context.Exception, message);
            //});
            ReturnResult returnResult = new ReturnResult("500", "响应服务器错误");
            context.Result = new JsonResult(returnResult);
            context.ExceptionHandled = true;
        }
    }
}
