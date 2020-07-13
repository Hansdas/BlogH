using Blog.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.IService
{
   public interface IVideoService
    {
        void Add(VideoModel videoModel);

        IList<VideoModel> ListPage(VideoConditionModel conditionModel);

        int Total(VideoConditionModel conditionModel = null);

        VideoModel GetById(int id);
    }
}
