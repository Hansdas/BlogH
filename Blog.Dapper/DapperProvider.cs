
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Blog.Dapper
{
  public  class DapperProvider
    {           
        private IDbConnection connection;
        private static string connStr = "";
        private DapperProvider()
        {

        }
        public DapperProvider(string conn)
        {
            if (connection == null)
            {
                connStr = conn;
                connection = CreateConnection();
            }
        }
        public static IDbConnection CreateConnection()
        {
            return new MySqlConnection(connStr);
        }
    }
}
