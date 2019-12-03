using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
  public  interface ICommentRepository: IRepository<Article, int>
    {
        /// <summary>
        /// 查询评论
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        IList<Comment> SelectByIds(IList<string> guids);
    }
}
