using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewModel
{
    /// <summary>
    /// 博客模型DTO
    /// </summary>
   public class WhisperModel
    {
        public string Id { get; set; }
        /// <summary>
        /// 作者账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 作者名字
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        ///  内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发表日期
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 评论集合
        /// </summary>
        public IList<CommentDataModel> Commentdatas { get; set; }
        public int CommentCount
        {
            get { return Commentdatas==null?0:Commentdatas.Count; }
        }
    }
    /// <summary>
    /// 评论模型DTO
    /// </summary>
    public class CommentDataModel
    {
        /// <summary>
        /// 头像路径
        /// </summary>
      public string UserPhotoPath { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
      public string CommentContent { get; set; }
        /// <summary>
        /// 评论者
        /// </summary>
        public string CommentUser { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public string CommentDate { get; set; }
    }
}
