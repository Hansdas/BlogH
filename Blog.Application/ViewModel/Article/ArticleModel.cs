using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewModel
{
    /// <summary>
    /// 文章模型
    /// </summary>
    public class ArticleModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string TextSection { get; set; }
        /// <summary>
        /// 专栏
        /// </summary>
        public string ArticleType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
       
    }
    public class PageInfoMode
    {
        /// <summary>
        /// 上一篇
        /// </summary>
        public int BeforeId { get; set; }
        /// <summary>
        /// 下一篇
        /// </summary>
        public int NextId { get; set; }
        /// <summary>
        /// 上一篇标题
        /// </summary>
        public string BeforeTitle { get; set; }
        /// <summary>
        /// 下一篇标题
        /// </summary>
        public string NextTitle { get; set; }
    }
}
