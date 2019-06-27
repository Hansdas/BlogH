using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewModel
{
    /// <summary>
    /// 博客模型
    /// </summary>
   public class BlogModel
    {
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        ///  内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string PhotoPath { get; set; }
        /// <summary>
        /// 发表日期
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int Praise { get; set; }
        /// <summary>
        /// 回复数
        /// </summary>
        public int Reply { get; set; }
        /// <summary>
        /// 评论集合
        /// </summary>
        public IList<CommentDataModel> Commentdatas { get; set; }
    }
    /// <summary>
    /// 评论模型
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
