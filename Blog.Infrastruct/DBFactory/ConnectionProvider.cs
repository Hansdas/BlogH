using Chloe.Infrastructure;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Blog.Infrastruct
{
    public class ConnectionProvider : IDbConnectionFactory
    {
        private static IDbConnection connection = null;
        private string connStr;
        private ConnectionProvider()
        {

        }
        public ConnectionProvider(string conn)
        {
            if (connection == null)
            {
                connStr = conn;
                connection = CreateConnection();
            }
        }
        public IDbConnection CreateConnection()
        {
            connection = new MySqlConnection(connStr);
            return connection;
        }
    }
}
