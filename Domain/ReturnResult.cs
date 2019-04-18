using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{      /// <summary>
       /// 控制器返回前端结果
       /// </summary>
    public class ReturnResult
    {
        /// <summary>
        /// 提示文本
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 状态结果
        /// </summary>
        public string Code { get; set; }

    }
    /// <summary>
    /// 分页返回数据
    /// </summary>
    public class PageResult:ReturnResult
    {
        /// <summary>
        /// 结果数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 数据结果
        /// </summary>
        public dynamic Data { get; set; }
    }
}
