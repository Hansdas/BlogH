using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperFactory
{
  public  class ConnectionProvider
    {
        private string _MySqlConnection;
        public static MySqlConnection connection;
        /// <summary>
        /// Startup类传入连接字符串
        /// </summary>
        /// <param name="conn"></param>
        public ConnectionProvider(string conn)
        {
            _MySqlConnection = conn;
            connection = GetConnection();
        }
        private ConnectionProvider()
        {

        }
        private MySqlConnection GetConnection()
        {
            //_MySqlConnection = ConfigurationProvider.configuration.GetConnectionString("MySqlConnection");
            using (MySqlConnection conn = new MySqlConnection(_MySqlConnection))
            {
                return conn;
            }
        }
    }
}
