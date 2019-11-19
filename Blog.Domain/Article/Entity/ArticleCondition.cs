using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
  public  class ArticleCondition:ConditionBase
    {
        /// <summary>
        /// 精确查询
        /// </summary>
        public string ArticleType { get; set; }
        /// <summary>
        /// id查询
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 大于id
        /// </summary>
        public int? ThanId { get; set; }
        /// <summary>
        /// 小于id
        /// </summary>
        public int LessId { get; set; }
        /// <summary>
        /// 根据提交人查询
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 根据是否根据草稿状态查询
        /// </summary>
        public bool? IsDraft { get; set; }
        /// <summary>
        /// 标题模糊查询
        /// </summary>
        public string TitleContain { get; set; }
    }
}
