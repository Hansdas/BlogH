
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Blog.Dapper
{
  public  class DapperProvider
    {           
        public static IDbConnection connection;
        private static string connStr = "";
        private DapperProvider()
        {

        }
        public DapperProvider(string conn)
        {
            connStr = conn;
            connection = CreateConnection();
        }
        private  IDbConnection CreateConnection()
        {
            return new MySqlConnection(connStr);
        }
    }
}
