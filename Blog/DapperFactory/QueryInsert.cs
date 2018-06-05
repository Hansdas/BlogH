using IDapperFactory;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using Orm;

namespace DapperFactory
{
    public class QueryInsert : QuerySelect, IQueryInsert
    {
        public int Insert(string sql)
        {
            int i = dbConnection.Execute(sql);
            return i;
        }

        public int Insert<T>(T t)
        {
            int i = dbConnection.Insert(t);
            return i;
        }
    }
}
