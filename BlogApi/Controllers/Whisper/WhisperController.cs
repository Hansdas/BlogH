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
using Org.BouncyCastle.Asn1.Ocsp;
using Ubiety.Dns.Core;

namespace BlogApi.Controllers
{
    [Route("api/whisper")]
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
        [Route("add")]
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
        [Route("page")]
        public JsonResult LoadWhisper(int pageIndex, int pageSize)
        {
            PageResult pageResult = new PageResult();
            try
            {
                IList<WhisperModel> whisperModels=_whisperService.SelectByPage(pageIndex, pageSize);
                pageResult.Code = "0";
                pageResult.Data = whisperModels;
                pageResult.Total = LoadTotal();
            }
            catch (Exception e)
            {
                pageResult.Code = "1";
                pageResult.Data = e.Message;
            }
            return new JsonResult(pageResult) ;
        }
        /// <summary>
        /// 加载广场模块的微语
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("square")]
        public async  Task<JsonResult> LoadSquareWhisper(int pageIndex, int top)
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
        /// <summary>
        /// 查询数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("total")]
        public int LoadTotal()
        {
            int total = _whisperRepository.SelectCount();
            return total;
        }
        /// <summary>
        /// 获取微语评论
        /// </summary>
        /// <param name="whisperId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{whisperId}/comments")]
        public JsonResult LoadComments(int whisperId)
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                returnResult.Data = _whisperService.SelectCommnetsByWhisper(whisperId);
                returnResult.Code = "0";
            }
            catch (Exception e)
            {
                returnResult.Code = "1";
                returnResult.Message = e.Message;
            }
            return new JsonResult(returnResult);
        }
        /// <summary>
        /// 评论微语
        /// </summary>
        /// <param name="whisperId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("comment/add")]
        public JsonResult AddComment()
        {
            string content = Request.Form["content"];
            int whisperId =Convert.ToInt32(Request.Form["whisperId"]);
            string revicer = Request.Form["revicer"];
            string replyId = Request.Form["replyId"];
            int commentType = Convert.ToInt32(Request.Form["commentType"]);
            ReturnResult returnResult = new ReturnResult();
            try
            {
                CommentModel commentModel = new CommentModel();
                commentModel.Content = content;
                commentModel.AdditionalData = replyId;
                commentModel.PostUser = Auth.GetLoginUser(_httpContext).Account;
                commentModel.Revicer = revicer;
                commentModel.CommentType = commentType;
                _whisperService.Review(commentModel, whisperId);
                returnResult.Code = "0";
            }
            catch (AuthException)
            {
                returnResult.Code = "401";
                returnResult.Data ="auth fail";
            }
            catch (Exception e)
            {
                returnResult.Code = "1";
                returnResult.Data = e.Message;
            }

            return new JsonResult(returnResult);
        }
    }
}