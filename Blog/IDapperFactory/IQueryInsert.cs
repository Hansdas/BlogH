using System;
using System.Collections.Generic;
using System.Text;

namespace IDapperFactory
{
  public  interface IQueryInsert
    {
        int Insert(string sql);
        int Insert<T>(T t);
    }
}
