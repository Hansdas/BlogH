using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.AOP.Cache
{
   public interface ICacheInterceptor
    {
        void Intercept(CacheAttribute attribute, IInvocation invocation);
    }
}
