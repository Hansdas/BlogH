using Blog.Domain.Core;
using CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article : AggregateRoot<int>
    {
        public Article(string title,string textSection, string content, ArticleType articleType, bool isDraft,IList<string> relatedFileList)
        {
            Title = title;
            TextSection = textSection;
            Content = content;
            ArticleType = articleType;
            IsDraft = isDraft;
            RelatedFileList = relatedFileList;
        }
        public Article(int id, string title,string textSection, string content, ArticleType articleType, bool isDraft, IList<string> relatedFileList, int praiseCount, int browserCount, DateTime createTime, DateTime? updateTime)
         : this(title,textSection, content, articleType, isDraft, relatedFileList)
        {
            Id = id;
            PraiseCount = praiseCount;
            BrowserCount = browserCount;
        }
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
        /// 相关文件guid(只用来数据持久化)
        /// </summary>
        public string FileGuids { get; private set; }
        /// <summary>
        /// 相关文件
        /// </summary>
        public IList<string> RelatedFileList {
            get
            {
                if (string.IsNullOrEmpty(RelatedFiles))
                    return new List<string>();
                return RelatedFiles.Split(',');
            }
            private set
            {
                RelatedFiles = value.ConvertTostring(",");
            }
        }
        /// <summary>
        /// 相关文件,数据库持久化，存放附件路径
        /// </summary>
        private string RelatedFiles { get;  set; }
    }
}
