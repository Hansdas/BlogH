using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 评论
    /// </summary>
   public class Comment: ValueObject
    {
        public Comment(string commentContent,string guid,string commentAccount,DateTime commentDate)
        {
            Guid = guid;
            CommentContent = commentContent;
            CommentAccount = commentAccount;
            CommentDate = commentDate;
        }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string CommentContent { get; private set; }
        /// <summary>
        /// guid(数据持久化，存入实体表)
        /// </summary>
        public string Guid { get; private set; }
        /// <summary>
        /// 评论人
        /// </summary>
        public string CommentAccount { get; private set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime CommentDate { get; private set; }
    }
}
