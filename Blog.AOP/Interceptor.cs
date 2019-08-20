using Blog.AOP.Cache;
using Blog.AOP.Transaction;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.AOP
{
    public class Interceptor : IInterceptor
    {
        private readonly ICacheInterceptor _cacheInterceptor;
        private readonly ITransactionInterceptor _transactionInterceptor;
        public Interceptor()
        {

        }
        public Interceptor(ICacheInterceptor cacheInterceptor, ITransactionInterceptor transactionInterceptor)
        {
            _cacheInterceptor = cacheInterceptor;
            _transactionInterceptor = transactionInterceptor;
        }
        public void Intercept(IInvocation invocation)
        {
            Attribute attribute = Core.GetAttribute(invocation.MethodInvocationTarget ?? invocation.Method) as Attribute;
            if (attribute is CacheAttribute)
                _cacheInterceptor.Intercept((CacheAttribute)attribute, invocation);
            else if (attribute is TransactionAttribute)
                _transactionInterceptor.Intercept((TransactionAttribute)attribute, invocation);
            else
                invocation.Proceed();
        }
    }
}
