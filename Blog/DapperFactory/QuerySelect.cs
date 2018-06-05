using IDapperFactory;
using MySql.Data.MySqlClient;
using Dapper;
using DBHelper;

namespace DapperFactory
{
    public class QuerySelect: IQuerySelect
    {
        public static MySqlConnection dbConnection = ConnectionProvider.Provider.connection;
        public virtual T SelectSingle<T>(string sql,object paras=null)
        {
            T t = default(T);
            t = dbConnection.QueryFirstOrDefault<T>(sql, paras);
            return t;
        }
        public virtual int SelectCount(string sql,object paras=null)
        {
            int count = dbConnection.QueryFirstOrDefault<int>(sql, paras);
            return count;
        }
    }
}
