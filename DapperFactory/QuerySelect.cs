using IDapperFactory;
using Dapper;
using MySql.Data.MySqlClient;
using DBHelper;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace DapperFactory
{
    public class QuerySelect: OrmBase,IQuerySelect
    {
        /// <summary>
        /// Dapper查询
        /// </summary>
        /// <param name="sql">sql（不要使用全表查询）</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public  T SelectSingle<T>(string sql,object paras=null)
        {
            T t = default(T);
            t = mySqlConnection.QueryFirstOrDefault<T>(sql, paras);
            return t;
        }
        public T SelectSingle<T>(Expression<Func<T, bool>> expression)
        {
            T t=base.SelectEntity<T>(expression);
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
    }
}
