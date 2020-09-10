using Blog.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApi
{      /// <summary>
       /// 控制器返回前端结果
       /// </summary>
    public class ApiResult
    {
        public ApiResult()
        {

        }
        public ApiResult(string code)
        {
            Code = code;
        }
        public ApiResult(string code, dynamic data) : this(code)
        {
            Data = data;
        }
        public ApiResult(string code, string message, dynamic data) : this(code, message)
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
        /// <summary>
        /// 返回成功json
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult Success(dynamic data=null)
        {
            return new ApiResult(HttpStatusCode.SUCCESS, data);
        }
        /// <summary>
        /// 返回失败json
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult AuthError(string message="auth fail")
        {
            return new ApiResult(HttpStatusCode.FORBIDDEN, message,null);
        }
        /// <summary>
        /// 返回失败json
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult Error(string code,string message)
        {
            return new ApiResult(code, message,null);
        }
        /// <summary>
        /// 分页json
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult PageResult(dynamic data,int total)
        {
            PageResult pageResult = new PageResult();
            pageResult.Code = HttpStatusCode.SUCCESS;
            pageResult.Data = data;
            pageResult.Total = total;
            return pageResult;
        }
    }
}
