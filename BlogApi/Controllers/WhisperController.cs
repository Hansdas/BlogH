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
            , IHubContext<SingalrClient, ISingalrClient> hubContext)
        {
            _whisperRepository = whisperRepository;
            _whisperService = whisperService;
            _httpContext = httpContext;
        }
        [HttpPost]
        [Route("add")]
        public ApiResult Publish()
        {
            string content = Request.Form["content"];
            try
            {
                UserModel userModel = Auth.GetLoginUser(_httpContext);
                Whisper whisper = new Whisper(userModel.Account, content);
                _whisperService.Insert(whisper);
                return ApiResult.Success();
            }
            catch (AuthException)
            {
                return ApiResult.AuthError();
            }
        }
        [HttpPost]
        [Route("page")]
        public ApiResult LoadWhisper([FromBody] WhisperConditionModel whisperConditionModel)
        {
            if (whisperConditionModel.LoginUser)
                whisperConditionModel.Account = Auth.GetLoginUser(_httpContext).Account;
            IList<WhisperModel> whisperModels = _whisperService.SelectByPage(whisperConditionModel.PageIndex, whisperConditionModel.PageSize, whisperConditionModel);
            int total = _whisperService.SelectCount(whisperConditionModel);
            return ApiResult.Success(new { list = whisperModels, total = total });
        }
        /// <summary>
        /// 加载广场模块的微语
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("square")]
        public async Task<ApiResult> LoadSquareWhisper(int pageIndex, int top)
        {
            try
            {
                IList<WhisperModel> whisperModels = await _whisperService.SelectByPageCache(pageIndex, top);
                return ApiResult.Success(whisperModels);
            }
            catch (AggregateException ex)
            {
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, ex.Message);
            }
        }
        /// <summary>
        /// 查询数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("total")]
        public ApiResult LoadTotal([FromBody] WhisperConditionModel whisperConditionModel)
        {
            WhisperCondiiton whisperCondiiton = new WhisperCondiiton();
            if (whisperConditionModel.LoginUser)
                whisperCondiiton.Account = Auth.GetLoginUser(_httpContext).Account;
            else
                whisperCondiiton.Account = whisperCondiiton.Account;
            int total = _whisperRepository.SelectCount(whisperCondiiton);
            return ApiResult.Success(total);
        }
        /// <summary>
        /// 获取微语评论
        /// </summary>
        /// <param name="whisperId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{whisperId}/comments")]
        public ApiResult LoadComments(int whisperId)
        {

            IList<CommentModel> commentModels = _whisperService.SelectCommnetsByWhisper(whisperId);
            return ApiResult.Success(commentModels);

        }
        /// <summary>
        /// 评论微语
        /// </summary>
        /// <param name="whisperId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("comment/add")]
        public ApiResult AddComment()
        {
            string content = Request.Form["content"];
            int whisperId = Convert.ToInt32(Request.Form["whisperId"]);
            string revicer = Request.Form["revicer"];
            string replyId = Request.Form["replyId"];
            int commentType = Convert.ToInt32(Request.Form["commentType"]);
            try
            {
                CommentModel commentModel = new CommentModel();
                commentModel.Content = content;
                commentModel.AdditionalData = replyId;
                commentModel.PostUser = Auth.GetLoginUser(_httpContext).Account;
                commentModel.Revicer = revicer;
                commentModel.CommentType = commentType;
                _whisperService.Review(commentModel, whisperId);
                return ApiResult.Success();
            }
            catch (AuthException)
            {
                return ApiResult.AuthError();
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public ApiResult Delete(int id)
        {
            _whisperService.DeleteById(id);
            return ApiResult.Success();
        }
    }
}