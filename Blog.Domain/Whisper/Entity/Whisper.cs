using Blog.Domain.Core;
using Newtonsoft.Json;
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
        public Whisper() { 
        }
        public Whisper(string account, string content)
        {
            Account = account;
            Content = content;
        }
        public Whisper(int id, string account,string accountName, string content, bool isPassing, string commentGuids, DateTime createTime)
        {
            Id = id;
            Account = account;
            AccountName = accountName;
            Content = content;
            CommentGuids = commentGuids;
            CreateTime = createTime;
            IsPassing = isPassing;
        }
        public Whisper(int id, string account,string accountName, string content, bool isPassing, IList<Comment> commentList, DateTime createTime)
        {
            Id = id;
            Account = account;
            AccountName = accountName;
            Content = content;
            CommentList = commentList;
            CreateTime = createTime;
            IsPassing = isPassing;
        }
        /// <summary>
        /// 提交人
        /// </summary>
        public string Account { get; private set; }
        /// <summary>
        /// 作者昵称
        /// </summary>
        public string AccountName { get; private set; }
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
        /// 该条数据审核是否通过
        /// </summary>
        public bool IsPassing { get; private set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount
        {
            get
            {
                if (string.IsNullOrEmpty(CommentGuids))
                    return 0;
                return CommentList.Count;
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate
        {
            get
            {
                return CreateTime.HasValue ? CreateTime.Value.ToString("yyyy-MM-dd hh:mm"):"";
            }
        }
        /// <summary>
        /// 获取评论guid集合
        /// </summary>
        /// <param name="whisper"></param>
        /// <returns></returns>
        public static IList<string> GetCommentGuidList(Whisper whisper)
        {
            if (whisper.CommentGuids == null)
                return new List<string>();
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
    }
}
