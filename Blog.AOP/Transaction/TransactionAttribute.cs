using Blog.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Blog.AOP.Transaction
{
    /// <summary>
    /// 事务处理特性
    /// </summary>
   public class TransactionAttribute:Attribute
    {

        public TransactionAttribute()
        {
            Timeout = new TimeSpan(0, 0, 20);
            TransactionScopeOption = TransactionScopeOption.RequiresNew;
            transactionOptions = new TransactionOptions() {
                Timeout = Timeout,
                IsolationLevel=Enum.Parse<IsolationLevel>(TransactionLevel.GetEnumValue().ToString())
            };
        }
        /// <summary>
        /// 超时时间
        /// </summary>
        public TimeSpan Timeout { get; set; }
        /// <summary>
        /// 事务级别
        /// </summary>
        public TransactionLevel TransactionLevel { get; set; }
        /// <summary>
        /// 事务信息
        /// </summary>
        public TransactionOptions transactionOptions { get; private set; }
        /// <summary>
        /// 事务范围
        /// </summary>
        public TransactionScopeOption TransactionScopeOption { get; set; }
    }
    public enum TransactionLevel
    {
        //序列化。最严格的隔离级别，当然并发性也是最差的，事务必须依次进行
        Serializable,
        //重复读。当事务A更新数据时，不容许其他事务进行任何的操作，但是当事务A进行读取的时候，其他事务只能读取，不能更新
        RepeatableRead,
        //提交读。当事务A更新数据时，不容许其他事务进行任何的操作包括读取，但事务A读取时，其他事务可以进行读取、更新
        ReadCommitted,
        //未提交读。当事务A更新某条数据的时候，不容许其他事务来更新该数据，但可以进行读取操作
        ReadUncommitted,
    }
}
