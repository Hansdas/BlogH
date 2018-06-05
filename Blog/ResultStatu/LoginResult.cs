using System;
using System.Collections.Generic;
using System.Text;

namespace ResultStatu
{
    /// <summary>
    /// 登录返回结果
    /// </summary>
    public class ReturnResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string Message { get; set; }

    }
}
