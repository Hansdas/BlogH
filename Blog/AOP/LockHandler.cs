using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AOP
{
    public class LockHandler : AbstractInterceptorAttribute
    {
        private static readonly object _lock = new object();
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            lock(_lock)
            {

            }
            throw new NotImplementedException();
        }
    }
}
