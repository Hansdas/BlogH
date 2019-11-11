using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core.Event
{
    /// <summary>
    /// 事件源，领域事件基类
    /// </summary>
    public class EventData
    {
        /// <summary>
        /// 事件时间
        /// </summary>
        public DateTime EventDate { get; set; }
        /// <summary>
        /// 事件源
        /// </summary>
        public object EventSource { get; set; }
    }
}
