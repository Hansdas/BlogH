using Blog.Common;
using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewModel
{
    /// <summary>
    /// 文章归档模型
    /// </summary>
   public class ArticleFileModel
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int ArticleType { get; set; }
        /// <summary>
        /// 类型名字
        /// </summary>
        public string ArticleTypeName 
        {
            get
            {
                return ArticleType.GetEnumText<ArticleType>();
            }
        }
    }
}
