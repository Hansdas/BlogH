
using Dapper;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DapperFactory
{
    public class QueryDapper:ExpressionFunc,IQueryDapper
    {
        #region Select
        /// <summary>
        /// 查询单条信息
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public T SelectSingle<T>(string sql, object paras = null)
        {
            T t = default(T);
            t = mySqlConnection.QueryFirstOrDefault<T>(sql, paras);
            return t;
        }
        /// <summary>
        /// 查询单条信息
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public T SelectSingle<T>(Expression<Func<T, bool>> expression)
        {
            T t = base.SelectEntity<T>(expression);
            return t;
        }

        public int SelectCount<T>(string sql, object paras = null)
        {
            int count = mySqlConnection.Query(sql, paras).Count();
            return count;
        }
        public int SelectCount<T>(Expression<Func<T, bool>> expression)
        {
            int count = base.Count<T>(expression);
            return count;
        }
        #endregion



        public T InsertSingle<T>(T t)
        {
            return base.Insert(t).Result;
        }
        public async Task<T> InsertSingle<T>(string sql,T t)
        {
            T entity = await mySqlConnection.ExecuteScalarAsync<T>(sql, t);
            return entity;
        }

    }
}
