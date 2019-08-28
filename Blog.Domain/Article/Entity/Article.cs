using Blog.Domain.Core;
using Common;
using System;
using System.Collections.Generic;

namespace Blog.Domain
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article : AggregateRoot<int>
    {
        public Article(int id,string title,string textSection, ArticleType articleType)
        {
            Id = id;
            Title = title;
            TextSection = textSection;
            ArticleType = articleType;
        }
        public Article(int id,string title,string content, ArticleType articleType,DateTime createTime)
        {
            Id = id;
            Title = title;
            Content = content;
            ArticleType = articleType;
            CreateTime = createTime;
        }
        public Article(string author, string title,string textSection, string content, ArticleType articleType, bool isDraft,IList<string> relatedFileList)
        {
            Title = title;
            TextSection = textSection;
            ArticleType = articleType;
            Author = author;          
            Content = content;
            IsDraft = isDraft;
            RelatedFileList = relatedFileList;
        }
        public Article(int id, string author, string title,string textSection, string content, ArticleType articleType, bool isDraft, string relatedfiles, int praiseCount, int browserCount, DateTime createTime, DateTime? updateTime)
        {
            Id = id;
            Title = title;
            TextSection = textSection;
            ArticleType = articleType;
            Author = author;
            Content = content;
            IsDraft = isDraft;
            RelatedFileList = relatedfiles.Split(',');
            PraiseCount = praiseCount;
            BrowserCount = browserCount;
            CreateTime = createTime;
            UpdateTime = updateTime;
        }
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
        /// 点赞数
        /// </summary>
        public int PraiseCount { get; private set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int BrowserCount { get; private set; }
        /// <summary>
        /// 相关文件
        /// </summary>
        public IList<string> RelatedFileList { get; private set; }
        /// <summary>
        /// 相关文件,数据库持久化，存放附件路径
        /// </summary>
        public string RelatedFiles {
            get {
                if (RelatedFileList == null || RelatedFileList.Count == 0)
                    return "";
                return RelatedFileList.ConvertTostring(',');
            }
        }
    }
}
