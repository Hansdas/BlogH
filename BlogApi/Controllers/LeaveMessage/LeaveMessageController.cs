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
        public JsonResult Add([FromBody] LeaveMessageModel model)
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                _leaveMessageService.Add(model);
                returnResult.Code = "0";
            }
            catch (Exception e)
            {
                returnResult.Code = "1";
                returnResult.Message = e.Message;
            }
            return new JsonResult(returnResult);
        }
    }
}