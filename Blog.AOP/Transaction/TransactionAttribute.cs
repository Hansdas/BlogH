using Blog.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Blog.AOP.Transaction
{
    /// <summary>
    /// 标记一个方法使用数据库事务
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
   public class TransactionAttribute:Attribute
    {
        public TransactionAttribute(TransactionLevel level)
        {
            Level = level;
        }
        /// <summary>
        /// 超时时间
        /// </summary>
        public int? TimeoutSecond { get; set; }
        /// <summary>
        /// 事务级别
        /// </summary>
        public TransactionLevel? Level { get; set; }
    }
    /// <summary>
    /// 事务级别
    /// </summary>
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
    ///// <summary>
    ///// 事务范围
    ///// </summary>
    //public enum ScopeOption
    //{
    //    //多个connection共用一个事务
    //    Required,
    //    //每个connection从新创建一个事物
    //    RequiresNew,
    //    //不参与事务
    //    Suppress
    //}
}
