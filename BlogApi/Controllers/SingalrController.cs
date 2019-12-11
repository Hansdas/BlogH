using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Common;
using Blog.Common.Socket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SingalrController : ControllerBase
    {
        private IHubContext<SingalrService, ISingalrClient> _hubContext;
        private ISingalrSvc _singalrSvc;
        public SingalrController(IHubContext<SingalrService, ISingalrClient> hubContext, ISingalrSvc singalrSvc)
        {
            _hubContext = hubContext;
            _singalrSvc = singalrSvc;
        }
        /// <summary>
        /// 查询未处理数量
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("{account}")]
        public async Task NewsCount(string account)
        {
            Message sendMessage = new Message();
            sendMessage.Data = "11";
            IReadOnlyList<string> connectionIds = (IReadOnlyList<string>)_singalrSvc.GetConnectionIds(account);
            await _hubContext.Clients.Clients(connectionIds).InvokeMessage(sendMessage);
        }
    }
}