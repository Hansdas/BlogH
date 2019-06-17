using System;
using System.Collections.Generic;
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
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        IEnumerable<T> SelectAll(Expression<Func<T, bool>> orderBy = null);

        /// <summary>
        /// 根据lambda条件查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        IEnumerable<T> Select(Expression<Func<T, bool>> condition, Expression<Func<T, bool>> orderBy=null);
        /// <summary>
        /// 根据lambda条件查询
        /// </summary>
        /// <returns></returns>
        T SelectSingle(Expression<Func<T, bool>> condition);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">分页索引页</param>
        /// <param name="pageSize">一页数量</param>
        /// <param name="condition">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        IEnumerable<T> SelectByPage(int pageIndex, int pageSize = 10,Expression<Func<T, bool>> condition=null, Expression<Func<T, bool>> orderBy = null);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        T Insert(T t);
        /// <summary>
        /// 根据lambda条件删除
        /// </summary>
        /// <param name="primaryKey"></param>
        void Delete(Expression<Func<T, bool>> condition);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="t"></param>
        void Update(T t);
    }
}
