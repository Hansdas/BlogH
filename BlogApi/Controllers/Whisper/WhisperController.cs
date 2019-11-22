using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application;
using Blog.Application.ViewModel;
using Blog.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WhisperController : ControllerBase
    {
        private IWhisperRepository _whisperRepository;
        private IWhisperService _whisperService;
        private IHttpContextAccessor _httpContext;
        public WhisperController(IWhisperRepository whisperRepository,IWhisperService whisperService,IHttpContextAccessor httpContext)
        {
            _whisperRepository = whisperRepository;
            _whisperService = whisperService;
            _httpContext = httpContext;
        }
        [HttpPost]
        public JsonResult Publish()
        {
            ReturnResult returnResult = new ReturnResult();
            UserModel userModel = Auth.GetLoginUser(_httpContext);
            string content = Request.Form["content"];
            Whisper whisper = new Whisper(userModel.Account, content);
            try
            {
                _whisperService.Insert(whisper);
                returnResult.Code = "200";
            }
            catch (Exception e)
            {
                returnResult.Code = "500";
                returnResult.Message = e.Message;
            }
            return new JsonResult(returnResult);
        }
        [HttpGet]
        public JsonResult LoadWhisper(int pageIndex,int pageSize)
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                UserModel userModel = Auth.GetLoginUser(_httpContext);
                WhisperCondiiton condiiton = new WhisperCondiiton();
                condiiton.Account = userModel.Account;
               IList<WhisperModel> whisperModels=_whisperService.SelectByPage(pageIndex, pageSize, condiiton);
                returnResult.Code = "200";
                returnResult.Data = whisperModels;
            }
            catch (Exception e)
            {
                returnResult.Code = "500";
                returnResult.Data = e.Message;
            }
            return new JsonResult(returnResult) ;
        }
        [HttpGet]
        public int LoadTotal()
        {
            int total = _whisperRepository.SelectCount();
            return total;
        }
    }
}