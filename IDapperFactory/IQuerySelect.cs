using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IDapperFactory
{
   public interface IQuerySelect
    {
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
    }
}
