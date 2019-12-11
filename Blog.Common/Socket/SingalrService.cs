
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
        Task InvokeMessage(Message sendMessage);
    }
    public class SingalrService : Hub<ISingalrClient>
    {
        private ISingalrSvc _singalrSvc;
        public SingalrService(ISingalrSvc singalrSvc)
        {
            _singalrSvc = singalrSvc;
        }
      
        public async Task SendMessageAsync(Message sendMessage)
        {
            await Clients.All.InvokeMessage(sendMessage);
        }

        public void SetConnectionMaps(string account)
        {
            string connectionid = Context.ConnectionId;
            _singalrSvc.SetConnectionMaps(connectionid, account);
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            _singalrSvc.Remove(Context.ConnectionId);           
            return base.OnDisconnectedAsync(exception);
        }
    }
}
