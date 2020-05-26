
using Blog.Common.Socket;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common
{
    /// <summary>
    /// 客户端js调用方法
    /// </summary>
    public interface ISingalrClient
    {
        /// <summary>
        /// 发送到指定客户端
        /// </summary>
        /// <param name="sendMessage"></param>
        /// <returns></returns>
        Task InvokeSomeConnectionMessage(Message sendMessage);
        /// <summary>
        /// 发送所有客户端
        /// </summary>
        /// <param name="sendMessage"></param>
        /// <returns></returns>
        Task InvokeMessage(Message sendMessage);
    }
    public class SingalrClient : Hub<ISingalrClient>
    {
        private ISingalrContent _content;
        public SingalrClient(ISingalrContent content)
        {
            _content = content;
        }
        public void SetConnectionMaps(string account)
        {
            string connectionid = Context.ConnectionId;
            _content.SetConnectionMaps(connectionid, account);
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            _content.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
