
using Blog.Application.ViewModel;
using Blog.Common;
using BlogApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private IHttpContextAccessor _httpContext;
        public GlobaExceptionFilterAttribute(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
    }
        public override void OnException(ExceptionContext context)
        {
            string message = context.Exception.Message;
            Task.Factory.StartNew(() =>
            {
                UserModel userModel= Auth.GetLoginUser(_httpContext);
                new LogUtils().LogError(context.Exception, "GlobaExceptionFilterAttribute", message, userModel.Account);
            });
            ReturnResult returnResult = new ReturnResult("1", context.Exception.Message);
            context.Result = new JsonResult(returnResult);
            context.ExceptionHandled = true;
        }
    }
}
