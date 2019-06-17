using Blog.Domain;
using Chloe;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Blog.Infrastruct
{
    /// <summary>
    /// 仓储接口实现基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class Repository<T, U> : IRepository<T, U>
    {
        private IDbContext dbContext;
        private IQuery<T> query;
        public Repository(IDbContext DbContext, IQuery<T> Query)
        {
            dbContext = DbContext;
            query = Query;
        }
        public Repository()
        {
        }
        private IQuery<T> CreateQuery => dbContext.Query<T>();
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<T> SelectAll(Expression<Func<T, bool>> orderBy = null)
        {
            if (orderBy == null)
                return CreateQuery.AsEnumerable();
            else
                return CreateQuery.OrderBy(orderBy).AsEnumerable();
        }
        /// <summary>
        /// 根据lambda条件查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<T> Select(Expression<Func<T, bool>> condition, Expression<Func<T, bool>> orderBy = null)
        {
            if(orderBy==null)
                return CreateQuery.Where(condition).AsEnumerable();
            else
                return CreateQuery.Where(condition).OrderBy(orderBy).AsEnumerable();
        }
        /// <summary>
        /// 根据lambda条件查询
        /// </summary>
        /// <returns></returns>
        public T SelectSingle(Expression<Func<T, bool>> condition)
        {
            return CreateQuery.Where(condition).FirstOrDefault();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">分页索引页</param>
        /// <param name="pageSize">一页数量</param>
        /// <param name="condition">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<T> SelectByPage(int pageIndex,int pageSize=10, Expression<Func<T, bool>> condition = null, Expression<Func<T, bool>> orderBy = null)
        {
            if (orderBy == null)
                return CreateQuery.Where(condition).TakePage(pageIndex, pageSize).AsEnumerable();
            else
                return CreateQuery.Where(condition).OrderBy(orderBy).TakePage(pageIndex, pageSize).AsEnumerable();
        }

        public void Delete(Expression<Func<T, U>> condition)
        {
            throw new NotImplementedException();
        }
       /// <summary>
       /// 插入数据
       /// </summary>
       /// <param name="t"></param>
       /// <returns></returns>
        public T Insert(T t)
        {
            t = dbContext.Insert<T>(t);
            return t;
        }

        public void Update(T t)
        {
            throw new NotImplementedException();
        }
        public void Delete(Expression<Func<T, bool>> condition)
        {
            throw new NotImplementedException();
        }
    }
}
