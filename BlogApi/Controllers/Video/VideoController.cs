using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application.IService;
using Blog.Application.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers.Video
{
    [Route("api")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private IVideoService _videoService;
        private IHttpContextAccessor _httpContext;
        public VideoController(IVideoService videoService, IHttpContextAccessor httpContextAccessor)
        {
            _videoService = videoService;
            _httpContext = httpContextAccessor;
        }
        [HttpPost]
        [Route("video/add")]
        public JsonResult Add([FromBody]VideoModel videoModel)
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                UserModel userModel = Auth.GetLoginUser(_httpContext);
                videoModel.AuthorAccount = userModel.Account;
                _videoService.Add(videoModel);
                returnResult.Code = "0";
            }
            catch (Exception ex)
            {
                returnResult.Code = "1";
                returnResult.Message = ex.Message;
            }
            return new JsonResult(returnResult);
        }
        [HttpPost]
        [Route("video/page")]
        public JsonResult ListPage([FromBody]VideoConditionModel conditionModel)
        {
            PageResult pageResult = new PageResult();
            try
            {
                IList<VideoModel> videoModels = _videoService.ListPage(conditionModel);
                int total = _videoService.Total(conditionModel);
                pageResult.Data = videoModels;
                pageResult.Total = total;

            }
            catch (Exception ex)
            {
                pageResult.Code = "1";
                pageResult.Message = ex.Message;
            }
            return new JsonResult(pageResult);
        }

        [HttpGet]
        [Route("video/{id}")]
        public JsonResult GetVideo(int id)
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                returnResult.Data = _videoService.GetById(id);
            }
            catch (Exception ex)
            {
                returnResult.Code = "1";
                returnResult.Message = ex.Message;
            }
            return new JsonResult(returnResult);
        }
    }
}