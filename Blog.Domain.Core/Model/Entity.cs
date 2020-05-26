using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core
{
    /// <summary>
    /// 实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
   public  class Entity<T>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public T Id { get; protected set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime? CreateTime { get; protected set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; protected set; }
    }
}
