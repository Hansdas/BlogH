using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewModel
{
   /// <summary>
   /// 分页查询条件
   /// </summary>
   public class PageConditionModel
    {
        /// <summary>
        /// 分页起始页，从1开始
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 分页数量
        /// </summary>
        public int PageSize { get; set; }
    }
}
