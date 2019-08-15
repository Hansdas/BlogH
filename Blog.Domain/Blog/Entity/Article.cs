using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 文章
    /// </summary>
   public class Article:BlogBase<int>
    {
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
    }
}
