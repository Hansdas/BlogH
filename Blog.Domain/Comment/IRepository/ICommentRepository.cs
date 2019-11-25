using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
  public  interface ICommentRepository: IRepository<Article, int>
    {
        IList<Comment> SelectByIds(IList<string> guids);
    }
}
