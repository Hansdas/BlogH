using System;
using System.Collections.Generic;
using System.Text;

namespace IDapperFactory
{
   public interface IQuerySelect
    {
        T SelectSingle<T>(string sql, object paras = null);
        int SelectCount(string sql, object paras = null);
    }
}
