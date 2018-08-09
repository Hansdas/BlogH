using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBHelper
{
  public  class ConnectionProvider
    {
        private static readonly object _lock = new object();
        private static ConnectionProvider _Provider = null;
        private static string _MySqlConnection;
        public MySqlConnection connection;
        /// <summary>
        /// Startup类传入连接字符串
        /// </summary>
        /// <param name="conn"></param>
        public ConnectionProvider(string conn)
        {
            _MySqlConnection = conn;
        }
        ConnectionProvider()
        {
            connection = GetConnection();
        }
        public static ConnectionProvider Provider
        {
            get
            {
                if (_Provider == null)
                {
                    lock (_lock)
                    {
                        if (_Provider == null)
                            _Provider = new ConnectionProvider();
                    }
                }
                return _Provider;
            }
        }
        private MySqlConnection GetConnection()
        {
            //_MySqlConnection = ConfigurationProvider.configuration.GetConnectionString("MySqlConnection");
            using (MySqlConnection conn = new MySqlConnection(_MySqlConnection))
            {
                conn.Open();
                return conn;
            }
        }
    }
}
