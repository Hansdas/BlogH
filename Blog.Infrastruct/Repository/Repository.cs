using Blog.Domain;
using Chloe;
using Chloe.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using System.Linq;

namespace Blog.Infrastruct
{
    /// <summary>
    /// 仓储接口实现基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class Repository<T, U> : IRepository<T, U>
    {
        private bool useTransaction;
        private IDbTransaction dbTransaction=null;
        protected dynamic CreateConnection(Func<IDbConnection,dynamic> excuteMethod)
        {
            using (IDbConnection dbConnection = ConnectionProvider.CreateConnection())
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
                if (useTransaction)
                {
                    dbTransaction= dbConnection.BeginTransaction();
                }
                try
                {
                    dynamic result =excuteMethod(dbConnection);
                    if(dbTransaction!=null)
                        dbTransaction.Commit();
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    return result;
                }
                catch (Exception)
                {
                    if (dbTransaction != null)
                        dbTransaction.Rollback();
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    throw;
                }
            }
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> SelectAll(string sql)
        {
           return CreateConnection(s => {
                return s.Query(sql);
            });
        }
        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<dynamic> Select(string sql,object param)
        {
            return CreateConnection(s => {
                return s.Query(sql, param);
            });
        }
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <returns></returns>
        public dynamic SelectSingle(string sql, object param)
        {
            return CreateConnection(s =>
            {
                dynamic dynamic = s.QueryFirstOrDefault(sql,param);
                return dynamic;
            });
        }  
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <returns></returns>
        public int Insert(string sql, T t, bool useDBTransaction = false)
        {
            return CreateConnection(s => {
                useTransaction = useDBTransaction;
                return s.ExecuteScalar(sql, t, dbTransaction);
            });
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <returns></returns>
        public void Update(string sql, T t, bool useDBTransaction = false)
        {
            CreateConnection(s => {
                useTransaction = useDBTransaction;
                return s.ExecuteScalar(sql, t, dbTransaction);
            });
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public void Delete(string sql, object param, bool useDBTransaction = false)
        {
            CreateConnection(s => {
                useTransaction = useDBTransaction;
                return s.ExecuteScalar(sql, param, dbTransaction);
            });
        }

    }
}
