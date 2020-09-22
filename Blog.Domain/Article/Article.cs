using Blog.Domain.Core;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article : Entity<int>
    {
        public Article(int id, string title, ArticleType articleType)
        {
            Id = id;
            Title = title;
            ArticleType = articleType;
        }
        public Article(int id, string title, ArticleType articleType, DateTime createTime)
        {
            Id = id;
            Title = title;
            ArticleType = articleType;
            CreateTime = createTime;
        }
        public Article(int id, string author, string title, string textSection, ArticleType articleType,DateTime createTime)
        {
            Id = id;
            Title = title;
            ArticleType = articleType;
            Author = author;
            TextSection = textSection;
            CreateTime = createTime;
        }
        public Article(int id,string author, string title,string textSection, string content, ArticleType articleType, bool isDraft,bool isSendEmail)
        {
            Id = id;
            Title = title;
            ArticleType = articleType;
            Author = author;          
            Content = content;
            IsDraft = isDraft;
            TextSection = textSection;
            IsSendEmail = isSendEmail;
        }
        public Article(int id,string title, string author, string content, ArticleType articleType, bool isDraft,IList<Comment> comments, DateTime createTime)
        {
            Id = id;
            Title = title;
            Author = author;
            ArticleType = articleType;
            Content = content;
            IsDraft = isDraft;
            CreateTime = createTime;
            Comments = comments;
            if (comments != null)
                CommentIds = string.Join(',', comments.Select(s => s.Guid));

        }
        public Article(int id,string title,string author,string textSection,ArticleType articleType,bool isDraft, int praiseCount, int browserCount, string commentIds,DateTime createTime)
        {
            Id = id;
            Title = title;
            Author = author;
            ArticleType = articleType;
            TextSection = textSection;
            IsDraft = isDraft;
            PraiseCount = praiseCount;
            BrowserCount = browserCount;
            CommentIds = commentIds;
            CreateTime = createTime;
        }
        public Article(int id, string author, string title,string textSection, string content, ArticleType articleType, bool isDraft,int praiseCount, int browserCount, DateTime createTime, DateTime? updateTime)
        {
            Id = id;
            Title = title;
            TextSection = textSection;
            ArticleType = articleType;
            Author = author;
            Content = content;
            IsDraft = isDraft;
            PraiseCount = praiseCount;
            BrowserCount = browserCount;
            CreateTime = createTime;
            UpdateTime = updateTime;
        }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; private set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// 文本截取显示
        /// </summary>
        public string TextSection { get; private set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; private set; }
        /// <summary>
        /// 专栏
        /// </summary>
        public ArticleType ArticleType { get; private set; }
        /// <summary>
        /// 是否为草稿
        /// </summary>
        public bool IsDraft { get; private set; }
        /// <summary>
        /// 有人评论是否发送邮件
        /// </summary>
        public bool IsSendEmail { get; private set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int PraiseCount { get; private set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int BrowserCount { get; private set; }
        /// <summary>
        /// 评论 ids
        /// </summary>
        public string CommentIds { get; private set; }
        /// <summary>
        /// 评论
        /// </summary>
        public IList<Comment> Comments { get; private set; }
    }
}
