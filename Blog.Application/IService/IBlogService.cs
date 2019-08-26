using Blog.Application.ViewModel;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application
{
   public interface IArticleService
    {
        void Publish(Article article);

        //IList<BlogModel> GetBlogModels(int pageIndex, int pageSize);
    }
}
