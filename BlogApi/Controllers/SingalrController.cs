using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application.IService;
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
        private IHubContext<SingalrClient, ISingalrClient> _hubContext;
        private ISingalrContent _singalrSvc;
        private ITidingsService _tidingsService;
        public SingalrController(IHubContext<SingalrClient, ISingalrClient> hubContext, ISingalrContent singalrSvc
            , ITidingsService tidingsService)
        {
            _hubContext = hubContext;
            _singalrSvc = singalrSvc;
            _tidingsService = tidingsService;
        }
        /// <summary>
        /// 查询未处理数量
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("{account}")]
        public async Task NewsCount(string account)
        {
            IReadOnlyList<string> connectionIds = (IReadOnlyList<string>)_singalrSvc.GetConnectionIds(account);
            int count=  _tidingsService.SelectCountByAccount(account);
            Message sendMessage = new Message();
            sendMessage.Data = count;
            await _hubContext.Clients.Clients(connectionIds).InvokeSomeConnectionMessage(sendMessage);
        }
    }
}