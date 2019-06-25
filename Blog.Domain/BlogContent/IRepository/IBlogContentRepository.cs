using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public interface IBlogContentRepository: IRepository<BlogContent, int>
    {
        /// <summary>
        /// 发表微语
        /// </summary>
        void PublishWhisper(BlogContent blogContent);
    }
}
