using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Socket
{
   public interface ISingalrContent
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
        /// <summary>
        /// 向所有客户端（用户）发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendAllClientsMessage(Message message);
        /// <summary>
        /// 向指定的部分客户端（用户）发送消息
        /// </summary>
        /// <param name="connectionIds"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendSomeClientsMessage(IReadOnlyList<string> connectionIds, Message message);
        /// <summary>
        /// 向指定的客户端（用户）发送消息
        /// </summary>
        /// <param name="connectionIds"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendClientMessage(string connection, Message message);
    }

    public class SingalrContent:ISingalrContent
    {
        private static ConcurrentDictionary<string, string> connectionMaps = new ConcurrentDictionary<string, string>();
        private IHubContext<SingalrClient, ISingalrClient> _hubContext;
        public SingalrContent(IHubContext<SingalrClient, ISingalrClient> hubContext)
        {
            _hubContext = hubContext;
        }
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
        #region 向客户端发送消息
        public  async Task SendAllClientsMessage(Message message)
        {
            await _hubContext.Clients.All.SendAllClientsMessage(message);
        }
        public async Task SendSomeClientsMessage(IReadOnlyList<string> connectionIds, Message message)
        {
            if (connectionIds == null || connectionIds.Count == 0)
                throw new ServiceException("指定的客户端连接为空");
            await _hubContext.Clients.Clients(connectionIds).SendSomeClientsMessage(message);
        }

        public async Task SendClientMessage(string connection, Message message)
        {
            if (string.IsNullOrEmpty(connection))
                throw new ArgumentNullException("指定的客户端连接为空");
            IReadOnlyList<string> connectionsByUser = (IReadOnlyList<string>)GetConnectionIds(connection);
            await _hubContext.Clients.Clients(connectionsByUser).SendClientMessage(message);
        }
        #endregion
    }
}
