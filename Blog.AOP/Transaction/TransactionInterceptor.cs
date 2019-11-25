using Blog.Common;
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
    public class TransactionInterceptor : ITransactionInterceptor
    {

        public void Intercept(TransactionAttribute attribute, IInvocation invocation)
        {
            if (attribute != null)
            {
                //事务过期和级别信息
                TransactionOptions transactionOptions = new TransactionOptions()
                {
                    IsolationLevel = Enum.Parse<System.Transactions.IsolationLevel>(attribute.Level.GetEnumValue().ToString()),
                    Timeout = attribute.TimeoutSecond.HasValue ? new TimeSpan(0, 0, attribute.TimeoutSecond.Value) : new TimeSpan(0, 0, 10)
                };
                //事务范围
                TransactionScopeOption transactionScopeOption = Enum.Parse<TransactionScopeOption>(attribute.TransactionScope.Value.GetEnumValue().ToString());
                using (TransactionScope transactionScope = new TransactionScope(transactionScopeOption, transactionOptions))
                {
                    try
                    {
                        invocation.Proceed();
                        transactionScope.Complete();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        transactionScope.Dispose();
                    }
                }
            }
        }
    }
}
