using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApi
{      /// <summary>
       /// 控制器返回前端结果
       /// </summary>
    public class ReturnResult
    {
        public ReturnResult()
        {

        }
        public ReturnResult(string code)
        {
            Code = code;
        }
        public ReturnResult(string code, string message) : this(code)
        {
            Message = message;
        }
        public ReturnResult(string code, dynamic data) : this(code)
        {
            Data = data;
        }
        public ReturnResult(string code, string message, dynamic data) : this(code, message)
        {
            Data = data;
        }
        /// <summary>
        /// 提示文本
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 响应结果
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public dynamic Data { get; set; }

    }
}
