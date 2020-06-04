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
        public int ArticleType { get; set; }
        /// <summary>
        /// 全文索引查询
        /// </summary>
        public string FullText { get; set; }
        /// <summary>
        /// 根据账号查询
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 标题模糊查询
        /// </summary>
        public string TitleContain { get; set; }
        /// <summary>
        /// 是否是草稿
        /// </summary>
        public string IsDraft { get; set; }
    }
}
