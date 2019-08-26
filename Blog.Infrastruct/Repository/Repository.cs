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
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class Repository<T, U> : IRepository<T, U>
    {
        private IDbConnection _dbConnection;
        public IDbConnection dbConnection
        {
            get
            {
                _dbConnection = DapperProvider.connection;
                if (_dbConnection.State != ConnectionState.Open)
                    _dbConnection.Open();
                return _dbConnection;
            }
        }
        private void Dispose()
        {
            if (_dbConnection == null)
                return;
            if (_dbConnection.State != ConnectionState.Closed)
                _dbConnection.Close();
            _dbConnection.Dispose();
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> SelectAll(string sql)
        {
                IEnumerable<dynamic> dynamics = dbConnection.Query(sql);
                Dispose();
                return dynamics;
        }
        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<dynamic> Select(string sql,object param)
        {
            IEnumerable<dynamic> dynamics = dbConnection.Query(sql,param);
            Dispose();
            return dynamics;
        }
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <returns></returns>
        public dynamic SelectSingle(string sql, object param)
        {
            dynamic dynamic = dbConnection.Query(sql, param).FirstOrDefault();
            Dispose();
            return dynamic;
        }  
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <returns></returns>
        public void Insert(string sql, T t)
        {
            dbConnection.Execute(sql,t);
            Dispose();
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <returns></returns>
        public void Update(string sql, T t)
        {
            dbConnection.Execute(sql, t);
            Dispose();
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public void Delete(string sql, object param)
        {
            dbConnection.Execute(sql, param);
            Dispose();
        }

        public T SelectSingle1(string sql, object param)
        {
            T t = dbConnection.Query<T>(sql, param).FirstOrDefault();
            Dispose();
            return t;
        }
    }
}
