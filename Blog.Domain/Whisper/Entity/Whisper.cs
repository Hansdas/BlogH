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
    public class Whisper : Entity<int>
    {
        public Whisper(int id, string account, string content, string commentGuids, int praiseCount, string praiseAccount,string uploadFileGuids, DateTime createTime)
        {
            Id = id;
            Account = account;
            Content = content;
            CommentGuids = commentGuids;
            PraiseAccount = praiseAccount;
            PraiseCount = praiseCount;
            UploadFileGuids = uploadFileGuids;
            CreateTime = CreateTime;
        }
        public Whisper(int id, string account, string content, IList<Comment> commentList, int praiseCount, string praiseAccount, IList<UploadFile> uploadFileList, DateTime createTime)
        {
            Id = id;
            Account = account;
            Content = content;
            CommentList = commentList;
            PraiseAccount = praiseAccount;
            PraiseCount = praiseCount;
            UploadFileList = uploadFileList;
            CreateTime = CreateTime;
        }
        /// <summary>
        /// 提交人
        /// </summary>
        public string Account { get; private set; }
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
        /// 文件guid(只用来数据持久化)
        /// </summary>
        public string UploadFileGuids { get; private set; }
        /// <summary>
        /// 相关图片
        /// </summary>
        public IList<UploadFile> UploadFileList { get; private set; }

        /// <summary>
        /// 获取评论guid集合
        /// </summary>
        /// <param name="whisper"></param>
        /// <returns></returns>
        public static IList<string> GetCommentGuidList(Whisper whisper)
        {
            return whisper.CommentList.Select(s => s.Guid).ToList();
        }
        /// <summary>
        /// 对评论guids赋值
        /// </summary>
        /// <param name="whisper"></param>
        public static void SetCommentGuids(Whisper whisper)
        {
            IList<string> commentGuidList = GetCommentGuidList(whisper);
            whisper.CommentGuids = string.Join(",", commentGuidList);
        }
        /// <summary>
        /// 获取附件guid集合
        /// </summary>
        /// <param name="whisper"></param>
        /// <returns></returns>
        public static IList<string> GetFileGuids(Whisper whisper)
        {
            return whisper.UploadFileList.Select(s => s.Guid).ToList();
        }
        /// <summary>
        /// 对评论guids赋值
        /// </summary>
        /// <param name="whisper"></param>
        public static void SetFileGuids(Whisper whisper)
        {
            IList<string> uploadFileGuidList = GetFileGuids(whisper);
            whisper.UploadFileGuids = string.Join(",", uploadFileGuidList);
        }
    }
}
