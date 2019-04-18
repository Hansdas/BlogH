using System;

namespace Domain.Log
{
    public class LoginLog: DomainBase
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 最近登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIp { get; set; }

        /// <summary>
        /// 登录状态
        /// </summary>
        public string LoginStatus { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailureReason { get; set; }

        /// <summary>
        /// 登录总次数
        /// </summary>
        public int LoginNum { get; set; }
    }
}
