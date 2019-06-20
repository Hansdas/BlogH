using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Core.Bus
{
    /// <summary>
    /// 中介处理接口
    /// </summary>
   public interface IMediatorHandler
    {
        /// <summary>
        /// 发送领域命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        Task SendCommand<T>(T command) where T :Command;
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        Task RaiseEvent<T>(T @event) where T : Event.Event;
    }
}
