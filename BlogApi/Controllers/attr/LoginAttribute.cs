using Domain.Log;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi.Controllers.attr
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false)]
    public class LoginAttribute:ActionFilterAttribute
    {
        LoginLog loginLog = new LoginLog();
        public override void OnActionExecuting(ActionExecutingContext context)
        {
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result as ObjectResult;
            context.Result = new ObjectResult(new { code = 202, result = result.Value });
        }
        /// <summary>
        /// 登录后
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}
