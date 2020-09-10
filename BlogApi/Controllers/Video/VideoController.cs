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
        public ApiResult Add([FromBody]VideoModel videoModel)
        {
                UserModel userModel = Auth.GetLoginUser(_httpContext);
                videoModel.AuthorAccount = userModel.Account;
                _videoService.Add(videoModel);
            return ApiResult.Success();
        }
        [HttpPost]
        [Route("video/page")]
        public ApiResult ListPage([FromBody]VideoConditionModel conditionModel)
        {

                IList<VideoModel> videoModels = _videoService.ListPage(conditionModel);
                int total = _videoService.Total(conditionModel);
            return ApiResult.Success(new { list = videoModels, total = total });

        }

        [HttpGet]
        [Route("video/{id}")]
        public ApiResult GetVideo(int id)
        {
            return ApiResult.Success(_videoService.GetById(id));
        }
    }
}