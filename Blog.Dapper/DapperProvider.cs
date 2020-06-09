
using Blog.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Blog.Dapper
{
  public  class DapperProvider
    {
       
        public  static IDbConnection connection()
        {
                return new MySqlConnection(connStr);
        }
        private static string connStr = null;
        private DapperProvider()
        {

        }
        static DapperProvider()
        {
            connStr = ConfigurationProvider.configuration.GetSection("ConnectionStrings:MySqlConnection").Value;
        }
    }
}
