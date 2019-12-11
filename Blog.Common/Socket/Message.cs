using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.Socket
{
    /// <summary>
    /// 发送消息的实体
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 发送人
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public dynamic Data { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string Revicer { get; set; }
    }
}
