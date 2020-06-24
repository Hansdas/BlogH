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
        public Comment()
        {

        }
        public Comment(string guid,string content, CommentType commentType, string postUser, string revicer,string additioanlDate)
        {
            Guid = guid;
            Content = content;
            PostUser = postUser;
            AdditionalData = additioanlDate;
            RevicerUser = revicer;
            CommentType = commentType;
        }
        public Comment(string guid, string content, CommentType commentType, string postUser, string postUsername, string revicer,string revicerUsername, string additioanlDate,string usingContent, DateTime postDate)
        {
            Guid = guid;
            Content = content;
            PostUser = postUser;
            PostUsername = postUsername;
            PostDate = postDate;
            AdditionalData = additioanlDate;
            RevicerUser = revicer;
            RevicerUsername = revicerUsername;
            CommentType = commentType;
            UsingContent = usingContent;
        }
        /// <summary>
        /// guid(数据持久化，存入实体表)
        /// </summary>
        public string Guid { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public CommentType CommentType { get; private set; }
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
        /// 附加数据
        /// </summary>
        public string AdditionalData { get;private set; }
        /// <summary>
        /// 评论接收人
        /// </summary>
        public string RevicerUser { get; private set; }
        /// <summary>
        ///  评论接收人昵称（不存库）
        /// </summary>
        public string RevicerUsername { get; private set; }
        /// <summary>
        /// 原文内容
        /// </summary>
        public string UsingContent { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime PostDate { get; private set; }
    }
}
