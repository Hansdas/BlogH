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
   public  interface IRepository
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
        ///查询数量
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int SelectCount(string sql, object param);
        /// <summary>
        /// 查询KeyValue配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string SelectKeyValue(string key);
    }
}
