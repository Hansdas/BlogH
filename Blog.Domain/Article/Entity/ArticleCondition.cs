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
    }
}
