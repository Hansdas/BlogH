﻿using System;
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
        public IList<CommentModel> Commentdatas { get; set; }
        public int CommentCount
        {
            get { return Commentdatas==null?0:Commentdatas.Count; }
        }
    }
}
