using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
   public  interface IRepository<T,U>
    {
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        IEnumerable<dynamic> SelectAll(string sql);
        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <returns></returns>
        IEnumerable<dynamic> Select(string sql, object param);
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <returns></returns>
        dynamic SelectSingle(string sql, object param);
    }
}
