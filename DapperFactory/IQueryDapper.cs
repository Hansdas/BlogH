using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DapperFactory
{
   public interface IQueryDapper
    {
        #region  Select
        /// <summary>
        /// 查询单条信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        T SelectSingle<T>(string sql, object paras = null);
        /// <summary>
        /// 查询单条信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        T SelectSingle<T>(Expression<Func<T, bool>> expression);
        int SelectCount<T>(Expression<Func<T, bool>> expression);
        int SelectCount<T>(string sql, object paras = null);
        #endregion

        #region Insert
        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        T InsertSingle<T>(T t);
        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<T> InsertSingle<T>(string sql, T t);
        #endregion
    }
}
