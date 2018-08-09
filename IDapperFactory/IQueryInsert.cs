using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IDapperFactory
{
  public  interface IQueryInsert
    {
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
        Task<T> InsertSingle<T>(string sql,T t);
    }
}
