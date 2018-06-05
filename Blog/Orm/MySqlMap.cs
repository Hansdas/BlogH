using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using Dapper;
using System.Reflection;
using System.Collections;

namespace Orm
{
   public static class MySqlMap
    {
        public static int Insert<T>(this MySqlConnection mySqlConnection,T t)
        {
           int i= mySqlConnection.Execute(t.BuildSqlExpress(),t);
            return i;
        }
        private static string BuildSqlExpress<T>(this T t)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into t_");
            var type = typeof(T);
            string tableName=type.Name;
            sb.Append(tableName);
            PropertyInfo[] propertys=type.GetProperties();
            Hashtable hashtable = new Hashtable();
            PropertyInfo propertyInfo;
            List<string> tableNames = new List<string>();
            List<string> values = new List<string>();
            for (int i=0;i<propertys.Length;i++)
            {
                propertyInfo = propertys[i];
                if (propertyInfo.Name == "Id")
                    continue;
                tableNames.Add(propertyInfo.Name);
                values.Add("@" + propertyInfo.Name);
            }
            sb.Append("(");
            sb.Append(string.Join(",", tableNames));
            sb.Append(") ");
            sb.Append("values (");
            sb.Append(string.Join(",", values));
            sb.Append(")");
            string sql = sb.ToString();
            return sql;
        }
    }
}
