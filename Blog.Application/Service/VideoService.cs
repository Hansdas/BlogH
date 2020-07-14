using Blog.Application.IService;
using Blog.Application.ViewModel;
using Blog.Domain;
using Blog.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.Service
{
   public class VideoService:IVideoService
    {
        private IEventBus _eventBus;
        private IVideoRepository _videoRepository;
        private IUserService _userService;
        public VideoService(IEventBus eventBus, IVideoRepository videoRepository, IUserService userService)
        {
            _eventBus = eventBus;
            _videoRepository = videoRepository;
            _userService = userService;
        }

        private VideoCondition ConvertCondition(VideoConditionModel conditionModel)
        {
            VideoCondition videoCondition = new VideoCondition();
            if (conditionModel == null)
                return videoCondition;
            return videoCondition;
        }
        public void Add(VideoModel videoModel)
        {
            Video video = new Video(videoModel.Description,
                videoModel.AuthorAccount,
                videoModel.Url,
                videoModel.Lable,
                videoModel.Size,
                0
                );
            CreateVideoCommand command = new CreateVideoCommand(video);
            _eventBus.Publish(command);
        }
        public IList<VideoModel> ListPage(VideoConditionModel conditionModel)
        {
            IList<Video> videos = _videoRepository.SelectByPage(conditionModel.PageSize, conditionModel.PageIndex,ConvertCondition(conditionModel));
            IList<VideoModel> videoModels = new List<VideoModel>();
            foreach (var item in videos)
            {
                VideoModel videoModel = new VideoModel();
                videoModel.Id = item.Id;
                videoModel.Lables = item.Lable.Split(",");
                videoModel.Description = item.Description;
                videoModel.Url = item.Url;
                videoModel.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd hh:mm");
                videoModels.Add(videoModel);
            }
            return videoModels;
        }

        public int Total(VideoConditionModel conditionModel = null)
        {
            return _videoRepository.SelectCount(ConvertCondition(conditionModel));
        }

        public VideoModel GetById(int id)
        {
            Video video = _videoRepository.SelectById(id);
            UserModel userModel=_userService.SelectUser(video.Author);
            VideoModel videoModel = new VideoModel();
            videoModel.Id = video.Id;
            foreach (var item in video.Lable.Split(","))
            {
                videoModel.Lable += "#" + item + "  ";
            }
            videoModel.Description = video.Description;
            videoModel.CreateTime = video.CreateTime.Value.ToString("yyyy-MM-dd hh:mm");
            videoModel.Author = userModel.Username;
            videoModel.AuthorAccount = video.Author;
            videoModel.Size = video.Size;
            videoModel.WatchCount = video.WatchCount;
            videoModel.Url = video.Url;
            return videoModel;
        }
    }
}
