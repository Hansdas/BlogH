using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Socket
{
   public interface ISingalrSvc
    {
        /// <summary>
        /// 获取connectionIds
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IList<string> GetConnectionIds(string value);
        /// <summary>
        ///设置connectionid与user的关系
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="value"></param>
        void SetConnectionMaps(string connectionId,string value);
        /// <summary>
        /// 删除connection关系
        /// </summary>
        /// <param name="key"></param>

        void Remove(string key);
    }
    public class SingalrSvc : ISingalrSvc
    {
        private static ConcurrentDictionary<string, string> connectionMaps = new ConcurrentDictionary<string, string>();
        public static IList<string> list = new List<string>();
        public static Dictionary<string, string> dic = new Dictionary<string, string>();
        public IList<string> GetConnectionIds(string value)
        {
            IList<string> connectionIds = new List<string>();
           foreach(KeyValuePair<string,string> item in connectionMaps)
            {
                if (item.Value == value)
                    connectionIds.Add(item.Key);
            }
            return connectionIds;
        }

        public void Remove(string key)
        {
            if (connectionMaps.ContainsKey(key))
                connectionMaps.TryRemove(key,out string value);
        }

        public void SetConnectionMaps(string connectionId, string value)
        {
            connectionMaps.TryAdd(connectionId, value);
        }
    }
}
