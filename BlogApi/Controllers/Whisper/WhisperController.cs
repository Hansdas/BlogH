using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application;
using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Common.Socket;
using Blog.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BlogApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class WhisperController : ControllerBase
    {
        private IWhisperRepository _whisperRepository;
        private IWhisperService _whisperService;
        private IHttpContextAccessor _httpContext;
        public WhisperController(IWhisperRepository whisperRepository, IWhisperService whisperService, IHttpContextAccessor httpContext
            ,IHubContext<SingalrClient, ISingalrClient> hubContext)
        {
            _whisperRepository = whisperRepository;
            _whisperService = whisperService;
            _httpContext = httpContext;
        }
        [HttpPost]
        [Route("whisper/add")]
        public JsonResult Publish()
        {
            ReturnResult returnResult = new ReturnResult();
            string content = Request.Form["content"];
            try
            {
                UserModel userModel = Auth.GetLoginUser(_httpContext);
                Whisper whisper = new Whisper(userModel.Account, content);
                _whisperService.Insert(whisper);
                returnResult.Code = "200";
            }
            catch(AuthException)
            {
                returnResult.Code = "401";
                returnResult.Message ="not login";
            }
            catch (Exception e)
            {
                returnResult.Code = "500";
                returnResult.Message = e.Message;
            }
            return new JsonResult(returnResult);
        }
        [HttpGet]
        public JsonResult LoadWhisper(int pageIndex, int pageSize)
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                UserModel userModel = Auth.GetLoginUser(_httpContext);
                WhisperCondiiton condiiton = new WhisperCondiiton();
                condiiton.Account = userModel.Account;
                IList<WhisperModel> whisperModels=_whisperService.SelectByPage(pageIndex, pageSize, condiiton);
                returnResult.Code = "0";
                returnResult.Data = new { lis= whisperModels ,count=LoadTotal()};
            }
            catch (Exception e)
            {
                returnResult.Code = "1";
                returnResult.Data = e.Message;
            }
            return new JsonResult(returnResult) ;
        }
        /// <summary>
        /// 加载广场模块的微语
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("whisper/square")]
        public async Task<JsonResult> LoadSquareWhisper(int pageIndex, int top)
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                IList<WhisperModel> whisperModels = await _whisperService.SelectByPageCache(pageIndex, top);
                returnResult.Code = "0";
                returnResult.Data = whisperModels;
            }
            catch (Exception e)
            {
                returnResult.Code = "1";
                returnResult.Data = e.Message;
            }
            return new JsonResult(returnResult);
        }
        [HttpGet]
        public int LoadTotal()
        {
            int total = _whisperRepository.SelectCount();
            return total;
        }
    }
}