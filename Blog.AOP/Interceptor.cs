using Blog.AOP.Cache;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.AOP
{
    public class Interceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Attribute attribute = Core.GetAttribute(invocation.MethodInvocationTarget ?? invocation.Method) as Attribute;
            if(attribute is CacheAttribute)
            {

            }
            throw new NotImplementedException();
        }
    }
}
