using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.AOP.Transaction
{
  public interface ITransactionInterceptor
    {
        void Intercept(TransactionAttribute attribute, IInvocation invocation);
    }
}
