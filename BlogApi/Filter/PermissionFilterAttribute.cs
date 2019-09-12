﻿using Blog.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// 使用Lamda生成sql时在实体类使用该特性
/// </summary>
namespace Blog.Attr
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class,AllowMultiple =false)]
    public class PermissionFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            UserModel userModel= Auth.GetLoginUser();
            if (userModel == null)
            {
                string loginContent = string.Format("<script>top.location.href='{0}';</script>", "Login");
                context.Result = new RedirectResult("/login/login");
            }
        }
    }
}
