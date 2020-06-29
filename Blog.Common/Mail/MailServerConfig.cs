using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.Mail
{
    /// <summary>
    /// smtp服务器配置
    /// </summary>
   public class MailServerConfig
    {
        /// <summary>
        /// 服务器（如qq邮箱的为smtp.qq.com）
        /// </summary>
        public string SMTP { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 找好名字
        /// </summary>
        public string EmailName { get; set; }
    }
}
