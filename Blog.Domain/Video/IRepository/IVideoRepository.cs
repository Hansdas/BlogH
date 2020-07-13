using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public interface IVideoRepository: IRepository
    {
        void Insert(Video video);
        Video SelectById(int id);
        IList<Video> SelectByPage(int pageSize, int pageIndex, VideoCondition condition = null);
        int SelectCount(VideoCondition videoCondition=null);


    }
}
