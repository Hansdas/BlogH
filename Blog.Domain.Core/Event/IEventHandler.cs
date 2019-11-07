using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core.Event
{
   /// <summary>
   /// 标记接口
   /// </summary>
    public interface IEventHandler
    {

    }
    /// <summary>
    /// 领域命令处理接口
    /// </summary>
    /// <typeparam name="TEventData"></typeparam>
    public interface IEventHandler<TEventData>:IEventHandler where TEventData: EventData
    {
        /// <summary>
        /// 处理发布的事件
        /// </summary>
        /// <param name="eventData"></param>
        void Handler(TEventData eventData);
    }
}
