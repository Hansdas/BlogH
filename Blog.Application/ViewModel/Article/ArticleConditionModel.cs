using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewModel
{
   public class ArticleConditionModel:PageConditionModel
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string ArticleType { get; set; }
        /// <summary>
        /// 全文索引查询
        /// </summary>
        public string FullText { get; set; }
    }
}
