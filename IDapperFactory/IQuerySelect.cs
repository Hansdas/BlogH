using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IDapperFactory
{
   public interface IQuerySelect
    {
        T SelectSingle<T>(string sql, object paras = null);
        T SelectSingle<T>(Expression<Func<T, bool>> expression);
        int SelectCount<T>(Expression<Func<T, bool>> expression);
        int SelectCount<T>(string sql, object paras = null);
    }
}
