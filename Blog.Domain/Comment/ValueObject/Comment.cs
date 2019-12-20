using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 评论
    /// </summary>
    public class Comment : ValueObject
    {
        public Comment(string guid,string content, string postUser,string replyGuid)
        {
            Guid = guid;
            Content = content;
            PostUser = postUser;
            ReplyGuid = replyGuid;
        }
        public Comment(string guid, string content,  string postUser, string postUsername, DateTime postDate)
        {
            Guid = guid;
            Content = content;
            PostUser = postUser;
            PostUsername = postUsername;
            PostDate = postDate;
        }
        /// <summary>
        /// guid(数据持久化，存入实体表)
        /// </summary>
        public string Guid { get; private set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; private set; }
        /// <summary>
        /// 评论人账号
        /// </summary>
        public string PostUser { get; private set; }
        /// <summary>
        /// 评论人昵称，不存库
        /// </summary>
        public string PostUsername { get; private set; }
        /// <summary>
        /// 被评论的id
        /// </summary>
        public string ReplyGuid { get;private set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime PostDate { get; private set; }
    }
}
