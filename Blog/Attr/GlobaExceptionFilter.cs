
using Blog.Application.ViewModel;
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

            if (!_logger.IsDebugEnabled)
            {
                Task.Factory.StartNew(() =>
                {
                    LogEventInfo lei = new LogEventInfo();
                    UserModel userModel = Auth.GetLoginUser();
                    lei.Properties["Account"] = userModel == null ? "" : userModel.Account;
                    lei.Properties["Level"] = LogLevel.Error;
                    lei.Properties["Message"] = context.Exception.Message;
                    _logger.Log(lei);
                });
            }
            ReturnResult returnResult = new ReturnResult("500", "响应服务器失败");
            context.Result = new JsonResult(returnResult);
            context.ExceptionHandled = true;
        }
    }
}
