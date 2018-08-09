using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SqlMap
{
   public static class InsertMap
    {
        public static Tuple<string, PropertyInfo[]> GetPropertyInfos<T>(this T t)
        {
            var type = typeof(T);
            string tableName = type.Name;
            PropertyInfo[] propertys = type.GetProperties();
            Tuple<string, PropertyInfo[]> tuple = new Tuple<string, PropertyInfo[]>(tableName,propertys);
            return tuple;
        }
        /// <summary>
        /// 返回insert语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string InsertSqlExpress<T>(this T t)
        {
            Tuple<string, PropertyInfo[]> tuple = GetPropertyInfos<T>(t);
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into t_");
            sb.Append(tuple.Item1);
            PropertyInfo propertyInfo;
            List<string> tableNames = new List<string>();
            List<string> values = new List<string>();
            for (int i = 0; i < tuple.Item2.Length; i++)
            {
                propertyInfo = tuple.Item2[i];
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
