using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article : BlogBase<int>
    {
        public Article(string title, string content, ArticleType articleType, bool isDraft,IList<UploadFile> uploadFiles)
        {
            Title = title;
            Content = content;
            ArticleType = articleType;
            IsDraft = isDraft;
            FileList = uploadFiles;
        }
        public Article(int id, string title, string content, ArticleType articleType, bool isDraft, IList<UploadFile> uploadFiles, int praiseCount, int browserCount, DateTime createTime, DateTime? updateTime)
         : this(title, content, articleType, isDraft, uploadFiles)
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
        /// 相关文件（业务临时使用）
        /// </summary>
        public IList<UploadFile> FileList { get; private set; }
        /// <summary>
        /// 获取附件guid集合
        /// </summary>
        /// <param name="whisper"></param>
        /// <returns></returns>
        public static IList<string> FileGuidList(Article article)
        {
            return article.FileList.Select(s => s.GUID).ToList();
        }
        public static void SetUploadFileGuids(Article article)
        {
            IList<string> uploadFileGuidList = FileGuidList(article);
            article.FileGuids = string.Join(",", uploadFileGuidList);
        }
    }
}
