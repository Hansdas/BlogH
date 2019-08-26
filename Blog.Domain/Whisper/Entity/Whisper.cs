using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 微语实体
    /// </summary>
    public class Whisper : AggregateRoot<int>
    {
        public Whisper(string content, IList<UploadFile> uploadFileList = null)
        {
            Content = content;
            UploadFileList = uploadFileList;
        }
        public Whisper(int id,string content, DateTime createTime, IList<UploadFile> uploadFileList = null, IList<Comment> comments = null)
            : this(content)
        {
            Id = id;
            UploadFileList = uploadFileList;
            CommentList = comments;
            CreateTime = createTime;
        }
        public Whisper(int id, string content, IList<Comment> commentList, int praiseCount, string praiseAccount, DateTime createTime, DateTime? updateTime)
        : this(content)
        {
            Id = id;
            Content = content;
            CommentList = commentList;
            PraiseAccount = praiseAccount;
            PraiseCount = praiseCount;
            CreateTime = CreateTime;
            UpdateTime = updateTime;
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; private set; }
        /// <summary>
        /// 评论
        /// </summary>
        public IList<Comment> CommentList { get; private set; }
        /// <summary>
        /// 评论guid(只用来数据持久化)
        /// </summary>
        public string CommentGuids { get; private set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount
        {
            get { return CommentList.Count; }
        }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int PraiseCount { get; private set; }
        /// <summary>
        /// 点赞账号
        /// </summary>
        public string PraiseAccount { get; private set; }
        /// <summary>
        /// 相关文件guid(只用来数据持久化)
        /// </summary>
        public string UploadFileGuids { get; private set; }
        /// <summary>
        /// 相关文件（业务临时使用）
        /// </summary>
        public IList<UploadFile> UploadFileList { get; private set; }

        /// <summary>
        /// 获取评论guid集合
        /// </summary>
        /// <param name="whisper"></param>
        /// <returns></returns>
        public static IList<string> CommentGuidList(Whisper whisper)
        {
            return whisper.CommentList.Select(s => s.GUID).ToList();
        }
        /// <summary>
        /// 对评论guids赋值
        /// </summary>
        /// <param name="whisper"></param>
        public static void SetCommentGuids(Whisper whisper)
        {
            IList<string> commentGuidList = CommentGuidList(whisper);
            whisper.CommentGuids = string.Join(",", commentGuidList);
        }
        /// <summary>
        /// 获取附件guid集合
        /// </summary>
        /// <param name="whisper"></param>
        /// <returns></returns>
        public static IList<string> UploadFileGuidList(Whisper whisper)
        {
            return whisper.UploadFileList.Select(s => s.GUID).ToList();
        }
        /// <summary>
        /// 对评论guids赋值
        /// </summary>
        /// <param name="whisper"></param>
        public static void SetUploadFileGuids(Whisper whisper)
        {
            IList<string> uploadFileGuidList = UploadFileGuidList(whisper);
            whisper.UploadFileGuids = string.Join(",", uploadFileGuidList);
        }
    }
}
