using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 新闻
    /// </summary>
    public class News : Entity<int>
    {
        public News(string title, string href, string origin,string originUrl)
        {
            Title = title;
            Href = href;
            Origin = origin;
            OriginUrl = originUrl;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Href { get; private set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Origin { get; private set; }
        /// <summary>
        /// 来源地址
        /// </summary>
        public string OriginUrl { get; private set; }
    }
}
