using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    public interface ICommentRepository : IRepository
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="comment"></param>
        void Insert(Comment comment);
        /// <summary>
        /// 查询评论
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        IList<Comment> SelectByIds(IList<string> guids);
        /// <summary>
        /// 查询评论
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        Comment SelectById(string guid);
    }
}
