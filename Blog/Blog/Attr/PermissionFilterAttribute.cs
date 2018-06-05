using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Attr
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class,AllowMultiple =false)]
    public class PermissionFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string loginContent = string.Format("<script>top.location.href='{0}';</script>", "Login");
            context.Result = new ContentResult() { Content = loginContent, ContentType = "text/html" };
        }
    }
}
