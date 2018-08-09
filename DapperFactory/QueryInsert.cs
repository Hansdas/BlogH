using DBHelper;
using IDapperFactory;
using MySql.Data.MySqlClient;
using Dapper;
using System.Threading.Tasks;

namespace DapperFactory
{
    public class QueryInsert:OrmBase,IQueryInsert
    {        
        public  T InsertSingle<T>(T t)
        {
            return base.Insert(t).Result;
        }

        public async Task<T> InsertSingle<T>(string sql,T t)
        {
            T entity = await mySqlConnection.ExecuteScalarAsync<T>(sql, t);
            return entity;
        }

    }
}
