using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using System.Linq;
using Blog.Dapper;

namespace Blog.Infrastruct
{

    /// <summary>
    /// 仓储接口实现基类
    /// </summary>
    public class Repository : IRepository
    {
        public IDbConnection DbConnection => DapperProvider.connection();
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> SelectAll(string sql)
        {
                IEnumerable<dynamic> dynamics = DbConnection.Query(sql);
                return dynamics;
        }
        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public IEnumerable<dynamic> Select(string sql,object param)
        {
            IEnumerable<dynamic> dynamics = DbConnection.Query(sql,param);
            return dynamics;
        }
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <returns></returns>
        public dynamic SelectSingle(string sql, object param)
        {
            dynamic dynamic = DbConnection.QueryFirstOrDefault(sql, param);
            return dynamic;
        }

        public int SelectCount(string sql, object param)
        {
          return  DbConnection.ExecuteScalar<int>(sql, param);
        }
    }
}
