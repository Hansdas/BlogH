using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application.IService;
using Blog.Application.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("api/leavemessage")]
    [ApiController]
    public class LeaveMessageController : ControllerBase
    {
        private ILeaveMessageService _leaveMessageService;
        public LeaveMessageController(ILeaveMessageService leaveMessageService)
        {
            _leaveMessageService = leaveMessageService;
        }
        [HttpPost]
        [Route("add")]
        public ApiResult Add([FromBody] LeaveMessageModel model)
        {
            _leaveMessageService.Add(model);
            return ApiResult.Success();
        }
    }
}