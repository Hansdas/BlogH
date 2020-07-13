using Blog.Common;
using Blog.Common.Json;
using Blog.Domain;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastruct
{
   public class NewsRepository: Repository, INewsRepository
    {
        public void InsertOrUpdate(IList<News> newsList)
        {
            string value = JsonHelper.Serialize(newsList);
            string select = "SELECT COUNT(*) FROM SYS_Config where config_key=@key";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@key", "news");
            parameters.Add("@value", value);
            int count= SelectCount(select, parameters);
            string sql = null;
            if (count > 0)
                sql = "INSERT INTO SYS_Config(config_key,config_value,config_createtime) VALUES(@key,@value,now())";
            else
                sql = "UPDATE SYS_Config SET config_value=@value WHERE config_key=@key";
            DbConnection.Execute(sql, parameters);
        }
        public IList<News> Select()
        {
            string sql = "SELECT config_value FROM SYS_Config where config_key=@key";
            string value= DbConnection.QueryFirst<string>(sql,new { key="news"});
            JsonSerializerSettings jsonSerializerSettings = new JsonContractResolver().SetJsonSerializerSettings();
            IList<News> newsList = JsonHelper.DeserializeObject<IList<News>>(value, jsonSerializerSettings);
            return newsList;
        }
    }
}
