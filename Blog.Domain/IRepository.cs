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
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        int Insert(string sql,T t, bool useDBTransaction = false);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="primaryKey"></param>
        void Delete(string sql, object param,bool useDBTransaction = false);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="t"></param>
        void Update(string sql,T t, bool useDBTransaction = false);
    }
}
