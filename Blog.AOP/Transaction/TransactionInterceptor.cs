using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;

namespace Blog.AOP.Transaction
{
    /// <summary>
    /// 事务拦截特性，
    /// </summary>
    public class TransactionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //TransactionAttribute transactionAttribute = Core.GetAttribute<TransactionAttribute>
            //    (invocation.MethodInvocationTarget??invocation.Method,typeof(TransactionAttribute));
            //if (transactionAttribute != null)
            //{
            //    //如果要嵌套使用TransactionScope，则使用TransactionScopeOption.Required
            //    using (TransactionScope transactionScope=new TransactionScope(transactionAttribute.TransactionScopeOption, transactionAttribute.transactionOptions))
            //    {
            //        try
            //        {
            //            invocation.Proceed();
            //            transactionScope.Complete();
            //        }
            //        catch (Exception)
            //        {

            //            throw;
            //        }
            //        finally
            //        {
            //            transactionScope.Dispose();
            //        }
            //    }
            //}
        }
    }
}
